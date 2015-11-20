using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Ow.Service;
using Autofac;
using Microsoft.Practices.ServiceLocation;
using Ow.Support;
using Ow.ViewModel;
using Ow.Model;
using GalaSoft.MvvmLight.Messaging;

namespace Ow
{
    public partial class RegistrationPage : PhoneApplicationPage
    {
        private RegistrationPageModel model;

        private RestService service;

        public RegistrationPage()
        {
            InitializeComponent();

            service = ServiceLocator.Current.GetInstance<RestService>();

            model = ServiceLocator.Current.GetInstance<RegistrationPageModel>();
        }

        #region Page events

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            model.Registration = (Registration) PhoneApplicationService
                .Current.State[Constants.Registration];

            CodeField.Focus();
        }

        #endregion

        #region Buttons events

        private async void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            WaitIndicator.IsVisible = true;

            User user = await service.Verify(model.Registration);

            WaitIndicator.IsVisible = false;

            if (user != null)
            {
                Messenger.Default.Send(user, Constants.ChatManagerStartChatSession);

                model.Session.User = user;
                model.Session.SaveData();

                NavigationService.Navigate(new Uri("/View/MainPage.xaml", UriKind.RelativeOrAbsolute));
            }
        }

        #endregion
    }
}