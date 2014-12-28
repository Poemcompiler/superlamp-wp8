using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;

namespace Superlamp.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private INavigationService navigationService;

        public MainViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;

            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            string uri = "";
            if (localSettings.Values.ContainsKey("Name"))
            {
                // TODO navigate mapView
                uri = "VMap";

            }
            else
            {
                // TODO navigate insertNameView
                uri = "VInsertName";
            }

            navigationService.NavigateTo(uri);
        }
    }
}