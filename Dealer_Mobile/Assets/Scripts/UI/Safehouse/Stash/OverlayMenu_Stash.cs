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

            ColumnData column_name = new ColumnData("Name");

            ColumnData column_grade = new ColumnData("Grade");

            ColumnData column_quantity = new ColumnData("Quantity");

            foreach (DrugItem item in inventory.GetDrugStash())
            {
                column_name.Values.Add(Drugs.Format(item.ID));
                column_grade.Values.Add(Drugs.Format(item.Grade));
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

