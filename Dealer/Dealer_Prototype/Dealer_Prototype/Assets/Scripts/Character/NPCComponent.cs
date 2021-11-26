using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class NPCComponent : CharacterComponent
{ 
    [Header("Stations")]
    public List<CharacterConstants.StationType> AvailableStations;

    [Range(0.0f, 10.0f)]
    public float IdleSeconds_Max = 5.0f;

    private void Awake()
    {
        Build();
    }

    private void Build()
    {
        if (NPCManager.Instance.RegisterNPC(this) == false)
        {
            Destroy(this.gameObject);
        }

        if(GetComponentInParent<ObjectSpawner>() == null) Destroy(this.gameObject);
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
        SetUpdateState(CharacterConstants.UpdateState.Busy);

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
        SetUpdateState(CharacterConstants.UpdateState.Busy);

        yield return new WaitForSeconds(Random.Range(0.0f, IdleSeconds_Max));

        SetUpdateState(CharacterConstants.UpdateState.Ready);
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

        SetUpdateState(CharacterConstants.UpdateState.Ready);
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

}
