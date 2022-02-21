using System;
using CIN_CITY.Features;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CIN_CITY.Views
{
    // Code behind page which thanks the User for taking part in the study and using the App    

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutThanksPage6 : EnhancedContentPage
    {
        internal AboutThanksPage6()
        {
            InitializeComponent();

            PageIcon.Source = ImageSource.FromFile((OnPlatform<string>)Application.Current.Resources["icon_thumbup"]);
        }
        // Selected to go to previous page
        protected override bool OnBackButtonPressed()
        {
            // Go back to Terms/Policy page
            Application.Current.MainPage = new AboutTermsPage5();
            return true;
        }
        // Completed the settings information gathering 
        private void FinishClicked(object sender, EventArgs e)
        {
            // Load the main page for the App which is the menu of incident reporting options
            Application.Current.MainPage = new MainPage();
        }
    }
}