using System.Collections;
using System.Collections.Generic;
using Constants;
using GameDelegates;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterModel : MonoBehaviour
{
    private Animator _animator;
    private WeaponSocket _weaponSocket;
    private Enumerations.Team _team;

    public ModelClicked OnModelClickedDelegate;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _weaponSocket = GetComponentInChildren<WeaponSocket>();
        _team = Enumerations.Team.Neutral;
    }

    public void ApplyCharacterInfo(CharacterInfo characterInfo)
    {
        SetupWeapon(characterInfo);
    }

    public void SetTeam(Enumerations.Team team)
    {
        _team = team;
    }

    private void SetupWeapon(CharacterInfo characterInfo)
    {
        Enumerations.WeaponID weaponID = characterInfo.WeaponID;

        if(weaponID != Enumerations.WeaponID.None && _weaponSocket != null)
        {
            GameObject weaponPrefab = Instantiate(PrefabManager.Instance.GetWeaponModel(weaponID), _weaponSocket.transform);
            weaponPrefab.transform.localScale = Vector3.one;

            _animator.runtimeAnimatorController = AnimationManager.Instance.GetControllerByWeaponID(weaponID);
        }
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
        HandleHightlight(_team);
    }

    private void OnMouseExit()
    {
        RemoveHighlight();
    }

    private void OnMouseDown()
    {
        OnModelClickedDelegate.Invoke();
    }

    //delegate functions
    public void HandleHightlight(Enumerations.Team team)
    {
       switch(team)
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

    public void HandleCharacterAction(Enumerations.CharacterAction action)
    {
        switch(action)
        {
            case Enumerations.CharacterAction.Move:
            case Enumerations.CharacterAction.Approach:
                ToWalking();
                break;
            default:
                ToIdle();
                break;
        }
    }
}
