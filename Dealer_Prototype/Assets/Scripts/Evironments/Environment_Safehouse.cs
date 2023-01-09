using System.Collections;
using System.Collections.Generic;
using Constants;
using GameDelegates;
using UnityEngine;

public class Environment_Safehouse : EnvironmentComponent
{
    [Header("Safehouse")]
    [SerializeField] private List<PlayerStation> _stations;

    private void Start()
    {
        Global.OnStationSelected += OnStationSelected;

        AudioUtility.DoorOpen();
    }

    protected override void ExitActions()
    {
        base.ExitActions();

        Global.OnStationSelected -= OnStationSelected;
    }

    private void OnStationSelected(Enumerations.SafehouseStation stationID)
    {
        foreach (PlayerStation station in _stations)
        {
            if (station.GetStationID() == stationID)
            {
                StartCoroutine(PerformStationSelect(stationID, station.GetEntryTransform()));
            }
        }
    }

    private IEnumerator PerformStationSelect(Enumerations.SafehouseStation station, Transform location)
    {
        Global.OnToggleUI(false);
        TransitionPanel transitionPanel = UIUtility.RequestFadeToBlack(0.25f);
        Global.OnToggleUI(false);

        yield return new WaitForSeconds(0.25f);

        _player.Teleport(location);

        if (station == Enumerations.SafehouseStation.Door)
        {
            GameObject cityMapObject = Instantiate<GameObject>(PrefabLibrary.GetCityMapCanvas(), this.transform);
            CityMapCanvas cityMapCanvas = cityMapObject.GetComponent<CityMapCanvas>();
            cityMapCanvas.Setup(OnBackButtonPressed);
        }
        else
        {
            Global.OnToggleUI(true);
        }

        transitionPanel.ToggleAndDestroy(false, 1);

        yield return null;
    }

    private void OnBackButtonPressed()
    {
        StartCoroutine(PerformStationSelect(Enumerations.SafehouseStation.None, walkToLocation));
    }
}
