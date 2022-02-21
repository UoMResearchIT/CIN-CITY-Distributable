using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using CIN_CITY.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CIN_CITY.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    // Displays a master menu page and defines the view model for the menu
    public partial class MasterMenu : ContentPage
    {
        public ListView ListView;

        public MasterMenu()
        {
            InitializeComponent();
            // bind to the view model
            BindingContext = new MasterMenuMasterViewModel();

            ListView = MenuItemsListView;
            Title = (string)Application.Current.Resources["text_menu"];
            MenuImage.Source = ImageSource.FromFile((OnPlatform<string>)Application.Current.Resources["ic_foca_icon"]);
        }

        // Navigate to previous page
        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                // Confirm that the user wishes to exit the App
                if (await DisplayAlert(
                (string)Application.Current.Resources["text_exit_title"],
                  (string)Application.Current.Resources["text_exit_thanks"],
                  (string)Application.Current.Resources["text_yes"],
                  (string)Application.Current.Resources["text_no"]))
                {
                    if (Device.RuntimePlatform == Device.Android)
                    {
                        Process.GetCurrentProcess().CloseMainWindow();
                        //or maybe Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
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