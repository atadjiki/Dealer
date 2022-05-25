using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CharacterTask
{
    public Constants.TaskConstants.TaskType Type { get; set; }

    public int DaysRemaining { get; set; }

    public static CharacterTask Empty()
    {
        CharacterTask task = new CharacterTask();
        task.DaysRemaining = -1;
        task.Type = Constants.TaskConstants.TaskType.None;

        return task;
    }
}
