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

    private int _updateEveryFrames = 60 * 3;
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
        if (npc.GetComponent<PlayerComponent>() != null)
        {
            player = npc.GetComponent<PlayerComponent>();
            return true;
        }

        if (Characters.Count == _popCap)
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

            if (npc.GetUpdateState() == CharacterConstants.UpdateState.Ready)
            {
                if (npc.CharacterMode == CharacterConstants.Mode.Wander)
                {
                    WanderModeUpdate(npc);
                }
                else if (npc.CharacterMode == CharacterConstants.Mode.Stationary)
                {
                    if (DebugManager.Instance.LogCharacter) Debug.Log(this.gameObject.name + " - Mode - Stationary");

                    bool success;
                    BehaviorHelper.IdleBehavior(npc, out success);
                }
                else if (npc.CharacterMode == CharacterConstants.Mode.Selected)
                {
                    if (DebugManager.Instance.LogCharacter) Debug.Log(this.gameObject.name + " - Mode - Selected");
                }
            }
            else if (npc.GetUpdateState() == CharacterConstants.UpdateState.Busy)
            {
                //    if(DebugManager.Instance.LogNPCManager) Debug.Log(npc.GetID() + "- cannot update, NPC is busy");
            }
        }

    }

    private void WanderModeUpdate(NPCComponent npc)
    {
        int randomIndex = Random.Range(0, 2);

        //if(DebugManager.Instance.LogNPCManager) Debug.Log(npc.GetID() + " - Selected behavior " + SelectedBehavior.ToString());

        if (randomIndex == 0)
        {
            bool success;
            BehaviorHelper.MoveToRandomLocation(npc, out success);
        }
        else if (randomIndex == 1)
        {

            Interactable jukeBox;
            if (FindInteractableByID(InteractableConstants.InteractableID.Jukebox, out jukeBox))
            {
                if (jukeBox != null && jukeBox.HasBeenInteractedWith(npc) == false)
                {
                    bool success;
                    BehaviorHelper.InteractWithBehavior(npc, jukeBox, out success);
                }
                else
                {
                    Debug.Log(npc.GetID() + " has already interacted with " + jukeBox.GetID());
                }
            }

        }

    }

    public bool FindInteractableByID(InteractableConstants.InteractableID ID, out Interactable result)
    {
        foreach (Interactable interactable in Interactables)
        {
            if (interactable.GetID() == ID.ToString())
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
        if (selectedNPC == null)
        {
            PossessNPC(NPC);
        }
        else if (selectedNPC != NPC)
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

        GameplayCanvas.Instance.OnCharacterSelected(NPC);
    }

    private void UnpossessNPC()
    {
        if (DebugManager.Instance.LogNPCManager) Debug.Log("Unselected " + selectedNPC.GetID());
        selectedNPC.PerformUnselect();
        selectedNPC = null;

        GameplayCanvas.Instance.OnCharacterDeselected();
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
            bool success;
            CharacterBehaviorScript behaviorScript = BehaviorHelper.MoveToBehavior(selectedNPC, Location, out success);

            selectedNPC.AddNewBehavior(behaviorScript);
        }
    }

    public void AttemptInteractWithPossesedNPC(Interactable interactable)
    {
        if (selectedNPC != null)
        {
            bool success;
            CharacterBehaviorScript behaviorScript;

            if (BehaviorHelper.IsInteractionAllowed(selectedNPC, interactable))
            {
                behaviorScript = BehaviorHelper.InteractWithBehavior(selectedNPC, interactable, out success);
            }
            else
            {
                behaviorScript = BehaviorHelper.ApproachBehavior(selectedNPC, interactable, out success);
            }

            selectedNPC.AddNewBehavior(behaviorScript);


        }
    }
}
