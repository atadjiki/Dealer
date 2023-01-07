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
        SceneManager.LoadScene(Name_Loading, LoadSceneMode.Single);
    }

    public static void GoToLocation(PlayerLocation location)
    {
        SceneManager.LoadScene(location.ToString(), LoadSceneMode.Single);
    }
}
