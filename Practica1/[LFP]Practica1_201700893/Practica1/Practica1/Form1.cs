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

namespace Practica1
{
    public partial class Form1 : Form
    {
        String FilePath = "";
        String[] reservadas = { "organigrama", "trabajador", "codigo", "nombre", "superiores" };
        String[] operadores = { ":", ";", "{", "}", "," };
        int[] errores = { 8000000, 8000000 };
        ListaToken lt;
        ListaError le;
        ListaTrabajador ltr;
        String nombreGrafica = string.Empty;

        public Form1()
        {
            InitializeComponent();
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Organigrama |*.ogm";
            open.Title = "Seleccionar Organigrama";
            if (open.ShowDialog() == DialogResult.OK)
            {
                FilePath = open.FileName;
                String reader = File.ReadAllText(FilePath);
                richTextBox1.Text = reader;
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
                    save.Filter = "Organigrama|.ogm";
                    save.Title = "Guardar Organigrama";
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
            save.Filter = "Organigrama|.ogm";
            save.Title = "Guardar Organigrama";
            if (save.ShowDialog() == DialogResult.OK)
            {
                writer = File.CreateText(save.FileName);
                writer.Write(richTextBox1.Text);
                writer.Close();
                MessageBox.Show("Archivo Guardado", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                FilePath = save.FileName;
            }
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Salir del Programa", "Salir", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                this.Close();
            }
        }

        private void analizarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            le = new ListaError();
            lt = new ListaToken();
            ltr = new ListaTrabajador();
            if (richTextBox1.Text != "")
            {
                panel1.Visible = true;
                progressBar1.Value = 0;
                String FullCode = richTextBox1.Text + " ";
                String lexema = string.Empty;
                String operador = string.Empty;
                String codigo = string.Empty;
                int fila = 1, columna = 0, estado = 0, estadoActual = 1, error = 0;
                int filaToken = 1;
                int columnaToken = 0;
                foreach (char c in FullCode)
                {
                    columna++;
                    switch (estadoActual)
                    {
                        case 1:
                            switch (c)
                            {
                                case 'o':
                                    estadoActual = 2;
                                    filaToken = fila;
                                    columnaToken = columna;
                                    lexema = string.Concat(lexema, c);
                                    break;
                                case 't':
                                    estadoActual = 12;
                                    filaToken = fila;
                                    columnaToken = columna;
                                    lexema = string.Concat(lexema, c);
                                    break;
                                case 'c':
                                    estadoActual = 21;
                                    filaToken = fila;
                                    columnaToken = columna;
                                    lexema = string.Concat(lexema, c);
                                    break;
                                case 'n':
                                    estadoActual = 26;
                                    filaToken = fila;
                                    columnaToken = columna;
                                    lexema = string.Concat(lexema, c);
                                    break;
                                case 's':
                                    estadoActual = 31;
                                    filaToken = fila;
                                    columnaToken = columna;
                                    lexema = string.Concat(lexema, c);
                                    break;
                                case ' ':
                                    if (error == 0)
                                    {
                                        estadoActual = 800;
                                        estado = 1;
                                    }
                                    break;
                                case '\n':
                                case '\r':
                                    fila++;
                                    columna = 0;
                                    if (error == 0)
                                    {
                                        estadoActual = 800;
                                        estado = 1;
                                    }
                                    break;
                                case ':':
                                case '{':
                                case '}':
                                case ';':
                                case ',':
                                    operador = string.Concat(operador, c);
                                    estadoActual = 800;
                                    estado = 1;
                                    filaToken = fila;
                                    columnaToken = columna;
                                    break;
                                case '"':
                                    estadoActual = 40;
                                    filaToken = fila;
                                    columnaToken = columna;
                                    break;
                                case '0':
                                case '1':
                                case '2':
                                case '3':
                                case '4':
                                case '5':
                                case '6':
                                case '7':
                                case '8':
                                case '9':
                                    estadoActual = 41;
                                    filaToken = fila;
                                    columnaToken = columna;
                                    codigo = string.Concat(codigo, c);
                                    break;
                                default:
                                    estadoActual = 801;
                                    estado = 1;
                                    break;
                            }
                            break;
                        case 2:
                            if (c == 'r')
                            {
                                estadoActual = 3;
                            }
                            else
                            {
                                error = 1;
                                estadoActual = 800;
                            }
                            lexema = string.Concat(lexema, c);
                            break;
                        case 3:
                            if (c == 'g')
                            {
                                estadoActual = 4;
                            }
                            else
                            {
                                error = 1;
                                estadoActual = 800;
                            }
                            lexema = string.Concat(lexema, c);
                            break;
                        case 4:
                            if (c == 'a')
                            {
                                estadoActual = 5;
                            }
                            else
                            {
                                error = 1;
                                estadoActual = 800;
                            }
                            lexema = string.Concat(lexema, c);
                            break;
                        case 5:
                            if (c == 'n')
                            {
                                estadoActual = 6;
                            }
                            else
                            {
                                error = 1;
                                estadoActual = 800;
                            }
                            lexema = string.Concat(lexema, c);
                            break;
                        case 6:
                            if (c == 'i')
                            {
                                estadoActual = 7;
                            }
                            else
                            {
                                error = 1;
                                estadoActual = 800;
                            }
                            lexema = string.Concat(lexema, c);
                            break;
                        case 7:
                            if (c == 'g')
                            {
                                estadoActual = 8;
                            }
                            else
                            {
                                error = 1;
                                estadoActual = 800;
                            }
                            lexema = string.Concat(lexema, c);
                            break;
                        case 8:
                            if (c == 'r')
                            {
                                estadoActual = 9;
                            }
                            else
                            {
                                error = 1;
                                estadoActual = 800;
                            }
                            lexema = string.Concat(lexema, c);
                            break;
                        case 9:
                            if (c == 'a')
                            {
                                estadoActual = 10;
                            }
                            else
                            {
                                error = 1;
                                estadoActual = 800;
                            }
                            lexema = string.Concat(lexema, c);
                            break;
                        case 10:
                            if (c == 'm')
                            {
                                estadoActual = 11;
                            }
                            else
                            {
                                error = 1;
                                estadoActual = 800;
                            }
                            lexema = string.Concat(lexema, c);
                            break;
                        case 11:
                            if (c == 'a')
                            {
                                if (error == 0)
                                {
                                    estadoActual = 800;
                                    estado = 1;
                                }
                                else
                                {
                                    estado = 801;
                                }
                            }
                            else
                            {
                                error = 1;
                                estadoActual = 800;
                            }
                            lexema = string.Concat(lexema, c);
                            break;
                        case 12:
                            if (c == 'r')
                            {
                                estadoActual = 13;
                            }
                            else
                            {
                                error = 1;
                                estadoActual = 800;
                            }
                            lexema = string.Concat(lexema, c);
                            break;
                        case 13:
                            if (c == 'a')
                            {
                                estadoActual = 14;
                            }
                            else
                            {
                                error = 1;
                                estadoActual = 800;
                            }
                            lexema = string.Concat(lexema, c);
                            break;
                        case 14:
                            if (c == 'b')
                            {
                                estadoActual = 15;
                            }
                            else
                            {
                                error = 1;
                                estadoActual = 800;
                            }
                            lexema = string.Concat(lexema, c);
                            break;
                        case 15:
                            if (c == 'a')
                            {
                                estadoActual = 16;
                            }
                            else
                            {
                                error = 1;
                                estadoActual = 800;
                            }
                            lexema = string.Concat(lexema, c);
                            break;
                        case 16:
                            if (c == 'j')
                            {
                                estadoActual = 17;
                            }
                            else
                            {
                                error = 1;
                                estadoActual = 800;
                            }
                            lexema = string.Concat(lexema, c);
                            break;
                        case 17:
                            if (c == 'a')
                            {
                                estadoActual = 18;
                            }
                            else
                            {
                                error = 1;
                                estadoActual = 800;
                            }
                            lexema = string.Concat(lexema, c);
                            break;
                        case 18:
                            if (c == 'd')
                            {
                                estadoActual = 19;
                            }
                            else
                            {
                                error = 1;
                                estadoActual = 800;
                            }
                            lexema = string.Concat(lexema, c);
                            break;
                        case 19:
                            if (c == 'o')
                            {
                                estadoActual = 20;
                            }
                            else
                            {
                                error = 1;
                                estadoActual = 800;
                            }
                            lexema = string.Concat(lexema, c);
                            break;
                        case 20:
                            if (c == 'r')
                            {
                                if (error == 0)
                                {
                                    estadoActual = 800;
                                    estado = 1;
                                }
                                else
                                {
                                    estado = 801;
                                }
                            }
                            else
                            {
                                error = 1;
                                estadoActual = 800;
                            }
                            lexema = string.Concat(lexema, c);
                            break;
                        case 21:
                            if (c == 'o')
                            {
                                estadoActual = 22;
                            }
                            else
                            {
                                error = 1;
                                estadoActual = 800;
                            }
                            lexema = string.Concat(lexema, c);
                            break;
                        case 22:
                            if (c == 'd')
                            {
                                estadoActual = 23;
                            }
                            else
                            {
                                error = 1;
                                estadoActual = 800;
                            }
                            lexema = string.Concat(lexema, c);
                            break;
                        case 23:
                            if (c == 'i')
                            {
                                estadoActual = 24;
                            }
                            else
                            {
                                error = 1;
                                estadoActual = 800;
                            }
                            lexema = string.Concat(lexema, c);
                            break;
                        case 24:
                            if (c == 'g')
                            {
                                estadoActual = 25;
                            }
                            else
                            {
                                error = 1;
                                estadoActual = 800;
                            }
                            lexema = string.Concat(lexema, c);
                            break;
                        case 25:
                            if (c == 'o')
                            {
                                if (error == 0)
                                {
                                    estadoActual = 800;
                                    estado = 1;
                                }
                                else
                                {
                                    estado = 801;
                                }
                            }
                            else
                            {
                                error = 1;
                                estadoActual = 800;
                            }
                            lexema = string.Concat(lexema, c);
                            break;
                        case 26:
                            if (c == 'o')
                            {
                                estadoActual = 27;
                            }
                            else
                            {
                                error = 1;
                                estadoActual = 800;
                            }
                            lexema = string.Concat(lexema, c);
                            break;
                        case 27:
                            if (c == 'm')
                            {
                                estadoActual = 28;
                            }
                            else
                            {
                                error = 1;
                                estadoActual = 800;
                            }
                            lexema = string.Concat(lexema, c);
                            break;
                        case 28:
                            if (c == 'b')
                            {
                                estadoActual = 29;
                            }
                            else
                            {
                                error = 1;
                                estadoActual = 800;
                            }
                            lexema = string.Concat(lexema, c);
                            break;
                        case 29:
                            if (c == 'r')
                            {
                                estadoActual = 30;
                            }
                            else
                            {
                                error = 1;
                                estadoActual = 800;
                            }
                            lexema = string.Concat(lexema, c);
                            break;
                        case 30:
                            if (c == 'e')
                            {
                                if (error == 0)
                                {
                                    estadoActual = 800;
                                    estado = 1;
                                }
                                else
                                {
                                    estado = 801;
                                }
                            }
                            else
                            {
                                error = 1;
                                estadoActual = 800;
                            }
                            lexema = string.Concat(lexema, c);
                            break;
                        case 31:
                            if (c == 'u')
                            {
                                estadoActual = 32;
                            }
                            else
                            {
                                error = 1;
                                estadoActual = 800;
                            }
                            lexema = string.Concat(lexema, c);
                            break;
                        case 32:
                            if (c == 'p')
                            {
                                estadoActual = 33;
                            }
                            else
                            {
                                error = 1;
                                estadoActual = 800;
                            }
                            lexema = string.Concat(lexema, c);
                            break;
                        case 33:
                            if (c == 'e')
                            {
                                estadoActual = 34;
                            }
                            else
                            {
                                error = 1;
                                estadoActual = 800;
                            }
                            lexema = string.Concat(lexema, c);
                            break;
                        case 34:
                            if (c == 'r')
                            {
                                estadoActual = 35;
                            }
                            else
                            {
                                error = 1;
                                estadoActual = 800;
                            }
                            lexema = string.Concat(lexema, c);
                            break;
                        case 35:
                            if (c == 'i')
                            {
                                estadoActual = 36;
                            }
                            else
                            {
                                error = 1;
                                estadoActual = 800;
                            }
                            lexema = string.Concat(lexema, c);
                            break;
                        case 36:
                            if (c == 'o')
                            {
                                estadoActual = 37;
                            }
                            else
                            {
                                error = 1;
                                estadoActual = 800;
                            }
                            lexema = string.Concat(lexema, c);
                            break;
                        case 37:
                            if (c == 'r')
                            {
                                estadoActual = 38;
                            }
                            else
                            {
                                estadoActual = 800;
                            }
                            lexema = string.Concat(lexema, c);
                            break;
                        case 38:
                            if (c == 'e')
                            {
                                estadoActual = 39;
                            }
                            else
                            {
                                error = 1;
                                estadoActual = 800;
                            }
                            lexema = string.Concat(lexema, c);
                            break;
                        case 39:
                            if (c == 's')
                            {
                                if (error == 0)
                                {
                                    estadoActual = 800;
                                    estado = 1;
                                }
                                else
                                {
                                    estado = 801;
                                }
                            }
                            else
                            {
                                error = 1;
                                estadoActual = 800;
                            }
                            lexema = string.Concat(lexema, c);
                            break;
                        case 40:
                            if (c.Equals('"'))
                            {
                                estadoActual = 803;
                            }
                            else
                            {
                                    lexema = string.Concat(lexema, c);
                            }
                            break;
                        case 41:
                            if (char.IsDigit(c))
                            {
                                codigo = string.Concat(codigo, c);
                                estadoActual = 41;
                            }
                            else if (c == ',' || c == ';' || c == '}')
                            {
                                operador = string.Concat(operador, c);
                                estadoActual = 800;
                                estado = 1;
                            }
                            else
                            {
                                estadoActual = 800;
                                estado = 1;
                            }
                            break;
                    }
                    switch (estadoActual)
                    {
                        case 800:
                            if (estado == 1)
                            {
                                if (lexema.Length > 0)
                                {
                                    lt.Agregar("reservada", lexema, filaToken, columnaToken);
                                }
                                if (codigo.Length > 0)
                                {
                                    lt.Agregar("codigo", codigo, filaToken, columnaToken);
                                }
                                if (operador.Length > 0)
                                {
                                    lt.Agregar("operador", operador, filaToken, columnaToken);
                                }
                                errores[0] = 8000000;
                                errores[1] = 8000000;
                            }
                            else
                            {
                                if (errores[0] == 8000000)
                                {
                                    errores[0] = fila;
                                    errores[1] = columna - 1;
                                    le.Agregar(errores[0], errores[1]);
                                }
                            }
                            codigo = string.Empty;
                            operador = string.Empty;
                            lexema = string.Empty;
                            estado = 0;
                            estadoActual = 1;
                            error = 0;
                            break;
                        case 801:
                            if (errores[0] == 8000000)
                            {
                                errores[0] = fila;
                                errores[1] = columna - 1;
                                le.Agregar(errores[0], errores[1]);
                            }
                            lexema = string.Empty;
                            operador = string.Empty;
                            codigo = string.Empty;
                            estado = 0;
                            estadoActual = 1;
                            error = 0;
                            break;
                        case 802:
                            lt.Agregar("operador", operador, filaToken, columnaToken);
                            break;
                        case 803:
                            lt.Agregar("cadena", lexema, filaToken, columnaToken);
                            operador = string.Empty;
                            lexema = string.Empty;
                            estado = 0;
                            estadoActual = 1;
                            error = 0;
                            break;
                    }
                }
                int contar = lt.Contar();
                progressBar1.Maximum = contar;
                progressBar1.Step = 1;
                NodoToken token = lt.primero;
                while (token != null)
                {
                    if (token.GetToken().GetToken().Equals("codigo") || token.GetToken().GetToken().Equals("cadena"))
                    {
                        String cadena = token.GetToken().GetCadena();
                        String tipo = token.GetToken().GetToken();
                        Colorear(cadena, tipo);
                    }
                    token = token.siguiente;
                }
                Colorear();
                richTextBox1.DeselectAll();
                richTextBox1.Focus();
                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                richTextBox1.SelectionColor = Color.Black;
                int numErrores = le.Contar();
                panel1.Visible = false;
                if (numErrores != 0)
                {
                    CrearTablaError();
                }
                else
                {
                    BloqueTrabajador();
                    CrearDot();
                }
                CrearTablaToken();
            }
            else
            {
                MessageBox.Show("Ingrese Codigo", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void acercaDeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            informacion inf = new informacion();
            inf.Show();
        }

        public void Colorear()
        {
            for (int i = 0; i < reservadas.Length; i++)
            {
                int index = 0;
                while (index <= richTextBox1.Text.LastIndexOf(reservadas[i]))
                {
                    richTextBox1.Find(reservadas[i], index, richTextBox1.TextLength, RichTextBoxFinds.None);
                    richTextBox1.SelectionColor = Color.Blue;
                    index = richTextBox1.Text.IndexOf(reservadas[i], index) + 1;
                }
                progressBar1.PerformStep();
            }
            for (int i = 0; i < operadores.Length; i++)
            {
                int index = 0;
                while (index <= richTextBox1.Text.LastIndexOf(operadores[i]))
                {
                    richTextBox1.Find(operadores[i], index, richTextBox1.TextLength, RichTextBoxFinds.None);
                    switch (operadores[i])
                    {
                        case ";":
                            richTextBox1.SelectionColor = Color.Red;
                            break;
                        case ":":
                            richTextBox1.SelectionColor = Color.Purple;
                            break;
                        case ",":
                            richTextBox1.SelectionColor = Color.SkyBlue;
                            break;
                        case "{":
                        case "}":
                            richTextBox1.SelectionColor = Color.DeepPink;
                            break;
                    }
                    index = richTextBox1.Text.IndexOf(operadores[i], index) + 1;
                    progressBar1.PerformStep();
                }
            }
        }

        public void Colorear(String cadena, String tipo)
        {
            switch (tipo)
            {
                case "codigo":
                    int index = 0;
                    while (index <= richTextBox1.Text.LastIndexOf(cadena))
                    {
                        richTextBox1.Find(cadena, index, richTextBox1.TextLength, RichTextBoxFinds.None);
                        if (richTextBox1.SelectionColor == Color.Green)
                        {

                        }
                        else
                        {
                            richTextBox1.SelectionColor = Color.Yellow;
                        }
                        progressBar1.PerformStep();
                        index = richTextBox1.Text.IndexOf(cadena, index) + 1;
                    }
                    break;
                case "cadena":
                    int index1 = 0;
                    String[] cadenas = cadena.Split('\n');
                    for (int i = 0; i < cadenas.Length; i++)
                    {
                        while (index1 <= richTextBox1.Text.LastIndexOf(cadenas[i]))
                        {
                            richTextBox1.Find(cadenas[i], index1, richTextBox1.TextLength, RichTextBoxFinds.None);
                            richTextBox1.SelectionColor = Color.Green;
                            progressBar1.PerformStep();
                            index1 = richTextBox1.Text.IndexOf(cadenas[i], index1) + 1;
                        }
                    }
                    break;
            }
        }

        public void BloqueTrabajador()
        {
            NodoToken token = lt.primero;
            int estadoActual = 1;
            String codigos = string.Empty;
            String nombre = string.Empty;
            String codigo = string.Empty;
            while (token != null)
            {
                String tipo = token.GetToken().GetToken();
                String cadena = token.GetToken().GetCadena();
                switch (estadoActual)
                {
                    case 1:
                        if (tipo.Equals("reservada") && cadena.Equals("codigo"))
                        {
                            estadoActual = 3;
                        }
                        if (tipo.Equals("reservada") && cadena.Equals("nombre"))
                        {
                            estadoActual = 4;
                        }
                        if (tipo.Equals("reservada") && cadena.Equals("superiores"))
                        {
                            estadoActual = 5;
                        }
                        if (tipo.Equals("reservada") && cadena.Equals("organigrama"))
                        {
                            estadoActual = 10;
                        }
                        if (tipo.Equals("reservada") && cadena.Equals("trabajador"))
                        {
                            estadoActual = 2;
                        }
                        if(tipo.Equals("operador") && cadena.Equals("}") && !codigo.Equals(""))
                        {
                            estadoActual = 100;
                        }
                        break;
                    case 2:
                        if(tipo.Equals("operador") && cadena.Equals(":"))
                        {
                            estadoActual = 2;
                        }
                        if(tipo.Equals("operador") && cadena.Equals("{"))
                        {
                            estadoActual = 1;
                        }
                        break;
                    case 3:
                        if (tipo.Equals("operador") && cadena.Equals(":"))
                        {
                            estadoActual = 3;
                        }
                        if (tipo.Equals("codigo"))
                        {
                            codigo = cadena;
                            estadoActual = 1;
                        }
                        break;
                    case 4:
                        if (tipo.Equals("operador") && cadena.Equals(":"))
                        {
                            estadoActual = 4;
                        }
                        if (tipo.Equals("cadena"))
                        {
                            nombre = cadena;
                            estadoActual = 1;
                        }
                        break;
                    case 5:
                        if (tipo.Equals("operador") && cadena.Equals(":"))
                        {
                            estadoActual = 5;
                        }
                        if (tipo.Equals("operador") && cadena.Equals("{"))
                        {
                            estadoActual = 5;
                        }
                        if (tipo.Equals("operador") && cadena.Equals("}"))
                        {
                            estadoActual = 1;
                        }
                        if (tipo.Equals("codigo"))
                        {
                            codigos = string.Concat(codigos, cadena);
                            estadoActual = 5;
                        }
                        if (tipo.Equals("operador") && cadena.Equals(","))
                        {
                            estadoActual = 5;
                            codigos = string.Concat(codigos, ",");
                        }
                        break;
                    case 10:
                        if (tipo.Equals("operador") && cadena.Equals(":"))
                        {
                            estadoActual = 11;
                        }
                        break;
                    case 11:
                        if (tipo.Equals("cadena"))
                        {
                            nombreGrafica = cadena;
                        }
                        else
                        {
                            estadoActual = 1;
                        }
                        break;
                }
                switch (estadoActual)
                {
                    case 100:
                        ltr.Agregar(nombre, codigo, codigos);
                        estadoActual = 1;
                        nombre = string.Empty;
                        codigo = string.Empty;
                        codigos = string.Empty;
                        estadoActual = 1;
                        break;
                }
                token = token.siguiente;
            }
        }

        public void CrearTablaError()
        {
            String html1 = "<!DOCTYPE html><html><head><title>Errores</title></head>" +
                "<body><table border = '1'><tr><td><center>No</center></td><td><center>Fila</center></td><td><center>" +
                "Columna</center></td><td><center>Tipo</center></td></tr>";
            String html2 = "</table></body></html>";
            SaveFileDialog save = new SaveFileDialog();
            StreamWriter writer;
            save.Filter = "Tabla Error|.html";
            save.Title = "Guardar Organigrama";
            if (save.ShowDialog() == DialogResult.OK)
            {
                int contador = 1;
                writer = File.CreateText(save.FileName);
                NodoError error = le.primero;
                writer.WriteLine(html1);
                while (error != null)
                {
                    int fila = error.GetError().GetFila();
                    int columna = error.GetError().GetColumna();
                    writer.WriteLine("<tr><td><center>");
                    writer.WriteLine(contador);
                    writer.WriteLine("</center></td><td><center>");
                    writer.WriteLine(fila);
                    writer.WriteLine("</center></td><td><center>");
                    writer.WriteLine(columna);
                    writer.WriteLine("</center></td><td><center>");
                    writer.WriteLine("Caracter Desconocido");
                    writer.WriteLine("</center></td></tr>");
                    error = error.siguiente;
                    contador++;
                }
                writer.WriteLine(html2);
                writer.Close();
                MessageBox.Show("Archivo Guardado", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                FilePath = save.FileName;
                Process.Start(FilePath);
            }
        }

        public void CrearDot()
        {
            SaveFileDialog save = new SaveFileDialog();
            StreamWriter writer;
            save.Filter = "Grafica|.txt";
            save.Title = "Guardar Organigrama";
            if (save.ShowDialog() == DialogResult.OK)
            {
                writer = File.CreateText(save.FileName);
                NodoTrabajador trabajador = ltr.primero;
                writer.WriteLine("digraph structs{");
                writer.WriteLine("rankdir=TB;");
                writer.WriteLine("node [shape=record, style=filled, fillcolor=seashell2];");
                int contador = 0;
                String[,] datos = new String[800, 3];
                while (trabajador != null)
                {
                    String codigo = trabajador.GetTrabajador().GetCodigo().ToString();
                    String nombre = trabajador.GetTrabajador().GetNombre();
                    String superiores = trabajador.GetTrabajador().GetSuperiores();
                    if (superiores.Length > 0)
                    {
                        writer.WriteLine("struct" + contador + " [shape=record, label=\"<" + codigo + ">"+"Cod. "+ codigo + " | " + nombre + "\"]");
                        datos[contador, 2] = superiores;
                        datos[contador, 0] = "struct" + contador;
                        datos[contador, 1] = codigo;
                        contador++;
                    }
                    if(superiores.Length == 0)
                    {
                        writer.WriteLine("struct" + contador + " [shape=record, label=\"<" + codigo + ">" + "Cod. " + codigo + " | " + nombre + "\"]");
                        datos[contador, 0] = "struct" + contador;
                        datos[contador, 1] = codigo;
                        datos[contador, 2] = superiores;
                        contador++;
                    }
                    trabajador = trabajador.siguiente;
                }
                for (int i = 0; i < datos.GetLength(0); i++)
                {
                    if (datos[i, 0] == null)
                    {
                        break;
                    }
                    else
                    {
                        String codigo = datos[i, 1];
                        String structura = datos[i, 0];
                        String codigos = datos[i, 2];
                        for (int j = 0; j < datos.GetLength(0); j++)
                        {
                            if (datos[j, 0] == null)
                            {

                            }
                            else
                            {
                                if (datos[i, 2].Contains(datos[j, 1]))
                                {
                                    String manda = datos[j, 0];
                                    String recibe = datos[i, 0];
                                    writer.WriteLine(manda + "->" + recibe);
                                }
                            }
                        }
                    }
                }
                writer.WriteLine("}");
                writer.Close();
                MessageBox.Show("Archivo Guardado", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                FilePath = save.FileName;
                String directory = Path.GetDirectoryName(FilePath);
                String name = Path.GetFileName(FilePath);
                Process cmd = new Process();
                cmd.StartInfo.FileName = "cmd.exe";
                cmd.StartInfo.RedirectStandardInput = true;
                cmd.StartInfo.RedirectStandardOutput = true;
                cmd.StartInfo.CreateNoWindow = true;
                cmd.StartInfo.UseShellExecute = false;
                cmd.Start();
                cmd.StandardInput.WriteLine("cd " + directory);
                cmd.StandardInput.WriteLine("dot -Tpng " + name + " -o imagen.png");
                cmd.StandardInput.Flush();
                cmd.StandardInput.Close();
                cmd.WaitForExit();
                CrearPagina(FilePath);
            }
        }

        public void CrearTablaToken()
        {
            String html1 = "<!DOCTYPE html><html><head><title>Tokens</title></head>" +
                "<body><table border = '1'><tr><td><center>No</center></td><td><center>Fila</center></td><td><center>" +
                "Columna</center></td><td><center>Tipo</center></td><td><center>Token</center></td></tr>";
            String html2 = "</table></body></html>";
            SaveFileDialog save = new SaveFileDialog();
            StreamWriter writer;
            save.Filter = "Tabla Token|.html";
            save.Title = "Guardar Organigrama";
            if (save.ShowDialog() == DialogResult.OK)
            {
                int contador = 1;
                writer = File.CreateText(save.FileName);
                NodoToken token = lt.primero;
                writer.WriteLine(html1);
                while (token != null)
                {
                    int fila = token.GetToken().GetFila();
                    int columna = token.GetToken().GetColumna();
                    String tipo = token.GetToken().GetToken();
                    String cadena = token.GetToken().GetCadena();
                    writer.WriteLine("<tr><td><center>");
                    writer.WriteLine(contador);
                    writer.WriteLine("</center></td><td><center>");
                    writer.WriteLine(fila);
                    writer.WriteLine("</center></td><td><center>");
                    writer.WriteLine(columna);
                    writer.WriteLine("</center></td><td><center>");
                    writer.WriteLine(tipo);
                    writer.WriteLine("</center></td><td><center>");
                    writer.WriteLine(cadena);
                    writer.WriteLine("</center></td></tr>");
                    token = token.siguiente;
                    contador++;
                }
                writer.WriteLine(html2);
                writer.Close();
                MessageBox.Show("Archivo Guardado", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                FilePath = save.FileName;
                Process.Start(FilePath);
            }
        }

        public void CrearPagina(String path)
        {
            String html1 = "<!DOCTYPE html><html><head>	<title>"+nombreGrafica+"</title></head><body><h3>"+nombreGrafica+"</h3><img src='";
            String html2 = "'></body></html>";
            SaveFileDialog save = new SaveFileDialog();
            StreamWriter writer;
            String newPath = Path.ChangeExtension(path, "html");
            String nombre = Path.GetFileName(newPath);
            String directory = Path.GetDirectoryName(newPath);
            writer = File.CreateText(newPath);
            writer.WriteLine(html1);
            writer.WriteLine(directory + "\\" + "imagen.png");
            writer.WriteLine(html2);
            writer.Close();
            Process.Start(newPath);
        }
    }
}