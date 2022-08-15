using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentComponent : MonoBehaviour
{
    [SerializeField] private bool debug = false;
    [SerializeField] private Constants.Enumerations.Environment environmentID;

    [SerializeField] private List<SpawnLocation> spawnLocations;

    [SerializeField] private CameraRig cameraRig;

    private void Start()
    {
        StartCoroutine(Coroutine_EnterActionsStart());
    }

    private void OnDestroy()
    {
        ExitActions();
    }

    protected virtual IEnumerator Coroutine_EnterActionsStart()
    {
        if (debug) Debug.Log("Environment " + this.name + " - enter actions");

        yield return Coroutine_PerformEnterActions();
    }

    protected virtual IEnumerator Coroutine_PerformEnterActions()
    {
        LevelManager.Instance.RegisterScene(Constants.Enumerations.SceneType.Environment, Constants.Enumerations.GetSceneNameFromEnvironmentID(environmentID));
        GameStateManager.Instance.SetEnvironment(environmentID);

        GameStateManager.Instance.ToGameplay();

        SpawnPlayer();

        yield return Coroutine_EnterActionsCompleted();
    }

    protected virtual IEnumerator Coroutine_EnterActionsCompleted()
    {
        yield return null;
    }

    protected virtual void ExitActions()
    {
        GameStateManager.Instance.SetEnvironment(Constants.Enumerations.Environment.None);
        if (debug) Debug.Log("Environment " + this.name + " - exit actions");
    }

    protected virtual void SpawnPlayer()
    {
        if (spawnLocations.Count > 0)
        {
            bool success;
            CharacterManager.CharacterData data = CharacterManager.Instance.GetCharacterData(Constants.Enumerations.CharacterID.Player, out success);

            if(success)
            {
                SpawnLocation spawnLocation = spawnLocations[Random.Range(0, spawnLocations.Count)];

                GameObject characterObject = Instantiate<GameObject>(data.Prefab, spawnLocation.transform);
            }
        }
    }
}

