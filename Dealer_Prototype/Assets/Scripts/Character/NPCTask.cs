using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEditor;
using UnityEngine;

public class NPCTask : MonoBehaviour
{
    public NPC.TaskID ID;
    public NPC.TaskState State = NPC.TaskState.ToDo;
    public float Lifetime = 0;

    public static NPCTask GenerateRandomTask(Transform parentTransform, List<NPC.TaskID> AllowedTasks)
    {
        if (AllowedTasks != null && AllowedTasks.Count > 0)
        {
            NPC.TaskID RandomID = AllowedTasks[Random.Range(0, AllowedTasks.Count)];

            return CreateGameObject(parentTransform, RandomID).GetComponent<NPCTask>();
        }

        return null;
    }

    public static GameObject CreateGameObject(Transform parentTransform, NPC.TaskID _ID)
    {
        GameObject npcTaskObject = new GameObject("NPC Task - " + _ID.ToString());
        npcTaskObject.transform.parent = parentTransform;

        NPCTask npcTask = npcTaskObject.AddComponent<NPCTask>();
        npcTask.ID = _ID;
        return npcTaskObject;
    }
}
