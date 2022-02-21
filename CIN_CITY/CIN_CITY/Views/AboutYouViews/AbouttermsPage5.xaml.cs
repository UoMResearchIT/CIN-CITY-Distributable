using System;
using System.Diagnostics;
using CIN_CITY.Features;
using CIN_CITY.ViewModels;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CIN_CITY.Views
{
    // Code behind page which displays an option to show Terms/Policy page and confirms User has agreed to the conditions of using the App

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutTermsPage5 : EnhancedContentPage
    {
        internal AboutTermsPage5()
        {
            InitializeComponent();
        }
        // Previous page has been selected
        protected override bool OnBackButtonPressed()
        {
            PreviousClicked(this, new EventArgs());
            return true;
        }
        // Previous page has been selected
        private void PreviousClicked(object sender, EventArgs e)
        {
            // Check that settings have been completed correctly
            if (App.vm1 != null)
            {
                // Ensure Ethnicity and postcodes page exists
                if (App.vm2 == null) App.vm2 = new AboutEthPostCodesPage2ViewModel();
                // Navigate to Ethnicity and postcodes page
                Application.Current.MainPage = new AboutEthPostCodesPage2(App.vm2);
            }
            else
            {
                // Big problem! Should never happen (but did)
                Debug.WriteLine("Big Error: AbouttermsPage5: During setup no VM1 exists, needs investigating");
                App.vm1 = new AboutDemoPage1ViewModel();
                Application.Current.MainPage = new AboutDemoPage1(App.vm1);
            }
        }
        // Next page has been selected
        private async void NextClicked(object sender, EventArgs e)
        {
            // Setup default values for Alerts, now done using defaults rather than User input
            Debug.WriteLine("Terms Page. Create VM4 Setting default nofications");
            if (App.vm4 == null) App.vm4 = new AboutNotiPage4ViewModel();
            
            // Enable notifications
            App.vm4.AlarmSwitch = true;

            // Set number of notifications to 5
            App.vm4.SelectedNoOfNotifications = 5;

            // Initiate native code to set up the notification alerts
            App.vm4.UpdateAlarm();

            // Show a message to user as processing takes a little time
            await Navigation.PushPopupAsync(new BusyPopup((string)Application.Current.Resources["text_saving"]));
            
            // Update the local database with the settings to use for subsequent incident reports
            App.UpdateSettings();
            try
            {
                await Navigation.PopAllPopupAsync();
            }
            catch (Exception)
            { }
            
            // Navigate to Thanks and final settings page
            Application.Current.MainPage = new AboutThanksPage6();
        }
        // Only enables the 'Next' button once the Terms checkbox is clicked
        // The checkbox should only be clicked once the User acknowledges they have read the Policy information       
        private void CheckTerms_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            var CheckTerms = sender as CheckBox;
            if (CheckTerms != null && CheckTerms.IsChecked) NextButton.IsEnabled = true;
            else NextButton.IsEnabled = false;
        }
        // Initiated when the User clicks to request to view the Policy information 
        private void TermsClicked(object sender, EventArgs e)
        {
            // Navigates to the Policy html which displays the User pariticiation information
            Navigation.PushModalAsync(new TermsPage());
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            MyCheckBox.IsChecked = !MyCheckBox.IsChecked;
        }
    }
}