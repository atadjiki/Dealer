using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Constants.Inventory;

public class DrugContainerViewItem : MonoBehaviour
{
    [Header("UI Elements")]

    [SerializeField] private TextMeshProUGUI Text_Name;

    [SerializeField] private TextMeshProUGUI Text_Quantity_L;

    [SerializeField] private TextMeshProUGUI Text_Quantity_R;

    [SerializeField] private Slider Slider_Quantity;

    [SerializeField] private Image Image_Icon;

    private Drugs.ID drugID;

    private float lastValue;

    public void Setup(DrugInventoryData data)
    {
        Sprite sprite = Instantiate<Sprite>(Resources.Load<Sprite>(Drugs.GetIconResourcePathByID(Drugs.ID.Cocaine)));

        Image_Icon.sprite = sprite;

        Text_Name.text = data.ID.ToString();

        drugID = data.ID;

        Text_Quantity_L.text = data.Quantity_Bag.ToString();
        Text_Quantity_R.text = data.Quantity_Stash.ToString();

        Slider_Quantity.minValue = 0;
        Slider_Quantity.maxValue = data.Quantity_Stash;
        Slider_Quantity.value = data.Quantity_Stash - data.Quantity_Bag;

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
                GameState.Instance.AddToBag(drugID, Mathf.Abs(difference));
            }
        }
        else
        {
            GameState.Instance.RemoveFromBag(drugID, Mathf.Abs(difference));
        }

        RefreshPanel();
    }

    private void RefreshPanel()
    {
        if(GameState.Instance != null)
        {

            foreach(DrugInventoryData data in GameState.Instance.GetInventory())
            {
                if(data.ID == drugID)
                {
                    //update the quantity labels
                    Text_Quantity_L.text = data.Quantity_Bag.ToString();
                    Text_Quantity_R.text = data.Quantity_Stash.ToString();

                    Slider_Quantity.minValue = 0;
                    Slider_Quantity.maxValue = Mathf.Abs(data.Quantity_Stash - data.Quantity_Bag);

                    lastValue = Slider_Quantity.value;
                }
            }
        }
    }
}
