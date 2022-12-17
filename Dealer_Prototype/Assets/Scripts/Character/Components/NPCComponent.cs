using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEditor;
using UnityEngine;

public class NPCComponent : CharacterComponent
{
    private List<NPC.TaskID> _allowedTasks;

    protected NavigatorComponent navigator;

    private NPCTask npcTask = null;

    public override void ProcessSpawnData(object _data)
    {
        NPCSpawnData npcData = (NPCSpawnData) _data;

        _modelID = npcData.ModelID;

        _allowedTasks = npcData.AllowedTasks;
    }

    public override IEnumerator PerformInitialize()
    {

        yield return base.PerformInitialize();

        //create a navigator to move the model
        if (navigator == null)
        {
            GameObject navigatorObject = new GameObject("Navigator");
            navigatorObject.transform.parent = this.transform;
            navigatorObject.transform.position = this.transform.position;
            navigatorObject.transform.rotation = this.transform.rotation;
            navigatorObject.transform.localScale = this.transform.localScale;
            navigator = navigatorObject.AddComponent<NavigatorComponent>();
            navigator.Initialize();
            yield return new WaitUntil(() => navigator.HasInitialized());


            model.transform.parent = navigatorObject.transform;

        }

        yield return null;
    }

    public void StartNPC()
    {
        StartCoroutine(SelectNewTask());
    }

    private IEnumerator TaskCompleted()
    {
        if(npcTask != null)
        {
            Destroy(npcTask.gameObject);
            npcTask = null;
        }

        yield return new WaitForSeconds(1.0f);

        yield return SelectNewTask();
    }

    private IEnumerator SelectNewTask()
    {
        if(_allowedTasks.Count > 0)
        {
            npcTask = NPCTask.GenerateRandomTask(this.transform, _allowedTasks);

            if (npcTask != null)
            {
                switch (npcTask.ID)
                {
                    case NPC.TaskID.GoToRandomLocation:
                        yield return StartCoroutine(Task_GoToRandomDestination(npcTask));
                        break;
                    case NPC.TaskID.PerformIdle:
                        yield return StartCoroutine(Task_PerformIdle(npcTask));
                        break;
                }
            }

            yield return TaskCompleted();
        }

        yield return null;
    }


    private IEnumerator Task_GoToRandomDestination(NPCTask npcTask)
    {
        npcTask.State = NPC.TaskState.InProgress;

        Vector3 destination = NavigationHelper.GetRandomPointOnGraph(model.transform.position);

        navigator.SetDestination(destination);

        navigator.ToggleMovement(true);
        model.ToWalking();

        yield return new WaitForSeconds(0.2f);

        while (navigator.IsMoving() && navigator.GetDistanceToDestination() > 0.1f)
        {
            npcTask.Lifetime += Time.fixedDeltaTime;

            Debug.DrawLine(navigator.GetStartOfPath(), navigator.GetNextPointInPath(), Color.blue, Time.fixedDeltaTime);

            Debug.DrawLine(navigator.transform.position, destination, Color.green, Time.fixedDeltaTime);

            yield return new WaitForFixedUpdate();
        }

        navigator.ToggleMovement(false);
        model.ToIdle();


        npcTask.State = NPC.TaskState.Complete;
    }

    private IEnumerator Task_PerformIdle(NPCTask npcTask)
    {
        npcTask.State = NPC.TaskState.InProgress;

        if(navigator != null) navigator.ToggleMovement(false);
        model.ToIdle();

        while ((npcTask.Lifetime += Time.fixedDeltaTime) < 1.0f)
        {
            npcTask.Lifetime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        npcTask.State = NPC.TaskState.Complete;
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.green;

            if (npcTask != null)
            {
                Handles.Label(model.transform.position + new Vector3(-0.5f, -0.1f, 0), "Task:");
                Handles.Label(model.transform.position + new Vector3(-0.5f, -0.75f, 0), npcTask.ID.ToString(), style);
                Handles.Label(model.transform.position + new Vector3(-0.5f, -1.25f, 0), npcTask.State.ToString());

                if (npcTask.State == NPC.TaskState.InProgress)
                {
                    Handles.Label(model.transform.position + new Vector3(-0.5f, -1.75f, 0), npcTask.Lifetime.ToString());
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
