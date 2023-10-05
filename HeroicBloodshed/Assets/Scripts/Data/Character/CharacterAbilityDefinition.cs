using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class AbilityHandler : MonoBehaviour
{
    private static float _waitTime = 0.5f;

    public static IEnumerator Coroutine_HandleAbility_Attack(CharacterComponent caster, CharacterComponent target)
    {
        yield return Coroutine_RotateTowards(target, caster);

        WeaponDefinition weaponDef = WeaponDefinition.Get(caster.GetWeaponID());
        WeaponAttackDefinition attackDef = WeaponAttackDefinition.Get(caster.GetWeaponID());

        target.ToggleHighlight(false);
        DamageInfo damageInfo = weaponDef.CalculateDamage(caster, target);

        if (damageInfo.IsCrit)
        {
            EncounterManager.Instance.RequestEventBanner("Critical Hit!", _waitTime);
        }

        yield return Coroutine_HandleDamage(target, damageInfo);

        //if kill, only fire once
        if (damageInfo.IsKill)
        {
            yield return Coroutine_FireWeaponAt(damageInfo);
            target.HandleEvent(damageInfo, CharacterEvent.KILLED);
            EncounterManager.Instance.RequestEventBanner(GetDisplayString(target.GetID()) + " killed!", _waitTime * 2);
            EncounterManager.Instance.FollowCharacter(target);
            yield return new WaitForSeconds(_waitTime * 2);
        }
        else
        {
            EncounterManager.Instance.RequestEventBanner(damageInfo.ActualDamage + " Damage!", _waitTime);

            int shotCount = attackDef.CalculateShotCount();

            for (int i = 0; i < shotCount; i++)
            {
                yield return Coroutine_FireWeaponAt(damageInfo);

                if(i == (shotCount/2))
                {
                    target.HandleEvent(damageInfo, CharacterEvent.HIT);
                }

                yield return new WaitForSecondsRealtime(attackDef.TimeBetweenShots);
            }
        }
        yield return new WaitForSeconds(_waitTime);
    }

    public static IEnumerator Coroutine_HandleAbility_Reload(CharacterComponent caster)
    {
        caster.HandleEvent(AbilityID.Reload, CharacterEvent.ABILITY);
        yield return new WaitForSeconds(_waitTime);
    }

    public static IEnumerator HandleAbility_SkipTurn(CharacterComponent caster)
    {
        //maybe have some anim play here
        caster.HandleEvent(AbilityID.SkipTurn, CharacterEvent.ABILITY);
        yield return new WaitForSeconds(_waitTime * 2);
    }

    //utility

    public static IEnumerator Coroutine_FireWeaponAt(DamageInfo damageInfo)
    {
        //rotate and pause momentarily
        yield return Coroutine_RotateTowards(damageInfo.caster, damageInfo.target);
        damageInfo.caster.HandleEvent(AbilityID.Attack, CharacterEvent.ABILITY);
    }

    public static IEnumerator Coroutine_RotateTowards(CharacterComponent caster, CharacterComponent target)
    {
        float currentTime = 0;
        float duration = 0.15f;

        Quaternion casterRotation = caster.transform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - caster.transform.position);

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
