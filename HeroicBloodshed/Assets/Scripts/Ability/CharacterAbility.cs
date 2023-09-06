using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class CharacterAbility : MonoBehaviour
{
    public enum AbilityState { NONE, START, PERFORM, END }

    [SerializeField] protected AbilityID _abilityID;

    protected AbilityState _state = AbilityState.NONE;

    protected CharacterComponent _caster;
    protected CharacterComponent _target;

    public void Setup()
    {
        StartCoroutine(Coroutine_Setup());
    }

    protected virtual IEnumerator Coroutine_Setup()
    {
        yield return null;
    }

    public void Start()
    {
        StartCoroutine(Coroutine_Start());
    }

    protected virtual IEnumerator Coroutine_Start()
    {
        yield return null;
    }

    public void Perform()
    {
        StartCoroutine(Coroutine_Perform());
    }

    protected virtual IEnumerator Coroutine_Perform()
    {
        yield return null;
    }

    public void End()
    {
        StartCoroutine(Coroutine_End());
    }

    protected virtual IEnumerator Coroutine_End()
    {
        yield return null;
    }
}
