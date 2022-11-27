using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public enum NavigatorTaskID { GoToRandomLocation, PerformIdle };
public enum NavigatorTaskState { ToDo, InProgress, Complete };

[System.Serializable]
public struct NavigatorTask
{
    private NavigatorTaskID ID;
    private NavigatorTaskState State;
    private float Lifetime;

    public NavigatorTask(NavigatorTaskID _ID)
    {
        ID = _ID;
        State = NavigatorTaskState.InProgress;
        Lifetime = 0;
    }

    public NavigatorTaskID GetID() { return ID; }
    public NavigatorTaskState GetState() { return State; }

    public NavigatorTaskState AdvanceState()
    {
        Lifetime = Time.time - Lifetime;

        if(State == NavigatorTaskState.ToDo) { State = NavigatorTaskState.InProgress; }
        else if (State == NavigatorTaskState.InProgress) { State = NavigatorTaskState.Complete; }

        return State;
    }

    public static NavigatorTask ChooseRandomTask()
    {
        NavigatorTaskID[] Tasks = (NavigatorTaskID[]) System.Enum.GetValues(typeof(NavigatorTaskID));

        NavigatorTaskID ChosenTask = Tasks[ Random.Range(0, Tasks.Length) ];

        Debug.Log("New navigator task -" + ChosenTask.ToString());

        return new NavigatorTask(ChosenTask);
    }

    public void PrintDebug()
    {
        Debug.Log("Task: " + ID.ToString() + " - State: " + State.ToString() + " - Duration: " + (Lifetime) + "s");
    }
}


[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(AIPath))]
public class NavigatorComponent : MonoBehaviour, IGameplayInitializer
{
    //pathfinding AI
    private Seeker _seeker;
    private AIPath _AI;
    //
    //tasks
    //
    private NavigatorTask _task;
    //
    private bool _initialized = false;

    public bool HasInitialized()
    {
        return _initialized;
    }

    public void Initialize()
    {
        StartCoroutine(PerformInitialize());
    }

    public IEnumerator PerformInitialize()
    {
        _seeker = GetComponent<Seeker>();

        yield return new WaitUntil(() => _seeker != null);

        _AI = GetComponent<AIPath>();

        yield return new WaitUntil(() => _AI != null);

        _initialized = true;
    }

    //navigator stuff

    public void Launch()
    {
        if (_task.GetState() == NavigatorTaskState.InProgress)
        {
            return;
        }

        _task = NavigatorTask.ChooseRandomTask();

        switch (_task.GetID())
        {
            case NavigatorTaskID.GoToRandomLocation:
                StartCoroutine(Task_GoToRandomDestination());
                break;
            case NavigatorTaskID.PerformIdle:
                StartCoroutine(Task_PerformIdle());
                break;
        }
    }

    private IEnumerator Task_GoToRandomDestination()
    {
        _task.PrintDebug();
        _task.AdvanceState();

        int radius = Random.Range(5, 10);

        Vector3 point = Random.insideUnitSphere * radius;
        point.y = 0;
        point += _AI.position;

        _AI.destination = point;
        _AI.SearchPath();

        Debug.Log("Going to point " + point.ToString());
        yield return new WaitForSeconds(0.1f);

        while (!_AI.isStopped && _AI.remainingDistance < 0.1f)
        {
            yield return new WaitForEndOfFrame();
        }

        _task.PrintDebug();
        _task.AdvanceState();

        Launch();
    }

    private IEnumerator Task_PerformIdle()
    {
        float waitTime = 2.0f;

        _task.PrintDebug();
        _task.AdvanceState();

        Debug.Log("Idling for " + waitTime);
        yield return new WaitForSeconds(waitTime);

        _task.PrintDebug();
        _task.AdvanceState();

        Launch();
    }
}
