using System.Collections;
using System.Collections.Generic;
using Constants;
using GameDelegates;
using UnityEngine;

public class Environment_Safehouse : EnvironmentComponent
{
    private PlayerComponent _player;

    [SerializeField] Transform entrace_WalkTo_Location;

    [SerializeField] private List<PlayerStation> _stations;

    private void Start()
    {
        Global.OnStationSelected += OnStationSelected;
    }

    protected override void OnPlayerSpanwed(PlayerComponent playerComponent)
    {
        base.OnPlayerSpanwed(playerComponent);

        _player = playerComponent;

        StartCoroutine(PerformEntranceScene());

    }

    public void OnPlayerDestinationReached()
    {
        ToggleStationUI(true);
    }

    private IEnumerator PerformEntranceScene()
    {
        yield return new WaitUntil( () => _player.HasInitialized());

        _player.OnDestinationReached += OnPlayerDestinationReached;

        _player.GoTo(entrace_WalkTo_Location.position);
    }

    private void OnStationSelected(Enumerations.SafehouseStation stationID)
    {
        Debug.Log("station selected " + stationID.ToString());

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
        ToggleStationUI(false);
        transitionPanel.Toggle(true, 0.25f);

        yield return new WaitForSeconds(0.25f);

        _player.Teleport(location);

        if (station == Enumerations.SafehouseStation.Door)
        {
            ToggleStationUI(false);
            GameObject cityMapObject = Instantiate<GameObject>(PrefabLibrary.GetCityMapCanvas(), this.transform);
            CityMapCanvas cityMapCanvas = cityMapObject.GetComponent<CityMapCanvas>();
            cityMapCanvas.OnBackButtonPressed += OnBackButtonPressed;
        }
        else
        {
            ToggleStationUI(true);
        }

        transitionPanel.Toggle(false, 1);

        yield return null;
    }

    private void OnBackButtonPressed()
    {
        StartCoroutine(PerformStationSelect(Enumerations.SafehouseStation.None, entrace_WalkTo_Location));
    }

    public void ToggleStationUI(bool flag)
    {
        foreach(PlayerStation station in _stations)
        {
            station.ToggleUI(flag);
        }
    }
}
