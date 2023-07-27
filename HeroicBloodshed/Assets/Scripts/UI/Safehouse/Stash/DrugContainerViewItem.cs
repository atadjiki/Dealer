using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Constants;

public class DrugContainerViewItem : MonoBehaviour
{
    [Header("UI Elements")]

    [SerializeField] private TextMeshProUGUI Text_Name;

    [SerializeField] private TextMeshProUGUI Text_Quantity_L;

    [SerializeField] private TextMeshProUGUI Text_Quantity_R;

    [SerializeField] private Slider Slider_Quantity;

    [SerializeField] private Image Image_Icon;

    private InventoryItemID _itemID;

    private float lastValue;

    public void Setup(InventoryItemID itemID)
    {
        Sprite sprite = Instantiate(Resources.Load<Sprite>(GetIconResourcePathByID(InventoryItemID.Cocaine)));

        Image_Icon.sprite = sprite;

        Text_Name.text = itemID.ToString();

        _itemID = itemID;

        if (GameState.Instance != null)
        {
            int quantity_bag = GameState.Instance.GetInventory(OwnerID.Player_Bag).GetQuantity(itemID);
            int quantity_stash = GameState.Instance.GetInventory(OwnerID.Player_Stash).GetQuantity(itemID);

            Text_Quantity_L.text = quantity_bag.ToString();
            Text_Quantity_R.text = quantity_stash.ToString();

            Slider_Quantity.minValue = 0;
            Slider_Quantity.maxValue = quantity_bag + quantity_stash;
            Slider_Quantity.value = quantity_stash;
        }

        Slider_Quantity.SetDirection(Slider.Direction.LeftToRight, true);

        lastValue = Slider_Quantity.value;

        Slider_Quantity.onValueChanged.AddListener(delegate { OnSliderValueChanged(); });
    }

    private void OnSliderValueChanged()
    {
        int difference = (int) (lastValue - Slider_Quantity.value);

        if(difference > 0)
        {
            if(GameState.Instance != null)
            {
                GameState.Instance.AddToBag(_itemID, Mathf.Abs(difference));
            }
        }
        else
        {
            GameState.Instance.RemoveFromBag(_itemID, Mathf.Abs(difference));
        }

        RefreshPanel();
    }

    private void RefreshPanel()
    {
        if(GameState.Instance != null)
        {
            int quantity_bag = GameState.Instance.GetInventory(OwnerID.Player_Bag).GetQuantity(_itemID);
            int quantity_stash = GameState.Instance.GetInventory(OwnerID.Player_Stash).GetQuantity(_itemID);

            //update the quantity labels
            Text_Quantity_L.text = quantity_bag.ToString();
            Text_Quantity_R.text = quantity_stash.ToString();

            lastValue = Slider_Quantity.value;
        }
    }
}
