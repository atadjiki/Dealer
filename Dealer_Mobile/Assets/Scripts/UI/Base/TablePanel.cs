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
    [SerializeField] private GameObject Header;
    [SerializeField] private GameObject Contents;

    [Header("Prefabs")]
    [SerializeField] private GameObject HeaderPrefab;
    [SerializeField] private GameObject RowPrefab;
    [SerializeField] private GameObject ColumnItemPrefab;

    public void GenerateTable(TableData tableData)
    {
        List<ColumnData> columns = tableData.Columns;

        if (columns == null) return;

        //generate the header first
        foreach(ColumnData column in columns)
        {
            //add the column title to the header
            GameObject headerObject = Instantiate<GameObject>(HeaderPrefab, Header.transform);
            Button columnHeaderButton = headerObject.GetComponent<Button>();

            if(columnHeaderButton != null) { columnHeaderButton.interactable = false; }

            TextMeshProUGUI columnHeaderText = headerObject.GetComponentInChildren<TextMeshProUGUI>();

            if(columnHeaderText != null)
            {
                columnHeaderText.SetText(column.Name);
            }
        }

        int rowCount = GetMaxColumnSize(columns);

        for(int i = 0; i < rowCount; i++)
        {
            GameObject rowPrefab = Instantiate<GameObject>(RowPrefab, Contents.transform);

            foreach(ColumnData column in columns)
            {
                if(column.Values.Count <= rowCount)
                {
                    GameObject columnPrefab = Instantiate<GameObject>(ColumnItemPrefab, rowPrefab.transform);
                    TextMeshProUGUI textMesh = columnPrefab.GetComponentInChildren<TextMeshProUGUI>();

                    if (textMesh != null)
                    {
                        textMesh.SetText(column.Values[i]);
                    }
                }
            }
        }
    }

    private int GetMaxColumnSize(List<ColumnData> columns)
    {
        int max = 0;

        foreach(ColumnData data in columns)
        {
            if(data.Values != null)
            {
                if(data.Values.Count > max)
                {
                    max = data.Values.Count;
                }
            }
        }

        return max;
    }
}


