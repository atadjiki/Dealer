using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class EncounterUITargetSelect : EncounterUIElement
{
    [SerializeField] protected Transform Container;

    [SerializeField] protected GameObject Prefab_Item;

    public override void Populate(EncounterModel model)
    {
        //add a portrait for each character in the enemy team
        foreach (CharacterComponent character in model.GetAllCharactersInTeam(TeamID.Enemy))
        {
            StartCoroutine(GenerateTargetButton(character));
        }
    }

    private IEnumerator GenerateTargetButton(CharacterComponent character)
    {
        yield return null;
    }
}
