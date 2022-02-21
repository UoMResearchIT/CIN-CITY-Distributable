using System;
using System.Diagnostics;
using System.Threading.Tasks;
using CIN_CITY.Features;
using CIN_CITY.ViewModels;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CIN_CITY.Services;

namespace CIN_CITY.Views
{
    [XamlCompilation(XamlCompilationOptions.Skip)]
    public partial class LoginPage : EnhancedContentPage
    {
        public LoginPage()
        {
            try
            {
                InitializeComponent();
                InitialiseEnhancedPage(MasterLayout);

                // Add icons
                HeaderImage.Source = ImageSource.FromFile((OnPlatform<string>)Application.Current.Resources["ic_foca_icon_big"]);

            }
            catch (Exception e)
            {
                Debug.WriteLine("LoginPage constructor: Exception caught : " + e.Message);
            }
        }

        // Launch the login actions when Login button has been touched
        private async void OnLoginButtonClicked(object sender, EventArgs args)
        {
            // Username and password must be provided
            if (UserEntry.Text.Equals("") || PassEntry.Text.Equals(""))
            {
                // Display error to user
                await DisplayAlert(
                    (string)Application.Current.Resources["text_confirmpw_fail_title"],
                    (string)Application.Current.Resources["text_register_nodetails"],
                    (string)Application.Current.Resources["text_ok"]);
            }
            else
            {
                // Check Location handling is enabled, can't continue App without this
                if (await (Application.Current as App)?.GetPermissionAsync())
                {
                    try
                    {
                        await Navigation.PushPopupAsync(new BusyPopup());
                    }
                    catch (Exception)
                    {
                    }

                    // Check if internet connectivity
                    // If no internet can't login/verify but allow to continue if previous login has happened
                    if (!DataService.Instance.IsConnected())
                    {
                        await DisplayAlert(
                           (string)Application.Current.Resources["text_connection_title"],
                           (string)Application.Current.Resources["text_connection_text"],
                           (string)Application.Current.Resources["text_ok"]);

                        // If user has stored credentials
                        if (DataService.Instance.IsAuthenticated())
                        {
                            LaunchAboutYouPages(); // Need settings populating each time going via 'Login' options
                        }
                    }
                    else
                    {
                        // Got internet access OK, so need to check login credentials
                        var success = await LoginAsync(UserEntry.Text, PassEntry.Text);
                        try
                        {
                            await Navigation.PopAllPopupAsync();
                        }
                        catch (Exception)
                        { }

                        // Login successful continue into App and get settings information
                        if (success)
                        {
                            LaunchAboutYouPages();
                        }
                    }
                }
            }

            // Ensure no 'talking to server' or 'busy' popups remain hanging about
            try
            {
                await Navigation.PopAllPopupAsync();
            }
            catch (Exception)
            { }
        }

        // Launch registration/login actions
        // Gather information from the user and populate settings local database
        private void LaunchAboutYouPages()
        {
            // Always clear the setting information by initiating the VMs and start from scratch    
            App.vm1 = new AboutDemoPage1ViewModel();
            App.vm2 = new AboutEthPostCodesPage2ViewModel();
            App.vm4 = new AboutNotiPage4ViewModel();

            // Display 1st Demographics page
            Application.Current.MainPage = new AboutDemoPage1(App.vm1);
        }

        // Login method
        // Called when internet connection is available
        private async Task<bool> LoginAsync(string username, string password)
        {
            // Wait for the response from the server
            bool status = false;

            // Attempt login with storage connect
            var response = await DataService.Instance.LoginAsync(username, password);

            if (response)
            {
                // Report sucessful login as a toast
                Device.BeginInvokeOnMainThread(() => DependencyService.Get<IToast>().ToastShort((string)Application.Current.Resources["text_login_success"]));
                Debug.WriteLine("LOGIN SUCCESSFUL!");
                status = true;
            }
            return status;
        }

        // Register method
        private async void OnRegisterButtonClicked(object sender, EventArgs args)
        {
            // Must provide user name and password to register
            if (UserEntry.Text.Equals("") || PassEntry.Text.Equals(""))
            {
                // Display error to user
                await DisplayAlert(
                    (string)Application.Current.Resources["text_confirmpw_fail_title"],
                    (string)Application.Current.Resources["text_register_nodetails"],
                    (string)Application.Current.Resources["text_ok"]);
                return;
            }

            // Check location permissions are enabled
            if (await (Application.Current as App)?.GetPermissionAsync())
            {
                // Confirm password has been entered correctly 
                await Navigation.PushPopupAsync(new ConfirmPassWordPopUp(CheckPassword));
            }
        }

        // Check that the password entered on the main register/login page matches the confirmation popup entry
        private async void CheckPassword(string password)
        {
            if (password == null || (!password.Equals(PassEntry.Text)))
            {
                // Display error to user
                await DisplayAlert(
                    (string)Application.Current.Resources["text_confirmpw_fail_title"],
                    (string)Application.Current.Resources["text_confirmpw_fail"],
                    (string)Application.Current.Resources["text_ok"]);
            }
            else
            {
                await Navigation.PushPopupAsync(new BusyPopup());
                var success = await RegisterAsync(UserEntry.Text, PassEntry.Text);
                try { await Navigation.PopAllPopupAsync(); } catch { }

                // Show the settings pages for demongraphic user entry
                if (success) LaunchAboutYouPages();
            }
        }

        // Register the username and password with server
        private async Task<bool> RegisterAsync(string username, string password)
        {
            bool status = false;

            // If not connected to internet can't continue as need to register and verify user and password before progressing
            if (!DataService.Instance.IsConnected())
            {
                await DisplayAlert(
                   (string)Application.Current.Resources["text_connection_title"],
                   (string)Application.Current.Resources["text_connection_text"],
                   (string)Application.Current.Resources["text_ok"]);
                return false;
            }

            // Register the user and wait for the response from the server
            var response = await DataService.Instance.RegisterAsync(username, password);

            // Only proceed if registration successful
            if (response)
            {
                // Report sucessful registration as a toast
                DependencyService.Get<IToast>().ToastShort((string)Application.Current.Resources["text_register_success"]);
                status = true;
                Debug.WriteLine("REGISTER SUCCESSFUL!");
            }
            return status;
        }
    }
}