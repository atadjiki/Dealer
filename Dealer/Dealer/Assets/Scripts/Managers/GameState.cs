using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour
{
    private static GameState _instance;

    public static GameState Instance { get { return _instance; } }

    public enum Objectives { CompletedSitDown, TalkedToStreetUrchin, ReceivedBadge }

    private HashSet<Objectives> Completed;

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
        Completed = new HashSet<Objectives>();

        DontDestroyOnLoad(this.gameObject);
    }

    public void ObjectiveCompleted(Objectives objective)
    {
        Completed.Add(objective);
    }

    public bool IsObjectiveCompleted(Objectives objective)
    {
        return Completed.Contains(objective);
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        else if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
    }

}
