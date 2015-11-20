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
using Ow.Model;
using Ow.Support;
using System.Windows.Media;
using Ow.ViewModel;
using System.Globalization;
using Ow.Resources;
using GalaSoft.MvvmLight.Messaging;

namespace Ow
{
    public partial class LoginPage : PhoneApplicationPage
    {
        private RestService restService;

        private CountryService countryService;

        private LoginPageModel model;

        private bool navigatingBack;

        public LoginPage()
        {
            InitializeComponent();

            restService = ServiceLocator.Current.GetInstance<RestService>();

            countryService = ServiceLocator.Current.GetInstance<CountryService>();

            model = ServiceLocator.Current.GetInstance<LoginPageModel>();

            navigatingBack = false;
        }

        #region Page events

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!navigatingBack)
            {
                string countryCode = RegionInfo.CurrentRegion.TwoLetterISORegionName;

                Country country = countryService.FindByCode(countryCode);

                if (country != null)
                {
                    CountryImage.Source = country.Image;
                    CountryCodeField.Text = country.DialCode;
                    CountryNameField.Text = country.Name;
                }
            }
            else
            {
                Country country = (Country)PhoneApplicationService
                    .Current.State[Constants.Country];

                CountryImage.Source = country.Image;
                CountryCodeField.Text = country.DialCode;
                CountryNameField.Text = country.Name;

                PhoneApplicationService.Current.State.Remove(Constants.Country);
            }

            PhoneNumberField.Focus();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigatingBack = (e.NavigationMode == NavigationMode.Back);
        }

        #endregion

        #region Buttons events

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            WaitIndicator.IsVisible = true;

            Login login = new Login();
            login.CountryCode = CountryCodeField.Text;
            login.PhoneNumber = PhoneNumberField.Text;

            User user = await restService.Signin(login);

            WaitIndicator.IsVisible = false;

            if (user != null)
            {
                Messenger.Default.Send(user, Constants.ChatManagerStartChatSession);

                model.Session.User = user;
                model.Session.SaveData();

                NavigationService.Navigate(new Uri("/View/MainPage.xaml", UriKind.RelativeOrAbsolute));
            }
            else
            {
                Registration registration = new Registration();
                registration.CountryCode = CountryCodeField.Text;
                registration.PhoneNumber = PhoneNumberField.Text;

                PhoneApplicationService.Current.State[Constants.Registration] = registration;

                NavigationService.Navigate(new Uri("/View/RegistrationPage.xaml", UriKind.RelativeOrAbsolute));
            }
        }

        #endregion

        #region Grid events

        private void Grid_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/CountryPage.xaml", UriKind.RelativeOrAbsolute));
        }

        #endregion
    }
}