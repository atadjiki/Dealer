using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    private static DebugManager _instance;

    public static DebugManager Instance { get { return _instance; } }

    public bool LogCharacter = true;
    public bool LogInput = false;
    public bool LogAStar = false;
    public bool LogNPCManager = true;

    public AstarPath _astarPath;

    public GameObject debugPanel;
    public GameObject npcPanel;
    public TMPro.TextMeshProUGUI nameText;
    public TextMeshProUGUI stateText;

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
        if(_astarPath != null && LogAStar)
        {
            _astarPath.logPathResults = PathLog.Normal;
        }
    }

    private void Update()
    {
        if(NPCManager.Instance.IsNPCCurrentlySelected())
        {
            npcPanel.SetActive(true);
            nameText.SetText(NPCManager.Instance.GetSelectedNPC().name);
            stateText.SetText(NPCManager.Instance.GetSelectedNPC().CurrentState.ToString());
        }
        else
        {
            npcPanel.SetActive(false);
        }
    }
}
