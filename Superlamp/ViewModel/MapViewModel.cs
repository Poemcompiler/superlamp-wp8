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
using Windows.Devices.Enumeration;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Media.Capture;
using Windows.UI.Xaml.Controls.Maps;

namespace Superlamp.ViewModel
{
    public class MapViewModel : ViewModelBase
    {
        MediaCapture mc = null;
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
                if (mc == null)
                {
                    mc = new MediaCapture();
                    var cameraId = await GetCameraId(Windows.Devices.Enumeration.Panel.Back);

                    var settings = new MediaCaptureInitializationSettings { VideoDeviceId = cameraId.Id };
                    settings.StreamingCaptureMode = StreamingCaptureMode.Video;

                    await mc.InitializeAsync(settings);
                }
                var videoDev = mc.VideoDeviceController;
                var tc = videoDev.TorchControl;
                if (tc.Supported)
                {
                    if (tc.PowerSupported)
                        tc.PowerPercent = 100;
                    tc.Enabled = !tc.Enabled;
                }
            });
        }

        private static async Task<DeviceInformation> GetCameraId(Windows.Devices.Enumeration.Panel desiredCamera)
        {
            DeviceInformation deviceID = (await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture))
                .FirstOrDefault(x => x.EnclosureLocation != null && x.EnclosureLocation.Panel == desiredCamera);

            if (deviceID != null) return deviceID;
            else throw new Exception(string.Format("Camera of type {0} doesn't exist.", desiredCamera));
        }

        public async Task StartMap()
        {
            await GetLocation();
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
            if (!string.IsNullOrEmpty(errorString))
                await dialogService.ShowMessage(errorString, "title");
        }
    }
}
