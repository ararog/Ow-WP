using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Ow.Resources;
using WebSocketRails;
using Ow.Support;
using Newtonsoft.Json.Linq;
using Ow.Model;
using Microsoft.Xna.Framework.Audio;
using System.IO;
using Ow.Service;
using Autofac;
using Microsoft.Practices.ServiceLocation;
using GalaSoft.MvvmLight.Messaging;
using Ow.ViewModel;

namespace Ow
{
    public partial class MainPage : PhoneApplicationPage
    {

        private ContactService contactsService;

        private HistoryService historyService;

        private MainPageModel model;

        public MainPage()
        {
            InitializeComponent();

            contactsService = ServiceLocator.Current.GetInstance<ContactService>();

            historyService = ServiceLocator.Current.GetInstance<HistoryService>();

            model = ServiceLocator.Current.GetInstance<MainPageModel>();
        }

        #region Page events
        
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            ListContacts.ItemsSource = contactsService.FindAllContacts();

            ListNotifications.ItemsSource = historyService.FindAllNotifications();

            Pivot.SelectedIndex = 1;

            Messenger.Default.Register<List<Contact>>(this, 
                Constants.ChatManagerUpdateContactsList, HandleContactsUpdate);

            Messenger.Default.Register<Notification>(this,
                Constants.ChatManagerNewNotification, HandleNewNotification);

            Messenger.Default.Register<object>(this,
                Constants.ChatManagerOnlineNotification, HandleOnlineNotification);

            Messenger.Default.Register<object>(this,
                Constants.ChatManagerOfflineNotification, HandleOfflineNotification);
        }

        #endregion

        #region Pivot events

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        #endregion

        #region ChatManager events 

        private void HandleContactsUpdate(List<Contact> contacts)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                ListContacts.ItemsSource = contacts;
            });
        }

        private void HandleNewNotification(Notification notification)
        {
            List<Notification> notifications = (List<Notification>) ListNotifications.ItemsSource;
            if(notifications != null)
                notifications.Insert(0, notification);

            ListNotifications.ItemsSource = notifications;

            int count = historyService.CountUnreadNotifications();
            if (count == 0)
            {
                if (ToolBar.ColumnDefinitions.Count == 3)
                    ToolBar.ColumnDefinitions[3].Width = new GridLength(64);

                CountLabel.Visibility = System.Windows.Visibility.Collapsed;
                CountLabel.Text = "";
            }
            else
            {
                if(ToolBar.ColumnDefinitions.Count == 3)
                    ToolBar.ColumnDefinitions[3].Width = new GridLength(94);
                
                CountLabel.Visibility = System.Windows.Visibility.Visible;
                CountLabel.Text = Convert.ToString(count);
            }
        }

        private void HandleOnlineNotification(object data)
        {
            WaitTitleView.Visibility = Visibility.Collapsed;

            TitleLabel.Visibility = Visibility.Collapsed;

            ListContacts.IsEnabled = true;
        }

        private void HandleOfflineNotification(object data)
        {
            WaitTitleView.Visibility = Visibility.Collapsed;

            TitleLabel.Visibility = Visibility.Visible;

            ListContacts.IsEnabled = false;
        }

        #endregion

        #region List events

        private void ListContacts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Contact contact = (Contact) ListContacts.SelectedItem;

            PhoneApplicationService.Current.State[Constants.Contact] = contact;

            NavigationService.Navigate(new Uri("/View/NotifyPage.xaml", 
                UriKind.RelativeOrAbsolute));
        }

        private void ListNotifications_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Notification notification = (Notification)ListNotifications.SelectedItem;
        }

        #endregion

        #region Button events

        private void SoundButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {

        }

        private void NotificationsButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Pivot.SelectedIndex = 2;
        }

        private void SettingsButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Pivot.SelectedIndex = 0;
        }

        private void AddButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {

        }

        private void NotificationsToContactsButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Pivot.SelectedIndex = 1;
        }

        private void SettingsToContactsButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Pivot.SelectedIndex = 1;
        }

        #endregion
    }
}