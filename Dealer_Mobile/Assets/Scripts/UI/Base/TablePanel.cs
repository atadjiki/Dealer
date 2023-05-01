using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

[Serializable]
public class ColumnData
{
    public string Name;

    public List<string> Values;

    public ColumnData(string _Name)
    {
        Name = _Name;
        Values = new List<string>();
    }
}

[Serializable]
public class TableData
{
    public List<ColumnData> Columns = new List<ColumnData>();
}

public class TablePanel : MonoBehaviour
{
    [Header("Table Transforms")]
    [SerializeField] private GameObject Contents;

    [Header("Prefabs")]
    [SerializeField] private GameObject Prefab_Column;
    [SerializeField] private GameObject Prefab_Column_Header;
    [SerializeField] private GameObject Prefab_Column_Cell;

    public void GenerateTable(TableData tableData)
    {
        List<ColumnData> columns = tableData.Columns;

        if (columns == null) return;

        foreach(ColumnData column in columns)
        {
            GameObject columnObject = Instantiate<GameObject>(Prefab_Column, Contents.transform);
            columnObject.name = "Col_" + column.Name;

            //generate the header first
            GameObject headerObject = Instantiate<GameObject>(Prefab_Column_Header, columnObject.transform);
            headerObject.name = "Header_" + column.Name;
            TextMeshProUGUI headerText = headerObject.GetComponentInChildren<TextMeshProUGUI>();
            if(headerText != null)
            {
                headerText.SetText(column.Name);
            }

            //now add all the column values
            foreach(string  value in column.Values)
            {
                GameObject cellObject = Instantiate<GameObject>(Prefab_Column_Cell, columnObject.transform);
                TextMeshProUGUI cellText = cellObject.GetComponentInChildren<TextMeshProUGUI>();
                if(cellText != null)
                {
                    cellText.SetText(value);
                }
            }
        }
    }
}
