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
    public partial class Reproductor : Form
    {
        int contador = 0;
        public Reproductor()
        {
            InitializeComponent();
            CargarPlaylist();
        }

        private void Reproductor_Load(object sender, EventArgs e)
        {

        }
        
        void CargarPlaylist()
        {
            int numero = Form1.numeroPlaylist;
            NodoCancion cancion = Form1.lc.primero;
            while (cancion != null)
            {
                if (!comboBox1.Items.Contains(cancion.GetCancion().getPlaylist()))
                {
                    comboBox1.Items.Add(cancion.GetCancion().getPlaylist());
                    contador++;
                }
                cancion = cancion.siguiente;
            }
            if (comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedIndex = 0;
            }
        }

        private void CargarCanciones()
        {
            String PlaylistActual = comboBox1.SelectedItem.ToString();
            var myPlayList = axWindowsMediaPlayer1.playlistCollection.newPlaylist(PlaylistActual);
            axWindowsMediaPlayer1.currentPlaylist = myPlayList;
            NodoCancion cancion = Form1.lc.primero;
            while(cancion != null)
            {
                String nombrePlaylist = cancion.GetCancion().getPlaylist();
                if(nombrePlaylist.Equals(PlaylistActual))
                {
                    String nombre = cancion.GetCancion().getNombre();
                    String artista = cancion.GetCancion().getArtista();
                    String album = cancion.GetCancion().getAlbum();
                    String año = cancion.GetCancion().getAño();
                    String duracion = cancion.GetCancion().getDuracion();
                    String url = cancion.GetCancion().getUrl();
                    ListViewItem agregar = new ListViewItem(nombre);
                    agregar.SubItems.Add(artista);
                    agregar.SubItems.Add(album);
                    agregar.SubItems.Add(año);
                    agregar.SubItems.Add(duracion);
                    agregar.SubItems.Add(url);
                    listView1.Items.Add(agregar);
                    var mediaItem = axWindowsMediaPlayer1.newMedia(url);
                    myPlayList.appendItem(mediaItem);
                }
                cancion = cancion.siguiente;
            }
            axWindowsMediaPlayer1.Ctlcontrols.play();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            CargarCanciones();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int indexEliminado = 0;
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                indexEliminado = item.Index;
                item.Remove();
                int items = listView1.Items.Count;
                for (int i = 0; i < items; i++)
                {
                    String cadena = listView1.Items[i].SubItems[5].Text;
                }
                WMPLib.IWMPMedia media = axWindowsMediaPlayer1.currentPlaylist.Item[indexEliminado];
                WMPLib.IWMPMedia siguiente = axWindowsMediaPlayer1.currentPlaylist.Item[indexEliminado+1];
                axWindowsMediaPlayer1.currentPlaylist.removeItem(media);
                if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsReady)
                {
                    axWindowsMediaPlayer1.Ctlcontrols.playItem(siguiente);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                int index = item.Index;
                WMPLib.IWMPMedia media = axWindowsMediaPlayer1.currentPlaylist.Item[index];
                axWindowsMediaPlayer1.Ctlcontrols.playItem(media);
            }
        }

        private void axWindowsMediaPlayer1_PlaylistChange(object sender, AxWMPLib._WMPOCXEvents_PlaylistChangeEvent e)
        {
        }
    }
}
