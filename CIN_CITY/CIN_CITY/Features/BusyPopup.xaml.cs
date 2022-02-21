
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CIN_CITY.Features
{
    // Displays a piece of text to indicate that the App is working on something

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BusyPopup : Rg.Plugins.Popup.Pages.PopupPage
    {
        private string text;
        public string Text { get { return text; } set { text = value; OnPropertyChanged("Text"); } }

        public BusyPopup(string title) : this()
        {
            Text = title;
        }

        public BusyPopup()
        {
            InitializeComponent();
            BindingContext = this;
            Text = (string)Application.Current.Resources["text_talking"];
        }

        // Invoked when a hardware back button is pressed
        protected override bool OnBackButtonPressed()
        {
            // Return true if you don't want to close this popup page when a back button is pressed
            return true;
        }

        // Invoked when background is clicked
        protected override bool OnBackgroundClicked()
        {
            // Return false if you don't want to close this popup page when a background of the popup page is clicked
            return false;
        }

    }
}