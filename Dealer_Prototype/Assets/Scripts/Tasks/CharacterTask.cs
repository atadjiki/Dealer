using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CharacterTask
{
    public Constants.TaskConstants.TaskType Type { get; set; }
    public Constants.TaskConstants.TaskState State { get; set; }
    public MarkedLocation markedLocation { get; set; }
    public int DaysRemaining { get; set; }

    public static CharacterTask Empty()
    {
        CharacterTask task = new CharacterTask();
        task.DaysRemaining = -1;
        task.markedLocation = null;
        task.Type = Constants.TaskConstants.TaskType.None;
        task.State = Constants.TaskConstants.TaskState.InActive;

        return task;
    }
}
