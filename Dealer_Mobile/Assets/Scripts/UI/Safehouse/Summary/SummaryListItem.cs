using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SummaryListItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{ 
    [SerializeField] private TextMeshProUGUI Text;

    [Header("Colors")]
    [SerializeField] private Color NormalColor;
    [SerializeField] private Color HighlightedColor;

    private Image itemImage;

    private void Awake()
    {
        itemImage = GetComponent<Image>();

        itemImage.color = NormalColor;
    }

    public void SetText(string text)
    {
        Text.text = text;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        itemImage.color = HighlightedColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        itemImage.color = NormalColor;
    }
}
