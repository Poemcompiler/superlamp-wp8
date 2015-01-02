using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;

namespace Superlamp.Services
{
    public interface IGeolocationService
    {
        Task<Geoposition> GetLocation();

        double Distance(Point from, Point to);

    }
}
