using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Constants;
using GameDelegates;
using TMPro;

public class StationCanvas : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Text_StationName;

    [SerializeField] private Button Button_Station;

    public void Setup(Enumerations.SafehouseStation station)
    {
        Text_StationName.text = DisplayText.Get(station);

        Button_Station.onClick.AddListener(delegate () { Global.OnStationSelected.Invoke(station); });
    }
}
