using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelUtility : MonoBehaviour
{
    public enum PlayerLocation { Safehouse, City };

    private static string Name_Loading = "Loading";

    public static void GoToLoading(PlayerLocation location)
    {
        GameState.location = location;
        SceneManager.LoadSceneAsync(Name_Loading, LoadSceneMode.Single);
    }

    public static AsyncOperation GoToLocation(PlayerLocation location)
    {
        return SceneManager.LoadSceneAsync(location.ToString(), LoadSceneMode.Single);
    }
}
