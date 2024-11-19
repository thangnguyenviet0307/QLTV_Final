namespace QLTV
{
    partial class fAdmin
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
            this.btnNext = new System.Windows.Forms.Button();
            this.btnLast = new System.Windows.Forms.Button();
            this.btnPrevious = new System.Windows.Forms.Button();
            this.btnFirst = new System.Windows.Forms.Button();
            this.txbPageAccount = new System.Windows.Forms.TextBox();
            this.btnSearchAcc = new System.Windows.Forms.Button();
            this.txbSearch = new System.Windows.Forms.TextBox();
            this.txbStudentID = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.btnResetPassword = new System.Windows.Forms.Button();
            this.txbDisplayName = new System.Windows.Forms.TextBox();
            this.txbUserName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnShowAccount = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.dtgvAccount = new System.Windows.Forms.DataGridView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.aCcountToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtgvAccount)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = global::QLTV.Properties.Resources.KhuS;
            this.panel1.Controls.Add(this.btnNext);
            this.panel1.Controls.Add(this.btnLast);
            this.panel1.Controls.Add(this.btnPrevious);
            this.panel1.Controls.Add(this.btnFirst);
            this.panel1.Controls.Add(this.txbPageAccount);
            this.panel1.Controls.Add(this.btnSearchAcc);
            this.panel1.Controls.Add(this.txbSearch);
            this.panel1.Controls.Add(this.txbStudentID);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.numericUpDown);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.btnResetPassword);
            this.panel1.Controls.Add(this.txbDisplayName);
            this.panel1.Controls.Add(this.txbUserName);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btnShowAccount);
            this.panel1.Controls.Add(this.btnEdit);
            this.panel1.Controls.Add(this.btnDelete);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Controls.Add(this.dtgvAccount);
            this.panel1.Controls.Add(this.menuStrip1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(845, 505);
            this.panel1.TabIndex = 0;
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(275, 431);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(91, 50);
            this.btnNext.TabIndex = 25;
            this.btnNext.Text = "Next";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnLast
            // 
            this.btnLast.Location = new System.Drawing.Point(372, 431);
            this.btnLast.Name = "btnLast";
            this.btnLast.Size = new System.Drawing.Size(82, 50);
            this.btnLast.TabIndex = 24;
            this.btnLast.Text = "Last";
            this.btnLast.UseVisualStyleBackColor = true;
            this.btnLast.Click += new System.EventHandler(this.btnLast_Click);
            // 
            // btnPrevious
            // 
            this.btnPrevious.Location = new System.Drawing.Point(103, 431);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(97, 50);
            this.btnPrevious.TabIndex = 23;
            this.btnPrevious.Text = "Previous";
            this.btnPrevious.UseVisualStyleBackColor = true;
            this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
            // 
            // btnFirst
            // 
            this.btnFirst.Location = new System.Drawing.Point(14, 431);
            this.btnFirst.Name = "btnFirst";
            this.btnFirst.Size = new System.Drawing.Size(83, 50);
            this.btnFirst.TabIndex = 22;
            this.btnFirst.Text = "First";
            this.btnFirst.UseVisualStyleBackColor = true;
            this.btnFirst.Click += new System.EventHandler(this.btnFirst_Click);
            // 
            // txbPageAccount
            // 
            this.txbPageAccount.Location = new System.Drawing.Point(206, 443);
            this.txbPageAccount.Name = "txbPageAccount";
            this.txbPageAccount.ReadOnly = true;
            this.txbPageAccount.Size = new System.Drawing.Size(63, 26);
            this.txbPageAccount.TabIndex = 21;
            this.txbPageAccount.Text = "1";
            this.txbPageAccount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txbPageAccount.TextChanged += new System.EventHandler(this.txbPageAccount_TextChanged);
            // 
            // btnSearchAcc
            // 
            this.btnSearchAcc.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSearchAcc.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearchAcc.Location = new System.Drawing.Point(714, 77);
            this.btnSearchAcc.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSearchAcc.Name = "btnSearchAcc";
            this.btnSearchAcc.Size = new System.Drawing.Size(116, 38);
            this.btnSearchAcc.TabIndex = 20;
            this.btnSearchAcc.Text = "Search";
            this.btnSearchAcc.UseVisualStyleBackColor = true;
            this.btnSearchAcc.Click += new System.EventHandler(this.btnSearchAcc_Click);
            // 
            // txbSearch
            // 
            this.txbSearch.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txbSearch.Location = new System.Drawing.Point(473, 83);
            this.txbSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txbSearch.Name = "txbSearch";
            this.txbSearch.Size = new System.Drawing.Size(223, 26);
            this.txbSearch.TabIndex = 19;
            this.txbSearch.TextChanged += new System.EventHandler(this.txbSearch_TextChanged);
            // 
            // txbStudentID
            // 
            this.txbStudentID.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txbStudentID.Location = new System.Drawing.Point(608, 237);
            this.txbStudentID.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txbStudentID.Name = "txbStudentID";
            this.txbStudentID.Size = new System.Drawing.Size(222, 26);
            this.txbStudentID.TabIndex = 18;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(469, 243);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 20);
            this.label3.TabIndex = 17;
            this.label3.Text = "Student ID: ";
            // 
            // numericUpDown
            // 
            this.numericUpDown.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numericUpDown.Location = new System.Drawing.Point(608, 284);
            this.numericUpDown.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.numericUpDown.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown.Name = "numericUpDown";
            this.numericUpDown.Size = new System.Drawing.Size(222, 26);
            this.numericUpDown.TabIndex = 16;
            this.numericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(469, 290);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 20);
            this.label4.TabIndex = 15;
            this.label4.Text = "Type:";
            // 
            // btnResetPassword
            // 
            this.btnResetPassword.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnResetPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnResetPassword.Location = new System.Drawing.Point(632, 361);
            this.btnResetPassword.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnResetPassword.Name = "btnResetPassword";
            this.btnResetPassword.Size = new System.Drawing.Size(184, 38);
            this.btnResetPassword.TabIndex = 14;
            this.btnResetPassword.Text = "Reset Password";
            this.btnResetPassword.UseVisualStyleBackColor = true;
            this.btnResetPassword.Click += new System.EventHandler(this.btnResetPassword_Click);
            // 
            // txbDisplayName
            // 
            this.txbDisplayName.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txbDisplayName.Location = new System.Drawing.Point(608, 185);
            this.txbDisplayName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txbDisplayName.Name = "txbDisplayName";
            this.txbDisplayName.Size = new System.Drawing.Size(222, 26);
            this.txbDisplayName.TabIndex = 12;
            // 
            // txbUserName
            // 
            this.txbUserName.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txbUserName.Location = new System.Drawing.Point(608, 131);
            this.txbUserName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txbUserName.Name = "txbUserName";
            this.txbUserName.Size = new System.Drawing.Size(223, 26);
            this.txbUserName.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(469, 192);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(123, 20);
            this.label2.TabIndex = 9;
            this.label2.Text = "Display Name:";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(469, 135);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 20);
            this.label1.TabIndex = 8;
            this.label1.Text = "User Name:";
            // 
            // btnShowAccount
            // 
            this.btnShowAccount.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnShowAccount.Location = new System.Drawing.Point(358, 69);
            this.btnShowAccount.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnShowAccount.Name = "btnShowAccount";
            this.btnShowAccount.Size = new System.Drawing.Size(84, 55);
            this.btnShowAccount.TabIndex = 6;
            this.btnShowAccount.Text = "Show";
            this.btnShowAccount.UseVisualStyleBackColor = true;
            this.btnShowAccount.Click += new System.EventHandler(this.btnSee_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnEdit.Location = new System.Drawing.Point(234, 69);
            this.btnEdit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(84, 55);
            this.btnEdit.TabIndex = 5;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDelete.Location = new System.Drawing.Point(124, 69);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(84, 55);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnAdd.Location = new System.Drawing.Point(14, 69);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(84, 55);
            this.btnAdd.TabIndex = 3;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // dtgvAccount
            // 
            this.dtgvAccount.AllowDrop = true;
            this.dtgvAccount.AllowUserToAddRows = false;
            this.dtgvAccount.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dtgvAccount.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dtgvAccount.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgvAccount.Location = new System.Drawing.Point(14, 131);
            this.dtgvAccount.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtgvAccount.Name = "dtgvAccount";
            this.dtgvAccount.ReadOnly = true;
            this.dtgvAccount.RowHeadersWidth = 51;
            this.dtgvAccount.RowTemplate.Height = 24;
            this.dtgvAccount.Size = new System.Drawing.Size(440, 268);
            this.dtgvAccount.TabIndex = 2;
            // 
            // menuStrip1
            // 
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aCcountToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(845, 33);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // aCcountToolStripMenuItem
            // 
            this.aCcountToolStripMenuItem.Name = "aCcountToolStripMenuItem";
            this.aCcountToolStripMenuItem.Size = new System.Drawing.Size(93, 29);
            this.aCcountToolStripMenuItem.Text = "Account";
            // 
            // fAdmin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(845, 505);
            this.Controls.Add(this.panel1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "fAdmin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Admin";
            this.Load += new System.EventHandler(this.fAdmin_Load);
            this.Resize += new System.EventHandler(this.fAdmin_Resize);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtgvAccount)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem aCcountToolStripMenuItem;
        private System.Windows.Forms.TextBox txbDisplayName;
        private System.Windows.Forms.TextBox txbUserName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnShowAccount;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.DataGridView dtgvAccount;
        private System.Windows.Forms.Button btnResetPassword;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericUpDown;
        private System.Windows.Forms.TextBox txbStudentID;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSearchAcc;
        private System.Windows.Forms.TextBox txbSearch;
        private System.Windows.Forms.TextBox txbPageAccount;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnLast;
        private System.Windows.Forms.Button btnPrevious;
        private System.Windows.Forms.Button btnFirst;
    }
}