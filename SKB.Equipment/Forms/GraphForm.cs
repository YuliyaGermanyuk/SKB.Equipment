using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DocsVision.BackOffice.ObjectModel.Services;
using DocsVision.Platform.ObjectManager.SystemCards;
using DocsVision.Platform.ObjectModel;
using RKIT.MyMessageBox;
using SKB.Base;
using SKB.Base.Ref;

namespace SKB.Equipment.Forms
{
    public partial class GraphForm : DevExpress.XtraEditors.XtraForm
    {
        #region Fields
        /// <summary>
        /// Объектный контекст.
        /// </summary>
        ObjectContext Context;
        /// <summary>
        /// 
        /// </summary>
        MyBaseCard BaseCard;
        /// <summary>
        /// 
        /// </summary>
        DocsVision.BackOffice.ObjectModel.Document DocumentCard;
        /// <summary>
        /// Текущий тип проверки
        /// </summary>
        RefEquipmentCard.Enums.TypeOfInspection Type;
        // <summary>
        /// Текущий интервал проведения проверок
        /// </summary>
        Decimal Interval;
        // <summary>
        /// Текущая единица измерения
        /// </summary>
        SKB.Base.Ref.RefEquipmentCard.Enums.Units Unit;
        #endregion

        #region Properties
        /// <summary>
        /// Дата проведения.
        /// </summary>
        public DateTime DateOfEventValue
        {
            get { return (DateTime)this.DateOfEvent.EditValue; }
        }
        /// <summary>
        /// Дата следующего проведения.
        /// </summary>
        public DateTime NextDateOfEventValue
        {
            get { return (DateTime)this.NextDateOfEvent.EditValue; }
        }
        /// <summary>
        /// Идентификатор документа.
        /// </summary>
        public Guid DocumentGuid
        {
            get { return Context.GetObjectRef(DocumentCard).Id; }
        }
        /// <summary>
        /// Номер документа.
        /// </summary>
        public String NumberOfDocumentValue
        {
            get { return this.NumberOfDocument.Text; }
        }
        #endregion

        /// <summary>
        /// Констурктор формы редактирования графика калиборовок/поверок/аттестаций
        /// </summary>
        /// <param name="BaseCard">Родительская карточка.</param>
        /// <param name="Type">Тип проверки (калибровка/поверка/аттестация).</param>
        /// <param name="DateOfEvent">Дата проведения.</param>
        /// <param name="NextDateOfEvent">Дата следующего проведения.</param>
        /// <param name="NumberOfDocument">Номер документа.</param>
        /// <param name="Document">Идентификатор карточки документа.</param>
        /// <param name="Interval">Периодичность проверки.</param>
        /// <param name="Unit">Единица измерения времени.</param>
        public GraphForm(MyBaseCard BaseCard, RefEquipmentCard.Enums.TypeOfInspection Type, DateTime DateOfEvent, DateTime NextDateOfEvent, String NumberOfDocument, Guid Document, Decimal Interval, RefEquipmentCard.Enums.Units Unit)
        {
            InitializeComponent();
            this.Location = ComputeLocation(this.Size);

            this.BaseCard = BaseCard;
            Context = BaseCard.Context;
            this.Type = Type;
            this.Interval = Interval;
            this.Unit = Unit;

            switch (Type)
            {
                case RefEquipmentCard.Enums.TypeOfInspection.Verification:
                    this.Text = "Данные о проведении поверки:";
                    break;
                case RefEquipmentCard.Enums.TypeOfInspection.Calibration:
                    this.Text = "Данные о проведении калибровки:";
                    break;
                case RefEquipmentCard.Enums.TypeOfInspection.Attestation:
                    this.Text = "Данные о проведении аттестации";
                    break;
            }
            
            if (DateOfEvent != DateTime.MinValue)
                this.DateOfEvent.EditValue = DateOfEvent;
            if (NextDateOfEvent != DateTime.MinValue)
                this.NextDateOfEvent.EditValue = NextDateOfEvent;
            this.NumberOfDocument.Text = NumberOfDocument;

            DocumentCard = Document == Guid.Empty ? null : Context.GetObject<DocsVision.BackOffice.ObjectModel.Document>(Document);

            this.Document.Text = DocumentCard == null ? "" : DocumentCard.MainInfo.FileName;
        }
        /// <summary>
        /// Обработчик кнопки "ОК".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OK_Click(object sender, EventArgs e)
        {
            try
            {
                List<String> NullableFilds = new List<string>();
                if ((this.DateOfEvent.EditValue == null) || ((DateTime)this.DateOfEvent.EditValue == DateTime.MinValue))
                    NullableFilds.Add("Дата проведения");
                if ((String.IsNullOrEmpty(this.NumberOfDocument.Text)) || (this.NumberOfDocument.Text == ""))
                    NullableFilds.Add("Номер документа");
                if ((String.IsNullOrEmpty(this.Document.Text)) || (this.Document.Text == ""))
                    NullableFilds.Add("Документ");

                if (NullableFilds.Count > 1)
                    throw new MyException("Заполните следующие поля:\n" + NullableFilds.Select(s => " - \"" + s + "\"").Aggregate(";\n") + ".");
                if (NullableFilds.Count == 1)
                    throw new MyException("Заполните поле \"" + NullableFilds.First() + ".");
                if (NullableFilds.Count == 0)
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception Ex) { MyMessageBox.Show(Ex.Message); }
        }
        /// <summary>
        /// Обработчик кнопки "Отмена".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
        /// <summary>
        /// Обработчик кнопки "Добавить документ".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_Click(object sender, EventArgs e)
        {
            OpenFileDialog MyFileDialog = new OpenFileDialog();

            MyFileDialog.Multiselect = false;

            switch (Type)
            {
                case RefEquipmentCard.Enums.TypeOfInspection.Verification:
                    MyFileDialog.Title = "Выберите файл, подтверждающий проведение поверки...";
                    break;
                case RefEquipmentCard.Enums.TypeOfInspection.Calibration:
                    MyFileDialog.Title = "Выберите файл, подтверждающий проведение калибровки...";
                    break;
                case RefEquipmentCard.Enums.TypeOfInspection.Attestation:
                    MyFileDialog.Title = "Выберите файл, подтверждающий проведение аттестации...";
                    break;
            }

            if (MyFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (DocumentCard != null)
                    Context.DeleteObject(DocumentCard);
                DocumentCard = Context.GetObject<DocsVision.BackOffice.ObjectModel.Document>(SKB.Base.MyHelper.CreateDocument(Context, MyFileDialog.FileName));
                this.Document.Text = DocumentCard.Description;
            }
        }
        /// <summary>
        /// Обработчик кнопки "Удалить документ".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Remove_Click(object sender, EventArgs e)
        {
            if (DocumentCard != null)
            {
                
                Context.DeleteObject(DocumentCard);
                DocumentCard = null;
                this.Document.Text = "";
            }
        }
        /// <summary>
        /// Обработчик кнопки "Открыть документ".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Open_Click(object sender, EventArgs e)
        {
            if (DocumentCard != null)
            {
                DocsVision.BackOffice.ObjectModel.DocumentFile DocFile = DocumentCard.Files.First();
                
                IVersionedFileCardService VersionedFileCardService = Context.GetService<IVersionedFileCardService>();
                BaseCard.CardScript.CardControl.CardHost.ShowCardModal(Context.GetObjectRef(DocumentCard).Id, DocsVision.Platform.CardHost.ActivateMode.ReadOnly);
            }
        }
        /// <summary>
        /// Обработчик события изменения поля "Дата проведения".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DateOfEvent_EditValueChanged(object sender, EventArgs e)
        {
            if ((this.DateOfEvent.EditValue != null) && ((DateTime)this.DateOfEvent.EditValue != DateTime.MinValue))
            {
                switch (Unit)
                {
                    case RefEquipmentCard.Enums.Units.Days:
                        this.NextDateOfEvent.EditValue = ((DateTime)this.DateOfEvent.EditValue).AddDays(Convert.ToInt32(Interval));
                        break;
                    case RefEquipmentCard.Enums.Units.Weeks:
                        this.NextDateOfEvent.EditValue = ((DateTime)this.DateOfEvent.EditValue).AddDays(Convert.ToInt32(Interval) * 7);
                        break;
                    case RefEquipmentCard.Enums.Units.Months:
                        this.NextDateOfEvent.EditValue = ((DateTime)this.DateOfEvent.EditValue).AddMonths(Convert.ToInt32(Interval));
                        break;
                    case RefEquipmentCard.Enums.Units.Years:
                        this.NextDateOfEvent.EditValue = ((DateTime)this.DateOfEvent.EditValue).AddYears(Convert.ToInt32(Interval));
                        break;
                }
            }
            else
            {
                this.NextDateOfEvent.EditValue = null;
            }
        }
        /// <summary>
        /// Вычисляет такое расположение формы, чтобы форма располагась как можно ближе к курсору.
        /// </summary>
        /// <param name="FormSize">Размер формы.</param>
        /// <returns></returns>
        internal static Point ComputeLocation(Size FormSize)
        {
            Point location = Cursor.Position;

            Int32 w = Screen.PrimaryScreen.Bounds.Size.Width - location.X;
            Int32 h = Screen.PrimaryScreen.Bounds.Size.Height - location.Y;

            return new Point(w < FormSize.Width ? location.X - (FormSize.Width - w) : location.X, h < FormSize.Height ? location.Y - (FormSize.Height - h) : location.Y);
        }
    }
}
