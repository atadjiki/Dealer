using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class AbilityHandler : MonoBehaviour
{
    private static float _waitTime = 1.0f;

    public static IEnumerator Coroutine_HandleAbility_Move(CharacterComponent caster, Vector3 destination)
    {
        CharacterNavigator navigator = caster.GetNavigator();

        caster.HandleEvent(null, CharacterEvent.MOVING);

        yield return navigator.Coroutine_MoveToPosition(destination);

        caster.HandleEvent(null, CharacterEvent.STOPPED);
    }

    public static IEnumerator Coroutine_HandleAbility_Attack(CharacterComponent caster, CharacterComponent target)
    {
        yield return Coroutine_RotateTowards(target, caster);

        WeaponDefinition weaponDef = WeaponDefinition.Get(caster.GetWeaponID());

        target.ToggleHighlight(false);
        DamageInfo damageInfo = weaponDef.CalculateDamage(caster, target);

        if (damageInfo.IsCrit)
        {
            EncounterManager.Instance.RequestEventBanner("Critical Hit!", _waitTime);
        }

        yield return Coroutine_HandleDamage(target, damageInfo);

        if (damageInfo.IsKill)
        {
            yield return Coroutine_FireWeaponAt(damageInfo);
            target.HandleEvent(damageInfo, CharacterEvent.KILLED);
            EncounterManager.Instance.RequestEventBanner(GetDisplayString(target.GetID()) + " killed!", _waitTime * 2);
            CameraRig.Instance.Follow(target);
            yield return new WaitForSeconds(_waitTime * 2);
        }
        else
        {
            EncounterManager.Instance.RequestEventBanner(damageInfo.ActualDamage + " Damage!", _waitTime);
            yield return Coroutine_FireWeaponAt(damageInfo);
            target.HandleEvent(damageInfo, CharacterEvent.HIT);
        }
        yield return new WaitForSeconds(_waitTime);
    }

    public static IEnumerator Coroutine_HandleAbility_Reload(CharacterComponent caster)
    {
        caster.HandleEvent(AbilityID.Reload, CharacterEvent.ABILITY);
        yield return new WaitForSeconds(_waitTime);
    }

    public static IEnumerator Coroutine_HandleAbility_SkipTurn(CharacterComponent caster)
    {
        caster.HandleEvent(AbilityID.SkipTurn, CharacterEvent.ABILITY);
        yield return new WaitForSeconds(_waitTime);
    }

    public static IEnumerator Coroutine_HandleAbility_Heal(CharacterComponent caster)
    {
        caster.HandleEvent(AbilityID.Heal, CharacterEvent.ABILITY);
        yield return new WaitForSeconds(_waitTime);
    }

    //utility

    public static IEnumerator Coroutine_FireWeaponAt(DamageInfo damageInfo)
    {
        yield return Coroutine_RotateTowards(damageInfo.caster, damageInfo.target);
        damageInfo.caster.HandleEvent(AbilityID.Attack, CharacterEvent.ABILITY);
    }

    public static IEnumerator Coroutine_RotateTowards(CharacterComponent caster, CharacterComponent target)
    {
        if(caster == null || target == null)
        {
            yield break;
        }

        float currentTime = 0;
        float duration = 0.15f;

        Quaternion casterRotation = caster.transform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(target.GetWorldLocation() - caster.GetWorldLocation());

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            caster.transform.rotation = Quaternion.Slerp(casterRotation, targetRotation, currentTime / duration);
            caster.transform.eulerAngles = new Vector3(0, caster.transform.eulerAngles.y, 0);
            yield return null;
        }

        caster.transform.rotation = targetRotation;
    }

    public static IEnumerator Coroutine_HandleDamage(CharacterComponent target, DamageInfo damageInfo)
    {
        target.HandleEvent(damageInfo, CharacterEvent.DAMAGE);
        yield return null;
    }
}
