namespace sistemadecomandos2
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.INSTRUCT_TYPE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TARGET_POS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MOVETYPE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MOVEVEL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TOOLNUM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VALVEAPERTURE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WORKZONE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BASENUM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CIRCAUXPOS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SLEEPTIME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RECETA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.INSTRUCT_TYPE,
            this.TARGET_POS,
            this.MOVETYPE,
            this.MOVEVEL,
            this.TOOLNUM,
            this.VALVEAPERTURE,
            this.WORKZONE,
            this.BASENUM,
            this.CIRCAUXPOS,
            this.SLEEPTIME,
            this.RECETA});
            this.dataGridView1.Location = new System.Drawing.Point(12, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(1430, 547);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // INSTRUCT_TYPE
            // 
            this.INSTRUCT_TYPE.HeaderText = "INSTRUCT_TYPE";
            this.INSTRUCT_TYPE.MinimumWidth = 6;
            this.INSTRUCT_TYPE.Name = "INSTRUCT_TYPE";
            this.INSTRUCT_TYPE.Width = 125;
            // 
            // TARGET_POS
            // 
            this.TARGET_POS.HeaderText = "TARGET_POS";
            this.TARGET_POS.MinimumWidth = 6;
            this.TARGET_POS.Name = "TARGET_POS";
            this.TARGET_POS.Width = 125;
            // 
            // MOVETYPE
            // 
            this.MOVETYPE.HeaderText = "MOVETYPE";
            this.MOVETYPE.MinimumWidth = 6;
            this.MOVETYPE.Name = "MOVETYPE";
            this.MOVETYPE.Width = 125;
            // 
            // MOVEVEL
            // 
            this.MOVEVEL.HeaderText = "MOVEVEL";
            this.MOVEVEL.MinimumWidth = 6;
            this.MOVEVEL.Name = "MOVEVEL";
            this.MOVEVEL.Width = 125;
            // 
            // TOOLNUM
            // 
            this.TOOLNUM.HeaderText = "TOOLNUM";
            this.TOOLNUM.MinimumWidth = 6;
            this.TOOLNUM.Name = "TOOLNUM";
            this.TOOLNUM.Width = 125;
            // 
            // VALVEAPERTURE
            // 
            this.VALVEAPERTURE.HeaderText = "VALVEAPERTURE";
            this.VALVEAPERTURE.MinimumWidth = 6;
            this.VALVEAPERTURE.Name = "VALVEAPERTURE";
            this.VALVEAPERTURE.Width = 125;
            // 
            // WORKZONE
            // 
            this.WORKZONE.HeaderText = "WORKZONE";
            this.WORKZONE.MinimumWidth = 6;
            this.WORKZONE.Name = "WORKZONE";
            this.WORKZONE.Width = 125;
            // 
            // BASENUM
            // 
            this.BASENUM.HeaderText = "BASENUM";
            this.BASENUM.MinimumWidth = 6;
            this.BASENUM.Name = "BASENUM";
            this.BASENUM.Width = 125;
            // 
            // CIRCAUXPOS
            // 
            this.CIRCAUXPOS.HeaderText = "CIRCAUXPOS";
            this.CIRCAUXPOS.MinimumWidth = 6;
            this.CIRCAUXPOS.Name = "CIRCAUXPOS";
            this.CIRCAUXPOS.Width = 125;
            // 
            // SLEEPTIME
            // 
            this.SLEEPTIME.HeaderText = "SLEEPTIME";
            this.SLEEPTIME.MinimumWidth = 6;
            this.SLEEPTIME.Name = "SLEEPTIME";
            this.SLEEPTIME.Width = 125;
            // 
            // RECETA
            // 
            this.RECETA.HeaderText = "RECETA";
            this.RECETA.MinimumWidth = 6;
            this.RECETA.Name = "RECETA";
            this.RECETA.Width = 125;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 590);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(154, 61);
            this.button1.TabIndex = 2;
            this.button1.Text = "Cargar Archivo";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(225, 590);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(154, 61);
            this.button2.TabIndex = 3;
            this.button2.Text = "Limpiar Visor";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1782, 723);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dataGridView1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn INSTRUCT_TYPE;
        private System.Windows.Forms.DataGridViewTextBoxColumn TARGET_POS;
        private System.Windows.Forms.DataGridViewTextBoxColumn MOVETYPE;
        private System.Windows.Forms.DataGridViewTextBoxColumn MOVEVEL;
        private System.Windows.Forms.DataGridViewTextBoxColumn TOOLNUM;
        private System.Windows.Forms.DataGridViewTextBoxColumn VALVEAPERTURE;
        private System.Windows.Forms.DataGridViewTextBoxColumn WORKZONE;
        private System.Windows.Forms.DataGridViewTextBoxColumn BASENUM;
        private System.Windows.Forms.DataGridViewTextBoxColumn CIRCAUXPOS;
        private System.Windows.Forms.DataGridViewTextBoxColumn SLEEPTIME;
        private System.Windows.Forms.DataGridViewTextBoxColumn RECETA;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}

