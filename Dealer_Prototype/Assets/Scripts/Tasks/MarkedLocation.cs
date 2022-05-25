using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkedLocation : MonoBehaviour
{
    public Constants.AnimationConstants.Anim LocationAnim;

    private CharacterComponent currentOccupant;
    private CharacterComponent previousOccupant;

    private void Awake()
    {
        currentOccupant = null;
        previousOccupant = null;
    }

    public void SetOccupant(CharacterComponent character)
    {
        previousOccupant = currentOccupant;
        currentOccupant = character;
    }

    public CharacterComponent GetCurrentOccupant()
    {
        return currentOccupant;
    }

    public CharacterComponent GetPreviousOccupant()
    {
        return previousOccupant;
    }

    public void Reset()
    {
        currentOccupant = null;
        previousOccupant = null; 
    }
}
