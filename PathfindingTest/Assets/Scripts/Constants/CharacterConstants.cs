using System;
using System.Collections.Generic;
using UnityEngine;

public static partial class Constants
{
    public enum CharacterAnim
    {
        Idle, 
        Running
    }

    public static string GetAnimString(CharacterAnim anim)
    {
        return anim.ToString();
    }
}