using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Superlamp.Model
{
    public class FlashLight
    {
        private string latitud;
        private string longitud;
        private string nom;
        private string temps;

        public string Latitud
        {
            get
            {
                return latitud;
            }

            set
            {
                latitud = value;
            }
        }

        public string Longitud
        {
            get
            {
                return longitud;
            }

            set
            {
                longitud = value;
            }
        }

        public string Nom
        {
            get
            {
                return nom;
            }

            set
            {
                nom = value;
            }
        }

        public string Temps
        {
            get
            {
                return temps;
            }

            set
            {
                temps = value;
            }
        }
    }
}
