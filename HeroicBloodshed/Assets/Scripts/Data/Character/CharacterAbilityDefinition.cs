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

        CharacterDefinition characterDef = CharacterDefinition.Get(caster.GetID());
        WeaponDefinition weaponDef = WeaponDefinition.Get(caster.GetWeaponID());
        WeaponAttackDefinition attackDef = WeaponAttackDefinition.Get(caster.GetWeaponID());

        target.ToggleHighlight(false);
        //calculate damage
        bool crit = characterDef.RollCritChance();
        DamageInfo damageInfo = weaponDef.CalculateDamage(crit);
        damageInfo.caster = caster;
        damageInfo.target = target;

        if (crit)
        {
            EncounterManager.Instance.RequestEventBanner("Critical Hit!", _waitTime);
        }

        yield return Coroutine_HandleDamage(target, damageInfo);

        for(int i = 0; i < attackDef.CalculateShotCount(); i++)
        {
            yield return Coroutine_FireWeaponAt(caster, target);
            yield return new WaitForSecondsRealtime(attackDef.TimeBetweenShots);
        }

        if (target.IsDead())
        {
            EncounterManager.Instance.RequestEventBanner(GetDisplayString(target.GetID()) + " killed!", _waitTime * 2);
            EncounterManager.Instance.FollowCharacter(target);
            yield return new WaitForSeconds(_waitTime * 2);
        }
        else
        {
            EncounterManager.Instance.RequestEventBanner(damageInfo.ActualDamage + " Damage!", _waitTime);
            yield return new WaitForSeconds(_waitTime);

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

    public static IEnumerator Coroutine_FireWeaponAt(CharacterComponent caster, CharacterComponent target)
    {
        //rotate and pause momentarily
        yield return Coroutine_RotateTowards(caster, target);
        yield return new WaitForSeconds(_waitTime);
        caster.HandleEvent(AbilityID.Attack, CharacterEvent.ABILITY);
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
