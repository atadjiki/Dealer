using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class OverlayMenu_Stash : OverlayMenu
{
    [SerializeField] private TextMeshProUGUI Text_Capacity;

    [Header("Header")]
    [SerializeField] private Button Button_Header_Name;
    [SerializeField] private Button Button_Header_Quality;
    [SerializeField] private Button Button_Header_Quantity;

    [Header("Detail Panel")]
    [SerializeField] private Image Image_Item;
    [SerializeField] private TextMeshProUGUI Text_Item;

    [Header("Prefab")]
    [SerializeField] private GameObject Prefab_ListItem;
}
