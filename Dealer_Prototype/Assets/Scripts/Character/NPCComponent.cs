using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public enum NPCTaskID { GoToRandomLocation, PerformIdle };
public enum NPCTaskState { ToDo, InProgress, Complete };

public enum NPCState { HasTask, NoTask };

[System.Serializable]
public struct NPCTask
{
    public NPCTaskID ID;
    public NPCTaskState TaskState;
    public NPCState State;
    public float Lifetime;

    public NPCTask(NPCTaskID _ID)
    {
        ID = _ID;
        State = NPCState.HasTask;
        TaskState = NPCTaskState.ToDo;
        Lifetime = 0;
    }

    public void ChooseNextTask(List<NPCTaskID> AllowedTasks)
    { 
        if(AllowedTasks != null && AllowedTasks.Count > 0)
        {
            NPCTaskID ChosenTask = AllowedTasks[Random.Range(0, AllowedTasks.Count)];

            this = new NPCTask(ChosenTask);

            return;
        }

        this.State = NPCState.NoTask;
    }
}

public class NPCComponent : CharacterComponent
{
    protected NavigatorComponent navigator;

    //
    //tasks
    //
    private NPCTask _task;
    //

    public override IEnumerator PerformInitialize()
    {
        //create a navigator to move the model
        if (navigator == null)
        {
            GameObject navigatorObject = new GameObject("Navigator");
            navigatorObject.transform.parent = transform;
            navigatorObject.transform.rotation = transform.rotation;
            navigator = navigatorObject.AddComponent<NavigatorComponent>();
            navigator.Initialize();
            yield return new WaitUntil(() => navigator.HasInitialized());
        }

        if (model == null)
        {
            //load our associated model
            GameObject spawnedCharacter = Instantiate(PrefabLibrary.GetCharacterModelByID(data.ModelID), navigator.transform);

            model = spawnedCharacter.GetComponent<CharacterModel>();
            yield return new WaitUntil(() => model != null);
        }

        _initialized = true;

        StartCoroutine(SelectNewTask());

        yield return null;
    }

    //tasks

    private IEnumerator SelectNewTask()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            _task.ChooseNextTask(data.AllowedTasks);

            if(_task.State == NPCState.HasTask)
            {
                switch (_task.ID)
                {
                    case NPCTaskID.GoToRandomLocation:
                        yield return StartCoroutine(Task_GoToRandomDestination());
                        break;
                    case NPCTaskID.PerformIdle:
                        yield return StartCoroutine(Task_PerformIdle());
                        break;
                }
            }

            yield return new WaitForSeconds(1.0f);
        }
    }

    private IEnumerator Task_GoToRandomDestination()
    {
        _task.TaskState = NPCTaskState.InProgress;

        Vector3 destination = NavigationHelper.GetRandomPointOnGraph(model.transform.position);

        navigator.SetDestination(destination);

        navigator.ToggleMovement(true);
        model.ToWalking();

        yield return new WaitForSeconds(0.2f);

        while (navigator.IsMoving() && navigator.GetDistanceToDestination() > 0.1f)
        {
            _task.Lifetime += Time.fixedDeltaTime;

            Debug.DrawLine(navigator.GetStartOfPath(), navigator.GetNextPointInPath(), Color.blue, Time.fixedDeltaTime);

            Debug.DrawLine(navigator.transform.position, destination, Color.green, Time.fixedDeltaTime);

            yield return new WaitForFixedUpdate();
        }

        navigator.ToggleMovement(false);
        model.ToIdle();
        

        _task.TaskState = NPCTaskState.Complete;
    }

    private IEnumerator Task_PerformIdle()
    {
        _task.TaskState = NPCTaskState.InProgress;

        navigator.ToggleMovement(false);
        model.ToIdle();

        while ((_task.Lifetime += Time.fixedDeltaTime) < 1.0f)
        {
            _task.Lifetime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        _task.TaskState = NPCTaskState.Complete;
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        if(Application.isPlaying && _initialized)
        {
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.green;

            if(_task.State == NPCState.HasTask)
            {
                Handles.Label(model.transform.position + new Vector3(-0.5f, -0.1f, 0), "Task:");
                Handles.Label(model.transform.position + new Vector3(-0.5f, -0.75f, 0), _task.ID.ToString(), style);
                Handles.Label(model.transform.position + new Vector3(-0.5f, -1.25f, 0), _task.TaskState.ToString());

                if (_task.TaskState == NPCTaskState.InProgress)
                {
                    Handles.Label(model.transform.position + new Vector3(-0.5f, -1.75f, 0), _task.Lifetime.ToString());
                }
            }
            else
            {
                Handles.Label(model.transform.position + new Vector3(-0.5f, -0.1f, 0), "No Task");
            }

        }
    }
#endif
}
