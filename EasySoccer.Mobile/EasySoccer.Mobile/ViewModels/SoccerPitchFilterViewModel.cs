using EasySoccer.Mobile.Views.CustomControl.Model;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace EasySoccer.Mobile.ViewModels
{
    public class SoccerPitchFilterViewModel : BindableBase, INavigationAware
    {
        public List<CheckBoxGroupModel> OrderFields { get; set; }
        public List<CheckBoxGroupModel> OrderPositions { get; set; }

        private string _titleOrderFields;
        public string TitleOrderFields
        {
            get { return _titleOrderFields; }
            set { SetProperty(ref _titleOrderFields, value); }
        }

        private string _titleOrderPositions;
        public string TitleOrderPositions
        {
            get { return _titleOrderPositions; }
            set { SetProperty(ref _titleOrderPositions, value); }
        }

        private string _filterText;
        public string FilterText
        {
            get { return _filterText; }
            set { SetProperty(ref _filterText, value); }
        }

        public DelegateCommand FilterCommand { get; set; }
        private INavigationService _navigationService;
        public SoccerPitchFilterViewModel(INavigationService navigationService)
        {
            OrderFields = new List<CheckBoxGroupModel>();
            OrderPositions = new List<CheckBoxGroupModel>();
            this.InitOrderFields();
            this.InitOrderPositions();
            TitleOrderFields = "Ordenação";
            TitleOrderPositions = "Posição da Ordenação";
            FilterCommand = new DelegateCommand(Filter);
            _navigationService = navigationService;
        }
        private void Filter()
        {
            var navigationParameters = new NavigationParameters();
            navigationParameters.Add("OrderField", OrderFields.Where(x => x.IsChecked).FirstOrDefault()?.Value);
            navigationParameters.Add("OrderDirection", OrderPositions.Where(x => x.IsChecked).FirstOrDefault()?.Value);
            navigationParameters.Add("FilterText", FilterText);
            _navigationService.GoBackAsync(navigationParameters);
        }

        private void InitOrderFields()
        {
            OrderFields.Add(new CheckBoxGroupModel
            {
                IsChecked = true,
                Text = "Próximo a sua localização",
                Value = "Location",
                CheckedChanged = (model) =>
                {
                    this.CheckBoxHasChanged(model, OrderFields);
                }
            });
            OrderFields.Add(new CheckBoxGroupModel
            {
                IsChecked = false,
                Text = "Nome",
                Value = "Name",
                CheckedChanged = (model) =>
                {
                    this.CheckBoxHasChanged(model, OrderFields);
                }
            });
        }

        private void InitOrderPositions()
        {
            OrderPositions.Add(new CheckBoxGroupModel
            {
                IsChecked = true,
                Text = "Crescente",
                Value = "ASC",
                CheckedChanged = (model) =>
                {
                    this.CheckBoxHasChanged(model, OrderPositions);
                }
            });

            OrderPositions.Add(new CheckBoxGroupModel
            {
                IsChecked = false,
                Text = "Decrescente",
                Value = "DESC",
                CheckedChanged = (model) =>
                {
                    this.CheckBoxHasChanged(model, OrderPositions);
                }
            });

        }

        private void CheckBoxHasChanged(CheckBoxGroupModel model, List<CheckBoxGroupModel> checkBoxGroups)
        {
            checkBoxGroups.ForEach(x =>
            {
                if (!x.Text.Equals(model.Text))
                    x.IsChecked = false;
            });
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
        }
    }
}
