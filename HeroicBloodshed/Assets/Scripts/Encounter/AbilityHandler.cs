using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class AbilityHandler : MonoBehaviour
{
    public static IEnumerator HandleAbility_Attack(CharacterComponent caster, CharacterComponent target)
    {
        yield return Coroutine_RotateTowards(target, caster);

        CharacterDefinition characterDef = CharacterDefinition.Get(caster.GetID());
        WeaponDefinition weaponDef = WeaponDefinition.Get(caster.GetWeaponID());

        target.ToggleHighlight(false);
        //calculate damage
        bool crit = characterDef.RollCritChance();
        DamageInfo damageInfo = weaponDef.CalculateDamage(crit);

        if (crit)
        {
            EncounterManager.Instance.RequestEventBanner("Critical Hit!", 1.0f);
        }

        yield return Coroutine_FireWeaponAt(caster, target);
        yield return Coroutine_HandleDamage(target, damageInfo);

        EncounterManager.Instance.RequestEventBanner(damageInfo.ActualDamage + " Damage!", 1.0f);

        yield return new WaitForSeconds(1.0f);
    }

    public static IEnumerator HandleAbility_Reload(CharacterComponent caster)
    {
        yield return Coroutine_PerformReload(caster);
        yield return new WaitForSeconds(1.0f);
    }

    public static IEnumerator HandleAbility_SkipTurn(CharacterComponent caster)
    {
        //maybe have some anim play here
        yield return new WaitForSeconds(1.0f);
    }

    //utility

    public static IEnumerator Coroutine_FireWeaponAt(CharacterComponent caster, CharacterComponent target)
    {
        //rotate and pause momentarily
        yield return Coroutine_RotateTowards(caster, target);
        yield return new WaitForSeconds(0.5f);
        caster.PerformAttack();
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

    public static IEnumerator Coroutine_PerformReload(CharacterComponent caster)
    {
        caster.PerformReload();
        yield return null;
    }

    public static IEnumerator Coroutine_HandleDamage(CharacterComponent target, DamageInfo damageInfo)
    {
        //calculate how much damage we should deal
        int health = target.GetHealth();

        health -= Mathf.Abs(damageInfo.ActualDamage);

        health = Mathf.Clamp(health, 0, health);

        target.SetHealth(health);

        if (target.IsDead())
        {
            target.Kill();
        }
        else
        {
            target.PerformDamageHit(damageInfo);
        }
        yield return null;
    }
}
