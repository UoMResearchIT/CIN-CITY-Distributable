using System;
using CIN_CITY.Features;
using CIN_CITY.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CIN_CITY.Views
{
    // Code behind page which requests Ethnicity plus first letters of home and school postcodes

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutEthPostCodesPage2 : EnhancedContentPage
    {
        internal AboutEthPostCodesPage2(AboutEthPostCodesPage2ViewModel vm)
        {
            InitializeComponent();
            // bind values to the view model
            BindingContext = vm;
        }

        private async void NextClicked(object sender, EventArgs e)
        {
            // Check that they have completed all 3 options
            if (App.vm2.SelectedEthnicity == null || App.vm2.HomePC == null)
            {
                await DisplayAlert(
                    (string)Application.Current.Resources["text_about_warning_title"],
                    (string)Application.Current.Resources["text_about_warning_text"],
                    (string)Application.Current.Resources["text_ok"]);
                return;
            }
            // Check that the length of the postcodes are valid
            if (App.vm2.HomePC.Length > 4 || App.vm2.HomePC.Length < 2)
            {
                await DisplayAlert(
                    (string)Application.Current.Resources["text_about_postcodelength_title"],
                    (string)Application.Current.Resources["text_about_postcodelength_invalid"],
                    (string)Application.Current.Resources["text_ok"]);
                return;
            }
            // Navigate to the Terms/Policy page
            Application.Current.MainPage = new AboutTermsPage5();
        }

        private void BackClicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new AboutDemoPage1(App.vm1);
        }
    }
}