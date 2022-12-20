using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEditor;
using UnityEngine;

public class NPCComponent : CharacterComponent
{
    protected NavigatorComponent navigator;

    protected Coroutine currentCoroutine;

    public override void ProcessSpawnData(object _data)
    {
        NPCSpawnData npcData = (NPCSpawnData) _data;

        _modelID = npcData.ModelID;
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

    public void GoTo(Vector3 location)
    {
        StopAllCoroutines();

        StartCoroutine(Task_GoTo(location));
    }

    public void Stop()
    {
        StopAllCoroutines();
        navigator.ToggleMovement(false);
        model.ToIdle();
    }

    private IEnumerator Task_GoTo(Vector3 location)
    {
        navigator.SetDestination(location);

        navigator.ToggleMovement(true);
        model.ToWalking();

        yield return new WaitForSeconds(0.2f);

        while (navigator.IsMoving() && navigator.GetDistanceToDestination() > 0.1f)
        {
            Debug.DrawLine(navigator.GetStartOfPath(), navigator.GetNextPointInPath(), Color.blue, Time.fixedDeltaTime);

            Debug.DrawLine(navigator.transform.position, location, Color.green, Time.fixedDeltaTime);

            yield return new WaitForFixedUpdate();
        }

        navigator.ToggleMovement(false);
        model.ToIdle();
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.green;

            //if (npcTask != null)
            //{
            //    Handles.Label(model.transform.position + new Vector3(-0.5f, -0.1f, 0), "Task:");
            //    Handles.Label(model.transform.position + new Vector3(-0.5f, -0.75f, 0), npcTask.ID.ToString(), style);
            //    Handles.Label(model.transform.position + new Vector3(-0.5f, -1.25f, 0), npcTask.State.ToString());

            //    if (npcTask.State == NPC.TaskState.InProgress)
            //    {
            //        Handles.Label(model.transform.position + new Vector3(-0.5f, -1.75f, 0), npcTask.Lifetime.ToString());
            //    }
            //}
            //else
            //{
            //    Handles.Label(model.transform.position + new Vector3(-0.5f, -0.1f, 0), "No Task");
            //}

        }
    }
#endif
}
