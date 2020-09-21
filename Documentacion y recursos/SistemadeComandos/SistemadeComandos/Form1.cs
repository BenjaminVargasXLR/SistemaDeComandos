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
using System.Text.RegularExpressions;

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

        /// <summary>
        /// Funcion que muestra el contenido dde un archivo
        /// </summary>
        /// <param name="namefile"></param>
        public void showLines(string namefile)
        {
            string[] lines = File.ReadAllLines(namefile);
            string line = string.Join("\n", lines);
            richTextBox1.AppendText(line + '\n');
        }

        public void showlist(List<string> list)
        {
            list.ForEach(item => richTextBox1.AppendText(item));
            richTextBox1.ScrollToCaret();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                richTextBox1.Clear();
                //Declaracion de variable que almacena la ruta del directorio
                string rootFile = String.Empty;

                if (g_file.ShowDialog().Equals(DialogResult.OK))
                {
                    rootFile = g_file.FileName;
                }
                if (g_file.FileName.Contains("doc1"))
                {
                    processtextDoc1();
                    showLines(g_pathProcessTextDoc1);
                    //GetComands();
                    //showlist(listademensajes);
                    List<string> listadecomandos = new List<string>();

                    listadecomandos = fillCombobox();

                    foreach (var item in listadecomandos)
                    {
                        comboBox1.Items.Add(item);
                    }

                }

                if (g_file.FileName.Contains("doc2"))
                {

                    processtextDoc2();
                    showLines(g_pathProcessTextDoc2);
           

                }
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
            
        }

        /// <summary>
        /// Funcion que retorna una lista con los datos de archivo dat
        /// </summary>
        /// <returns></returns>
        public List<string> loadDatFile()
        {
            string[] lines = File.ReadAllLines(g_pathProcessTextDoc2);

            List<string> listaDePosiciones = new List<string>();

            foreach (var item in lines)
            {
                listaDePosiciones.Add(item);
            }

            return listaDePosiciones;

        }



        /// <summary>
        /// Cargando comandos en combobox
        /// </summary>
        /// <returns></returns>
        public List<string> fillCombobox()
        {
            List<string> comandos = new List<string>();

            string[] lines = File.ReadAllLines(g_pathProcessTextDoc1);

            foreach (var item in lines)
            {
                string[] line = item.Split(';');

                comandos.Add(line[3]);

            }

            comandos = comandos.Select(i => i).Distinct().ToList();

            return comandos;


        }

        //Ruta de archivo de salida para archivos procesados de DOC1
        string g_pathProcessTextDoc1 = @"C:\Users\thebo\Desktop\processtext\docfilterDoc1.txt";

        /// <summary>
        /// Función que elimina texto no necesario del .source 
        /// </summary>
        public void processtextDoc1()
        {
            string line = null;

            try
            {
                using (StreamReader reader = new StreamReader(g_file.FileName))
                {
                    using (StreamWriter writer = new StreamWriter(g_pathProcessTextDoc1))
                    {
                        while ((line = reader.ReadLine()) != null)
                        {
                            if (!line.Contains(";FOLD PTP") && !line.Contains(";FOLD LIN") && !line.Contains(";FOLD CIRC") && !line.Contains("CMD_SETENTRY") && !line.Contains("CMD_INIT") && !line.Contains("CMD_CHANGEWORKZONE") && !line.Contains("CMD_CHANGETOOL") && !line.Contains("CMD_VALVEAPERTURE") && !line.Contains("CMD_SLEEP"))
                                continue;
                            //line = Regex.Replace(line, @"\s", "");
                            line = line.Trim('"');
                            line = line.Replace('=',' ');
                            line = line.Trim();
                            line = line.Replace(' ', ';');

                            writer.WriteLine(line);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message,"ERROR");
            }

           
        }

        //Ruta de archivos de salida para archivos procesados de DoC2
        string g_pathProcessTextDoc2 = @"C:\Users\thebo\Desktop\processtext\docfilterDoc2.txt";

        List<string> listademensajes = new List<string>();

        public void GetComands()
        {
            string[] lines = File.ReadAllLines(g_pathProcessTextDoc1);

            foreach (string item in lines)
            {
                string[] line = item.Split(';');

                if (line[0].Equals("CMD_INIT")  || line[0].Equals("CMD_CHANGEWORKZONE") || line[0].Equals("CMD_CHANGETOOL") || line[0].Equals("CMD_VALVEAPERTURE") || line.Equals("CMD_SLEEP"))
                {
                    listademensajes.Add(line[0]+'\n');
                }
                if (line[0].Equals("CMD_SETENTRY"))
                {
                    if (!String.IsNullOrEmpty(line[3]))
                    {
                        listademensajes.Add(line[0] + ';' + line[3]+'\n');
                    }

                }
                if (line[1].Equals("FOLD"))
                {
                    listademensajes.Add(line[2] + ';' + line[3]+';'+line[7]+'\n');
                }

            }

            listademensajes = listademensajes.Select(i => i).Distinct().ToList();
        }

        public void processtextDoc2()
        {
            string line = null;

            try
            {
                using (StreamReader reader = new StreamReader(g_file.FileName))
                {
                    using (StreamWriter writer = new StreamWriter(g_pathProcessTextDoc2))
                    {
                        while ((line = reader.ReadLine()) != null)
                        {
                            if (!line.Contains("DECL"))
                                continue;
                            //line = Regex.Replace(line, @"\s", "");
                            line = line.Trim('"');
                            line = line.Replace('=', ';');
                            line = line.Replace(',', ';');
                            line = line.Replace('"', ' ');
                            line = line.Replace('{', ' ');
                            line = line.Replace('}', ' ');

                            line = line.Trim(' ');
                            
                            writer.WriteLine(line);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "ERROR");
            }


        }
    }
}
