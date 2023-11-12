using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class EnvironmentUtil : MonoBehaviour
{
    public static CharacterComponent SpawnCharacter(TeamID teamID, CharacterID characterID)
    {
        if(EnvironmentManager.Instance == null)
        {
            Debug.Log("Environment Manager is not initialized");
                return null;
        }

        //see if we have a marker available to spawn them in
        foreach (EnvironmentTile tile in EnvironmentManager.Instance.GetTilesContainingSpawnPoints(teamID))
        {
            GameObject characterObject = CreateCharacterObject(teamID + "_" + characterID, tile);
            CharacterComponent characterComponent = AddComponentByTeam(characterID, characterObject);

            return characterComponent;
        }

        return null;
    }

    private static GameObject CreateCharacterObject(string name, EnvironmentTile tile)
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

    private static CharacterComponent AddComponentByTeam(CharacterID characterID, GameObject characterObject)
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
