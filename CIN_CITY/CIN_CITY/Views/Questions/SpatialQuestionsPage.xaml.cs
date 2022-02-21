using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIN_CITY.Features;
using CIN_CITY.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CIN_CITY.Views.Questions
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SpatialQuestionsPage : ContentPage
    {
        public SpatialQuestionsPage(ReportType reportType)
        {
            InitializeComponent();
            BindingContext = new SpatialQuestionsViewModel(reportType);
        }
    }
}