using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Newtonsoft.Json;
using Superlamp.Model;
using Superlamp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Devices.Enumeration;
using Windows.Devices.Geolocation;
using Windows.Media.Capture;

namespace Superlamp.ViewModel
{
    public class MapViewModel : ViewModelBase
    {
        MediaCapture mc = null;
        private IGeolocationService geolocationService;
        private IDialogService dialogService;
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

                    if (tc.Enabled)
                    {
                        // make flashlight visible
                    }
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
            await GetCurrentLocation();
            await GetOtherLocations();
        }

        /// <summary>
        /// Gets current location
        /// </summary>
        /// <returns></returns>
        async Task GetCurrentLocation()
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

        /// <summary>
        /// Gets other superlamp user locations
        /// </summary>
        /// <returns></returns>
        public async Task GetOtherLocations()
        {
            System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "http://superlamp.azurewebsites.net/getpositions.php");

            HttpResponseMessage response = await client.SendAsync(request);
            string data = await response.Content.ReadAsStringAsync();
            List<FlashLight> light = JsonConvert.DeserializeObject<List<FlashLight>>(data);
        }
    }
}
