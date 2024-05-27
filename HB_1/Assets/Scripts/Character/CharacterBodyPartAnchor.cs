using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class CharacterBodyPartAnchor : MonoBehaviour
{
    [SerializeField] private BodyPartID ID;

    public BodyPartID GetID()
    {
        return ID;
    }
}
