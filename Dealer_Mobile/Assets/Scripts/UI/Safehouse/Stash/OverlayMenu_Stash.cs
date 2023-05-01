using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Text;
using static Constants.Inventory;

public class OverlayMenu_Stash : OverlayMenu
{
    [SerializeField] private TextMeshProUGUI Text_Capacity;

    [Header("Table")]
    [SerializeField] private DataTable Table;

    [Header("Detail Panel")]
    [SerializeField] private Image Image_Item;
    [SerializeField] private TextMeshProUGUI Text_Item;

    private void Awake()
    {
        DataTableInfo tableData = CreateTableData();

        Table.GenerateTable(tableData);
    }

    private DataTableInfo CreateTableData()
    {
        //get player data
        Inventory inventory = FindObjectOfType<Inventory>(true);

        if(inventory != null)
        {
            //pack it into columns
            DataTableInfo tableData = new DataTableInfo();

            tableData.Columns.Add("Name");
            tableData.Columns.Add("Grade");
            tableData.Columns.Add("Quantity");

            foreach (DrugItem item in inventory.GetDrugStash())
            {
                RowData rowData = new RowData();

                rowData.Add("Name", Drugs.Format(item.ID));
                rowData.Add("Grade", Drugs.Format(item.Grade));
                rowData.Add("Quantity", "" + item.Quantity);

                tableData.Rows.Add(rowData);
            }

            return tableData;
        }

        Debug.Log("Inventory was null");
        return null;
    }

    public static string AppendSpaces(string str, int size)
    { 
        StringBuilder sb = new StringBuilder();
        sb.Append(str);

        if ((size - str.Length) > 0)
        {
            sb.Append(' ', (size - str.Length));
        }

        return sb.ToString();
    }
}

