using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practica1
{
    class ListaTrabajador
    {
        public NodoTrabajador primero;
        public NodoTrabajador ultimo;

        public ListaTrabajador() { }

        public void Agregar(String nombre, String codigo, String superiores)
        {
            Trabajador act = new Trabajador(nombre, codigo, superiores);
            if (primero == null)
            {
                primero = new NodoTrabajador(act);
                ultimo = primero;
            }
            else
            {
                NodoTrabajador nuevo = new NodoTrabajador(act);
                ultimo.siguiente = nuevo;
                ultimo = nuevo;
            }
        }
    }
}
