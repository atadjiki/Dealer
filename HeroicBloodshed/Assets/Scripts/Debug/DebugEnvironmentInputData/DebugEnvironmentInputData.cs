using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebugEnvironmentTileData : MonoBehaviour
{
    [SerializeField] private GameObject Prefab_ListItem;

    private void Update()
    {
        if(EnvironmentManager.Instance != null)
        {
            EnvironmentInputData inputData = EnvironmentManager.Instance.GetInputData();

            
        }
    }

    private void GenerateListItem(string label, string value)
    {
      //  GameObject prefabObject = Instantiate<GameObject>(Prefab_ListItem)
    }
}
