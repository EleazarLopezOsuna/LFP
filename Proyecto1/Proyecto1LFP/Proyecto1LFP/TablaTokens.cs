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
    public partial class TablaTokens : Form
    {
        public TablaTokens()
        {
            InitializeComponent();
            agregar();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }
        void agregar()
        {
            NodoToken token = Form1.lt.primero;
            while (token != null)
            {
                int rowld = dataGridView1.Rows.Add();
                DataGridViewRow row = dataGridView1.Rows[rowld];
                row.Cells["Token"].Value = token.GetToken().GetToken();
                row.Cells["Cadena"].Value = token.GetToken().GetCadena();
                row.Cells["Fila"].Value = token.GetToken().GetFila();
                row.Cells["Columna"].Value = token.GetToken().GetColumna();
                token = token.siguiente;
                
            }
        }
    }
}
