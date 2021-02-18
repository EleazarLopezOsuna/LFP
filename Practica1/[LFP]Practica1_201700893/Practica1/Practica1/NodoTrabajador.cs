using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practica1
{
    class NodoTrabajador
    {
        public Trabajador trabajador;
        public NodoTrabajador siguiente;

        public NodoTrabajador(Trabajador trabajador)
        {
            this.trabajador = trabajador;
            this.siguiente = null;
        }

        public Trabajador GetTrabajador()
        {
            return trabajador;
        }

        public void SetToken(Trabajador trabajador)
        {
            this.trabajador = trabajador;
        }

        public NodoTrabajador GetSiguiente()
        {
            return siguiente;
        }

        public void SetSiguiente(NodoTrabajador siguiente)
        {
            this.siguiente = siguiente;
        }
    }
}
