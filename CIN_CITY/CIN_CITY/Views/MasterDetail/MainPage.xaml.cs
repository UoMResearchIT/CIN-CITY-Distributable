using System;
using System.Diagnostics;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CIN_CITY.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    // Displays a menu detail page
    public partial class MainPage : FlyoutPage
    {
        public MainPage()
        {
            InitializeComponent();
            MasterPage.ListView.ItemSelected += ListView_ItemSelected;

            // Sets up the main page as the 'home' with 3 incident report options
            Detail = new NavigationPage(new ReportIncidentPage())
            {
                BarBackgroundColor = (Color)Application.Current.Resources["uom_purple"],
                BarTextColor = Color.White
            };
        }

        // Event when a menu option has been selected
        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterMenuItem;
            if (item == null)
                return;

            // The 'Exit' option on the menu sets target type as null
            if (item.TargetType == null)
            {
                HandleExit();
            }
            else
            {
                // Otherwise NO i.e not Exitting, create the requested page using target type
                var page = (Page)Activator.CreateInstance(item.TargetType);
                page.Title = item.Title;

                // set the detail part of the menu to the requested page
                Detail = new NavigationPage(page)
                {
                    BarBackgroundColor = (Color)Application.Current.Resources["uom_purple"],
                    BarTextColor = Color.White
                };
                // hide the 'slider' menu options page
                IsPresented = false;
                // Nothing is now currently selected on the menu
                MasterPage.ListView.SelectedItem = null;
            }
        }

        private void HandleExit()
        {
            SettingsPage currentPage = ((Application.Current.MainPage as MainPage).Detail as NavigationPage).CurrentPage as SettingsPage;
            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    // If notifications have changed check if they have unsaved changes & want to save them
                    if (currentPage != null && currentPage.HasBeenChanges())
                    {
                        if (await Application.Current.MainPage.DisplayAlert(
                        (string)Application.Current.Resources["text_alerts_title"],
                        (string)Application.Current.Resources["text_savechanges_text"],
                        (string)Application.Current.Resources["text_yes"],
                        (string)Application.Current.Resources["text_no"]))
                        {
                            // Yes - save the changes
                            currentPage.SaveClicked(this, new EventArgs());
                        }
                        else
                        {
                            // No - re-set the values to the original values on the page and lose the changes
                            App.vm4.SelectedNoOfNotifications = currentPage.SavedNoOfNotifications;
                            App.vm4.AlarmSwitch = currentPage.SavedAlarmSwitch;
                        }
                    }
                    // Confirm that the User wishes to exit the App
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
                    // Tidy up
                    IsPresented = false;
                    MasterPage.ListView.SelectedItem = null;
                    return;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            });
        }
    }
}