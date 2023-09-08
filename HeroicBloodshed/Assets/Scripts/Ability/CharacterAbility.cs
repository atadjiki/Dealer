using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class CharacterAbility : MonoBehaviour
{
    public static IEnumerator Perform(CharacterComponent caster)
    {
        AbilityID abilityID = caster.GetActiveAbility();
        //CharacterComponent target = null;

        Debug.Log(caster.GetID() + " performing ability " + abilityID.ToString());

        switch (abilityID)
        {
            case AbilityID.Attack:
                yield return HandleAbility_Attack(caster);
                break;
            case AbilityID.SkipTurn:
                yield return HandleAbility_SkipTurn(caster);
                break;
            default:
                break;
        }
    }

    private static IEnumerator HandleAbility_Attack(CharacterComponent caster)
    {
        CharacterComponent target = EncounterManager.Instance.FindTargetForCharacter(caster);

        if(target != null)
        {
            EncounterManager.Instance.FollowCharacter(target);
            yield return new WaitForSeconds(1.0f);
            target.Kill();
            yield return new WaitForSeconds(1.0f);
            EncounterManager.Instance.UnfollowCharacter();
            yield return new WaitForSeconds(1.0f);
        }
        else
        {
            Debug.Log("target is null!");
        }
        yield return null;
    }

    private static IEnumerator HandleAbility_SkipTurn(CharacterComponent caster)
    {
        yield return new WaitForSeconds(1.0f);
    }
}
