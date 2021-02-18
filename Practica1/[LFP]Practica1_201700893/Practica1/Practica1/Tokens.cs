using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practica1
{
    class Tokens
    {
        private String token, cadena;
        private int fila, columna;

        public Tokens(String token, String cadena, int fila, int columna)
        {
            this.token = token;
            this.cadena = cadena;
            this.fila = fila;
            this.columna = columna;
        }

        public String GetToken()
        {
            return token;
        }
        public int GetFila()
        {
            return fila;
        }
        public int GetColumna()
        {
            return columna;
        }
        public String GetCadena()
        {
            return cadena;
        }
        public void SetToken(String token)
        {
            this.token = token;
        }
        public void SetCadena(String cadena)
        {
            this.cadena = cadena;
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
