using System.Collections.Generic;
using CIN_CITY.Features;
using Xamarin.Forms;

namespace CIN_CITY.ViewModels
{
    // view model defining Age and Gender information
    class AboutDemoPage1ViewModel : VMWithObservableProperty
    {
        // Icon
        public ImageSource PageIcon { get; private set; }
            = ImageSource.FromFile((OnPlatform<string>)Application.Current.Resources["icon_person"]);

        // Age List for picker
        public List<string> BirthYears { get; private set; } = new List<string>();

        // Gender  List for picker
        public List<string> Genders { get; private set; } = new List<string>();

        // Selected Age option
        private string selectedAge;
        public string SelectedAge
        {
            get
            {
                return selectedAge;
            }
            set
            {
                selectedAge = value;
                OnPropertyChanged("SelectedAge");
            }
        }
        // Selected Gender option
        private string selectedGender;
        public string SelectedGender
        {
            get
            {
                return selectedGender;
            }
            set
            {
                selectedGender = value;
                OnPropertyChanged("SelectedGender");
            }
        }

        // Ctor
        public AboutDemoPage1ViewModel()
        {
            // Create the list for age
            for (int y = 16; y < 100; y++)
                BirthYears.Add(y.ToString());

            // Create the list of gender options
            Genders.Add((string)Application.Current.Resources["text_female"]);
            Genders.Add((string)Application.Current.Resources["text_male"]);
            Genders.Add((string)Application.Current.Resources["text_trans"]);
            Genders.Add((string)Application.Current.Resources["text_prefernot"]);
        }
    }
}
