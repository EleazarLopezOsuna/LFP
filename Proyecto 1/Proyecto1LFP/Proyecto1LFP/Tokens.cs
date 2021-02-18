using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1LFP
{
    public class Tokens
    {
        public String token, cadena;
        public int fila, columna, repeticion;

        public Tokens(String token, String cadena, int fila, int columna, int repeticion)
        {
            this.token = token;
            this.cadena = cadena;
            this.fila = fila;
            this.columna = columna;
            this.repeticion = repeticion;
        }

        public String GetToken() => token;
        public int GetFila() => fila;
        public int GetColumna() => columna;
        public String GetCadena() => cadena;
        public int GetRepeticion() => repeticion;

        public void SetToken(String token) => this.token = token;
        public void SetCadena(String cadena) => this.cadena = cadena;
        public void SetFila(int fila) => this.fila = fila;
        public void SetColumna(int columna) => this.columna = columna;
        public void setRepeticion(int repeticion) => this.repeticion = repeticion;
    }
}
