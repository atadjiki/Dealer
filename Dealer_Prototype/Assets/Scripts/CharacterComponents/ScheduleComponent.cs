using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class ScheduleComponent : MonoBehaviour
{
    private HashSet<AIConstants.ScheduleTaskID> PreviousTasks;

    public void AddTask(AIConstants.ScheduleTaskID ID)
    {
        PreviousTasks.Add(ID);
    }

    public bool HasCompletedTask(AIConstants.ScheduleTaskID ID)
    {
        return PreviousTasks.Contains(ID);
    }

    public bool IsEligibleFor(AIConstants.ScheduleTaskID ID)
    {
        return false;
    }
}
