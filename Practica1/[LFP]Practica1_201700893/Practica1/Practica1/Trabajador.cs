using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practica1
{
    class Trabajador
    {
        private String nombre;
        private String codigo;
        private String superiores;

        public Trabajador(String nombre, String codigo, String superiores)
        {
            this.nombre = nombre;
            this.codigo = codigo;
            this.superiores = superiores;
        }

        public String GetNombre()
        {
            return nombre;
        }
        public String GetCodigo()
        {
            return codigo;
        }
        public String GetSuperiores()
        {
            return superiores;
        }
        public void SetNombre(String nombre)
        {
            this.nombre = nombre;
        }
        public void SetCodigo(String codigo)
        {
            this.codigo = codigo;
        }
        public void SetSuperiores(String superiores)
        {
            this.superiores = superiores;
        }
    }
}
