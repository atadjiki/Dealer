using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static Constants;

public class EncounterHelper
{
    public static CharacterComponent AddComponentByTeam(CharacterID characterID, GameObject characterObject)
    {
        TeamID teamID = GetTeamByID(characterID);

        switch (teamID)
        {
            case TeamID.Player:
                PlayerCharacterComponent playerCharacterComponent = characterObject.AddComponent<PlayerCharacterComponent>();
                playerCharacterComponent.SetID(characterID);
                return playerCharacterComponent;
            case TeamID.Enemy:
                EnemyCharacterComponent enemyCharacterComponent = characterObject.AddComponent<EnemyCharacterComponent>();
                enemyCharacterComponent.SetID(characterID);
                return enemyCharacterComponent;
            default:
                return null;
        }
    }

    public static GameObject CreateCharacterObject(string name, Transform markerTransform)
    {
        GameObject characterObject = new GameObject(name);
        characterObject.transform.parent = markerTransform;
        characterObject.transform.localPosition = Vector3.zero;
        characterObject.transform.localRotation = Quaternion.identity;
        return characterObject;
    }

    public static IEnumerator SpawnCharacters(Encounter encounter)
    {
        foreach (CharacterComponent characterComponent in encounter.GetAllCharacters())
        {
            yield return characterComponent.SpawnCharacter();
        }
    }

    public static IEnumerator DespawnCharacters(Encounter encounter)
    {
        foreach (CharacterComponent characterComponent in encounter.GetAllCharacters())
        {
            yield return characterComponent.PerformCleanup();
        }
    }
}
