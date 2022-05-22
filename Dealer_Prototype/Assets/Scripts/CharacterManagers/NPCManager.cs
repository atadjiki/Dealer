using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class NPCManager : CharacterManager
{
    private static NPCManager _instance;

    public static NPCManager Instance { get { return _instance; } }

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

    public override bool Register(CharacterComponent Character)
    {

        NPCComponent npc = Character.GetComponent<NPCComponent>();

        if(npc != null)
        {

            if (Characters.Count == _popCap)
            {
                DebugManager.Instance.Print(DebugManager.Log.LogNPCManager, "Could not register NPC, reached pop cap");
                return false;
            }

            Characters.Add(npc);

            npc.SetUpdateState(AIConstants.UpdateState.Ready);

            DebugManager.Instance.Print(DebugManager.Log.LogNPCManager, "Registered NPC " + npc.GetID());

            if(PartyPanelList.Instance)
            {
                PartyPanelList.Instance.RegisterCharacter(Character);
            }
            if(NPCPanel.Instance)
            {
                NPCPanel.Instance.RegisterCharacter(Character);
            }

            return true;
        }

        return false;
    }

    public override void UnRegister(CharacterComponent Character)
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
            Characters.Remove(npc);
        }  
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
            if (npc.GetUpdateState() == AIConstants.UpdateState.Ready)
            {
                if (npc.CharacterMode == AIConstants.Mode.Wander)
                {
                    DebugManager.Instance.Print(DebugManager.Log.LogNPCManager, this.gameObject.name + " - Mode - Wander");
                    WanderModeUpdate(npc);
                }
                else if(npc.CharacterMode == AIConstants.Mode.Schedule)
                {
                    DebugManager.Instance.Print(DebugManager.Log.LogNPCManager, this.gameObject.name + " - Mode - Schedule");
                    ScheduleModeUpdate(npc);
                }
                else if (npc.CharacterMode == AIConstants.Mode.Stationary)
                {
                    DebugManager.Instance.Print(DebugManager.Log.LogNPCManager, this.gameObject.name + " - Mode - Stationary");

                    bool success;
                    BehaviorHelper.IdleBehavior(npc, out success);
                }
                else if (npc.CharacterMode == AIConstants.Mode.Selected)
                {
                    DebugManager.Instance.Print(DebugManager.Log.LogNPCManager, this.gameObject.name + " - Mode - Selected");
                }
            }
            else if (npc.GetUpdateState() == AIConstants.UpdateState.Busy)
            {
//                DebugManager.Instance.Print(DebugManager.Log.LogNPCManager, npc.GetID() + "- cannot update, NPC is busy");
            }
        }

    }

    private void WanderModeUpdate(NPCComponent npc)
    {
        bool success;
        CharacterBehaviorScript behaviorScript = null;

        int randomIndex = Random.Range(0, 2);

        //if(DebugManager.Instance.LogNPCManager) Debug.Log(npc.GetID() + " - Selected behavior " + SelectedBehavior.ToString());

        if (randomIndex == 0)
        {
            behaviorScript = BehaviorHelper.MoveToRandomLocation(npc, out success);
        }
        else if (randomIndex == 1)
        {
            Interactable generic;
            if (InteractableManager.Instance.FindInteractableByID(InteractableConstants.InteractableID.Generic, out generic))
            {
                if (generic != null && generic.HasBeenInteractedWith(npc) == false)
                {
                    behaviorScript = BehaviorHelper.ApproachInteractableBehavior(npc, generic, out success);
                }
                else
                {
                    Debug.Log(npc.GetID() + " has already interacted with " + generic.GetID());
                }
            }

        }

        npc.AddNewBehavior(behaviorScript);
    }

    private void ScheduleModeUpdate(NPCComponent npc)
    {
        //see what tasks the NPC is eligible for
        foreach(CharacterScheduledTask task in npc.GetSpawnData().GetScheduledTasks())
        {
            
        }
    }

}
