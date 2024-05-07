using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class CharacterComponent : MonoBehaviour
{
    private EnvironmentNavigator _navigator;
    private CharacterAnimator _animator;

    private void Awake()
    {
        _navigator = GetComponentInChildren<EnvironmentNavigator>();
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

    public void OnDestinationReached(EnvironmentNavigator navigator)
    {
        _animator.SetAnim(CharacterAnim.Idle);
    }

    public EnvironmentNavigator GetNavigator()
    {
        return _navigator;
    }
}
