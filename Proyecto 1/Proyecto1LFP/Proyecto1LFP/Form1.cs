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

        public String[] reservadas = { "playlist", "nombre", "cancion", "url", "artista", "álbum", "duracion", "repeticion", "nveces", "año" };
        public Char[] simbolos = { '<', '>', '=', '/' };
        int esCancion = 0;
        int repetir = 0;
        int duracion = 0;
        int año = 0;
        int nveces = 1;
        int nombre = 1;
        int url = 1;
        String FilePath = "";
        public static ListaError le;
        public static ListaToken lt;
        public static ListaCancion lc;
        public static int numeroPlaylist = 0;
        Regex compararRepeticiones = new Regex(@"^[\d]+$");
        Regex compararDuracion = new Regex(@"^[0-9]?[0-9]:[0-9][0-9]$");
        Regex compararAño = new Regex(@"^[\d]{4}$");


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Playlist |*.plst";
            open.Title = "Seleccionar Playlist";
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
                    save.Filter = "Playlist |*.plst";
                    save.Title = "Seleccionar Playlist";
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
            save.Filter = "Playlist |*.plst";
            save.Title = "Seleccionar Playlist";
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
            ArrayList rep = new ArrayList();
            rep.Add(1);
            numeroPlaylist = 0;
            le = new ListaError();
            lt = new ListaToken();
            lc = new ListaCancion();
            if (richTextBox1.Text != "")
            {
                String FullCode = richTextBox1.Text + " ";
                String lexema = string.Empty;
                String lexemaAux = string.Empty;
                String operador = string.Empty;
                String codigo = string.Empty;
                int fila = 1, columna = 0, estado = 0, error = 0;
                char estadoActual = 'A';
                int filaToken = 1;
                Regex nombreCancion = new Regex(@"[a-z|0-9|\-]*", RegexOptions.IgnoreCase);
                foreach (char c in FullCode)
                {
                    columna++;
                    switch (estadoActual)
                    {
                        case 'A':
                            if (Char.IsLetter(c))
                            {
                                estadoActual = 'B';
                                lexema = string.Concat(lexema, c);
                            }
                            else if (c.Equals('"'))
                            {
                                estadoActual = 'C';
                                lexema = string.Concat(lexema, c);
                            }
                            else if (c.Equals(' '))
                            {
                                estadoActual = 'A';
                            }
                            else if (c.Equals('\n') || c.Equals('\r') || c.Equals('\t'))
                            {
                                fila++;
                                columna = 0;
                                estadoActual = 'A';
                            }
                            else
                            {
                                int contador = 0;
                                for (int i = 0; i < simbolos.Length; i++)
                                {
                                    if (c.Equals(simbolos[i]))
                                    {
                                        contador++;
                                    }
                                }
                                if (contador == 1)
                                {
                                    estadoActual = 'D';
                                    lexema = string.Concat(lexema, c);
                                }
                                else
                                {
                                    estadoActual = 'I';
                                    lexema = string.Concat(lexema, c);
                                }
                            }
                            break;
                        case 'B':
                            if (Char.IsLetter(c) || c.Equals('\t'))
                            {
                                estadoActual = 'B';
                                lexema = string.Concat(lexema, c);
                                String lexema1 = lexema.ToLower();
                                String normalL = lexema1.Normalize(NormalizationForm.FormD);
                                for (int i = 0; i < reservadas.Length; i++)
                                {
                                    String normalR = reservadas[i].Normalize(NormalizationForm.FormD);
                                    Regex reg = new Regex("[^a-zA-Z0-9 ]");
                                    string texto1 = reg.Replace(normalR, "");
                                    string texto2 = reg.Replace(normalL, "");
                                    if (texto1.Equals(texto2))
                                    {
                                        estadoActual = 'G';
                                    }
                                }
                            }
                            else if (c.Equals(' '))
                            {
                                estadoActual = 'G';

                            }
                            else if (c.Equals('\n') || c.Equals('\r'))
                            {
                                fila++;
                                columna = 0;
                                estadoActual = 'G';
                            }
                            else
                            {
                                estadoActual = 'H';
                                lexema = string.Concat(lexema, c);
                            }
                            break;
                        case 'C':
                            estadoActual = 'E';
                            lexema = string.Concat(lexema, c);
                            break;
                        case 'E':
                            if (c.Equals('"'))
                            {
                                estadoActual = 'F';
                                lexema = string.Concat(lexema, c);
                            }
                            else
                            {
                                estadoActual = 'E';
                                lexema = string.Concat(lexema, c);
                            }
                            break;
                        case 'N':
                            if (Char.IsLetterOrDigit(c) || c.Equals('-') || c.Equals(' '))
                            {
                                lexema = string.Concat(lexema, c);
                            }
                            else if (c.Equals('<'))
                            {
                                lexemaAux = string.Concat(lexemaAux, c);
                                estadoActual = 'M';
                            }
                            break;
                    }
                    //Comprueba los estados de Aceptacion G y D
                    //G indica que es una palabra reservada
                    //D indica que es un simbolo
                    //F indica que es una cadena
                    //H indica que es un error
                    int cantidad = 0;
                    int index = 0;
                    switch (estadoActual)
                    {
                        case 'D':
                            //Simbolo
                            int contadorSimbolo = 0;
                            for (int i = 0; i < simbolos.Length; i++)
                            {
                                Char d = ' ';
                                Char.TryParse(lexema, out d);
                                if (d.Equals(simbolos[i]))
                                {
                                    contadorSimbolo++;
                                }
                            }
                            if (contadorSimbolo == 1)
                            {
                                lt.Agregar("simbolo", lexema, fila, columna, nveces);
                            }
                            if (lexema.Equals(">") && esCancion == 1)
                            {
                                estadoActual = 'N';
                            }
                            else
                            {
                                estadoActual = 'A';
                            }
                            lexema = string.Empty;
                            break;

                        case 'F':
                            //Cadena
                            cantidad = lexema.Length;
                            index = columna - cantidad;
                            int agregado = 0;
                            if (año != 0)
                            {
                                String cambio = lexema.Trim(new Char[] { '"', ' ' });
                                if (compararAño.IsMatch(cambio))
                                {
                                    //Es año
                                    lt.Agregar("cadena", lexema, fila, index, nveces);
                                    año = 0;
                                    agregado++;
                                }
                                else
                                {
                                    le.Agregar("Error de Formato", lexema, fila, index);
                                }
                            }
                            if (repetir == 1 && agregado == 0)
                            {
                                String cambio = lexema.Trim(new Char[] { '"', ' ' });
                                if (compararRepeticiones.IsMatch(cambio))
                                {
                                    //Es repeticion
                                    int conteo = 1;
                                    rep.Add(int.Parse(cambio));
                                    foreach (int d in rep)
                                    {
                                        conteo = conteo * d;
                                    }
                                    nveces = conteo;
                                    lt.Agregar("cadena", lexema, fila, index, nveces);
                                    agregado++;
                                }
                                else if (compararAño.IsMatch(cambio))
                                {
                                    //Es año
                                    lt.Agregar("cadena", lexema, fila, index, nveces);
                                    año = 0;
                                    agregado++;
                                }
                                else if (compararDuracion.IsMatch(cambio))
                                {
                                    //Es duracion
                                    lt.Agregar("cadena", lexema, fila, index, nveces);
                                    duracion = 0;
                                    agregado++;
                                }
                                else if (url == 1)
                                {
                                    if (File.Exists(cambio))
                                    {
                                        lt.Agregar("cadena", lexema, fila, index, nveces);
                                        agregado++;
                                    }
                                    else
                                    {
                                        le.Agregar("Cancion Inexistente", lexema, fila, index);
                                    }
                                    url = 0;
                                }
                                else if (nombre == 1)
                                {
                                    lt.Agregar("cadena", lexema, fila, index, nveces);
                                    agregado++;
                                }
                                else
                                {
                                    le.Agregar("Error de Formato", lexema, fila, index);
                                }
                            }
                            if (duracion != 0)
                            {
                                String cambio = lexema.Trim(new Char[] { '"', ' ' });
                                if (compararDuracion.IsMatch(cambio))
                                {
                                    //Es duracion
                                    lt.Agregar("cadena", lexema, fila, index, nveces);
                                    duracion = 0;
                                    agregado++;
                                }
                                else
                                {
                                    le.Agregar("Error de Formato", lexema, fila, index);
                                }
                            }
                            if (agregado == 0)
                            {
                                lt.Agregar("cadena", lexema, fila, index, nveces);
                            }
                            lexema = string.Empty;
                            estadoActual = 'A';
                            break;

                        case 'G':
                            //Reservada
                            cantidad = lexema.Length;
                            index = columna - cantidad;
                            int contadorReservada = 0;
                            String lexema1 = lexema.ToLower();
                            String normalL = lexema1.Normalize(NormalizationForm.FormD);
                            for (int i = 0; i < reservadas.Length; i++)
                            {
                                String normalR = reservadas[i].Normalize(NormalizationForm.FormD);
                                Regex reg = new Regex("[^a-zA-Z0-9 ]");
                                string texto1 = reg.Replace(normalR, "");
                                string texto2 = reg.Replace(normalL, "");
                                if (texto1.Equals(texto2))
                                {
                                    contadorReservada++;
                                    NodoToken token = lt.ultimo;
                                    if (texto1.Equals("playlist") && token.GetToken().GetCadena().Equals("<"))
                                    {
                                        numeroPlaylist++;
                                    }
                                    if (texto1.Equals("cancion") && token.GetToken().GetCadena().Equals("<"))
                                    {
                                        esCancion = 1;
                                    }
                                    if (texto1.Equals("nveces"))
                                    {
                                        repetir = 1;
                                    }
                                    if (texto1.Equals("repeticion") && token.GetToken().GetCadena().Equals("/"))
                                    {
                                        repetir = 0;
                                        rep.RemoveAt(rep.Count - 1);
                                        int conteo = 1;
                                        foreach(int d in rep)
                                        {
                                            conteo = conteo * d;
                                        }
                                        nveces = conteo;
                                    }
                                    if (texto1.Equals("ano"))
                                    {
                                        año = 1;
                                    }
                                    if (texto1.Equals("duracion"))
                                    {
                                        duracion = 1;
                                    }
                                    if (texto1.Equals("nombre"))
                                    {
                                        nombre = 1;
                                    }
                                    if (texto1.Equals("url"))
                                    {
                                        url = 1;
                                    }
                                }

                            }
                            if (contadorReservada == 1)
                            {
                                lt.Agregar("reservada", lexema, fila, columna, nveces);
                            }
                            else
                            {
                                le.Agregar("Error Lexico", lexema, fila, columna);
                            }
                            estadoActual = 'A';
                            lexema = string.Empty;
                            lexemaAux = string.Empty;
                            break;

                        case 'H':
                            //Error
                            cantidad = lexema.Length;
                            index = columna - cantidad;
                            le.Agregar("Caracter no Encontrado", lexema, fila, index);
                            lexema = string.Empty;
                            estadoActual = 'A';
                            break;

                        case 'M':
                            lt.Agregar("nombre", lexema, fila, columna, nveces);
                            lt.Agregar("simbolo", lexemaAux, fila, columna, nveces);
                            lexema = string.Empty;
                            lexemaAux = string.Empty;
                            estadoActual = 'A';
                            esCancion = 0;
                            break;
                    }

                }
                if (lt.primero != null)
                {
                    tokenToolStripMenuItem.Enabled = true;
                }
                else
                {
                    tokenToolStripMenuItem.Enabled = false;
                }
                if (le.primero != null)
                {
                    errorToolStripMenuItem.Enabled = true;
                }
                else
                {
                    errorToolStripMenuItem.Enabled = false;
                }
                Colorear();
                richTextBox1.DeselectAll();
                richTextBox1.Focus();
                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                richTextBox1.SelectionColor = Color.Black;
                AgruparCanciones();
                if (le.primero == null)
                {
                    Reproductor rp = new Reproductor();
                    rp.Visible = true;
                }
            }
            else
            {
                MessageBox.Show("Ingrese Codigo", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tokenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TablaTokens tok = new TablaTokens();
            tok.Show();
        }

        void Colorear()
        {
            for (int i = 0; i < reservadas.Length; i++)
            {
                Regex reg = new Regex(reservadas[i], RegexOptions.IgnoreCase);
                foreach (Match find in reg.Matches(richTextBox1.Text))
                {
                    richTextBox1.Select(find.Index, find.Length);
                    richTextBox1.SelectionColor = Color.Purple;
                }
            }
            NodoToken token = lt.primero;
            while (token != null)
            {
                int index = 0;
                if (token.GetToken().GetToken().Equals("cadena"))
                {
                    while (index <= richTextBox1.Text.LastIndexOf(token.GetToken().GetCadena()))
                    {
                        richTextBox1.Find(token.GetToken().GetCadena(), index, richTextBox1.TextLength, RichTextBoxFinds.None);
                        richTextBox1.SelectionColor = Color.LightBlue;
                        index = richTextBox1.Text.IndexOf(token.GetToken().GetCadena(), index) + 1;
                    }
                }
                token = token.siguiente;
            }
            for (int i = 0; i < simbolos.Length; i++)
            {
                int index = 0;
                while (index <= richTextBox1.Text.LastIndexOf(simbolos[i]))
                {
                    richTextBox1.Find(simbolos[i].ToString(), index, richTextBox1.TextLength, RichTextBoxFinds.None);
                    switch (simbolos[i])
                    {
                        case '=':
                            richTextBox1.SelectionColor = Color.Yellow;
                            break;
                        case '<':
                            richTextBox1.SelectionColor = Color.Red;
                            break;
                        case '>':
                            richTextBox1.SelectionColor = Color.Green;
                            break;
                        case '/':
                            richTextBox1.SelectionColor = Color.Blue;
                            break;
                    }
                    index = richTextBox1.Text.IndexOf(simbolos[i], index) + 1;
                }
            }
        }

        void AgruparCanciones()
        {
            NodoToken token = lt.primero;
            String nombrePlaylist = "";
            String nombreCancion = "";
            String duracionCancion = "";
            String artistaCancion = "";
            String añoCancion = "";
            String albumCancion = "";
            String urlCancion = "";
            int conteo = 0;
            while (token != null)
            {
                String tipo = token.GetToken().GetCadena().ToLower();
                switch (tipo)
                {
                    case "nombre":
                        nombrePlaylist = token.GetSiguiente().GetSiguiente().GetToken().GetCadena().Trim(new Char[] { '"', ' ' });
                        break;
                    case "url":
                        urlCancion = token.GetSiguiente().GetSiguiente().GetToken().GetCadena().Trim(new Char[] { '"', ' ' });
                        conteo++;
                        break;
                    case "artista":
                        artistaCancion = token.GetSiguiente().GetSiguiente().GetToken().GetCadena().Trim(new Char[] { '"', ' ' });
                        conteo++;
                        break;
                    case "album":
                    case "álbum":
                        albumCancion = token.GetSiguiente().GetSiguiente().GetToken().GetCadena().Trim(new Char[] { '"', ' ' });
                        conteo++;
                        break;
                    case "año":
                        añoCancion = token.GetSiguiente().GetSiguiente().GetToken().GetCadena().Trim(new Char[] { '"', ' ' });
                        conteo++;
                        break;
                    case "duracion":
                        duracionCancion = token.GetSiguiente().GetSiguiente().GetToken().GetCadena().Trim(new Char[] { '"', ' ' });
                        conteo++;
                        break;

                }
                if (token.GetToken().GetToken().Equals("nombre"))
                {
                    nombreCancion = token.GetToken().GetCadena();
                    conteo++;
                }
                if (conteo == 6)
                {
                    int repeticiones = token.GetToken().GetRepeticion();
                    for (int i = 0; i < repeticiones; i++)
                    {
                        lc.Agregar(duracionCancion, nombreCancion, añoCancion, urlCancion, albumCancion, artistaCancion, nombrePlaylist);
                    }
                    conteo = 0;
                }
                token = token.siguiente;
            }
        }

        private void errorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TablaErrores tok = new TablaErrores();
            tok.Show();
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
