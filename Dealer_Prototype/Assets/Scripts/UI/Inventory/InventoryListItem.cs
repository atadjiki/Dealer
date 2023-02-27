using System.Collections;
using System.Collections.Generic;
using Constants;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class InventoryListItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Text_Name;
    [SerializeField] private TextMeshProUGUI Text_Quantity;

    public void Setup(Enumerations.InventoryID name, int quantity)
    {
        Text_Name.text = name.ToString();
        Text_Quantity.text = quantity.ToString();
    }
}
