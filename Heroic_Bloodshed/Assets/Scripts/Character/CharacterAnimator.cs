using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

[RequireComponent(typeof(Animator))]
public class CharacterAnimator : MonoBehaviour, ICharacterEventHandler
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetAnim(CharacterAnim anim, float transitionTime = 0.25f)
    {
        string state = GetAnimString(anim);

        _animator.CrossFadeInFixedTime(state, transitionTime);
    }

    public void HandleEvent(CharacterEvent characterEvent, object eventData)
    {
        switch(characterEvent)
        {
            case CharacterEvent.DESTINATION_REACHED:
                SetAnim(CharacterAnim.IDLE);
                break;
            case CharacterEvent.MOVEMENT_BEGIN:
            {
                MovementPathInfo info = (MovementPathInfo)eventData;
                HandleEvent_MovementBegin(info);
            }
            break;
        }
    }

    private void HandleEvent_MovementBegin(MovementPathInfo info)
    {
        if (info.PathType == MovementPathType.MOVE)
        {
            SetAnim(CharacterAnim.MOVING);
        }
        else if (info.PathType == MovementPathType.VAULT_OBSTACLE)
        {
            SetAnim(CharacterAnim.VAULT_OBSTACLE);
        }
        else if (info.PathType == MovementPathType.VAULT_WALL)
        {
            SetAnim(CharacterAnim.VAULT_WALL);
        }
    }
}
