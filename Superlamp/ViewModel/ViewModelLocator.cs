/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:Superlamp"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using Superlamp.Services;
using Superlamp.Views;

namespace Superlamp.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public sealed class ViewModelLocator
    {
        static readonly object padlock = new object();
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<MapViewModel>();
            SimpleIoc.Default.Register<InsertNameViewModel>();

            var navigationService = this.CreateNavigationService();
            SimpleIoc.Default.Register<IDataService, DataService>();
            SimpleIoc.Default.Register<INavigationService>(() => navigationService);
            SimpleIoc.Default.Register<IDialogService, DialogService>();
        }

        private INavigationService CreateNavigationService()
        {
            var navigationService = new NavigationService();
            navigationService.Configure("VMain", typeof(MainPage));
            navigationService.Configure("VMap", typeof(MapPage));
            navigationService.Configure("VInsertName", typeof(InsertNamePage));
            return navigationService;
        }

        public MainViewModel VMMain
        {
            get
            {
                lock(padlock)
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public MapViewModel VMMap
        {
            get
            {
                lock(padlock)
                return ServiceLocator.Current.GetInstance<MapViewModel>();
            }
        }

        public InsertNameViewModel VMInsertName
        {
            get
            {
                lock(padlock)
                return ServiceLocator.Current.GetInstance<InsertNameViewModel>();
            }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}