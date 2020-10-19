using SKB.Base;
namespace SKB.Equipment.Forms
{
    partial class GraphForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GraphForm));
            this.OK = new DevExpress.XtraEditors.SimpleButton();
            this.Cancel = new DevExpress.XtraEditors.SimpleButton();
            this.DateOfEvent = new DevExpress.XtraEditors.DateEdit();
            this.NextDateOfEvent = new DevExpress.XtraEditors.DateEdit();
            this.NumberOfDocument = new DevExpress.XtraEditors.TextEdit();
            this.LDateOfEvent = new DevExpress.XtraEditors.LabelControl();
            this.LNumberOfDocument = new DevExpress.XtraEditors.LabelControl();
            this.LNextDateOfEvent = new DevExpress.XtraEditors.LabelControl();
            this.LDocument = new DevExpress.XtraEditors.LabelControl();
            this.Document = new DevExpress.XtraEditors.TextEdit();
            this.Add = new DevExpress.XtraEditors.SimpleButton();
            this.Remove = new DevExpress.XtraEditors.SimpleButton();
            this.Open = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.DateOfEvent.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DateOfEvent.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NextDateOfEvent.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NextDateOfEvent.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumberOfDocument.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Document.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // OK
            // 
            this.OK.Location = new System.Drawing.Point(332, 166);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(75, 23);
            this.OK.TabIndex = 6;
            this.OK.Text = "ОК";
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // Cancel
            // 
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(419, 166);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 7;
            this.Cancel.Text = "Отмена";
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // DateOfEvent
            // 
            this.DateOfEvent.EditValue = null;
            this.DateOfEvent.Location = new System.Drawing.Point(20, 34);
            this.DateOfEvent.Name = "DateOfEvent";
            this.DateOfEvent.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.DateOfEvent.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.DateOfEvent.Size = new System.Drawing.Size(225, 20);
            this.DateOfEvent.TabIndex = 1;
            this.DateOfEvent.EditValueChanged += new System.EventHandler(this.DateOfEvent_EditValueChanged);
            // 
            // NextDateOfEvent
            // 
            this.NextDateOfEvent.EditValue = null;
            this.NextDateOfEvent.Enabled = false;
            this.NextDateOfEvent.Location = new System.Drawing.Point(268, 34);
            this.NextDateOfEvent.Name = "NextDateOfEvent";
            this.NextDateOfEvent.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.NextDateOfEvent.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.NextDateOfEvent.Size = new System.Drawing.Size(225, 20);
            this.NextDateOfEvent.TabIndex = 0;
            // 
            // NumberOfDocument
            // 
            this.NumberOfDocument.Location = new System.Drawing.Point(20, 82);
            this.NumberOfDocument.Name = "NumberOfDocument";
            this.NumberOfDocument.Size = new System.Drawing.Size(473, 20);
            this.NumberOfDocument.TabIndex = 2;
            // 
            // LDateOfEvent
            // 
            this.LDateOfEvent.Location = new System.Drawing.Point(20, 15);
            this.LDateOfEvent.Name = "LDateOfEvent";
            this.LDateOfEvent.Size = new System.Drawing.Size(94, 13);
            this.LDateOfEvent.TabIndex = 0;
            this.LDateOfEvent.Text = "Дата проведения:";
            // 
            // LNumberOfDocument
            // 
            this.LNumberOfDocument.Location = new System.Drawing.Point(20, 63);
            this.LNumberOfDocument.Name = "LNumberOfDocument";
            this.LNumberOfDocument.Size = new System.Drawing.Size(93, 13);
            this.LNumberOfDocument.TabIndex = 0;
            this.LNumberOfDocument.Text = "Номер документа:";
            // 
            // LNextDateOfEvent
            // 
            this.LNextDateOfEvent.Location = new System.Drawing.Point(268, 15);
            this.LNextDateOfEvent.Name = "LNextDateOfEvent";
            this.LNextDateOfEvent.Size = new System.Drawing.Size(162, 13);
            this.LNextDateOfEvent.TabIndex = 0;
            this.LNextDateOfEvent.Text = "Дата следующего проведения:";
            // 
            // LDocument
            // 
            this.LDocument.Location = new System.Drawing.Point(20, 108);
            this.LDocument.Name = "LDocument";
            this.LDocument.Size = new System.Drawing.Size(54, 13);
            this.LDocument.TabIndex = 0;
            this.LDocument.Text = "Документ:";
            // 
            // Document
            // 
            this.Document.Enabled = false;
            this.Document.Location = new System.Drawing.Point(20, 127);
            this.Document.Name = "Document";
            this.Document.Size = new System.Drawing.Size(398, 20);
            this.Document.TabIndex = 0;
            // 
            // Add
            // 
            this.Add.Image = ((System.Drawing.Image)(resources.GetObject("Add.Image")));
            this.Add.Location = new System.Drawing.Point(419, 125);
            this.Add.Name = "Add";
            this.Add.Size = new System.Drawing.Size(24, 24);
            this.Add.TabIndex = 3;
            this.Add.Text = "Add";
            this.Add.Click += new System.EventHandler(this.Add_Click);
            // 
            // Remove
            // 
            this.Remove.Image = ((System.Drawing.Image)(resources.GetObject("Remove.Image")));
            this.Remove.Location = new System.Drawing.Point(444, 125);
            this.Remove.Name = "Remove";
            this.Remove.Size = new System.Drawing.Size(24, 24);
            this.Remove.TabIndex = 4;
            this.Remove.Text = "Remove";
            this.Remove.Click += new System.EventHandler(this.Remove_Click);
            // 
            // Open
            // 
            this.Open.Image = ((System.Drawing.Image)(resources.GetObject("Open.Image")));
            this.Open.Location = new System.Drawing.Point(469, 125);
            this.Open.Name = "Open";
            this.Open.Size = new System.Drawing.Size(24, 24);
            this.Open.TabIndex = 5;
            this.Open.Text = "Open";
            this.Open.Click += new System.EventHandler(this.Open_Click);
            // 
            // GraphForm
            // 
            this.AcceptButton = this.OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.Cancel;
            this.ClientSize = new System.Drawing.Size(508, 201);
            this.Controls.Add(this.Open);
            this.Controls.Add(this.Remove);
            this.Controls.Add(this.Add);
            this.Controls.Add(this.Document);
            this.Controls.Add(this.LDocument);
            this.Controls.Add(this.LNextDateOfEvent);
            this.Controls.Add(this.LNumberOfDocument);
            this.Controls.Add(this.LDateOfEvent);
            this.Controls.Add(this.NumberOfDocument);
            this.Controls.Add(this.NextDateOfEvent);
            this.Controls.Add(this.DateOfEvent);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.OK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(524, 239);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(524, 239);
            this.Name = "GraphForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            ((System.ComponentModel.ISupportInitialize)(this.DateOfEvent.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DateOfEvent.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NextDateOfEvent.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NextDateOfEvent.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumberOfDocument.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Document.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton OK;
        private DevExpress.XtraEditors.SimpleButton Cancel;
        private DevExpress.XtraEditors.DateEdit DateOfEvent;
        private DevExpress.XtraEditors.DateEdit NextDateOfEvent;
        private DevExpress.XtraEditors.TextEdit NumberOfDocument;
        private DevExpress.XtraEditors.LabelControl LDateOfEvent;
        private DevExpress.XtraEditors.LabelControl LNumberOfDocument;
        private DevExpress.XtraEditors.LabelControl LNextDateOfEvent;
        private DevExpress.XtraEditors.LabelControl LDocument;
        private DevExpress.XtraEditors.TextEdit Document;
        private DevExpress.XtraEditors.SimpleButton Add;
        private DevExpress.XtraEditors.SimpleButton Remove;
        private DevExpress.XtraEditors.SimpleButton Open;
    }
}