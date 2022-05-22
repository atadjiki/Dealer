using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    private static PartyManager _instance;

    public static PartyManager Instance { get { return _instance; } }

    private List<CharacterInfo> Party;
    private const int _popCap = 4;

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

    public List<CharacterInfo> GetParty()
    {
        return Party;
    }

    private void Build()
    {
        Party = new List<CharacterInfo>();

        GameStateManager.Instance.onLevelStart += OnLevelStart;
    }

    private void OnLevelStart()
    {
        Debug.Log(this.name + " - received on level start");
        InitializeCharacters(GameStateManager.Instance.state);
    }

    private void InitializeCharacters(GameState state)
    {
        Party = new List<CharacterInfo>();

        foreach(CharacterInfo info in state.partyInfo)
        {
            Register(info);
            SpawnPoint.Instance.AttemptSpawn(info);
        }
    }

    public void OnStateChanged(GameState state)
    {

    }

    public bool Register(CharacterInfo characterInfo)
    {
        if (Party.Count == _popCap)
        {
            DebugManager.Instance.Print(DebugManager.Log.LogNPCManager, "Could not register NPC, reached pop cap");
            return false;
        }

        Party.Add(characterInfo);

        DebugManager.Instance.Print(DebugManager.Log.LogNPCManager, "Registered NPC " + characterInfo.ID);

        if (PartyPanelList.Instance)
        {
            PartyPanelList.Instance.UpdateList();
        }
        //if (CharacterPanel.Instance)
        //{
        //    CharacterPanel.Instance.RegisterCharacter(characterInfo);
        //}

        return true;
    }

    public void UnRegister(CharacterInfo Character)
    {
        Party.Remove(Character);

        if (PartyPanelList.Instance)
        {
            PartyPanelList.Instance.UpdateList();
        }
        //if (CharacterPanel.Instance)
        //{
        //    CharacterPanel.Instance.UnRegisterCharacter(Character);
        //}

     

    }
}
