using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using Constants;

[RequireComponent(typeof(Button))]
public class GridItem_Drug : MonoBehaviour
{
    [Header("UI Elements")]

    [SerializeField] private TextMeshProUGUI Text_Name;
    [SerializeField] private TextMeshProUGUI Text_Grade;
    [SerializeField] private TextMeshProUGUI Text_Quantity;

    [SerializeField] private GameObject Panel_Icon;

    [SerializeField] private Image Image_Icon;
    [SerializeField] private Image Image_Tint;

    private Button _itemButton;
    private DrugItem _drugItem;

    private void Awake()
    {
        _itemButton = GetComponent<Button>();
    }

    public void Setup(DrugItem drugItem)
    {
        _drugItem = drugItem;

        Text_Name.text = Constants.Inventory.Drugs.Format(drugItem.ID);
        Text_Grade.text = drugItem.Grade.ToString();
        Text_Quantity.text = "" + drugItem.Quantity;

        Panel_Icon.SetActive(true);

        Image_Tint.color = ColorHelper.FromGrade(drugItem.Grade);

        if(_itemButton != null)
        {
            _itemButton.onClick.AddListener(() => OnItemClicked());
        }
    }

    public void OnItemClicked()
    {
        Debug.Log("Item clicked " + _drugItem.ID.ToString());
    }

}
