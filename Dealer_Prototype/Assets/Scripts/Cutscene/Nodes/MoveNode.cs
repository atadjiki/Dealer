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
    [SerializeField] private bool checkMaxTime = false;
    [SerializeField] private float maxTime;


    private bool _destinationReached;
    private bool _maxTimeExceeded;
    private bool _minTimeExceeded;

    [Space]
    [SerializeField] private CutsceneNode _next;

    private CutsceneCharacterComponent characterComponent;

    public override void Setup(Cutscene cutscene, Action OnComplete)
    {
        base.Setup(cutscene, OnComplete);

        characterComponent = cutscene.FindCharacter(CharacterID);

        if (characterComponent != null)
        {
            characterComponent.OnDestinationReached += OnDestinationReached;

            StartCoroutine(NodeUpdate());
        }
    }

    private IEnumerator NodeUpdate()
    {
        characterComponent.GoTo(Location.position, Location.rotation);

        if (checkMaxTime == false)
        {
            while (_destinationReached == false)
            {
                yield return new WaitForEndOfFrame();
            }
        }
        else if(checkMaxTime)
        {
            float currentTime = 0;

            while(currentTime < maxTime)
            {
                currentTime += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
        }

        CompleteNode();
    }

    private void OnDestinationReached()
    {
        _destinationReached = true;
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
