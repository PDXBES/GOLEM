using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Word = Microsoft.Office.Interop.Word;
using ESRI.ArcGIS.Output;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;

namespace GOLEMv0_1
{
    public partial class FormGOLEMMain : Form
    {
        DataTable StartLinks = new DataTable();
        DataColumn StartID = new DataColumn("ID", System.Type.GetType("System.Int32"));

        DataTable StopLinks = new DataTable();
        DataColumn StopID = new DataColumn("ID", System.Type.GetType("System.Int32"));

        int? startNodeOBJECTID = 0;
        int hasStartLinkOBJECTIDS = 0;
        int modelID = 1;
        int CompareModelID = 2;
        string decPlaces = "0.0000";
        int newNodeName = -1;
        int StormID = 1;
        string fileName = "";
        int resultsID = 0;
        int createMassiveDamage = 0;
        int massiveDamageProtection = 8;
        int DWFModel = 0;
        string theImagePath = "";

        string folder;
        private static string CONNECTION_STR = "Data Source=BESDBDEV1;Initial Catalog=GOLEM;Trusted_Connection = true;";

        public FormGOLEMMain()
        {
            InitializeComponent();
        }

     private static DataTable CreateDataTable(IEnumerable<long> ids)
     {
         DataTable table = new DataTable();
         table.Columns.Add("ID", typeof(long));
         foreach (long id in ids)
         {
             table.Rows.Add(id);
         }
         return table;
     }

     private void btnCreateDamagedPipeModelGroup_Click(object sender, EventArgs e)
     {
         btnCreateDamagedPipeModelGroup.Enabled = false;
         btnCreateSWWM5BASEModel.Enabled = false;
         dgvStartLinks.Enabled = false;
         dgvStopLinks.Enabled = false;

         StartLinks.Columns.Clear();
         StopLinks.Columns.Clear();

         StartLinks.Columns.Add(StartID);
         StopLinks.Columns.Add(StopID);
         foreach (DataGridViewRow row in dgvStartLinks.Rows)
         {
             if (row.Cells[0].Value == null)
             { }
             else
             {
                 DataRow dataRow = StartLinks.Rows.Add();
                 dataRow[0] = row.Cells[0].Value;
             }
         }

         foreach (DataGridViewRow row in dgvStopLinks.Rows)
         {
             if (row.Cells[0].Value == null)
             { }
             else
             {
                 DataRow dataRow = StopLinks.Rows.Add();
                 dataRow[0] = row.Cells[0].Value;
             }
         }
         try
         {
             FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
             folderBrowserDialog1.ShowDialog();

             //place a copy of the xpx file into a designated folder
             if (folderBrowserDialog1.SelectedPath != "")
             {
                 folder = folderBrowserDialog1.SelectedPath;
                 //[Trace Model]
                 using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
                 {
                     SqlCommand cmd = conn.CreateCommand();
                     cmd.CommandText = "GIS.GOLEM_TRACEBYLINK_BASE";
                     cmd.CommandType = System.Data.CommandType.StoredProcedure;
                     SqlParameter parameterStart;
                     SqlParameter parameterStop;
                     parameterStart = cmd.Parameters.AddWithValue("@StartLinks", StartLinks);
                     parameterStop = cmd.Parameters.AddWithValue("@StopLinks", StopLinks);
                     parameterStart.SqlDbType = SqlDbType.Structured;
                     parameterStart.TypeName = "GIS.IDList";
                     parameterStop.SqlDbType = SqlDbType.Structured;
                     parameterStop.TypeName = "GIS.IDList2";

                     conn.Open();
                     cmd.CommandTimeout = 0;
                     cmd.ExecuteNonQuery();
                 }

                 pnlCancelBackgroundWorker.Visible = true;
                 progressBar1.Style = ProgressBarStyle.Marquee;
                 this.Refresh();
                 backgroundWorker.RunWorkerAsync();
             }
         }

         catch (Exception ex)
         {
             MessageBox.Show("Error Creating Models: " + ex.Message, "Error Creating Models", MessageBoxButtons.OK, MessageBoxIcon.Error);
         }

         btnCreateDamagedPipeModelGroup.Enabled = true;
         btnCreateSWWM5BASEModel.Enabled = true;
         dgvStartLinks.Enabled = true;
         dgvStopLinks.Enabled = true;
     }

     private void btnCreateMassiveDamagePipeModelPair_Click(object sender, EventArgs e)
     {
         createMassiveDamage = 1;
         btnCreateDamagedPipeModelGroup.Enabled = false;
         btnCreateSWWM5BASEModel.Enabled = false;
         dgvStartLinks.Enabled = false;
         dgvStopLinks.Enabled = false;

         StartLinks.Columns.Clear();
         StopLinks.Columns.Clear();

         StartLinks.Columns.Add(StartID);
         StopLinks.Columns.Add(StopID);
         foreach (DataGridViewRow row in dgvStartLinks.Rows)
         {
             if (row.Cells[0].Value == null)
             { }
             else
             {
                 DataRow dataRow = StartLinks.Rows.Add();
                 dataRow[0] = row.Cells[0].Value;
             }
         }

         foreach (DataGridViewRow row in dgvStopLinks.Rows)
         {
             if (row.Cells[0].Value == null)
             { }
             else
             {
                 DataRow dataRow = StopLinks.Rows.Add();
                 dataRow[0] = row.Cells[0].Value;
             }
         }
         try
         {
             FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
             folderBrowserDialog1.ShowDialog();

             //place a copy of the xpx file into a designated folder
             if (folderBrowserDialog1.SelectedPath != "")
             {
                 folder = folderBrowserDialog1.SelectedPath;
                 //[Trace Model]
                 using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
                 {
                     SqlCommand cmd = conn.CreateCommand();
                     cmd.CommandText = "GIS.GOLEM_TRACEBYLINK_BASE";
                     cmd.CommandType = System.Data.CommandType.StoredProcedure;
                     SqlParameter parameterStart;
                     SqlParameter parameterStop;
                     parameterStart = cmd.Parameters.AddWithValue("@StartLinks", StartLinks);
                     parameterStop = cmd.Parameters.AddWithValue("@StopLinks", StopLinks);
                     parameterStart.SqlDbType = SqlDbType.Structured;
                     parameterStart.TypeName = "GIS.IDList";
                     parameterStop.SqlDbType = SqlDbType.Structured;
                     parameterStop.TypeName = "GIS.IDList2";

                     conn.Open();
                     cmd.CommandTimeout = 0;
                     cmd.ExecuteNonQuery();
                 }

                 pnlCancelBackgroundWorker.Visible = true;
                 progressBar1.Style = ProgressBarStyle.Marquee;
                 this.Refresh();
                 backgroundWorker.RunWorkerAsync();
             }
         }

         catch (Exception ex)
         {
             MessageBox.Show("Error Creating Models: " + ex.Message, "Error Creating Models", MessageBoxButtons.OK, MessageBoxIcon.Error);
         }

         btnCreateDamagedPipeModelGroup.Enabled = true;
         btnCreateSWWM5BASEModel.Enabled = true;
         dgvStartLinks.Enabled = true;
         dgvStopLinks.Enabled = true;
     }

     private void btnCreateSWWM5BASEModel_Click(object sender, EventArgs e)
     {
         StormID = (int)(long)cboStormType.SelectedValue;

         btnCreateDamagedPipeModelGroup.Enabled = false;
         btnCreateSWWM5BASEModel.Enabled = false;
         dgvStartLinks.Enabled = false;
         dgvStopLinks.Enabled = false;

         StartLinks.Columns.Clear();
         StopLinks.Columns.Clear();

         StartLinks.Columns.Add(StartID);
         StopLinks.Columns.Add(StopID);
         foreach (DataGridViewRow row in dgvStartLinks.Rows)
         {
             if (row.Cells[0].Value == null)
             { }
             else
             {
                 DataRow dataRow = StartLinks.Rows.Add();
                 dataRow[0] = row.Cells[0].Value;
             }
         }

         foreach (DataGridViewRow row in dgvStopLinks.Rows)
         {
             if (row.Cells[0].Value == null)
             { }
             else
             {
                 DataRow dataRow = StopLinks.Rows.Add();
                 dataRow[0] = row.Cells[0].Value;
             }
         }

         FolderBrowserDialog saveFileDialog1 = new FolderBrowserDialog();
         saveFileDialog1.ShowDialog();
         //place a copy of the xpx file into a designated folder
         if (saveFileDialog1.SelectedPath != "")
         {
             folder = saveFileDialog1.SelectedPath;
             pnlCancelBackgroundWorker.Visible = true;
             progressBar1.Style = ProgressBarStyle.Marquee;
             this.Refresh();
             backgroundWorkerSingle.RunWorkerAsync();
         }



         /*SaveFileDialog saveFileDialog1 = new SaveFileDialog();
         saveFileDialog1.Filter = "Inp File|*.inp";
         saveFileDialog1.Title = "Save an Inp File";
         saveFileDialog1.ShowDialog();
         //place a copy of the xpx file into a designated folder
         if (saveFileDialog1.FileName != "")
         {
             fileName = saveFileDialog1.FileName;
             pnlCancelBackgroundWorker.Visible = true;
             progressBar1.Style = ProgressBarStyle.Marquee;
             this.Refresh();
             backgroundWorkerSingle.RunWorkerAsync();
             
         }*/

         btnCreateDamagedPipeModelGroup.Enabled = true;
         btnCreateSWWM5BASEModel.Enabled = true;
         dgvStartLinks.Enabled = true;
         dgvStopLinks.Enabled = true;
     }

     private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
     {
         SqlDataReader sqlDR;
         int linkID;

         using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
         {
             SqlCommand cmd = conn.CreateCommand();
             cmd.CommandType = System.Data.CommandType.Text;
             cmd.CommandText = "SELECT MAX(ModelID) FROM [GOLEM].[GIS].[GOLEM_TracedLinks_BASE]";
             conn.Open();
             cmd.CommandTimeout = 0;
             modelID = (int)cmd.ExecuteScalar();
         }

         string fileName_base = "GOLEM_" + modelID.ToString() + "_0_" + (100).ToString() + ".inp";
         GolemMethods.CreateSWMM5ModelBASE_DWF(modelID, 0, (decimal)1.0, folder + "\\" + fileName_base, 1, CONNECTION_STR, createMassiveDamage, massiveDamageProtection,decPlaces, StormID, DWFModel);

         //Get a list of the pipes in the trace
         if (createMassiveDamage == 0)
         {
             using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
             {
                 SqlCommand cmd = conn.CreateCommand();
                 cmd.CommandType = System.Data.CommandType.Text;
                 cmd.CommandText = "SELECT LinkID FROM [GOLEM].[GIS].[GOLEM_ModelLinks]";
                 conn.Open();
                 cmd.CommandTimeout = 0;
                 sqlDR = cmd.ExecuteReader();

                 while (sqlDR.Read())
                 {
                     //Create a name for the new model
                     //DO NOT let a pipe have a 0 size.  This will break swmm5.
                     linkID = ((int)sqlDR["LinkID"]);
                     //for (int i = 2; i <= 2; i++)
                     //{
                     string fileName_resize = "GOLEM_" + modelID.ToString() + "_" + linkID.ToString() + "_" + (/*i * */50).ToString() + ".inp";
                     //CreateSWMM5ModelBASE_DWF(modelID, linkID, (decimal)(/*i **/ 0.5), folder + "\\" + fileName_resize, 1);
                     GolemMethods.ResizePipe(folder + "\\" + fileName_base, folder + "\\" + fileName_resize, linkID.ToString(), (float)0.5, decPlaces);
                     //}
                 }
             }
         }
         else
         {
             fileName = "GOLEM_" + modelID.ToString() + "_-1_" + (/*i * */50).ToString() + ".inp";
             GolemMethods.CreateSWMM5ModelBASE_DWF(modelID, -1, (decimal)0.5, folder + "\\" + fileName, 1, CONNECTION_STR, createMassiveDamage, massiveDamageProtection, decPlaces, StormID, DWFModel);
             createMassiveDamage = 0;
         }
         
     }

     private void backgroundWorker_RunWorkerCompleted_1(object sender, RunWorkerCompletedEventArgs e)
     {
         pnlCancelBackgroundWorker.Visible = false;
         if (e.Cancelled)
         {
             MessageBox.Show("Model Creation Cancelled",
                         "GOLEM Model Canceled", MessageBoxButtons.OK, MessageBoxIcon.Warning);
         }
         else if (e.Error != null)
         {
             MessageBox.Show("Error executing GOLEM: " + e.Error.Message,
                         "Error Executing GOLEM", MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
         else
         {
             MessageBox.Show("Successfully executed GOLEM!",
                         "GOLEM Executed Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
         }
     }

     private void backgroundWorkerSingle_DoWork(object sender, DoWorkEventArgs e)
     {
         //folder = System.IO.Path.GetDirectoryName(fileName);

         //[Trace Model]
         using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
         {
             SqlCommand cmd = conn.CreateCommand();
             cmd.CommandText = "GIS.GOLEM_TRACEBYLINK_BASE";
             cmd.CommandType = System.Data.CommandType.StoredProcedure;
             SqlParameter parameterStart;
             SqlParameter parameterStop;
             SqlParameter parameterParentModelID;
             SqlParameter parameterBaseModelID;
             SqlParameter parameterModelLocation;
             SqlParameter parameterStormType;
             SqlParameter parameterVariable1;
             SqlParameter parameterVariable2;
             SqlParameter parameterVariable3;

             parameterStart = cmd.Parameters.AddWithValue("@StartLinks", StartLinks);
             parameterStop = cmd.Parameters.AddWithValue("@StopLinks", StopLinks);
             //parameterParentModelID = cmd.Parameters.AddWithValue("@ParentModelID", null);
             //parameterBaseModelID = cmd.Parameters.AddWithValue("@BaseModelID", null);
             parameterModelLocation = cmd.Parameters.AddWithValue("@ModelLocation", folder);
             parameterStormType = cmd.Parameters.AddWithValue("@StormType", StormID);
             parameterVariable1 = cmd.Parameters.AddWithValue("@Variable1", "0");
             parameterVariable2 = cmd.Parameters.AddWithValue("@Variable2", "0");
             parameterVariable3 = cmd.Parameters.AddWithValue("@Variable3", "0");

             parameterStart.SqlDbType = SqlDbType.Structured;
             parameterStart.TypeName = "GIS.IDList";
             parameterStop.SqlDbType = SqlDbType.Structured;
             parameterStop.TypeName = "GIS.IDList2";

             conn.Open();
             cmd.CommandTimeout = 0;

             SqlParameter retval = cmd.Parameters.Add("@ModelID", SqlDbType.Int);
             retval.Direction = ParameterDirection.ReturnValue;
             cmd.ExecuteNonQuery(); // MISSING
             modelID = (int)cmd.Parameters["@ModelID"].Value;
             //modelID = (int)cmd.ExecuteScalar();
         }
         SqlDataReader sqlDR;
         int linkID;
         string fileName_base = "GOLEM_" + modelID.ToString() + "_0_" + (100).ToString() + ".inp";
         
         GolemMethods.CreateSWMM5ModelBASE_DWF(modelID, 0, (decimal)1.0, folder + "\\" + fileName_base, 1, CONNECTION_STR, createMassiveDamage, massiveDamageProtection, decPlaces, StormID, DWFModel);
     }

     private void backgroundWorkerSingle_RunWorkerCompleted_1(object sender, RunWorkerCompletedEventArgs e)
     {
         pnlCancelBackgroundWorker.Visible = false;
         if (e.Cancelled)
         {
             MessageBox.Show("Model Creation Cancelled",
                         "GOLEM Model Canceled", MessageBoxButtons.OK, MessageBoxIcon.Warning);
         }
         else if (e.Error != null)
         {
             MessageBox.Show("Error executing GOLEM: " + e.Error.Message,
                         "Error Executing GOLEM", MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
         else
         {
             MessageBox.Show("Successfully executed GOLEM!",
                         "GOLEM Executed Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
         }
         DWFModel = 0;
     }

     private void btnRunModelsAndProcessReports_Click(object sender, EventArgs e)
     {
         GolemMethods.RunModelsAndProcessReports(CONNECTION_STR, resultsID);
     }

     private void buttonDWFDischargeModel_Click(object sender, EventArgs e)
     {
         DWFModel = 1;
         btnCreateDamagedPipeModelGroup.Enabled = false;
         btnCreateSWWM5BASEModel.Enabled = false;
         dgvStartLinks.Enabled = false;
         dgvStopLinks.Enabled = false;

         StartLinks.Columns.Clear();
         StopLinks.Columns.Clear();

         StartLinks.Columns.Add(StartID);
         StopLinks.Columns.Add(StopID);
         foreach (DataGridViewRow row in dgvStartLinks.Rows)
         {
             if (row.Cells[0].Value == null)
             { }
             else
             {
                 DataRow dataRow = StartLinks.Rows.Add();
                 dataRow[0] = row.Cells[0].Value;
             }
         }

         foreach (DataGridViewRow row in dgvStopLinks.Rows)
         {
             if (row.Cells[0].Value == null)
             { }
             else
             {
                 DataRow dataRow = StopLinks.Rows.Add();
                 dataRow[0] = row.Cells[0].Value;
             }
         }
         FolderBrowserDialog saveFileDialog1 = new FolderBrowserDialog();
         saveFileDialog1.ShowDialog();
         //place a copy of the xpx file into a designated folder
         if (saveFileDialog1.SelectedPath != "")
         {
             folder = saveFileDialog1.SelectedPath;
             pnlCancelBackgroundWorker.Visible = true;
             progressBar1.Style = ProgressBarStyle.Marquee;
             this.Refresh();
             backgroundWorkerSingle.RunWorkerAsync();
         }

         btnCreateDamagedPipeModelGroup.Enabled = true;
         btnCreateSWWM5BASEModel.Enabled = true;
         dgvStartLinks.Enabled = true;
         dgvStopLinks.Enabled = true;
     }

     void CreateSpreadsheet(String documentPath)
     {
         //Get the data from database into datatable
         using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
         {
             SqlCommand cmd = conn.CreateCommand();
             cmd.CommandType = System.Data.CommandType.Text;
             cmd.CommandText = "SELECT [link_id] " +
                                "  ,[hansen_compkey] " +
                                "  ,[MaximumFlowCFS] " +
                                "  ,[MaxFullFlowRatio] " +
                                "  ,[FullFlow] " +
                                "  ,[PipeShape] " +
                                "  ,[FullArea] " +
                                "  ,[HydRad] " +
                                "  ,[FullDepth] " +
                                "  ,[ResultsID] " +
                                "  ,[DWFDischargeAvailableCFS] " +
                                "  ,[node_name] " +
                                "  FROM [GOLEM].[GIS].GOLEM_View_DIRECTROUTE WHERE link_id <> 0 ORDER BY [level] ASC";
             conn.Open();
             cmd.CommandTimeout = 0;
             
             SqlDataReader sqlDR;
             sqlDR = cmd.ExecuteReader();
            
             Word._Application oWord;
             //Word._Document oDoc;
             oWord = new Word.Application();
             oWord.Visible = false;

             object start = 0, end = 0;
             object oTemplate = "\\\\OBERON\\GRP117\\Templates\\ASM\\BES Letterhead 2013.dot";
             Word.Document document = oWord.Documents.Add(/*ref oTemplate*/Type.Missing, Type.Missing, Type.Missing, Type.Missing);
             //Word.Range rng = document.Range(ref start, ref end);

             //Insert a paragraph at the beginning of the document.
             Word.Paragraph oPara1;
             oPara1 = document.Content.Paragraphs.Add(Type.Missing);
             oPara1.Range.Text = "System Trace Report";
             oPara1.Range.Font.Bold = 1;
             oPara1.Range.Font.Size = 24;
             oPara1.Format.SpaceAfter = 24;    //24 pt spacing after paragraph.
             oPara1.Range.InsertParagraphAfter();

             //Insert a paragraph at the beginning of the document.
             Word.Paragraph oPara2;
             oPara2 = document.Content.Paragraphs.Add(Type.Missing);
             oPara2.Range.Text = "This is a dry weather discharge flow report.  This document is currently under construction.  " +
                                "The results are available in the following table, and a map of the area can be found in Picture 1.  " +
                                "Use of these results are at your own risk until this paragraph is removed or changed, as this entire " +
                                "system is still undergoing testing.";

             oPara2.Range.Font.Bold = 0;
             oPara2.Range.Font.Size = 12;
             oPara2.Format.SpaceAfter = 24;    //24 pt spacing after paragraph.
             oPara2.Range.InsertParagraphAfter();

             // Insert a title for the table and paragraph marks. 
             Word.Paragraph oPara3 = document.Content.Paragraphs.Add(Type.Missing);
             Word.Range rng = oPara3.Range;
             //rng.InsertBefore("Traced Pipes");
             rng.Font.Name = "Verdana";
             rng.Font.Bold = 0;
             rng.Font.Size = 12;
             rng.InsertParagraphAfter();
             rng.InsertParagraphAfter();
             rng.SetRange(rng.End, rng.End);

             // Add the table.
             rng.Tables.Add(oPara3.Range, 1, 5, Type.Missing, Type.Missing);

             // Format the table and apply a style. 
             Word.Table tbl = document.Tables[1];
             tbl.Range.Font.Bold = 0;
             tbl.Range.Font.Size = 12;
             tbl.Columns.DistributeWidth();

             object styleName = "Table Professional";
             tbl.set_Style(ref styleName);
             tbl.Rows[1].HeadingFormat = 0;

             // Insert document properties into cells. 
             tbl.Cell(1, 1).Range.Text = "HansenID";
             tbl.Cell(1, 2).Range.Text = "PipeShape";
             tbl.Cell(1, 3).Range.Text = "Full Area (ft2)";
             tbl.Cell(1, 4).Range.Text = "Hyd. Radius";
             tbl.Cell(1, 5).Range.Text = "Allowable DWF discharge, CFS";

             int rows = 2;
             while (sqlDR.Read())
             {
                 Word.Row theRow = tbl.Rows.Add(Type.Missing);
                 tbl.Cell(rows, 1).Range.Text = (sqlDR.GetString(11)).ToString();
                 tbl.Cell(rows, 2).Range.Text = (sqlDR.GetString(5)).ToString();
                 tbl.Cell(rows, 3).Range.Text = (sqlDR.GetDouble(6)).ToString();
                 tbl.Cell(rows, 4).Range.Text = (sqlDR.GetDouble(7)).ToString();
                 tbl.Cell(rows, 5).Range.Text = (sqlDR.GetDouble(10)).ToString();
                 theRow.SetHeight(15, Word.WdRowHeightRule.wdRowHeightExactly);
                 rows = rows + 1;
             }
             
             //Insert JEPG of trace into document
             /*object myFalse = false;
             object myTrue = true;
             object myEndOfDoc = "\\endofdoc";
             // Set the position where the image will be placed
             object myImageRange = document.Bookmarks[myEndOfDoc].Range;
             // Set the file of the picture
             string imagePath = theImagePath;
             // Add the picture to the document
             Word.InlineShape ILS = document.InlineShapes.AddPicture(imagePath, ref myFalse, ref myTrue, ref myImageRange);
             ILS.Borders.Enable = 1;
             ILS.Select();
             object oLable = oWord.CaptionLabels.Add(": System DWF Trace");
             object caption = oWord.CaptionLabels["Picture"];

             oWord.Selection.InsertCaption(ref caption, ref oLable, Type.Missing, Type.Missing, Type.Missing);
             
             */
             document.SaveAs(documentPath + "\\DWF_Report.doc");
             document.Close(null, null, null);
             //oWord.DisplayAlerts = Word.WdAlertLevel.wdAlertsNone;
             //oWord.Quit();
             /*if (document != null)
                 System.Runtime.InteropServices.Marshal.ReleaseComObject(document);
             if (oWord != null)
                 System.Runtime.InteropServices.Marshal.ReleaseComObject(oWord);*/
             document = null;
             oWord = null;
             GC.Collect(); // force final cleanup!
         }
     }

     private void btnCreateSpreadsheet_Click(object sender, EventArgs e)
     {
         //Check to see if the Node indicated exists within the trace
         //if the node does not exist, then don't create the document.
         //Run the GOLEM_DirectRoute procedure to find the route
         //from the indicated node to the most downstream pipe of the
         //current trace, the procedure should fill a table called
         //GOLEM_DirectRoute, which is what a variant of GOLEM_VIEW_LINKSRESULTS
         //will join on.
         //The view will be called GOLEM_ViewResults_DirectRoute 
         //This new view should be added to the MXD.
         if (checkForNodeInTrace(tbInputNode.Text) == true)
         {
             runGolemDirectRoute(tbInputNode.Text);
             //Get the folder the user wants to output to
             FolderBrowserDialog fbd = new FolderBrowserDialog();
             if (fbd.ShowDialog() == DialogResult.OK)
             {
                 folder = fbd.SelectedPath;
                 //CreateJPEGFromActiveView();
                 theImagePath = GolemMXDCreator.CreateJPEGFromActiveView(folder, theImagePath);
                 if (theImagePath.CompareTo("") != 0)
                 {
                     CreateSpreadsheet(folder);
                 }
             }
         }
         else
         {
             MessageBox.Show("Node does not exist in the most recent trace.");
         }
     }

     public bool checkForNodeInTrace(string nodeName)
     {
         using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
         {
             SqlCommand cmd = conn.CreateCommand();
             cmd.CommandType = System.Data.CommandType.Text;
             cmd.CommandText = "SELECT COUNT(*) " +
                               " FROM	[GOLEM].[GIS].[GOLEM_TracedNodes_BASE] " +
                               " WHERE	node_name = '" + nodeName + "' AND node_name NOT LIKE '%XXXX%'";
             conn.Open();
             cmd.CommandTimeout = 0;

             int sqlDR;
             sqlDR = (int)cmd.ExecuteScalar();

             if (sqlDR > 0)
             {
                 return true;
             }
             else
             {
                 return false;
             }
         }

         return false;
     }

     public bool runGolemDirectRoute(string nodeName)
     {
         //[Trace Model]
         using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
         {
             SqlCommand cmd = conn.CreateCommand();
             cmd.CommandText = "GIS.GOLEM_TRACEBYNODEDOWN_BASE";
             cmd.CommandType = System.Data.CommandType.StoredProcedure;
             SqlParameter parameterStart;
             parameterStart = cmd.Parameters.AddWithValue("@StartNode", nodeName);

             conn.Open();
             cmd.CommandTimeout = 0;
             cmd.ExecuteNonQuery();
         }
         return true;
     }

     private void FormGOLEMMain_Load(object sender, EventArgs e)
     {
         // TODO: This line of code loads data into the 'gOLEMDataSet.GOLEM_Storms_BASE' table. You can move, or remove it, as needed.
         this.gOLEM_Storms_BASETableAdapter.Fill(this.gOLEMDataSet.GOLEM_Storms_BASE);
     }

     private void btnBreakEverythingInSWMM5BaseModel_Click(object sender, EventArgs e)
     {
         string fileName_base = "GOLEM_" + modelID.ToString() + "_0_" + (100).ToString() + ".inp";
         SqlDataReader sqlDR;// = new SqlDataReader();
         int linkID = 0;
         Int64 newModelID = 0;
         createMassiveDamage = 0;

         //Get a list of the pipes in the trace
         using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
         {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "SELECT LinkID FROM [GOLEM].[GIS].[GOLEM_ModelLinks]";
            conn.Open();
            cmd.CommandTimeout = 0;
            sqlDR = cmd.ExecuteReader();

            while (sqlDR.Read())
            {
                linkID = ((int)sqlDR["LinkID"]);
                //add a new model to GOLEM_Models using this info
                //ModelName
                //ModelDesc
                //ParentModelID
                //BaseModelID
                //ModelDate
                //ModelerName
                //ModelLocation
                //StormType
                //Variable1
                //Variable2
                //Variable3
                using (SqlConnection conn2 = new SqlConnection(CONNECTION_STR))
                {
                    SqlCommand cmd2 = conn2.CreateCommand();
                    cmd2.CommandType = System.Data.CommandType.Text;
                    cmd2.CommandText = "INSERT INTO [GOLEM].[GIS].GOLEM_Models " +
                    "(" +
                        "[ModelName]," +
                        "[ModelDesc]," +
                        "[ParentModelID]," +
                        "[BaseModelID]," +
                        "[ModelDate]," +
                        "[ModelerName]," +
                        "[ModelLocation]," +
                        "[StormType]," +
                        "[Variable1]," +
                        "[Variable2]," +
                        "[Variable3]) OUTPUT INSERTED.GOLEMKEY " +
                        "VALUES ('GOLEM PipeBreak', 'Single pipe modified to 50% less capacity', " + modelID.ToString() + "," + modelID.ToString() + ",GETDATE(), SUSER_NAME(), '" +
                        folder + "', " + StormID.ToString() + ", '" + linkID.ToString() + "', '50', '0') ";
                    conn2.Open();
                    cmd2.CommandTimeout = 0;
                    newModelID = (Int64)cmd2.ExecuteScalar();

                    //Create a name for the new model
                    //DO NOT let a pipe have a 0 size.  This will break swmm5.
                    string fileName_resize = "GOLEM_" + newModelID.ToString() + "_" + linkID.ToString() + "_" + (/*i * */50).ToString() + ".inp";
                    GolemMethods.ResizePipe(folder + "\\" + fileName_base, folder + "\\" + fileName_resize, linkID.ToString(), (float)0.5, decPlaces);
                }
            }
         }
      }

     private void MM5BrokenModels_Click(object sender, EventArgs e)
     {
         //Get the results models ready
         using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
         {
             SqlCommand cmd = conn.CreateCommand();
             cmd.CommandType = System.Data.CommandType.Text;
             cmd.CommandText  = " DELETE FROM [GOLEM].[GIS].[GOLEM_UserResultsCurrent] WHERE UserName = SUSER_NAME();";
             cmd.CommandText += " INSERT INTO [GOLEM].[GIS].[GOLEM_UserResultsCurrent] (UserName, UserID, ResultsID, Primacy) ";
             cmd.CommandText += " VALUES (SUSER_NAME(), SUSER_ID(), NULL, 0); ";
             cmd.CommandText += " UPDATE [GOLEM].[GIS].[GOLEM_UserResultsCurrent] ";
             cmd.CommandText += " SET ResultsID = [GOLEM_RESULTS].GOLEMKEY FROM [GOLEM].[GIS].[GOLEM_RESULTS] WHERE ModelID = " + modelID.ToString();
             cmd.CommandText += " AND UserName = SUSER_NAME() ";
             conn.Open();
             cmd.CommandTimeout = 0;
             cmd.ExecuteNonQuery();
         }
     }

     private void buttonModifyMannings_Click(object sender, EventArgs e)
     {
         string fileName_base = "GOLEM_" + modelID.ToString() + "_0_" + (100).ToString() + ".inp";
         SqlDataReader sqlDR;// = new SqlDataReader();
         int linkID = 0;
         Int64 newModelID = 0;
         createMassiveDamage = 0;

         //Modify the mannings N value for all of the pipes right off
         using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
         {
             int nInt = 8;
             for(double N = 0.008; N < .02; N = N + 0.001)
             {
                 using (SqlConnection conn2 = new SqlConnection(CONNECTION_STR))
                 {
                     SqlCommand cmd2 = conn2.CreateCommand();
                     cmd2.CommandType = System.Data.CommandType.Text;
                     cmd2.CommandText = "INSERT INTO [GOLEM].[GIS].GOLEM_Models " +
                     "(" +
                         "[ModelName]," +
                         "[ModelDesc]," +
                         "[ParentModelID]," +
                         "[BaseModelID]," +
                         "[ModelDate]," +
                         "[ModelerName]," +
                         "[ModelLocation]," +
                         "[StormType]," +
                         "[Variable1]," +
                         "[Variable2]," +
                         "[Variable3]) OUTPUT INSERTED.GOLEMKEY " +
                         "VALUES ('GOLEM Mannings Change', 'All Mannings Values Changed', " + modelID.ToString() + "," + modelID.ToString() + ",GETDATE(), SUSER_NAME(), '" +
                         folder + "', " + StormID.ToString() + ", '" + "Mannings" + "', "+ N.ToString() +", '0') ";
                     conn2.Open();
                     cmd2.CommandTimeout = 0;
                     newModelID = (Int64)cmd2.ExecuteScalar();

                     //Create a name for the new model
                     //DO NOT let a pipe have a 0 size.  This will break swmm5.
                     string fileName_resize = "GOLEM_" + newModelID.ToString() + "_" + "0" + "_" + nInt.ToString() + ".inp";
                     GolemMethods.ModifyMannings(folder + "\\" + fileName_base, folder + "\\" + fileName_resize, "0", (float)N, decPlaces);
                     nInt++;
                 }
             }
         }
     }

     private void buttonBuildCityModels_Click(object sender, EventArgs e)
     {
         string fileName_base = "GOLEM_" + modelID.ToString() + "_0_" + (100).ToString() + ".inp";
         SqlDataReader sqlDR;// = new SqlDataReader();
         int linkID = 0;
         Int64 newModelID = 0;
         createMassiveDamage = 0;

         StormID = (int)(long)cboStormType.SelectedValue;

         FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();

         folderBrowserDialog1.ShowDialog();

         if (folderBrowserDialog1.SelectedPath != "")
         {
             //Get a list of all the start pipes for the whole city
             //Get the list of pipes that are entryways to the interceptors
             using (SqlConnection conn = new SqlConnection(CONNECTION_STR))
             {
                 SqlCommand cmd = conn.CreateCommand();
                 cmd.CommandType = System.Data.CommandType.Text;
                 cmd.CommandText = "SELECT link_id FROM [GOLEM].[GIS].[GOLEM_VIEW_HANSENSTARTPIPES]";
                 conn.Open();
                 cmd.CommandTimeout = 0;
                 sqlDR = cmd.ExecuteReader();

                 StartLinks.Columns.Add(StartID);
                 StopLinks.Columns.Add(StopID);

                 while (sqlDR.Read())
                 {
                     linkID = ((int)sqlDR["link_id"]);
                     using (SqlConnection conn2 = new SqlConnection(CONNECTION_STR))
                     {
                         
                         using (SqlConnection connX = new SqlConnection(CONNECTION_STR))
                         {
                             SqlCommand cmdX = connX.CreateCommand();
                             cmdX.CommandText = "GIS.GOLEM_TRACEBYLINK_BASE";
                             cmdX.CommandType = System.Data.CommandType.StoredProcedure;
                             SqlParameter parameterStart;
                             SqlParameter parameterStop;
                             SqlParameter parameterModelLocation;
                             SqlParameter parameterStormType;
                             SqlParameter parameterVariable1;
                             SqlParameter parameterVariable2;
                             SqlParameter parameterVariable3;

                             StartLinks.Clear();
                             StopLinks.Clear();

                             

                             DataRow dataRow = StartLinks.Rows.Add();
                             dataRow[0] = linkID;

                             parameterStart = cmdX.Parameters.AddWithValue("@StartLinks", StartLinks);
                             parameterStop = cmdX.Parameters.AddWithValue("@StopLinks", StopLinks);
                             //parameterParentModelID = cmd.Parameters.AddWithValue("@ParentModelID", null);
                             //parameterBaseModelID = cmd.Parameters.AddWithValue("@BaseModelID", null);
                             parameterModelLocation = cmdX.Parameters.AddWithValue("@ModelLocation", folderBrowserDialog1.SelectedPath);
                             parameterStormType = cmdX.Parameters.AddWithValue("@StormType", StormID);
                             parameterVariable1 = cmdX.Parameters.AddWithValue("@Variable1", "0");
                             parameterVariable2 = cmdX.Parameters.AddWithValue("@Variable2", "0");
                             parameterVariable3 = cmdX.Parameters.AddWithValue("@Variable3", "0");

                             parameterStart.SqlDbType = SqlDbType.Structured;
                             parameterStart.TypeName = "GIS.IDList";
                             parameterStop.SqlDbType = SqlDbType.Structured;
                             parameterStop.TypeName = "GIS.IDList2";

                             connX.Open();
                             cmdX.CommandTimeout = 0;

                             SqlParameter retval = cmdX.Parameters.Add("@ModelID", SqlDbType.Int);
                             retval.Direction = ParameterDirection.ReturnValue;
                             cmdX.ExecuteNonQuery(); // MISSING
                             newModelID = (int)cmdX.Parameters["@ModelID"].Value;
                             //modelID = (int)cmd.ExecuteScalar();
                         }


















                         //Create a name for the new model
                         string fileName_thisModel = "GOLEM_" + newModelID.ToString() + "_" + linkID.ToString() + "_" + (0).ToString() + ".inp";

                         //Create the model text file.
                         GolemMethods.CreateSWMM5ModelBASE_DWF((int)newModelID, 0, 0, folderBrowserDialog1.SelectedPath 
                             + "\\" + fileName_thisModel, 0, CONNECTION_STR, 0, 0, decPlaces, StormID, 0);

                         //Add a new GOLEM_RESULTS line, and get the key ID for that new line
                         using (SqlConnection conn3 = new SqlConnection(CONNECTION_STR))
                         {
                             SqlCommand cmd3 = conn3.CreateCommand();
                             cmd3.CommandType = System.Data.CommandType.Text;
                             cmd3.CommandText = "INSERT INTO [GOLEM].[GIS].GOLEM_RESULTS ([ModelID], [Variable1], [Variable2], [Description]) " +
                                                 " OUTPUT INSERTED.GOLEMKEY " +
                                                 " VALUES (" + newModelID.ToString() + "," + linkID.ToString() + "," + (0).ToString() + "," + "'" + "GOLEM_" + newModelID.ToString() + "_" + linkID.ToString() + "_" + (0).ToString() + "_.rpt') ";
                             conn3.Open();
                             cmd3.CommandTimeout = 0;
                             resultsID = (int)cmd3.ExecuteScalar();
                         }

                         //Run this model and analyze results.
                         GolemMethods.RunModelsAndProcessReportsAuto(CONNECTION_STR, resultsID, folderBrowserDialog1.SelectedPath + "\\" + "GOLEM_" + newModelID.ToString() + "_" + linkID.ToString() + "_" + (0).ToString() + ".inp");
                         //Create a jpg output of the results.
                     }
                 }
             }
         }
      }
   }
}

