using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace SistemadeComandos
{
    public partial class Form1 : Form
    {

        //Instancia para abrir un documento
        OpenFileDialog g_file = new OpenFileDialog();

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Funcion que filtra y muestra mensajes de archivo 
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        public string listMessage(string[] lines, string element)
        {
            string l = String.Empty;
            if (!String.IsNullOrEmpty(element))
            {
                var line = lines.Where(x => x.Contains(element)).ToList();
                l = string.Join("\n", line);
                return l;
            }
            else
            {
                l = string.Join("\n", lines);
                return l;
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                
                //Declaracion de variable que almacena la ruta del directorio
                string rootFile = String.Empty;

                if (g_file.ShowDialog().Equals(DialogResult.OK))
                {
                    rootFile = g_file.FileName;
                }
                //Lectura de todas las lineas del documento
                string[] lines = File.ReadAllLines(rootFile);
                //paso de linea a variable string
                //var LinesRead = lines.Where(x => x.Contains("LCPDATP24")).ToList();
                string line = string.Join("\n", lines);
                //adiocion de las lineas leidas al visor
                richTextBox1.AppendText(line + '\n');
                richTextBox1.ScrollToCaret();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {

            try
            {
                richTextBox1.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                richTextBox1.Clear();
                string text = comboBox1.SelectedItem.ToString();
                string[] lines = File.ReadAllLines(g_file.FileName);
                string line = this.listMessage(lines, text);

                richTextBox1.AppendText(line + '\n');
                richTextBox1.ScrollToCaret();
            }
            catch(Exception ex)
            {

            }
            


        }
    }
}
