using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Constants;
using GameDelegates;
using UnityEngine.EventSystems;

public class StationCanvas : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject Text_Tooltip;

    [SerializeField] private Button Button_Station;

    private void Awake()
    {
        Text_Tooltip.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Text_Tooltip.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Text_Tooltip.SetActive(false);
    }

    public void Setup(Enumerations.SafehouseStation station)
    {
        Button_Station.onClick.AddListener(delegate () { Global.OnStationSelected.Invoke(station); });
    }

}
