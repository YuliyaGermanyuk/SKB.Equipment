using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout.Utils;

using DocsVision.BackOffice.ObjectModel;
using DocsVision.BackOffice.WinForms;
using DocsVision.BackOffice.WinForms.Design.LayoutItems;
using DocsVision.BackOffice.WinForms.Design.PropertyControls;
using DocsVision.Platform.CardHost;
using DocsVision.Platform.WinForms;

using SKB.Base;
using SKB.Base.Dictionary;
using SKB.Base.Services;
using SKB.Base.Ref;
using SKB.Base.Synchronize;
using SKB.Equipment;

using RKIT.MyCollectionControl.Design.Control;
using RKIT.MyMessageBox;
using DevExpress.Utils;
using DevExpress.XtraEditors.Repository;
using DocsVision.ExtendedToolkit.MyRichEditControl.Design.Control;
using SKB.Equipment.Forms;

namespace SKB.Equipment.Cards
{
    /// <summary>
    /// Карточка "Оборудование".
    /// </summary>
    public class EquipmentCard : MyBaseCard, IUploadingCard
    {
        #region Fields
        ///
        /// КОНТРОЛЫ ДЛЯ ВЫГРУЗКИ ФАЙЛОВ В АРХИВНУЮ ПАПКУ НА СЕРВЕРЕ
        /// 
        /// <summary>
        /// Коллекционный контрол категорий карточки
        /// </summary>
        private CollectionControlView control_Categories;
        /// <summary>
        /// Кнопка "Загрузить файлы"
        /// </summary>
        private SimpleButton button_Upload;
        /// <summary>
        /// Поле "Файлы"
        /// </summary>
        private TextEdit edit_Files;
        /// <summary>
        /// Поле "Папка"
        /// </summary>
        private TextEdit edit_Folder;
        ///
        /// СИНХРОНИЗАЦИЯ РЕКВИЗИТОВ КАРТОЧКИ С АРХИВНОЙ ПАПКОЙ НА СЕРВЕРЕ
        /// 
        /// <summary>
        /// Требуется синхронизация реквизитов карточки с архивной папкой на сервере
        /// </summary>
        private bool needSynchronize;
        /// <summary>
        /// Изменения реквизитов карточки
        /// </summary>
        private Dictionary<string, ChangingValue<string>> changes;
        #endregion

        #region Properties
        ///
        /// КОНТРОЛЫ ДЛЯ ВЫГРУЗКИ ФАЙЛОВ В АРХИВНУЮ ПАПКУ НА СЕРВЕРЕ 
        ///
        /// <summary>
        /// Коллекционный контрол категорий карточки
        /// </summary>
        public CollectionControlView Control_Categories
        {
            get { return control_Categories; }
        }
        /// <summary>
        /// Кнопка "Загрузить файлы"
        /// </summary>
        public SimpleButton Button_Upload
        { 
            get { return button_Upload; } 
        }
        /// <summary>
        /// Поле "Файлы"
        /// </summary>
        public TextEdit Edit_Files
        { 
            get { return edit_Files; } 
        }
        /// <summary>
        /// Поле "Папка"
        /// </summary>
        public TextEdit Edit_Folder
        {
            get { return edit_Folder; }
        }
        ///
        /// СТРУКТУРА АРХИВА НА СЕРВЕРЕ
        /// 
        /// <summary>
        /// Полный путь к архивной папке на сервере
        /// </summary>
        public string FolderPath
        {
            get { return (GetControlValue(RefEquipmentCard.MainInfo.Folder) ?? String.Empty).ToString(); }
            set { SetControlValue(RefEquipmentCard.MainInfo.Folder, value); }
        }
        /// <summary>
        /// Название раздела в архиве на сервере
        /// </summary>
        public string ArchiveSection
        {
            get { return "EQUIPMENT"; }
        }
        /// <summary>
        /// Название подраздела в архиве на сервере.
        /// </summary>
        public string ArchiveSubSection 
        {
            get { return Control_Categories.Text; }
        }
        /// <summary>
        /// Название регистрационной папки на сервере.
        /// </summary>
        public string FolderName 
        {
            get { return String.Format("{0}. {1} [АН {2}]", GetControlValue(RefEquipmentCard.MainInfo.Name), GetControlValue(RefEquipmentCard.MainInfo.BrandModel), CurrentNumerator.Number); }
        }
        ///
        /// СИНХРОНИЗАЦИЯ РЕКВИЗИТОВ КАРТОЧКИ С АРХИВНОЙ ПАПКОЙ НА СЕРВЕРЕ
        /// 
        /// <summary>
        /// Перечень изменений карточки (Key - название свойства, Value - данные об изменении свойства)
        /// </summary>
        public Dictionary<string, ChangingValue<string>> Changes
        {
            get { return changes; }
            set { changes = value; }
        }
        /// <summary>
        /// Требуется синхронизация реквизитов карточки с архивной папкой на сервере
        /// </summary>
        public bool NeedSynchronize
        {
            get { return needSynchronize; }
            set { needSynchronize = value; }
        }
        /// 
        /// РЕЖИМЫ ОТКРЫТИЯ КАРТОЧКИ, ПОДДЕРЖИВАЮЩЕЙ ВЫГРУЗКУ ФАЙЛОВ В АРХИВНУЮ ПАПКУ НА СЕРВЕРЕ
        /// 
        /// <summary>
        /// Идентификатор режима открытия карточки "Открыть файлы"
        /// </summary>
        public Guid OpenFilesMode
        {
            get { return RefEquipmentCard.Modes.OpenFiles; }
        }
        /// <summary>
        /// Идентификатор режима открытия карточки "Открыть карточку и файлы"
        /// </summary>
        public Guid OpenCardAndFilesMode
        {
            get { return RefEquipmentCard.Modes.OpenCardAndFiles; }
        }

        ///
        /// ТАБЛИЧНЫЕ КАРТОЧКИ
        ///
        /// <summary>
        /// Таблица "Ответственные за оборудование".
        /// </summary>
        ITableControl Table_ResponsibleForEquipment;
        /// <summary>
        /// Таблица "Сведения об отказах и ремонтах".
        /// </summary>
        ITableControl Table_FailuresAndRepairs;
        /// <summary>
        /// Таблица "Длительное хранение".
        /// </summary>
        ITableControl Table_LongTermStorage;
        /// <summary>
        /// Таблица "Приборы".
        /// </summary>
        ITableControl Table_UsedForDevices;
        /// <summary>
        /// Таблица "График поверок".
        /// </summary>
        ITableControl Table_VerificationGraph;
        /// <summary>
        /// Таблица "График калибровок".
        /// </summary>
        ITableControl Table_CalibrationGraph;
        /// <summary>
        /// Таблица "График аттестаций".
        /// </summary>
        ITableControl Table_AttestationGraph;
        /// <summary>
        /// Таблица "График технического обслуживания".
        /// </summary>
        ITableControl Table_MaintenanceGraph;
        /// <summary>
        /// Таблица "Применяемость".
        /// </summary>
        ITableControl Table_Applicability;
        /// <summary>
        /// Таблица "Ответственные за оборудование".
        /// </summary>
        GridView Grid_ResponsibleForEquipment;
        /// <summary>
        /// Таблица "Сведения об отказах и ремонтах".
        /// </summary>
        GridView Grid_FailuresAndRepairs;
        /// <summary>
        /// Таблица "Длительное хранение".
        /// </summary>
        GridView Grid_LongTermStorage;
        /// <summary>
        /// Таблица "Приборы".
        /// </summary>
        GridView Grid_UsedForDevices;
        /// <summary>
        /// Таблица "График поверок".
        /// </summary>
        GridView Grid_VerificationGraph;
        /// <summary>
        /// Таблица "График калибровок".
        /// </summary>
        GridView Grid_CalibrationGraph;
        /// <summary>
        /// Таблица "График аттестаций".
        /// </summary>
        GridView Grid_AttestationGraph;
        /// <summary>
        /// Таблица "График технического обслуживания".
        /// </summary>
        GridView Grid_MaintenanceGraph;
        /// <summary>
        /// Таблица "Применяемость".
        /// </summary>
        GridView Grid_Applicability;
        /// <summary>
        /// Группа "Дополнительная информация"
        /// </summary>
        DevExpress.XtraLayout.LayoutControlGroup AddInfo_Group;
        /// <summary>
        /// Вкладка "Метрологическая лаборатория"
        /// </summary>
        DevExpress.XtraLayout.LayoutControlGroup MetrologicalLaboratory_Group;
        /// <summary>
        /// Форматированный текст "Назначение"
        /// </summary>
        MyRichEditControl control_PurposeFormatting;
        /// <summary>
        /// Форматированный текст "Технические характеристики"
        /// </summary>
        MyRichEditControl control_SpecificationsFormatting;
        /// <summary>
        /// Оборудование находится в метрологической лаборатории
        /// </summary>
        Boolean IsMetrologicalLaboratory
        {
            get
            {
                if (Table_ResponsibleForEquipment.RowCount != 0)
                {
                    if (Table_ResponsibleForEquipment.Select(RefEquipmentCard.ResponsibleForEquipment.Department).Any(r => r.ToGuid().Equals(MyHelper.RefStaff_MS)))
                        return true;
                    else
                        return false;
                }
                else
                {
                    return false;
                }
            }
        }
        #endregion

        /// <summary>
        /// Инициализирует карточку по заданным данным.
        /// </summary>
        /// <param name="ClassBase">Скрипт карточки.</param>
        /// 
        public EquipmentCard(ScriptClassBase ClassBase) : base(ClassBase)
        {
            /* Получение рабочих объектов */
            // Тестовый комментарий
            control_Categories = ICardControl.FindPropertyItem<CollectionControlView>(RefEquipmentCard.Categories.Alias);
            control_Categories.SingleResult = true;
            control_Categories.Properties.Buttons[0].Visible = true;

            control_PurposeFormatting = ICardControl.FindPropertyItem<MyRichEditControl>(RefEquipmentCard.TechnicalInformation.PurposeFormatting);
            control_SpecificationsFormatting = ICardControl.FindPropertyItem<MyRichEditControl>(RefEquipmentCard.TechnicalInformation.SpecificationsFormatting);

            button_Upload = ICardControl.FindPropertyItem<SimpleButton>(RefEquipmentCard.Buttons.Upload);
            edit_Files = ICardControl.FindPropertyItem<TextEdit>(RefEquipmentCard.MainInfo.Files);
            edit_Folder = ICardControl.FindPropertyItem<TextEdit>(RefEquipmentCard.MainInfo.Folder);

            Table_ResponsibleForEquipment = ICardControl.FindPropertyItem<ITableControl>(RefEquipmentCard.ResponsibleForEquipment.Alias);
            Table_FailuresAndRepairs = ICardControl.FindPropertyItem<ITableControl>(RefEquipmentCard.FailuresAndRepairs.Alias);
            Table_LongTermStorage = ICardControl.FindPropertyItem<ITableControl>(RefEquipmentCard.LongTermStorage.Alias);

            Table_UsedForDevices = ICardControl.FindPropertyItem<ITableControl>(RefEquipmentCard.UsedForDevices.Alias);
            Table_Applicability = ICardControl.FindPropertyItem<ITableControl>(RefEquipmentCard.Applicability.Alias);
            
            Table_VerificationGraph = ICardControl.FindPropertyItem<ITableControl>(RefEquipmentCard.VerificationGraph.Alias);
            Table_CalibrationGraph = ICardControl.FindPropertyItem<ITableControl>(RefEquipmentCard.CalibrationGraph.Alias);
            Table_AttestationGraph = ICardControl.FindPropertyItem<ITableControl>(RefEquipmentCard.AttestationGraph.Alias);
            Table_MaintenanceGraph = ICardControl.FindPropertyItem<ITableControl>(RefEquipmentCard.MaintenanceGraph.Alias);

            Grid_ResponsibleForEquipment = ICardControl.GetGridView(RefEquipmentCard.ResponsibleForEquipment.Alias);
            Grid_FailuresAndRepairs = ICardControl.GetGridView(RefEquipmentCard.FailuresAndRepairs.Alias);
            Grid_LongTermStorage = ICardControl.GetGridView(RefEquipmentCard.LongTermStorage.Alias);

            Grid_UsedForDevices = ICardControl.GetGridView(RefEquipmentCard.UsedForDevices.Alias);
            Grid_Applicability = ICardControl.GetGridView(RefEquipmentCard.Applicability.Alias);

            Grid_VerificationGraph = ICardControl.GetGridView(RefEquipmentCard.VerificationGraph.Alias);
            Grid_CalibrationGraph = ICardControl.GetGridView(RefEquipmentCard.CalibrationGraph.Alias);
            Grid_AttestationGraph = ICardControl.GetGridView(RefEquipmentCard.AttestationGraph.Alias);
            Grid_MaintenanceGraph = ICardControl.GetGridView(RefEquipmentCard.MaintenanceGraph.Alias);

            AddInfo_Group = ICardControl.LayoutControl.Items.OfType<DevExpress.XtraLayout.LayoutControlGroup>().First(r => r.Text == "Дополнительная информация");
            MetrologicalLaboratory_Group = ICardControl.LayoutControl.Items.OfType<DevExpress.XtraLayout.LayoutControlGroup>().First(r => r.Text == "Метрологическая лаборатория");

            /* Назначение прав */
            NeedAssign = CardScript.CardControl.ActivateFlags.HasFlag(ActivateFlags.New) || CardScript.CardControl.ActivateFlags.HasFlag(ActivateFlags.NewFromTemplate);
            NeedSynchronize = false;
            
            /*WriteLog("Определяем режим открытия.");
            // Режим открытия карточки
            if (!NeedAssign)
            {

                if (CardScript.CardControl.ModeId == this.OpenFilesMode || CardScript.CardControl.ModeId.IsEmpty())
                {
                    WriteLog("По умолчанию открываем папку.");
                    if (MyHelper.OpenFolder(this.FolderPath).HasValue)
                        CardScript.CardFrame.Close();
                }
                else if (CardScript.CardControl.ModeId == this.OpenCardAndFilesMode)
                    MyHelper.OpenFolder((GetControlValue(this.FolderPath) ?? String.Empty).ToString());
            }
            WriteLog("Режим определен.");*/
            /* Привязка методов */
            if (!IsReadOnly)
            {
                CardScript.CardControl.CardClosed -= CardControl_CardClosed;
                CardScript.CardControl.CardClosed += CardControl_CardClosed;
                CardScript.CardControl.Saved -= CardControl_Saved;
                CardScript.CardControl.Saved += CardControl_Saved;
                CardScript.CardControl.Saving -= CardControl_Saving;
                CardScript.CardControl.Saving += CardControl_Saving;
                Button_Upload.Click -= Button_Upload_Click;
                Button_Upload.Click += Button_Upload_Click;
                Edit_Files.DoubleClick -= Edit_Files_DoubleClick;
                Edit_Files.DoubleClick += Edit_Files_DoubleClick;

                Grid_ResponsibleForEquipment.AddDoubleClickHandler(new ResponsibleForEquipment_DoubleClickDelegate(ResponsibleForEquipment_DoubleClick));
                Grid_UsedForDevices.AddDoubleClickHandler(new Devices_DoubleClickDelegate(Devices_DoubleClick));
                Grid_Applicability.AddDoubleClickHandler(new Applicability_DoubleClickDelegate(Applicability_DoubleClick));
                Grid_VerificationGraph.AddDoubleClickHandler(new VerificationGraph_DoubleClickDelegate(VerificationGraph_DoubleClick));
                Grid_CalibrationGraph.AddDoubleClickHandler(new VerificationGraph_DoubleClickDelegate(CalibrationGraph_DoubleClick));
                Grid_AttestationGraph.AddDoubleClickHandler(new VerificationGraph_DoubleClickDelegate(AttestationGraph_DoubleClick));
                Grid_MaintenanceGraph.AddDoubleClickHandler(new VerificationGraph_DoubleClickDelegate(MaintenanceGraph_DoubleClick));

                control_Categories.ValueChanged -= Categories_ValueChanged;
                control_Categories.ValueChanged += Categories_ValueChanged;

                // Кнопки таблицы "Ответственные за оборудование"
                ICardControl.RemoveTableBarItem(RefEquipmentCard.ResponsibleForEquipment.Alias, 4);
                ICardControl.RemoveTableBarItem(RefEquipmentCard.ResponsibleForEquipment.Alias, 3);
                ICardControl.AddTableBarItem(RefEquipmentCard.ResponsibleForEquipment.Alias, "Добавить", MyHelper.Image_Table_Add).ItemClick += AddResponsibleForEquipmentButton_ItemClick;
                ICardControl.AddTableBarItem(RefEquipmentCard.ResponsibleForEquipment.Alias, "Удалить", MyHelper.Image_Table_Remove).ItemClick += RemoveResponsibleForEquipmentButton_ItemClick;

                // Кнопки таблицы "Сведения об отказах и ремонтах"
                ICardControl.RemoveTableBarItem(RefEquipmentCard.FailuresAndRepairs.Alias, 4);
                ICardControl.RemoveTableBarItem(RefEquipmentCard.FailuresAndRepairs.Alias, 3);
                ICardControl.AddTableBarItem(RefEquipmentCard.FailuresAndRepairs.Alias, "Добавить", MyHelper.Image_Table_Add).ItemClick += AddFailuresAndRepairsButton_ItemClick;
                ICardControl.AddTableBarItem(RefEquipmentCard.FailuresAndRepairs.Alias, "Удалить", MyHelper.Image_Table_Remove).ItemClick += RemoveFailuresAndRepairsButton_ItemClick;

                // Кнопки таблицы "Длительное хранение"
                ICardControl.RemoveTableBarItem(RefEquipmentCard.LongTermStorage.Alias, 4);
                ICardControl.RemoveTableBarItem(RefEquipmentCard.LongTermStorage.Alias, 3);
                ICardControl.AddTableBarItem(RefEquipmentCard.LongTermStorage.Alias, "Добавить", MyHelper.Image_Table_Add).ItemClick += AddLongTermStorageButton_ItemClick;
                ICardControl.AddTableBarItem(RefEquipmentCard.LongTermStorage.Alias, "Удалить", MyHelper.Image_Table_Remove).ItemClick += RemoveLongTermStorageButton_ItemClick;

                // Кнопки таблицы "Приборы"
                ICardControl.RemoveTableBarItem(RefEquipmentCard.UsedForDevices.Alias, 4);
                ICardControl.RemoveTableBarItem(RefEquipmentCard.UsedForDevices.Alias, 3);
                ICardControl.AddTableBarItem(RefEquipmentCard.UsedForDevices.Alias, "Добавить", MyHelper.Image_Table_Add).ItemClick += AddUsedForDevicesButton_ItemClick;
                ICardControl.AddTableBarItem(RefEquipmentCard.UsedForDevices.Alias, "Удалить", MyHelper.Image_Table_Remove).ItemClick += RemoveUsedForDevicesButton_ItemClick;

                // Кнопки таблицы "Применяемость"
                ICardControl.RemoveTableBarItem(RefEquipmentCard.Applicability.Alias, 4);
                ICardControl.RemoveTableBarItem(RefEquipmentCard.Applicability.Alias, 3);
                ICardControl.AddTableBarItem(RefEquipmentCard.Applicability.Alias, "Добавить", MyHelper.Image_Table_Add).ItemClick += AddApplicabilityButton_ItemClick;
                ICardControl.AddTableBarItem(RefEquipmentCard.Applicability.Alias, "Удалить", MyHelper.Image_Table_Remove).ItemClick += RemoveApplicabilityButton_ItemClick;

                // Кнопки таблицы "График поверок"
                ICardControl.RemoveTableBarItem(RefEquipmentCard.VerificationGraph.Alias, 4);
                ICardControl.RemoveTableBarItem(RefEquipmentCard.VerificationGraph.Alias, 3);
                ICardControl.AddTableBarItem(RefEquipmentCard.VerificationGraph.Alias, "Добавить", MyHelper.Image_Table_Add).ItemClick += AddVerificationGraphButton_ItemClick;
                ICardControl.AddTableBarItem(RefEquipmentCard.VerificationGraph.Alias, "Удалить", MyHelper.Image_Table_Remove).ItemClick += RemoveVerificationGraphButton_ItemClick;

                // Кнопки таблицы "График калибровок"
                ICardControl.RemoveTableBarItem(RefEquipmentCard.CalibrationGraph.Alias, 4);
                ICardControl.RemoveTableBarItem(RefEquipmentCard.CalibrationGraph.Alias, 3);
                ICardControl.AddTableBarItem(RefEquipmentCard.CalibrationGraph.Alias, "Добавить", MyHelper.Image_Table_Add).ItemClick += AddCalibrationGraphButton_ItemClick;
                ICardControl.AddTableBarItem(RefEquipmentCard.CalibrationGraph.Alias, "Удалить", MyHelper.Image_Table_Remove).ItemClick += RemoveCalibrationGraphButton_ItemClick;

                // Кнопки таблицы "График аттестаций"
                ICardControl.RemoveTableBarItem(RefEquipmentCard.AttestationGraph.Alias, 4);
                ICardControl.RemoveTableBarItem(RefEquipmentCard.AttestationGraph.Alias, 3);
                ICardControl.AddTableBarItem(RefEquipmentCard.AttestationGraph.Alias, "Добавить", MyHelper.Image_Table_Add).ItemClick += AddAttestationGraphButton_ItemClick;
                ICardControl.AddTableBarItem(RefEquipmentCard.AttestationGraph.Alias, "Удалить", MyHelper.Image_Table_Remove).ItemClick += RemoveAttestationGraphButton_ItemClick;

                // Кнопки таблицы "График технического обслуживания"
                ICardControl.RemoveTableBarItem(RefEquipmentCard.MaintenanceGraph.Alias, 4);
                ICardControl.RemoveTableBarItem(RefEquipmentCard.MaintenanceGraph.Alias, 3);
                ICardControl.AddTableBarItem(RefEquipmentCard.MaintenanceGraph.Alias, "Добавить", MyHelper.Image_Table_Add).ItemClick += AddMaintenanceGraphButton_ItemClick;
                ICardControl.AddTableBarItem(RefEquipmentCard.MaintenanceGraph.Alias, "Удалить", MyHelper.Image_Table_Remove).ItemClick += RemoveMaintenanceGraphButton_ItemClick;
            }

            /* Получение номера */
            if (GetControlValue(RefEquipmentCard.MainInfo.ArchiveNumber).ToGuid().IsEmpty())
            {
                CurrentNumerator = CardScript.GetNumber(RefEquipmentCard.ArchiveNumberRuleName);
                CurrentNumerator.Number = Convert.ToInt32(CurrentNumerator.Number).ToString("00000");
                SetControlValue(RefEquipmentCard.MainInfo.ArchiveNumber, Context.GetObjectRef<BaseCardNumber>(CurrentNumerator).Id);
                WriteLog("Выдали номер: " + CurrentNumerator.Number);
            }
            else
            {
                CurrentNumerator = Context.GetObject<BaseCardNumber>(GetControlValue(RefEquipmentCard.MainInfo.ArchiveNumber));
            }

            // Настройка внешнего вида
            Customize();

            // Скрытие поля "Папка"
            UploadingControlsCustomize();
            WriteLog(@"Сокрытие поля 'папка'.");
            // Обновление перечня изменений
            changes = new Dictionary<string, ChangingValue<string>>();
            RefreshChanges();
            WriteLog(@"Обновление перечня изменений.");
        }

        #region Delegates

        private delegate void ResponsibleForEquipment_DoubleClickDelegate(Object sender, EventArgs e);
        private delegate void Devices_DoubleClickDelegate(Object sender, EventArgs e);
        private delegate void Applicability_DoubleClickDelegate(Object sender, EventArgs e);
        private delegate void VerificationGraph_DoubleClickDelegate(Object sender, EventArgs e);

        #endregion

        #region Methods
        /// <summary>
        /// Настройка внешнего вида.
        /// </summary>
        public override void Customize()
        {
            // Настройка внешнего вида таблиц

            #region Таблица "Ответственные за оборудование"
            // Заголовки столбцов
            foreach (DevExpress.XtraGrid.Columns.GridColumn iCol in Grid_ResponsibleForEquipment.Columns)
                iCol.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            // Ячейки столбцов
            Grid_ResponsibleForEquipment.Columns[RefEquipmentCard.ResponsibleForEquipment.CommissioningDate].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            Grid_ResponsibleForEquipment.Columns[RefEquipmentCard.ResponsibleForEquipment.DecommissioningDate].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            Grid_ResponsibleForEquipment.Columns[RefEquipmentCard.ResponsibleForEquipment.Department].AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            Grid_ResponsibleForEquipment.Columns[RefEquipmentCard.ResponsibleForEquipment.Employee].AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            Grid_ResponsibleForEquipment.Columns[RefEquipmentCard.ResponsibleForEquipment.Position].AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            // Столбец "Дата ввода в эксплуатацию"
            if (Grid_ResponsibleForEquipment.Columns[RefEquipmentCard.ResponsibleForEquipment.CommissioningDate].ColumnEdit is RepositoryItemDateEdit)
            {
                RepositoryItemDateEdit Repository = Grid_ResponsibleForEquipment.Columns[RefEquipmentCard.ResponsibleForEquipment.CommissioningDate].ColumnEdit as RepositoryItemDateEdit;
                Repository.Mask.EditMask = "dd.MM.yyyy";
                Repository.Mask.UseMaskAsDisplayFormat = true;
            }
            // Столбец "Дата вывода из эксплуатации"
            if (Grid_ResponsibleForEquipment.Columns[RefEquipmentCard.ResponsibleForEquipment.DecommissioningDate].ColumnEdit is RepositoryItemDateEdit)
            {
                RepositoryItemDateEdit Repository = Grid_ResponsibleForEquipment.Columns[RefEquipmentCard.ResponsibleForEquipment.DecommissioningDate].ColumnEdit as RepositoryItemDateEdit;
                Repository.Mask.EditMask = "dd.MM.yyyy";
                Repository.Mask.UseMaskAsDisplayFormat = true;
            }
            // Сокрытие дефолтных кнопок
            ICardControl.HideTableBarItem(RefEquipmentCard.ResponsibleForEquipment.Alias, 0, true);
            ICardControl.HideTableBarItem(RefEquipmentCard.ResponsibleForEquipment.Alias, 1, true);
            ICardControl.HideTableBarItem(RefEquipmentCard.ResponsibleForEquipment.Alias, 2, true);
            // Кнопка "Удалить" блокируется при отсутствии строк
            if (Table_ResponsibleForEquipment.RowCount == 0) ICardControl.DisableTableBarItem(RefEquipmentCard.ResponsibleForEquipment.Alias, 4, true);
            // Кнопки "Добавить" и "Удалить" блокируются при отсутствии прав на операцию редактирования таблицы
            if (!Context.IsOperationAllowed(CardScript.BaseObject, RefEquipmentCard.ResponsibleForEquipment.Alias))
            {
                ICardControl.DisableTableBarItem(RefEquipmentCard.ResponsibleForEquipment.Alias, 3, true);
                ICardControl.DisableTableBarItem(RefEquipmentCard.ResponsibleForEquipment.Alias, 4, true);
            }
            #endregion

            #region Таблица "Сведения об отказах и ремонтах"
            // Заголовки столбцов
            foreach (DevExpress.XtraGrid.Columns.GridColumn iCol in Grid_FailuresAndRepairs.Columns)
                iCol.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            // Ячейки столбцов
            Grid_FailuresAndRepairs.Columns[RefEquipmentCard.FailuresAndRepairs.FailureDate].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            Grid_FailuresAndRepairs.Columns[RefEquipmentCard.FailuresAndRepairs.FailureDescription].AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            Grid_FailuresAndRepairs.Columns[RefEquipmentCard.FailuresAndRepairs.Employee].AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            // Столбец "Дата"
            if (Grid_FailuresAndRepairs.Columns[RefEquipmentCard.FailuresAndRepairs.FailureDate].ColumnEdit is RepositoryItemDateEdit)
            {
                RepositoryItemDateEdit Repository = Grid_FailuresAndRepairs.Columns[RefEquipmentCard.FailuresAndRepairs.FailureDate].ColumnEdit as RepositoryItemDateEdit;
                Repository.Mask.EditMask = "dd.MM.yyyy";
                Repository.Mask.UseMaskAsDisplayFormat = true;
            }
            // Сокрытие дефолтных кнопок
            ICardControl.HideTableBarItem(RefEquipmentCard.FailuresAndRepairs.Alias, 0, true);
            ICardControl.HideTableBarItem(RefEquipmentCard.FailuresAndRepairs.Alias, 1, true);
            ICardControl.HideTableBarItem(RefEquipmentCard.FailuresAndRepairs.Alias, 2, true);
            // Кнопка "Удалить" блокируется при отсутствии строк
            if (Table_FailuresAndRepairs.RowCount == 0) ICardControl.DisableTableBarItem(RefEquipmentCard.FailuresAndRepairs.Alias, 4, true);
            // Кнопки "Добавить" и "Удалить" блокируются при отсутствии прав на операцию редактирования таблицы
            if (!Context.IsOperationAllowed(CardScript.BaseObject, RefEquipmentCard.FailuresAndRepairs.Alias))
            {
                ICardControl.DisableTableBarItem(RefEquipmentCard.FailuresAndRepairs.Alias, 3, true);
                ICardControl.DisableTableBarItem(RefEquipmentCard.FailuresAndRepairs.Alias, 4, true);
            }
            #endregion

            #region Таблица "Длительное хранение"
            // Заголовки столбцов
            foreach (DevExpress.XtraGrid.Columns.GridColumn iCol in Grid_LongTermStorage.Columns)
                iCol.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            // Ячейки столбцов
            Grid_LongTermStorage.Columns[RefEquipmentCard.LongTermStorage.StartDate].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            Grid_LongTermStorage.Columns[RefEquipmentCard.LongTermStorage.EndDate].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            // Столбец "Дата начала хранения"
            if (Grid_LongTermStorage.Columns[RefEquipmentCard.LongTermStorage.StartDate].ColumnEdit is RepositoryItemDateEdit)
            {
                RepositoryItemDateEdit Repository = Grid_LongTermStorage.Columns[RefEquipmentCard.LongTermStorage.StartDate].ColumnEdit as RepositoryItemDateEdit;
                Repository.Mask.EditMask = "dd.MM.yyyy";
                Repository.Mask.UseMaskAsDisplayFormat = true;
            }
            // Столбец "Дата окончания хранения"
            if (Grid_LongTermStorage.Columns[RefEquipmentCard.LongTermStorage.EndDate].ColumnEdit is RepositoryItemDateEdit)
            {
                RepositoryItemDateEdit Repository = Grid_LongTermStorage.Columns[RefEquipmentCard.LongTermStorage.EndDate].ColumnEdit as RepositoryItemDateEdit;
                Repository.Mask.EditMask = "dd.MM.yyyy";
                Repository.Mask.UseMaskAsDisplayFormat = true;
            }
            // Сокрытие дефолтных кнопок
            ICardControl.HideTableBarItem(RefEquipmentCard.LongTermStorage.Alias, 0, true);
            ICardControl.HideTableBarItem(RefEquipmentCard.LongTermStorage.Alias, 1, true);
            ICardControl.HideTableBarItem(RefEquipmentCard.LongTermStorage.Alias, 2, true);
            // Кнопка "Удалить" блокируется при отсутствии строк
            if (Table_LongTermStorage.RowCount == 0) ICardControl.DisableTableBarItem(RefEquipmentCard.LongTermStorage.Alias, 4, true);
            // Кнопки "Добавить" и "Удалить" блокируются при отсутствии прав на операцию редактирования таблицы
            if (!Context.IsOperationAllowed(CardScript.BaseObject, RefEquipmentCard.LongTermStorage.Alias))
            {
                ICardControl.DisableTableBarItem(RefEquipmentCard.LongTermStorage.Alias, 3, true);
                ICardControl.DisableTableBarItem(RefEquipmentCard.LongTermStorage.Alias, 4, true);
            }
            #endregion

            #region Таблица "Приборы"
            // Заголовки столбцов
            foreach (DevExpress.XtraGrid.Columns.GridColumn iCol in Grid_UsedForDevices.Columns)
                iCol.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            // Ячейки столбцов
            Grid_UsedForDevices.Columns[RefEquipmentCard.UsedForDevices.Device].AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            Grid_UsedForDevices.Columns[RefEquipmentCard.UsedForDevices.Calibration].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            Grid_UsedForDevices.Columns[RefEquipmentCard.UsedForDevices.Verify].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            // Сокрытие дефолтных кнопок
            ICardControl.HideTableBarItem(RefEquipmentCard.UsedForDevices.Alias, 0, true);
            ICardControl.HideTableBarItem(RefEquipmentCard.UsedForDevices.Alias, 1, true);
            ICardControl.HideTableBarItem(RefEquipmentCard.UsedForDevices.Alias, 2, true);
            // Кнопка "Удалить" блокируется при отсутствии строк
            if (Table_UsedForDevices.RowCount == 0) ICardControl.DisableTableBarItem(RefEquipmentCard.UsedForDevices.Alias, 4, true);
            // Кнопки "Добавить" и "Удалить" блокируются при отсутствии прав на операцию редактирования таблицы
            if (!Context.IsOperationAllowed(CardScript.BaseObject, RefEquipmentCard.UsedForDevices.Alias))
            {
                ICardControl.DisableTableBarItem(RefEquipmentCard.UsedForDevices.Alias, 3, true);
                ICardControl.DisableTableBarItem(RefEquipmentCard.UsedForDevices.Alias, 4, true);
            }
            #endregion

            #region Таблица "Применяемость"
            // Заголовки столбцов
            foreach (DevExpress.XtraGrid.Columns.GridColumn iCol in Grid_Applicability.Columns)
                iCol.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            // Сокрытие дефолтных кнопок
            ICardControl.HideTableBarItem(RefEquipmentCard.Applicability.Alias, 0, true);
            ICardControl.HideTableBarItem(RefEquipmentCard.Applicability.Alias, 1, true);
            ICardControl.HideTableBarItem(RefEquipmentCard.Applicability.Alias, 2, true);
            // Кнопка "Удалить" блокируется при отсутствии строк
            if (Table_Applicability.RowCount == 0) ICardControl.DisableTableBarItem(RefEquipmentCard.Applicability.Alias, 4, true);
            // Кнопки "Добавить" и "Удалить" блокируются при отсутствии прав на операцию редактирования таблицы
            if (!Context.IsOperationAllowed(CardScript.BaseObject, RefEquipmentCard.Applicability.Alias))
            {
                ICardControl.DisableTableBarItem(RefEquipmentCard.Applicability.Alias, 3, true);
                ICardControl.DisableTableBarItem(RefEquipmentCard.Applicability.Alias, 4, true);
            }
            #endregion

            #region Таблица "График поверок"
            // Заголовки столбцов
            foreach (DevExpress.XtraGrid.Columns.GridColumn iCol in Grid_VerificationGraph.Columns)
                iCol.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            // Ячейки столбцов
            Grid_VerificationGraph.Columns[RefEquipmentCard.VerificationGraph.DateOfVerification].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            Grid_VerificationGraph.Columns[RefEquipmentCard.VerificationGraph.DateOfNextVerification].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            Grid_VerificationGraph.Columns[RefEquipmentCard.VerificationGraph.VerificationDocument].AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            Grid_VerificationGraph.Columns[RefEquipmentCard.VerificationGraph.NumberOfDocument].AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            // Столбец "Дата проведения поверки"
            if (Grid_VerificationGraph.Columns[RefEquipmentCard.VerificationGraph.DateOfVerification].ColumnEdit is RepositoryItemDateEdit)
            {
                RepositoryItemDateEdit Repository = Grid_VerificationGraph.Columns[RefEquipmentCard.VerificationGraph.DateOfVerification].ColumnEdit as RepositoryItemDateEdit;
                Repository.Mask.EditMask = "dd.MM.yyyy";
                Repository.Mask.UseMaskAsDisplayFormat = true;
            }
            // Столбец "Дата следующей поверки"
            if (Grid_VerificationGraph.Columns[RefEquipmentCard.VerificationGraph.DateOfNextVerification].ColumnEdit is RepositoryItemDateEdit)
            {
                RepositoryItemDateEdit Repository = Grid_VerificationGraph.Columns[RefEquipmentCard.VerificationGraph.DateOfNextVerification].ColumnEdit as RepositoryItemDateEdit;
                Repository.Mask.EditMask = "dd.MM.yyyy";
                Repository.Mask.UseMaskAsDisplayFormat = true;
            }
            // Сокрытие дефолтных кнопок
            ICardControl.HideTableBarItem(RefEquipmentCard.VerificationGraph.Alias, 0, true);
            ICardControl.HideTableBarItem(RefEquipmentCard.VerificationGraph.Alias, 1, true);
            ICardControl.HideTableBarItem(RefEquipmentCard.VerificationGraph.Alias, 2, true);
            // Кнопка "Удалить" блокируется при отсутствии строк
            if (Table_VerificationGraph.RowCount == 0) ICardControl.DisableTableBarItem(RefEquipmentCard.VerificationGraph.Alias, 4, true);
            // Кнопки "Добавить" и "Удалить" блокируются при отсутствии прав на операцию редактирования таблицы
            if (!Context.IsOperationAllowed(CardScript.BaseObject, RefEquipmentCard.VerificationGraph.Alias))
            {
                ICardControl.DisableTableBarItem(RefEquipmentCard.VerificationGraph.Alias, 3, true);
                ICardControl.DisableTableBarItem(RefEquipmentCard.VerificationGraph.Alias, 4, true);
            }
            #endregion

            #region Таблица "График калибровок"
            // Заголовки столбцов
            foreach (DevExpress.XtraGrid.Columns.GridColumn iCol in Grid_CalibrationGraph.Columns)
                iCol.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            // Ячейки столбцов
            Grid_CalibrationGraph.Columns[RefEquipmentCard.CalibrationGraph.DateOfCalibration].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            Grid_CalibrationGraph.Columns[RefEquipmentCard.CalibrationGraph.DateOfNextCalibration].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            Grid_CalibrationGraph.Columns[RefEquipmentCard.CalibrationGraph.CalibrationDocument].AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            Grid_CalibrationGraph.Columns[RefEquipmentCard.CalibrationGraph.NumberOfDocument].AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            // Столбец "Дата проведения калибровки"
            if (Grid_CalibrationGraph.Columns[RefEquipmentCard.CalibrationGraph.DateOfCalibration].ColumnEdit is RepositoryItemDateEdit)
            {
                RepositoryItemDateEdit Repository = Grid_CalibrationGraph.Columns[RefEquipmentCard.CalibrationGraph.DateOfCalibration].ColumnEdit as RepositoryItemDateEdit;
                Repository.Mask.EditMask = "dd.MM.yyyy";
                Repository.Mask.UseMaskAsDisplayFormat = true;
            }
            // Столбец "Дата следующей калибровки"
            if (Grid_CalibrationGraph.Columns[RefEquipmentCard.CalibrationGraph.DateOfNextCalibration].ColumnEdit is RepositoryItemDateEdit)
            {
                RepositoryItemDateEdit Repository = Grid_CalibrationGraph.Columns[RefEquipmentCard.CalibrationGraph.DateOfNextCalibration].ColumnEdit as RepositoryItemDateEdit;
                Repository.Mask.EditMask = "dd.MM.yyyy";
                Repository.Mask.UseMaskAsDisplayFormat = true;
            }
            // Сокрытие дефолтных кнопок
            ICardControl.HideTableBarItem(RefEquipmentCard.CalibrationGraph.Alias, 0, true);
            ICardControl.HideTableBarItem(RefEquipmentCard.CalibrationGraph.Alias, 1, true);
            ICardControl.HideTableBarItem(RefEquipmentCard.CalibrationGraph.Alias, 2, true);
            // Кнопка "Удалить" блокируется при отсутствии строк
            if (Table_CalibrationGraph.RowCount == 0) ICardControl.DisableTableBarItem(RefEquipmentCard.CalibrationGraph.Alias, 4, true);
            // Кнопки "Добавить" и "Удалить" блокируются при отсутствии прав на операцию редактирования таблицы
            if (!Context.IsOperationAllowed(CardScript.BaseObject, RefEquipmentCard.CalibrationGraph.Alias))
            {
                ICardControl.DisableTableBarItem(RefEquipmentCard.CalibrationGraph.Alias, 3, true);
                ICardControl.DisableTableBarItem(RefEquipmentCard.CalibrationGraph.Alias, 4, true);
            }
            #endregion

            #region Таблица "График аттестаций"
            // Заголовки столбцов
            foreach (DevExpress.XtraGrid.Columns.GridColumn iCol in Grid_AttestationGraph.Columns)
                iCol.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            // Ячейки столбцов
            Grid_AttestationGraph.Columns[RefEquipmentCard.AttestationGraph.DateOfAttestation].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            Grid_AttestationGraph.Columns[RefEquipmentCard.AttestationGraph.DateOfNextAttestation].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            Grid_AttestationGraph.Columns[RefEquipmentCard.AttestationGraph.AttestationDocument].AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            Grid_AttestationGraph.Columns[RefEquipmentCard.AttestationGraph.NumberOfDocument].AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            // Столбец "Дата проведения аттестации"
            if (Grid_AttestationGraph.Columns[RefEquipmentCard.AttestationGraph.DateOfAttestation].ColumnEdit is RepositoryItemDateEdit)
            {
                RepositoryItemDateEdit Repository = Grid_AttestationGraph.Columns[RefEquipmentCard.AttestationGraph.DateOfAttestation].ColumnEdit as RepositoryItemDateEdit;
                Repository.Mask.EditMask = "dd.MM.yyyy";
                Repository.Mask.UseMaskAsDisplayFormat = true;
            }
            // Столбец "Дата следующей аттестации"
            if (Grid_AttestationGraph.Columns[RefEquipmentCard.AttestationGraph.DateOfNextAttestation].ColumnEdit is RepositoryItemDateEdit)
            {
                RepositoryItemDateEdit Repository = Grid_AttestationGraph.Columns[RefEquipmentCard.AttestationGraph.DateOfNextAttestation].ColumnEdit as RepositoryItemDateEdit;
                Repository.Mask.EditMask = "dd.MM.yyyy";
                Repository.Mask.UseMaskAsDisplayFormat = true;
            }
            // Сокрытие дефолтных кнопок
            ICardControl.HideTableBarItem(RefEquipmentCard.AttestationGraph.Alias, 0, true);
            ICardControl.HideTableBarItem(RefEquipmentCard.AttestationGraph.Alias, 1, true);
            ICardControl.HideTableBarItem(RefEquipmentCard.AttestationGraph.Alias, 2, true);
            // Кнопка "Удалить" блокируется при отсутствии строк
            if (Table_AttestationGraph.RowCount == 0) ICardControl.DisableTableBarItem(RefEquipmentCard.AttestationGraph.Alias, 4, true);
            // Кнопки "Добавить" и "Удалить" блокируются при отсутствии прав на операцию редактирования таблицы
            if (!Context.IsOperationAllowed(CardScript.BaseObject, RefEquipmentCard.AttestationGraph.Alias))
            {
                ICardControl.DisableTableBarItem(RefEquipmentCard.AttestationGraph.Alias, 3, true);
                ICardControl.DisableTableBarItem(RefEquipmentCard.AttestationGraph.Alias, 4, true);
            }
            #endregion

            #region Таблица "График технического обслуживания"
            // Заголовки столбцов
            foreach (DevExpress.XtraGrid.Columns.GridColumn iCol in Grid_MaintenanceGraph.Columns)
                iCol.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            // Ячейки столбцов
            Grid_MaintenanceGraph.Columns[RefEquipmentCard.MaintenanceGraph.DateOfMaintenance].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            Grid_MaintenanceGraph.Columns[RefEquipmentCard.MaintenanceGraph.DateOfNextMaintenance].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            Grid_MaintenanceGraph.Columns[RefEquipmentCard.MaintenanceGraph.BaseDocument].AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            Grid_MaintenanceGraph.Columns[RefEquipmentCard.MaintenanceGraph.Employee].AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            Grid_MaintenanceGraph.Columns[RefEquipmentCard.MaintenanceGraph.Position].AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            // Столбец "Дата проведения технического обслуживания"
            if (Grid_MaintenanceGraph.Columns[RefEquipmentCard.MaintenanceGraph.DateOfMaintenance].ColumnEdit is RepositoryItemDateEdit)
            {
                RepositoryItemDateEdit Repository = Grid_MaintenanceGraph.Columns[RefEquipmentCard.MaintenanceGraph.DateOfMaintenance].ColumnEdit as RepositoryItemDateEdit;
                Repository.Mask.EditMask = "dd.MM.yyyy";
                Repository.Mask.UseMaskAsDisplayFormat = true;
            }
            // Столбец "Дата следующего технического обслуживания"
            if (Grid_MaintenanceGraph.Columns[RefEquipmentCard.MaintenanceGraph.DateOfNextMaintenance].ColumnEdit is RepositoryItemDateEdit)
            {
                RepositoryItemDateEdit Repository = Grid_MaintenanceGraph.Columns[RefEquipmentCard.MaintenanceGraph.DateOfNextMaintenance].ColumnEdit as RepositoryItemDateEdit;
                Repository.Mask.EditMask = "dd.MM.yyyy";
                Repository.Mask.UseMaskAsDisplayFormat = true;
            }
            // Сокрытие дефолтных кнопок
            ICardControl.HideTableBarItem(RefEquipmentCard.MaintenanceGraph.Alias, 0, true);
            ICardControl.HideTableBarItem(RefEquipmentCard.MaintenanceGraph.Alias, 1, true);
            ICardControl.HideTableBarItem(RefEquipmentCard.MaintenanceGraph.Alias, 2, true);
            // Кнопка "Удалить" блокируется при отсутствии строк
            if (Table_MaintenanceGraph.RowCount == 0) ICardControl.DisableTableBarItem(RefEquipmentCard.MaintenanceGraph.Alias, 4, true);
            // Кнопки "Добавить" и "Удалить" блокируются при отсутствии прав на операцию редактирования таблицы
            if (!Context.IsOperationAllowed(CardScript.BaseObject, RefEquipmentCard.MaintenanceGraph.Alias))
            {
                ICardControl.DisableTableBarItem(RefEquipmentCard.MaintenanceGraph.Alias, 3, true);
                ICardControl.DisableTableBarItem(RefEquipmentCard.MaintenanceGraph.Alias, 4, true);
            }
            #endregion

            AddInfoCustomize();
        }
        /// <summary>
        /// Настройка внешнего вида полей выгрузки файлов на сервер.
        /// </summary>
        public void UploadingControlsCustomize()
        {
            if (!((IPropertyControl)Edit_Folder).AllowEdit)
                ICardControl.FindLayoutItem(Edit_Folder.Name).Visibility = LayoutVisibility.OnlyInCustomization;
        }
        /// <summary>
        /// Настройка области дополнительной информации.
        /// </summary>
        public void AddInfoCustomize()
        {
            // Настройка группы "Дополнительная информация"
            if (IsMetrologicalLaboratory)
            {
                if (!AddInfo_Group.IsNull()) AddInfo_Group.Expanded = true;
                if (!MetrologicalLaboratory_Group.IsNull())
                {
                    MetrologicalLaboratory_Group.Enabled = true;
                    MetrologicalLaboratory_Group.Selected = true;
                }
            }
            else
            {
                if (!AddInfo_Group.IsNull()) AddInfo_Group.Expanded = false;
                if (!MetrologicalLaboratory_Group.IsNull())
                {
                    if ((MetrologicalLaboratory_Group.Enabled == true) && (GetControlValue(RefEquipmentCard.MetrologicalLaboratory.NumberOfRegistrationСard) != null) 
                        && (GetControlValue(RefEquipmentCard.MetrologicalLaboratory.NumberOfRegistrationСard).ToGuid() != Guid.Empty))
                    {
                        MyHelper.ReleaseNumber(CardScript, Context.GetObject<BaseCardNumber>(GetControlValue(RefEquipmentCard.MetrologicalLaboratory.NumberOfRegistrationСard)));
                        SetControlValue(RefEquipmentCard.MetrologicalLaboratory.NumberOfRegistrationСard, null);
                    }
                    MetrologicalLaboratory_Group.Enabled = false;
                }
            }
        }
        /// <summary>
        /// Проверяет правильность заполнения реквизитов перед сохранением карточки и выгрузкой файлов на сервер
        /// </summary>
        /// <returns></returns>
        public bool Check()
        {
            try
            {
                changes[RefEquipmentCard.MainInfo.Name].NewValue = (GetControlValue(RefEquipmentCard.MainInfo.Name) ?? String.Empty).ToString();
                changes[RefEquipmentCard.MainInfo.BrandModel].NewValue = (GetControlValue(RefEquipmentCard.MainInfo.BrandModel) ?? String.Empty).ToString();
                changes[RefEquipmentCard.MainInfo.ArchiveNumber].NewValue = (GetControlValue(RefEquipmentCard.MainInfo.ArchiveNumber) ?? String.Empty).ToString();
                changes[RefEquipmentCard.Categories.CategoriesValue].NewValue = Control_Categories.Text;

                // Проверка названия оборудования
                if (String.IsNullOrWhiteSpace(changes[RefEquipmentCard.MainInfo.Name].NewValue))
                    throw new MyException("Введите название оборудования!");
                else if (changes[RefEquipmentCard.MainInfo.Name].NewValue.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                    throw new MyException("Название оборудования содержит недопустимые символы!");

                // Проверка марки/модели оборудования
                if (String.IsNullOrWhiteSpace(changes[RefEquipmentCard.MainInfo.BrandModel].NewValue))
                    throw new MyException("Введите марку/модель оборудования!");
                else if (changes[RefEquipmentCard.MainInfo.BrandModel].NewValue.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                    throw new MyException("Марка/модель оборудования содержит недопустимые символы!");

                // Проверка архивного номера
                if (String.IsNullOrWhiteSpace(changes[RefEquipmentCard.MainInfo.ArchiveNumber].NewValue))
                    throw new MyException("Введите архивный номер оборудования!");
                else if (changes[RefEquipmentCard.MainInfo.ArchiveNumber].NewValue.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                    throw new MyException("Архивный номер оборудования содержит недопустимые символы!");

                // Проверка типа документа
                if (!Control_Categories.SelectedItems.Any())
                    throw new MyException("Укажите тип оборудования!");
                else if (Control_Categories.Text.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                    throw new MyException("Тип оборудования содержит недопустимые символы!");
                return true;
            }
            catch (MyException Ex)
            {
                MyMessageBox.Show(Ex.Message, "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }
        /// <summary>
        /// Обновляет список изменений.
        /// </summary>
        public override void RefreshChanges()
        {
            WriteLog(@"Название.");
            String S = (GetControlValue(RefEquipmentCard.MainInfo.Name) ?? String.Empty).ToString();
            WriteLog(@"Определили название.");
            ChangingValue<String> Cange_Name = new ChangingValue<String>(S);
            WriteLog(@"Создали изменение.");
            changes.Add(RefEquipmentCard.MainInfo.Name, Cange_Name);
            WriteLog(@"Марка.");
            changes.Add(RefEquipmentCard.MainInfo.BrandModel, new ChangingValue<String>((GetControlValue(RefEquipmentCard.MainInfo.BrandModel) ?? String.Empty).ToString()));
            WriteLog(@"Архивный номер.");
            changes.Add(RefEquipmentCard.MainInfo.ArchiveNumber, new ChangingValue<String>((GetControlValue(RefEquipmentCard.MainInfo.ArchiveNumber) ?? String.Empty).ToString()));
            WriteLog(@"Категория.");
            changes.Add(RefEquipmentCard.Categories.CategoriesValue, new ChangingValue<String>((Control_Categories.Text ?? String.Empty).ToString()));
            WriteLog(@"Обновили список изменений.");
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Событие закрытия карточки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CardControl_CardClosed(Object sender, EventArgs e)
        {
            try
            {
                // Отвязка методов
                Edit_Files.DoubleClick -= Edit_Files_DoubleClick;
                Button_Upload.Click -= Button_Upload_Click;
                this.CardScript.CardControl.Saved -= CardControl_Saved;
                this.CardScript.CardControl.CardClosed -= CardControl_CardClosed;
            }
            catch (Exception Ex) { CallError(Ex); }
        }
        /// <summary>
        /// Событие сохранения карточки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CardControl_Saving(Object sender, EventArgs e)
        {
            try
            {
                // Проверка заполнения поля "Дата ввода в эксплуатацию" в таблице "Ответственные за оборудование"
                if (Table_ResponsibleForEquipment.RowCount != 0)
                {
                    if (Table_ResponsibleForEquipment.Select(RefEquipmentCard.ResponsibleForEquipment.CommissioningDate).Any(r => r == null))
                        throw new MyException("Укажите дату ввода в эксплуатацию в таблице \"Ответственные за оборудование\".");
                }

                // Проверки заполнения полей раздела "Метрологическая лаборатория"
                if (MetrologicalLaboratory_Group.Enabled)
                {
                    List<String> NullableFilds = new List<string>();
                    if (GetControlValue(RefEquipmentCard.MetrologicalLaboratory.NumberOfRegistrationСard) == null)
                        NullableFilds.Add("Номер учетной карточки");
                    if (GetControlValue(RefEquipmentCard.MainInfo.SerialNumber) == null)
                        NullableFilds.Add("Заводской №");
                    if (GetControlValue(RefEquipmentCard.MainInfo.YearOfIssue) == null)
                        NullableFilds.Add("Год выпуска");
                    if (GetControlValue(RefEquipmentCard.MainInfo.Manufacturer) == null)
                        NullableFilds.Add("Изготовитель");

                    if (GetControlValue(RefEquipmentCard.MetrologicalLaboratory.BasicDocument) == null)
                        NullableFilds.Add("Основной эксплуатационный документ");
                    if (GetControlValue(RefEquipmentCard.MetrologicalLaboratory.Status) == null)
                    { NullableFilds.Add("Статус"); }
                    else
                    {
                        // Статусы "Эталон" и "Средство измерения (СИ)" требуют, чтобы были заполнены поля "Отображаемое название в документах" и "Используется для калибровки/поверки приборов"
                        if (((Int32)GetControlValue(RefEquipmentCard.MetrologicalLaboratory.Status) == (Int32)RefEquipmentCard.MetrologicalLaboratory.CardStatus.Gauge) ||
                            ((Int32)GetControlValue(RefEquipmentCard.MetrologicalLaboratory.Status) == (Int32)RefEquipmentCard.MetrologicalLaboratory.CardStatus.MeasuringTool))
                        {
                            if (GetControlValue(RefEquipmentCard.MetrologicalLaboratory.DisplayNameInDocuments).IsNull() || (String.IsNullOrEmpty(GetControlValue(RefEquipmentCard.MetrologicalLaboratory.DisplayNameInDocuments).ToString())))
                                NullableFilds.Add("Отображаемое название в документах");
                            if (Table_UsedForDevices.RowCount == 0)
                                NullableFilds.Add("Используется для калибровки/поверки приборов");
                        }
                        // Статус "Длительное хранение" требует, чтобы была заполнена таблица "Длительное хранение"
                        if ((Int32)GetControlValue(RefEquipmentCard.MetrologicalLaboratory.Status) == (Int32)RefEquipmentCard.MetrologicalLaboratory.CardStatus.LongTermStorage)
                        {
                            Boolean StartLongTermStorage = false;
                            for (int i = 0; i < Table_LongTermStorage.RowCount; i++)
                            {
                                if ((Table_LongTermStorage[i][RefEquipmentCard.LongTermStorage.StartDate] != null) && (Table_LongTermStorage[i][RefEquipmentCard.LongTermStorage.EndDate] == null))
                                    StartLongTermStorage = true;
                            }
                            if (StartLongTermStorage == false)
                                throw new MyException("Укажите дату начала длительного хранения оборудования в разделе \"Дополнительная информация\".");
                        }
                        else
                        {
                            // Отсутствие статуса "Длительное хранение" требует, чтобы в таблице "Длительное хранение" не было незавершенных периодов длительного хранения.
                            Boolean EndLongTermStorage = true;
                            for (int i = 0; i < Table_LongTermStorage.RowCount; i++)
                            {
                                if ((Table_LongTermStorage[i][RefEquipmentCard.LongTermStorage.StartDate] != null) && (Table_LongTermStorage[i][RefEquipmentCard.LongTermStorage.EndDate] == null))
                                    EndLongTermStorage = false;
                            }
                            if (EndLongTermStorage == false)
                                throw new MyException("Укажите дату окончания длительного хранения оборудования в разделе \"Дополнительная информация\".");
                        }

                        if ((Int32)GetControlValue(RefEquipmentCard.MetrologicalLaboratory.Status) == (Int32)RefEquipmentCard.MetrologicalLaboratory.CardStatus.Gauge)
                        {
                            if ((GetControlValue(RefEquipmentCard.Verification.VerificationInterval) == null) || ((Int32)GetControlValue(RefEquipmentCard.Verification.VerificationInterval) < 1))
                                throw new MyException("Укажите межповерочный интервал для данного оборудования на вкладке \"График калибровок/поверок/аттестаций\".");

                            if ((GetControlValue(RefEquipmentCard.Attestation.AttestationInterval) == null) || ((Int32)GetControlValue(RefEquipmentCard.Attestation.AttestationInterval) < 1))
                                throw new MyException("Укажите межаттестационный интервал для данного оборудования на вкладке \"График калибровок/поверок/аттестаций\".");
                        }

                        if ((Int32)GetControlValue(RefEquipmentCard.MetrologicalLaboratory.Status) == (Int32)RefEquipmentCard.MetrologicalLaboratory.CardStatus.MeasuringTool)
                        {
                            if (((GetControlValue(RefEquipmentCard.Verification.VerificationInterval) == null) || ((Int32)GetControlValue(RefEquipmentCard.Verification.VerificationInterval) < 1)) &&
                            ((GetControlValue(RefEquipmentCard.Attestation.AttestationInterval) == null) || ((Int32)GetControlValue(RefEquipmentCard.Attestation.AttestationInterval) < 1)))
                                throw new MyException("Укажите межповерочный или межкалибровочный интервал для данного оборудования на вкладке \"График калибровок/поверок/аттестаций\".");
                        }
                    }

                    if (NullableFilds.Count > 1)
                        throw new MyException("Заполните следующие поля:\n" + NullableFilds.Select(s => " - \"" + s + "\"").Aggregate(";\n") + ".");
                    if (NullableFilds.Count == 1)
                        throw new MyException("Заполните поле \"" + NullableFilds.First() + "\".");
                }

                // Проверка заполнения таблиц вкладки "График калибровок/поверок/аттестаций"
                if ((GetControlValue(RefEquipmentCard.Verification.VerificationInterval) != null) && ((Int32)GetControlValue(RefEquipmentCard.Verification.VerificationInterval) > 0))
                {
                    if (Table_VerificationGraph.RowCount == 0)
                        throw new MyException("Укажите данные о последней поверке оборудования в таблице \"График поверок\".");
                }

                if ((GetControlValue(RefEquipmentCard.Calibration.CalibrationInterval) != null) && ((Int32)GetControlValue(RefEquipmentCard.Calibration.CalibrationInterval) > 0))
                {
                    if (Table_CalibrationGraph.RowCount == 0)
                        throw new MyException("Укажите данные о последней калибровке оборудования в таблице \"График калибровок\".");
                }

                if ((GetControlValue(RefEquipmentCard.Attestation.AttestationInterval) != null) && ((Int32)GetControlValue(RefEquipmentCard.Attestation.AttestationInterval) > 0))
                {
                    if (Table_AttestationGraph.RowCount == 0)
                        throw new MyException("Укажите данные о последней аттестации оборудования в таблице \"График аттестаций\".");
                }
               
            }
            catch (MyException Ex)
            { MyMessageBox.Show(Ex.Message); }
            catch (Exception Ex) { CallError(Ex); }
        }
        /// <summary>
        /// Событие сохранения карточки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CardControl_Saved(Object sender, EventArgs e)
        {
            try
            {
                CardScript.CardData.Sections[RefEquipmentCard.TechnicalInformation.ID].FirstRow.SetString(RefEquipmentCard.TechnicalInformation.PurposeText, control_PurposeFormatting.GetText());
                CardScript.CardData.Sections[RefEquipmentCard.TechnicalInformation.ID].FirstRow.SetString(RefEquipmentCard.TechnicalInformation.SpecificationsText, control_SpecificationsFormatting.GetText());

                StringBuilder Digest = new StringBuilder(Control_Categories.Text + ". " + GetControlValue(RefEquipmentCard.MainInfo.Name));
                CardScript.UpdateDescription(CardScript.CardData.IsTemplate ? "Шаблон" : Digest.ToString());
            }
            catch (Exception Ex) { CallError(Ex); }
        }

        #region Обработчики таблицы "Ответственные за оборудование"
        /// <summary>
        /// Обработчик кнопки "Добавить" таблицы "Ответственные за оборудование" 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddResponsibleForEquipmentButton_ItemClick(Object sender, EventArgs e)
        {
            SelectForm SelectEmployeeForm = new SelectForm(
                    "Выберите ответственного сотрудника:",
                    this.CardScript.CardFrame.CardHost,
                    Context,
                    Base.Dictionary.SelectionType.StaffEmployee,
                    Guid.Empty,
                    "",
                    false,
                    true,
                    true);

            switch (SelectEmployeeForm.ShowDialog())
            {
                case DialogResult.OK:
                    {
                        BaseCardProperty NewRow = Table_ResponsibleForEquipment.AddRow(CardScript.BaseObject);
                        NewRow[RefEquipmentCard.ResponsibleForEquipment.Employee] = SelectEmployeeForm.SelectedItem.Id;
                        NewRow[RefEquipmentCard.ResponsibleForEquipment.Department] = Context.GetUnitByEmployee(SelectEmployeeForm.SelectedItem.Id);
                        Table_ResponsibleForEquipment.RefreshRows();
                        ICardControl.DisableTableBarItem(RefEquipmentCard.ResponsibleForEquipment.Alias, 4, false);
                        Grid_ResponsibleForEquipment.FocusedRowHandle = -1;
                        break;
                    }
                case DialogResult.Cancel:
                    {
                        break;
                    }
            }
            AddInfoCustomize();
        }
        /// <summary>
        /// Обработчик кнопки "Удалить" таблицы "Ответственные за оборудование" 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveResponsibleForEquipmentButton_ItemClick(Object sender, EventArgs e)
        {
            if (Table_ResponsibleForEquipment.FocusedRowItem[RefEquipmentCard.ResponsibleForEquipment.Department].ToGuid() == MyHelper.RefStaff_MS)
            {
                switch (MyMessageBox.Show("Если вы удалите всех сотрудников метрологической лаборатории из данной таблицы, то дополнительная информация об оборудовании метрологической лаборатории будет удалена!",
                    "Внимание!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning))
                {
                    case DialogResult.OK:
                        break;
                    case DialogResult.Cancel:
                        return;
                }
            }

            Table_ResponsibleForEquipment.RemoveRow(CardScript.BaseObject, Table_ResponsibleForEquipment.FocusedRowItem);
            if (Table_ResponsibleForEquipment.RowCount == 0) ICardControl.DisableTableBarItem(RefEquipmentCard.ResponsibleForEquipment.Alias, 4, true);
            AddInfoCustomize();
        }
        /// <summary>
        /// Обработчик события двойного клика по таблице "Ответственные за оборудование" 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResponsibleForEquipment_DoubleClick(Object sender, EventArgs e)
        {
            if (Grid_ResponsibleForEquipment.FocusedColumn.Name == RefEquipmentCard.ResponsibleForEquipment.Employee)
            {
                Guid DepartmentID = Table_ResponsibleForEquipment.FocusedRowItem[RefEquipmentCard.ResponsibleForEquipment.Department].ToGuid();
                Guid EmployeeID = Table_ResponsibleForEquipment.FocusedRowItem[RefEquipmentCard.ResponsibleForEquipment.Employee].ToGuid();
                
                SelectForm SelectEmployeeForm = new SelectForm(
                    "Выберите ответственного сотрудника:",
                    this.CardScript.CardFrame.CardHost,
                    Context,
                    Base.Dictionary.SelectionType.StaffEmployee,
                    DepartmentID,
                    Context.GetEmployeeDisplay(EmployeeID),
                    false,
                    true,
                    true);

                switch (SelectEmployeeForm.ShowDialog())
                {
                    case DialogResult.OK:
                        {
                            if ((DepartmentID == MyHelper.RefStaff_MS) && (Context.GetUnitByEmployee(SelectEmployeeForm.SelectedItem.Id) != MyHelper.RefStaff_MS))
                            {
                                switch (MyMessageBox.Show("Если вы удалите всех сотрудников метрологической лаборатории из данной таблицы, то дополнительная информация об оборудовании метрологической лаборатории будет удалена!",
                                    "Внимание!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning))
                                {
                                    case DialogResult.OK:
                                        break;
                                    case DialogResult.Cancel:
                                        return;
                                }
                            }

                            Table_ResponsibleForEquipment.FocusedRowItem[RefEquipmentCard.ResponsibleForEquipment.Employee] = SelectEmployeeForm.SelectedItem.Id;
                            Table_ResponsibleForEquipment.FocusedRowItem[RefEquipmentCard.ResponsibleForEquipment.Department] = Context.GetUnitByEmployee(SelectEmployeeForm.SelectedItem.Id);
                            Table_ResponsibleForEquipment.RefreshRow(Table_ResponsibleForEquipment.FocusedRowIndex);
                            Grid_ResponsibleForEquipment.FocusedRowHandle = -1;
                            if ((DepartmentID == MyHelper.RefStaff_MS) || (Context.GetUnitByEmployee(SelectEmployeeForm.SelectedItem.Id) == MyHelper.RefStaff_MS)) AddInfoCustomize();
                            break;
                        }
                    case DialogResult.Cancel:
                        {
                            break;
                        }
                }
            }

            if (Grid_ResponsibleForEquipment.FocusedColumn.Name == RefEquipmentCard.ResponsibleForEquipment.Department)
            {
                Guid DepartmentID = Table_ResponsibleForEquipment.FocusedRowItem[RefEquipmentCard.ResponsibleForEquipment.Department].ToGuid();
                Guid EmployeeID = Table_ResponsibleForEquipment.FocusedRowItem[RefEquipmentCard.ResponsibleForEquipment.Employee].ToGuid();

                SelectForm SelectDepartmentForm = new SelectForm(
                    "Выберите подразделение ответственного сотрудника:",
                    this.CardScript.CardFrame.CardHost,
                    Context,
                    Base.Dictionary.SelectionType.StaffUnit,
                    Guid.Empty,
                    Context.GetUnitDisplay(DepartmentID),
                    false,
                    true,
                    true);

                switch (SelectDepartmentForm.ShowDialog())
                {
                    case DialogResult.OK:
                        {
                            if ((DepartmentID == MyHelper.RefStaff_MS) && (SelectDepartmentForm.SelectedItem.Id != MyHelper.RefStaff_MS))
                            {
                                switch (MyMessageBox.Show("Если вы удалите всех сотрудников метрологической лаборатории из данной таблицы, то дополнительная информация об оборудовании метрологической лаборатории будет удалена!",
                                    "Внимание!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning))
                                {
                                    case DialogResult.OK:
                                        break;
                                    case DialogResult.Cancel:
                                        return;
                                }
                            }

                            Table_ResponsibleForEquipment.FocusedRowItem[RefEquipmentCard.ResponsibleForEquipment.Department] = SelectDepartmentForm.SelectedItem.Id;
                            if (!SelectDepartmentForm.SelectedItem.Id.Equals(Context.GetUnitByEmployee(EmployeeID)))
                                Table_ResponsibleForEquipment.FocusedRowItem[RefEquipmentCard.ResponsibleForEquipment.Employee] = Context.GetManagerOfUnit(SelectDepartmentForm.SelectedItem.Id);
                            Table_ResponsibleForEquipment.RefreshRow(Table_ResponsibleForEquipment.FocusedRowIndex);
                            Grid_ResponsibleForEquipment.FocusedRowHandle = -1;
                            if ((DepartmentID == MyHelper.RefStaff_MS) || (SelectDepartmentForm.SelectedItem.Id == MyHelper.RefStaff_MS)) AddInfoCustomize();
                            break;
                        }
                    case DialogResult.Cancel:
                        {
                            break;
                        }
                }
            }
        }
        #endregion

        #region Обработчики таблицы "Сведения об отказах и ремонтах"
        /// <summary>
        /// Обработчик кнопки "Добавить" таблицы "Сведения об отказах и ремонтах"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddFailuresAndRepairsButton_ItemClick(Object sender, EventArgs e)
        {
            Table_FailuresAndRepairs.AddRow(CardScript.BaseObject);
            ICardControl.DisableTableBarItem(RefEquipmentCard.FailuresAndRepairs.Alias, 4, false);
        }
        /// <summary>
        /// Обработчик кнопки "Удалить" таблицы "Сведения об отказах и ремонтах"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveFailuresAndRepairsButton_ItemClick(Object sender, EventArgs e)
        {
            Table_FailuresAndRepairs.RemoveRow(CardScript.BaseObject, Table_FailuresAndRepairs.FocusedRowItem);
            if (Table_FailuresAndRepairs.RowCount == 0) ICardControl.DisableTableBarItem(RefEquipmentCard.FailuresAndRepairs.Alias, 4, true);
        }
        #endregion

        #region Обработчики таблицы "Длительное хранение"
        /// <summary>
        /// Обработчик кнопки "Добавить" таблицы "Длительное хранение"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddLongTermStorageButton_ItemClick(Object sender, EventArgs e)
        {
            Table_LongTermStorage.AddRow(CardScript.BaseObject);
            ICardControl.DisableTableBarItem(RefEquipmentCard.LongTermStorage.Alias, 4, false);
        }
        /// <summary>
        /// Обработчик кнопки "Удалить" таблицы "Длительное хранение"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveLongTermStorageButton_ItemClick(Object sender, EventArgs e)
        {
            Table_LongTermStorage.RemoveRow(CardScript.BaseObject, Table_LongTermStorage.FocusedRowItem);
            if (Table_LongTermStorage.RowCount == 0) ICardControl.DisableTableBarItem(RefEquipmentCard.LongTermStorage.Alias, 4, true);
        }
        #endregion
        
        #region Обработчики таблицы "Приборы"
        /// <summary>
        /// Обработчик события двойного клика по таблице "Приборы"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Devices_DoubleClick(Object sender, EventArgs e)
        {
            try
            {
                if (Grid_UsedForDevices.FocusedColumn.Name == RefEquipmentCard.UsedForDevices.Device)
                {
                    Guid DeviceID = Table_UsedForDevices.FocusedRowItem[RefEquipmentCard.UsedForDevices.Device].ToGuid();

                    SelectForm SelectDeviceForm = new SelectForm(
                        "Выберите прибор:",
                        this.CardScript.CardFrame.CardHost,
                        Context,
                        Base.Dictionary.SelectionType.UniversalItem,
                        MyHelper.RefItem_Devices,
                        UniversalCard.GetItemName(DeviceID),
                        false,
                        false,
                        true);

                    switch (SelectDeviceForm.ShowDialog())
                    {
                        case DialogResult.OK:
                            {
                                if (Table_UsedForDevices.Select(RefEquipmentCard.UsedForDevices.Device).Any(r => r == SelectDeviceForm.SelectedItem.Id))
                                    throw new MyException("Выбранный Вами прибор уже указан в данной таблице.");

                                Table_UsedForDevices.FocusedRowItem[RefEquipmentCard.UsedForDevices.Device] = SelectDeviceForm.SelectedItem.Id;
                                Table_UsedForDevices.RefreshRow(Table_UsedForDevices.FocusedRowIndex);
                                Grid_UsedForDevices.FocusedRowHandle = -1;
                                break;
                            }
                        case DialogResult.Cancel:
                            {
                                break;
                            }
                    }
                }
            }
            catch (MyException Ex)
            { MyMessageBox.Show(Ex.Message); }
        }
        /// <summary>
        /// Обработчик кнопки "Добавить" таблицы "Приборы"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddUsedForDevicesButton_ItemClick(Object sender, EventArgs e)
        {
            try
            {
                SelectForm SelectDeviceForm = new SelectForm(
                        "Выберите прибор:",
                        this.CardScript.CardFrame.CardHost,
                        Context,
                        Base.Dictionary.SelectionType.UniversalItem,
                        MyHelper.RefItem_Devices,
                        UniversalCard.GetItemName(Guid.Empty),
                        false,
                        false,
                        true);

                switch (SelectDeviceForm.ShowDialog())
                {
                    case DialogResult.OK:
                        {
                            if (Table_UsedForDevices.Select(RefEquipmentCard.UsedForDevices.Device).Any(r => r == SelectDeviceForm.SelectedItem.Id))
                                throw new MyException("Выбранный Вами прибор уже указан в данной таблице.");

                            Table_UsedForDevices.AddRow(CardScript.BaseObject)[RefEquipmentCard.UsedForDevices.Device] = SelectDeviceForm.SelectedItem.Id;
                            Table_UsedForDevices.RefreshRows();
                            Grid_UsedForDevices.FocusedRowHandle = -1;
                            ICardControl.DisableTableBarItem(RefEquipmentCard.UsedForDevices.Alias, 4, false);
                            break;
                        }
                    case DialogResult.Cancel:
                        {
                            break;
                        }
                }
            }
            catch (MyException Ex)
            { MyMessageBox.Show(Ex.Message); }
        }
        /// <summary>
        /// Обработчик кнопки "Удалить" таблицы "Приборы"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveUsedForDevicesButton_ItemClick(Object sender, EventArgs e)
        {
            Table_UsedForDevices.RemoveRow(CardScript.BaseObject, Table_UsedForDevices.FocusedRowItem);
            if (Table_UsedForDevices.RowCount == 0) ICardControl.DisableTableBarItem(RefEquipmentCard.UsedForDevices.Alias, 4, true);
        }
        #endregion

        #region Обработчики таблицы "Применяемость"
        /// <summary>
        /// Обработчик события двойного клика по таблице "Применяемость" 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Applicability_DoubleClick(Object sender, EventArgs e)
        {
            if (Grid_Applicability.FocusedColumn.Name == RefEquipmentCard.Applicability.Code)
            {
                Guid CodeID = Table_Applicability.FocusedRowItem[RefEquipmentCard.Applicability.Code].ToGuid();

                SelectForm SelectCodeForm = new SelectForm(
                    "Выберите код сборочного узла/детали:",
                    this.CardScript.CardFrame.CardHost,
                    Context,
                    Base.Dictionary.SelectionType.UniversalItem,
                    MyHelper.RefItem_SKBCode,
                    UniversalCard.GetItemName(CodeID),
                    false,
                    false,
                    true);

                switch (SelectCodeForm.ShowDialog())
                {
                    case DialogResult.OK:
                        {
                            Table_Applicability.FocusedRowItem[RefEquipmentCard.Applicability.Code] = SelectCodeForm.SelectedItem.Id;
                            Table_Applicability.RefreshRow(Table_Applicability.FocusedRowIndex);
                            Grid_Applicability.FocusedRowHandle = -1;
                            break;
                        }
                    case DialogResult.Cancel:
                        {
                            break;
                        }
                }
            }
        }
        /// <summary>
        /// Обработчик кнопки "Добавить" таблицы "Применяемость"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddApplicabilityButton_ItemClick(Object sender, EventArgs e)
        {
            try
            {
                SelectForm SelectCodeForm = new SelectForm(
                    "Выберите код сборочного узла/детали:",
                    this.CardScript.CardFrame.CardHost,
                    Context,
                    Base.Dictionary.SelectionType.UniversalItem,
                    MyHelper.RefItem_SKBCode,
                    "",
                    false,
                    false,
                    true);

                switch (SelectCodeForm.ShowDialog())
                {
                    case DialogResult.OK:
                        {
                            Table_Applicability.FocusedRowItem[RefEquipmentCard.Applicability.Code] = SelectCodeForm.SelectedItem.Id;
                            Table_Applicability.RefreshRow(Table_Applicability.FocusedRowIndex);
                            Grid_Applicability.FocusedRowHandle = -1;

                            break;
                        }
                    case DialogResult.Cancel:
                        {
                            break;
                        }
                }
            }
            catch (MyException Ex)
            { MyMessageBox.Show(Ex.Message); }
        }
        /// <summary>
        /// Обработчик кнопки "Удалить" таблицы "Применяемость"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveApplicabilityButton_ItemClick(Object sender, EventArgs e)
        {
            Table_Applicability.RemoveRow(CardScript.BaseObject, Table_Applicability.FocusedRowItem);
            if (Table_Applicability.RowCount == 0) ICardControl.DisableTableBarItem(RefEquipmentCard.Applicability.Alias, 4, true);
        }
        #endregion

        #region Обработчики таблицы "График поверок"
        /// <summary>
        /// Обработчик события двойного клика по таблице "График поверок" 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VerificationGraph_DoubleClick(Object sender, EventArgs e)
        {
            try
            {
                Int32 VerificationInterval = GetControlValue(RefEquipmentCard.Verification.VerificationInterval) == null ? 0 : (Int32)GetControlValue(RefEquipmentCard.Verification.VerificationInterval);
                if (VerificationInterval == 0)
                {
                    throw new MyException("Укажите межповерочный интервал.");
                }
                else
                {
                    RefEquipmentCard.Enums.Units Unit = RefEquipmentCard.Enums.Units.Months;

                    switch ((Int32)GetControlValue(RefEquipmentCard.Verification.VerifyUnitOfTime))
                    {
                        case 0:
                            Unit = RefEquipmentCard.Enums.Units.Days;
                            break;
                        case 1:
                            Unit = RefEquipmentCard.Enums.Units.Weeks;
                            break;
                        case 2:
                            Unit = RefEquipmentCard.Enums.Units.Months;
                            break;
                        case 3:
                            Unit = RefEquipmentCard.Enums.Units.Years;
                            break;
                    }

                    GraphForm NewForm = new GraphForm(this, RefEquipmentCard.Enums.TypeOfInspection.Verification, 
                        (DateTime)Table_VerificationGraph.FocusedRowItem[RefEquipmentCard.VerificationGraph.DateOfVerification],
                        (DateTime)Table_VerificationGraph.FocusedRowItem[RefEquipmentCard.VerificationGraph.DateOfNextVerification], 
                        Table_VerificationGraph.FocusedRowItem[RefEquipmentCard.VerificationGraph.NumberOfDocument].ToString(),
                        Table_VerificationGraph.FocusedRowItem[RefEquipmentCard.VerificationGraph.VerificationDocument].ToGuid(), 
                        VerificationInterval, 
                        Unit);
                    switch (NewForm.ShowDialog())
                    {
                        case DialogResult.OK:
                            BaseCardProperty Row = Table_VerificationGraph[Table_VerificationGraph.FocusedRowIndex];
                            Row[RefEquipmentCard.VerificationGraph.DateOfVerification] = NewForm.DateOfEventValue;
                            Row[RefEquipmentCard.VerificationGraph.DateOfNextVerification] = NewForm.NextDateOfEventValue;
                            Row[RefEquipmentCard.VerificationGraph.NumberOfDocument] = NewForm.NumberOfDocumentValue;
                            Row[RefEquipmentCard.VerificationGraph.VerificationDocument] = NewForm.DocumentGuid;
                            Table_VerificationGraph.RefreshRows();
                            break;
                        case DialogResult.Cancel:
                            break;
                    }
                }
            }
            catch (Exception Ex) { MyMessageBox.Show(Ex.Message); }
        }
        /// <summary>
        /// Обработчик кнопки "Добавить" таблицы "График поверок"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddVerificationGraphButton_ItemClick(Object sender, EventArgs e)
        {
            try
            {
                Int32 VerificationInterval = GetControlValue(RefEquipmentCard.Verification.VerificationInterval) == null ? 0 : (Int32)GetControlValue(RefEquipmentCard.Verification.VerificationInterval);
                if (VerificationInterval == 0)
                {
                    throw new MyException("Укажите межповерочный интервал.");
                }
                else
                {
                    RefEquipmentCard.Enums.Units Unit = RefEquipmentCard.Enums.Units.Months;

                    switch ((Int32)GetControlValue(RefEquipmentCard.Verification.VerifyUnitOfTime))
                    {
                        case 0:
                            Unit = RefEquipmentCard.Enums.Units.Days;
                            break;
                        case 1:
                            Unit = RefEquipmentCard.Enums.Units.Weeks;
                            break;
                        case 2:
                            Unit = RefEquipmentCard.Enums.Units.Months;
                            break;
                        case 3:
                            Unit = RefEquipmentCard.Enums.Units.Years;
                            break;
                    }

                    GraphForm NewForm = new GraphForm(this, RefEquipmentCard.Enums.TypeOfInspection.Verification, DateTime.MinValue, DateTime.MinValue, "", Guid.Empty, VerificationInterval, Unit);
                    switch (NewForm.ShowDialog())
                    {
                        case DialogResult.OK:
                            BaseCardProperty NewRow = Table_VerificationGraph.AddRow(CardScript.BaseObject);
                            NewRow[RefEquipmentCard.VerificationGraph.DateOfVerification] = NewForm.DateOfEventValue;
                            NewRow[RefEquipmentCard.VerificationGraph.DateOfNextVerification] = NewForm.NextDateOfEventValue;
                            NewRow[RefEquipmentCard.VerificationGraph.NumberOfDocument] = NewForm.NumberOfDocumentValue;
                            NewRow[RefEquipmentCard.VerificationGraph.VerificationDocument] = NewForm.DocumentGuid;
                            Table_VerificationGraph.RefreshRows();
                            ICardControl.DisableTableBarItem(RefEquipmentCard.VerificationGraph.Alias, 4, false);
                            break;
                        case DialogResult.Cancel:
                            break;
                    }
                }
            }
            catch (Exception Ex) { MyMessageBox.Show(Ex.Message); }
        }
        /// <summary>
        /// Обработчик кнопки "Удалить" таблицы "График поверок"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveVerificationGraphButton_ItemClick(Object sender, EventArgs e)
        {
            if ((Table_VerificationGraph[Table_VerificationGraph.FocusedRowIndex][RefEquipmentCard.VerificationGraph.VerificationDocument] != null) &&
                (Table_VerificationGraph[Table_VerificationGraph.FocusedRowIndex][RefEquipmentCard.VerificationGraph.VerificationDocument].ToGuid() != Guid.Empty))
                Context.DeleteObject(Context.GetObject<Document>(Table_VerificationGraph[Table_VerificationGraph.FocusedRowIndex][RefEquipmentCard.VerificationGraph.VerificationDocument].ToGuid()));         

            Table_VerificationGraph.RemoveRow(CardScript.BaseObject, Table_VerificationGraph.FocusedRowItem);
            if (Table_VerificationGraph.RowCount == 0) ICardControl.DisableTableBarItem(RefEquipmentCard.VerificationGraph.Alias, 4, true);
        }
        #endregion

        #region Обработчики таблицы "График калибровок"
        /// <summary>
        /// Обработчик события двойного клика по таблице "График калибровок" 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CalibrationGraph_DoubleClick(Object sender, EventArgs e)
        {
            try
            {
                Int32 CalibrationInterval = GetControlValue(RefEquipmentCard.Calibration.CalibrationInterval) == null ? 0 : (Int32)GetControlValue(RefEquipmentCard.Calibration.CalibrationInterval);
                if (CalibrationInterval == 0)
                {
                    throw new MyException("Укажите межкалибровочный интервал.");
                }
                else
                {
                    RefEquipmentCard.Enums.Units Unit = RefEquipmentCard.Enums.Units.Months;

                    switch ((Int32)GetControlValue(RefEquipmentCard.Calibration.CalibrationUnitOfTime))
                    {
                        case 0:
                            Unit = RefEquipmentCard.Enums.Units.Days;
                            break;
                        case 1:
                            Unit = RefEquipmentCard.Enums.Units.Weeks;
                            break;
                        case 2:
                            Unit = RefEquipmentCard.Enums.Units.Months;
                            break;
                        case 3:
                            Unit = RefEquipmentCard.Enums.Units.Years;
                            break;
                    }

                    GraphForm NewForm = new GraphForm(this, RefEquipmentCard.Enums.TypeOfInspection.Calibration,
                        (DateTime)Table_CalibrationGraph.FocusedRowItem[RefEquipmentCard.CalibrationGraph.DateOfCalibration],
                        (DateTime)Table_CalibrationGraph.FocusedRowItem[RefEquipmentCard.CalibrationGraph.DateOfNextCalibration],
                        Table_CalibrationGraph.FocusedRowItem[RefEquipmentCard.CalibrationGraph.NumberOfDocument].ToString(),
                        Table_CalibrationGraph.FocusedRowItem[RefEquipmentCard.CalibrationGraph.CalibrationDocument].ToGuid(),
                        CalibrationInterval,
                        Unit);
                    switch (NewForm.ShowDialog())
                    {
                        case DialogResult.OK:
                            BaseCardProperty Row = Table_CalibrationGraph[Table_CalibrationGraph.FocusedRowIndex];
                            Row[RefEquipmentCard.CalibrationGraph.DateOfCalibration] = NewForm.DateOfEventValue;
                            Row[RefEquipmentCard.CalibrationGraph.DateOfNextCalibration] = NewForm.NextDateOfEventValue;
                            Row[RefEquipmentCard.CalibrationGraph.NumberOfDocument] = NewForm.NumberOfDocumentValue;
                            Row[RefEquipmentCard.CalibrationGraph.CalibrationDocument] = NewForm.DocumentGuid;
                            Table_CalibrationGraph.RefreshRows();
                            break;
                        case DialogResult.Cancel:
                            break;
                    }
                }
            }
            catch (Exception Ex) { MyMessageBox.Show(Ex.Message); }
        }
        /// <summary>
        /// Обработчик кнопки "Добавить" таблицы "График калибровок"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddCalibrationGraphButton_ItemClick(Object sender, EventArgs e)
        {
            try
            {
                Int32 CalibrationInterval = GetControlValue(RefEquipmentCard.Calibration.CalibrationInterval) == null ? 0 : (Int32)GetControlValue(RefEquipmentCard.Calibration.CalibrationInterval);
                if (CalibrationInterval == 0)
                {
                    throw new MyException("Укажите межкалибровочный интервал.");
                }
                else
                {
                    RefEquipmentCard.Enums.Units Unit = RefEquipmentCard.Enums.Units.Months;

                    switch ((Int32)GetControlValue(RefEquipmentCard.Calibration.CalibrationUnitOfTime))
                    {
                        case 0:
                            Unit = RefEquipmentCard.Enums.Units.Days;
                            break;
                        case 1:
                            Unit = RefEquipmentCard.Enums.Units.Weeks;
                            break;
                        case 2:
                            Unit = RefEquipmentCard.Enums.Units.Months;
                            break;
                        case 3:
                            Unit = RefEquipmentCard.Enums.Units.Years;
                            break;
                    }

                    GraphForm NewForm = new GraphForm(this, RefEquipmentCard.Enums.TypeOfInspection.Calibration, DateTime.MinValue, DateTime.MinValue, "", Guid.Empty, CalibrationInterval, Unit);
                    switch (NewForm.ShowDialog())
                    {
                        case DialogResult.OK:
                            BaseCardProperty NewRow = Table_CalibrationGraph.AddRow(CardScript.BaseObject);
                            NewRow[RefEquipmentCard.CalibrationGraph.DateOfCalibration] = NewForm.DateOfEventValue;
                            NewRow[RefEquipmentCard.CalibrationGraph.DateOfNextCalibration] = NewForm.NextDateOfEventValue;
                            NewRow[RefEquipmentCard.CalibrationGraph.NumberOfDocument] = NewForm.NumberOfDocumentValue;
                            NewRow[RefEquipmentCard.CalibrationGraph.CalibrationDocument] = NewForm.DocumentGuid;
                            Table_CalibrationGraph.RefreshRows();
                            ICardControl.DisableTableBarItem(RefEquipmentCard.CalibrationGraph.Alias, 4, false);
                            break;
                        case DialogResult.Cancel:
                            break;
                    }
                }
            }
            catch (Exception Ex) { MyMessageBox.Show(Ex.Message); }
        }
        /// <summary>
        /// Обработчик кнопки "Удалить" таблицы "График калибровок"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveCalibrationGraphButton_ItemClick(Object sender, EventArgs e)
        {
            if ((Table_CalibrationGraph[Table_CalibrationGraph.FocusedRowIndex][RefEquipmentCard.CalibrationGraph.CalibrationDocument] != null) &&
                (Table_CalibrationGraph[Table_CalibrationGraph.FocusedRowIndex][RefEquipmentCard.CalibrationGraph.CalibrationDocument].ToGuid() != Guid.Empty))
                Context.DeleteObject(Context.GetObject<Document>(Table_CalibrationGraph[Table_CalibrationGraph.FocusedRowIndex][RefEquipmentCard.CalibrationGraph.CalibrationDocument].ToGuid()));

            Table_CalibrationGraph.RemoveRow(CardScript.BaseObject, Table_CalibrationGraph.FocusedRowItem);
            if (Table_CalibrationGraph.RowCount == 0) ICardControl.DisableTableBarItem(RefEquipmentCard.CalibrationGraph.Alias, 4, true);
        }
        #endregion

        #region Обработчики таблицы "График аттестаций"
        /// <summary>
        /// Обработчик события двойного клика по таблице "График аттестаций" 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AttestationGraph_DoubleClick(Object sender, EventArgs e)
        {
            try
            {
                Int32 AttestationInterval = GetControlValue(RefEquipmentCard.Attestation.AttestationInterval) == null ? 0 : (Int32)GetControlValue(RefEquipmentCard.Attestation.AttestationInterval);
                if (AttestationInterval == 0)
                {
                    throw new MyException("Укажите межаттестационный интервал.");
                }
                else
                {
                    RefEquipmentCard.Enums.Units Unit = RefEquipmentCard.Enums.Units.Months;

                    switch ((Int32)GetControlValue(RefEquipmentCard.Attestation.AttestationUnitOfTime))
                    {
                        case 0:
                            Unit = RefEquipmentCard.Enums.Units.Days;
                            break;
                        case 1:
                            Unit = RefEquipmentCard.Enums.Units.Weeks;
                            break;
                        case 2:
                            Unit = RefEquipmentCard.Enums.Units.Months;
                            break;
                        case 3:
                            Unit = RefEquipmentCard.Enums.Units.Years;
                            break;
                    }

                    GraphForm NewForm = new GraphForm(this, RefEquipmentCard.Enums.TypeOfInspection.Attestation,
                        (DateTime)Table_AttestationGraph.FocusedRowItem[RefEquipmentCard.AttestationGraph.DateOfAttestation],
                        (DateTime)Table_AttestationGraph.FocusedRowItem[RefEquipmentCard.AttestationGraph.DateOfNextAttestation],
                        Table_AttestationGraph.FocusedRowItem[RefEquipmentCard.AttestationGraph.NumberOfDocument].ToString(),
                        Table_AttestationGraph.FocusedRowItem[RefEquipmentCard.AttestationGraph.AttestationDocument].ToGuid(),
                        AttestationInterval,
                        Unit);
                    switch (NewForm.ShowDialog())
                    {
                        case DialogResult.OK:
                            BaseCardProperty Row = Table_AttestationGraph[Table_AttestationGraph.FocusedRowIndex];
                            Row[RefEquipmentCard.AttestationGraph.DateOfAttestation] = NewForm.DateOfEventValue;
                            Row[RefEquipmentCard.AttestationGraph.DateOfNextAttestation] = NewForm.NextDateOfEventValue;
                            Row[RefEquipmentCard.AttestationGraph.NumberOfDocument] = NewForm.NumberOfDocumentValue;
                            Row[RefEquipmentCard.AttestationGraph.AttestationDocument] = NewForm.DocumentGuid;
                            Table_AttestationGraph.RefreshRows();
                            break;
                        case DialogResult.Cancel:
                            break;
                    }
                }
            }
            catch (Exception Ex) { MyMessageBox.Show(Ex.Message); }
        }
        /// <summary>
        /// Обработчик кнопки "Добавить" таблицы "График аттестаций"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddAttestationGraphButton_ItemClick(Object sender, EventArgs e)
        {
            try
            {
                Int32 AttestationInterval = GetControlValue(RefEquipmentCard.Attestation.AttestationInterval) == null ? 0 : (Int32)GetControlValue(RefEquipmentCard.Attestation.AttestationInterval);
                if (AttestationInterval == 0)
                {
                    throw new MyException("Укажите межаттестационный интервал.");
                }
                else
                {
                    RefEquipmentCard.Enums.Units Unit = RefEquipmentCard.Enums.Units.Months;

                    switch ((Int32)GetControlValue(RefEquipmentCard.Attestation.AttestationUnitOfTime))
                    {
                        case 0:
                            Unit = RefEquipmentCard.Enums.Units.Days;
                            break;
                        case 1:
                            Unit = RefEquipmentCard.Enums.Units.Weeks;
                            break;
                        case 2:
                            Unit = RefEquipmentCard.Enums.Units.Months;
                            break;
                        case 3:
                            Unit = RefEquipmentCard.Enums.Units.Years;
                            break;
                    }

                    GraphForm NewForm = new GraphForm(this, RefEquipmentCard.Enums.TypeOfInspection.Attestation, DateTime.MinValue, DateTime.MinValue, "", Guid.Empty, AttestationInterval, Unit);
                    switch (NewForm.ShowDialog())
                    {
                        case DialogResult.OK:
                            BaseCardProperty NewRow = Table_AttestationGraph.AddRow(CardScript.BaseObject);
                            NewRow[RefEquipmentCard.AttestationGraph.DateOfAttestation] = NewForm.DateOfEventValue;
                            NewRow[RefEquipmentCard.AttestationGraph.DateOfNextAttestation] = NewForm.NextDateOfEventValue;
                            NewRow[RefEquipmentCard.AttestationGraph.NumberOfDocument] = NewForm.NumberOfDocumentValue;
                            NewRow[RefEquipmentCard.AttestationGraph.AttestationDocument] = NewForm.DocumentGuid;
                            Table_AttestationGraph.RefreshRows();
                            ICardControl.DisableTableBarItem(RefEquipmentCard.AttestationGraph.Alias, 4, false);
                            break;
                        case DialogResult.Cancel:
                            break;
                    }
                }
            }
            catch (Exception Ex) { MyMessageBox.Show(Ex.Message); }
        }
        // <summary>
        /// Обработчик кнопки "Удалить" таблицы "График аттестаций"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveAttestationGraphButton_ItemClick(Object sender, EventArgs e)
        {
            if ((Table_AttestationGraph[Table_AttestationGraph.FocusedRowIndex][RefEquipmentCard.AttestationGraph.AttestationDocument] != null) &&
                (Table_AttestationGraph[Table_AttestationGraph.FocusedRowIndex][RefEquipmentCard.AttestationGraph.AttestationDocument].ToGuid() != Guid.Empty))
                Context.DeleteObject(Context.GetObject<Document>(Table_AttestationGraph[Table_CalibrationGraph.FocusedRowIndex][RefEquipmentCard.AttestationGraph.AttestationDocument].ToGuid()));

            Table_AttestationGraph.RemoveRow(CardScript.BaseObject, Table_AttestationGraph.FocusedRowItem);
            if (Table_AttestationGraph.RowCount == 0) ICardControl.DisableTableBarItem(RefEquipmentCard.AttestationGraph.Alias, 4, true);
        }
        #endregion

        #region Обработчики таблицы "График технического обслуживания"
        /// <summary>
        /// Обработчик события двойного клика по таблице "График технического обслуживания" 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MaintenanceGraph_DoubleClick(Object sender, EventArgs e)
        {
            try
            {
                Int32 MaintenanceInterval = GetControlValue(RefEquipmentCard.Maintenance.MaintenanceInterval) == null ? 0 : (Int32)GetControlValue(RefEquipmentCard.Maintenance.MaintenanceInterval);
                if (MaintenanceInterval == 0)
                {
                    throw new MyException("Укажите периодичность обслуживания.");
                }
                else
                {
                    RefEquipmentCard.Enums.Units Unit = RefEquipmentCard.Enums.Units.Months;

                    switch ((Int32)GetControlValue(RefEquipmentCard.Maintenance.MaintenanceUnitOfTime))
                    {
                        case 0:
                            Unit = RefEquipmentCard.Enums.Units.Days;
                            break;
                        case 1:
                            Unit = RefEquipmentCard.Enums.Units.Weeks;
                            break;
                        case 2:
                            Unit = RefEquipmentCard.Enums.Units.Months;
                            break;
                        case 3:
                            Unit = RefEquipmentCard.Enums.Units.Years;
                            break;
                    }

                    MaintenanceForm NewForm = new MaintenanceForm(this, RefEquipmentCard.Enums.TypeOfInspection.Maintenance,
                        (DateTime)Table_MaintenanceGraph.FocusedRowItem[RefEquipmentCard.MaintenanceGraph.DateOfMaintenance],
                        (DateTime)Table_MaintenanceGraph.FocusedRowItem[RefEquipmentCard.MaintenanceGraph.DateOfNextMaintenance],
                        Table_MaintenanceGraph.FocusedRowItem[RefEquipmentCard.MaintenanceGraph.Employee].ToGuid(),
                        Table_MaintenanceGraph.FocusedRowItem[RefEquipmentCard.MaintenanceGraph.BaseDocument].ToGuid(),
                        MaintenanceInterval,
                        Unit);
                    switch (NewForm.ShowDialog())
                    {
                        case DialogResult.OK:
                            BaseCardProperty Row = Table_MaintenanceGraph[Table_MaintenanceGraph.FocusedRowIndex];
                            Row[RefEquipmentCard.MaintenanceGraph.DateOfMaintenance] = NewForm.DateOfEventValue;
                            Row[RefEquipmentCard.MaintenanceGraph.DateOfNextMaintenance] = NewForm.NextDateOfEventValue;
                            Row[RefEquipmentCard.MaintenanceGraph.Employee] = NewForm.EmployeeGuid;
                            Row[RefEquipmentCard.MaintenanceGraph.Position] = NewForm.PositionGuid;
                            Row[RefEquipmentCard.MaintenanceGraph.BaseDocument] = NewForm.DocumentGuid;
                            Table_MaintenanceGraph.RefreshRows();
                            break;
                        case DialogResult.Cancel:
                            break;
                    }
                }
            }
            catch (Exception Ex) { MyMessageBox.Show(Ex.Message); }
        }
        /// <summary>
        /// Обработчик кнопки "Добавить" таблицы "График технического обслуживания"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddMaintenanceGraphButton_ItemClick(Object sender, EventArgs e)
        {
            try
            {
                Int32 MaintenanceInterval = GetControlValue(RefEquipmentCard.Maintenance.MaintenanceInterval) == null ? 0 : (Int32)GetControlValue(RefEquipmentCard.Maintenance.MaintenanceInterval);
                if (MaintenanceInterval == 0)
                {
                    throw new MyException("Укажите периодичность обслуживания.");
                }
                else
                {
                    RefEquipmentCard.Enums.Units Unit = RefEquipmentCard.Enums.Units.Months;

                    switch ((Int32)GetControlValue(RefEquipmentCard.Maintenance.MaintenanceUnitOfTime))
                    {
                        case 0:
                            Unit = RefEquipmentCard.Enums.Units.Days;
                            break;
                        case 1:
                            Unit = RefEquipmentCard.Enums.Units.Weeks;
                            break;
                        case 2:
                            Unit = RefEquipmentCard.Enums.Units.Months;
                            break;
                        case 3:
                            Unit = RefEquipmentCard.Enums.Units.Years;
                            break;
                    }

                    MaintenanceForm NewForm = new MaintenanceForm(this, RefEquipmentCard.Enums.TypeOfInspection.Maintenance, DateTime.MinValue, DateTime.MinValue, Guid.Empty, Guid.Empty, MaintenanceInterval, Unit);
                    switch (NewForm.ShowDialog())
                    {
                        case DialogResult.OK:
                            BaseCardProperty NewRow = Table_MaintenanceGraph.AddRow(CardScript.BaseObject);
                            NewRow[RefEquipmentCard.MaintenanceGraph.DateOfMaintenance] = NewForm.DateOfEventValue;
                            NewRow[RefEquipmentCard.MaintenanceGraph.DateOfNextMaintenance] = NewForm.NextDateOfEventValue;
                            NewRow[RefEquipmentCard.MaintenanceGraph.Employee] = NewForm.EmployeeGuid;
                            NewRow[RefEquipmentCard.MaintenanceGraph.Position] = NewForm.PositionGuid;
                            NewRow[RefEquipmentCard.MaintenanceGraph.BaseDocument] = NewForm.DocumentGuid;
                            Table_MaintenanceGraph.RefreshRows();
                            ICardControl.DisableTableBarItem(RefEquipmentCard.MaintenanceGraph.Alias, 4, false);
                            break;
                        case DialogResult.Cancel:
                            break;
                    }
                }
            }
            catch (Exception Ex) { MyMessageBox.Show(Ex.Message); }
        }
        // <summary>
        /// Обработчик кнопки "Удалить" таблицы "График технического обслуживания"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveMaintenanceGraphButton_ItemClick(Object sender, EventArgs e)
        {
            if ((Table_MaintenanceGraph[Table_MaintenanceGraph.FocusedRowIndex][RefEquipmentCard.MaintenanceGraph.BaseDocument] != null) &&
                (Table_MaintenanceGraph[Table_MaintenanceGraph.FocusedRowIndex][RefEquipmentCard.MaintenanceGraph.BaseDocument].ToGuid() != Guid.Empty))
                Context.DeleteObject(Context.GetObject<Document>(Table_MaintenanceGraph[Table_MaintenanceGraph.FocusedRowIndex][RefEquipmentCard.MaintenanceGraph.BaseDocument].ToGuid()));

            Table_MaintenanceGraph.RemoveRow(CardScript.BaseObject, Table_MaintenanceGraph.FocusedRowItem);
            if (Table_MaintenanceGraph.RowCount == 0) ICardControl.DisableTableBarItem(RefEquipmentCard.MaintenanceGraph.Alias, 4, true);
        }
        #endregion

        #region Обработчики контрола "Группа оборудования"
        /// <summary>
        /// Обработчик события изменения значения контрола "Группа оборудования"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Categories_ValueChanged(Object sender, EventArgs e)
        {
            if (control_Categories.SelectedItems.Count > 0)
            {
                Guid CurrentEquipmentGroup = control_Categories.SelectedItems.First().ObjectId;
                if (CurrentEquipmentGroup == RefEquipmentCard.EquipmentGroups.Software)
                {
                    // Разблокировать поле "Дата окончания лицензии"
                }
                else
                {
                    // Заблокировать поле "Дата окончания лицензии"
                }
                if (CurrentEquipmentGroup == RefEquipmentCard.EquipmentGroups.ToolsAndAccessories)
                {
                    // Разблокировать доп. информацию для остастки в производстве.
                }
                else
                {
                    // Заблокировать доп. информацию для остастки в производстве.
                }
            }
            else
            {
                // Заблокировать поле "Дата окончания лицензии"
                // Заблокировать доп. информацию для остастки в производстве.
            }
        }
        #endregion
        
        #endregion
    }
}