using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterHelper : MonoBehaviour
{
    public static GameObject CreateCharacterObject(string name, Transform markerTransform)
    {
        GameObject characterObject = new GameObject(name);
        characterObject.transform.parent = markerTransform;
        characterObject.transform.localPosition = Vector3.zero;
        characterObject.transform.localRotation = Quaternion.identity;
        return characterObject;
    }
}
