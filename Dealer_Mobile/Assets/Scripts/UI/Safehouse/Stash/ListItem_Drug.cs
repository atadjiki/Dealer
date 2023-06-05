using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Constants.Inventory;

public class ListItem_Drug : ListItem
{
    [Header("UI Elements")]

    [SerializeField] private TextMeshProUGUI Text_Name;

    [SerializeField] private TextMeshProUGUI Text_Quantity_L;

    [SerializeField] private TextMeshProUGUI Text_Quantity_R;

    [SerializeField] private Slider Slider_Quantity;

    [SerializeField] private Image Image_Icon;

    public void Setup(KeyValuePair<Drugs.ID, int> pair)
    {
        Text_Name.text = pair.Key.ToString();
        Text_Quantity_L.text = 0 + "";
        Text_Quantity_R.text = pair.Value.ToString();

        Sprite sprite = Instantiate<Sprite>(Resources.Load<Sprite>(Drugs.GetIconResourcePathByID(pair.Key)));

        Image_Icon.sprite = sprite;
    }
}
