using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CIN_CITY.Features;
using CIN_CITY.Services;
using CIN_CITY.Views;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CIN_CITY.ViewModels
{
    class SpatialQuestionsViewModel : VMWithObservableProperty
    {
        public List<string> FeelingPickerSource { get; }

        public List<string> IntensityPickerSource { get; }

        public List<string> CompanyPickerSource { get; }

        private string feelingPicked;
        public string FeelingPicked
        {
            get => feelingPicked;
            set
            {
                SetPropertyAndValidate(ref feelingPicked, value);
                ShowOpinionQuestion = value.Equals((string)Application.Current.Resources["feeling_ans3"]);
                ShowMotivationQuestion = !ShowOpinionQuestion;
            }
        }

        private string intensityPicked;
        public string IntensityPicked { get => intensityPicked; set => SetPropertyAndValidate(ref intensityPicked, value); }

        private string companyPicked;
        public string CompanyPicked { get => companyPicked; set => SetPropertyAndValidate(ref companyPicked, value); }

        private string opinionText;
        public string OpinionText { get => opinionText; set => SetPropertyAndValidate(ref opinionText, value); }

        private string motivationText;
        public string MotivationText { get => motivationText; set => SetPropertyAndValidate(ref motivationText, value); }

        private bool showOpinionQuestion;
        public bool ShowOpinionQuestion { get => showOpinionQuestion; set => SetProperty(ref showOpinionQuestion, value); }

        private bool showMotivationQuestion;
        public bool ShowMotivationQuestion { get => showMotivationQuestion; set => SetProperty(ref showMotivationQuestion, value); }

        public ICommand SubmitCommand { get; }

        private bool submitEnabled;
        public bool SubmitEnabled { get => submitEnabled; set => SetProperty(ref submitEnabled, value); }

        private readonly ReportType reportType;

        public SpatialQuestionsViewModel(ReportType reportType)
        {
            this.reportType = reportType;
            SubmitCommand = new Command(async () =>
            {
                if (await Application.Current.MainPage.DisplayAlert(
                (string)Application.Current.Resources["text_submit_title"],
                (string)Application.Current.Resources["text_submit_text"],
                (string)Application.Current.Resources["text_yes"],
                (string)Application.Current.Resources["text_no"]))
                {
                    // Prepare data packet and mask with popup
                    await Application.Current.MainPage.Navigation.PushPopupAsync(new BusyPopup());
                    await Task.Run(async () => await DataService.Instance.CreateAndUploadRecord())
                    .ContinueWith(async t =>
                    {
                        await MainThread.InvokeOnMainThreadAsync(async () =>
                        {
                            try { await Application.Current.MainPage.Navigation.PopAllPopupAsync(); } catch { }
                            if (t.Result)
                            {
                                var popup = new ThankYouPopup();
                                await (Application.Current.MainPage as FlyoutPage)?.Detail.Navigation.PopToRootAsync();
                                await Application.Current.MainPage.Navigation.PushPopupAsync(popup);
                                await Task.Delay(1500);
                                await Application.Current.MainPage.Navigation.RemovePopupPageAsync(popup);
                            }
                        });
                    });
                }
            });

            FeelingPickerSource = new List<string>
            {
                (string) Application.Current.Resources["feeling_ans1"],
                (string) Application.Current.Resources["feeling_ans2"],
                (string) Application.Current.Resources["feeling_ans3"]
            };

            IntensityPickerSource = new List<string>
            {
                (string)Application.Current.Resources["intensity_ans1"],
                (string)Application.Current.Resources["intensity_ans2"],
                (string)Application.Current.Resources["intensity_ans3"],
                (string)Application.Current.Resources["intensity_ans3_1"],
                (string)Application.Current.Resources["intensity_ans4"]
            };

            CompanyPickerSource = new List<string>
            {
                (string)Application.Current.Resources["company_ans1"],
                (string)Application.Current.Resources["company_ans2"]
            };
        }

        private void SetPropertyAndValidate<T>(ref T property, T value)
        {
            SetProperty(ref property, value);
            Validate();
        }

        private void Validate()
        {
            // Validation to enable the button
            SubmitEnabled =
                !string.IsNullOrEmpty(FeelingPicked) &&
                !string.IsNullOrEmpty(IntensityPicked) &&
                !string.IsNullOrEmpty(CompanyPicked) &&
                (!ShowOpinionQuestion || (ShowOpinionQuestion && !string.IsNullOrWhiteSpace(OpinionText))) &&
                (!ShowMotivationQuestion || (ShowMotivationQuestion && !string.IsNullOrWhiteSpace(MotivationText)));
        }
    }
}