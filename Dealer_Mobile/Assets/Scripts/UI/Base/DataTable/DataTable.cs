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
public class DataTableInfo
{
    public List<ColumnData> Columns = new List<ColumnData>();
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

    private Dictionary<string, DataTableColumn> _columnObjects;

    private List<List<string>> _roWValues;

    private void Awake()
    {
        _columnObjects = new Dictionary<string, DataTableColumn>();
        _roWValues = new List<List<string>>();
    }

    public void GenerateTable(DataTableInfo tableInfo)
    {
        if (tableInfo == null) { return; }

        _data = tableInfo;

        GenerateColumnObjects();

        GenerateHeaders();

        GenerateValues();
    }

    private void GenerateColumnObjects()
    {
        foreach(ColumnData columnData in _data.Columns)
        {
            GameObject columnObject = Instantiate<GameObject>(Prefab_Column, Contents.transform);
            columnObject.name = "Col_" + columnData.Name;

            DataTableColumn column = columnObject.GetComponent<DataTableColumn>();

            if(column != null)
            {
                _columnObjects.Add(columnData.Name, column);
            }
        }
    }

    private void GenerateHeaders()
    {
        foreach(ColumnData columnData in _data.Columns)
        {
            GameObject columnObject = _columnObjects[columnData.Name].gameObject;

            GameObject headerObject = Instantiate<GameObject>(Prefab_Column_Header, columnObject.transform);
            headerObject.name = "Header_" + columnData.Name;
            TextMeshProUGUI headerText = headerObject.GetComponentInChildren<TextMeshProUGUI>();
            if (headerText != null)
            {
                headerText.SetText(columnData.Name);
            }

            DataTableHeaderCell headerCell = headerObject.GetComponent<DataTableHeaderCell>();
            if (headerCell != null)
            {
                headerCell.onClick.AddListener(() => OnHeaderSelected(columnData.Name));
            }
        }
    }

    private void GenerateValues()
    {
        foreach (ColumnData columnData in _data.Columns)
        {
            GameObject columnObject = _columnObjects[columnData.Name].gameObject;

            foreach (string value in columnData.Values)
            {
                GameObject cellObject = Instantiate<GameObject>(Prefab_Column_Cell, columnObject.transform);
                TextMeshProUGUI cellText = cellObject.GetComponentInChildren<TextMeshProUGUI>();
                if (cellText != null)
                {
                    cellText.SetText(value);
                }
            }
        }
    }

    private void OnHeaderSelected(string columnID)
    {
        Debug.Log("Column " + columnID + " clicked");

        SortDataBy(columnID);
    }

    private void SortDataBy(string columnID)
    {
        ColumnData columnData = GetColumnData(columnID);

        if(columnData != null)
        {
            List<string> sortedValues = columnData.Values;

            sortedValues.Sort();
        }
    }

    private ColumnData GetColumnData(string columnID)
    {
        foreach(ColumnData columnData in _data.Columns)
        {
            if(columnData.Name == columnID)
            {
                return columnData;
            }
        }

        return null;
    }
}
