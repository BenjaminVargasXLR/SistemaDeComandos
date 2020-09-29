using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
namespace sistemadecomandos2
{
    public partial class Form1 : Form
    {
        //Instancia para abrir un documento
        OpenFileDialog g_file = new OpenFileDialog();
       
        //Lista de carga de docs
        Dictionary<string, string> docs = new Dictionary<string, string>() {
            {"doc1","" },
            {"doc2","" }
        };
        
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
            try
            {

                if (!File.Exists(g_path))
                {
                    Directory.CreateDirectory(g_path);
                }

                this.ShowData.Enabled = false;

                this.btn_ExportToCSV.Enabled = false;

                this.label1.BackColor = Color.Red;
                this.label1.Text = "DOC1 NO CARGADO";
                this.label2.BackColor = Color.Red;
                this.label2.Text = "DOC2 NO CARGADO";
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

      

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        //Variable Global para asignar a los comandos de movimiento MOVECMD en la visualización
        string MOVECMD = "MOVECMD";


        /// <summary>
        /// Boton que carga los archivos al programa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            #region LogicaCargaDeArchivosEnPrograma

            try
            {
                
                dataGridView1.Rows.Clear();
                this.btn_ExportToCSV.Enabled = false;

                string rootfile = String.Empty;

                if (g_file.ShowDialog().Equals(DialogResult.OK))
                {
                    rootfile = g_file.FileName;

                }
                if (g_file.FileName.Contains("doc1.csv"))
                {
                    processtextDoc1();
                    GetComands();
                    this.label1.BackColor = Color.Green;
                    this.label1.Text = "Doc1 SRC CARGADO";
                }
                if (g_file.FileName.Contains("doc2.csv"))
                {
                    processtextDoc2();
                    getComandsDoc2();
                    this.label2.BackColor = Color.Green;
                    this.label2.Text = "Doc2 DAT CARGADO";
                }
                this.avilableButtonShowMessage();

                g_file.FileName = string.Empty;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            #endregion

        }




        //Ruta de archivo de salida para archivos procesados de DOC1
        string g_pathProcessTextDoc1 = g_path+"/"+"docfilterDoc1.csv";


        /// <summary>
        /// Función que elimina texto no necesario del .source 
        /// </summary>
        public void processtextDoc1()
        {
            #region LogicaProcesamientodeSRC
            string line = null;

            try
            {
                using (StreamReader reader = new StreamReader(g_file.FileName))
                {
                    using (StreamWriter writer = new StreamWriter(g_pathProcessTextDoc1))
                    {

                        while ((line = reader.ReadLine()) != null)
                        {
                            if (!line.Contains(";FOLD PTP") && !line.Contains(";FOLD LIN") && !line.Contains(";FOLD CIRC") && !line.Contains("CMD_SETENTRY") && !line.Contains("CMD_INIT") && !line.Contains("CMD_CHANGEWORKZONE") && !line.Contains("CMD_CHANGETOOL") && !line.Contains("CMD_VALVEAPERTURE") && !line.Contains("CMD_SLEEP") && !line.Contains("CMD_ENDZONE") && !line.Contains("CMD_FINALIZE"))
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

                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            #endregion
        }



        //Lista global que almacena comandos y sus caracticas de archivo SRC
        List<string> listademensajes = new List<string>();
        /// <summary>
        /// FUncion que almacena los comandos y sus caracteristicas de archivo SRC
        /// </summary>
        public void GetComands()
        {
            try
            {
                #region LogicaGetComandos

                List<string> lines = File.ReadAllLines(g_pathProcessTextDoc1).ToList();
                int cont = 0;
                string text = String.Empty;

               
                
                foreach (string item in lines)
                {
                    string[] line = item.Split(';');

                    if (line[0].Equals("CMD_INIT"))
                    {
                        listademensajes.Add(line[0] + ';' + line[3] + '\n');
                    }
                    if (line[0].Equals("CMD_SLEEP"))
                    {
                        
                            listademensajes.Add(line[0] + ';' + line[3] + '\n');
                    }
                    if (line[0].Equals("CMD_CHANGETOOL"))
                    {
                        if (line.Count() > 1)
                        {
                            
                                listademensajes.Add(line[0] + ';' + line[3] + '\n');
                            


                        }
                    }
                    if (line[0].Equals("CMD_CHANGEWORKZONE"))
                    {
                        if (line.Count() > 1)
                        {
                            
                                listademensajes.Add(line[0] + ';' + line[3] + '\n');
                            
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

                                listademensajes.Add(line[0] + ";" + line[3] + ";" + data[7] + ";" + data[10] + ";" + data[11] + ";" + data[3] + "\n");

                            }
                        }

                    }
                    if (line[0].Equals("CMD_VALVEAPERTURE"))
                    {
                        if (line.Count() > 1)
                        {
                            
                                listademensajes.Add(line[0] + ";" + line[3] + "\n");
                            


                        }
                    }
                    if (line[0].Equals("CMD_ENDZONE"))
                    {
                        listademensajes.Add(line[0] + ";" + line[3] + "\n");
                    }
                    if (line[0].Equals("CMD_FINALIZE"))
                    {
                        listademensajes.Add(line[0] + ";" + line[3] + "\n");
                    }
                    if (line[1].Equals("FOLD"))
                    {
                        string[] data = text.Split(';');

                        if (!line[3].Equals(data[3]))
                        {
                            listademensajes.Add(line[2] + ';' + line[3] + ';' + line[7] + ';' + line[10] + ';' + line[11] + '\n');
                        }



                    }
                    cont++;
                }

                //listademensajes = listademensajes.Select(i => i).Distinct().ToList();
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            
        }

        //Lista Global para almacenar las coordenas de archivo DAT
        List<string> listademensajesDoc2 = new List<string>();
        /// <summary>
        /// Funcion que devuelve un listado de coordenadas del archivo dat
        /// </summary>
        public void getComandsDoc2()
        {

            #region LogicaGetComandsDoc2

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
                            if (!auxSpaceline[1].Equals("PDAT") && !auxSpaceline[1].Equals("FDAT") && !auxSpaceline[1].Equals("LDAT") && !auxSpaceline[1].Equals("INT") && !auxSpaceline[1].Equals("STATE_T"))
                            {
                                string poses = line[1] + ',' + line[2] + ',' + line[3] + ',' + line[4] + ',' + line[5] + ',' + line[6] + ',' + line[7] + ',' + line[8] + ',' + line[9] + ',' + line[10] + ',' + line[11] + ',' + line[12] + ',' + line[13] + ',' + line[14];
                                listademensajesDoc2.Add(auxSpaceline[2] + ';' + poses + '\n');
                            }
                        }

                        
                    }

                }

              
            }
            #endregion



        }

        /// <summary>
        /// Funcion que consulta 7 devuelve un valor de coordenadas de la lista de coordendas (DAT)
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public string getPoses(string text)
        {
            #region LogicaObtenciondeposicionesdeDAT

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
            #endregion
        }

        

        /// <summary>
        /// Funcion que habilita boton de mostrar datos cuando los documentos necesarios son cargados
        /// </summary>
        public void avilableButtonShowMessage()
        {
            
            
            if (g_file.FileName.Contains("doc1.csv"))
            {
                docs["doc1"] = "doc1.csv";
            }
            if (g_file.FileName.Contains("doc2.csv"))
            {
                docs["doc2"] = "doc2.csv";
            }


            
            if (docs["doc1"].Equals("doc1.csv") && docs["doc2"].Equals("doc2.csv"))
            {
                this.ShowData.Enabled = true;
                docs["doc1"] = String.Empty;
                docs["doc2"] = String.Empty;
            }
           
        }
        /// <summary>
        /// Boton que limpia la pantalla del datagridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            this.dataGridView1.Rows.Clear();
            this.btn_ExportToCSV.Enabled = false;
        }

      


        /// <summary>
        /// Boton dedicado a mostrar los mensajes y filtrar y consultar a las listas globales que almacenan la data de los archivos SRC y DAT
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowData_Click(object sender, EventArgs e)
        {
            
            #region LogicaParaMostrarDatos
           
            try
            {
                this.btn_ExportToCSV.Enabled = true;
                listademensajes.RemoveAt(0);
                listademensajes.RemoveAt(0);
                listademensajes.RemoveAt(0);
                listademensajes.RemoveAt(0);
                listademensajes.RemoveAt(0);
                listademensajes.RemoveAt(0);
                this.dataGridView1.Rows.Clear();
                string changeWorkzone = string.Empty;
                string defaultPose = "X 0,Y 0,Z 0,A 0,B 0,C 0,S 0,T 0,E1 0,E2 0.0,E3 0.0,E4 0.0,E5 0.0,E6 0.0";

                int cont = 0;

                


                foreach (var item in listademensajes)
                {


                    string[] line = item.Split(';');

                    if (dataGridView1.Rows.Count.Equals(0))
                    {
                        this.ShowData.Enabled = false;
                    }


                    if (line[0].Equals("PTP") || line[0].Equals("LIN"))
                    {

                        if (!String.IsNullOrEmpty(line[2]) && !line[1].Equals("HOME"))
                        {
                            string poses = getPoses("X" + line[1]);
                            string[] auxPoses = poses.Split(';');
                            string tool = line[3].Trim('[', ']', '\n', 'T', 'o', 'l');
                            string Base = line[4].Trim('B', 'a', 's', 'e', '[', ']', '\n');
                            this.dataGridView1.Rows.Add(MOVECMD, auxPoses[1], line[0], line[2], tool, "-1", changeWorkzone, Base, defaultPose, "-1");

                        }
                        if (line[1].Equals("HOME"))
                        {
                            this.dataGridView1.Rows.Add(MOVECMD, defaultPose, line[0], "-1", "-1", "-1", "-1", "-1", defaultPose, "-1");
                        }


                    }

                    if (line[0].Equals("CIRC"))
                    {
                        //Numero de coordenada Circ
                        int NCirc = Int32.Parse(line[1].Trim('C'));
                        int NCirc2 = NCirc + 1;
                        string poseCirc2 = "XC" + NCirc2;

                        string poseCirc1 = getPoses("X" + line[1]);
                        string poseCir2 = getPoses(poseCirc2);
                        string[] auxPosescirc1 = poseCirc1.Split(';');
                        string[] auxPosescirc2 = poseCir2.Split(';');
                        string tool = line[3].Trim('[', ']', '\n', 'T', 'o', 'l');
                        string Base = line[4].Trim('B', 'a', 's', 'e', '[', ']', '\n');
                        this.dataGridView1.Rows.Add(MOVECMD, auxPosescirc2[1], line[0], line[2], tool, "-1", changeWorkzone, Base, auxPosescirc1[1], "-1");
                    }

                    if (line[0].Equals("CMD_VALVEAPERTURE"))
                    {
                        string[] auxline = line[0].Split('_');

                        this.dataGridView1.Rows.Add(auxline[1], defaultPose, "NULL", "-1", "-1", line[1], changeWorkzone, "-1", defaultPose, "-1");
                    }
                    if (line[0].Equals("CMD_ENDZONE"))
                    {
                        string[] auxline = line[0].Split('_');

                        
                            this.dataGridView1.Rows.Add(auxline[1], defaultPose, "NULL", "-1", "-1", "-1", changeWorkzone, "-1", defaultPose, "-1");
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

                            this.dataGridView1.Rows.Add(auxline[1], auxposes[1], "NULL", line[2], tool, "-1", line[1], Base, defaultPose, "-1");

                        }

                    }

                    if (line[0].Equals("CMD_INIT"))
                    {
                        string[] auxline = line[0].Split('_');

                        this.dataGridView1.Rows.Add(auxline[1], defaultPose, "NULL", "-1", "-1", "-1", "-1", "-1", defaultPose, "-1");
                    }
                    if (line[0].Equals("CMD_FINALIZE"))
                    {
                        string[] auxline = line[0].Split('_');

                        if (!line[1].Equals("0\n"))
                        {
                            this.dataGridView1.Rows.Add(auxline[1], defaultPose, "NULL", "-1", "-1", "-1", "-1", "-1", defaultPose, "-1");

                        }

                    }
                    if (line[0].Equals("CMD_CHANGEWORKZONE"))
                    {
                        string[] auxline = line[0].Split('_');
                        changeWorkzone = line[1];
                        this.dataGridView1.Rows.Add(auxline[1], defaultPose, "NULL", "-1", "-1", "-1", line[1], "-1", defaultPose, "-1");
                    }
                    if (line[0].Equals("CMD_SLEEP"))
                    {
                        string[] auxline = line[0].Split('_');

                        this.dataGridView1.Rows.Add(auxline[1], defaultPose, "NULL", "-1", "-1", "-1", changeWorkzone, "-1", defaultPose, line[1]);
                    }
                    if (line[0].Equals("CMD_CHANGETOOL"))
                    {
                        string[] auxline = line[0].Split('_');

                        this.dataGridView1.Rows.Add(auxline[1], defaultPose, "NULL", "-1", "-1", "-1", changeWorkzone, "-1", defaultPose, "-1", "-1");
                    }
                    cont++;
                }

                int nFilas = dataGridView1.Rows.Count;

                int nEmptyRows = 1000 - nFilas;

                if (nEmptyRows>0)
                {
                    for (int i = 0; i < nEmptyRows; i++)
                    {
                        this.dataGridView1.Rows.Add("INIT", defaultPose, "NULL", "-1", "-1", "-1", "-1", "-1", defaultPose, "-1");
                    }
                }

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    try
                    {
                        switch (dataGridView1.Rows[i].Cells[0].Value.ToString())
                        {
                            case "INIT":
                                dataGridView1.Rows[i].Cells[0].Style.BackColor = Color.CadetBlue;
                                break;
                            case "MOVECMD":
                                dataGridView1.Rows[i].Cells[0].Style.BackColor = Color.Orange;
                                break;
                            case "SETENTRY":
                                dataGridView1.Rows[i].Cells[0].Style.BackColor = Color.Red;
                                break;
                            case "CHANGEWORKZONE":
                                dataGridView1.Rows[i].Cells[0].Style.BackColor = Color.MediumPurple;
                                break;
                            case "CHANGETOOL":
                                dataGridView1.Rows[i].Cells[0].Style.BackColor = Color.Blue;
                                break;
                            case "VALVEAPERTURE":
                                dataGridView1.Rows[i].Cells[0].Style.BackColor = Color.LightBlue;
                                break;
                            case "ENDZONE":
                                dataGridView1.Rows[i].Cells[0].Style.BackColor = Color.Yellow;
                                break;
                            case "FINALIZE":
                                dataGridView1.Rows[i].Cells[0].Style.BackColor = Color.PeachPuff;
                                break;
                            case "SLEEP":
                                dataGridView1.Rows[i].Cells[0].Style.BackColor = Color.Green;
                                break;

                        }
                    }
                    catch (Exception)
                    {

                    }

                    
                }
             
                listademensajes.Clear();
                listademensajesDoc2.Clear();
                this.label1.BackColor = Color.Red;
                this.label1.Text = "DOC1 NO CARGADO";
                this.label2.BackColor = Color.Red;
                this.label2.Text = "DOC2 NO CARGADO";
                this.ShowData.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            #endregion


        }



        //Ruta de archivos de salida para archivos procesados de DoC2
        string g_pathProcessTextDoc2 = g_path+"/"+ "docfilterDoc2.csv";

        /// <summary>
        /// Funcion dedicada a procesar los datos obtenidos del archivo de coordenadas DAT
        /// </summary>
        public void processtextDoc2()
        {
            #region LogicaProcesamientoDeDAT

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
            #endregion

        }




        private void copyAlltoClipboard()
        {
            ////Copy to clipboard
            //dataGridView1.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            //DataObject dataObj = dataGridView1.GetClipboardContent();
            //if (dataObj != null)
            //    Clipboard.SetDataObject(dataObj);


            dataGridView1.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            dataGridView1.SelectAll();
            DataObject dataObj = dataGridView1.GetClipboardContent();
            if (dataObj != null)
                Clipboard.SetDataObject(dataObj);
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Exception Occurred while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

        /// <summary>
        /// Boton dedicado a exportar los datos del DATAGRIDVIEW a un CSV (EN DESARROLLO)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ExportToCSV_Click(object sender, EventArgs e)
        {
            try
            {

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Excel Documents (*.xls)|*.xls";
                sfd.FileName = "Receta_Generada.xls";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    // Copiando contenido de datagridview
                    copyAlltoClipboard();



                    object misValue;

                    misValue = System.Reflection.Missing.Value;


                    Excel.Application xlexcel = new Excel.Application();

                    xlexcel.DisplayAlerts = false;
                    Excel.Workbook xlWorkBook = xlexcel.Workbooks.Add(misValue);
                    Excel.Worksheet xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);


                    Excel.Range rng = xlWorkSheet.get_Range("D:D").Cells;
                    rng.NumberFormat = "@";

                    // pegar copia en hoja de trabajo
                    Excel.Range CR = (Excel.Range)xlWorkSheet.Cells[1, 1];
                    CR.Select();
                    xlWorkSheet.PasteSpecial(CR, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);


                    Excel.Range delRng = xlWorkSheet.get_Range("A:A").Cells;
                    delRng.Delete(Type.Missing);
                    xlWorkSheet.get_Range("A1").Select();

                    // guardado de excel
                    xlWorkBook.SaveAs(sfd.FileName, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                    xlexcel.DisplayAlerts = true;
                    xlWorkBook.Close(true, misValue, misValue);
                    xlexcel.Quit();

                    releaseObject(xlWorkSheet);
                    releaseObject(xlWorkBook);
                    releaseObject(xlexcel);

                    // limpieza de cilpboard y datagridview
                    Clipboard.Clear();
                    this.dataGridView1.ClearSelection();


                }
            }
            catch(Exception ex)
            {
               

                MessageBox.Show(ex.Message,"ERROR",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                closeProcess("EXCEL");
                MessageBox.Show("Se han cerrado las hojas de excel, Vuelva a exportar el documento", "INFO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Funcion para cerrar procesos de halcon
        /// </summary>
        /// <param name="name"></param>
        public void closeProcess(string name)
        {
            foreach (Process proceso in Process.GetProcesses())
            {
                if (proceso.ProcessName == name)
                {
                    proceso.Kill();
                }
            }
        }
    }
}
