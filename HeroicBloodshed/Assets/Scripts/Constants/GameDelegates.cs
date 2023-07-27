using static Constants;

public delegate void DrugTransfer(OwnerID Sender, OwnerID Recepient, InventoryItemID Item, int Quantity);

public delegate void SafehouseMenuComplete(SafehouseMenuID MenuID);

public class Global
{
    public static DrugTransfer OnDrugTransfer;

    public static SafehouseMenuComplete OnSafehouseMenuComplete;
}
