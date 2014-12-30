using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Superlamp.Services
{
    public interface IDataService
    {
        Task<int> AddLocation();
    }
}
