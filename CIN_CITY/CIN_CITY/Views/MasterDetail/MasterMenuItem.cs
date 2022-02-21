using System;
using Xamarin.Forms;

namespace CIN_CITY.Views
{
    // Base class 
    public class MasterMenuItem
    {
        // Menu item structure for main menu options
        public MasterMenuItem()
        {
            TargetType = typeof(ContentPage);

        }
        // Position on the menu page for the menu item
        public int Id { get; set; }

        // Title displayed on the menu page for the menu item
        public string Title { get; set; }

        // Icon associated with the menu item on the menu page
        public ImageSource IconSource { get; set; }

        // Type of content page which will be navigated to from the menu page
        // Null if the option is 'exit'
        public Type TargetType { get; set; }
    }
}