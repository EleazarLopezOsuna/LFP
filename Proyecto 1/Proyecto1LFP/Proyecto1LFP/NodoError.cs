using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1LFP
{
    public class NodoError
    {
        public Error error;
        public NodoError siguiente;

        public NodoError(Error error)
        {
            this.error = error;
            this.siguiente = null;
        }

        public Error GetError()
        {
            return error;
        }

        public void SetError(Error error)
        {
            this.error = error;
        }

        public NodoError GetSiguiente()
        {
            return siguiente;
        }

        public void SetSiguiente(NodoError siguiente)
        {
            this.siguiente = siguiente;
        }
    }
}
