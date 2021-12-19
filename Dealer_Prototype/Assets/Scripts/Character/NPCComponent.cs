using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class NPCComponent : CharacterComponent
{ 

    internal override void Initialize(SpawnData spawnData)
    {
        base.Initialize(spawnData);

        if (NPCManager.Instance.RegisterNPC(this) == false)
        {
            Destroy(this.gameObject);
        }
    }

    internal override IEnumerator DoInitialize()
    {
        yield return base.DoInitialize();

        //register camera
        CharacterCameraManager.Instance.RegisterCharacterCamera(this);

        SetCurrentBehavior(spawnData.BehaviorMode);

        AllowedBehaviors = spawnData.AllowedBehaviors;
        AllowedInteractables = spawnData.AllowedInteractables;

        //ready to begin behaviors
        updateState = CharacterConstants.UpdateState.Ready; //let the manager know we're ready to be handled

        yield return null;
    }

    private void OnDestroy()
    {
        NPCManager.Instance.UnRegisterNPC(this);
    }

    public override void OnDestinationReached(Vector3 destination)
    {
        base.OnDestinationReached(destination);

        if (ActionCoroutine != null) StopCoroutine(ActionCoroutine);
    }

    public override void OnMouseEnter()
    {
        base.OnMouseEnter();
        if(GetCurrentBehavior() == CharacterConstants.Mode.Selected)
        {
            GameplayCanvas.Instance.SetInteractionTipTextContext(GameplayCanvas.InteractionContext.Deselect);
        }
        else
        {
            GameplayCanvas.Instance.SetInteractionTipTextContext(GameplayCanvas.InteractionContext.Select);
        }
        
    }

    public override void OnMouseExit()
    {
        base.OnMouseExit();
        GameplayCanvas.Instance.ClearInteractionTipText();
    }

    public override void OnMouseClicked()
    {
        base.OnMouseClicked();
    }

    public override void PerformSelect()
    {
        base.PerformSelect();
        GoToIdle();
    }

    public override void PerformUnselect()
    {
        base.PerformUnselect();
        GoToIdle();
    }

    public override void GoToIdle()
    {
        base.GoToIdle();
        PerformAction(CharacterConstants.ActionType.Idle);
    }

}
