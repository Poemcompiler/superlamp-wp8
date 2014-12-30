using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using Superlamp.Views;

namespace Superlamp.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private INavigationService navigationService;

        public MainViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
        }

        public void Initialize()
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            string url = "";
            if (localSettings.Values.ContainsKey("Name"))
                // TODO navigate mapView
                url = "VMap";
            else
                // TODO navigate insertNameView
                url = "VInsertName";
            navigationService.NavigateTo(url);
        }
    }
}