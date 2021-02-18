using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto1LFP
{
    public partial class TablaErrores : Form
    {
        public TablaErrores()
        {
            InitializeComponent();
            agregar();
        }
        void agregar()
        {
            NodoError token = Form1.le.primero;
            while (token != null)
            {
                int rowld = dataGridView1.Rows.Add();
                DataGridViewRow row = dataGridView1.Rows[rowld];
                row.Cells["Token"].Value = token.GetError().getTipo();
                row.Cells["Cadena"].Value = token.GetError().getCadena();
                row.Cells["Fila"].Value = token.GetError().GetFila();
                row.Cells["Columna"].Value = token.GetError().GetColumna();
                token = token.siguiente;
            }
        }
    }
}
