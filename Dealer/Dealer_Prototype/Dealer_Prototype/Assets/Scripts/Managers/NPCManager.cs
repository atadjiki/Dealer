using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    private static NPCManager _instance;

    public static NPCManager Instance { get { return _instance; } }

    private List<NPCComponent> Characters;
    private int _popCap = 20;

    private int _updateEveryFrames = 120;
    private int _currentFrames = 0;

    private NPCComponent selectedNPC = null;

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
        Characters = new List<NPCComponent>();
    }

    public bool HasNotExceededPopCap()
    {
        return Characters.Count < _popCap;
    }

    public bool RegisterNPC(NPCComponent npc)
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

    public void UnRegisterNPC(NPCComponent npc)
    {
        if (DebugManager.Instance.LogNPCManager) Debug.Log("Unregistered NPC " + npc.name);
        Characters.Remove(npc);
    }

    private void Update()
    {
        if (_currentFrames >= _updateEveryFrames)
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
        foreach (NPCComponent npc in Characters)
        {

            if(npc.GetUpdateState() == CharacterConstants.UpdateState.Ready)
            {
                if (npc.GetCurrentBehavior() == CharacterConstants.Behavior.Wander)
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
                else if (npc.GetCurrentBehavior() == CharacterConstants.Behavior.Stationary)
                {
                    if (DebugManager.Instance.LogCharacter) Debug.Log(this.gameObject.name + " - Behavior - Idle");
                    npc.PerformAction(CharacterConstants.ActionType.Idle);
                }
                else if(npc.GetCurrentBehavior() == CharacterConstants.Behavior.Possesed)
                {
                    if (DebugManager.Instance.LogCharacter) Debug.Log(this.gameObject.name + " - Behavior - Possesed");
                }
            }
        }
        
    }

    public void HandleNPCSelection(NPCComponent NPC)
    {
        //if nobody is selected, register
        if(selectedNPC == null)
        {
            PossessNPC(NPC);
        }
        else if(selectedNPC != NPC)
        {
            UnpossessNPC();
            PossessNPC(NPC);
        }
        else
        {
            UnpossessNPC();
        }
    }

    private void PossessNPC(NPCComponent NPC)
    {
        if (DebugManager.Instance.LogNPCManager) Debug.Log("Selected " + NPC.GetID());
        selectedNPC = NPC;
        selectedNPC.PerformSelect();
    }

    private void UnpossessNPC()
    { 
        if (DebugManager.Instance.LogNPCManager) Debug.Log("Unselected " + selectedNPC.GetID());
        selectedNPC.PerformUnselect();
        selectedNPC = null;
    }

    public bool IsNPCCurrentlySelected()
    {
        return (selectedNPC != null);
    }

    public NPCComponent GetSelectedNPC()
    {
        return selectedNPC;
    }

    public void AttemptMoveOnPossesedNPC(Vector3 Location)
    {
        if (selectedNPC != null)
        {
            selectedNPC.MoveToLocation(Location);
        }
    }
}
