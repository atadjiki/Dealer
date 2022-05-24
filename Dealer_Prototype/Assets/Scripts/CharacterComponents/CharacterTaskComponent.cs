using Constants;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTaskComponent : MonoBehaviour
{

    [SerializeField] private TaskConstants.TaskState state = TaskConstants.TaskState.WaitingForUpdate;

    public TaskConstants.TaskState GetState()
    {
        return state;
    }

    public void SetState(TaskConstants.TaskState _state)
    {
        state = _state;
        PartyPanelList.Instance.UpdateList();
    }

    [SerializeField] private CharacterTask task;

    private void Awake()
    {
        task = CharacterTask.Empty();
        PartyPanelList.Instance.UpdateList();
    }

    public CharacterTask GetTask()
    {
        return task;
    }

    public void SetTask(CharacterTask _task)
    {
        task = _task;
        PartyPanelList.Instance.UpdateList();
    }

    public static string GetUIString(CharacterComponent character)
    {
        string uiString = "";

        if (character.GetTaskComponent().GetState() == Constants.TaskConstants.TaskState.WaitingForUpdate)
        {
            if (character.GetNavigatorComponent().State == NavigatorComponent.MovementState.Moving)
            {
                uiString += "moving";
            }
            else
            {
                uiString += "waiting for update";
            }
        }
        else if(character.GetTaskComponent().GetState() == TaskConstants.TaskState.Idle)
        {
            uiString += "idle";
        }
        else if (character.GetTaskComponent().GetTask().Type != Constants.TaskConstants.TaskType.None)
        {
            uiString += character.GetTaskComponent().GetTask().Type.ToString();
        }

        return uiString;
    }
}
