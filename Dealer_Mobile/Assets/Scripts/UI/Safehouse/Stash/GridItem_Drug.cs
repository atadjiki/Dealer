using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using Constants;
using static Constants.Inventory;

[RequireComponent(typeof(Button))]
public class GridItem_Drug : MonoBehaviour
{
    [Header("UI Elements")]

    [SerializeField] private TextMeshProUGUI Text_Name;
    //[SerializeField] private TextMeshProUGUI Text_Grade;
    [SerializeField] private TextMeshProUGUI Text_Quantity;

    [SerializeField] private GameObject Panel_Icon;

    [SerializeField] private Image Image_Icon;
    //[SerializeField] private Image Image_Tint;

    private string _ID;

    private Button _itemButton;

    private void Awake()
    {
        _itemButton = GetComponent<Button>();
    }

    public void Setup(KeyValuePair<Drugs.ID, int> pair)
    {
        _ID = pair.Key.ToString();

        Text_Name.text = _ID;
        //Text_Grade.text = drugItem.Grade.ToString();
        Text_Quantity.text = pair.Value.ToString();

        Panel_Icon.SetActive(true);

        //Image_Tint.color = ColorHelper.FromGrade(drugItem.Grade);

        if(_itemButton != null)
        {
            _itemButton.onClick.AddListener(() => OnItemClicked());
        }
    }

    public void OnItemClicked()
    {
        Debug.Log("Item clicked " + _ID.ToString());
    }

}
