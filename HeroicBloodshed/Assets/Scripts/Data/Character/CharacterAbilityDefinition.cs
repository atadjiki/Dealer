using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class AbilityHandler : MonoBehaviour
{
    private static float _waitTime = 1.0f;

    public static IEnumerator PerformAbility(AbilityID abilityID, CharacterComponent caster, CharacterComponent target = null, Vector3 destination = new Vector3())
    {
        switch (abilityID)
        {
            case AbilityID.MoveHalf:
            case AbilityID.MoveFull:
                yield return Coroutine_HandleAbility_Move(caster, destination);
                break;
            case AbilityID.FireWeapon:
                yield return Coroutine_HandleAbility_Attack(caster, target);
                break;
            case AbilityID.Reload:
                yield return Coroutine_HandleAbility_Reload(caster);
                break;
            case AbilityID.SkipTurn:
                yield return Coroutine_HandleAbility_SkipTurn(caster);
                break;
            case AbilityID.Heal:
                yield return Coroutine_HandleAbility_Heal(caster);
                break;
            default:
                break;
        }
    }

    private static IEnumerator Coroutine_HandleAbility_Move(CharacterComponent caster, Vector3 destination)
    {
        CameraRig.Instance.Follow(caster);

        yield return new WaitForSeconds(1.0f);

        CharacterNavigator navigator = caster.GetNavigator();

        caster.HandleEvent(CharacterEvent.MOVING);

        yield return navigator.Coroutine_MoveToPosition(destination);

        caster.HandleEvent(CharacterEvent.STOPPED);
    }

    private static IEnumerator Coroutine_HandleAbility_Attack(CharacterComponent caster, CharacterComponent target)
    {
        if (CameraRig.IsActive())
        {
            float waitTime = CameraRig.Instance.GoBetween(caster, target);

            yield return new WaitForSeconds(waitTime + 0.5f); //add a lil extra time 
        }

        yield return Coroutine_RotateTowards(target, caster);

        WeaponDefinition weaponDef = WeaponDefinition.Get(caster.GetWeaponID());

        target.ToggleHighlight(false);
        DamageInfo damageInfo = weaponDef.CalculateDamage(caster, target);

        if (damageInfo.IsCrit)
        {
            if (EncounterManager.IsActive())
            {
                EncounterManager.Instance.RequestEventBanner("Critical Hit!", _waitTime);
            }
        }

        yield return Coroutine_HandleDamage(target, damageInfo);

        if (damageInfo.IsKill)
        {
            yield return Coroutine_FireWeaponAt(damageInfo);
            target.HandleEvent(CharacterEvent.DEATH, damageInfo);

            if (EncounterManager.IsActive())
            {
                EncounterManager.Instance.RequestEventBanner(GetDisplayString(target.GetID()) + " killed!", _waitTime * 2);
            }

            if (CameraRig.IsActive())
            {
                CameraRig.Instance.Follow(target);
            }

            yield return new WaitForSeconds(_waitTime * 2);
        }
        else
        {
            if(EncounterManager.IsActive())
            {
                EncounterManager.Instance.RequestEventBanner(damageInfo.ActualDamage + " Damage!", _waitTime);
            }

            yield return Coroutine_FireWeaponAt(damageInfo);
        }
        yield return new WaitForSeconds(_waitTime);
    }

    private static IEnumerator Coroutine_HandleAbility_Reload(CharacterComponent caster)
    {
        if(CameraRig.IsActive())
        {
            CameraRig.Instance.Follow(caster);
        }

        caster.HandleEvent(CharacterEvent.RELOAD);
        yield return new WaitForSeconds(_waitTime);
    }

    private static IEnumerator Coroutine_HandleAbility_SkipTurn(CharacterComponent caster)
    {
        if (CameraRig.IsActive())
        {
            CameraRig.Instance.Follow(caster);
        }

        caster.HandleEvent(CharacterEvent.SKIP_TURN);
        yield return new WaitForSeconds(_waitTime);
    }

    private static IEnumerator Coroutine_HandleAbility_Heal(CharacterComponent caster)
    {
        if (CameraRig.IsActive())
        {
            CameraRig.Instance.Follow(caster);
        }

        caster.HandleEvent(CharacterEvent.HEAL);
        yield return new WaitForSeconds(_waitTime);
    }

    //utility

    private static IEnumerator Coroutine_FireWeaponAt(DamageInfo damageInfo)
    {
        yield return Coroutine_RotateTowards(damageInfo.caster, damageInfo.target);

        WeaponAttackDefinition attackDef = damageInfo.caster.GetWeaponAttackDefinition();

        int shotCount = attackDef.CalculateShotCount();
        for (int i = 0; i < shotCount; i++)
        {
            if(!damageInfo.IsKill)
            {
                if (damageInfo.ActualDamage < damageInfo.BaseDamage)
                {
                    damageInfo.target.HandleEvent(CharacterEvent.HIT_LIGHT, damageInfo);
                }
                else
                {
                    damageInfo.target.HandleEvent(CharacterEvent.HIT_HARD, damageInfo);
                }
            }

            damageInfo.caster.HandleEvent(CharacterEvent.FIRE);

            yield return new WaitForSeconds(attackDef.TimeBetweenShots);
        }
    }

    public static IEnumerator Coroutine_RotateTowards(CharacterComponent caster, CharacterComponent target)
    {
        if(caster == null || target == null)
        {
            yield break;
        }

        float currentTime = 0;
        float duration = 0.15f;

        Quaternion targetRotation = Quaternion.LookRotation(target.GetWorldLocation() - caster.GetWorldLocation());

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            caster.SetWorldRotation(Quaternion.Slerp(caster.GetWorldRotation(), targetRotation, currentTime / duration));
            caster.SetWorldEulerAngles(new Vector3(0, caster.GetWorldRotation().eulerAngles.y, 0));
            yield return null;
        }

        caster.SetWorldRotation(targetRotation);
    }

    private static IEnumerator Coroutine_HandleDamage(CharacterComponent target, DamageInfo damageInfo)
    {
        target.HandleEvent(CharacterEvent.DAMAGE, damageInfo);
        yield return null;
    }
}
