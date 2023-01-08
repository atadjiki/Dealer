using System.Collections;
using System.Collections.Generic;
using Constants;
using GameDelegates;
using UnityEngine;

public class Environment_Safehouse : EnvironmentComponent
{
    [SerializeField] private List<PlayerStation> _stations;

    private void Start()
    {
        Global.OnStationSelected += OnStationSelected;

        AudioUtility.DoorOpen();
    }

    protected override void OnPlayerSpawned(PlayerComponent playerComponent)
    {
        base.OnPlayerSpawned(playerComponent);

        _player = playerComponent;

        StartCoroutine(PerformEntranceScene());
    }

    public void OnPlayerDestinationReached()
    {
        Global.OnToggleUI(true);
    }

    private IEnumerator PerformEntranceScene()
    {
        yield return new WaitUntil( () => _player.HasInitialized());

        _player.OnDestinationReached += OnPlayerDestinationReached;

        _player.GoTo(entrace_WalkTo_Location.position);

        yield return new WaitForSeconds(0.5f);

        _musicSource.Play();
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
            cityMapCanvas.OnBackButtonPressed += OnBackButtonPressed;
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
        StartCoroutine(PerformStationSelect(Enumerations.SafehouseStation.None, entrace_WalkTo_Location));
    }
}
