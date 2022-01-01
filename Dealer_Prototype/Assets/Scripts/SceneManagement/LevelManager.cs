using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Constants;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject LoadScreen;
    [SerializeField] private TextMeshProUGUI LoadScreen_Text;
    [SerializeField] private TextMeshProUGUI LoadScreen_LevelName;

    private enum State { Busy, None };

    private State _state = State.None;

    public static int BuildIndexFromLevelName(Constants.LevelDataConstants.LevelName Name)
    {
        switch(Name)
        {
            case LevelDataConstants.LevelName.RootLevel:
                return 0;
            case LevelDataConstants.LevelName.StartMenu:
                return 1;
            case LevelDataConstants.LevelName.Apartment:
                return 2;
        }

        return 0;
    }

    /// ////
    ///

    private static LevelManager _instance;

    public static LevelManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        Build();
    }

    private void Build()
    {
        LoadScreen_LevelName.text = SceneManager.GetActiveScene().name;
    }

    public void LoadLevel(LevelData levelData)
    {
        if(_state == State.None)
        {
            Debug.Log("loading level " + levelData.Name);
            StartCoroutine(DoLoadLevel(levelData));
        }
        else
        {
            Debug.Log("Level manager is busy");
        }

    }

    private IEnumerator DoLoadLevel(LevelData levelData)
    {
        int buildIndex = LevelManager.BuildIndexFromLevelName(levelData.Name);

        float loadInterval = 0.5f;
        //
        _state = State.Busy;
        GameState.Instance.ToState(GameState.State.Loading);
        LoadScreen.SetActive(true);
        LoadScreen_LevelName.text = "";
        LoadScreen_Text.text = "loading.";
        //
        yield return new WaitForSeconds(loadInterval);
        //
        //load level
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(buildIndex);
        loadOperation.allowSceneActivation = false;
        LoadScreen_Text.text = "loading..";
        while (loadOperation.progress < 0.9f)
        {
            yield return new WaitForSeconds(loadInterval);
        }
        loadOperation.allowSceneActivation = true;
        //
        yield return new WaitForSeconds(loadInterval);
        LoadScreen_Text.text = "loading...";
        //
        //load start menu
        //
        if (levelData.Type == LevelDataConstants.LevelType.GameLevel)
        {
            PrefabFactory.CreatePrefab(Constants.RegistryID.PerLevel_Managers, null);
            yield return new WaitForSeconds(2.0f);
        }

        //
        yield return new WaitForSeconds(loadInterval);
        //
        LoadScreen_Text.text = "loading....";
        yield return new WaitForSeconds(loadInterval);

        if (levelData.Type == LevelDataConstants.LevelType.Menu)
        {
            GameState.Instance.ToState(GameState.State.MainMenu);
        }
        else if(levelData.Type == LevelDataConstants.LevelType.GameLevel)
        {
            GameState.Instance.ToState(GameState.State.GamePlay);
        }

        LoadScreen_LevelName.text = SceneManager.GetActiveScene().name;
        LoadScreen.SetActive(false);
        _state = State.None;
        //
    }
    
}
