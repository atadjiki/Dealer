using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

namespace GameDelegates
{
    public delegate void DrugTransfer(Constants.Inventory.OwnerID Sender, Constants.Inventory.OwnerID Recepient, Constants.Inventory.ItemID Item, int Quantity);

    public delegate void SafehouseMenuComplete(Safehouse.SafehouseMenu Menu);

    public class Global
    {
        public static DrugTransfer OnDrugTransfer;

        public static SafehouseMenuComplete OnSafehouseMenuComplete;
    }
}
