using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using static Constants;

public class CharacterComponent : MonoBehaviour
{
    private CharacterNavigator _navigator;
    private CharacterAnimator _animator;

    private void Awake()
    {
        _navigator = GetComponentInChildren<CharacterNavigator>();
        _navigator.DestinationReachedCallback += OnDestinationReached;

        _animator = GetComponentInChildren<CharacterAnimator>();
    }

    private void OnDestroy()
    {
        _navigator.DestinationReachedCallback -= OnDestinationReached;
    }

    public void MoveTo(Vector3 destination)
    {
        _navigator.MoveTo(destination);
        _animator.SetAnim(CharacterAnim.Running);
    }

    public void OnDestinationReached(CharacterNavigator navigator)
    {
        _animator.SetAnim(CharacterAnim.Idle);

    }

    public CharacterNavigator GetNavigator()
    {
        return _navigator;
    }
}
