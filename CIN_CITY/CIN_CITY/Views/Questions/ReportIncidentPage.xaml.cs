using System;
using System.Diagnostics;
using CIN_CITY.Features;
using CIN_CITY.ViewModels;
using CIN_CITY.Views.Questions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CIN_CITY.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    // Code behind for page which displays 3 menu options to report on a knife incident
    public partial class ReportIncidentPage : ContentPage
    {
        public ReportIncidentPage()
        {
            InitializeComponent();
        }

        // Report Menu Option to report on an incident which is triggered by getting an alert message on the phone
        private async void OnButtonAlertClicked(object sender, EventArgs args)
        {         
            await Navigation.PushAsync(new SpatialQuestionsPage(ReportType.FromAlert));
        }

        // Report Menu Option to report on an incident which is occurring now
        private async void OnButtonNowClicked(object sender, EventArgs args)
        {
            // If worried page is initiated from here then it's not from a notification
            await Navigation.PushAsync(new SpatialQuestionsPage(ReportType.ReportNow));
        }

        // Previous button touched
        protected override bool OnBackButtonPressed()
        {
            // Confirm that the user wishes to exit the App
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