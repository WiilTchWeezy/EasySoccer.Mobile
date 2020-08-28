using EasySoccer.Mobile.Views.CustomControl.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EasySoccer.Mobile.Views.CustomControl
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CheckBoxText : ContentView
    {
        public static readonly BindableProperty TitleTextProperty = BindableProperty.Create(
                                         propertyName: "TitleText",
                                         returnType: typeof(string),
                                         declaringType: typeof(CheckBoxText),
                                         defaultValue: "",
                                         defaultBindingMode: BindingMode.TwoWay,
                                         propertyChanged: TitleTextPropertyChanged);

        public string TitleText
        {
            get { return base.GetValue(TitleTextProperty).ToString(); }
            set { base.SetValue(TitleTextProperty, value); }
        }

        public CheckBoxText()
        {
            InitializeComponent();
            this.checkBox.CheckedChanged += CheckBox_CheckedChanged;
        }

        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value)
            {
                var model = (CheckBoxGroupModel)this.BindingContext;
                model.CheckedChanged(model);
            }
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            this.checkBox.IsChecked = !this.checkBox.IsChecked;
        }

        private static void TitleTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CheckBoxText)bindable;
            control.lblText.Text = newValue.ToString();
        }
    }
}