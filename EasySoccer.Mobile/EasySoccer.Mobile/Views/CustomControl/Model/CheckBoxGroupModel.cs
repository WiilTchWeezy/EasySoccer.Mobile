using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EasySoccer.Mobile.Views.CustomControl.Model
{
    public class CheckBoxGroupModel : INotifyPropertyChanged
    {
        private bool _isChecked;
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (_isChecked != value)
                {
                    _isChecked = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsChecked)));
                }
            }
        }

        public string Text { get; set; }

        public string Value { get; set; }

        public Action<CheckBoxGroupModel> CheckedChanged { get; set; }

        public CheckBoxGroupModel()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
