using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1LFP
{
    public class Error
    {
        private int fila, columna;
        private string tipo, cadena;

        public Error(String tipo, String cadena, int fila, int columna)
        {
            this.tipo = tipo;
            this.cadena = cadena;
            this.fila = fila;
            this.columna = columna;
        }

        public String getTipo()
        {
            return tipo;
        }
        public String getCadena()
        {
            return cadena;
        }
        public void setTipo(String tipo)
        {
            this.tipo = tipo;
        }
        public void setCadena(String cadena)
        {
            this.cadena = cadena;
        }
        public int GetFila()
        {
            return fila;
        }
        public int GetColumna()
        {
            return columna;
        }
        public void SetFila(int fila)
        {
            this.fila = fila;
        }
        public void SetColumna(int columna)
        {
            this.columna = columna;
        }
    }
}
