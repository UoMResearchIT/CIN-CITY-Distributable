using System.Diagnostics;
using CIN_CITY.Features;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CIN_CITY.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    // Code behind for page which displays a thank you to the user for paticipating in the study

    public partial class ThankYouPopup : Rg.Plugins.Popup.Pages.PopupPage
    {
        public ThankYouPopup()
        {
            InitializeComponent();

            // Icons
            PageIcon.Source = ImageSource.FromFile((OnPlatform<string>)Application.Current.Resources["icon_thumbup"]);
        }
    }
}