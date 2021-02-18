using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1LFP
{
    public class ListaCancion
    {
        public NodoCancion primero;
        public NodoCancion ultimo;

        public ListaCancion() { }

        public void Agregar(String duracion, String nombre, String año, String url, String album, String artista, String playlist)
        {
            Cancion act = new Cancion(duracion, nombre, año, url, album, artista, playlist);
            if (primero == null)
            {
                primero = new NodoCancion(act);
                ultimo = primero;
            }
            else
            {
                NodoCancion nuevo = new NodoCancion(act);
                ultimo.siguiente = nuevo;
                ultimo = nuevo;
            }
        }

        public int Contar()
        {
            int contador = 0;
            NodoCancion cancion = primero;
            while (cancion != null)
            {
                contador++;
                cancion = cancion.siguiente;
            }
            return contador;
        }
    }
}
