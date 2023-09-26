using System.Collections;
using System.Collections.Generic;
using static Constants;
using UnityEngine;
using EPOOutline;

public class CharacterWeapon : MonoBehaviour, ICharacterEventReceiver
{
    protected WeaponID _ID;
    protected CharacterID _characterID;
    protected int _ammo = 0;

    private bool _canReceive = true;

    protected Outlinable _outline;

    public virtual void Setup(CharacterID characterID, WeaponID weaponID)
    {
        _ID = weaponID;
        _characterID = characterID;
        _ammo = GetMaxAmmo();

        Constants.TeamID team = Constants.GetTeamByID(characterID);

        Color teamColor = Constants.GetColorByTeam(team, 1.0f);

        SetupOutline(teamColor);
    }

    private void SetupOutline(Color color)
    {
        color.a = 1.0f;

        _outline.OutlineParameters.Color = color; //setup outliner color
    }


    private void SetDeadOutline()
    {
        Color color = _outline.OutlineParameters.Color;

        color.a = 0.25f;

        _outline.OutlineParameters.Color = color;
    }

    public void HandleEvent(object eventData, CharacterEvent characterEvent)
    {
        if (!_canReceive) { return; }

        switch (characterEvent)
        {
            case CharacterEvent.KILLED:
                HandleEvent_Dead();
                _canReceive = false;
                break;
            default:
                break;
        }
    }

    private void HandleEvent_Dead()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.isKinematic = false;

        this.transform.parent = null;

        SetDeadOutline();

        StartCoroutine(Coroutine_DestroyAfterTime(5.0f));
    }

    private IEnumerator Coroutine_DestroyAfterTime(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);

        Destroy(this.gameObject);
    }

    public WeaponID GetID()
    {
        return _ID;
    }
    public virtual void OnAbility(AbilityID ability)
    {
    }

    public int GetRemainingAmmo()
    {
        return _ammo;
    }

    public int GetMaxAmmo()
    {
        WeaponDefinition weaponDefinition = WeaponDefinition.Get(GetID());

        return weaponDefinition.Ammo;
    }

    public bool HasAmmo()
    {
        return _ammo > 0;
    }

    public bool CanReceiveCharacterEvents()
    {
        return _canReceive;
    }
}
