namespace SKB.Equipment.Forms
{
    partial class MaintenanceForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MaintenanceForm));
            this.OK = new DevExpress.XtraEditors.SimpleButton();
            this.Cancel = new DevExpress.XtraEditors.SimpleButton();
            this.DateOfEvent = new DevExpress.XtraEditors.DateEdit();
            this.NextDateOfEvent = new DevExpress.XtraEditors.DateEdit();
            this.LDateOfEvent = new DevExpress.XtraEditors.LabelControl();
            this.LNextDateOfEvent = new DevExpress.XtraEditors.LabelControl();
            this.LDocument = new DevExpress.XtraEditors.LabelControl();
            this.Document = new DevExpress.XtraEditors.TextEdit();
            this.Add = new DevExpress.XtraEditors.SimpleButton();
            this.Remove = new DevExpress.XtraEditors.SimpleButton();
            this.Open = new DevExpress.XtraEditors.SimpleButton();
            this.Employee = new DevExpress.XtraEditors.ButtonEdit();
            this.LEmployee = new DevExpress.XtraEditors.LabelControl();
            this.Position = new DevExpress.XtraEditors.TextEdit();
            this.LPosition = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.DateOfEvent.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DateOfEvent.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NextDateOfEvent.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NextDateOfEvent.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Document.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Employee.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Position.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // OK
            // 
            this.OK.Location = new System.Drawing.Point(328, 210);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(75, 23);
            this.OK.TabIndex = 6;
            this.OK.Text = "ОК";
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // Cancel
            // 
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(415, 210);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 7;
            this.Cancel.Text = "Отмена";
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // DateOfEvent
            // 
            this.DateOfEvent.EditValue = null;
            this.DateOfEvent.Location = new System.Drawing.Point(16, 34);
            this.DateOfEvent.Name = "DateOfEvent";
            this.DateOfEvent.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.DateOfEvent.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.DateOfEvent.Size = new System.Drawing.Size(229, 20);
            this.DateOfEvent.TabIndex = 1;
            this.DateOfEvent.EditValueChanged += new System.EventHandler(this.DateOfEvent_EditValueChanged);
            // 
            // NextDateOfEvent
            // 
            this.NextDateOfEvent.EditValue = null;
            this.NextDateOfEvent.Enabled = false;
            this.NextDateOfEvent.Location = new System.Drawing.Point(261, 34);
            this.NextDateOfEvent.Name = "NextDateOfEvent";
            this.NextDateOfEvent.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.NextDateOfEvent.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.NextDateOfEvent.Size = new System.Drawing.Size(229, 20);
            this.NextDateOfEvent.TabIndex = 0;
            // 
            // LDateOfEvent
            // 
            this.LDateOfEvent.Location = new System.Drawing.Point(17, 15);
            this.LDateOfEvent.Name = "LDateOfEvent";
            this.LDateOfEvent.Size = new System.Drawing.Size(94, 13);
            this.LDateOfEvent.TabIndex = 0;
            this.LDateOfEvent.Text = "Дата проведения:";
            // 
            // LNextDateOfEvent
            // 
            this.LNextDateOfEvent.Location = new System.Drawing.Point(261, 15);
            this.LNextDateOfEvent.Name = "LNextDateOfEvent";
            this.LNextDateOfEvent.Size = new System.Drawing.Size(162, 13);
            this.LNextDateOfEvent.TabIndex = 0;
            this.LNextDateOfEvent.Text = "Дата следующего проведения:";
            // 
            // LDocument
            // 
            this.LDocument.Location = new System.Drawing.Point(17, 153);
            this.LDocument.Name = "LDocument";
            this.LDocument.Size = new System.Drawing.Size(111, 13);
            this.LDocument.TabIndex = 0;
            this.LDocument.Text = "Документ-основание:";
            // 
            // Document
            // 
            this.Document.Enabled = false;
            this.Document.Location = new System.Drawing.Point(17, 172);
            this.Document.Name = "Document";
            this.Document.Size = new System.Drawing.Size(398, 20);
            this.Document.TabIndex = 0;
            // 
            // Add
            // 
            this.Add.Image = ((System.Drawing.Image)(resources.GetObject("Add.Image")));
            this.Add.Location = new System.Drawing.Point(416, 170);
            this.Add.Name = "Add";
            this.Add.Size = new System.Drawing.Size(24, 24);
            this.Add.TabIndex = 3;
            this.Add.Text = "Add";
            this.Add.Click += new System.EventHandler(this.Add_Click);
            // 
            // Remove
            // 
            this.Remove.Image = ((System.Drawing.Image)(resources.GetObject("Remove.Image")));
            this.Remove.Location = new System.Drawing.Point(441, 170);
            this.Remove.Name = "Remove";
            this.Remove.Size = new System.Drawing.Size(24, 24);
            this.Remove.TabIndex = 4;
            this.Remove.Text = "Remove";
            this.Remove.Click += new System.EventHandler(this.Remove_Click);
            // 
            // Open
            // 
            this.Open.Image = ((System.Drawing.Image)(resources.GetObject("Open.Image")));
            this.Open.Location = new System.Drawing.Point(466, 170);
            this.Open.Name = "Open";
            this.Open.Size = new System.Drawing.Size(24, 24);
            this.Open.TabIndex = 5;
            this.Open.Text = "Open";
            this.Open.Click += new System.EventHandler(this.Open_Click);
            // 
            // Employee
            // 
            this.Employee.Location = new System.Drawing.Point(17, 82);
            this.Employee.Name = "Employee";
            this.Employee.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.Employee.Size = new System.Drawing.Size(473, 20);
            this.Employee.TabIndex = 8;
            this.Employee.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.Employee_ButtonClick);
            // 
            // LEmployee
            // 
            this.LEmployee.Location = new System.Drawing.Point(17, 63);
            this.LEmployee.Name = "LEmployee";
            this.LEmployee.Size = new System.Drawing.Size(163, 13);
            this.LEmployee.TabIndex = 9;
            this.LEmployee.Text = "Ответственный за проведение:";
            // 
            // Position
            // 
            this.Position.Enabled = false;
            this.Position.Location = new System.Drawing.Point(17, 127);
            this.Position.Name = "Position";
            this.Position.Size = new System.Drawing.Size(473, 20);
            this.Position.TabIndex = 10;
            // 
            // LPosition
            // 
            this.LPosition.Location = new System.Drawing.Point(17, 108);
            this.LPosition.Name = "LPosition";
            this.LPosition.Size = new System.Drawing.Size(61, 13);
            this.LPosition.TabIndex = 11;
            this.LPosition.Text = "Должность:";
            // 
            // MaintenanceForm
            // 
            this.AcceptButton = this.OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.Cancel;
            this.ClientSize = new System.Drawing.Size(508, 248);
            this.Controls.Add(this.LPosition);
            this.Controls.Add(this.Position);
            this.Controls.Add(this.LEmployee);
            this.Controls.Add(this.Employee);
            this.Controls.Add(this.Open);
            this.Controls.Add(this.Remove);
            this.Controls.Add(this.Add);
            this.Controls.Add(this.Document);
            this.Controls.Add(this.LDocument);
            this.Controls.Add(this.LNextDateOfEvent);
            this.Controls.Add(this.LDateOfEvent);
            this.Controls.Add(this.NextDateOfEvent);
            this.Controls.Add(this.DateOfEvent);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.OK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(524, 286);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(524, 286);
            this.Name = "MaintenanceForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            ((System.ComponentModel.ISupportInitialize)(this.DateOfEvent.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DateOfEvent.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NextDateOfEvent.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NextDateOfEvent.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Document.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Employee.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Position.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton OK;
        private DevExpress.XtraEditors.SimpleButton Cancel;
        private DevExpress.XtraEditors.DateEdit DateOfEvent;
        private DevExpress.XtraEditors.DateEdit NextDateOfEvent;
        private DevExpress.XtraEditors.LabelControl LDateOfEvent;
        private DevExpress.XtraEditors.LabelControl LNextDateOfEvent;
        private DevExpress.XtraEditors.LabelControl LDocument;
        private DevExpress.XtraEditors.TextEdit Document;
        private DevExpress.XtraEditors.SimpleButton Add;
        private DevExpress.XtraEditors.SimpleButton Remove;
        private DevExpress.XtraEditors.SimpleButton Open;
        private DevExpress.XtraEditors.ButtonEdit Employee;
        private DevExpress.XtraEditors.LabelControl LEmployee;
        private DevExpress.XtraEditors.TextEdit Position;
        private DevExpress.XtraEditors.LabelControl LPosition;
    }
}