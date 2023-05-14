using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

namespace GameDelegates
{
    public delegate void DrugTransfer(Constants.Inventory.ID From, Constants.Inventory.ID To, Constants.Inventory.Drugs.ID ID, int amount);

    public class Global
    {
        public static DrugTransfer OnDrugTransfer;
    }
}
