using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ListItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Colors")]
    [SerializeField] protected Color NormalColor;
    [SerializeField] protected Color HighlightedColor;
    [SerializeField] protected Color ClickedColor;

    [SerializeField] private Image Image_Tint;

    private void Awake()
    {
        Image_Tint.color = NormalColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Image_Tint.color = HighlightedColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Image_Tint.color = NormalColor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Image_Tint.color = NormalColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Image_Tint.color = ClickedColor;

        HandleClick();
    }

    protected virtual void HandleClick()
    {

    }
}
