using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Text;

public class OverlayMenu_Stash : OverlayMenu
{
    [SerializeField] private TextMeshProUGUI Text_Capacity;

    [Header("Table")]
    [SerializeField] private TablePanel DataTable;

    [Header("Detail Panel")]
    [SerializeField] private Image Image_Item;
    [SerializeField] private TextMeshProUGUI Text_Item;

    private void Awake()
    {
        TableData tableData = CreateTableData();

        DataTable.GenerateTable(tableData);
    }

    private TableData CreateTableData()
    {
        //get player data
        Inventory inventory = FindObjectOfType<Inventory>(true);

        if(inventory != null)
        {
            //pack it into columns
            TableData tableData = new TableData();

            ColumnData column_name = new ColumnData("Name");

            ColumnData column_grade = new ColumnData("Grade");

            ColumnData column_quantity = new ColumnData("Quantity");

            foreach (InventoryItem item in inventory.GetItems())
            {
                string itemName = Constants.Inventory.Format(item.ID);

                itemName = AppendSpaces(itemName, 12);

                column_name.Values.Add(itemName);
                column_grade.Values.Add(Constants.Inventory.Drugs.Quality.None.ToString());
                column_quantity.Values.Add(""+item.Quantity);
            }

            tableData.Columns.Add(column_name);
            tableData.Columns.Add(column_grade);
            tableData.Columns.Add(column_quantity);

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

