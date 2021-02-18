using Microsoft.JScript;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LFPproyecto2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Dictionary<String, int> diccionario = new Dictionary<String, int>();

        String FilePath = "";

        int preanalisis = 0;
        int puntero = 0;

        String[] reservadas = { "principal", "intervalo", "nivel", "enemigo", "personaje", "dimensiones", "inicio_personaje", "ubicacion_salida", "pared", "casilla", "varias_casillas", "caminata", "paso", "variable" };
        char[] simbolos = { '[', ']', '(', ')', '*', '/', '+', '-', '{', '}', ':', ';', '=', ',', '.' };
        int[] primPn = { 6, 7, 8, 9 };
        int[] primP = { 2, 3, 4, 5 };
        int[] primPPa = { 15, 17 };
        int[] primPPa_1 = { 10, 11, 14 };
        int[] primOper = { 15, 16 };
        int[] primVar = { 30, 31 };
        int[] primVar_2 = { 30 };
        int[] primVar_1 = { 31 };
        int[] primPe = { 12 };
        int[] primPPe = { 15, 17 };
        int[] primPPe_1 = { 12, 13, 14 };
        int[] primK = { 15, 16 };

        ArrayList tokens;
        ArrayList erroresLexicos;
        ArrayList erroresSintacticos;
        public static ArrayList valoresTablero;

        String casillaX_inicio = string.Empty;
        String casillaX_fin = string.Empty;
        String casillaY_inicio = string.Empty;
        String casillaY_fin = string.Empty;

        bool esPared = false;
        bool esPersonaje = false;

        int enemigos = 0;

        String izquierda = string.Empty;
        String termino1 = string.Empty;
        String termino2 = string.Empty;
        String operador = string.Empty;
        int termino = 0;

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
            tokens = new ArrayList();
            erroresLexicos = new ArrayList();
            erroresSintacticos = new ArrayList();
            valoresTablero = new ArrayList();
            richTextBox1.Text = richTextBox1.Text.ToLower();
            analizadorLexico();
            ArrayList arrayTemp = tokens.GetRange(puntero, 1);
            foreach (String[] v in arrayTemp)
            {
                preanalisis = System.Convert.ToInt32(v[1]);
            }
            colorearTexto();
            analizadorSintactico();
            generarTablas();
            if (erroresLexicos.Count != 0 || erroresSintacticos.Count != 0)
            {

            }
            else
            {
                Tablero tab = new Tablero();
                tab.Visible = true;
            }
        }

        private void analizadorLexico()
        {
            String texto = richTextBox1.Text;
            char estadoActual = 'A';
            int columna = 0;
            int fila = 0;
            int cambioFila = 0;
            int esError = 0;
            String lexema = string.Empty;
            String lexema2 = string.Empty;
            //Cadena | Tipo | Fila | Columna
            foreach (char c in texto)
            {
                switch (estadoActual)
                {
                    case 'A':
                        if (char.IsLetter(c))
                        {
                            estadoActual = 'B';
                            lexema = string.Concat(lexema, c);
                            columna++;
                        }
                        else if (char.IsDigit(c))
                        {
                            estadoActual = 'C';
                            lexema = string.Concat(lexema, c);
                            columna++;
                        }
                        else if (simbolos.Contains(c))
                        {
                            estadoActual = 'D';
                            lexema = string.Concat(lexema, c);
                            columna++;
                        }
                        else if (c.Equals('\n') || c.Equals('\r'))
                        {
                            fila++;
                            columna = 0;
                        }
                        else if (c.Equals('\t') || c.Equals(' '))
                        {
                            columna++;
                        }
                        else
                        {
                            estadoActual = 'J';
                            esError = 1;
                            lexema = string.Concat(lexema, c);
                            columna++;
                        }
                        break;
                    case 'B':
                        if (char.IsLetterOrDigit(c) || c.Equals('_'))
                        {
                            estadoActual = 'E';
                            lexema = string.Concat(lexema, c);
                            columna++;
                        }
                        else if (c.Equals('\n') || c.Equals('\r'))
                        {
                            estadoActual = 'F';
                            cambioFila = 1;
                        }
                        else if (c.Equals('\t') || c.Equals(' '))
                        {
                            estadoActual = 'F';
                            columna++;
                        }
                        else if (simbolos.Contains(c))
                        {
                            estadoActual = 'G';
                            lexema2 = string.Concat(lexema2, c);
                            columna++;
                        }
                        else
                        {
                            esError = 1;
                            lexema = string.Concat(lexema, c);
                            columna++;
                        }
                        break;
                    case 'C':
                        if (char.IsDigit(c))
                        {
                            estadoActual = 'C';
                            lexema = string.Concat(lexema, c);
                            columna++;
                        }
                        else if (c.Equals('\n') || c.Equals('\r'))
                        {
                            estadoActual = 'H';
                            cambioFila = 1;
                        }
                        else if (c.Equals('\t') || c.Equals(' '))
                        {
                            estadoActual = 'H';
                            columna++;
                        }
                        else if (simbolos.Contains(c))
                        {
                            estadoActual = 'I';
                            lexema2 = string.Concat(lexema2, c);
                            columna++;
                        }
                        else
                        {
                            esError = 1;
                            lexema = string.Concat(lexema, c);
                            columna++;
                        }
                        break;
                    case 'E':
                        if (char.IsLetterOrDigit(c) || c.Equals('_'))
                        {
                            estadoActual = 'E';
                            lexema = string.Concat(lexema, c);
                            columna++;
                        }
                        else if (c.Equals('\n') || c.Equals('\r'))
                        {
                            estadoActual = 'F';
                            cambioFila = 1;
                        }
                        else if (c.Equals('\t') || c.Equals(' '))
                        {
                            estadoActual = 'F';
                            columna++;
                            lexema = string.Concat(lexema, c);
                        }
                        else if (simbolos.Contains(c))
                        {
                            estadoActual = 'G';
                            lexema2 = string.Concat(lexema2, c);
                            columna++;
                        }
                        else
                        {
                            esError = 1;
                            lexema = string.Concat(lexema, c);
                            columna++;
                        }
                        break;
                    case 'J':
                        columna++;
                        String palabraF = lexema.ToLower().Trim();
                        int indexF = columna - lexema.Length;
                        String sIndexF = System.Convert.ToString(indexF);
                        String sFilaJ = System.Convert.ToString(fila);
                        if (c.Equals('\n') || c.Equals('\r'))
                        {
                            String[] errorF = { lexema.Trim(), "Léxico", sIndexF, sFilaJ };
                            erroresLexicos.Add(errorF);
                            esError = 0;
                            fila++;
                            columna = 0;
                            estadoActual = 'A';
                            lexema = string.Empty;
                        }
                        else if (c.Equals('\t') || c.Equals(' '))
                        {
                            String[] errorF = { lexema.Trim(), "Léxico", sIndexF, sFilaJ };
                            erroresLexicos.Add(errorF);
                            esError = 0;
                            estadoActual = 'A';
                            lexema = string.Empty;
                        }
                        else
                        {
                            lexema = string.Concat(lexema, c);
                        }
                        break;
                    default:
                        esError = 1;
                        break;
                }
                String sFila = System.Convert.ToString(fila);
                String sColumna = System.Convert.ToString(columna - 1);
                switch (estadoActual)
                {
                    case 'D':
                        if (esError == 0)
                        {
                            int numero1 = buscarSimbolo(lexema.ElementAt(0)) + 17;
                            if (numero1 == 21 || numero1 == 22 || numero1 == 23 || numero1 == 24)
                            {
                                numero1 = 21;
                            }
                            String[] tokenD = { lexema, numero1.ToString(), sColumna, sFila };
                            tokens.Add(tokenD);
                            lexema = string.Empty;
                            estadoActual = 'A';
                        }
                        else
                        {
                            String[] errorD = { lexema, "Léxico", sColumna, sFila };
                            erroresLexicos.Add(errorD);
                            esError = 0;
                        }
                        break;
                    case 'F':
                        String palabraF = lexema.ToLower().Trim();
                        int indexF = columna - lexema.Length;
                        String sIndexF = System.Convert.ToString(indexF);
                        if (esError == 0)
                        {
                            if (reservadas.Contains(palabraF))
                            {
                                int numero1 = buscarReservada(palabraF) + 1;
                                String num = System.Convert.ToString(numero1);
                                String[] tokenF = { lexema.Trim(), num, sIndexF, sFila };
                                tokens.Add(tokenF);
                            }
                            else
                            {
                                String[] tokenF = { lexema.Trim(), "15", sIndexF, sFila };
                                tokens.Add(tokenF);
                            }
                        }
                        else
                        {
                            String[] errorF = { lexema.Trim(), "Léxico", sIndexF, sFila };
                            erroresLexicos.Add(errorF);
                            esError = 0;
                        }
                        lexema = string.Empty;
                        estadoActual = 'A';
                        if (cambioFila == 1)
                        {
                            columna = 0;
                            cambioFila = 0;
                            fila++;
                        }
                        break;
                    case 'G':
                        String palabraG = lexema.ToLower();
                        int indexG = columna - lexema.Length - 1;
                        String sIndexG = System.Convert.ToString(indexG);
                        if (esError == 0)
                        {
                            if (reservadas.Contains(palabraG))
                            {
                                int numero1 = buscarReservada(palabraG) + 1;
                                String num = System.Convert.ToString(numero1);
                                String[] tokenG = { lexema.Trim(), num, sIndexG, sFila };
                                tokens.Add(tokenG);
                            }
                            else
                            {
                                String[] tokenG = { lexema.Trim(), "15", sIndexG, sFila };
                                tokens.Add(tokenG);
                            }
                        }
                        else
                        {
                            String[] errorG = { lexema.Trim(), "Léxico", sIndexG, sFila };
                            erroresLexicos.Add(errorG);
                            esError = 0;
                        }
                        int numero = buscarSimbolo(lexema2.ElementAt(0)) + 17;
                        if (numero == 21 || numero == 22 || numero == 23 || numero == 24)
                        {
                            numero = 21;
                        }
                        String[] tokenG1 = { lexema2, numero.ToString(), sColumna, sFila };
                        tokens.Add(tokenG1);
                        lexema = string.Empty;
                        lexema2 = string.Empty;
                        estadoActual = 'A';
                        break;
                    case 'H':
                        String palabraH = lexema;
                        int indexH = columna - lexema.Length - 1;
                        String sIndexH = System.Convert.ToString(indexH);
                        if (esError == 0)
                        {
                            String[] tokenH = { palabraH, "16", sIndexH, sFila };
                            tokens.Add(tokenH);
                        }
                        else
                        {
                            String[] errorH = { lexema.Trim(), "Léxico", sIndexH, sFila };
                            erroresLexicos.Add(errorH);
                            esError = 0;
                        }
                        lexema = string.Empty;
                        estadoActual = 'A';
                        if (cambioFila == 1)
                        {
                            columna = 0;
                            cambioFila = 0;
                            fila++;
                        }
                        break;
                    case 'I':
                        String palabraI = lexema;
                        int indexI = columna - lexema.Length - 1;
                        String sIndexI = System.Convert.ToString(indexI);
                        if (esError == 0)
                        {
                            String[] tokenH = { palabraI, "16", sIndexI, sFila };
                            tokens.Add(tokenH);
                        }
                        else
                        {
                            String[] errorH = { lexema.Trim(), "Léxico", sIndexI, sFila };
                            erroresLexicos.Add(errorH);
                            esError = 0;
                        }
                        int numero2 = buscarSimbolo(lexema2.ElementAt(0)) + 17;
                        if (numero2 == 21 || numero2 == 22 || numero2 == 23 || numero2 == 24)
                        {
                            numero2 = 21;
                        }
                        String[] tokenI1 = { lexema2, numero2.ToString(), sColumna, sFila };
                        tokens.Add(tokenI1);
                        lexema = string.Empty;
                        lexema2 = string.Empty;
                        estadoActual = 'A';
                        break;
                }
            }
            switch (estadoActual)
            {
                case 'E':
                case 'B':
                    String sFila = System.Convert.ToString(fila);
                    String sColumna = System.Convert.ToString(columna);
                    String palabra = lexema.ToLower().Trim();
                    int index = columna - lexema.Length;
                    String sIndex = System.Convert.ToString(index);
                    if (esError == 0)
                    {
                        if (reservadas.Contains(palabra))
                        {
                            int numero1 = buscarReservada(lexema.Trim()) + 1;
                            String num = System.Convert.ToString(numero1);
                            String[] tokenF = { lexema, num, sIndex, sFila };
                            tokens.Add(tokenF);
                        }
                        else
                        {
                            String[] tokenF = { lexema, "15", sIndex, sFila };
                            tokens.Add(tokenF);
                        }
                    }
                    else
                    {
                        String[] errorH = { lexema.Trim(), "Léxico", sIndex, sFila };
                        erroresLexicos.Add(errorH);
                        esError = 0;
                    }
                    lexema = string.Empty;
                    estadoActual = 'A';
                    if (cambioFila == 1)
                    {
                        columna = 0;
                        cambioFila = 1;
                        fila++;
                    }
                    break;
                case 'C':
                    String sFilaC = System.Convert.ToString(fila);
                    String sColumnaC = System.Convert.ToString(columna);
                    int indexC = columna - lexema.Length;
                    String sIndexC = System.Convert.ToString(indexC);
                    String[] tokenC = { lexema, "16", sIndexC, sFilaC };
                    tokens.Add(tokenC);
                    lexema = string.Empty;
                    estadoActual = 'A';
                    if (cambioFila == 1)
                    {
                        columna = 0;
                        cambioFila = 1;
                        fila++;
                    }
                    break;
                case 'J':
                    String sFilaA = System.Convert.ToString(fila);
                    String sColumnaA = System.Convert.ToString(columna);
                    int indexA = columna - lexema.Length;
                    String sIndexA = System.Convert.ToString(indexA);
                    if (!string.IsNullOrEmpty(lexema))
                    {
                        String[] errorH = { lexema.Trim(), "Léxico", sIndexA, sFilaA };
                        erroresLexicos.Add(errorH);
                        esError = 0;
                    }
                    break;
            }
        }

        private void colorearTexto()
        {
            String[] numeros = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
            for (int i = 0; i < numeros.Length; i++)
            {
                int index = 0;
                while (index <= richTextBox1.Text.LastIndexOf(numeros[i]))
                {
                    richTextBox1.Find(numeros[i], index, richTextBox1.TextLength, RichTextBoxFinds.None);
                    richTextBox1.SelectionColor = Color.Red;
                    index = richTextBox1.Text.IndexOf(numeros[i], index) + 1;
                }
            }
            foreach (String[] v in tokens)
            {
                int[] rango = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
                if (rango.Contains(System.Convert.ToInt32(v[1])))
                {
                    int index = 0;
                    while (index <= richTextBox1.Text.LastIndexOf(v[0]))
                    {
                        richTextBox1.Find(v[0], index, richTextBox1.TextLength, RichTextBoxFinds.None);
                        richTextBox1.SelectionColor = Color.Blue;
                        index = richTextBox1.Text.IndexOf(v[0], index) + 1;
                    }
                }
            }
            foreach (String[] v in tokens)
            {
                if (v[1] == "15")
                {
                    int index = 0;
                    while (index <= richTextBox1.Text.LastIndexOf(v[0]))
                    {
                        richTextBox1.Find(v[0], index, richTextBox1.TextLength, RichTextBoxFinds.None);
                        if (richTextBox1.SelectionColor == Color.White)
                        {
                            richTextBox1.SelectionColor = Color.Green;
                        }
                        index = richTextBox1.Text.IndexOf(v[0], index) + 1;
                    }
                }
            }
            for (int i = 0; i < simbolos.Length; i++)
            {
                int index = 0;
                while (index <= richTextBox1.Text.LastIndexOf(simbolos[i]))
                {
                    richTextBox1.Find(simbolos[i].ToString(), index, richTextBox1.TextLength, RichTextBoxFinds.None);
                    switch (simbolos[i])
                    {
                        case ';':
                        case '.':
                        case ',':
                            richTextBox1.SelectionColor = Color.Purple;
                            break;
                        case ':':
                            richTextBox1.SelectionColor = Color.Yellow;
                            break;
                        case '{':
                        case '}':
                            richTextBox1.SelectionColor = Color.DeepPink;
                            break;
                        case '[':
                        case ']':
                            richTextBox1.SelectionColor = Color.SkyBlue;
                            break;
                        case '+':
                        case '-':
                        case '/':
                        case '*':
                        case '=':
                            richTextBox1.SelectionColor = Color.Orange;
                            break;
                        case '(':
                        case ')':
                            richTextBox1.SelectionColor = Color.Brown;
                            break;
                    }
                    index = richTextBox1.Text.IndexOf(simbolos[i], index) + 1;
                }
            }
        }

        private void analizadorSintactico()
        {
            S();
            String[] dato = { "cantidadEnemigos", enemigos.ToString() }; 
            valoresTablero.Add(dato);
        }

        public void S()
        {
            Parea(17);//[
            Parea(1);//principal
            Parea(18);//]
            Parea(27);//:
            Parea(25);//{
            if (preanalisis == 17)
            {
                Cuerpo();
            }
            Parea(26);//}
        }

        public void Cuerpo()
        {
            Parea(17);//[
            if (primP.Contains(preanalisis))
            {
                P();
            }
            if (preanalisis == 17)
            {
                Cuerpo();
            }
        }

        public void P()
        {
            switch (preanalisis)
            {
                case 2:
                    Parea(2);//intervalo
                    Parea(18);//]
                    Parea(27);//:
                    Parea(19);//(
                    if (preanalisis == 16)
                    {
                        String valor = string.Empty;
                        foreach (String[] v in buscarToken())
                        {
                            valor = v[0];
                        }
                        String[] intervalo = { "intervalo", valor };
                        valoresTablero.Add(intervalo);
                    }
                    Parea(16);//num
                    Parea(20);//)
                    Parea(28);//;
                    break;
                case 3:
                    Parea(3);//nivel
                    Parea(18);//]
                    Parea(27);//:
                    Parea(25);//{
                    if (preanalisis == 17)
                    {
                        Parea(17);//[
                        if (primPn.Contains(preanalisis))
                        {
                            P_N();
                        }
                    }
                    Parea(26);//}
                    break;
                case 4:
                    Parea(4);//enemigo
                    Parea(18);//]
                    Parea(27);//:
                    Parea(25);//{
                    if (preanalisis == 17)
                    {
                        Parea(17);//[
                        if (primPe.Contains(preanalisis))
                        {
                            enemigos++;
                            esPared = false;
                            esPersonaje = false;
                            P_E();
                        }
                    }
                    Parea(26);//}
                    break;
                case 5:
                    Parea(5);//personaje
                    Parea(18);//]
                    Parea(27);//:
                    Parea(25);//{
                    if (primPPe.Contains(preanalisis))
                    {
                        esPersonaje = true;
                        esPared = false;
                        P_Pe();
                    }
                    Parea(26);//}
                    break;
                default:
                    //ERROR
                    break;
            }
        }

        public void P_N()
        {
            switch (preanalisis)
            {
                case 6:
                    String valorX = string.Empty;
                    String valorY = string.Empty;
                    Parea(6);//dimensiones
                    Parea(18);//]
                    Parea(27);//:
                    Parea(19);//(
                    if (preanalisis == 16)
                    {
                        foreach (String[] v in buscarToken())
                        {
                            valorX = v[0];
                        }
                    }
                    Parea(16);//num
                    Parea(30);//,
                    if (preanalisis == 16)
                    {
                        foreach (String[] v in buscarToken())
                        {
                            valorY = v[0];
                        }
                        String[] dimensiones = { "dimensiones", valorX, valorY };
                        valoresTablero.Add(dimensiones);
                    }
                    Parea(16);//num
                    Parea(20);//)
                    Parea(28);//;
                    if (preanalisis == 17)
                    {
                        Parea(17);//[
                        if (primPn.Contains(preanalisis))
                        {
                            P_N();
                        }
                    }
                    break;
                case 7:
                    String posicionX = string.Empty;
                    String posicionY = string.Empty;
                    Parea(7);//inicio_personaje
                    Parea(18);//]
                    Parea(27);//:
                    Parea(19);//(
                    if (preanalisis == 16)
                    {
                        foreach (String[] v in buscarToken())
                        {
                            posicionX = v[0];
                        }
                    }
                    Parea(16);//num
                    Parea(30);//,
                    if (preanalisis == 16)
                    {
                        foreach (String[] v in buscarToken())
                        {
                            posicionY = v[0];
                        }
                        String[] dimensiones = { "posicion_inicial_jugador", posicionX, posicionY };
                        valoresTablero.Add(dimensiones);
                    }
                    Parea(16);//num
                    Parea(20);//)
                    Parea(28);//;
                    if (preanalisis == 17)
                    {
                        Parea(17);//[
                        if (primPn.Contains(preanalisis))
                        {
                            P_N();
                        }
                    }
                    break;
                case 8:
                    String posicionX1 = string.Empty;
                    String posicionY1 = string.Empty;
                    Parea(8);//ubicacion_salida
                    Parea(18);//]
                    Parea(27);//:
                    Parea(19);//(
                    if (preanalisis == 16)
                    {
                        foreach (String[] v in buscarToken())
                        {
                            posicionX1 = v[0];
                        }
                    }
                    Parea(16);//num
                    Parea(30);//,
                    if (preanalisis == 16)
                    {
                        foreach (String[] v in buscarToken())
                        {
                            posicionY1 = v[0];
                        }
                        String[] dimensiones = { "posicion_final_jugador", posicionX1, posicionY1 };
                        valoresTablero.Add(dimensiones);
                    }
                    Parea(16);//num
                    Parea(20);//)
                    Parea(28);//;
                    if (preanalisis == 17)
                    {
                        Parea(17);//[
                        if (primPn.Contains(preanalisis))
                        {
                            P_N();
                        }
                    }
                    break;
                case 9:
                    Parea(9);//pared
                    Parea(18);//]
                    Parea(27);//:
                    Parea(25);//{
                    if (primPPa.Contains(preanalisis))
                    {
                        esPared = true;
                        esPersonaje = false;
                        P_Pa();
                    }
                    Parea(26);//}
                    if (preanalisis == 17)
                    {
                        Parea(17);//[
                        if (primPn.Contains(preanalisis))
                        {
                            P_N();
                        }
                        else if (primPn.Contains(preanalisis))
                        {
                            P_N();
                        }
                        else if (primPe.Contains(preanalisis))
                        {
                            P_E();
                        }
                        else if (primPPe.Contains(preanalisis))
                        {
                            P_Pe();
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        public void P_Pa()
        {
            esPared = true;
            esPersonaje = false;
            switch (preanalisis)
            {
                case 17:
                    Parea(17);//[
                    if (primPPa_1.Contains(preanalisis))
                    {
                        P_Pa_1();
                    }
                    if (primPPa.Contains(preanalisis))
                    {
                        P_Pa();
                    }
                    break;
                case 15:
                    if (preanalisis == 15)
                    {
                        foreach (String[] v in buscarToken())
                        {
                            izquierda = v[0];
                        }
                    }
                    Parea(15);//id
                    Parea(27);//:
                    Parea(29);//=
                    if (primOper.Contains(preanalisis))
                    {
                        Oper();
                    }
                    Parea(28);//;
                    if (primPPa.Contains(preanalisis))
                    {
                        P_Pa();
                    }
                    break;
                default:
                    break;
            }
        }

        public void P_Pa_1()
        {
            switch (preanalisis)
            {
                case 10:
                    String casillaX = string.Empty;
                    String casillaY = string.Empty;
                    Parea(10);//casilla
                    Parea(18);//]
                    Parea(27);//:
                    Parea(19);//(
                    if (preanalisis == 15)
                    {
                        foreach (String[] v in buscarToken())
                        {
                            diccionario.TryGetValue(v[0], out int a);
                            casillaX = a.ToString();
                        }
                        Parea(15);//id
                    }
                    else if (preanalisis == 16)
                    {
                        foreach (String[] v in buscarToken())
                        {
                            casillaX = v[0];
                        }
                        Parea(16);//num
                    }
                    Parea(30);//,
                    if (preanalisis == 15)
                    {
                        foreach (String[] v in buscarToken())
                        {
                            diccionario.TryGetValue(v[0], out int a);
                            casillaY = a.ToString();
                        }
                        Parea(15);//id
                    }
                    else if (preanalisis == 16)
                    {
                        foreach (String[] v in buscarToken())
                        {
                            casillaY = v[0];
                        }
                        Parea(16);//num
                    }
                    String[] casilla = { "casilla", casillaX, casillaY };
                    valoresTablero.Add(casilla);
                    Parea(20);//)
                    Parea(28);//;
                    break;
                case 11:
                    Parea(11);//varias_casillas
                    Parea(18);//]
                    Parea(27);//:
                    Parea(19);//(
                    if (preanalisis == 15)
                    {
                        foreach (String[] v in buscarToken())
                        {
                            diccionario.TryGetValue(v[0], out int a);
                            casillaX_inicio = a.ToString();
                        }
                        Parea(15);//id
                    }
                    else if (preanalisis == 16)
                    {
                        foreach (String[] v in buscarToken())
                        {
                            casillaX_inicio = v[0];
                        }
                        Parea(16);//num
                    }
                    if (primVar.Contains(preanalisis))
                    {
                        VariacionPerPar();
                    }
                    Parea(20);//)
                    Parea(28);//;
                    break;
                case 14:
                    String variable = string.Empty;
                    Parea(14);//variable
                    diccionario.Clear();
                    Parea(18);//]
                    Parea(27);//:
                    if (preanalisis == 15)
                    {
                        foreach (String[] v in buscarToken())
                        {
                            variable = v[0];
                        }
                        diccionario.Add(variable, 0);
                        String[] vari = { "variable", "pared", variable };
                        valoresTablero.Add(vari);
                    }
                    Parea(15);//id
                    if (primVar_2.Contains(preanalisis))
                    {
                        Variacion_2();
                    }
                    Parea(28);//;
                    break;
                default:
                    break;
            }
        }

        public void VariacionPerPar()
        {
            switch (preanalisis)
            {
                case 31:
                    Parea(31);//.
                    Parea(31);//.
                    if (preanalisis == 15)
                    {
                        foreach (String[] v in buscarToken())
                        {
                            diccionario.TryGetValue(v[0], out int a);
                            casillaX_fin = a.ToString();
                        }
                        Parea(15);//id
                    }
                    else if (preanalisis == 16)
                    {
                        foreach (String[] v in buscarToken())
                        {
                            casillaX_fin = v[0];
                        }
                        Parea(16);//num
                    }
                    Parea(30);//,
                    if (preanalisis == 15)
                    {
                        foreach (String[] v in buscarToken())
                        {
                            diccionario.TryGetValue(v[0], out int a);
                            casillaY_fin = a.ToString();
                        }
                        Parea(15);//id
                    }
                    else if (preanalisis == 16)
                    {
                        foreach (String[] v in buscarToken())
                        {
                            casillaY_fin = v[0];
                        }
                        Parea(16);//num
                    }
                    if (esPared == true)
                    {
                        String[] variasCasillas = { "varias_casillas", casillaX_inicio + "|" + casillaX_fin, casillaY_fin };
                        valoresTablero.Add(variasCasillas);
                    }
                    else if (esPersonaje == true)
                    {
                        String[] variasCasillas = { "caminata", casillaX_inicio + "|" + casillaX_fin, casillaY_fin };
                        valoresTablero.Add(variasCasillas);
                    }
                    break;
                case 30:
                    Parea(30);//,
                    if (preanalisis == 15)
                    {
                        foreach (String[] v in buscarToken())
                        {
                            diccionario.TryGetValue(v[0], out int a);
                            casillaY_inicio = a.ToString();
                        }
                        Parea(15);//id
                    }
                    else if (preanalisis == 16)
                    {
                        foreach (String[] v in buscarToken())
                        {
                            casillaY_inicio = v[0];
                        }
                        Parea(16);//num
                    }
                    if (primVar_1.Contains(preanalisis))
                    {
                        Variacion_1PerPar();
                    }
                    break;
            }
        }

        public void Variacion_1PerPar()
        {
            if (preanalisis == 31)
            {
                Parea(31);//.
                Parea(31);//.
                if (preanalisis == 15)
                {
                    foreach (String[] v in buscarToken())
                    {
                        diccionario.TryGetValue(v[0], out int a);
                        casillaY_fin = a.ToString();
                    }
                    Parea(15);//id
                }
                else if (preanalisis == 16)
                {
                    foreach (String[] v in buscarToken())
                    {
                        casillaY_fin = v[0];
                    }
                    Parea(16);//num
                }
            }
            if (esPared == true)
            {
                String[] variasCasillas = { "varias_casillas", casillaX_inicio, casillaY_inicio + "|" + casillaY_fin };
                valoresTablero.Add(variasCasillas);
            }
            else if (esPersonaje == true)
            {
                String[] variasCasillas = { "caminata", casillaX_inicio, casillaY_inicio + "|" + casillaY_fin };
                valoresTablero.Add(variasCasillas);
            }

        }

        public void Variacion_2()
        {
            if (preanalisis == 30)
            {
                Parea(30);//,
                if (preanalisis == 15)
                {
                    String variable = string.Empty;
                    foreach (String[] v in buscarToken())
                    {
                        variable = v[0];
                    }
                    if (esPared == true)
                    {
                        String[] vari = { "variable", "pared", variable };
                        diccionario.Add(variable, 0);
                        valoresTablero.Add(vari);
                    }
                    else if (esPersonaje == true)
                    {
                        String[] vari = { "variable", "personaje", variable };
                        diccionario.Add(variable, 0);
                        valoresTablero.Add(vari);
                    }
                }
                Parea(15);//id
            }
            if (primVar_2.Contains(preanalisis))
            {
                Variacion_2();
            }
        }

        public void P_E()
        {
            if (preanalisis == 12)
            {
                Parea(12);//caminata
                Parea(18);//]
                Parea(27);//:
                Parea(19);//(
                if (preanalisis == 16)
                {
                    foreach (String[] v in buscarToken())
                    {
                        casillaX_inicio = v[0];
                    }
                }
                Parea(16);//num
                if (primVar.Contains(preanalisis))
                {
                    Variacion();
                }
                Parea(20);//)
                Parea(28);//;
                if (primPe.Contains(preanalisis))
                {
                    P_E();
                }
            }
        }

        public void Variacion()
        {
            switch (preanalisis)
            {
                case 31:
                    Parea(31);//.
                    Parea(31);//.
                    if (preanalisis == 16)
                    {
                        foreach (String[] v in buscarToken())
                        {
                            casillaX_fin = v[0];
                        }
                    }
                    Parea(16);//num
                    Parea(30);//,
                    if (preanalisis == 16)
                    {
                        foreach (String[] v in buscarToken())
                        {
                            casillaY_inicio = v[0];
                        }
                    }
                    Parea(16);//num
                    String[] caminataEnemigo = { "caminata_" + enemigos.ToString(), casillaX_inicio + "|" + casillaX_fin, casillaY_inicio };
                    valoresTablero.Add(caminataEnemigo);
                    break;
                case 30:
                    Parea(30);//,
                    if (preanalisis == 16)
                    {
                        foreach (String[] v in buscarToken())
                        {
                            casillaY_inicio = v[0];
                        }
                    }
                    Parea(16);//num
                    if (primVar_1.Contains(preanalisis))
                    {
                        Variacion_1();
                    }
                    break;
            }
        }

        public void Variacion_1()
        {
            Parea(31);//.
            Parea(31);//.
            if (preanalisis == 16)
            {
                foreach (String[] v in buscarToken())
                {
                    casillaY_fin = v[0];
                }
            }
            Parea(16);//num
            String[] caminataEnemigo = { "caminata_" + enemigos.ToString(), casillaX_inicio, casillaY_inicio + "|" + casillaY_fin };
            valoresTablero.Add(caminataEnemigo);
        }

        public void P_Pe()
        {
            if (preanalisis == 17)
            {
                Parea(17);//[
                if (primPPe_1.Contains(preanalisis))
                {
                    P_Pe_1();
                }
            }
            else if (preanalisis == 15)
            {
                if (preanalisis == 15)
                {
                    foreach (String[] v in buscarToken())
                    {
                        izquierda = v[0];
                    }
                }
                Parea(15);//id
                Parea(27);//:
                Parea(29);//=
                if (primOper.Contains(preanalisis))
                {
                    Oper();
                }
                Parea(28);//;
                if (primPPe.Contains(preanalisis))
                {
                    P_Pe();
                }
            }
        }

        public void P_Pe_1()
        {
            switch (preanalisis)
            {
                case 12:
                    Parea(12);//caminata
                    Parea(18);//]
                    Parea(27);//:
                    Parea(19);//(
                    if (preanalisis == 15)
                    {
                        foreach (String[] v in buscarToken())
                        {
                            diccionario.TryGetValue(v[0], out int a);
                            casillaX_inicio = a.ToString();
                        }
                        Parea(15);//id
                    }
                    else if (preanalisis == 16)
                    {
                        foreach (String[] v in buscarToken())
                        {
                            casillaX_inicio = v[0];
                        }
                        Parea(16);//num
                    }
                    if (primVar.Contains(preanalisis))
                    {
                        VariacionPerPar();
                    }
                    Parea(20);//)
                    Parea(28);//;
                    if (primPPe.Contains(preanalisis))
                    {
                        P_Pe();
                    }
                    break;
                case 13:
                    String pasoX = string.Empty;
                    String pasoY = string.Empty;
                    Parea(13);//paso
                    Parea(18);//]
                    Parea(27);//:
                    Parea(19);//(
                    if (preanalisis == 15)
                    {
                        foreach (String[] v in buscarToken())
                        {
                            diccionario.TryGetValue(v[0], out int a);
                            pasoX = a.ToString();
                        }
                        Parea(15);//id
                    }
                    else if (preanalisis == 16)
                    {
                        foreach (String[] v in buscarToken())
                        {
                            pasoX = v[0];
                        }
                        Parea(16);//num
                    }
                    Parea(30);//,
                    if (preanalisis == 15)
                    {
                        foreach (String[] v in buscarToken())
                        {
                            diccionario.TryGetValue(v[0], out int a);
                            pasoY = a.ToString();
                        }
                        Parea(15);//id
                    }
                    else if (preanalisis == 16)
                    {
                        foreach (String[] v in buscarToken())
                        {
                            pasoY = v[0];
                        }
                        Parea(16);//num
                    }
                    String[] paso = { "paso", pasoX, pasoY };
                    valoresTablero.Add(paso);
                    Parea(20);//)
                    Parea(28);//;
                    if (primPPe.Contains(preanalisis))
                    {
                        P_Pe();
                    }
                    break;
                case 14:
                    String variable = string.Empty;
                    Parea(14);//variable
                    diccionario.Clear();
                    Parea(18);//]
                    Parea(27);//:
                    if (preanalisis == 15)
                    {
                        foreach (String[] v in buscarToken())
                        {
                            variable = v[0];
                        }
                        diccionario.Add(variable, 0);
                    }
                    String[] vari = { "variable", "personaje", variable };
                    valoresTablero.Add(vari);
                    Parea(15);//id
                    if (primVar_2.Contains(preanalisis))
                    {
                        Variacion_2();
                    }
                    Parea(28);//;
                    if (primPPe.Contains(preanalisis))
                    {
                        P_Pe();
                    }
                    break;
                default:
                    break;
            }
        }

        public void Oper()
        {
            if (primK.Contains(preanalisis))
            {
                termino = 1;
                K();
            }
            if (preanalisis == 21)
            {
                Oper_2();
                if (esPared)
                {
                    String temp1 = "";
                    String temp2 = "";
                    if (diccionario.ContainsKey(termino1))
                    {
                        //esID
                        diccionario.TryGetValue(termino1, out int a);
                        temp1 = System.Convert.ToString(a);
                    }
                    else
                    {
                        temp1 = termino1;
                    }
                    if (diccionario.ContainsKey(termino2))
                    {
                        //esID
                        diccionario.TryGetValue(termino2, out int a);
                        temp2 = System.Convert.ToString(a);
                    }
                    else
                    {
                        temp2 = termino2;
                    }
                    String operacion = temp1 + operador + temp2;
                    String total = EvaluarExpresion(operacion);
                    int totaln = System.Convert.ToInt32(total);
                    diccionario.Remove(izquierda);
                    diccionario.Add(izquierda, totaln);
                    //String[] operacion = { "operacion", "pared", izquierda, termino1, operador, termino2 };
                    //valoresTablero.Add(operacion);
                }
                else
                {
                    String temp1 = "";
                    String temp2 = "";
                    if (diccionario.ContainsKey(termino1))
                    {
                        //esID
                        diccionario.TryGetValue(termino1, out int a);
                        temp1 = System.Convert.ToString(a);
                    }
                    else
                    {
                        temp1 = termino1;
                    }
                    if (diccionario.ContainsKey(termino2))
                    {
                        //esID
                        diccionario.TryGetValue(termino2, out int a);
                        temp2 = System.Convert.ToString(a);
                    }
                    else
                    {
                        temp2 = termino2;
                    }
                    String operacion = temp1 + operador + temp2;
                    String total = EvaluarExpresion(operacion);
                    int totaln = System.Convert.ToInt32(total);
                    diccionario.Remove(izquierda);
                    diccionario.Add(izquierda, totaln);
                    //String[] operacion = { "operacion", "personaje", izquierda, termino1, operador, termino2 };
                    //valoresTablero.Add(operacion);
                }
            }
            else
            {
                if (esPared)
                {
                    String temp1 = "";
                    if (diccionario.ContainsKey(termino1))
                    {
                        //esID
                        diccionario.TryGetValue(termino1, out int a);
                        temp1 = System.Convert.ToString(a);
                    }
                    else
                    {
                        temp1 = termino1;
                    }
                    String operacion = temp1;
                    String total = EvaluarExpresion(operacion);
                    int totaln = System.Convert.ToInt32(total);
                    diccionario.Remove(izquierda);
                    diccionario.Add(izquierda, totaln);
                    //String[] operacion = { "operacion", "pared", izquierda, termino1 };
                    //valoresTablero.Add(operacion);
                }
                else
                {
                    String temp1 = "";
                    if (diccionario.ContainsKey(termino1))
                    {
                        //esID
                        diccionario.TryGetValue(termino1, out int a);
                        temp1 = System.Convert.ToString(a);
                    }
                    else
                    {
                        temp1 = termino1;
                    }
                    String operacion = temp1;
                    String total = EvaluarExpresion(operacion);
                    int totaln = System.Convert.ToInt32(total);
                    diccionario.Remove(izquierda);
                    diccionario.Add(izquierda, totaln);
                    //String[] operacion = { "operacion", "personaje", izquierda, termino1 };
                    //valoresTablero.Add(operacion);
                }
            }
        }

        public void K()
        {
            if (preanalisis == 15)
            {
                foreach (String[] v in buscarToken())
                {
                    if (termino == 1)
                    {
                        termino1 = v[0];
                    }
                    else
                    {
                        termino2 = v[0];
                    }
                }
                Parea(15);//id
            }
            else if (preanalisis == 16)
            {
                foreach (String[] v in buscarToken())
                {
                    if (termino == 1)
                    {
                        termino1 = v[0];
                    }
                    else
                    {
                        termino2 = v[0];
                    }
                }
                Parea(16);//num
            }
        }

        public void Oper_2()
        {
            if (preanalisis == 21)
            {
                termino = 2;
                foreach (String[] v in buscarToken())
                {
                    operador = v[0];
                }
                Parea(21);//+,-,/,*
                if (primK.Contains(preanalisis))
                {
                    K();
                }
            }
        }

        public void Parea(int token)
        {
            if (token == preanalisis)
            {
                //    MessageBox.Show("Se espera: " + token);
                //    MessageBox.Show("Viene: " + preanalisis);
                preanalisis = nextToken();
            }
            else
            {
                //    MessageBox.Show("ERROR");
                //    MessageBox.Show("Se espera: " + token);
                String tokenAlmacenado = string.Empty;
                String filaAlmacenada = string.Empty;
                String columnaAlmacenada = string.Empty;
                String tipo = "Sintactico";
                String tokenEsperado = string.Empty;
                ArrayList dato = tokens.GetRange(puntero, 1);
                foreach (String[] v in dato)
                {
                    tokenAlmacenado = v[0];
                    filaAlmacenada = v[3];
                    columnaAlmacenada = v[2];
                }
                if (token < 15)
                {
                    tokenEsperado = reservadas[token - 1];
                }
                else if (token > 16)
                {
                    tokenEsperado = System.Convert.ToString(simbolos[token - 17]);
                }
                else if (token == 15)
                {
                    tokenEsperado = "id";
                }
                else if (token == 16)
                {
                    tokenEsperado = "num";
                }
                String[] errorSintactico = { tokenAlmacenado, tokenEsperado, filaAlmacenada, columnaAlmacenada };
                erroresSintacticos.Add(errorSintactico);
                preanalisis = nextToken();
            }
        }

        public int nextToken()
        {
            int devolver = 0;
            int temp = puntero + 1;
            if (temp < tokens.Count)
            {
                puntero++;
                ArrayList arrayTemp = tokens.GetRange(puntero, 1);
                foreach (String[] v in arrayTemp)
                {
                    devolver = System.Convert.ToInt32(v[1]);
                }
            }
            return devolver;
        }

        public int buscarSimbolo(char simbolo)
        {
            int index = 0;
            for (int i = 0; i < simbolos.Length; i++)
            {
                if (simbolo.Equals(simbolos[i]))
                {
                    index = i;
                }
            }
            return index;
        }

        public int buscarReservada(String simbolo)
        {
            int index = 0;
            for (int i = 0; i < reservadas.Length; i++)
            {
                if (simbolo.Equals(reservadas[i]))
                {
                    index = i;
                }
            }
            return index;
        }

        private void generarTablas()
        {
            if (erroresLexicos.Count == 0 && erroresSintacticos.Count == 0)
            {
                //Tabla de Tokens
                String html1 = "<!DOCTYPE html><html><head><title>Tokens</title></head>" +
                 "<body><table border = '1'><tr><td><center>No</center></td><td><center>Fila</center></td><td><center>" +
                 "Columna</center></td><td><center>Tipo</center></td><td><center>Token</center></td></tr>";
                String html2 = "</table></body></html>";
                SaveFileDialog save = new SaveFileDialog();
                StreamWriter writer;
                save.Filter = "Tabla Token|.html";
                save.Title = "Guardar Tokens";
                if (save.ShowDialog() == DialogResult.OK)
                {
                    int contador = 1;
                    writer = File.CreateText(save.FileName);
                    writer.WriteLine(html1);
                    foreach (String[] v in tokens)
                    {
                        String cadena = v[0].ToString();
                        String tipo = v[1].ToString();
                        String columna = v[2].ToString();
                        String fila = v[3].ToString();
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
                        contador++;
                    }
                    writer.WriteLine(html2);
                    writer.Close();
                    MessageBox.Show("Archivo Guardado", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    FilePath = save.FileName;
                    Process.Start(FilePath);
                }
            }

            if (erroresLexicos.Count != 0)
            {
                //Tabla de Errores
                String html1 = "<!DOCTYPE html><html><head><title>Errores Lexicos</title></head>" +
                 "<body><table border = '1'><tr><td><center>No</center></td><td><center>Fila</center></td><td><center>" +
                 "Columna</center></td><td><center>Tipo</center></td><td><center>Token</center></td></tr>";
                String html2 = "</table></body></html>";
                SaveFileDialog save = new SaveFileDialog();
                StreamWriter writer;
                save.Filter = "Tabla Errores Lexicos|.html";
                save.Title = "Guardar Errores Lexicos";
                if (save.ShowDialog() == DialogResult.OK)
                {
                    int contador = 1;
                    writer = File.CreateText(save.FileName);
                    writer.WriteLine(html1);
                    foreach (String[] v in erroresLexicos)
                    {
                        String cadena = v[0].ToString();
                        String tipo = v[1].ToString();
                        String columna = v[2].ToString();
                        String fila = v[3].ToString();
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
                        contador++;
                    }
                    writer.WriteLine(html2);
                    writer.Close();
                    MessageBox.Show("Archivo Guardado", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    FilePath = save.FileName;
                    Process.Start(FilePath);
                }
            }

            if (erroresSintacticos.Count != 0)
            {
                //Tabla de Errores
                String html1 = "<!DOCTYPE html><html><head><title>Errores Sintacticos</title></head>" +
                 "<body><table border = '1'><tr><td><center>No</center></td><td><center>Fila</center></td><td><center>" +
                 "Columna</center></td><td><center>Se esperaba</center></td></tr>";
                String html2 = "</table></body></html>";
                SaveFileDialog save = new SaveFileDialog();
                StreamWriter writer;
                save.Filter = "Tabla Errores Sintacticos|.html";
                save.Title = "Guardar Errores Sintacticos";
                if (save.ShowDialog() == DialogResult.OK)
                {
                    int contador = 1;
                    writer = File.CreateText(save.FileName);
                    writer.WriteLine(html1);
                    foreach (String[] v in erroresSintacticos)
                    {
                        String cadena = v[1].ToString();
                        String tipo = v[0].ToString();
                        String columna = v[3].ToString();
                        String fila = v[2].ToString();
                        writer.WriteLine("<tr><td><center>");
                        writer.WriteLine(contador);
                        writer.WriteLine("</center></td><td><center>");
                        writer.WriteLine(fila);
                        writer.WriteLine("</center></td><td><center>");
                        writer.WriteLine(columna);
                        writer.WriteLine("</center></td><td><center>");
                        writer.WriteLine(cadena);
                        writer.WriteLine("</center></td></tr>");
                        contador++;
                    }
                    writer.WriteLine(html2);
                    writer.Close();
                    MessageBox.Show("Archivo Guardado", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    FilePath = save.FileName;
                    Process.Start(FilePath);
                }
            }
        }

        private void generarTablero()
        {
            foreach (String[] v in valoresTablero)
            {
                for (int i = 0; i < v.Length; i++)
                {
                    //MessageBox.Show(v[i]);
                }
            }
        }

        private ArrayList buscarToken()
        {
            ArrayList temp = tokens.GetRange(puntero, 1);
            return temp;
        }

        public String EvaluarExpresion(string expresion)
        {
            Microsoft.JScript.Vsa.VsaEngine Engine = Microsoft.JScript.Vsa.VsaEngine.CreateEngine();
            try
            {
                object o = Eval.JScriptEvaluate(expresion, Engine);
                return o.ToString();
            }
            catch (Exception)
            {
                return null;
            }
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

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Salir del Programa", "Salir", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(nextToken().ToString());
        }
    }
}
