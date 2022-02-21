using System;
using System.Diagnostics;
using CIN_CITY.Features;
using CIN_CITY.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CIN_CITY.Views
{
    // Code behind page which requests Age and Gender information

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutDemoPage1 : EnhancedContentPage
    {
        internal AboutDemoPage1(AboutDemoPage1ViewModel vm)
        {
            InitializeComponent();
            // bind values to the view model
            BindingContext = vm;
        }

        private async void NextClicked(object sender, EventArgs e)
        {
            // Check that they have selected an option

            if (App.vm1.SelectedGender == null || App.vm1.SelectedAge == null)
            {
                await DisplayAlert(
                    (string)Application.Current.Resources["text_about_warning_title"],
                    (string)Application.Current.Resources["text_about_warning_text"],
                    (string)Application.Current.Resources["text_ok"]);
                return;
            }

            if (App.vm2 == null) App.vm2 = new AboutEthPostCodesPage2ViewModel();
            Application.Current.MainPage = new AboutEthPostCodesPage2(App.vm2);
        }

        protected override bool OnBackButtonPressed()
        {
            // Confirm that the User wishes to exit the App
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (await DisplayAlert(
                    (string)Application.Current.Resources["text_exit_title"],
                    (string)Application.Current.Resources["text_exit_thanks"],
                    (string)Application.Current.Resources["text_yes"],
                    (string)Application.Current.Resources["text_no"]))
                {
                    if (Device.RuntimePlatform == Device.Android)
                    {
                        Process.GetCurrentProcess().CloseMainWindow();
                    }
                    if (Device.RuntimePlatform == Device.iOS)
                    {
                        Process.GetCurrentProcess().Kill();
                    }
                }
            });
            return true;
        }
    }
}