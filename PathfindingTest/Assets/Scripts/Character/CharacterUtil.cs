using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class CharacterUtil : MonoBehaviour
{
    public static CharacterComponent BuildCharacterObject(CharacterDefinition data, Transform parent)
    {
        GameObject characterObject = new GameObject("Character_" + data.ID.ToString().ToLowerInvariant());
        characterObject.transform.parent = parent;

        CharacterComponent characterComponent = characterObject.AddComponent<CharacterComponent>();

        return characterComponent;
    }
}
