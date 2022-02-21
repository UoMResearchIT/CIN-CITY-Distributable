using System;
using System.Diagnostics;
using CIN_CITY.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CIN_CITY.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    // Code behind for Settings page which displays alert preferences
    public partial class SettingsPage : ContentPage
    {
        // Retains original values for number of notifications and if alerts are enabled
        // if the user leaves the page without saving, a prompt confirms whether they do wish to lose the changes or save them
       
        private bool savedAlarmSwitch;
        public bool SavedAlarmSwitch
        {
            get
            {
                return savedAlarmSwitch;
            }
            set
            {
                savedAlarmSwitch = value;
            }
        }

        private int savedNoOfNotifications;
        public int SavedNoOfNotifications
        {
            get
            {
                return savedNoOfNotifications;
            }
            set
            {
                savedNoOfNotifications = value;
            }
        }
        public SettingsPage()
        {
            InitializeComponent();
            // Check if a view model exists, if not create one
            if (App.vm4 == null) App.vm4 = new AboutNotiPage4ViewModel();

            // bind to the view model
            BindingContext = App.vm4;

            // save the original values for later checking
            SavedNoOfNotifications = App.vm4.SelectedNoOfNotifications;
            SavedAlarmSwitch = App.vm4.AlarmSwitch;
        }
        // 'Save' button touched
        public async void SaveClicked(object sender, EventArgs e)
        {
            // Check that they have selected a number for notifications if notifications are enabled 
            if (App.vm4.AlarmSwitch && App.vm4.SelectedNoOfNotifications < 1)
            {
                await DisplayAlert(
                    (string)Application.Current.Resources["text_about_warning_title"],
                    (string)Application.Current.Resources["text_error_notifications"],
                    (string)Application.Current.Resources["text_ok"]);
                return;
            }

            Debug.WriteLine("VM4 Updating the Alarm....");

            // Update the Alert notifications
            App.vm4.UpdateAlarm();

            // Update local settings DB with alert preferences
            App.UpdateSettings();

            // Stash new settings to ensure they always 'save' if updated when leaving the page
            SavedNoOfNotifications = App.vm4.SelectedNoOfNotifications;
            SavedAlarmSwitch = App.vm4.AlarmSwitch;

            // Display 'Home' page
            try
            {
                var master = Application.Current.MainPage as FlyoutPage;
                if (master == null)
                {
                    master = new MainPage();
                    Application.Current.MainPage = master;
                }
                master.Detail = new NavigationPage(new ReportIncidentPage());
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception caught " + ex.Message + " " + ex.StackTrace + " " + ex.InnerException);
            }
        }

        public bool HasBeenChanges()
        {
            return (SavedNoOfNotifications != App.vm4.SelectedNoOfNotifications ||
                SavedAlarmSwitch != App.vm4.AlarmSwitch);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            // Check they have remembered to press save if they have changed notifications
            // If not show a confirmation message to the user
            if (HasBeenChanges())
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (await Application.Current.MainPage.DisplayAlert(
                        (string)Application.Current.Resources["text_alerts_title"],
                        (string)Application.Current.Resources["text_savechanges_text"],
                        (string)Application.Current.Resources["text_yes"],
                        (string)Application.Current.Resources["text_no"]))
                    {
                        // Yes - save the changes
                        SaveClicked(this, new EventArgs());
                    }
                    else
                    {
                        // No - re-set the values to the original values and lose the changes
                        App.vm4.SelectedNoOfNotifications = SavedNoOfNotifications;
                        App.vm4.AlarmSwitch = SavedAlarmSwitch;
                    }
                });
                return;
            }
        }

        protected override bool OnBackButtonPressed()
        {
            // Check they want to save their changes (if they have changed notifications) before leaving the page
            if (SavedNoOfNotifications != App.vm4.SelectedNoOfNotifications || SavedAlarmSwitch != App.vm4.AlarmSwitch)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (await Application.Current.MainPage.DisplayAlert(
                        (string)Application.Current.Resources["text_alerts_title"],
                        (string)Application.Current.Resources["text_savechanges_text"],
                        (string)Application.Current.Resources["text_yes"],
                        (string)Application.Current.Resources["text_no"]))
                    {
                        // Yes - save the changes
                        SaveClicked(this, new EventArgs());
                    }
                    else
                    {
                        // No - re-set the values to the original values and lose the changes
                        App.vm4.SelectedNoOfNotifications = SavedNoOfNotifications;
                        App.vm4.AlarmSwitch = SavedAlarmSwitch;
                    }
                });
            }
            // Show the 'slider' menu page 
            try
            {
                (Application.Current.MainPage as FlyoutPage).IsPresented = true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception caught " + e.Message + " " + e.StackTrace + " " + e.InnerException);
            }
            return true;
        }
    }
}