using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class NPCController : CharacterComponent
{

    public enum Behavior { Stationary, Wander, None };
    public enum StationType { Conversation, Bar, Leaning, None };

    public enum ActionType { Move, Idle, None };
    private ActionType LastAction = ActionType.None;
    public ActionType GetLastAction() { return LastAction; }

    public Behavior BehaviorMode = Behavior.Wander;
    public List<StationType> AvailableStations;

    private float Wander_SecondsBeforeMoving_Min = 4.0f;
    private float Wander_SecondsBeforeMoving_Max = 7.0f;

    private Coroutine ActionCoroutine;

    public enum UpdateState { Ready, Busy, None };
    public UpdateState updateState = UpdateState.None;

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

        Initialize();

        updateState = UpdateState.Ready;
      //  BehaviorUpdate();
        
    }

    private void OnDestroy()
    {
        NPCManager.Instance.UnRegisterNPC(this);
    }

    

    private Vector3 PickRandomPoint()
    {
        var point = Random.onUnitSphere * Random.Range(moveRadius, moveRadius*1.5f);
        point.y = 0;
        point += this.transform.position;

        var graph = AstarPath.active.data.recastGraph;

        if(graph != null)
        {
            return graph.GetNearest(point, NNConstraint.Default).clampedPosition;
        }
        else
        {
            return point;
        }   
    }

    public void PerformAction(ActionType action)
    {
        if(action == ActionType.Idle)
        {
            ActionCoroutine = StartCoroutine(PerformAction_Idle());
        }
        else if(action == ActionType.Move)
        {
            ActionCoroutine = StartCoroutine(PerformAction_MoveToRandomPoint());
        }
    }


    private IEnumerator PerformAction_MoveToRandomPoint()
    {
        LastAction = ActionType.Move;
        updateState = UpdateState.Busy;

        while (true)
        {
            if (MoveToLocation(PickRandomPoint()))
            {
                yield break;
            }
        }
    }

    private IEnumerator PerformAction_Idle()
    {
        LastAction = ActionType.Idle;
        updateState = UpdateState.Busy;

        yield return new WaitForSeconds(Random.Range(Wander_SecondsBeforeMoving_Min, Wander_SecondsBeforeMoving_Max));

        updateState = UpdateState.Ready;
    }

    public override void OnDestinationReached(Vector3 destination)
    {
        base.OnDestinationReached(destination);

        if (ActionCoroutine != null) StopCoroutine(ActionCoroutine);

        updateState = UpdateState.Ready;
    }

}
