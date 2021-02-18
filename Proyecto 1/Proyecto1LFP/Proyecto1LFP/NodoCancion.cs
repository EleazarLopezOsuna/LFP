using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1LFP
{
    public class NodoCancion
    {
        public Cancion cancion;
        public NodoCancion siguiente;

        public NodoCancion(Cancion cancion)
        {
            this.cancion = cancion;
            this.siguiente = null;
        }

        public Cancion GetCancion()
        {
            return cancion;
        }

        public void SetCancion(Cancion cancion)
        {
            this.cancion = cancion;
        }

        public NodoCancion GetSiguiente()
        {
            return siguiente;
        }

        public void SetSiguiente(NodoCancion siguiente)
        {
            this.siguiente = siguiente;
        }
    }
}
