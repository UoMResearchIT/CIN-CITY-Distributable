using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using CIN_CITY.Droid.CustomRenderers;
using CIN_CITY.Features;

[assembly: ExportRenderer(typeof(CustomPicker), typeof(CustomPickerRenderer))]
namespace CIN_CITY.Droid.CustomRenderers
{
    public class CustomPickerRenderer : PickerRenderer
    {
        IElementController ElementController => Element;

        public CustomPickerRenderer(Context context) : base(context)
        {
        }

        private AlertDialog _dialog;

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);
            Control.Click += EditText_Click;
        }

        private void EditText_Click(object sender, EventArgs e)
        {
            if (Element.Items != null && Element.Items.Any())
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(Context, Resource.Style.AppCompatDialogStyle);
                builder.SetTitle(Element.Title ?? "");

                ArrayAdapter<string> arrayAdapter = new ArrayAdapter<string>(Context, Resource.Layout.picker_item, Resource.Id.title_label);
                foreach (var item in Element.Items) arrayAdapter.Add(item);

                builder.SetNegativeButton((string)Xamarin.Forms.Application.Current.Resources["text_cancel"], (s, e) =>
                {
                    ElementController.SetValueFromRenderer(VisualElement.IsFocusedProperty, false);
                    // It is possible for the Content of the Page to be changed when Focus is changed.
                    // In this case, we'll lose our Control.
                    Control?.ClearFocus();
                    _dialog?.Dismiss();
                });

                builder.SetAdapter(arrayAdapter, (s, e) =>
                {
                    string strName = arrayAdapter.GetItem(e.Which);
                    ElementController.SetValueFromRenderer(Picker.SelectedItemProperty, strName);
                    // It is possible for the Content of the Page to be changed on SelectedIndexChanged.
                    // In this case, the Element & Control will no longer exist.
                    if (Element != null)
                    {
                        ElementController.SetValueFromRenderer(VisualElement.IsFocusedProperty, false);
                        // It is also possible for the Content of the Page to be changed when Focus is changed.
                        // In this case, we'll lose our Control.
                        Control?.ClearFocus();
                    }
                    _dialog?.Dismiss();
                });
                _dialog = builder.Show();
            }





            //var list = new NumberPicker(Context);

            //            if (Element.Items != null && Element.Items.Any())
            //            {
            //                // set style here
            //                picker.MaxValue = Element.Items.Count - 1;
            //                picker.MinValue = 0;

            //                //picker.SetBackgroundColor(Android.Graphics.Color.White);
            //                picker.SetDisplayedValues(Element.Items.ToArray());
            //                picker.WrapSelectorWheel = false;
            //                picker.Value = Element.SelectedIndex;
            //                picker.
            //            }

            //            var layout = new LinearLayout(Context) { Orientation = Orientation.Vertical };
            //            layout.AddView(picker);
            //            ElementController.SetValueFromRenderer(VisualElement.IsFocusedProperty, true);
            //            var builder = new AlertDialog.Builder(Context, Resource.Style.AppCompatDialogStyle);
            //            builder.SetView(layout);
            //            builder.SetTitle(Element.Title ?? "");
            //            builder.SetNegativeButton("Cancel", (s, a) =>
            //            {
            //                ElementController.SetValueFromRenderer(VisualElement.IsFocusedProperty, false);
            //                // It is possible for the Content of the Page to be changed when Focus is changed.
            //                // In this case, we'll lose our Control.
            //                Control?.ClearFocus();
            //                _dialog = null;
            //            });
            //            builder.SetPositiveButton("Confirm", (s, a) =>
            //            {
            //                ElementController.SetValueFromRenderer(Picker.SelectedIndexProperty, picker.Value);
            //                // It is possible for the Content of the Page to be changed on SelectedIndexChanged.
            //                // In this case, the Element & Control will no longer exist.
            //                if (Element != null)
            //                {
            //                    ElementController.SetValueFromRenderer(VisualElement.IsFocusedProperty, false);
            //                    // It is also possible for the Content of the Page to be changed when Focus is changed.
            //                    // In this case, we'll lose our Control.
            //                    Control?.ClearFocus();
            //                }
            //                _dialog = null;
            //            });

            //            _dialog = builder.Create();
            //            _dialog.DismissEvent += (s, a) =>
            //            {
            //                ElementController?.SetValueFromRenderer(VisualElement.IsFocusedProperty, false);
            //            };
            //            _dialog.Show();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Control.Click -= EditText_Click;
            }
            base.Dispose(disposing);
        }
    }
}