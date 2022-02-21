using System.Collections.Generic;
using CIN_CITY.Features;
using CIN_CITY.Views;
using Xamarin.Forms;

namespace CIN_CITY.ViewModels
{
    // view model defining Ethnicity, first letters of home and school postcodes
    class AboutEthPostCodesPage2ViewModel : VMWithObservableProperty
    {
        private bool ethnicityEnabled = true;
        public bool EthnicityEnabled
        {
            get
            {
                return ethnicityEnabled;
            }
            set
            {
                ethnicityEnabled = value;
                OnPropertyChanged("EthnicityEnabled");
            }
        }
        // Ethnicity List for picker
        public List<string> Ethnicity { get; private set; } = new List<string>();

        // Selected Ethnicity option
        private string selectedEthnicity;
        public string SelectedEthnicity
        {
            get
            {
                return selectedEthnicity;
            }
            set
            {
                selectedEthnicity = value;
                OnPropertyChanged("SelectedEthnicity");
            }
        }
        // First 3 characters of home postcode
        public string HomePC { get; set; }

        // First 3 characters of school postcode
        public string SchoolPC { get; set; }

        public ImageSource PageIcon { get; private set; }
            = ImageSource.FromFile((OnPlatform<string>)Application.Current.Resources["icon_flower"]);

        // Ctor
        public AboutEthPostCodesPage2ViewModel()
        {
            // Create the entries for the list of ethnicity options
            Ethnicity.Add((string)Application.Current.Resources["text_white_british"]);
            Ethnicity.Add((string)Application.Current.Resources["text_white_irish"]);
            Ethnicity.Add((string)Application.Current.Resources["text_white_other"]);
        }
    }
}
