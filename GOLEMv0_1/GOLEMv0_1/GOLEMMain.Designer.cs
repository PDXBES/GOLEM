namespace GOLEMv0_1
{
    partial class FormGOLEMMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnCreateSWWM5BASEModel = new System.Windows.Forms.Button();
            this.dgvStartLinks = new System.Windows.Forms.DataGridView();
            this.LinkIDColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvStopLinks = new System.Windows.Forms.DataGridView();
            this.LinkIDColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblStartLinks = new System.Windows.Forms.Label();
            this.lblStopLinks = new System.Windows.Forms.Label();
            this.btnCreateDamagedPipeModelGroup = new System.Windows.Forms.Button();
            this.pnlCancelBackgroundWorker = new System.Windows.Forms.Panel();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.lblModelRunStatus = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorkerSingle = new System.ComponentModel.BackgroundWorker();
            this.btnRunModelsAndProcessReports = new System.Windows.Forms.Button();
            this.btnCreateMassiveDamagePipeModelPair = new System.Windows.Forms.Button();
            this.buttonDWFDischargeModel = new System.Windows.Forms.Button();
            this.lblInputNode = new System.Windows.Forms.Label();
            this.btnCreateSpreadsheet = new System.Windows.Forms.Button();
            this.tbInputNode = new System.Windows.Forms.TextBox();
            this.cboStormType = new System.Windows.Forms.ComboBox();
            this.gOLEMStormsBASEBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.gOLEMDataSet = new GOLEMv0_1.GOLEMDataSet();
            this.lblStormType = new System.Windows.Forms.Label();
            this.gOLEM_Storms_BASETableAdapter = new GOLEMv0_1.GOLEMDataSetTableAdapters.GOLEM_Storms_BASETableAdapter();
            this.btnBreakEverythingInSWMM5BaseModel = new System.Windows.Forms.Button();
            this.btnCompareSWMM5BrokenModels = new System.Windows.Forms.Button();
            this.buttonModifyMannings = new System.Windows.Forms.Button();
            this.buttonBuildCityModels = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStartLinks)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStopLinks)).BeginInit();
            this.pnlCancelBackgroundWorker.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gOLEMStormsBASEBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gOLEMDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCreateSWWM5BASEModel
            // 
            this.btnCreateSWWM5BASEModel.Location = new System.Drawing.Point(394, 291);
            this.btnCreateSWWM5BASEModel.Name = "btnCreateSWWM5BASEModel";
            this.btnCreateSWWM5BASEModel.Size = new System.Drawing.Size(178, 40);
            this.btnCreateSWWM5BASEModel.TabIndex = 0;
            this.btnCreateSWWM5BASEModel.Text = "Create SWMM 5 BASE Model";
            this.btnCreateSWWM5BASEModel.UseVisualStyleBackColor = true;
            this.btnCreateSWWM5BASEModel.Click += new System.EventHandler(this.btnCreateSWWM5BASEModel_Click);
            // 
            // dgvStartLinks
            // 
            this.dgvStartLinks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStartLinks.ColumnHeadersVisible = false;
            this.dgvStartLinks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.LinkIDColumn1});
            this.dgvStartLinks.Location = new System.Drawing.Point(12, 35);
            this.dgvStartLinks.Name = "dgvStartLinks";
            this.dgvStartLinks.RowHeadersVisible = false;
            this.dgvStartLinks.Size = new System.Drawing.Size(142, 250);
            this.dgvStartLinks.TabIndex = 1;
            // 
            // LinkIDColumn1
            // 
            this.LinkIDColumn1.HeaderText = "LinkIDColumn1";
            this.LinkIDColumn1.Name = "LinkIDColumn1";
            // 
            // dgvStopLinks
            // 
            this.dgvStopLinks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStopLinks.ColumnHeadersVisible = false;
            this.dgvStopLinks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.LinkIDColumn2});
            this.dgvStopLinks.Location = new System.Drawing.Point(12, 321);
            this.dgvStopLinks.Name = "dgvStopLinks";
            this.dgvStopLinks.RowHeadersVisible = false;
            this.dgvStopLinks.Size = new System.Drawing.Size(142, 175);
            this.dgvStopLinks.TabIndex = 2;
            // 
            // LinkIDColumn2
            // 
            this.LinkIDColumn2.HeaderText = "LinkIDColumn2";
            this.LinkIDColumn2.Name = "LinkIDColumn2";
            // 
            // lblStartLinks
            // 
            this.lblStartLinks.AutoSize = true;
            this.lblStartLinks.Location = new System.Drawing.Point(12, 19);
            this.lblStartLinks.Name = "lblStartLinks";
            this.lblStartLinks.Size = new System.Drawing.Size(57, 13);
            this.lblStartLinks.TabIndex = 3;
            this.lblStartLinks.Text = "Start Links";
            // 
            // lblStopLinks
            // 
            this.lblStopLinks.AutoSize = true;
            this.lblStopLinks.Location = new System.Drawing.Point(12, 305);
            this.lblStopLinks.Name = "lblStopLinks";
            this.lblStopLinks.Size = new System.Drawing.Size(57, 13);
            this.lblStopLinks.TabIndex = 4;
            this.lblStopLinks.Text = "Stop Links";
            // 
            // btnCreateDamagedPipeModelGroup
            // 
            this.btnCreateDamagedPipeModelGroup.Location = new System.Drawing.Point(160, 376);
            this.btnCreateDamagedPipeModelGroup.Name = "btnCreateDamagedPipeModelGroup";
            this.btnCreateDamagedPipeModelGroup.Size = new System.Drawing.Size(178, 49);
            this.btnCreateDamagedPipeModelGroup.TabIndex = 5;
            this.btnCreateDamagedPipeModelGroup.Text = "Create Damaged Pipe Model Group";
            this.btnCreateDamagedPipeModelGroup.UseVisualStyleBackColor = true;
            this.btnCreateDamagedPipeModelGroup.Click += new System.EventHandler(this.btnCreateDamagedPipeModelGroup_Click);
            // 
            // pnlCancelBackgroundWorker
            // 
            this.pnlCancelBackgroundWorker.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlCancelBackgroundWorker.Controls.Add(this.progressBar1);
            this.pnlCancelBackgroundWorker.Controls.Add(this.lblModelRunStatus);
            this.pnlCancelBackgroundWorker.Controls.Add(this.buttonCancel);
            this.pnlCancelBackgroundWorker.Location = new System.Drawing.Point(12, 204);
            this.pnlCancelBackgroundWorker.Name = "pnlCancelBackgroundWorker";
            this.pnlCancelBackgroundWorker.Size = new System.Drawing.Size(565, 100);
            this.pnlCancelBackgroundWorker.TabIndex = 23;
            this.pnlCancelBackgroundWorker.Visible = false;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(3, 35);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(555, 33);
            this.progressBar1.TabIndex = 2;
            // 
            // lblModelRunStatus
            // 
            this.lblModelRunStatus.Location = new System.Drawing.Point(9, 9);
            this.lblModelRunStatus.Name = "lblModelRunStatus";
            this.lblModelRunStatus.Size = new System.Drawing.Size(246, 41);
            this.lblModelRunStatus.TabIndex = 1;
            this.lblModelRunStatus.Text = "Generating Report";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(3, 74);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(555, 23);
            this.buttonCancel.TabIndex = 0;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted_1);
            // 
            // backgroundWorkerSingle
            // 
            this.backgroundWorkerSingle.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerSingle_DoWork);
            this.backgroundWorkerSingle.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerSingle_RunWorkerCompleted_1);
            // 
            // btnRunModelsAndProcessReports
            // 
            this.btnRunModelsAndProcessReports.Location = new System.Drawing.Point(394, 447);
            this.btnRunModelsAndProcessReports.Name = "btnRunModelsAndProcessReports";
            this.btnRunModelsAndProcessReports.Size = new System.Drawing.Size(178, 49);
            this.btnRunModelsAndProcessReports.TabIndex = 24;
            this.btnRunModelsAndProcessReports.Text = "Run Models and Process Reports";
            this.btnRunModelsAndProcessReports.UseVisualStyleBackColor = true;
            this.btnRunModelsAndProcessReports.Click += new System.EventHandler(this.btnRunModelsAndProcessReports_Click);
            // 
            // btnCreateMassiveDamagePipeModelPair
            // 
            this.btnCreateMassiveDamagePipeModelPair.Location = new System.Drawing.Point(160, 321);
            this.btnCreateMassiveDamagePipeModelPair.Name = "btnCreateMassiveDamagePipeModelPair";
            this.btnCreateMassiveDamagePipeModelPair.Size = new System.Drawing.Size(178, 49);
            this.btnCreateMassiveDamagePipeModelPair.TabIndex = 25;
            this.btnCreateMassiveDamagePipeModelPair.Text = "Create Massive Damage Pipe Model Pair";
            this.btnCreateMassiveDamagePipeModelPair.UseVisualStyleBackColor = true;
            this.btnCreateMassiveDamagePipeModelPair.Click += new System.EventHandler(this.btnCreateMassiveDamagePipeModelPair_Click);
            // 
            // buttonDWFDischargeModel
            // 
            this.buttonDWFDischargeModel.Location = new System.Drawing.Point(399, 35);
            this.buttonDWFDischargeModel.Name = "buttonDWFDischargeModel";
            this.buttonDWFDischargeModel.Size = new System.Drawing.Size(178, 40);
            this.buttonDWFDischargeModel.TabIndex = 26;
            this.buttonDWFDischargeModel.Text = "Create DWF Discharge Model";
            this.buttonDWFDischargeModel.UseVisualStyleBackColor = true;
            this.buttonDWFDischargeModel.Click += new System.EventHandler(this.buttonDWFDischargeModel_Click);
            // 
            // lblInputNode
            // 
            this.lblInputNode.AutoSize = true;
            this.lblInputNode.Location = new System.Drawing.Point(185, 19);
            this.lblInputNode.Name = "lblInputNode";
            this.lblInputNode.Size = new System.Drawing.Size(60, 13);
            this.lblInputNode.TabIndex = 28;
            this.lblInputNode.Text = "Input Node";
            // 
            // btnCreateSpreadsheet
            // 
            this.btnCreateSpreadsheet.Location = new System.Drawing.Point(399, 81);
            this.btnCreateSpreadsheet.Name = "btnCreateSpreadsheet";
            this.btnCreateSpreadsheet.Size = new System.Drawing.Size(178, 40);
            this.btnCreateSpreadsheet.TabIndex = 29;
            this.btnCreateSpreadsheet.Text = "Create Spreadsheet";
            this.btnCreateSpreadsheet.UseVisualStyleBackColor = true;
            this.btnCreateSpreadsheet.Click += new System.EventHandler(this.btnCreateSpreadsheet_Click);
            // 
            // tbInputNode
            // 
            this.tbInputNode.Location = new System.Drawing.Point(188, 36);
            this.tbInputNode.Name = "tbInputNode";
            this.tbInputNode.Size = new System.Drawing.Size(100, 20);
            this.tbInputNode.TabIndex = 30;
            // 
            // cboStormType
            // 
            this.cboStormType.DataSource = this.gOLEMStormsBASEBindingSource;
            this.cboStormType.DisplayMember = "StormType";
            this.cboStormType.FormattingEnabled = true;
            this.cboStormType.Location = new System.Drawing.Point(188, 111);
            this.cboStormType.Name = "cboStormType";
            this.cboStormType.Size = new System.Drawing.Size(171, 21);
            this.cboStormType.TabIndex = 31;
            this.cboStormType.ValueMember = "GOLEM_ID";
            // 
            // gOLEMStormsBASEBindingSource
            // 
            this.gOLEMStormsBASEBindingSource.DataMember = "GOLEM_Storms_BASE";
            this.gOLEMStormsBASEBindingSource.DataSource = this.gOLEMDataSet;
            // 
            // gOLEMDataSet
            // 
            this.gOLEMDataSet.DataSetName = "GOLEMDataSet";
            this.gOLEMDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // lblStormType
            // 
            this.lblStormType.AutoSize = true;
            this.lblStormType.Location = new System.Drawing.Point(185, 95);
            this.lblStormType.Name = "lblStormType";
            this.lblStormType.Size = new System.Drawing.Size(61, 13);
            this.lblStormType.TabIndex = 32;
            this.lblStormType.Text = "Storm Type";
            // 
            // gOLEM_Storms_BASETableAdapter
            // 
            this.gOLEM_Storms_BASETableAdapter.ClearBeforeFill = true;
            // 
            // btnBreakEverythingInSWMM5BaseModel
            // 
            this.btnBreakEverythingInSWMM5BaseModel.Location = new System.Drawing.Point(394, 337);
            this.btnBreakEverythingInSWMM5BaseModel.Name = "btnBreakEverythingInSWMM5BaseModel";
            this.btnBreakEverythingInSWMM5BaseModel.Size = new System.Drawing.Size(178, 40);
            this.btnBreakEverythingInSWMM5BaseModel.TabIndex = 33;
            this.btnBreakEverythingInSWMM5BaseModel.Text = "Break Everything in SWMM 5 BASE Model";
            this.btnBreakEverythingInSWMM5BaseModel.UseVisualStyleBackColor = true;
            this.btnBreakEverythingInSWMM5BaseModel.Click += new System.EventHandler(this.btnBreakEverythingInSWMM5BaseModel_Click);
            // 
            // btnCompareSWMM5BrokenModels
            // 
            this.btnCompareSWMM5BrokenModels.Location = new System.Drawing.Point(394, 385);
            this.btnCompareSWMM5BrokenModels.Name = "btnCompareSWMM5BrokenModels";
            this.btnCompareSWMM5BrokenModels.Size = new System.Drawing.Size(178, 40);
            this.btnCompareSWMM5BrokenModels.TabIndex = 34;
            this.btnCompareSWMM5BrokenModels.Text = "Compare SWMM 5 Broken Models";
            this.btnCompareSWMM5BrokenModels.UseVisualStyleBackColor = true;
            // 
            // buttonModifyMannings
            // 
            this.buttonModifyMannings.Location = new System.Drawing.Point(399, 127);
            this.buttonModifyMannings.Name = "buttonModifyMannings";
            this.buttonModifyMannings.Size = new System.Drawing.Size(178, 40);
            this.buttonModifyMannings.TabIndex = 35;
            this.buttonModifyMannings.Text = "Modify Mannings";
            this.buttonModifyMannings.UseVisualStyleBackColor = true;
            this.buttonModifyMannings.Click += new System.EventHandler(this.buttonModifyMannings_Click);
            // 
            // buttonBuildCityModels
            // 
            this.buttonBuildCityModels.Location = new System.Drawing.Point(160, 447);
            this.buttonBuildCityModels.Name = "buttonBuildCityModels";
            this.buttonBuildCityModels.Size = new System.Drawing.Size(178, 49);
            this.buttonBuildCityModels.TabIndex = 36;
            this.buttonBuildCityModels.Text = "Build City Models";
            this.buttonBuildCityModels.UseVisualStyleBackColor = true;
            this.buttonBuildCityModels.Click += new System.EventHandler(this.buttonBuildCityModels_Click);
            // 
            // FormGOLEMMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(589, 508);
            this.Controls.Add(this.buttonBuildCityModels);
            this.Controls.Add(this.buttonModifyMannings);
            this.Controls.Add(this.btnCompareSWMM5BrokenModels);
            this.Controls.Add(this.btnBreakEverythingInSWMM5BaseModel);
            this.Controls.Add(this.lblStormType);
            this.Controls.Add(this.cboStormType);
            this.Controls.Add(this.tbInputNode);
            this.Controls.Add(this.btnCreateSpreadsheet);
            this.Controls.Add(this.lblInputNode);
            this.Controls.Add(this.buttonDWFDischargeModel);
            this.Controls.Add(this.btnCreateMassiveDamagePipeModelPair);
            this.Controls.Add(this.btnRunModelsAndProcessReports);
            this.Controls.Add(this.pnlCancelBackgroundWorker);
            this.Controls.Add(this.btnCreateDamagedPipeModelGroup);
            this.Controls.Add(this.lblStopLinks);
            this.Controls.Add(this.lblStartLinks);
            this.Controls.Add(this.dgvStopLinks);
            this.Controls.Add(this.dgvStartLinks);
            this.Controls.Add(this.btnCreateSWWM5BASEModel);
            this.Name = "FormGOLEMMain";
            this.Text = "GOLEM v 0.1";
            this.Load += new System.EventHandler(this.FormGOLEMMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvStartLinks)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStopLinks)).EndInit();
            this.pnlCancelBackgroundWorker.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gOLEMStormsBASEBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gOLEMDataSet)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCreateSWWM5BASEModel;
        private System.Windows.Forms.DataGridView dgvStartLinks;
        private System.Windows.Forms.DataGridView dgvStopLinks;
        private System.Windows.Forms.Label lblStartLinks;
        private System.Windows.Forms.Label lblStopLinks;
        private System.Windows.Forms.DataGridViewTextBoxColumn LinkIDColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn LinkIDColumn2;
        private System.Windows.Forms.Button btnCreateDamagedPipeModelGroup;
        private System.Windows.Forms.Panel pnlCancelBackgroundWorker;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label lblModelRunStatus;
        private System.Windows.Forms.Button buttonCancel;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.ComponentModel.BackgroundWorker backgroundWorkerSingle;
        private System.Windows.Forms.Button btnRunModelsAndProcessReports;
        private System.Windows.Forms.Button btnCreateMassiveDamagePipeModelPair;
        private System.Windows.Forms.Button buttonDWFDischargeModel;
        private System.Windows.Forms.Label lblInputNode;
        private System.Windows.Forms.Button btnCreateSpreadsheet;
        private System.Windows.Forms.TextBox tbInputNode;
        private System.Windows.Forms.ComboBox cboStormType;
        private System.Windows.Forms.Label lblStormType;
        private GOLEMDataSet gOLEMDataSet;
        private System.Windows.Forms.BindingSource gOLEMStormsBASEBindingSource;
        private GOLEMDataSetTableAdapters.GOLEM_Storms_BASETableAdapter gOLEM_Storms_BASETableAdapter;
        private System.Windows.Forms.Button btnBreakEverythingInSWMM5BaseModel;
        private System.Windows.Forms.Button btnCompareSWMM5BrokenModels;
        private System.Windows.Forms.Button buttonModifyMannings;
        private System.Windows.Forms.Button buttonBuildCityModels;
    }
}

