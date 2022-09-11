using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class UIButton_RosterPicker_Inactive : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Button _button;
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _text;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _text.text = "(click to edit)";
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _text.text = string.Empty;
    }
}
