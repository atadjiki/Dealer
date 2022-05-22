using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class BasicCharacterManager : MonoBehaviour
{
    private static BasicCharacterManager _instance;

    public static BasicCharacterManager Instance { get { return _instance; } }

    private List<BasicCharacter> Characters;
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

    private void Build()
    {
        Characters = new List<BasicCharacter>();
    }

    public bool Register(BasicCharacter Character)
    {
        if (Character != null)
        {

            if (Characters.Count == _popCap)
            {
                DebugManager.Instance.Print(DebugManager.Log.LogNPCManager, "Could not register NPC, reached pop cap");
                return false;
            }

            Characters.Add(Character);

            DebugManager.Instance.Print(DebugManager.Log.LogNPCManager, "Registered NPC " + Character.GetID());

            if (PartyPanelList.Instance)
            {
                PartyPanelList.Instance.RegisterCharacter(Character);
            }
            if (NPCPanel.Instance)
            {
                NPCPanel.Instance.RegisterCharacter(Character);
            }

            return true;
        }

        return false;
    }

    public void UnRegister(BasicCharacter Character)
    {
        if (PartyPanelList.Instance)
        {
            PartyPanelList.Instance.UnRegisterCharacter(Character);
        }
        if (NPCPanel.Instance)
        {
            NPCPanel.Instance.UnRegisterCharacter(Character);
        }

        NPCComponent npc = Character.GetComponent<NPCComponent>();

        if (npc != null)
        {
            DebugManager.Instance.Print(DebugManager.Log.LogNPCManager, "Unregistered NPC " + npc.GetID());
            Characters.Remove(Character);
        }
    }
}
