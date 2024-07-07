namespace UI_LAYER.Pepole
{
    partial class frmManagePepole
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
            this.dgvPeople = new System.Windows.Forms.DataGridView();
            this.cmsPeople = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.muIt_ShowDetails = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.muIt_AddNewPerson = new System.Windows.Forms.ToolStripMenuItem();
            this.muIt_Edit = new System.Windows.Forms.ToolStripMenuItem();
            this.muIt_Delete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.muIt_SendEmail = new System.Windows.Forms.ToolStripMenuItem();
            this.muIt_PhoneCall = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.lblTitle = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbFilterBy = new System.Windows.Forms.ComboBox();
            this.txtFilterValue = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblRecords = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.btnAddPepole = new System.Windows.Forms.Button();
            this.pbPersonImage = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPeople)).BeginInit();
            this.cmsPeople.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPersonImage)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvDrivers
            // 
            this.dgvPeople.AllowUserToAddRows = false;
            this.dgvPeople.AllowUserToDeleteRows = false;
            this.dgvPeople.BackgroundColor = System.Drawing.Color.Azure;
            this.dgvPeople.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPeople.ContextMenuStrip = this.cmsPeople;
            this.dgvPeople.Location = new System.Drawing.Point(12, 242);
            this.dgvPeople.MultiSelect = false;
            this.dgvPeople.Name = "dgvDrivers";
            this.dgvPeople.ReadOnly = true;
            this.dgvPeople.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPeople.Size = new System.Drawing.Size(1161, 358);
            this.dgvPeople.TabIndex = 0;
            this.dgvPeople.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPeople_CellDoubleClick);
            // 
            // cmsPeople
            // 
            this.cmsPeople.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.muIt_ShowDetails,
            this.toolStripSeparator1,
            this.muIt_AddNewPerson,
            this.muIt_Edit,
            this.muIt_Delete,
            this.toolStripSeparator2,
            this.muIt_SendEmail,
            this.muIt_PhoneCall,
            this.toolStripMenuItem1});
            this.cmsPeople.Name = "cmsPeople";
            this.cmsPeople.Size = new System.Drawing.Size(142, 170);
            // 
            // muIt_ShowDetails
            // 
            this.muIt_ShowDetails.Image = global::UI_LAYER.Properties.Resources.PersonDetails_32;
            this.muIt_ShowDetails.Name = "muIt_ShowDetails";
            this.muIt_ShowDetails.Size = new System.Drawing.Size(141, 22);
            this.muIt_ShowDetails.Text = "Show Details";
            this.muIt_ShowDetails.Click += new System.EventHandler(this.muIt_ShowDetails_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(138, 6);
            // 
            // muIt_AddNewPerson
            // 
            this.muIt_AddNewPerson.Image = global::UI_LAYER.Properties.Resources.Add_Person_40;
            this.muIt_AddNewPerson.Name = "muIt_AddNewPerson";
            this.muIt_AddNewPerson.Size = new System.Drawing.Size(141, 22);
            this.muIt_AddNewPerson.Text = "Add New";
            this.muIt_AddNewPerson.Click += new System.EventHandler(this.muIt_AddNewPerson_Click);
            // 
            // muIt_Edit
            // 
            this.muIt_Edit.Image = global::UI_LAYER.Properties.Resources.edit_32;
            this.muIt_Edit.Name = "muIt_Edit";
            this.muIt_Edit.Size = new System.Drawing.Size(141, 22);
            this.muIt_Edit.Text = "Edit";
            this.muIt_Edit.Click += new System.EventHandler(this.muIt_Edit_Click);
            // 
            // muIt_Delete
            // 
            this.muIt_Delete.Image = global::UI_LAYER.Properties.Resources.Delete_32;
            this.muIt_Delete.Name = "muIt_Delete";
            this.muIt_Delete.Size = new System.Drawing.Size(141, 22);
            this.muIt_Delete.Text = "Delete";
            this.muIt_Delete.Click += new System.EventHandler(this.muIt_Delete_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(138, 6);
            // 
            // muIt_SendEmail
            // 
            this.muIt_SendEmail.Image = global::UI_LAYER.Properties.Resources.Email_32;
            this.muIt_SendEmail.Name = "muIt_SendEmail";
            this.muIt_SendEmail.Size = new System.Drawing.Size(141, 22);
            this.muIt_SendEmail.Text = "Send Email";
            this.muIt_SendEmail.Click += new System.EventHandler(this.muIt_SendEmail_Click);
            // 
            // muIt_PhoneCall
            // 
            this.muIt_PhoneCall.Image = global::UI_LAYER.Properties.Resources.Phone_32;
            this.muIt_PhoneCall.Name = "muIt_PhoneCall";
            this.muIt_PhoneCall.Size = new System.Drawing.Size(141, 22);
            this.muIt_PhoneCall.Text = "Phone Call";
            this.muIt_PhoneCall.Click += new System.EventHandler(this.muIt_PhoneCall_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(141, 22);
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.Black;
            this.lblTitle.Location = new System.Drawing.Point(470, 149);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(258, 39);
            this.lblTitle.TabIndex = 90;
            this.lblTitle.Text = "Manage People";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 217);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 18);
            this.label1.TabIndex = 91;
            this.label1.Text = "Filter By";
            // 
            // cbFilterBy
            // 
            this.cbFilterBy.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cbFilterBy.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbFilterBy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbFilterBy.FormattingEnabled = true;
            this.cbFilterBy.Items.AddRange(new object[] {
            "None",
            "Person ID",
            "National No.",
            "First Name",
            "Second Name",
            "Third Name",
            "Last Name",
            "Nationality",
            "Gendor",
            "Phone",
            "Email"});
            this.cbFilterBy.Location = new System.Drawing.Point(79, 216);
            this.cbFilterBy.Name = "cbFilterBy";
            this.cbFilterBy.Size = new System.Drawing.Size(178, 21);
            this.cbFilterBy.TabIndex = 92;
            this.cbFilterBy.SelectedIndexChanged += new System.EventHandler(this.cbFilterBy_SelectedIndexChanged);
            // 
            // txtFilterValue
            // 
            this.txtFilterValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFilterValue.Location = new System.Drawing.Point(268, 216);
            this.txtFilterValue.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtFilterValue.Name = "txtFilterValue";
            this.txtFilterValue.Size = new System.Drawing.Size(256, 20);
            this.txtFilterValue.TabIndex = 94;
            this.txtFilterValue.Visible = false;
            this.txtFilterValue.TextChanged += new System.EventHandler(this.txtFilterValue_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 606);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 18);
            this.label2.TabIndex = 96;
            this.label2.Text = "Records :";
            // 
            // lblRecords
            // 
            this.lblRecords.AutoSize = true;
            this.lblRecords.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblRecords.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRecords.Location = new System.Drawing.Point(91, 606);
            this.lblRecords.Name = "lblRecords";
            this.lblRecords.Size = new System.Drawing.Size(35, 18);
            this.lblRecords.TabIndex = 97;
            this.lblRecords.Text = "???";
            // 
            // button1
            // 
            this.button1.Image = global::UI_LAYER.Properties.Resources.Close_32;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(1061, 606);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(112, 37);
            this.button1.TabIndex = 95;
            this.button1.Text = "Close";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // btnAddPepole
            // 
            this.btnAddPepole.Image = global::UI_LAYER.Properties.Resources.Add_Person_40;
            this.btnAddPepole.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnAddPepole.Location = new System.Drawing.Point(1074, 184);
            this.btnAddPepole.Name = "btnAddPepole";
            this.btnAddPepole.Size = new System.Drawing.Size(99, 53);
            this.btnAddPepole.TabIndex = 93;
            this.btnAddPepole.UseVisualStyleBackColor = true;
            this.btnAddPepole.Click += new System.EventHandler(this.btnAddPepole_Click);
            // 
            // pbPersonImage
            // 
            this.pbPersonImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pbPersonImage.Image = global::UI_LAYER.Properties.Resources.People_400;
            this.pbPersonImage.InitialImage = null;
            this.pbPersonImage.Location = new System.Drawing.Point(531, 14);
            this.pbPersonImage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pbPersonImage.Name = "pbPersonImage";
            this.pbPersonImage.Size = new System.Drawing.Size(144, 130);
            this.pbPersonImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbPersonImage.TabIndex = 89;
            this.pbPersonImage.TabStop = false;
            // 
            // frmManagePepole
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1185, 652);
            this.Controls.Add(this.lblRecords);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtFilterValue);
            this.Controls.Add(this.btnAddPepole);
            this.Controls.Add(this.cbFilterBy);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.pbPersonImage);
            this.Controls.Add(this.dgvPeople);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.Name = "frmManagePepole";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Manage Pepole";
            this.Load += new System.EventHandler(this.frmManagePepole_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPeople)).EndInit();
            this.cmsPeople.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbPersonImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvPeople;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.PictureBox pbPersonImage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbFilterBy;
        private System.Windows.Forms.Button btnAddPepole;
        private System.Windows.Forms.TextBox txtFilterValue;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblRecords;
        private System.Windows.Forms.ContextMenuStrip cmsPeople;
        private System.Windows.Forms.ToolStripMenuItem muIt_ShowDetails;
        private System.Windows.Forms.ToolStripMenuItem muIt_AddNewPerson;
        private System.Windows.Forms.ToolStripMenuItem muIt_Edit;
        private System.Windows.Forms.ToolStripMenuItem muIt_Delete;
        private System.Windows.Forms.ToolStripMenuItem muIt_SendEmail;
        private System.Windows.Forms.ToolStripMenuItem muIt_PhoneCall;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
    }
}