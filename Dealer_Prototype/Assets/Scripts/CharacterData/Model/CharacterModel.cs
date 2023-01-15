using System.Collections;
using System.Collections.Generic;
using Constants;
using GameDelegates;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterModel : MonoBehaviour
{
    private Animator _animator;
    private Enumerations.Team _team;

    public ModelClicked OnModelClickedDelegate;

    public bool allowHighlight = false;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _team = Enumerations.Team.Neutral;
    }

    public void SetTeam(Enumerations.Team team)
    {
        _team = team;
    }

    private void ToIdle()
    {
        _animator.Play("Idle_Male");
    }

    private void ToWalking()
    {
        _animator.Play("Walking_Male");
    }

    private void OnMouseEnter()
    {
        if (allowHighlight)
        {
            HandleHightlight(_team);
        }

    }

    private void OnMouseExit()
    {
        if (allowHighlight)
        {
            RemoveHighlight();
        }

    }

    private void OnMouseDown()
    {
        OnModelClickedDelegate.Invoke();
    }

    //delegate functions
    public void HandleHightlight(Enumerations.Team team)
    {
        switch (team)
        {
            case Enumerations.Team.Player:
                MaterialHelper.SetPlayerOutline(this);
                break;
            case Enumerations.Team.Neutral:
                MaterialHelper.SetNeutralOutline(this);
                break;
            case Enumerations.Team.Enemy:
                MaterialHelper.SetEnemyOutline(this);
                break;
        }
    }

    public void RemoveHighlight()
    {
        MaterialHelper.ResetCharacterOutline(this);
    }

    public void HandleCharacterAction(Enumerations.CommandType action)
    {
        switch (action)
        {
            case Enumerations.CommandType.Move:
            case Enumerations.CommandType.Approach:
                ToWalking();
                break;
            default:
                ToIdle();
                break;
        }
    }
}
