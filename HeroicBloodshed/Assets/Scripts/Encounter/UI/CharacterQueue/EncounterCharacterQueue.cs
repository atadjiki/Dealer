using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class EncounterCharacterQueue : EncounterCanvasItemContainer
{
    [SerializeField] protected TeamID _team = TeamID.None;

    public override void Populate(EncounterModel model)
    {
        //add a portrait for each character in the player queue
        foreach (CharacterComponent character in model.GetAllCharactersInTeam(_team))
        {
            GameObject queueItemObject = Instantiate(Prefab_Item, Container);
            EncounterCharacterQueueItem characterQueueItem = queueItemObject.GetComponent<EncounterCharacterQueueItem>();
            characterQueueItem.Setup(character);

            if (character == model.GetCurrentCharacter())
            {
                characterQueueItem.SetActive();
            }
            else if (character.IsDead())
            {
                characterQueueItem.SetDead();
            }
            else
            {
                characterQueueItem.SetInactive();
            }
        }
    }
}
