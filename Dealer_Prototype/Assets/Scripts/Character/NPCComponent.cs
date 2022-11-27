using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEditor;
using UnityEngine;

public enum NPCTaskID { GoToRandomLocation, PerformIdle };
public enum NPCTaskState { ToDo, InProgress, Complete };

[System.Serializable]
public struct NPCTask
{
    public NPCTaskID ID;
    public NPCTaskState State;
    public float Lifetime;

    public NPCTask(NPCTaskID _ID)
    {
        ID = _ID;
        State = NPCTaskState.ToDo;
        Lifetime = 0;
    }

    public static NPCTask ChooseRandomTask()
    {
        NPCTaskID[] Tasks = (NPCTaskID[])System.Enum.GetValues(typeof(NPCTaskID));

        NPCTaskID ChosenTask = Tasks[Random.Range(0, Tasks.Length)];

        return new NPCTask(ChosenTask);
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
            yield return new WaitForSeconds(1.0f);

            _task = NPCTask.ChooseRandomTask();

            switch (_task.ID)
            {
                case NPCTaskID.GoToRandomLocation:
                    yield return StartCoroutine(Task_GoToRandomDestination());
                    break;
                case NPCTaskID.PerformIdle:
                    yield return StartCoroutine(Task_PerformIdle());
                    break;
            }

            yield return new WaitForSeconds(1.0f);
        }
    }

    private IEnumerator Task_GoToRandomDestination()
    {
        _task.State = NPCTaskState.InProgress;

        int radius = Random.Range(5, 10);

        Vector3 point = Random.insideUnitSphere * radius;
        point.y = 0;
        point += model.transform.position;

        navigator.SetDestination(point);

        yield return new WaitForSeconds(0.2f);

        navigator.ToggleMovement(true);
        model.ToWalking();

        while (navigator.IsMoving())
        {
            _task.Lifetime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        navigator.ToggleMovement(false);
        model.ToIdle();

        _task.State = NPCTaskState.Complete;
    }

    private IEnumerator Task_PerformIdle()
    {
        _task.State = NPCTaskState.InProgress;

        navigator.ToggleMovement(false);
        model.ToIdle();

        while ((_task.Lifetime += Time.fixedDeltaTime) < 2.0f)
        {
            _task.Lifetime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        _task.State = NPCTaskState.Complete;
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        if(Application.isPlaying)
        {
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.green;

            Handles.Label(model.transform.position + new Vector3(-0.5f, -0.1f, 0), "Task:");
            Handles.Label(model.transform.position + new Vector3(-0.5f, -0.75f, 0), _task.ID.ToString(), style);
            Handles.Label(model.transform.position + new Vector3(-0.5f, -1.25f, 0), _task.State.ToString());

            if (_task.State == NPCTaskState.InProgress)
            {
                Handles.Label(model.transform.position + new Vector3(-0.5f, -1.75f, 0), _task.Lifetime.ToString());
            }
        }

    }
#endif
}
