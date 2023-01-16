using System;
using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class MoveNode : CutsceneNode
{
    [Space]
    [SerializeField] private Enumerations.CharacterID CharacterID;
    [SerializeField] private Transform Location;

    [Space]
    [SerializeField] private CutsceneNode _next;

    private CutsceneCharacterComponent characterComponent;

    public override void Setup(Cutscene cutscene, Action OnComplete)
    {
        base.Setup(cutscene, OnComplete);

        characterComponent = cutscene.FindCharacter(CharacterID);

        if (characterComponent != null)
        {
            characterComponent.OnDestinationReached += CompleteNode;
            characterComponent.GoTo(Location.position);
        }
    }

    public override CutsceneNode GetNext()
    {
        return _next;
    }

    private void OnDestroy()
    {
        if(characterComponent != null)
        {
            characterComponent.OnDestinationReached -= CompleteNode;
        }
    }
}
