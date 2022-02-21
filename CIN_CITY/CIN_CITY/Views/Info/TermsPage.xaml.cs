using System;
using System.IO;
using System.Reflection;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CIN_CITY.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    // Wrapper class to display the html which provides Terms/Policy information for the App
    public partial class TermsPage : ContentPage
    {
        public TermsPage()
        {
            InitializeComponent();

            // Create a web view to host an HTML privacy policy
            var myWebView = new WebView();
            var myHtmlSource = new HtmlWebViewSource();

            // Snippet from docs allow reading of an embedded resource
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(TermsPage)).Assembly;
            Stream stream = assembly.GetManifestResourceStream("CIN_CITY.HTML.policy.html");
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
            MasterStack.Content = myWebView;
        }

        protected void BackClicked(object sender, EventArgs args)
        {
            base.OnBackButtonPressed();
        }

        protected override bool OnBackButtonPressed()
        {
            return base.OnBackButtonPressed();
        }
    }
}