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
            this.label1.Text = "DOC1 NO CARGADO";

            this.label2.Text = "DOC2 NO CARGADO";
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
                this.label1.Text = "Doc1 SRC CARGADO";
            }
            if (g_file.FileName.Contains("doc2"))
            {
                processtextDoc2();
                getComandsDoc2();
                this.label2.Text = "Doc2 DAT CARGADO";
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
            int cont = 0;
            string text = String.Empty;
            foreach (string item in lines)
            {
                string[] line = item.Split(';');

                if (line[0].Equals("CMD_INIT") || line.Equals("CMD_SLEEP"))
                {
                    listademensajes.Add(line[0]+';'+line[3]+ '\n');
                }
                if (line[0].Equals("CMD_CHANGETOOL"))
                {
                    if (line.Count()>1)
                    {
                        if (!line[3].Equals("0"))
                        {
                            listademensajes.Add(line[0] + ';' + line[3] + '\n');
                        }

                        
                    }
                }
                if (line[0].Equals("CMD_CHANGEWORKZONE"))
                {
                    if (line.Count() > 1)
                    {
                        if (!line[3].Equals("0"))
                        {
                            listademensajes.Add(line[0] + ';' + line[3] + '\n');
                        }
                    }
                }
                if (line[0].Equals("CMD_SETENTRY"))
                {
                    if (!String.IsNullOrEmpty(line[3]))
                    {

                        if (!line[3].Equals("0"))
                        {
                            text = lines[cont + 1];
                            string[] data = text.Split(';');

                            listademensajes.Add(line[0] + ";" + line[3] + ";" + data[7]+";"+data[10]+";"+data[11]+";"+data[3]+"\n") ;

                        }
                    }

                }
                if (line[0].Equals("CMD_VALVEAPERTURE"))
                {
                    if (line.Count() > 1)
                    {
                        if (!line[3].Equals("0"))
                        {
                            listademensajes.Add(line[0] + ";" + line[3] + "\n");
                        }

                        
                    }
                }
                if (line[1].Equals("FOLD"))
                {
                    string[] data = text.Split(';');

                    if (!line[3].Equals(data[3]))
                    {
                        listademensajes.Add(line[2] + ';' + line[3] + ';' + line[7] + ';' + line[10] +';'+ line[11]+ '\n');
                    }

                    

                }
                cont++;
            }

            listademensajes = listademensajes.Select(i => i).Distinct().ToList();
        }


        List<string> listademensajesDoc2 = new List<string>();

        public void getComandsDoc2()
        {
            string[] lines = File.ReadAllLines(g_pathProcessTextDoc2);

            foreach (var item in lines)
            {
                string[] line = item.Split(';');

                if (!String.IsNullOrEmpty(line[0]))
                {
                    if (!line[0].Contains("FOLD"))
                    {
                        string[] auxSpaceline = line[0].Split(' ');


                        if (line.Count() > 1)
                        {
                            if (!auxSpaceline[1].Equals("PDAT") && !auxSpaceline[1].Equals("FDAT") && !auxSpaceline[1].Equals("LDAT"))
                            {
                                string poses = line[1] + ',' + line[2] + ',' + line[3] + ',' + line[4] + ',' + line[5] + ',' + line[6] + ',' + line[7] + ',' + line[8] + ',' + line[9] + ',' + line[10] + ',' + line[11] + ',' + line[12] + ',' + line[13] + ',' + line[14];
                                listademensajesDoc2.Add(auxSpaceline[2] + ';' + poses + '\n');
                            }
                        }

                        
                    }

                }

              
            }


        }

        public string getPoses(string text)
        {
            string line = String.Empty;

            foreach (var item in listademensajesDoc2)
            {
                if (item.Contains(text))
                {
                    line = item;
                    break;
                }
            }

            


            return line;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.dataGridView1.Rows.Clear();
        }

        private void ShowData_Click(object sender, EventArgs e)
        {

            string changeWorkzone = string.Empty;
            string circPoses = "1";
            string defaultPose = "X 0,Y 0,Z 0,A 0,B 0,C 0,S 0,T 0,E1 0,E2 0.0,E3 0.0,E4 0.0,E5 0.0,E6 0.0";

                int cont = 0;

                foreach (var item in listademensajes)
                {


                    string[] line = item.Split(';');

                    if (line[0].Equals("PTP") || line[0].Equals("LIN"))
                    {

                        if (!String.IsNullOrEmpty(line[2]))
                        {
                            string poses = getPoses("X" + line[1]);
                            string[] auxPoses = poses.Split(';');
                            string tool = line[3].Trim('[', ']', '\n', 'T', 'o', 'l');
                            string Base = line[4].Trim('B', 'a', 's', 'e', '[', ']', '\n');
                            this.dataGridView1.Rows.Add(MOVECMD, auxPoses[1], line[0], line[2], tool,"-1", changeWorkzone, Base,defaultPose,"-1");
                        }
                        else
                        {

                            this.dataGridView1.Rows.Add(MOVECMD, "", line[0], "-1", "-1");
                        }

                    }

                    if (line[0].Equals("CIRC"))
                    {
                    //Numero de coordenada Circ
                        int NCirc = Int32.Parse(line[1].Trim('C'));
                        int NCirc2 = NCirc + 1;
                        string poseCirc2 = "XC" + NCirc2;  
                        
                        string poseCirc1= getPoses("X" + line[1]);
                        string poseCir2 = getPoses(poseCirc2);
                        string[] auxPosescirc1 = poseCirc1.Split(';');
                        string[] auxPosescirc2 = poseCir2.Split(';');
                        string tool = line[3].Trim('[', ']', '\n', 'T', 'o', 'l');
                        string Base = line[4].Trim('B', 'a', 's', 'e', '[', ']', '\n');
                        this.dataGridView1.Rows.Add(MOVECMD,auxPosescirc2[1], line[0], line[2], tool, "-1", changeWorkzone, Base,auxPosescirc1[1], "-1");
                    }
                
                    if (line[0].Equals("CMD_VALVEAPERTURE"))
                    {
                        string[] auxline = line[0].Split('_');

                        this.dataGridView1.Rows.Add(auxline[1], defaultPose, "NULL", "-1", "-1", line[1], changeWorkzone,"-1",defaultPose,"-1");



                    }

                    if (line[0].Equals("CMD_SETENTRY"))
                    {
                        if (line[1] != ("0\n"))
                        {

                            string[] auxline = line[0].Split('_');

                            string dataposes = line[5].Trim('\n');

                            string poses = getPoses("X" + dataposes);
                            string[] auxposes = poses.Split(';');

                            string tool = line[3].Trim('[', ']', 'T', 'o', 'l', '\n');
                            string Base = line[4].Trim('B', 'a', 's', 'e', '[', ']', '\n');

                            this.dataGridView1.Rows.Add(auxline[1], auxposes[1], "NULL", line[2], tool,"-1",line[1],Base,defaultPose,"-1");

                        }

                    }

                if (line[0].Equals("CMD_INIT"))
                {
                    string[] auxline = line[0].Split('_');

                    this.dataGridView1.Rows.Add(auxline[1], defaultPose, "NULL", "-1", "-1", "-1", "-1", "-1", defaultPose, "-1");
                }
                if (line[0].Equals("CMD_CHANGEWORKZONE"))
                {
                    string[] auxline = line[0].Split('_');
                    changeWorkzone = line[1];
                    this.dataGridView1.Rows.Add(auxline[1], defaultPose, "NULL", "-1", "-1", "-1",line[1], "-1", defaultPose, "-1");
                }
                if (line[0].Equals("CMD_SLEEP"))
                {
                    string[] auxline = line[0].Split('_');

                    this.dataGridView1.Rows.Add(auxline[1], defaultPose, "NULL", "-1", "-1", "-1", changeWorkzone, "-1", defaultPose, "-1");
                }
                if (line[0].Equals("CMD_CHANGETOOL"))
                {
                    string[] auxline = line[0].Split('_');

                    this.dataGridView1.Rows.Add(auxline[1], defaultPose, "NULL", "-1", "-1", "-1", changeWorkzone, "-1", defaultPose, "-1","-1");
                }
                //else
                //{
                //    string[] auxline = line[0].Split('_');

                //    if (auxline.Count() >= 2)
                //    {

                //        if (!String.IsNullOrEmpty(auxline[1]))
                //        {
                //            this.dataGridView1.Rows.Add(auxline[1], "", "NULL", "-1", "-1","-1","-1","-1","","-1");

                //        }

                //    }

                //}
                cont++;
                }

        }


        //Ruta de archivos de salida para archivos procesados de DoC2
        string g_pathProcessTextDoc2 = @"C:\Users\thebo\Desktop\processtext\docfilterDoc2.csv";


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
                            line = line.Trim('\n');
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
