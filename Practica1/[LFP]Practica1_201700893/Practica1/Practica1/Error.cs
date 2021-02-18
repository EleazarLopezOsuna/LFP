using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practica1
{
    class Error
    {
        private int fila, columna;

        public Error(int fila, int columna)
        {
            this.fila = fila;
            this.columna = columna;
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
