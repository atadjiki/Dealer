using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ListItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected TextMeshProUGUI Text;

    [Header("Colors")]
    [SerializeField] protected Color NormalColor;
    [SerializeField] protected Color HighlightedColor;

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
