using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practica1
{
    class NodoToken
    {
        public Tokens token;
        public NodoToken siguiente;

        public NodoToken(Tokens token)
        {
            this.token = token;
            this.siguiente = null;
        }

        public Tokens GetToken()
        {
            return token;
        }

        public void SetToken(Tokens token)
        {
            this.token = token;
        }

        public NodoToken GetSiguiente()
        {
            return siguiente;
        }

        public void SetSiguiente(NodoToken siguiente)
        {
            this.siguiente = siguiente;
        }
    }
}
