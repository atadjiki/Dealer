using System.Collections;
using System.Collections.Generic;
using TMPro;
using Constants;
using UnityEngine.UI;
using UnityEngine;
using GameDelegates;

public class SafehouseCanvas : MonoBehaviour
{
    [SerializeField] private Button Button_Phone;
    [SerializeField] private Button Button_Save;
    [SerializeField] private Button Button_Stash;
    [SerializeField] private Button Button_Door;

    public static SafehouseStationSelected OnStationSelected;

    private void Awake()
    {
        Button_Phone.onClick.AddListener(   delegate () { OnClicked(Enumerations.SafehouseStation.Phone);   });
        Button_Save.onClick.AddListener(    delegate () { OnClicked(Enumerations.SafehouseStation.Save);    });
        Button_Stash.onClick.AddListener(   delegate () { OnClicked(Enumerations.SafehouseStation.Stash);   });
        Button_Door.onClick.AddListener(    delegate () { OnClicked(Enumerations.SafehouseStation.Door);    });

    }

    public void OnClicked(Enumerations.SafehouseStation station)
    {
        if(OnStationSelected != null)
        {
            OnStationSelected.Invoke(station);
        }
    }
}
