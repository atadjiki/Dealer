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
            case LevelName.StartMenu:
                return 0;
            case LevelName.Apartment:
                return 1;
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
        DontDestroyOnLoad(this.gameObject);
        LoadScreen_LevelName.text = SceneManager.GetActiveScene().name;
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
        //
        _state = State.Busy;
        LoadScreen.SetActive(true);
        LoadScreen_LevelName.text = "";
        LoadScreen_Text.text = "loading.";
        yield return new WaitForSeconds(1.0f);
        //

        //load level
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(buildIndex);
        loadOperation.allowSceneActivation = false;
        LoadScreen_Text.text = "loading..";
        while (loadOperation.progress < 0.9f)
        {
            yield return new WaitForSeconds(1.0f);
        }
        loadOperation.allowSceneActivation = true;
        //
        //
        LoadScreen_Text.text = "loading...";
        yield return new WaitForSeconds(1.0f);
        LoadScreen_LevelName.text = SceneManager.GetActiveScene().name;
        LoadScreen.SetActive(false);
        _state = State.None;
        //
    }
    
}
