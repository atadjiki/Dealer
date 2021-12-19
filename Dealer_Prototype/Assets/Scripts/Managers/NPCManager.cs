using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    private static NPCManager _instance;

    public static NPCManager Instance { get { return _instance; } }

    private List<Interactable> Interactables;

    private List<NPCComponent> Characters;
    private int _popCap = 10;

    private int _updateEveryFrames = 60*3;
    private int _currentFrames = 0;

    private NPCComponent selectedNPC = null;

    private PlayerComponent player;

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
        Interactables = new List<Interactable>();
        Characters = new List<NPCComponent>();
    }

    public bool HasNotExceededPopCap()
    {
        return Characters.Count < _popCap;
    }

    public bool RegisterNPC(NPCComponent npc)
    {

        //check if player
        if(npc.GetComponent<PlayerComponent>() != null)
        {
            player = npc.GetComponent<PlayerComponent>();
            return true;
        }

        if(Characters.Count == _popCap)
        {
            if (DebugManager.Instance.LogNPCManager) Debug.Log("Could not register NPC, reached pop cap");
            return false;
        }

        Characters.Add(npc);

        if (DebugManager.Instance.LogNPCManager && npc != null) Debug.Log("Registered NPC " + npc.GetID());
        return true;
    }

    public void UnRegisterNPC(NPCComponent npc)
    {
        if (DebugManager.Instance.LogNPCManager && npc != null) Debug.Log("Unregistered NPC " + npc.GetID());
        Characters.Remove(npc);
    }

    public bool RegisterInteractable(Interactable interactable)
    {
        Interactables.Add(interactable);

        if (DebugManager.Instance.LogNPCManager && interactable != null) Debug.Log("Registered Interactable " + interactable.GetID());
        return true;
    }

    public void UnRegisterInteractable(Interactable interactable)
    {
        if (DebugManager.Instance.LogNPCManager && interactable != null) Debug.Log("Unregistered Interactable " + interactable.GetID());
        Interactables.Remove(interactable);
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
                if (npc.GetCurrentBehavior() == CharacterConstants.Mode.Wander)
                {
                    WanderModeUpdate(npc);
                }
                else if (npc.GetCurrentBehavior() == CharacterConstants.Mode.Stationary)
                {
                    if (DebugManager.Instance.LogCharacter) Debug.Log(this.gameObject.name + " - Mode - Stationary");
                    npc.PerformAction(CharacterConstants.ActionType.Idle);
                }
                else if(npc.GetCurrentBehavior() == CharacterConstants.Mode.Selected)
                {
                    if (DebugManager.Instance.LogCharacter) Debug.Log(this.gameObject.name + " - Mode - Selected");
                }
            }
            else if(npc.GetUpdateState() == CharacterConstants.UpdateState.Busy)
            {
            //    if(DebugManager.Instance.LogNPCManager) Debug.Log(npc.GetID() + "- cannot update, NPC is busy");
            }
        }
        
    }

    private void WanderModeUpdate(NPCComponent npc)
    {
        CharacterConstants.Behavior SelectedBehavior = npc.GetAllowedBehaviors()[Random.Range(0, npc.GetAllowedBehaviors().Count)];

        //if(DebugManager.Instance.LogNPCManager) Debug.Log(npc.GetID() + " - Selected behavior " + SelectedBehavior.ToString());
        
        if(SelectedBehavior == CharacterConstants.Behavior.MoveToRandomLocation)
        {
            MoveToRandomLocation moveToRandomLocationScript
                = CreateBehaviorObject(npc.GetID() + " move to random location behavior").AddComponent<MoveToRandomLocation>();

            CharacterBehaviorScript.BehaviorData behaviorData = new CharacterBehaviorScript.BehaviorData
            {
                Character = npc,
                Behavior = moveToRandomLocationScript
            };

            FireBehavior(moveToRandomLocationScript, behaviorData);
        }
        else if(SelectedBehavior == CharacterConstants.Behavior.FindInteractable)
        {
            InteractableConstants.InteractableID SelectedInteraction = npc.GetAllowedInteractables()[Random.Range(0, npc.GetAllowedInteractables().Count)];

            if(SelectedInteraction == InteractableConstants.InteractableID.Jukebox)
            {
                Interactable jukeBox;
                if (FindInteractableByID(SelectedInteraction, out jukeBox))
                {
                    if (jukeBox != null && jukeBox.HasBeenInteractedWith(npc) == false)
                    {
                        InteractWithJukebox interactionscript
                            = CreateBehaviorObject(npc.GetID() + " - " + jukeBox.GetID() + " interaction behavior").AddComponent<InteractWithJukebox>();

                        CharacterBehaviorScript.BehaviorData data = new CharacterBehaviorScript.BehaviorData
                        {
                            Character = npc,
                            Interactable = jukeBox,
                            Behavior = interactionscript
                        };


                        FireBehavior(interactionscript, data);
                    }
                    else
                    {
                        Debug.Log(npc.GetID() + " has already interacted with " + jukeBox.GetID());
                    }
                }
            }
        }
       
    }

    public GameObject CreateBehaviorObject(string name)
    {
        GameObject behaviorObject = new GameObject(name);
        behaviorObject.transform.parent = this.transform;

        return behaviorObject;
    }

    private void FireBehavior(CharacterBehaviorScript behaviorScript, CharacterBehaviorScript.BehaviorData data)
    {
       behaviorScript.BeginBehavior(data);
    }

    public bool FindInteractableByID(InteractableConstants.InteractableID ID, out Interactable result)
    {
        foreach(Interactable interactable in Interactables)
        {
            if(interactable.GetID() == ID.ToString())
            {
                result = interactable;
                return true;
            }
        }

        result = null;
        return false; 
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
            selectedNPC.MoveToBehavior(Location);
        }
    }

    public void AttemptInteractWithPossesedNPC(Interactable interactable)
    {
        if(selectedNPC != null)
        {
            selectedNPC.InteractWithBehavior(interactable);
        }
    }
}
