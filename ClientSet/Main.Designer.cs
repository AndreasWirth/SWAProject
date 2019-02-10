namespace Client
{
    partial class Main
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
            this.button1 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.lbWriteableParameters = new System.Windows.Forms.Label();
            this.dgvReadableParameters = new System.Windows.Forms.DataGridView();
            this.lbReadableParameters = new System.Windows.Forms.Label();
            this.cbCommands = new System.Windows.Forms.ComboBox();
            this.lbSelectedCommand = new System.Windows.Forms.Label();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnSendSelCommand = new System.Windows.Forms.Button();
            this.btnRequestFireKey = new System.Windows.Forms.Button();
            this.btnUpdateAcPara = new System.Windows.Forms.Button();
            this.pbConnection = new System.Windows.Forms.ProgressBar();
            this.waitingTimer = new System.Windows.Forms.Timer(this.components);
            this.bwWaitForConnection = new System.ComponentModel.BackgroundWorker();
            this.btnReleaseFireKey = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReadableParameters)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(626, 498);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(212, 30);
            this.button1.TabIndex = 0;
            this.button1.Text = "AndisTestButton";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6});
            this.dataGridView1.Location = new System.Drawing.Point(14, 328);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(500, 200);
            this.dataGridView1.TabIndex = 1;
            // 
            // lbWriteableParameters
            // 
            this.lbWriteableParameters.AutoSize = true;
            this.lbWriteableParameters.Location = new System.Drawing.Point(11, 308);
            this.lbWriteableParameters.Name = "lbWriteableParameters";
            this.lbWriteableParameters.Size = new System.Drawing.Size(141, 17);
            this.lbWriteableParameters.TabIndex = 2;
            this.lbWriteableParameters.Text = "Writable Parameters:";
            // 
            // dgvReadableParameters
            // 
            this.dgvReadableParameters.AllowUserToAddRows = false;
            this.dgvReadableParameters.AllowUserToDeleteRows = false;
            this.dgvReadableParameters.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgvReadableParameters.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReadableParameters.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3});
            this.dgvReadableParameters.Location = new System.Drawing.Point(13, 86);
            this.dgvReadableParameters.Name = "dgvReadableParameters";
            this.dgvReadableParameters.RowTemplate.Height = 24;
            this.dgvReadableParameters.Size = new System.Drawing.Size(500, 200);
            this.dgvReadableParameters.TabIndex = 3;
            // 
            // lbReadableParameters
            // 
            this.lbReadableParameters.AutoSize = true;
            this.lbReadableParameters.Location = new System.Drawing.Point(11, 66);
            this.lbReadableParameters.Name = "lbReadableParameters";
            this.lbReadableParameters.Size = new System.Drawing.Size(150, 17);
            this.lbReadableParameters.TabIndex = 4;
            this.lbReadableParameters.Text = "Readable Parameters:";
            // 
            // cbCommands
            // 
            this.cbCommands.FormattingEnabled = true;
            this.cbCommands.Location = new System.Drawing.Point(519, 86);
            this.cbCommands.Name = "cbCommands";
            this.cbCommands.Size = new System.Drawing.Size(319, 24);
            this.cbCommands.TabIndex = 5;
            // 
            // lbSelectedCommand
            // 
            this.lbSelectedCommand.AutoSize = true;
            this.lbSelectedCommand.Location = new System.Drawing.Point(516, 66);
            this.lbSelectedCommand.Name = "lbSelectedCommand";
            this.lbSelectedCommand.Size = new System.Drawing.Size(142, 17);
            this.lbSelectedCommand.TabIndex = 6;
            this.lbSelectedCommand.Text = "Seleceted Command:";
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "ID";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 50;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Value";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 73;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn3.HeaderText = "Description";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "ID";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Width = 50;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "Value";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.Width = 73;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn6.HeaderText = "Description";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            // 
            // btnSendSelCommand
            // 
            this.btnSendSelCommand.Location = new System.Drawing.Point(519, 116);
            this.btnSendSelCommand.Name = "btnSendSelCommand";
            this.btnSendSelCommand.Size = new System.Drawing.Size(140, 30);
            this.btnSendSelCommand.TabIndex = 7;
            this.btnSendSelCommand.Text = "Send Command";
            this.btnSendSelCommand.UseVisualStyleBackColor = true;
            this.btnSendSelCommand.Click += new System.EventHandler(this.btnSendSelCommand_Click);
            // 
            // btnRequestFireKey
            // 
            this.btnRequestFireKey.Location = new System.Drawing.Point(14, 18);
            this.btnRequestFireKey.Name = "btnRequestFireKey";
            this.btnRequestFireKey.Size = new System.Drawing.Size(140, 30);
            this.btnRequestFireKey.TabIndex = 8;
            this.btnRequestFireKey.Text = "Request a FireKey";
            this.btnRequestFireKey.UseVisualStyleBackColor = true;
            this.btnRequestFireKey.Click += new System.EventHandler(this.btnRequestFireKey_Click);
            // 
            // btnUpdateAcPara
            // 
            this.btnUpdateAcPara.Location = new System.Drawing.Point(373, 292);
            this.btnUpdateAcPara.Name = "btnUpdateAcPara";
            this.btnUpdateAcPara.Size = new System.Drawing.Size(140, 30);
            this.btnUpdateAcPara.TabIndex = 9;
            this.btnUpdateAcPara.Text = "Update Parameters";
            this.btnUpdateAcPara.UseVisualStyleBackColor = true;
            this.btnUpdateAcPara.Click += new System.EventHandler(this.btnUpdateAcPara_Click);
            // 
            // pbConnection
            // 
            this.pbConnection.Location = new System.Drawing.Point(12, 544);
            this.pbConnection.Name = "pbConnection";
            this.pbConnection.Size = new System.Drawing.Size(826, 23);
            this.pbConnection.TabIndex = 10;
            // 
            // waitingTimer
            // 
            this.waitingTimer.Interval = 20;
            this.waitingTimer.Tick += new System.EventHandler(this.waitingTimer_Tick);
            // 
            // bwWaitForConnection
            // 
            this.bwWaitForConnection.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwWaitForConnection_DoWork);
            // 
            // btnReleaseFireKey
            // 
            this.btnReleaseFireKey.Location = new System.Drawing.Point(160, 18);
            this.btnReleaseFireKey.Name = "btnReleaseFireKey";
            this.btnReleaseFireKey.Size = new System.Drawing.Size(140, 30);
            this.btnReleaseFireKey.TabIndex = 11;
            this.btnReleaseFireKey.Text = "Release FireKey";
            this.btnReleaseFireKey.UseVisualStyleBackColor = true;
            this.btnReleaseFireKey.Click += new System.EventHandler(this.btnReleaseFireKey_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(854, 579);
            this.Controls.Add(this.btnReleaseFireKey);
            this.Controls.Add(this.pbConnection);
            this.Controls.Add(this.btnUpdateAcPara);
            this.Controls.Add(this.btnRequestFireKey);
            this.Controls.Add(this.btnSendSelCommand);
            this.Controls.Add(this.lbSelectedCommand);
            this.Controls.Add(this.cbCommands);
            this.Controls.Add(this.lbReadableParameters);
            this.Controls.Add(this.dgvReadableParameters);
            this.Controls.Add(this.lbWriteableParameters);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.button1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Main";
            this.Text = "Server Mask";
            this.Load += new System.EventHandler(this.Main_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReadableParameters)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label lbWriteableParameters;
        private System.Windows.Forms.DataGridView dgvReadableParameters;
        private System.Windows.Forms.Label lbReadableParameters;
        private System.Windows.Forms.ComboBox cbCommands;
        private System.Windows.Forms.Label lbSelectedCommand;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.Button btnSendSelCommand;
        private System.Windows.Forms.Button btnRequestFireKey;
        private System.Windows.Forms.Button btnUpdateAcPara;
        private System.Windows.Forms.ProgressBar pbConnection;
        private System.Windows.Forms.Timer waitingTimer;
        private System.ComponentModel.BackgroundWorker bwWaitForConnection;
        private System.Windows.Forms.Button btnReleaseFireKey;
    }
}