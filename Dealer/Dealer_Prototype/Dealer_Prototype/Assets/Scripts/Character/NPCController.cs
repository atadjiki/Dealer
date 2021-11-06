using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class NPCController : CharacterComponent
{

    public enum Behavior { Stationary, Wander, None };
    public enum StationType { Conversation, Bar, Leaning, None };

    public enum ActionType { Moved, Idled, None };
    private ActionType LastAction = ActionType.None;

    public Behavior BehaviorMode = Behavior.Wander;
    public List<StationType> AvailableStations;

    private float Wander_SecondsBeforeMoving_Min = 4.0f;
    private float Wander_SecondsBeforeMoving_Max = 7.0f;

    private Coroutine WanderCoroutine;

    private void Awake()
    {
        Build();
    }

    private void Build()
    {
        Initialize();

        BehaviorUpdate();
        
    }

    private void BehaviorUpdate()
    {
        if (BehaviorMode == Behavior.Wander)
        {
            if (LastAction == ActionType.Idled)
            {
                if (DebugManager.Instance.LogCharacter) Debug.Log(this.gameObject.name + " - Behavior - MoveToRandomPoint");
                WanderCoroutine = StartCoroutine(PerformAction_MoveToRandomPoint());
            }
            else if (LastAction == ActionType.Moved || LastAction == ActionType.None)
            {
                if (DebugManager.Instance.LogCharacter) Debug.Log(this.gameObject.name + " - Behavior - Idle");
                WanderCoroutine = StartCoroutine(PerformAction_Idle());
            }
            
        }
        else if(BehaviorMode == Behavior.Stationary)
        {
            if (DebugManager.Instance.LogCharacter) Debug.Log(this.gameObject.name + " - Behavior - Idle");
            WanderCoroutine = StartCoroutine(PerformAction_Idle());
        }
    }

    Vector3 PickRandomPoint()
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

    public IEnumerator PerformAction_MoveToRandomPoint()
    {
        LastAction = ActionType.Moved;

        while (true)
        {
            if (MoveToLocation(PickRandomPoint()))
            {
                yield break;
            }
        }
    }

    public IEnumerator PerformAction_Idle()
    {
        LastAction = ActionType.Idled;

        yield return new WaitForSeconds(Random.Range(Wander_SecondsBeforeMoving_Min, Wander_SecondsBeforeMoving_Max));

        BehaviorUpdate();
    }

    public override void OnDestinationReached(Vector3 destination)
    {
        base.OnDestinationReached(destination);

        if(WanderCoroutine != null) StopCoroutine(WanderCoroutine);

        BehaviorUpdate();
    }
}
