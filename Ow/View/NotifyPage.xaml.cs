using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using GalaSoft.MvvmLight.Messaging;
using Ow.Model;
using Ow.Support;

namespace Ow.View
{
    public partial class NotifyPage : PhoneApplicationPage
    {
        private Contact contact;

        public NotifyPage()
        {
            InitializeComponent();
        }

        #region Page events

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            contact = (Contact)PhoneApplicationService
                    .Current.State[Constants.Contact];

            List<NotificationType> notificationTypes = new List<NotificationType>();

            notificationTypes.Add(new NotificationType { Id = 1, Name = "vamo", Text = "Vamo?" });
            notificationTypes.Add(new NotificationType { Id = 2, Name = "perai", Text = "Peraí" });
            notificationTypes.Add(new NotificationType { Id = 3, Name = "chegou", Text = "Chegou" });
            notificationTypes.Add(new NotificationType { Id = 4, Name = "maneiro", Text = "Maneiro" });
            notificationTypes.Add(new NotificationType { Id = 5, Name = "tocomfome", Text = "Tô com fome" });
            notificationTypes.Add(new NotificationType { Id = 6, Name = "owkey", Text = "Ok!" });

            NotificationTypesList.ItemsSource = notificationTypes;

            PhoneApplicationService.Current.State.Remove(Constants.Contact);
        }

        #endregion

        #region List events

        private void NotificationTypesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Notification notification = new Notification();

            notification.Contact = contact;

            Messenger.Default.Send<Notification>(notification);

            NavigationService.GoBack();
        }

        #endregion

        #region Image events

        private void BackImage_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.GoBack();
        }

        #endregion
    }
}