using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CIN_CITY.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    // Displayed from the 'Registration' option to confirm entered password matches registration password
    public partial class ConfirmPassWordPopUp : Rg.Plugins.Popup.Pages.PopupPage
    {
        readonly Action<string> actionClicked;

        internal ConfirmPassWordPopUp(Action<string> actionOnConfirmClicked)
        {
            InitializeComponent();
            actionClicked = actionOnConfirmClicked;
            Title = (string)Application.Current.Resources["text_confirmpassword_popup"];
            BindingContext = this;
        }

        // Invoked when a hardware back button is pressed
        protected override bool OnBackButtonPressed()
        {
            // Return true if you don't want to close this popup page when a back button is pressed
            return base.OnBackButtonPressed();
        }

        // Invoked when background is clicked
        protected override bool OnBackgroundClicked()
        {
            // Return false if you don't want to close this popup page when a background of the popup page is clicked
            return base.OnBackgroundClicked();
        }

        // Invoked when background is clicked
        protected void OnConfirmClicked(object sender, EventArgs e)
        {
            // Kickoff the method in login to check the password
            actionClicked.Invoke(PasswordEntry.Text);
        }
    }
}