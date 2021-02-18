using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1LFP
{
    public class ListaToken
    {
        public NodoToken primero;
        public NodoToken ultimo;

        public ListaToken() { }

        public void Agregar(String token, String cadena, int fila, int columna, int repeticion)
        {
            Tokens act = new Tokens(token, cadena, fila, columna, repeticion);
            if(primero == null)
            {
                primero = new NodoToken(act);
                ultimo = primero;
            }
            else
            {
                NodoToken nuevo = new NodoToken(act);
                ultimo.siguiente = nuevo;
                ultimo = nuevo;
            }
        }

        public int Contar()
        {
            int contador = 0;
            NodoToken token = primero;
            while (token != null)
            {
                contador++;
                token = token.siguiente;
            }
            return contador;
        }
    }
}
