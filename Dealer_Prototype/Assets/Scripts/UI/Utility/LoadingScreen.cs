using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDelegates;
using static LevelUtility;

public class LoadingScreen : MonoBehaviour
{
    private void Awake()
    {
        Global.OnLevelHasLoaded += OnLevelHasLoaded;

        StartCoroutine(LoadLevel());
    }

    private IEnumerator LoadLevel()
    {
        PlayerLocation location = GameState.location;

        AsyncOperation asyncOperation = GoToLocation(location);

        yield return new WaitWhile( () => !asyncOperation.isDone );
    }

    private void OnLevelHasLoaded()
    {
        Destroy(this.gameObject);
    }
}
