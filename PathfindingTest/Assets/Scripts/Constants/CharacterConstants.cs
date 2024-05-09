using System;
using System.Collections.Generic;
using UnityEngine;

public static partial class Constants
{
    public enum CharacterID
    {
        Debugbert, //the default blue skeleton from Mixamo!
    }

    public enum CharacterAnim
    {
        Idle, 
        Moving
    }

    public static string GetAnimString(CharacterAnim anim)
    {
        return anim.ToString();
    }
}