using Constants;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTaskComponent : MonoBehaviour
{
    [SerializeField] private CharacterTask task;

    private void Awake()
    {
        task = CharacterTask.Empty();
    }

    public CharacterTask GetTask()
    {
        return task;
    }

    public void SetTask(CharacterTask _task)
    {
        task = _task;
    }
}
