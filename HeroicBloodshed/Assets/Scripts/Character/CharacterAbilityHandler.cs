using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class CharacterAbilityHandler : MonoBehaviour, ICharacterEventReceiver
{
    private static float _waitTime = 1.0f;

    private CharacterComponent _caster;

    public void Setup(CharacterComponent caster)
    {
        _caster = caster;
    }

    public IEnumerator PerformAbility(AbilityID abilityID, CharacterComponent target = null, Vector3 destination = new Vector3())
    {
        switch (abilityID)
        {
            case AbilityID.MoveHalf:
            case AbilityID.MoveFull:
                yield return Coroutine_HandleAbility_Move(destination);
                break;
            case AbilityID.FireWeapon:
                yield return Coroutine_HandleAbility_Attack(target);
                break;
            case AbilityID.Reload:
                yield return Coroutine_HandleAbility_Reload();
                break;
            case AbilityID.SkipTurn:
                yield return Coroutine_HandleAbility_SkipTurn();
                break;
            case AbilityID.Heal:
                yield return Coroutine_HandleAbility_Heal();
                break;
            case AbilityID.Grenade:
                break;
            case AbilityID.Interact:
                break;
            default:
                break;
        }
    }

    private IEnumerator Coroutine_HandleAbility_Move(Vector3 destination)
    {
        Debug.Log("Handling Ability: Move");

        CameraRig.Instance.Follow(_caster);

        yield return new WaitForSeconds(1.0f);

        CharacterNavigator navigator = _caster.GetNavigator();

        _caster.HandleEvent(CharacterEvent.MOVING);

        yield return navigator.Coroutine_MoveToPosition(destination);

        _caster.HandleEvent(CharacterEvent.STOPPED);
    }

    private IEnumerator Coroutine_HandleAbility_Attack(CharacterComponent target)
    {
        if (CameraRig.IsActive())
        {
            float waitTime = CameraRig.Instance.GoBetween(_caster, target);

            yield return new WaitForSecondsRealtime(waitTime + 0.5f); //add a lil extra time 
        }

        yield return Coroutine_RotateTowards(target);

        WeaponDefinition weaponDef = WeaponDefinition.Get(_caster.GetWeaponID());

        int chanceToHit = weaponDef.CalculateChanceToHit(_caster, target);

        bool success = (Random.Range(1, 100) < chanceToHit);

        target.ToggleHighlight(false);

        StartCoroutine(Coroutine_FireWeaponAt(target));

        if (success)
        {
            DamageInfo damageInfo = weaponDef.CalculateDamage(_caster, target);

            StartCoroutine(Coroutine_HandleDamage(target, damageInfo));

            if (damageInfo.IsKill)
            {
                target.HandleEvent(CharacterEvent.DEATH, damageInfo);

                if (EncounterManager.IsActive())
                {
                    EncounterManager.Instance.RequestEventBanner(GetDisplayString(target.GetID()) + " killed!", _waitTime * 2);
                }

                yield return new WaitForSeconds(_waitTime * 2);
            }
            else if (damageInfo.IsCrit)
            {
                if (EncounterManager.IsActive())
                {
                    EncounterManager.Instance.RequestEventBanner("Critical Hit!", _waitTime);
                }
            }
            else {

                if (EncounterManager.IsActive())
                {
                    EncounterManager.Instance.RequestEventBanner(damageInfo.ActualDamage + " Damage!", _waitTime);
                }

                yield return new WaitForSeconds(_waitTime);
            }
        }
        else
        {
            Debug.Log("Miss!");

            if (EncounterManager.IsActive())
            {
                EncounterManager.Instance.RequestEventBanner("Missed!", _waitTime);
            }

            yield return new WaitForSeconds(_waitTime);
        }

        yield return new WaitForSeconds(_waitTime);
    }

    private IEnumerator Coroutine_HandleAbility_Reload()
    {
        if (CameraRig.IsActive())
        {
            CameraRig.Instance.Follow(_caster);
        }

        _caster.HandleEvent(CharacterEvent.RELOAD);
        yield return new WaitForSeconds(_waitTime);
    }

    private IEnumerator Coroutine_HandleAbility_SkipTurn()
    {
        if (CameraRig.IsActive())
        {
            CameraRig.Instance.Follow(_caster);
        }

        _caster.HandleEvent(CharacterEvent.SKIP_TURN);
        yield return new WaitForSeconds(_waitTime);
    }

    private IEnumerator Coroutine_HandleAbility_Heal()
    {
        if (CameraRig.IsActive())
        {
            CameraRig.Instance.Follow(_caster);
        }

        _caster.HandleEvent(CharacterEvent.HEAL);
        yield return new WaitForSeconds(_waitTime);
    }

    //utility

    private IEnumerator Coroutine_FireWeaponAt(CharacterComponent target)
    {
        HandleEvent(CharacterEvent.TARGETING, target);

        yield return Coroutine_RotateTowards(target);

        WeaponAttackDefinition attackDef = _caster.GetWeaponAttackDefinition();

        int shotTotal = attackDef.CalculateShotCount();

        for (int i = 0; i < shotTotal; i++)
        {
            _caster.HandleEvent(CharacterEvent.FIRE);

            yield return new WaitForSeconds(attackDef.TimeBetweenShots);
        }

        HandleEvent(CharacterEvent.UNTARGETING, null);
    }

    private IEnumerator Coroutine_RotateTowards(CharacterComponent target)
    {
        if (_caster == null || target == null)
        {
            yield break;
        }

        float currentTime = 0;
        float duration = 0.15f;

        Quaternion targetRotation = Quaternion.LookRotation(target.GetWorldLocation() - _caster.GetWorldLocation());

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            _caster.SetWorldRotation(Quaternion.Slerp(_caster.GetWorldRotation(), targetRotation, currentTime / duration));
            _caster.SetWorldEulerAngles(new Vector3(0, _caster.GetWorldRotation().eulerAngles.y, 0));
            yield return null;
        }

        _caster.SetWorldRotation(targetRotation);
    }

    private IEnumerator Coroutine_HandleDamage(CharacterComponent target, DamageInfo damageInfo)
    {
        target.HandleEvent(CharacterEvent.DAMAGE, damageInfo);

        WeaponAttackDefinition attackDef = _caster.GetWeaponAttackDefinition();

        int hits = 0;
        int hitTotal = Random.Range(1, attackDef.CalculateShotCount());

        for (int i = 0; i < hitTotal; i++)
        {
            if (hits < hitTotal)
            {
                if (damageInfo.IsCrit)
                {
                    target.HandleEvent(CharacterEvent.HIT_HARD);
                }
                else
                {
                    target.HandleEvent(CharacterEvent.HIT_LIGHT);
                }

                hits++;
            }

            yield return new WaitForSeconds(attackDef.TimeBetweenShots);
        }

        yield return null;
    }

    public void RotateTowardsNearestTarget()
    {
        CharacterComponent closestEnemy = _caster.GetClosestEnemy();

        if (closestEnemy != null)
        {
            RotateTowards(closestEnemy);
        }
    }

    public void RotateTowards(CharacterComponent target)
    {
        StartCoroutine(Coroutine_RotateTowards(target));
    }

    public void HandleEvent(CharacterEvent characterEvent, object eventData)
    {
        switch(characterEvent)
        {
            case CharacterEvent.UPDATE:
                RotateTowardsNearestTarget();
                break;
            default:
                break;
        }
    }

    public bool CanReceiveCharacterEvents()
    {
        return false;
    }
}
