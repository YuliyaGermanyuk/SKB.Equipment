using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SKB.Base.Ref
{
    /// <summary>
    /// Схема карточки "Оборудование".
    /// </summary>
    public static class RefEquipmentCard
    {
        /// <summary>
        /// Псевдоним карточки.
        /// </summary>
        public const String Alias = "EquipmentCard";
        /// <summary>
        /// Название карточки.
        /// </summary>
        public const String Name = "Оборудование";
        /// <summary>
        /// Идентификатор типа карточки.
        /// </summary>
        public static readonly Guid ID = new Guid("{00690046-7CF1-4A0F-9A88-48AC3EED0E51}");
        /// <summary>
        /// Название правила для получения архивного номера.
        /// </summary>
        public const String ArchiveNumberRuleName = "СКБ Оборудование";
        /// <summary>
        /// Название правила для получения номера учетной карточки метрологической лаборатории.
        /// </summary>
        public const String NumberOfRegistrationСardRuleName = "СКБ Оборудование метрологической лаборатории";
        /// <summary>
        /// Команды ленты.
        /// </summary>
        public static class RibbonItems
        {
            /// <summary>
            /// Команда "Отправить".
            /// </summary>
            public const String Send = "Send";
        }
        /// <summary>
        /// Кнопки карточки.
        /// </summary>
        public static class Buttons
        {
            /// <summary>
            /// Кнопка "Загрузить файлы".
            /// </summary>
            public const String Upload = "Upload";
        }
        /// <summary>
        /// Роли карточки.
        /// </summary>
        public static class UserRoles
        {
            /// <summary>
            /// Администратор.
            /// </summary>
            public const String Admin = "Admin";
            /// <summary>
            /// Все.
            /// </summary>
            public const String AllUsers = "AllUsers";
            /// <summary>
            /// Регистратор
            /// </summary>
            public const String Creator = "Creator";
            /// <summary>
            /// Специалист по обеспечению бизнеса.
            /// </summary>
            public const String ProvisionManager = "ProvisionManager";
            /// <summary>
            /// Сотрудник, ответственный за оборудование
            /// </summary>
            public const String ResponsibleForEquipment = "ResponsibleForEquipment";
            /// <summary>
            /// Руководитель, ответственный за оборудование
            /// </summary>
            public const String ManagerResponsibleForEquipment = "ManagerResponsibleForEquipment";
            /// <summary>
            /// Бухгалтер
            /// </summary>
            public const String Accountant = "Accountant";
        }
        /// <summary>
        /// Действия карточки "Оборудование".
        /// </summary>
        public static class Actions
        {
            /// <summary>
            /// Действие "Открыть файлы".
            /// </summary>
            public static readonly Guid OpenFiles = new Guid("{D82B885B-AB88-4E3C-833A-98A5C66A75C7}");
            /// <summary>
            /// Действие "Открыть карточку и файлы".
            /// </summary>
            public static readonly Guid OpenCardAndFiles = new Guid("{6210C6F2-6B49-465B-A603-6ACA592B19B9}");
            /// <summary>
            /// Действие "Открыть карточку".
            /// </summary>
            public static readonly Guid OpenCard = new Guid("{D7DD8A1A-E091-4C56-83A9-F1BF20231E7F}");
            /// <summary>
            /// Действие "Удалить карточку и файлы".
            /// </summary>
            public static readonly Guid Delete = new Guid("{10F6CDFF-7732-480C-ACAC-E29FCFB4E4FC}");
        }
        /// <summary>
        /// Режимы открытия карточки "Оборудование".
        /// </summary>
        public static class Modes
        {
            /// <summary>
            /// Режим открытия "Открытие файлов".
            /// </summary>
            public static readonly Guid OpenFiles = new Guid("{049BB63B-22F9-4172-8EE7-198467BF7B8E}");
            /// <summary>
            /// Режим открытия "Открытие карточки и файлов".
            /// </summary>
            public static readonly Guid OpenCardAndFiles = new Guid("{F49E1427-C752-435F-9E17-2EC3F989CA6B}");
            /// <summary>
            /// Режим открытия "Открытие карточки".
            /// </summary>
            public static readonly Guid OpenCard = new Guid("{E9D3A825-EAC9-41EA-81C5-A2373D398D5D}");
        }
        /// <summary>
        /// Секция "Основная информация".
        /// </summary>
        public static class MainInfo
        {
            /// <summary>
            /// Псевдоним секции.
            /// </summary>
            public const String Alias = "MainInfo";
            /// <summary>
            /// Идентификатор секции.
            /// </summary>
            public static readonly Guid ID = new Guid("{181BB466-D6CB-42AA-92EE-B3D69B1E1AB6}");
            /// <summary>
            /// Поле "Архивный номер".
            /// </summary>
            public const String ArchiveNumber = "ArchiveNumber";
            /// <summary>
            /// Поле "Бухгалтерский номер".
            /// </summary>
            public const String AccountingNumber = "AccountingNumber";
            /// <summary>
            /// Поле "Дата регистрации".
            /// </summary>
            public const String RegistrationDate = "RegistrationDate";
            /// <summary>
            /// Поле "Регистратор".
            /// </summary>
            public const String Registrar = "Registrar";
            /// <summary>
            /// Поле "Дата оприходования".
            /// </summary>
            public const String PostingDate = "PostingDate";
            /// <summary>
            /// Поле "Дата выбытия".
            /// </summary>
            public const String DisposalsDate = "DisposalsDate";
            /// <summary>
            /// Поле "Наименование оборудования".
            /// </summary>
            public const String Name = "Name";
            /// <summary>
            /// Поле "Марка/модель".
            /// </summary>
            public const String BrandModel = "BrandModel";
            /// <summary>
            /// Поле "Поставщик".
            /// </summary>
            public const String Supplier = "Supplier";
            /// <summary>
            /// Поле "Дата окончания гарантии".
            /// </summary>
            public const String EndDateOfGuarantee = "EndDateOfGuarantee";
            /// <summary>
            /// Поле "Комментарии".
            /// </summary>
            public const String Comments = "Comments";
            /// <summary>
            /// Поле "Файлы".
            /// </summary>
            public const String Files = "Files";
            /// <summary>
            /// Поле "Папка".
            /// </summary>
            public const String Folder = "Folder";
            /// <summary>
            /// Поле "Уведомления".
            /// </summary>
            public const String NoticeList = "NoticeList";
            /// <summary>
            /// Поле "Срок действия".
            /// </summary>
            public const String Validity = "Validity";
            /// <summary>
            /// Поле "Год выпуска".
            /// </summary>
            public const String YearOfIssue = "YearOfIssue";
            /// <summary>
            /// Поле "Дата ввода в эксплуатацию".
            /// </summary>
            public const String CommissioningDate = "CommissioningDate";
            /// <summary>
            /// Поле "Дата вывода из эксплуатации".
            /// </summary>
            public const String DecommissioningDate = "DecommissioningDate";
            /// <summary>
            /// Поле "Заводской номер".
            /// </summary>
            public const String SerialNumber = "SerialNumber";
            /// <summary>
            /// Поле "Изготовитель".
            /// </summary>
            public const String Manufacturer = "Manufacturer";
        }
        /// <summary>
        /// Секция "Ответственные за оборудование".
        /// </summary>
        public static class ResponsibleForEquipment
        {
            /// <summary>
            /// Псевдоним секции.
            /// </summary>
            public const String Alias = "ResponsibleForEquipment";
            /// <summary>
            /// Идентификатор секции.
            /// </summary>
            public static readonly Guid ID = new Guid("{D1703B9E-C220-4781-AB00-65EBAE363A8F}");
            /// <summary>
            /// Поле "Сотрудник".
            /// </summary>
            public const String Employee = "Employee";
            /// <summary>
            /// Поле "Подразделение".
            /// </summary>
            public const String Department = "Department";
            /// <summary>
            /// Поле "Дата ввода в эксплуатацию".
            /// </summary>
            public const String CommissioningDate = "CommissioningDate";
            /// <summary>
            /// Поле "Должность".
            /// </summary>
            public const String Position = "Position";
            /// <summary>
            /// Поле "Дата вывода из эксплуатации".
            /// </summary>
            public const String DecommissioningDate = "DecommissioningDate";
        }
        /// <summary>
        /// Секция "Категории".
        /// </summary>
        public static class Categories
        {
            /// <summary>
            /// Псевдоним секции.
            /// </summary>
            public const String Alias = "Categories";
            /// <summary>
            /// Идентификатор секции.
            /// </summary>
            public static readonly Guid ID = new Guid("{12F49D29-62B3-4C67-ACE1-2E5FB3155FA7}");
            /// <summary>
            /// Поле "Категории".
            /// </summary>
            public const String CategoriesValue = "Categories";
        }
        /// <summary>
        /// Секция "Метрологическая лаборатория".
        /// </summary>
        public static class MetrologicalLaboratory
        {
            /// <summary>
            /// Отображаемое состояние карточки.
            /// </summary>
            public enum CardStatus
            {
                /// <summary>
                /// Эталон.
                /// </summary>
                [Description("Gauge")]
                Gauge = 0,
                /// <summary>
                /// Средство измерения (СИ).
                /// </summary>
                [Description("MeasuringTool")]
                MeasuringTool = 1,
                /// <summary>
                /// Испытательное.
                /// </summary>
                [Description("Testing")]
                Testing = 2,
                /// <summary>
                /// Вспомогательное.
                /// </summary>
                [Description("Auxiliary")]
                Auxiliary = 3,
                /// <summary>
                /// Длительное хранение.
                /// </summary>
                [Description("LongTermStorage")]
                LongTermStorage = 4,
                /// <summary>
                /// Непригодное.
                /// </summary>
                [Description("Unfit")]
                Unfit = 5,
            };
            /// <summary>
            /// Псевдоним секции.
            /// </summary>
            public const String Alias = "MetrologicalLaboratory";
            /// <summary>
            /// Идентификатор секции.
            /// </summary>
            public static readonly Guid ID = new Guid("{2F19AD22-2733-4AF2-AD10-F80C7043B180}");
            /// <summary>
            /// Поле "Номер учетной карточки".
            /// </summary>
            public const String NumberOfRegistrationСard = "NumberOfRegistrationСard";
            /// <summary>
            /// Поле "Номер в ГосРеестре".
            /// </summary>
            public const String NumberOfStateRegister = "NumberOfStateRegister";
            /// <summary>
            /// Поле "Статус".
            /// </summary>
            public const String Status = "Status";
            /// <summary>
            /// Поле "Основной эксплуатационный документ".
            /// </summary>
            public const String BasicDocument = "BasicDocument";
            /// <summary>
            /// Поле "Отображаемое название в документах".
            /// </summary>
            public const String DisplayNameInDocuments = "DisplayNameInDocuments";
            /// <summary>
            /// Поле "Программное обеспечение".
            /// </summary>
            public const String Software = "Software";
        }
        /// <summary>
        /// Секция "Поверка".
        /// </summary>
        public static class Verification
        {
            /// <summary>
            /// Псевдоним секции.
            /// </summary>
            public const String Alias = "Verification";
            /// <summary>
            /// Идентификатор секции.
            /// </summary>
            public static readonly Guid ID = new Guid("{04067E85-6DFA-48BA-9134-25806BBD9B61}");
            /// <summary>
            /// Поле "Межповерочный интервал".
            /// </summary>
            public const String VerificationInterval = "VerificationInterval";
            /// <summary>
            /// Поле "Единица измерения времени".
            /// </summary>
            public const String VerifyUnitOfTime = "VerifyUnitOfTime";
        }
        /// <summary>
        /// Секция "Калибровка".
        /// </summary>
        public static class Calibration
        {
            /// <summary>
            /// Псевдоним секции.
            /// </summary>
            public const String Alias = "Calibration";
            /// <summary>
            /// Идентификатор секции.
            /// </summary>
            public static readonly Guid ID = new Guid("{3B982E27-B674-4210-9C6E-FCF39DDD9876}");
            /// <summary>
            /// Поле "Межкалибровочный интервал".
            /// </summary>
            public const String CalibrationInterval = "CalibrationInterval";
            /// <summary>
            /// Поле "Единица измерения времени".
            /// </summary>
            public const String CalibrationUnitOfTime = "CalibrationUnitOfTime";
        }
        /// <summary>
        /// Секция "Аттестация".
        /// </summary>
        public static class Attestation
        {
            /// <summary>
            /// Псевдоним секции.
            /// </summary>
            public const String Alias = "Attestation";
            /// <summary>
            /// Идентификатор секции.
            /// </summary>
            public static readonly Guid ID = new Guid("{54CD291B-E6B5-4F84-BF8E-C4D00B185E4B}");
            /// <summary>
            /// Поле "Межаттестационный интервал".
            /// </summary>
            public const String AttestationInterval = "AttestationInterval";
            /// <summary>
            /// Поле "Единица измерения времени".
            /// </summary>
            public const String AttestationUnitOfTime = "AttestationUnitOfTime";
        }
        /// <summary>
        /// Секция "Техническая информация".
        /// </summary>
        public static class TechnicalInformation
        {
            /// <summary>
            /// Псевдоним секции.
            /// </summary>
            public const String Alias = "TechnicalInformation";
            /// <summary>
            /// Идентификатор секции.
            /// </summary>
            public static readonly Guid ID = new Guid("{6F4D8EC6-6BEF-4756-9E0C-110E2077385E}");
            /// <summary>
            /// Поле "Назначение (текст)".
            /// </summary>
            public const String PurposeText = "PurposeText";
            /// <summary>
            /// Поле "Назначение (форматирование)".
            /// </summary>
            public const String PurposeFormatting = "PurposeFormatting";
            /// <summary>
            /// Поле "Технические характеристики (текст)".
            /// </summary>
            public const String SpecificationsText = "SpecificationsText";
            /// <summary>
            /// Поле "Технические характеристики (форматирование)".
            /// </summary>
            public const String SpecificationsFormatting = "SpecificationsFormatting";
        }
        /// <summary>
        /// Секция "Техническое обслуживание".
        /// </summary>
        public static class Maintenance
        {
            /// <summary>
            /// Псевдоним секции.
            /// </summary>
            public const String Alias = "Maintenance";
            /// <summary>
            /// Идентификатор секции.
            /// </summary>
            public static readonly Guid ID = new Guid("{8ACBF2A7-E8F6-4BE3-8FFC-13C68787925A}");
            /// <summary>
            /// Поле "Периодичность обслуживания".
            /// </summary>
            public const String MaintenanceInterval = "MaintenanceInterval";
            /// <summary>
            /// Поле "Единица измерения времени".
            /// </summary>
            public const String MaintenanceUnitOfTime = "MaintenanceUnitOfTime";
        }
        /// <summary>
        /// Секция "Сведения об отказах и ремонтах".
        /// </summary>
        public static class FailuresAndRepairs
        {
            /// <summary>
            /// Псевдоним секции.
            /// </summary>
            public const String Alias = "FailuresAndRepairs";
            /// <summary>
            /// Идентификатор секции.
            /// </summary>
            public static readonly Guid ID = new Guid("{ACF0E137-EB73-4DC3-8D4C-0FC9C99DFD98}");
            /// <summary>
            /// Поле "Дата".
            /// </summary>
            public const String FailureDate = "FailureDate";
            /// <summary>
            /// Поле "Описание".
            /// </summary>
            public const String FailureDescription = "FailureDescription";
            /// <summary>
            /// Поле "Сотрудник".
            /// </summary>
            public const String Employee = "Employee";
        }
        /// <summary>
        /// Секция "Длительное хранение".
        /// </summary>
        public static class LongTermStorage
        {
            /// <summary>
            /// Псевдоним секции.
            /// </summary>
            public const String Alias = "LongTermStorage";
            /// <summary>
            /// Идентификатор секции.
            /// </summary>
            public static readonly Guid ID = new Guid("{056C1DB7-C333-41BD-8BAB-EE9B45AB33C8}");
            /// <summary>
            /// Поле "Дата начала длительного хранения".
            /// </summary>
            public const String StartDate = "StartDate";
            /// <summary>
            /// Поле "Дата окончания длительного хранения".
            /// </summary>
            public const String EndDate = "EndDate";
        }
        /// <summary>
        /// Секция "Приборы".
        /// </summary>
        public static class UsedForDevices
        {
            /// <summary>
            /// Псевдоним секции.
            /// </summary>
            public const String Alias = "UsedForDevices";
            /// <summary>
            /// Идентификатор секции.
            /// </summary>
            public static readonly Guid ID = new Guid("{33D3E420-9B17-46C3-897D-2CADE88025F3}");
            /// <summary>
            /// Поле "Прибор".
            /// </summary>
            public const String Device = "Device";
            /// <summary>
            /// Поле "Калибровка".
            /// </summary>
            public const String Calibration = "Calibration";
            /// <summary>
            /// Поле "Поверка".
            /// </summary>
            public const String Verify = "Verify";
        }
        /// <summary>
        /// Секция "График поверок".
        /// </summary>
        public static class VerificationGraph
        {
            /// <summary>
            /// Псевдоним секции.
            /// </summary>
            public const String Alias = "VerificationGraph";
            /// <summary>
            /// Идентификатор секции.
            /// </summary>
            public static readonly Guid ID = new Guid("{EA3C6498-67F2-4823-90EE-86219494D090}");
            /// <summary>
            /// Поле "Дата проведения поверки".
            /// </summary>
            public const String DateOfVerification = "DateOfVerification";
            /// <summary>
            /// Поле "Дата следующей поверки".
            /// </summary>
            public const String DateOfNextVerification = "DateOfNextVerification";
            /// <summary>
            /// Поле "Документ, подтверждающий поверку".
            /// </summary>
            public const String VerificationDocument = "VerificationDocument";
            /// <summary>
            /// Поле "Номер документа".
            /// </summary>
            public const String NumberOfDocument = "NumberOfDocument";
            /// <summary>
            /// Поле "Следующая поверка выполнена".
            /// </summary>
            public const String NextVerificationIsDone = "NextVerificationIsDone";
        }
        /// <summary>
        /// Секция "График калибровок".
        /// </summary>
        public static class CalibrationGraph
        {
            /// <summary>
            /// Псевдоним секции.
            /// </summary>
            public const String Alias = "CalibrationGraph";
            /// <summary>
            /// Идентификатор секции.
            /// </summary>
            public static readonly Guid ID = new Guid("{37577B42-E728-4739-89F3-5AABA49FE617}");
            /// <summary>
            /// Поле "Дата проведения калибровки".
            /// </summary>
            public const String DateOfCalibration = "DateOfCalibration";
            /// <summary>
            /// Поле "Дата следующей калибровки".
            /// </summary>
            public const String DateOfNextCalibration = "DateOfNextCalibration";
            /// <summary>
            /// Поле "Документ, подтверждающий калибровку".
            /// </summary>
            public const String CalibrationDocument = "CalibrationDocument";
            /// <summary>
            /// Поле "Номер документа".
            /// </summary>
            public const String NumberOfDocument = "NumberOfDocument";
            /// <summary>
            /// Поле "Следующая калибровка выполнена".
            /// </summary>
            public const String NextCalibrationIsDone = "NextCalibrationIsDone";
        }
        /// <summary>
        /// Секция "График аттестаций".
        /// </summary>
        public static class AttestationGraph
        {
            /// <summary>
            /// Псевдоним секции.
            /// </summary>
            public const String Alias = "AttestationGraph";
            /// <summary>
            /// Идентификатор секции.
            /// </summary>
            public static readonly Guid ID = new Guid("{BE2648DB-8F81-4488-9C14-33BDD681D573}");
            /// <summary>
            /// Поле "Дата проведения аттестации".
            /// </summary>
            public const String DateOfAttestation = "DateOfAttestation";
            /// <summary>
            /// Поле "Дата следующей аттестации".
            /// </summary>
            public const String DateOfNextAttestation = "DateOfNextAttestation";
            /// <summary>
            /// Поле "Документ, подтверждающий аттестацию".
            /// </summary>
            public const String AttestationDocument = "AttestationDocument";
            /// <summary>
            /// Поле "Номер документа".
            /// </summary>
            public const String NumberOfDocument = "NumberOfDocument";
            /// <summary>
            /// Поле "Следующая аттестация выполнена".
            /// </summary>
            public const String NextAttestationIsDone = "NextAttestationIsDone";
        }
        /// <summary>
        /// Секция "График технического обслуживания".
        /// </summary>
        public static class MaintenanceGraph
        {
            /// <summary>
            /// Псевдоним секции.
            /// </summary>
            public const String Alias = "MaintenanceGraph";
            /// <summary>
            /// Идентификатор секции.
            /// </summary>
            public static readonly Guid ID = new Guid("{E284F9C6-5789-4A1F-BEB7-48A7D6E0DD39}");
            /// <summary>
            /// Поле "Дата технического обслуживания".
            /// </summary>
            public const String DateOfMaintenance = "DateOfMaintenance";
            /// <summary>
            /// Поле "Дата следующего технического обслуживания".
            /// </summary>
            public const String DateOfNextMaintenance = "DateOfNextMaintenance";
            /// <summary>
            /// Поле "Документ-основание".
            /// </summary>
            public const String BaseDocument = "BaseDocument";
            /// <summary>
            /// Поле "Ответственный сотрудник".
            /// </summary>
            public const String Employee = "Employee";
            /// <summary>
            /// Поле "Должность ответственного".
            /// </summary>
            public const String Position = "Position";
        }
        /// <summary>
        /// Секция "Применяемость".
        /// </summary>
        public static class Applicability
        {
            /// <summary>
            /// Псевдоним секции.
            /// </summary>
            public const String Alias = "Applicability";
            /// <summary>
            /// Идентификатор секции.
            /// </summary>
            public static readonly Guid ID = new Guid("{4FB4A7D8-DB20-46B2-8012-C97B40A6B650}");
            /// <summary>
            /// Поле "Код СКБ".
            /// </summary>
            public const String Code = "Code";
        }
        /// <summary>
        /// Подразделения процесса
        /// </summary>
        public static class Departments
        {
            /// <summary>
            /// Метрологическая лаборатория.
            /// </summary>
            public const String MetrologicalLaboratory = "Метрологическая лаборатория";
        }
        /// <summary>
        /// Группы оборудования
        /// </summary>
        public static class EquipmentGroups
        {
            /// <summary>
            /// БП - Бытовые приборы.
            /// </summary>
            public static readonly Guid HouseholdEquipment = new Guid("{09071BDF-6B27-4351-9964-D3470C65894B}");
            /// <summary>
            /// ВТ - Вычислительная техника.
            /// </summary>
            public static readonly Guid ComputingEquipment = new Guid("{8BE45C6A-CFD6-44B2-8920-E177116B9CA5}");
            /// <summary>
            /// ИО - Инструменты и оснастка.
            /// </summary>
            public static readonly Guid ToolsAndAccessories = new Guid("{0C3428CA-5A08-48A3-A64D-2DEB1D065802}");
            /// <summary>
            /// КИП - Контрольно-измерительные приборы.
            /// </summary>
            public static readonly Guid ControlAndMeasuringEquipment = new Guid("{3D40404A-4237-4848-8435-6CC0F5AF8324}");
            /// <summary>
            /// МБ - Мебель.
            /// </summary>
            public static readonly Guid Furniture = new Guid("{C048C630-0720-4FEC-85E5-511154D23DBE}");
            /// <summary>
            /// ПО - Программное обеспечение (покупное).
            /// </summary>
            public static readonly Guid Software = new Guid("{D7B3928E-7F5C-4B68-AA42-BBE04FA3585C}");
            /// <summary>
            /// СО - Станки и оборудование.
            /// </summary>
            public static readonly Guid MachineTools = new Guid("{9533108E-4111-40F8-BC5C-3E685B796E85}");
        }
        public static class Enums
        {
            /// <summary>
            /// Типы проверки.
            /// </summary>
            public enum TypeOfInspection
            {
                /// <summary>
                /// Поверка.
                /// </summary>
                [Description("Verification")]
                Verification = 0,
                /// <summary>
                /// Калибровка.
                /// </summary>
                [Description("Calibration")]
                Calibration = 1,
                /// <summary>
                /// Аттестация.
                /// </summary>
                [Description("Attestation")]
                Attestation = 2,
                /// <summary>
                /// Техническое обслуживание.
                /// </summary>
                [Description("Maintenance")]
                Maintenance = 3

            };
            /// <summary>
            /// Единицы измерения.
            /// </summary>
            public enum Units
            {
                /// <summary>
                /// Дни.
                /// </summary>
                [Description("Days")]
                Days = 0,
                /// <summary>
                /// Недели.
                /// </summary>
                [Description("Weeks")]
                Weeks = 1,
                /// <summary>
                /// Месяцы.
                /// </summary>
                [Description("Months")]
                Months = 2,
                /// <summary>
                /// Годы.
                /// </summary>
                [Description("Years")]
                Years = 3
            };
        }
    }
}