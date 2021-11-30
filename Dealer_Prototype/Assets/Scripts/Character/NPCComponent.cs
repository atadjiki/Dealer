using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class NPCComponent : CharacterComponent
{ 
    [Range(0.0f, 10.0f)]
    public float IdleSeconds_Max = 5.0f;

    internal override void Initialize(SpawnData spawnData)
    {
        base.Initialize(spawnData);

        if (NPCManager.Instance.RegisterNPC(this) == false)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        NPCManager.Instance.UnRegisterNPC(this);
    }

    public void PerformAction(CharacterConstants.ActionType action)
    {
        if(action == CharacterConstants.ActionType.Idle)
        {
            ActionCoroutine = StartCoroutine(PerformAction_Idle());
        }
        else if(action == CharacterConstants.ActionType.Move)
        {
            ActionCoroutine = StartCoroutine(PerformAction_MoveToRandomPoint());
        }
    }

    private IEnumerator PerformAction_MoveToRandomPoint()
    {
        LastAction = CharacterConstants.ActionType.Move;

        while (true)
        {
            if (MoveToRandomLocation())
            {
                yield break;
            }
        }
    }

    private IEnumerator PerformAction_Idle()
    {
        LastAction = CharacterConstants.ActionType.Idle;

        yield return new WaitForSeconds(Random.Range(0.0f, IdleSeconds_Max));
    }

    public void GoToIdle()
    {
        if(ActionCoroutine != null) StopCoroutine(ActionCoroutine);

        PerformAction(CharacterConstants.ActionType.Idle);

        ToIdle();
    }

    public override void OnDestinationReached(Vector3 destination)
    {
        base.OnDestinationReached(destination);

        if (ActionCoroutine != null) StopCoroutine(ActionCoroutine);
    }

    public override void OnMouseEnter()
    {
        base.OnMouseEnter();
        GameplayCanvas.Instance.SetInteractionTipText(this);
    }

    public override void OnMouseExit()
    {
        base.OnMouseExit();
        GameplayCanvas.Instance.ClearInteractionTipText();
    }

    public override void OnMouseClicked()
    {
        base.OnMouseClicked();

        NPCManager.Instance.HandleNPCSelection(this);
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

}
