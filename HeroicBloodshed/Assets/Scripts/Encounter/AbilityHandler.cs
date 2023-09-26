using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class AbilityHandler : MonoBehaviour
{
    public static float defaultWaitTime = 0.75f;
    public static IEnumerator HandleAbility_Attack(CharacterComponent caster, CharacterComponent target)
    {
        yield return Coroutine_RotateTowards(target, caster);

        CharacterDefinition characterDef = CharacterDefinition.Get(caster.GetID());
        WeaponDefinition weaponDef = WeaponDefinition.Get(caster.GetWeaponID());

        target.ToggleHighlight(false);
        //calculate damage
        bool crit = characterDef.RollCritChance();
        DamageInfo damageInfo = weaponDef.CalculateDamage(crit);
        damageInfo.caster = caster;
        damageInfo.target = target;

        if (crit)
        {
            EncounterManager.Instance.RequestEventBanner("Critical Hit!", defaultWaitTime);
        }

        yield return Coroutine_FireWeaponAt(caster, target);
        yield return Coroutine_HandleDamage(target, damageInfo);

        if (target.IsDead())
        {
            EncounterManager.Instance.RequestEventBanner(GetDisplayString(target.GetID()) + " killed!", defaultWaitTime * 2);
            EncounterManager.Instance.FollowCharacter(target);
            yield return new WaitForSeconds(defaultWaitTime * 2);
        }
        else
        {
            EncounterManager.Instance.RequestEventBanner(damageInfo.ActualDamage + " Damage!", defaultWaitTime);
            yield return new WaitForSeconds(defaultWaitTime);

        }
        yield return new WaitForSeconds(defaultWaitTime);
    }

    public static IEnumerator HandleAbility_Reload(CharacterComponent caster)
    {
        caster.HandleEvent(AbilityID.Reload, CharacterEvent.ABILITY);
        yield return new WaitForSeconds(defaultWaitTime);
    }

    public static IEnumerator HandleAbility_SkipTurn(CharacterComponent caster)
    {
        //maybe have some anim play here
        caster.HandleEvent(AbilityID.SkipTurn, CharacterEvent.ABILITY);
        yield return new WaitForSeconds(defaultWaitTime * 2);
    }

    //utility

    public static IEnumerator Coroutine_FireWeaponAt(CharacterComponent caster, CharacterComponent target)
    {
        //rotate and pause momentarily
        yield return Coroutine_RotateTowards(caster, target);
        yield return new WaitForSeconds(defaultWaitTime);
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
