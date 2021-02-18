using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace tarea_3
{
    public partial class Form1 : Form
    {
        String[] patterns = {
                @"^(\d+)(\.)?(\d*)$",// 0 Reconoce Decimales
                @"^0*1(0|10*1)*$",//1 Cadenas con binarios de 1 impar
                @"^(\d)+([\+\-\*\/]){1}(\d)+(([\+\-\*\/]){1}(\d)+)*$",//2 Cadenas con operaciones aritmeticas
                @"^([A-Z]+)([a-zA-Z0-9]*)(\d+)([a-zA-Z0-9]*)$",//3 Cadenas que empiezan con Mayuscula y tienen 1 digito por lo menos
                @"^https://www.([a-z]*)(\w+).com$",//4 Cadenas con link
                @"^([a-z]+)(\w)*@([a-z]+).([a-z]+)$",//5 Correo Electronico
                @"^(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[012])/(2[0-9]{3}|3000)$",//6 Fecha con formado DIA/MES/AÑO
                @"^(True|False)((\|{2}|\&{2})(True|False)){0,1}$",//7 Boleano con operadores OR AND
                @"^(([1-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5]).){3}([1-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$",//8 IP
                @"^System.out.println\(\""(\s*|\w*)*\""\)$"//9 Cadena de Java
            };
        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "") {
                MessageBox.Show("Ingrese Cadena","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            else
            {
                Match match = Regex.Match(textBox1.Text,patterns[comboBox1.SelectedIndex]);
                if (match.Success) {
                    label2.Text = patterns[comboBox1.SelectedIndex];
                    label4.Text = "Cadena Valida";
                    label4.ForeColor = Color.Green;
                }
                else {
                    label2.Text = patterns[comboBox1.SelectedIndex];
                    label4.Text = "Cadena Invalida";
                    label4.ForeColor = Color.Red;
                }
            }
        }
    }
}
