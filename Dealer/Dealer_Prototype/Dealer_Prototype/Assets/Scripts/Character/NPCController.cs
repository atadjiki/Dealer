using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class NPCController : CharacterComponent
{

    public enum Behavior { Idle, Wander };
    public enum StationType { Conversation, Bar, Leaning };

    public Behavior BehaviorMode = Behavior.Idle;
    public List<StationType> AvailableStations;

    private float Wander_SecondsBeforeMoving_Min = 2.0f;
    private float Wander_SecondsBeforeMoving_Max = 5.0f;

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
            WanderCoroutine = StartCoroutine(WanderMode());
            if (DebugManager.Instance.LogCharacter) Debug.Log(this.gameObject.name + " - Behavior Update - Wander");
        }
    }

    Vector3 PickRandomPoint()
    {
        var point = Random.insideUnitSphere * moveRadius;
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


    public IEnumerator WanderMode()
    {
        yield return new WaitForSeconds(Random.Range(Wander_SecondsBeforeMoving_Min, Wander_SecondsBeforeMoving_Max));

        while (true)
        {
            if (MoveToLocation(PickRandomPoint()))
            {
                yield break;
            }
        }
 
 
    }

    public override void OnDestinationReached(Vector3 destination)
    {
        base.OnDestinationReached(destination);

        StopCoroutine(WanderCoroutine);

        BehaviorUpdate();
    }

}
