using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

using DocsVision.BackOffice.WinForms;
using DocsVision.BackOffice.ObjectModel;
using DocsVision.Platform.CardHost;
using DocsVision.Platform.WinForms;
using DocsVision.Platform.ObjectManager;

using SKB.Base.Ref;
using SKB.Base;

namespace SKB.Equipment.Controls
{
    /// <summary>
    /// Компонент карточки "Оборудование".
    /// </summary>
    [Customizable(true)]
    [CardFrameWindowType(typeof(CardFrameForm))]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    [Guid("65501459-7F9C-4B7A-822A-45E49C80FFE6")]
    public sealed class EquipmentCardControl : BaseCardControl
    {
        /// <summary>
        /// Инициализирует компонент карточки.
        /// </summary>
        public EquipmentCardControl() : base() { System.Windows.Forms.MessageBox.Show("Инициализация"); }
        /// <summary>
        /// Обработчик события, инициируемого после активации компонента карточки.
        /// </summary>
        /// <param name="e">Параметры активации.</param>
        protected override void OnCardActivated (CardActivatedEventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Активация");
            base.OnCardActivated(e);
        }
        /// <summary>
        /// Обработчик специального события, инициируемого после активации пользоваетелем одного из методов карточки.
        /// </summary>
        /// <param name="e">Параметры метода карточки.</param>
        protected override void OnCardAction (CardActionEventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Действие");
            base.OnCardAction(e);
            try
            {
                System.Windows.Forms.MessageBox.Show("Режимы");
                if (e.ActionId == RefEquipmentCard.Actions.OpenFiles)
                {
                    if (!CardData.IsNull())
                    {
                        String FolderPath = CardData.Sections[RefEquipmentCard.MainInfo.ID].FirstRow.GetString(RefEquipmentCard.MainInfo.Folder);
                        if (!String.IsNullOrWhiteSpace(FolderPath) && Directory.Exists(FolderPath))
                            Process.Start("explorer", "\"" + FolderPath + "\"");
                        else
                            CardHost.ShowCard(CardData.Id, RefEquipmentCard.Modes.OpenFiles, this.CardData.ArchiveState == ArchiveState.NotArchived ? ActivateMode.Edit : ActivateMode.ReadOnly);
                    }
                }
                else if (e.ActionId == RefEquipmentCard.Actions.OpenCardAndFiles)
                    CardHost.ShowCard(CardData.Id, RefEquipmentCard.Modes.OpenCardAndFiles, this.CardData.ArchiveState == ArchiveState.NotArchived ? ActivateMode.Edit : ActivateMode.ReadOnly);
                else if (e.ActionId == RefEquipmentCard.Actions.OpenCard)
                    CardHost.ShowCard(CardData.Id, RefEquipmentCard.Modes.OpenCard, this.CardData.ArchiveState == ArchiveState.NotArchived ? ActivateMode.Edit : ActivateMode.ReadOnly);
                else if (e.ActionId == RefEquipmentCard.Actions.Delete)
                {
                    IList<StatesOperation> Operations = StateService.GetOperations(BaseObject.SystemInfo.CardKind) ?? new List<StatesOperation>();
                    StatesOperation Operation = Operations.FirstOrDefault(item => item.DefaultName == "Modify");
                    if (!Operation.IsNull())
                    {
                        if (AccessCheckingService.IsOperationAllowed(BaseObject, Operation))
                        {
                            switch (ShowMessage("Вы уверены, что хотите удалить выбранную карточку и связанные файлы?", "Docsvision Navigator", MessageType.Question, MessageButtons.YesNo))
                            {
                                case MessageResult.Yes:
                                    Boolean ByMe;
                                    String OwnerName;
                                    if (!LockService.IsObjectLocked<BaseCard>(BaseObject, out ByMe, out OwnerName))
                                    {
                                        if (Session.DeleteCard(CardData.Id))
                                            ShowMessage("Карточка и файлы удалены!", "Docsvision Navigator", MessageType.Information, MessageButtons.Ok);
                                        else
                                            ShowMessage("Не удалось удалить карточку!" + Environment.NewLine 
                                                + "Обратитесь к системному администратору!", "Docsvision Navigator", MessageType.Error, MessageButtons.Ok);
                                    }
                                    else
                                        ShowMessage("Невозможно удалить карточку " + BaseObject.Description + "." + Environment.NewLine
                                                + "Карточка заблокирована " + (ByMe ? "вами" : "пользователем " + OwnerName) + "!", "Docsvision Navigator", MessageType.Warning, MessageButtons.Ok);
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex) {this.ProcessException(ex);}
        }
    }
}