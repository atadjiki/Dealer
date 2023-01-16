using System.Collections;
using System.Collections.Generic;
using Constants;
using GameDelegates;
using UnityEngine;

public class Environment_Safehouse : EnvironmentComponent
{
    [Header("Player Spawn")]
    [SerializeField] protected PlayerSpawner playerSpawner;
    [SerializeField] protected Transform walkToLocation;
    protected PlayerComponent _player;

    [Space]
    [Header("Safehouse")]
    [SerializeField] private List<PlayerStation> _stations;

    private void Start()
    {
        Global.OnStationSelected += OnStationSelected;
        Global.OnPlayerSpawned += OnPlayerSpawned;

        AudioUtility.DoorOpen();
    }

    protected override IEnumerator Coroutine_PerformEnterActions()
    {
        yield return base.Coroutine_PerformEnterActions();

        SpawnPlayer();

    }

    protected override void ExitActions()
    {
        base.ExitActions();

        Global.OnStationSelected -= OnStationSelected;
        Global.OnPlayerSpawned -= OnPlayerSpawned;
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

    private void SpawnPlayer()
    {
        if (playerSpawner != null)
        {
            playerSpawner.PerformSpawn();
        }
    }

    private void OnPlayerSpawned(PlayerComponent playerComponent)
    {
        _player = playerComponent;

        StartCoroutine(PerformEntranceScene());
    }

    private void OnPlayerDestinationReached()
    {
        Global.OnToggleUI(true);
    }

    private IEnumerator PerformEntranceScene()
    {
        yield return new WaitUntil(() => _player.HasInitialized());

        _player.OnDestinationReached += OnPlayerDestinationReached;

        _player.GoTo(walkToLocation.position);

        yield return new WaitForSeconds(0.5f);

        musicSource.Play();
    }
}
