using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using UnityEngine.EventSystems;
using static Constants;

public class EnvironmentUtil : MonoBehaviour
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

    public static bool IsWithinCharacterRange(int distance, CharacterComponent character, out MovementRangeType rangeType)
    {
        int threshold;

        int AP = character.GetActionPoints();
        int movementRange = character.GetMovementRange();

        if (AP >= GetAbilityCost(AbilityID.MoveFull))
        {
            threshold = (movementRange * 2);
        }
        else
        {
            threshold = movementRange;
        }

        if (distance <= threshold)
        {
            if (distance <= character.GetMovementRange())
            {
                rangeType = MovementRangeType.Half;
            }
            else if(AP > 1)
            {
                rangeType = MovementRangeType.Full;
            }
            else
            {
                rangeType = MovementRangeType.Half;
            }

            return true;
        }

        rangeType = MovementRangeType.None;
        return false;
    }

    public static bool CheckIsMouseBlocked()
    {
        if (Camera.main == null)
        {
            return true;
        }

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }

        return false;
    }
}
