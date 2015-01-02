using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;

namespace Superlamp.Services
{
    public class GeolocationService : IGeolocationService
    {
        /// <summary>
        /// Obtains current location of device
        /// </summary>
        /// <returns>
        /// A Geoposition with latitude and longitude.
        /// Throws an exception if can't get location.
        /// if ((uint)ex.HResult == 0x80004004) -> LocationIsDisabled
        /// else -> Can't get location
        /// </returns>
        public async Task<Geoposition> GetLocation()
        {
            Geolocator geolocator = new Geolocator();
            geolocator.DesiredAccuracy = PositionAccuracy.Default;

            IAsyncOperation<Geoposition> locationTask = null;
            Geoposition position = null;
            try
            {
                locationTask = geolocator.GetGeopositionAsync(TimeSpan.FromMinutes(1), TimeSpan.FromSeconds(15));
                position = await locationTask;
            }
            finally
            {
                if (locationTask != null)
                {
                    if (locationTask.Status == AsyncStatus.Started)
                        locationTask.Cancel();

                    locationTask.Close();
                }
            }
            return position;
        }

        /// <summary>
        /// Returns the distance in miles or kilometers of any two
        /// latitude / longitude points.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public double Distance(Point from, Point to)
        {
            double R = 6371;

            double dLat = this.toRadian(to.X - from.X);
            double dLon = this.toRadian(to.Y - from.Y);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(this.toRadian(from.X)) * Math.Cos(this.toRadian(to.X)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));
            double d = R * c;

            return d;
        }

        /// <summary>
        /// Convert to Radians.
        /// </summary>
        /// <param name=”val”></param>
        /// <returns></returns>
        private double toRadian(double val)
        {
            return (Math.PI / 180) * val;
        }
    }
}
