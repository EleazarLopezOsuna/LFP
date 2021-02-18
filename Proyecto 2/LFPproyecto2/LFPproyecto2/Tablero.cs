using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LFPproyecto2
{
    public partial class Tablero : Form
    {
        ArrayList Valores = Form1.valoresTablero;
        ArrayList Pared;
        ArrayList RecorridoJugador;
        ArrayList RecorridoEnemigo;
        String posicionJugador = string.Empty;
        String posicionFinal = string.Empty;
        bool error = false;
        int intervalo = 0;
        ArrayList numeroEnemigos = new ArrayList();


        public Tablero()
        {
            InitializeComponent();
            CrearTablero();

        }


        public void CrearTablero()
        {
            Pared = new ArrayList();
            RecorridoJugador = new ArrayList();
            RecorridoEnemigo = new ArrayList();
            int dimensionX = 0;
            int dimensionY = 0;
            String posicionX = string.Empty;
            String posicionY = string.Empty;

            foreach (String[] v in Valores)
            {
                if (v[0].Equals("intervalo"))
                {
                    intervalo = Convert.ToInt32(v[1]);
                }
                if (v[0].Equals("dimensiones"))
                {
                    dimensionX = Convert.ToInt32(v[1]);
                    dimensionY = Convert.ToInt32(v[2]);
                }
                if (v[0].Equals("casilla"))
                {
                    posicionX = v[1];
                    posicionY = v[2];
                    Pared.Add(posicionX + posicionY);
                }
                if (v[0].Equals("posicion_inicial_jugador"))
                {
                    posicionX = v[1];
                    posicionY = v[2];
                    posicionJugador = posicionX + posicionY;
                }
                if (v[0].Equals("cantidadEnemigos"))
                {
                    for (int i = 1; i <= Convert.ToInt32(v[1]); i++)
                    {
                        numeroEnemigos.Add(i);
                    }
                }
                if (v[0].Equals("posicion_final_jugador"))
                {
                    posicionX = v[1];
                    posicionY = v[2];
                    posicionFinal = posicionX + posicionY;
                }
                if (v[0].Equals("casilla"))
                {
                    String sX = v[1];
                    String sY = v[2];
                    Pared.Add(sX + sY);
                }
                if (v[0].Equals("paso"))
                {
                    String sX = v[1];
                    String sY = v[2];
                    RecorridoJugador.Add(sX + sY);
                }
                if (v[0].StartsWith("caminata_"))
                {
                    String[] datos = v[0].Split('_');
                    String[] sX = v[1].Split('|');
                    String[] sY = v[2].Split('|');
                    int numero1X = -1;
                    int numero2X = -1;
                    int numero1Y = -1;
                    int numero2Y = -1;
                    if (sX.Length == 2)
                    {
                        try
                        {
                            numero1X = Convert.ToInt32(sX[0]);
                            numero2X = Convert.ToInt32(sX[1]);
                            numero1Y = Convert.ToInt32(sY[0]);
                            if (numero1X < numero2X)
                            {
                                for (int i = numero1X; i <= numero2X; i++)
                                {
                                    posicionX = Convert.ToString(i);
                                    posicionY = sY[0];
                                    String[] recorrido = { datos[1], posicionX + posicionY };
                                    RecorridoEnemigo.Add(recorrido);
                                    //MessageBox.Show("AGREGADO    DE = " + numero1X + "   HASTA = " + numero2X);
                                }
                                for (int i = numero2X; i >= numero1X; i--)
                                {
                                    posicionX = Convert.ToString(i);
                                    posicionY = sY[0];
                                    String[] recorrido = { datos[1], posicionX + posicionY };
                                    RecorridoEnemigo.Add(recorrido);
                                    //MessageBox.Show("AGREGADO    DE = " + numero1X + "   HASTA = " + numero2X);
                                }
                            }
                            else
                            {
                                for (int i = numero1X; i >= numero2X; i--)
                                {
                                    posicionX = Convert.ToString(i);
                                    posicionY = sY[0];
                                    String[] recorrido = { datos[1], posicionX + posicionY };
                                    RecorridoEnemigo.Add(recorrido);
                                    //MessageBox.Show("AGREGADO    DE = " + numero1X + "   HASTA = " + numero2X);
                                }
                                for (int i = numero2X; i <= numero1X; i++)
                                {
                                    posicionX = Convert.ToString(i);
                                    posicionY = sY[0];
                                    String[] recorrido = { datos[1], posicionX + posicionY };
                                    RecorridoEnemigo.Add(recorrido);
                                    //MessageBox.Show("AGREGADO    DE = " + numero1X + "   HASTA = " + numero2X);
                                }
                            }
                            MessageBox.Show("Enemigo: " + datos[1]);
                        }
                        catch (Exception)
                        {

                        }
                    }
                    else if (sY.Length == 2)
                    {
                        try
                        {
                            numero1X = Convert.ToInt32(sX[0]);
                            numero1Y = Convert.ToInt32(sY[0]);
                            numero2Y = Convert.ToInt32(sY[1]);
                            if (numero1Y < numero2Y)
                            {
                                for (int i = numero1Y; i <= numero2Y; i++)
                                {
                                    posicionY = Convert.ToString(i);
                                    posicionX = sX[0];
                                    String[] recorrido = { datos[1], posicionX + posicionY };
                                    RecorridoEnemigo.Add(recorrido);
                                    //MessageBox.Show("AGREGADO    X = " + posicionX + "   Y = " + posicionY);
                                    //MessageBox.Show("Enemigo: " + datos[1]);
                                }
                                for (int i = numero2Y; i >= numero1Y; i--)
                                {
                                    posicionY = Convert.ToString(i);
                                    posicionX = sX[0];
                                    String[] recorrido = { datos[1], posicionX + posicionY };
                                    RecorridoEnemigo.Add(recorrido);
                                    //MessageBox.Show("AGREGADO    X = " + posicionX + "   Y = " + posicionY);
                                    //MessageBox.Show("Enemigo: " + datos[1]);
                                }
                            }
                            else
                            {
                                for (int i = numero1Y; i >= numero2Y; i--)
                                {
                                    posicionY = Convert.ToString(i);
                                    posicionX = sX[0];
                                    String[] recorrido = { datos[1], posicionX + posicionY };
                                    RecorridoEnemigo.Add(recorrido);
                                    //MessageBox.Show("AGREGADO    DE = "+numero1Y+"   HASTA = "+numero2Y);
                                }
                                for (int i = numero2Y; i <= numero1Y; i++)
                                {
                                    posicionY = Convert.ToString(i);
                                    posicionX = sX[0];
                                    String[] recorrido = { datos[1], posicionX + posicionY };
                                    RecorridoEnemigo.Add(recorrido);
                                    //MessageBox.Show("AGREGADO    DE = "+numero1Y+"   HASTA = "+numero2Y);
                                }
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
                if (v[0].Equals("caminata"))
                {
                    String[] sX = v[1].Split('|');
                    String[] sY = v[2].Split('|');
                    int numero1X = -1;
                    int numero2X = -1;
                    int numero1Y = -1;
                    int numero2Y = -1;
                    if (sX.Length == 2)
                    {
                        try
                        {
                            numero1X = Convert.ToInt32(sX[0]);
                            numero2X = Convert.ToInt32(sX[1]);
                            numero1Y = Convert.ToInt32(sY[0]);
                            if (numero1X < numero2X)
                            {
                                for (int i = numero1X; i <= numero2X; i++)
                                {
                                    posicionX = Convert.ToString(i);
                                    posicionY = sY[0];
                                    RecorridoJugador.Add(posicionX + posicionY);
                                    //MessageBox.Show("AGREGADO    DE = " + numero1X + "   HASTA = " + numero2X);
                                }
                            }
                            else
                            {
                                for (int i = numero1X; i >= numero2X; i--)
                                {
                                    posicionX = Convert.ToString(i);
                                    posicionY = sY[0];
                                    RecorridoJugador.Add(posicionX + posicionY);
                                    //MessageBox.Show("AGREGADO    DE = " + numero1X + "   HASTA = " + numero2X);
                                }
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }
                    else if (sY.Length == 2)
                    {
                        try
                        {
                            numero1X = Convert.ToInt32(sX[0]);
                            numero1Y = Convert.ToInt32(sY[0]);
                            numero2Y = Convert.ToInt32(sY[1]);
                            if (numero1Y < numero2Y)
                            {
                                for (int i = numero1Y; i <= numero2Y; i++)
                                {
                                    posicionY = Convert.ToString(i);
                                    posicionX = sX[0];
                                    RecorridoJugador.Add(posicionX + posicionY);
                                    //MessageBox.Show("AGREGADO    DE = "+numero1Y+"   HASTA = "+numero2Y);
                                }
                            }
                            else
                            {
                                for (int i = numero1Y; i >= numero2Y; i--)
                                {
                                    posicionY = Convert.ToString(i);
                                    posicionX = sX[0];
                                    RecorridoJugador.Add(posicionX + posicionY);
                                    //MessageBox.Show("AGREGADO    DE = "+numero1Y+"   HASTA = "+numero2Y);
                                }
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
                if (v[0].Equals("varias_casillas"))
                {
                    String[] sX = v[1].Split('|');
                    String[] sY = v[2].Split('|');
                    int numero1X = -1;
                    int numero2X = -1;
                    int numero1Y = -1;
                    int numero2Y = -1;
                    if (sX.Length == 2)
                    {
                        try
                        {
                            numero1X = Convert.ToInt32(sX[0]);
                            numero2X = Convert.ToInt32(sX[1]);
                            numero1Y = Convert.ToInt32(sY[0]);
                            for (int i = numero1X; i <= numero2X; i++)
                            {
                                posicionX = Convert.ToString(i);
                                posicionY = sY[0];
                                Pared.Add(posicionX + posicionY);
                                //MessageBox.Show("AGREGADO    DE = " + numero1X + "   HASTA = " + numero2X);
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }
                    else if (sY.Length == 2)
                    {
                        try
                        {
                            numero1X = Convert.ToInt32(sX[0]);
                            numero1Y = Convert.ToInt32(sY[0]);
                            numero2Y = Convert.ToInt32(sY[1]);
                            for (int i = numero1Y; i <= numero2Y; i++)
                            {
                                posicionY = Convert.ToString(i);
                                posicionX = sX[0];
                                Pared.Add(posicionX + posicionY);
                                //MessageBox.Show("AGREGADO    DE = "+numero1Y+"   HASTA = "+numero2Y);
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }
            panelJuego.ColumnCount = dimensionX;
            panelJuego.RowCount = dimensionY;
            for (int i = 0; i < dimensionX; i++)
            {
                for (int j = 0; j < dimensionY; j++)
                {
                    panelJuego.Controls.Add(new Label { Size = new Size(30, 30), Margin = new Padding(0), Name = i.ToString() + j.ToString() }, i, j);
                }
            }

            String[] numero = new string[numeroEnemigos.Count];
            int contador = 0;
            foreach(String[] v in RecorridoEnemigo)
            {
                if (v[0].Equals(Convert.ToString(contador + 1)))
                {
                    numero[contador] = v[1];
                    contador++;
                }
            }

            foreach (Control ctrl in panelJuego.Controls)
            {
                if (ctrl is Label)
                {
                    if (Pared.Contains(ctrl.Name))
                    {
                        Image imagen = global::LFPproyecto2.Properties.Resources.Pared;
                        ctrl.BackgroundImage = imagen;
                        ctrl.Name = "pared" + ctrl.Name;
                    }
                    else if (posicionJugador.Equals(ctrl.Name))
                    {
                        Image imagen = global::LFPproyecto2.Properties.Resources.Thor;
                        ctrl.BackgroundImage = imagen;
                    }
                    else if (posicionFinal.Equals(ctrl.Name))
                    {
                        Image imagen = global::LFPproyecto2.Properties.Resources.descarga;
                        ctrl.BackgroundImage = imagen;
                    }else if (numero.Contains(ctrl.Name))
                    {
                        Image imagen = global::LFPproyecto2.Properties.Resources.Thor;
                        ctrl.BackgroundImage = imagen;
                    }
                    else
                    {
                        Image imagen = global::LFPproyecto2.Properties.Resources.Fondo;
                        ctrl.BackgroundImage = imagen;
                    }
                }
            }
            panelJuego.AutoSize = true;
            panelJuego.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            panelJuego.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.AddRows;
        }

        public void moverJugador()
        {
            foreach (String v in RecorridoJugador)
            {
                if (posicionJugador != v)
                {
                    Control[] ctrl2 = panelJuego.Controls.Find(v, true);
                   if(ctrl2 != null)
                    {
                        ctrl2[0].BackgroundImage = global::LFPproyecto2.Properties.Resources.Thor;
                        Control[] ctrl1 = panelJuego.Controls.Find(posicionJugador, true);
                        ctrl1[0].BackgroundImage = global::LFPproyecto2.Properties.Resources.Fondo;
                        posicionJugador = v;
                    }
                    else
                    {
                        MessageBox.Show("ERROR AL RECORRER RUTA");
                        error = true;
                        break;
                    }
                }
                if (posicionJugador.Equals(posicionFinal))
                {
                    MessageBox.Show("RUTA COMPLETADA CON EXITO");
                    break;
                }
                Thread.Sleep(intervalo);
            }
            if (!posicionJugador.Equals(posicionFinal))
            {
                MessageBox.Show("RUTA COMPLETADA SIN EXITO");
            }
        }

        public void moverEnemigo(int enemigo)
        {
            String[] posicionesEnemigos = new String[numeroEnemigos.Count];
            String[] posicionesTemporales = new String[numeroEnemigos.Count];
            String[] posicionEnemigo = new String[numeroEnemigos.Count];
            for (int i = 0; i < posicionEnemigo.Length; i++)
            {
                posicionEnemigo[i] = "-1";
            }
            bool termino = false;
            while (termino == false || error == false)
            {
                foreach (String[] v in RecorridoEnemigo)
                {
                    if (v[0].Equals(Convert.ToString(enemigo)))
                    {
                        if (posicionEnemigo[Convert.ToInt32(v[0]) - 1].Equals("-1"))
                        {
                            Control[] ctrl = panelJuego.Controls.Find(v[1], true);
                            posicionesEnemigos[Convert.ToInt32(v[0]) - 1] = v[1];
                            posicionesTemporales[Convert.ToInt32(v[0]) - 1] = v[1];
                            ctrl[0].BackgroundImage = global::LFPproyecto2.Properties.Resources.Thor;
                            posicionEnemigo[Convert.ToInt32(v[0]) - 1] = "0";
                            Thread.Sleep(intervalo);
                        }
                        else
                        {
                            Control[] ctrl = panelJuego.Controls.Find(v[1], true);
                            posicionesEnemigos[Convert.ToInt32(v[0]) - 1] = v[1];
                            ctrl[0].BackgroundImage = global::LFPproyecto2.Properties.Resources.Thor;
                            Control[] ctrl1 = panelJuego.Controls.Find(posicionesTemporales[Convert.ToInt32(v[0]) - 1], true);
                            ctrl1[0].BackgroundImage = global::LFPproyecto2.Properties.Resources.Fondo;
                            posicionesTemporales[Convert.ToInt32(v[0]) - 1] = v[1];
                            Thread.Sleep(intervalo);
                        }
                    }
                    if (posicionJugador.Equals(posicionFinal))
                    {
                        termino = true;
                        break;
                    }
                }
                for (int i = 0; i < posicionEnemigo.Length; i++)
                {
                    posicionEnemigo[i] = "-1";
                }
            }
        }

        private void panelJuego_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread recorridoJugador = new Thread(moverJugador);
            recorridoJugador.Start();
            foreach (int n in numeroEnemigos)
            {
                Thread recorridoEnemigo = new Thread(() => moverEnemigo(n));
                recorridoEnemigo.Start();
            }
        }
    }
}
