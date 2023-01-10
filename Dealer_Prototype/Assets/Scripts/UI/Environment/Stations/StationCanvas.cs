using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Constants;
using GameDelegates;
using UnityEngine.EventSystems;

public class StationCanvas : MonoBehaviour
{
    [SerializeField] private Button Button_Station;

    public void Setup(Enumerations.SafehouseStation station)
    {
        Button_Station.onClick.AddListener(delegate ()
        {
            Global.OnStationSelected.Invoke(station);
            AudioUtility.ButtonClick();
        });
    }

}
