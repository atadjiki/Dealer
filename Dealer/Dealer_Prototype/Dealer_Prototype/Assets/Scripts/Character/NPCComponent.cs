using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class NPCComponent : CharacterComponent
{
    
    private CharacterConstants.ActionType LastAction = CharacterConstants.ActionType.None;
    public CharacterConstants.ActionType GetLastAction() { return LastAction; }

    public CharacterConstants.Behavior BehaviorMode = CharacterConstants.Behavior.Wander;
    public List<CharacterConstants.StationType> AvailableStations;

    private float Wander_SecondsBeforeMoving_Min = 4.0f;
    private float Wander_SecondsBeforeMoving_Max = 7.0f;

    private Coroutine ActionCoroutine;
    
    public CharacterConstants.UpdateState updateState = CharacterConstants.UpdateState.None;

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

        Initialize(); //now that we are registered, do some setup stuff

        updateState = CharacterConstants.UpdateState.Ready; //let the manager know we're ready to be handled
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
        updateState = CharacterConstants.UpdateState.Busy;

        while (true)
        {
            if (_navigator.MoveToRandomLocation())
            {
                yield break;
            }
        }
    }

    private IEnumerator PerformAction_Idle()
    {
        LastAction = CharacterConstants.ActionType.Idle;
        updateState = CharacterConstants.UpdateState.Busy;

        yield return new WaitForSeconds(Random.Range(Wander_SecondsBeforeMoving_Min, Wander_SecondsBeforeMoving_Max));

        updateState = CharacterConstants.UpdateState.Ready;
    }

    public override void OnDestinationReached(Vector3 destination)
    {
        base.OnDestinationReached(destination);

        if (ActionCoroutine != null) StopCoroutine(ActionCoroutine);

        updateState = CharacterConstants.UpdateState.Ready;
    }

}
