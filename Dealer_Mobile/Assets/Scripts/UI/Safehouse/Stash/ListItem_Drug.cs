using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Constants.Inventory;

public class ListItem_Drug : MonoBehaviour
{
    [Header("UI Elements")]

    [SerializeField] private TextMeshProUGUI Text_Name;

    [SerializeField] private TextMeshProUGUI Text_Quantity;

    [SerializeField] private Image Image_Icon;

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
        Text_Quantity.text = pair.Value.ToString();

        Sprite sprite = Instantiate<Sprite>(Resources.Load<Sprite>(Drugs.GetIconResourcePathByID(pair.Key)));

        Image_Icon.sprite = sprite;

        if (_itemButton != null)
        {
            _itemButton.onClick.AddListener(() => OnItemClicked());
        }
    }

    public void OnItemClicked()
    {
        Debug.Log("Item clicked " + _ID.ToString());
    }
}
