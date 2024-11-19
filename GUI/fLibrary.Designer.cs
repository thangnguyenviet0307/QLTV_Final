namespace QLTV
{
    partial class fLibrary
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnLast = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrevious = new System.Windows.Forms.Button();
            this.btnFirst = new System.Windows.Forms.Button();
            this.txbPageFile = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnShow = new System.Windows.Forms.Button();
            this.btnDownload = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.btnConvert = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnUpload = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.dtgvLibrary = new System.Windows.Forms.DataGridView();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgvLibrary)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = global::QLTV.Properties.Resources.KhuS;
            this.panel1.Controls.Add(this.btnLast);
            this.panel1.Controls.Add(this.btnNext);
            this.panel1.Controls.Add(this.btnPrevious);
            this.panel1.Controls.Add(this.btnFirst);
            this.panel1.Controls.Add(this.txbPageFile);
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Controls.Add(this.btnShow);
            this.panel1.Controls.Add(this.btnDownload);
            this.panel1.Controls.Add(this.textBox2);
            this.panel1.Controls.Add(this.btnConvert);
            this.panel1.Controls.Add(this.btnDelete);
            this.panel1.Controls.Add(this.btnUpload);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.dtgvLibrary);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1191, 661);
            this.panel1.TabIndex = 0;
            // 
            // btnLast
            // 
            this.btnLast.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnLast.Location = new System.Drawing.Point(851, 556);
            this.btnLast.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnLast.Name = "btnLast";
            this.btnLast.Size = new System.Drawing.Size(84, 55);
            this.btnLast.TabIndex = 15;
            this.btnLast.Text = "Last";
            this.btnLast.UseVisualStyleBackColor = true;
            this.btnLast.Click += new System.EventHandler(this.btnLast_Click);
            // 
            // btnNext
            // 
            this.btnNext.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnNext.Location = new System.Drawing.Point(704, 556);
            this.btnNext.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(84, 55);
            this.btnNext.TabIndex = 14;
            this.btnNext.Text = "Next";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPrevious
            // 
            this.btnPrevious.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnPrevious.Location = new System.Drawing.Point(410, 556);
            this.btnPrevious.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(84, 55);
            this.btnPrevious.TabIndex = 13;
            this.btnPrevious.Text = "Previous";
            this.btnPrevious.UseVisualStyleBackColor = true;
            this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
            // 
            // btnFirst
            // 
            this.btnFirst.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnFirst.Location = new System.Drawing.Point(270, 556);
            this.btnFirst.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnFirst.Name = "btnFirst";
            this.btnFirst.Size = new System.Drawing.Size(84, 55);
            this.btnFirst.TabIndex = 12;
            this.btnFirst.Text = "First";
            this.btnFirst.UseVisualStyleBackColor = true;
            this.btnFirst.Click += new System.EventHandler(this.btnFirst_Click);
            // 
            // txbPageFile
            // 
            this.txbPageFile.Location = new System.Drawing.Point(565, 570);
            this.txbPageFile.Name = "txbPageFile";
            this.txbPageFile.ReadOnly = true;
            this.txbPageFile.Size = new System.Drawing.Size(91, 26);
            this.txbPageFile.TabIndex = 11;
            this.txbPageFile.Text = "1";
            this.txbPageFile.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txbPageFile.TextChanged += new System.EventHandler(this.txbPageFile_TextChanged);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSearch.Location = new System.Drawing.Point(837, 136);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(98, 29);
            this.btnSearch.TabIndex = 10;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnShow
            // 
            this.btnShow.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnShow.Location = new System.Drawing.Point(837, 195);
            this.btnShow.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(98, 55);
            this.btnShow.TabIndex = 9;
            this.btnShow.Text = "Show";
            this.btnShow.UseVisualStyleBackColor = true;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // btnDownload
            // 
            this.btnDownload.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDownload.Location = new System.Drawing.Point(410, 195);
            this.btnDownload.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(93, 55);
            this.btnDownload.TabIndex = 8;
            this.btnDownload.Text = "Download";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // textBox2
            // 
            this.textBox2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.textBox2.Location = new System.Drawing.Point(459, 136);
            this.textBox2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(329, 26);
            this.textBox2.TabIndex = 7;
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged_1);
            // 
            // btnConvert
            // 
            this.btnConvert.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnConvert.Location = new System.Drawing.Point(704, 195);
            this.btnConvert.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(84, 55);
            this.btnConvert.TabIndex = 6;
            this.btnConvert.Text = "Convert";
            this.btnConvert.UseVisualStyleBackColor = true;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDelete.Location = new System.Drawing.Point(565, 195);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(91, 55);
            this.btnDelete.TabIndex = 5;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnUpload
            // 
            this.btnUpload.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnUpload.Location = new System.Drawing.Point(270, 195);
            this.btnUpload.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(84, 55);
            this.btnUpload.TabIndex = 4;
            this.btnUpload.Text = "Upload";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(265, 140);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(172, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "Enter Keyword:";
            // 
            // dtgvLibrary
            // 
            this.dtgvLibrary.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.dtgvLibrary.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dtgvLibrary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgvLibrary.Location = new System.Drawing.Point(248, 258);
            this.dtgvLibrary.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtgvLibrary.Name = "dtgvLibrary";
            this.dtgvLibrary.ReadOnly = true;
            this.dtgvLibrary.RowHeadersWidth = 51;
            this.dtgvLibrary.RowTemplate.Height = 24;
            this.dtgvLibrary.Size = new System.Drawing.Size(710, 268);
            this.dtgvLibrary.TabIndex = 0;
            // 
            // fLibrary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1191, 661);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "fLibrary";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Library Management";
            this.Load += new System.EventHandler(this.fLibrary_Load);
            this.Resize += new System.EventHandler(this.fLibrary_Resize);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgvLibrary)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dtgvLibrary;
        private System.Windows.Forms.Button btnConvert;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.TextBox textBox2;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.Button btnShow;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnLast;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrevious;
        private System.Windows.Forms.Button btnFirst;
        private System.Windows.Forms.TextBox txbPageFile;
    }
}