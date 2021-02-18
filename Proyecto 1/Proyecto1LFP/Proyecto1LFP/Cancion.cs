using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1LFP
{
    public class Cancion
    {
        private String duracion, nombre, año, url, artista, album;
        private String playlist;

        public Cancion(String duracion, String nombre, String año, String url, String album, String artista, String playlist)
        {
            this.duracion = duracion;
            this.nombre = nombre;
            this.año = año;
            this.url = url;
            this.album = album;
            this.artista = artista;
            this.playlist = playlist;
        }
        public String getDuracion() => duracion;
        public String getNombre() => nombre;
        public String getAño() => año;
        public String getUrl() => url;
        public String getAlbum() => album;
        public String getArtista() => artista;
        public String getPlaylist() => playlist;

        public void setDuracion(String duracion) => this.duracion = duracion;
        public void setNombre(String nombre) => this.nombre = nombre;
        public void setAño(String año) => this.año = año;
        public void setUrl(String url) => this.url = url;
        public void setAlbum(String album) => this.album = album;
        public void setArtista(String artista) => this.artista = artista;
        public void setPlaylist(String playlist) => this.playlist = playlist;
    }
}
