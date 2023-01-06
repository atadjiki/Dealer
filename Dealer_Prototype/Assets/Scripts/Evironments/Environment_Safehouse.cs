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

    protected override void OnPlayerSpanwed(PlayerComponent playerComponent)
    {
        base.OnPlayerSpanwed(playerComponent);

        _player = playerComponent;

        StartCoroutine(PerformEntranceScene());

    }

    public void OnPlayerDestinationReached()
    {
        //TODO:: tell stations to show their canvases and buttons if they have them
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
        //TODO:: tell stations to hide canvases
        transitionPanel.Toggle(true, 0.25f);

        yield return new WaitForSeconds(0.25f);

        _player.Teleport(location);

        if (station == Enumerations.SafehouseStation.Door)
        {
            //TODO:: tell stations to hide canvases
            GameObject cityMapObject = Instantiate<GameObject>(PrefabLibrary.GetCityMapCanvas(), this.transform);
            CityMapCanvas cityMapCanvas = cityMapObject.GetComponent<CityMapCanvas>();
            cityMapCanvas.OnBackButtonPressed += OnBackButtonPressed;
        }
        else
        {
            //TODO:: tell stations to show canvases
        }

        transitionPanel.Toggle(false, 1);

        yield return null;
    }

    private void OnBackButtonPressed()
    {
        StartCoroutine(PerformStationSelect(Enumerations.SafehouseStation.None, entrace_WalkTo_Location));
    }
}
