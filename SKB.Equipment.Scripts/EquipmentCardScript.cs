using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocsVision.BackOffice.WinForms;
using DocsVision.Platform.WinForms;

using SKB.Equipment.Cards;

namespace Equipment
{
    public class EquipmentCardScript : ScriptClassBase
    {
        #region Properties
        SKB.Equipment.Cards.EquipmentCard Card = null;
        #endregion

        #region Event Handlers
        private void ServicesCard_CardActivated(Object sender, CardActivatedEventArgs e)
        {
            Card = new EquipmentCard(this);
        }
        #endregion
    }
}