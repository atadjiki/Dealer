using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(StateMachine))]
public class EncounterStateController : MonoBehaviour
{
    private StateMachine _stateMachine;

    private void Awake()
    {
        _stateMachine = GetComponent<StateMachine>();
    }

    private void Start()
    {
    }
}
