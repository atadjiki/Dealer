using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class CharacterUtil : MonoBehaviour
{
    public static CharacterComponent BuildCharacterObject(CharacterData data, Transform parent)
    {
        GameObject characterObject = new GameObject(data.ID.ToString());
        characterObject.transform.parent = parent;

        CharacterComponent characterComponent = characterObject.AddComponent<CharacterComponent>();

        return characterComponent;
    }
}
