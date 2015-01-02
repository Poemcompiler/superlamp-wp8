using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Superlamp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Media.Capture;
using Windows.UI.Xaml.Controls.Maps;

namespace Superlamp.ViewModel
{
    public class MapViewModel : ViewModelBase
    {
        private IGeolocationService geolocationService;
        private IDialogService dialogService;
        private bool _hasLocation;
        private Geopoint _myPoint;
        private RelayCommand _changeLight;

        public MapViewModel(IDialogService dialogService, IGeolocationService geolocationService)
        {
            this.dialogService = dialogService;
            this.geolocationService = geolocationService;

            InitializeCommands();
        }

        public Geopoint MyPoint
        {
            get { return _myPoint; }
            set
            {
                _myPoint = value;
                RaisePropertyChanged(() => MyPoint);
            }
        }


        public ICommand ChangeLight
        {
            get
            {
                return _changeLight;
            }
        }

        private void InitializeCommands()
        {
            _changeLight = new RelayCommand(async () =>
            {
                var mediaDev = new MediaCapture();
                await mediaDev.InitializeAsync();
                var videoDev = mediaDev.VideoDeviceController;
                var tc = videoDev.TorchControl;
                if (tc.Supported)
                {
                    if (tc.PowerSupported)
                        tc.PowerPercent = 100;
                    tc.Enabled = true;
                }
            });
        }

        public void StartMap()
        {

        }

        /// <summary>
        /// Gets current location
        /// </summary>
        /// <returns></returns>
        async Task GetLocation()
        {
            String errorString = String.Empty;
            try
            {
                Geoposition geoposition = await geolocationService.GetLocation();
                MyPoint = geoposition.Coordinate.Point;
            }
            catch (Exception ex)
            {
                if ((uint)ex.HResult == 0x80004004)
                    errorString = "locationDisabled";
                else
                    errorString = "cantGetLocation";
            }
            if (string.IsNullOrEmpty(errorString))
                await dialogService.ShowMessage(errorString, "title");
        }
    }
}
