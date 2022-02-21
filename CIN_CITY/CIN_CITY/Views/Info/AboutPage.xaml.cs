using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CIN_CITY.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    // Wrapper class to display the html which provides general information about the App
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();

            // Create a web view to host an HTML privacy policy
            var myWebView = new WebView();
            var myHtmlSource = new HtmlWebViewSource();

            // Snippet from docs allow reading of an embedded resource
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(HelpPage)).Assembly;
            Stream stream = assembly.GetManifestResourceStream("CIN_CITY.HTML.about.html");
            string source = "";

            // "Using" syntax is an elegant way to manage disposable objects
            // Read the html into memory
            using (var reader = new StreamReader(stream))
            {
                source = reader.ReadToEnd();
            }

            // Assign the source to the web view
            myHtmlSource.Html = source;
            myWebView.Source = myHtmlSource;

            // For some reason, only displays if using FillAndExpand
            myWebView.HorizontalOptions = LayoutOptions.FillAndExpand;
            myWebView.VerticalOptions = LayoutOptions.FillAndExpand;

            // Stick policy on top of the stack
            MasterStack.Children.Insert(0, myWebView);
        }
        // Previous page is selected
        protected override bool OnBackButtonPressed()
        {
            try
            {
                // Show 'slider' part of the menu options onto the current page
                (Application.Current.MainPage as FlyoutPage).IsPresented = true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception caught " + e.Message + " " + e.StackTrace + " " + e.InnerException);
            }
            return true;
        }
    }
}