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
        Reset();
    }

    public void SetOccupant(CharacterComponent character)
    {
        previousOccupant = currentOccupant;
        currentOccupant = character;
    }

    public void Reset()
    {
        currentOccupant = null;
        previousOccupant = null; 
    }


    public CharacterComponent GetCurrentOccupant() { return currentOccupant; }

    public CharacterComponent GetPreviousOccupant() { return previousOccupant; }
}
