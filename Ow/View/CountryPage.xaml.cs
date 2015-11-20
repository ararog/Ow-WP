using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Ow.ViewModel;
using Ow.Service;
using Microsoft.Practices.ServiceLocation;
using Ow.Model;
using System.Windows.Media;
using Ow.Resources;
using Ow.Support;

namespace Ow.View
{
    public partial class CountryPage : PhoneApplicationPage
    {
        private CountryService countryService;

        private CountryPageModel model;

        public CountryPage()
        {
            InitializeComponent();

            countryService = ServiceLocator.Current.GetInstance<CountryService>();

            model = ServiceLocator.Current.GetInstance<CountryPageModel>();
        }

        #region Page events

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            List<Country> countriesFound = countryService.FindAllCountries();

            CountriesList.ItemsSource = countriesFound;

            CountrySearchField.Focus();
        }

        #endregion

        #region Buttons events

        private void CancelSearchButton_Click(object sender, RoutedEventArgs e)
        {
            List<Country> countriesFound = countryService.FindAllCountries();

            CountriesList.ItemsSource = countriesFound;

            CountrySearchField.Text = "";
        }

        #endregion

        #region Fields events

        private void CountrySearchField_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<Country> countriesFound = countryService.FindByName(CountrySearchField.Text);

            CountriesList.ItemsSource = countriesFound;
        }

        #endregion

        #region List events

        private void CountriesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Country country = (Country) CountriesList.SelectedItem;

            PhoneApplicationService.Current.State[Constants.Country] = country;

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