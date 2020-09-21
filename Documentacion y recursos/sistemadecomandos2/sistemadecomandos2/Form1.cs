using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sistemadecomandos2
{
    public partial class Form1 : Form
    {
        //Instancia para abrir un documento
        OpenFileDialog g_file = new OpenFileDialog();

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

       

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public void addDGV()
        {
            DataGridView dgv = new DataGridView();


        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        string MOVECMD = "MOVECMD";

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();

            string rootfile = String.Empty;

            if (g_file.ShowDialog().Equals(DialogResult.OK))
            {
                rootfile = g_file.FileName;

            }
            if (g_file.FileName.Contains("doc1"))
            {
                processtextDoc1();
                GetComands();

                int cont = 0;

                foreach (var item in listademensajes)
                {


                    string[] line = item.Split(';');

                    if (line[0].Equals("PTP") || line[0].Equals("LIN") || line[0].Equals("CIRC"))
                    {
                        if (line.Count()==3)
                        {
                            if (!String.IsNullOrEmpty(line[2]))
                            {
                                this.dataGridView1.Rows.Add(MOVECMD, "", line[0], line[2]);
                            }
                            else
                            {

                                this.dataGridView1.Rows.Add(MOVECMD, "", line[0], "-1");
                            }

                           

                        }

                    }
                    if (line[0].Equals("CMD_SETENTRY"))
                    {
                        if (line[1]!=("0\n"))
                        {
                            string text = listademensajes.ElementAt(cont + 1);
                        }

                    }
                    else
                    {
                        string[] auxline = line[0].Split('_');

                        if (auxline.Count()>=2)
                        {

                            if (!String.IsNullOrEmpty(auxline[1]))
                            {
                                this.dataGridView1.Rows.Add(auxline[1], "", "NULL", "-1");

                            }

                        }

                       
                       

                       
                    }
                    cont++;
                }

            }
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
                            line = line.Replace('=', ' ');
                            line = line.Trim();
                            line = line.Replace(' ', ';');

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

        
        List<string> listademensajes = new List<string>();

        public void GetComands()
        {
            string[] lines = File.ReadAllLines(g_pathProcessTextDoc1);

            foreach (string item in lines)
            {
                string[] line = item.Split(';');

                if (line[0].Equals("CMD_INIT") || line[0].Equals("CMD_CHANGEWORKZONE") || line[0].Equals("CMD_CHANGETOOL") || line[0].Equals("CMD_VALVEAPERTURE") || line.Equals("CMD_SLEEP"))
                {
                    listademensajes.Add(line[0] + '\n');
                }
                if (line[0].Equals("CMD_SETENTRY"))
                {
                    if (!String.IsNullOrEmpty(line[3]))
                    {
                        listademensajes.Add(line[0] + ';' + line[3] + '\n');
                    }

                }
                if (line[1].Equals("FOLD"))
                {
                    listademensajes.Add(line[2] + ';' + line[3] + ';' + line[7]+ '\n');

                }

            }

            listademensajes = listademensajes.Select(i => i).Distinct().ToList();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.dataGridView1.Rows.Clear();
        }
    }
}
