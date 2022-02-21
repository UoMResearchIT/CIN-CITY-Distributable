using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using CIN_CITY.Features;
using CIN_CITY.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CIN_CITY.ViewModels
{
    // View model for the master menu 
    class MasterMenuMasterViewModel : VMWithObservableProperty
    {
        public ObservableCollection<MasterMenuItem> MenuItems { get; set; }

        public string VersionText { get; } = $"{VersionTracking.CurrentVersion} (Build {VersionTracking.CurrentBuild})";

        // Define the options which will appear on the menu page
        public MasterMenuMasterViewModel()
        {
            MenuItems = new ObservableCollection<MasterMenuItem>(new[]
            {
                // First item,  'home' page with 3 further report incident menu options
                new MasterMenuItem {
                    Id = 0,
                    Title = (string)Application.Current.Resources["text_menu_reportincident"],
                    TargetType = typeof(ReportIncidentPage),
                    IconSource = ImageSource.FromFile((OnPlatform<string>)Application.Current.Resources["icon_house"])
                },
                // Second item, update alert notification preferences
                new MasterMenuItem {
                    Id = 1,
                    Title = (string)Application.Current.Resources["text_menu_notifications"],
                    TargetType = typeof(SettingsPage),
                    IconSource = ImageSource.FromFile((OnPlatform<string>)Application.Current.Resources["icon_alarm_small"])
                },
                    // Third item, display 'about' html information
                new MasterMenuItem {
                        Id = 2,
                    Title = (string)Application.Current.Resources["text_menu_about"],
                    TargetType = typeof(AboutPage),
                    IconSource = ImageSource.FromFile((OnPlatform<string>)Application.Current.Resources["icon_info"])
                },
                    // Forth item, display 'help' html information
                    new MasterMenuItem {
                    Id = 3,
                    Title = (string)Application.Current.Resources["text_menu_help"],
                    TargetType = typeof(HelpPage),
                    IconSource = ImageSource.FromFile((OnPlatform<string>)Application.Current.Resources["icon_help"])
                },
                // Fifth item, exit the Application
                    new MasterMenuItem {
                    Id = 4,
                    Title = (string)Application.Current.Resources["text_menu_exit"],
                    TargetType = null,
                    IconSource = ImageSource.FromFile((OnPlatform<string>)Application.Current.Resources["icon_close"])
                }
            });
        }
    }
}
