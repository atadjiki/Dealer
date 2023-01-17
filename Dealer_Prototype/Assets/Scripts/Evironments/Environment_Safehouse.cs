using System.Collections;
using System.Collections.Generic;
using Constants;
using GameDelegates;
using UnityEngine;

public class Environment_Safehouse : EnvironmentComponent
{
    [Header("Player Spawn")]
    [SerializeField] protected PlayerSpawner playerSpawner;
    protected PlayerComponent _player;

    [Space]
    [Header("Safehouse")]
    [SerializeField] private List<PlayerStation> _stations;

    [Space]
    [Header("Cutscene")]
    [SerializeField] private GameObject entryCutscene;

    private void Start()
    {
        Global.OnStationSelected += OnStationSelected;
        Global.OnPlayerSpawned += OnPlayerSpawned;

        AudioUtility.DoorOpen();
    }

    protected override IEnumerator Coroutine_PerformEnterActions()
    {
        yield return base.Coroutine_PerformEnterActions();

        cutscenePlayer.PlayCutscene(entryCutscene, EntryCutsceneComplete);
    }

    private void EntryCutsceneComplete()
    {
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
        StartCoroutine(PerformStationSelect(Enumerations.SafehouseStation.None, playerSpawner.transform));
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
    }
}
