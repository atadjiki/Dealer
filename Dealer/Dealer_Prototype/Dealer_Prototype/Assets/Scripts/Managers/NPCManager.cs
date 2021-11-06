using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    private static NPCManager _instance;

    public static NPCManager Instance { get { return _instance; } }

    public List<NPCController> Characters;
    private int _popCap = 5;

    private int _updateEveryFrames = 120;
    private int _currentFrames = 0;

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
        Characters = new List<NPCController>();
    }

    public bool RegisterNPC(NPCController npc)
    {
        if(Characters.Count == _popCap)
        {
            if (DebugManager.Instance.LogNPCManager) Debug.Log("Could not register NPC, reached pop cap");
            return false;
        }

        Characters.Add(npc);
        if (DebugManager.Instance.LogNPCManager) Debug.Log("Registered NPC " + npc.name);
        return true;
    }

    public void UnRegisterNPC(NPCController npc)
    {
        if (DebugManager.Instance.LogNPCManager) Debug.Log("Unregistered NPC " + npc.name);
        Characters.Remove(npc);
    }

    private void Update()
    {
        if(_currentFrames >= _updateEveryFrames)
        {
            _currentFrames = 0;
            BehaviorUpdate();
        }
        else
        {
            _currentFrames++;
        }
       
    }

    private void BehaviorUpdate()
    {

        //if (DebugManager.Instance.LogNPCManager) Debug.Log("NPC Manager - Behavior Update");

        foreach (NPCController npc in Characters)
        {

            if(npc.updateState == CharacterConstants.UpdateState.Ready)
            {
                if (npc.BehaviorMode == CharacterConstants.Behavior.Wander)
                {
                    if (npc.GetLastAction() == CharacterConstants.ActionType.Idle)
                    {
                        if (DebugManager.Instance.LogCharacter) Debug.Log(this.gameObject.name + " - Behavior - MoveToRandomPoint");
                        npc.PerformAction(CharacterConstants.ActionType.Move);

                    }
                    else if (npc.GetLastAction() == CharacterConstants.ActionType.Move || npc.GetLastAction() == CharacterConstants.ActionType.None)
                    {
                        if (DebugManager.Instance.LogCharacter) Debug.Log(this.gameObject.name + " - Behavior - Idle");
                        npc.PerformAction(CharacterConstants.ActionType.Idle);
                    }

                }
                else if (npc.BehaviorMode == CharacterConstants.Behavior.Stationary)
                {
                    if (DebugManager.Instance.LogCharacter) Debug.Log(this.gameObject.name + " - Behavior - Idle");
                    npc.PerformAction(CharacterConstants.ActionType.Idle);
                }
            }
        }
        
    }

}
