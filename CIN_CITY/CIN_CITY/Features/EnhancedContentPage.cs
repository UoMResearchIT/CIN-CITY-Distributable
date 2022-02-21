using Xamarin.Forms;

namespace CIN_CITY.Features
{
    // Base template page for App which groups common features together 
    // Each new App page can inherit these values
    public class EnhancedContentPage : ContentPage, IBusyIndicator
    {
        // Text on the activity indicator
        private string indicatorText = (string)Application.Current.Resources["text_talking"];
        public string IndicatorText
        {
            get
            {
                return indicatorText;
            }
            set
            {
                indicatorText = value;
                OnPropertyChanged("IndicatorText");
            }
        }

        // Constructor taking title -- will be overridden by XAML setting
        public EnhancedContentPage(string title) : this()
        {
            Title = title;
        }

        // Default Constructor
        public EnhancedContentPage()
        {
        }

        // Interface Implementation
        public void SetBusy()
        {
            Device.BeginInvokeOnMainThread(() => IsBusy = false);
            base.IsBusy = false;
        }

        public void SetBusy(string message)
        {
            IndicatorText = message;
            Device.BeginInvokeOnMainThread(() => IsBusy = true);
            base.IsBusy = true;
        }

        // Activity Indicator creation
        public void InitialiseEnhancedPage(AbsoluteLayout layout)
        {
            // Build background
            var box = new BoxView
            {
                BackgroundColor = (Color)Application.Current.Resources["uom_grey"],
                Opacity = 0.9
            };
            AbsoluteLayout.SetLayoutBounds(box, new Rectangle(.5, .5, 1, 1));
            AbsoluteLayout.SetLayoutFlags(box, AbsoluteLayoutFlags.All);

            // Build activity indicator
            var activityStack = new StackLayout()
            {
                Padding = new Thickness((double)Application.Current.Resources["margin"]),
                HorizontalOptions = LayoutOptions.Fill
            };
            //activityStack.BackgroundColor.A = 0.5;
            var indicator = new ActivityIndicator()
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                IsRunning = false,
                Color = Color.White
            };
            var text = new Label()
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Fill,
                Text = "<NULL>",
                TextColor = Color.White
            };
            activityStack.Children.Add(indicator);
            activityStack.Children.Add(text);
            AbsoluteLayout.SetLayoutBounds(activityStack, new Rectangle(.5, .5, -1, -1));
            AbsoluteLayout.SetLayoutFlags(activityStack, AbsoluteLayoutFlags.PositionProportional);
            layout.Children.Add(box);
            layout.Children.Add(activityStack);

            // Set bindings
            indicator.BindingContext = this;
            indicator.SetBinding(ActivityIndicator.IsRunningProperty, new Binding("IsBusy"));
            text.BindingContext = this;
            text.SetBinding(Label.TextProperty, new Binding("IndicatorText"));
            activityStack.BindingContext = this;
            activityStack.SetBinding(StackLayout.IsVisibleProperty, new Binding("IsBusy"));
            box.BindingContext = this;
            box.SetBinding(BoxView.IsVisibleProperty, new Binding("IsBusy"));
            SetBusy();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}
