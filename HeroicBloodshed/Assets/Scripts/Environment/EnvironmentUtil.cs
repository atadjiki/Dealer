using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class EnvironmentUtil : MonoBehaviour
{
    public static GameObject CreateCharacterObject(string name, EnvironmentTile tile)
    {
        GameObject characterObject = new GameObject(name);
        characterObject.transform.localPosition = Vector3.zero;
        characterObject.transform.position = tile.transform.position;

        if(tile.ContainsSpawnPoint())
        {
            EnvironmentSpawnPoint spawnPoint = tile.GetSpawnPoint();

            characterObject.transform.localEulerAngles = spawnPoint.transform.eulerAngles;
        }

        return characterObject;
    }

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
}
