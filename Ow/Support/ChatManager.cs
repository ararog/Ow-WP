using GalaSoft.MvvmLight.Messaging;
using Microsoft.Devices;
using Microsoft.Phone.Net.NetworkInformation;
using Microsoft.Xna.Framework.Audio;
using Newtonsoft.Json.Linq;
using Ow.Model;
using Ow.Service;
using Ow.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WebSocketRails;

namespace Ow.Support
{
    public class ChatManager
    {
        private Dictionary<string, WebSocketRailsChannel> connections;

        private WebSocketRailsDispatcher dispatcher;

        private Notification notification;

        private User user;

        Timer timer;

        public RestService RestService { set; private get; }

        public AlertService AlertService { set; private get; }

        public ContactService ContactService { set; private get; }

        public HistoryService HistoryService { set; private get; }

        public Session Session { set; private get; }


        public ChatManager()
        {
            connections = new Dictionary<string, WebSocketRailsChannel>();
        }

        public void Initialize()
        {
            user = Session.User;

            if (user == null)
            {
                Messenger.Default.Register<User>(this, Constants.ChatManagerStartChatSession, HandleLogin);
            }
            else
            {
                StartChat();
            }

            Messenger.Default.Register<Notification>(this, Constants.ChatManagerSendNotification, HandleNotification);

            DeviceNetworkInformation.NetworkAvailabilityChanged += new EventHandler<NetworkNotificationEventArgs>(HandleConnectionChanged);
        }

        #region Messenger methods

        private void HandleLogin(User user)
        {
            if (user != null)
            {
    			this.user = user;
    			StartChat();
    		}
        }

        private void HandleNotification(Notification outgoingNotification)
        {
            if (outgoingNotification != null)
            {
    			String phoneNumber = StringUtils.normalizePhoneNumber(
    					outgoingNotification.Contact.CountryCode, 
    					outgoingNotification.Contact.PhoneNumber);
    						
    			WebSocketRailsChannel contactChannel = connections[phoneNumber];
    						
    			contactChannel.Trigger("notification_event", outgoingNotification);
    		}
        }

        #endregion

        #region Connection status methods

        private void HandleConnectionChanged(object sender, NetworkNotificationEventArgs e)
        {
            switch (e.NotificationType)
            {
                case NetworkNotificationType.InterfaceConnected:
                    if(user != null && ! "connected".Equals(dispatcher.State))
                        StartChat();
                    break;

                case NetworkNotificationType.InterfaceDisconnected:
                    StopChat();
                    break;
            }
        }

        #endregion

        #region Chat methods

        private void StartChat() 
        {
            if (dispatcher == null)
            {
                dispatcher = new WebSocketRailsDispatcher(
                    new Uri("ws://oww.herokuapp.com/websocket"));

                dispatcher.Bind("connection_opened", (o, e) =>
                {
                    StartSync();
                });

                dispatcher.Bind("connection_closed", (o, e) =>
                {
                    StopSync();
                });

                dispatcher.Bind("connection_error", (o, e) =>
                {
                    StopSync();
                });

            }

            dispatcher.Connect();
        }

        private void StopChat()
        {
            Messenger.Default.Send<Notification>(notification, Constants.ChatManagerOfflineNotification);

            foreach (string channel in connections.Keys)
                dispatcher.Unsubscribe(channel);

            string phoneNumber = StringUtils.normalizePhoneNumber(
                    user.CountryCode,
                    user.PhoneNumber);

            dispatcher.Unsubscribe(phoneNumber);

            dispatcher.Disconnect();

            dispatcher = null;

            connections.Clear();
        }

        private void CreateRooms(List<Contact> contacts)
        {
    		WebSocketRailsChannel channel;
    
    		String phoneNumber;
    		
    		foreach(Contact contact in contacts) {
    		
    			phoneNumber = StringUtils.normalizePhoneNumber(
    					contact.CountryCode, 
    					contact.PhoneNumber);
    		
    			if(connections.ContainsKey(phoneNumber)) {	
    
    				channel = dispatcher.Subscribe(phoneNumber);
    				connections[phoneNumber] = channel;
    			}
    		}		
    		
    		phoneNumber = StringUtils.normalizePhoneNumber(
    				user.CountryCode, 
    				user.PhoneNumber);

            if (dispatcher.IsSubscribed(phoneNumber))
            {
                channel = dispatcher.Subscribe(phoneNumber);

                channel.Bind("notification_event", MessageReceived);

                Messenger.Default.Send<Notification>(notification, Constants.ChatManagerOnlineNotification);

                dispatcher.Trigger("connected_event", user);
            }
        }

        private void MessageReceived(object sender, WebSocketRailsDataEventArgs e)
        {
            if (e.Data is JObject)
            {
                notification = (e.Data as JObject).ToObject<Notification>();

                notification.Date = DateTime.Now;

                notification.Read = false;

                HistoryService.SaveNotification(notification);

                AlertService.ExecuteAlertForNotification(notification, ! Session.Mute);

                Messenger.Default.Send<Notification>(notification, Constants.ChatManagerNewNotification);
            }
        }

        #endregion

        #region Contacts sync methods

        private void StartSync()
        {
            timer = new Timer(TimerCallback, null, 0, 300000);
        }

        private void StopSync()
        {
            timer.Dispose();
        }

        private async void TimerCallback(object state)
        {
            List<Contact> contactsOnDevice = await ContactService.FindDeviceContacts();
            if(contactsOnDevice != null)
            { 
                List<Contact> contacts = await RestService.SyncContacts(
                    user, contactsOnDevice);

                if (contacts != null) 
                { 
                    Messenger.Default.Send<List<Contact>>(contacts, Constants.ChatManagerUpdateContactsList);

                    ContactService.SaveAll(contacts);

                    CreateRooms(contacts);
                }
            }
        }

        #endregion
    }
}
