using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;

    public static LevelManager Instance { get { return _instance; } }

    public enum Level { OutsideYard, ShadyBar, SideStreet, BoxingGym };

    public Level CurrentLevel;
    private HashSet<Level> LoadedLevels;

    private bool Loading = false;

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
        LoadedLevels = new HashSet<Level>();

        DontDestroyOnLoad(this.gameObject);
    }

    public bool isLoading()
    {
        return Loading;
    }

    public void AddCurrentLevel()
    {
        if(Loading == false)
        {
            LoadLevelAdditive(CurrentLevel);
        }
    }

    public void SwitchToCurrentLevel()
    {
        if (Loading == false)
        {
            LoadLevelSingular(CurrentLevel);
        }
    }

    //switch to a the desired level, but unload everything else
    public void LoadLevelSingular(Level Level)
    {
        if (Loading == false)
        {
            if(LoadedLevels.Contains(Level) == false)
            {
                StartCoroutine(DoLoadLevel(Level, LoadSceneMode.Additive));

                foreach (Level toUnload in LoadedLevels)
                {
                    SceneManager.UnloadSceneAsync(EnumToString(toUnload));
                }
                LoadedLevels.Clear();
                LoadedLevels.Add(Level);
                Debug.Log(ListLoadedLevels());
            }
            else
            {
                foreach (Level toUnload in LoadedLevels)
                {
                    if(toUnload != Level)
                    {
                        SceneManager.UnloadSceneAsync(EnumToString(toUnload));
                    }
                }
                LoadedLevels.Clear();
                LoadedLevels.Add(Level);
                Debug.Log(ListLoadedLevels());
            }
            
        }
        else if (Loading)
        {
            Debug.Log("Level Manager is busy");
        }
    }

    //add the desired level to the pool of already loaded levels
    public void LoadLevelAdditive(Level Level)
    {
        if(Loading == false && LoadedLevels.Contains(Level) == false)
        {
            StartCoroutine(DoLoadLevel(Level, LoadSceneMode.Additive));
            LoadedLevels.Add(Level);
            Debug.Log(ListLoadedLevels());
        }
        else if(LoadedLevels.Contains(Level))
        {
            Debug.Log(Level + " already loaded");
        }
        else if(Loading)
        {
            Debug.Log("Level Manager is busy");
        }
    }

    IEnumerator DoLoadLevel(Level Level, LoadSceneMode Mode)
    {
        Loading = true;
        yield return null;
        //DeletePlayerInstance();
        SceneManager.LoadSceneAsync(EnumToString(Level), Mode);
        //SpawnPlayer();
        Loading = false;

    }

    public void DeletePlayerInstance()
    {
        if(PlayerController.Instance != null)
        {
            Destroy(PlayerController.Instance.gameObject);
        }
    }

    public void SpawnPlayer()
    {
        Instantiate<GameObject>(Resources.Load<GameObject>("Player"));
    }

    public string EnumToString(Level Level)
    {
        if (Level == Level.BoxingGym)
        {
            return "BoxingGym";
        }
        else if(Level == Level.OutsideYard)
        {
            return "OutsideYard";
        }
        else if(Level == Level.ShadyBar)
        {
            return "ShadyBar";
        }
        else if(Level == Level.SideStreet)
        {
            return "SideStreet";
        }

        return "";
    }

    public string ListLoadedLevels()
    {
        string result = "Levels currently loaded: ";

        if(LoadedLevels.Count > 0)
        {
            foreach (Level level in LoadedLevels)
            {
                result += level + ", ";
            }

            result = result.Substring(0, result.Length - 2);
        }
        else
        {
            result = "No levels loaded.";
        }

        return result;
    }
}
