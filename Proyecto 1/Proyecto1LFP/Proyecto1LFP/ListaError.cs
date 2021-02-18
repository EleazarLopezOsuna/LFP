using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1LFP
{
    public class ListaError
    {
        public NodoError primero;
        public NodoError ultimo;

        public ListaError() { }

        public void Agregar(String tipo, String cadena,int fila, int columna)
        {
            Error act = new Error(tipo, cadena,fila, columna);
            if (primero == null)
            {
                primero = new NodoError(act);
                ultimo = primero;
            }
            else
            {
                NodoError nuevo = new NodoError(act);
                ultimo.siguiente = nuevo;
                ultimo = nuevo;
            }
        }

        public int Contar()
        {
            int contador = 0;
            NodoError error = primero;
            while (error != null)
            {
                contador++;
                error = error.siguiente;
            }
            return contador;
        }
    }
}
