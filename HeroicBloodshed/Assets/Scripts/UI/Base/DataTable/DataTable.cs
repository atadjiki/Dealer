using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.EventSystems;

[Serializable]
public class RowData : Dictionary<string, string> { }

[Serializable]
public class DataTableInfo
{
    public List<string> Columns = new List<string>(); 
    public List<RowData> Rows = new List<RowData>();
}

public class DataTable : MonoBehaviour
{
    [Header("Table Transforms")]
    [SerializeField] private GameObject Contents;

    [Header("Prefabs")]
    [SerializeField] private GameObject Prefab_Column;
    [SerializeField] private GameObject Prefab_Column_Header;
    [SerializeField] private GameObject Prefab_Column_Cell;

    private DataTableInfo _data;

    private Dictionary<string, GameObject> _columnObjects = new Dictionary<string, GameObject>();

    public void GenerateTable(DataTableInfo tableInfo)
    {
        if (tableInfo == null) { return; }

        if(tableInfo.Columns.Count == 0) { return; }

        _data = tableInfo;

        GenerateColumns();

        SelectColumn(_data.Columns[0]);
    }

    private void GenerateColumns()
    {
        foreach(string columnID in _data.Columns)
        {
            GameObject columnObject = Instantiate<GameObject>(Prefab_Column, Contents.transform);
            columnObject.name = "Col_" + columnID;

            if(columnID != null)
            {
                _columnObjects.Add(columnID, columnObject);
            }

            GameObject headerObject = Instantiate<GameObject>(Prefab_Column_Header, columnObject.transform);
            headerObject.name = "Header_" + columnID;
            TextMeshProUGUI headerText = headerObject.GetComponentInChildren<TextMeshProUGUI>();
            if (headerText != null)
            {
                headerText.SetText(columnID);
            }

            DataTableHeaderCell headerCell = headerObject.GetComponent<DataTableHeaderCell>();
            if (headerCell != null)
            {
                headerCell.onClick.AddListener(() => SelectColumn(columnID));
            }
        }
    }

    private void GenerateRows(List<RowData> Rows)
    {
        foreach (RowData rowData in Rows)
        {
            foreach (string key in rowData.Keys)
            {
                GameObject columnObject = _columnObjects[key].gameObject;

                GameObject cellObject = Instantiate<GameObject>(Prefab_Column_Cell, columnObject.transform);
                TextMeshProUGUI cellText = cellObject.GetComponentInChildren<TextMeshProUGUI>();
                if (cellText != null)
                {
                    cellText.SetText(rowData[key]);
                }
            }
        }
    }

    private void SelectColumn(string columnID)
    {
        ClearContents();

        GenerateColumns();

        GenerateRows(SortDataBy(columnID));

        DataTableHeaderCell headerCell = _columnObjects[columnID].GetComponentInChildren<DataTableHeaderCell>();
        if(headerCell != null)
        {
            headerCell.Select();
        }
    }

    private List<RowData> SortDataBy(string columnID)
    {
        //map each row to its value for this column
        List<KeyValuePair<string, RowData>> RowMap = new List<KeyValuePair<string, RowData>>();

        foreach(RowData rowData in _data.Rows)
        {
            RowMap.Add(new KeyValuePair<string, RowData>(rowData[columnID], rowData));
        }

        List<RowData> SortedRows = new List<RowData>();

        foreach(KeyValuePair<string,RowData> pair in RowMap.OrderBy( x => x.Key))
        {
            SortedRows.Add(pair.Value);
        }

        return SortedRows;
    }

    private void ClearContents()
    {
        for (int i = Contents.transform.childCount - 1; i >= 0; --i)
        {
            var child = Contents.transform.GetChild(i).gameObject;
            Destroy(child);
        }

        _columnObjects = new Dictionary<string, GameObject>();
    }
}
