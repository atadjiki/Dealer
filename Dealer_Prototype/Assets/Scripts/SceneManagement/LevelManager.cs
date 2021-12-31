using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject LoadScreen;
    [SerializeField] private TextMeshProUGUI LoadScreen_Text;
    [SerializeField] private TextMeshProUGUI LoadScreen_LevelName;

    public enum LevelName { RootLevel, StartMenu, Apartment };

    private enum State { Busy, None };

    private State _state = State.None;

    public static int BuildIndexFromLevelName(LevelName Name)
    {
        switch(Name)
        {
            case LevelName.RootLevel:
                return 0;
            case LevelName.StartMenu:
                return 1;
            case LevelName.Apartment:
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

        LoadLevel(LevelName.StartMenu);
    }

    public void LoadLevel(LevelName LevelName)
    {
        if(_state == State.None)
        {
            StartCoroutine(DoLoadLevel(LevelManager.BuildIndexFromLevelName(LevelName)));
        }
    }

    private IEnumerator DoLoadLevel(int buildIndex)
    {
        float loadInterval = 0.5f;
        //
        _state = State.Busy;
        GameState.Instance.ToState(GameState.State.Loading);
        LoadScreen.SetActive(true);
        LoadScreen_LevelName.text = "";
        LoadScreen_Text.text = "loading.";
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
        //
        LoadScreen_Text.text = "loading...";
        yield return new WaitForSeconds(loadInterval);
        LoadScreen_LevelName.text = SceneManager.GetActiveScene().name;
        LoadScreen.SetActive(false);
        _state = State.None;
        //
    }
    
}
