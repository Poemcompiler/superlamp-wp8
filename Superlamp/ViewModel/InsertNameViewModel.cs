using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;

namespace Superlamp.ViewModel
{
    public class InsertNameViewModel : ViewModelBase
    {
        private INavigationService navigationService;

        public InsertNameViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;

            InitializeCommands();
        }

        private string _name;
        private RelayCommand _doAccept;

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
                RaisePropertyChanged("Name");
            }
        }

        public ICommand DoAccept
        {
            get
            {
                return _doAccept;
            }
        }

        private void InitializeCommands()
        {
            _doAccept = new RelayCommand(() => {
                var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                localSettings.Values.Add(new KeyValuePair<string, object>("Name", _name));

                navigationService.NavigateTo("VMap");
            });
        }
    }
}
