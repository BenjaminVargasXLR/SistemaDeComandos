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
        //path en c de archivos procesados
        public static string g_path = "C:/FolderFilesSystemComands";
        //Lista de carga de docs
        Dictionary<string, string> docs = new Dictionary<string, string>() {
            {"doc1","" },
            {"doc2","" }
        };
        
   

       

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
                //Limpieza de datos remanentes en datagridview 
                dataGridView1.Rows.Clear();
                //bloqueo de boton para exportar excel
                this.btn_ExportToCSV.Enabled = false;

                //Dialogo de captura para ruta a archivo que se cargara al sistema
                if (g_file.ShowDialog().Equals(DialogResult.OK))
                {
                    //si el archivo se llama doc1.csv se cargara al sistema
                    if (g_file.FileName.Contains("doc1.csv"))
                    {
                        //Funcion para procesar el texto de doc1
                        processtextDoc1();
                        //Funcion para segundo procesamiento en memoria
                        GetComands();
                        //Cambio de label a doc1 cargado
                        this.label1.BackColor = Color.Green;
                        this.label1.Text = "Doc1 SRC CARGADO";
                    }
                    //si el archivo se llama doc2.csv se cargara al sistema
                    if (g_file.FileName.Contains("doc2.csv"))
                    {
                        //Funcion para procesar el texto de doc2
                        processtextDoc2();
                        //funcion para segundo procesamiento en memoria
                        getComandsDoc2();
                        //Cambio de label a doc2 cargado
                        this.label2.BackColor = Color.Green;
                        this.label2.Text = "Doc2 DAT CARGADO";
                    }

                }
               //Funcion que guarda los nombres de los archivos en un diccionaio
                this.avilableButtonShowMessage();
                //seteo del nombre del archivo cargado a empty
                g_file.FileName = string.Empty;
            }
            catch (Exception ex)
            {
                this.label1.BackColor = Color.Red;
                this.label1.Text = "DOC1 NO CARGADO";
                this.label2.BackColor = Color.Red;
                this.label2.Text = "DOC2 NO CARGADO";
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                closeProcess("EXCEL");
                MessageBox.Show("Se han cerrado las hojas de excel, Vuelva a cargar el documento", "INFO", MessageBoxButtons.OK, MessageBoxIcon.Information);
              

            }
            #endregion

        }




        //Ruta de archivo de salida para archivos procesados de DOC1
        string g_pathProcessTextDoc1 = g_path+"/"+"docfilterDoc1.csv";


       
        /// <summary>
        /// Función que elimina texto no necesario del archivo .source 
        /// </summary>
        public void processtextDoc1()
        {
            #region LogicaProcesamientodeSRC
            string line = null;
            bool encontrado = false;

            try
            {
                //Lectura de archivo
                using (StreamReader reader = new StreamReader(g_file.FileName))
                {
                    //Escritura de archivo
                    using (StreamWriter writer = new StreamWriter(g_pathProcessTextDoc1))
                    {
                        //Mientas la linea no sea nula
                        while ((line = reader.ReadLine()) != null)
                        {
                            if (!line.Contains("START_RECIPE = 1") && !encontrado)
                            {

                                continue;
                            }

                            encontrado = true;

                            if (encontrado)
                            {
                                //Si la linea no contiene alguna de las condiciones pasa de largo a la siguiente linea
                                if (!line.Contains(";FOLD PTP") && !line.Contains(";FOLD LIN") && !line.Contains(";FOLD CIRC") && !line.Contains("CMD_SETENTRY") && !line.Contains("CMD_INIT") && !line.Contains("CMD_CHANGEWORKZONE") && !line.Contains("CMD_CHANGETOOL") && !line.Contains("CMD_VALVEAPERTURE") && !line.Contains("CMD_SLEEP") && !line.Contains("CMD_ENDZONE") && !line.Contains("CMD_FINALIZE"))
                                    continue;

                                line = line.Trim('"');
                                line = line.Replace('=', ' ');
                                line = line.Trim();
                                line = line.Replace(' ', ';');

                                writer.WriteLine(line);
                            }

                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                closeProcess("EXCEL");
                MessageBox.Show("Se han cerrado las hojas de excel, Vuelva a cargar el documento", "INFO", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                //Obtencion de las lineas desde el archivo doc1
                List<string> lines = File.ReadAllLines(g_pathProcessTextDoc1).ToList();
                //Contador para usar en indices de arreglo
                int cont = 0;
                string text = String.Empty;

               
                //Se recorre la lista si se cumple algunas de las condiciones se guarda el mensaje en la lista global
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
                    //Si el comando es cmd_setentry almacena la información de la linea siguiente
                    if (line[0].Equals("CMD_SETENTRY"))
                    {
                        if (!String.IsNullOrEmpty(line[3]))
                        {

                            if (!line[3].Equals("0"))
                            {
                                //Linea siguiente a setentry
                                //Variable de texto que almacena los datos obtenidos para setentry
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
                        //se guardan todos los datos que no sean para setentry y que sea comandos de movimientos
                        if (!line[3].Equals(data[3]))
                        {
                            listademensajes.Add(line[2] + ';' + line[3] + ';' + line[7] + ';' + line[10] + ';' + line[11] + '\n');
                        }



                    }
                    //Contador para almacenar los datos en el indice
                    cont++;
                }

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
        /// Funcion que consulta y devuelve un valor de coordenadas de la lista de coordendas (DAT)
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
        /// Funcion que habilita boton de mostrar datos cuando los documentos necesarios son cargados(usa un diccionario)
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

            //se eliminan las primeras 6 filas de la lista en memoria
                //listademensajes.RemoveAt(0);
                //listademensajes.RemoveAt(0);
                //listademensajes.RemoveAt(0);
                //listademensajes.RemoveAt(0);
                //listademensajes.RemoveAt(0);
                //listademensajes.RemoveAt(0);
                // se limpia datagridview con data remanente
                this.dataGridView1.Rows.Clear();
                
                string changeWorkzone = string.Empty;
                //Posicion por defecto a variables que no tienen posición
                string defaultPose = "X 0,Y 0,Z 0,A 0,B 0,C 0,S 0,T 0,E1 0,E2 0.0,E3 0.0,E4 0.0,E5 0.0,E6 0.0";

                int cont = 0;

                

                //foreach para agregar los datos a datagridview
                foreach (var item in listademensajes)
                {


                    string[] line = item.Split(';');
                    //Si el datagridview no tiene dato el boton de mostrar se desabilita
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

                //obtencion de cantidad de filas en datagridview
                int nFilas = dataGridView1.Rows.Count;

                //cantidad de filas vacias segun diferencia de 1000 lineas
                int nEmptyRows = 1000 - nFilas;

                //si la diferencia es mayor que 0 se llenan el resto de las lineas con data por default
                if (nEmptyRows>0)
                {
                    for (int i = 0; i < nEmptyRows; i++)
                    {
                        this.dataGridView1.Rows.Add("INIT", defaultPose, "NULL", "-1", "-1", "-1", "-1", "-1", defaultPose, "-1");
                    }
                }

                //Colores en fila segun tipo de comando
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
                        //catch vacio puesto que la ultima fila del datagridview es vacia y genera un error
                    }

                    
                }
                //se limpian las listas con datos de los documentos
                listademensajes.Clear();
                listademensajesDoc2.Clear();
                //se setean los botones para volver a ingresar nuevos archivos
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

                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                closeProcess("EXCEL");
                MessageBox.Show("Se han cerrado las hojas de excel, Vuelva a cargar el documento", "INFO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            #endregion

        }



        //Funcion que copia todo el contenido de datagridview
        private void copyAlltoClipboard()
        {
            dataGridView1.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            dataGridView1.SelectAll();
            DataObject dataObj = dataGridView1.GetClipboardContent();
            if (dataObj != null)
                Clipboard.SetDataObject(dataObj);
        }

        //Función que libera cualquier objeto al momento de terminar de crear el excel
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
                MessageBox.Show("Excepcion ocurrida al mientras se libero un objeto " + ex.ToString());
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
