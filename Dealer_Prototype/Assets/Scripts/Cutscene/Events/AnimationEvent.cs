using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

[System.Serializable]
public struct AnimEventPair
{
    public CharacterModel Model;
    public Animations.ID Anim;
}

[System.Serializable]
public class AnimationEvent : CutsceneEvent
{
    public List<AnimEventPair> Data;
}
