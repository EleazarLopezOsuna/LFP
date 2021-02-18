using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Collections;

namespace Proyecto1LFP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        String FilePath = "";
       
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Laberinto |*.DSI";
            open.Title = "Seleccionar Laberinto";
            if (open.ShowDialog() == DialogResult.OK)
            {
                FilePath = open.FileName;
                System.IO.StreamReader str = new System.IO.StreamReader(open.FileName, Encoding.Default);
                String contenido = str.ReadToEnd();
                str.Close();
                richTextBox1.Text = contenido;
            }
            open.Dispose();
        }

        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            StreamWriter writer;
            switch (FilePath)
            {
                case "":
                    save.Filter = "Laberinto |*.DSI";
                    save.Title = "Seleccionar Laberinto";
                    if (save.ShowDialog() == DialogResult.OK)
                    {
                        writer = File.CreateText(save.FileName);
                        writer.Write(richTextBox1.Text);
                        writer.Close();
                        MessageBox.Show("Archivo Guardado", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    FilePath = save.FileName;
                    break;

                default:
                    writer = new StreamWriter(FilePath);
                    writer.Write(richTextBox1.Text);
                    writer.Close();
                    MessageBox.Show("Archivo Guardado", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
            }
        }

        private void guardarComoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            StreamWriter writer;
            save.Filter = "Laberinto |*.DSI";
            save.Title = "Seleccionar Laberinto";
            if (save.ShowDialog() == DialogResult.OK)
            {
                writer = File.CreateText(save.FileName);
                writer.Write(richTextBox1.Text);
                writer.Close();
                MessageBox.Show("Archivo Guardado", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                FilePath = save.FileName;
            }
        }

        private void acercaDeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            informacion inf = new informacion();
            inf.Show();
        }

        private void anaizarToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Salir del Programa", "Salir", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                this.Close();
            }
        }
    }

}
