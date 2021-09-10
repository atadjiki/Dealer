using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationConstants : MonoBehaviour
{
    public enum Animations
    {
        ButtonPush, Carrying, Defeat, HandsForward, Idle, Leaning, Male_Sitting_1,
        Male_Sitting_2, Male_Sitting_Relaxed, Running, Talking, ThoughtfulHeadNod, Walking
    }

    public static string ButtonPush = "ButtonPush";
    public static string Carrying = "Carrying";
    public static string Defeat = "Defeat";
    public static string HandsForward = "HandsForward";
    public static string Idle = "Idle";
    public static string Leaning = "Leaning";
    public static string Male_Sitting_1 = "Male_Sitting_1";
    public static string Male_Sitting_2 = "Male_Sitting_2";
    public static string Male_Sitting_Relaxed = "Male_Sitting_Relaxed";
    public static string Running = "Running";
    public static string Talking = "Talking";
    public static string ThoughtfulHeadNod = "ThoughtfulHeadNod";
    public static string Walking = "Walking";

    public static string GetAnimByEnum(Animations anim)
    {
        if (anim == Animations.ButtonPush)
        {
            return ButtonPush;
        }
        else if (anim == Animations.Carrying)
        {
            return Carrying;
        }
        else if (anim == Animations.Defeat)
        {
            return Defeat;
        }
        else if (anim == Animations.HandsForward)
        {
            return HandsForward;
        }
        else if (anim == Animations.Idle)
        {
            return Idle;
        }
        else if (anim == Animations.Leaning)
        {
            return Leaning;
        }
        else if (anim == Animations.Male_Sitting_1)
        {
            return Male_Sitting_1;
        }
        else if (anim == Animations.Male_Sitting_2)
        {
            return Male_Sitting_2;
        }
        else if (anim == Animations.Male_Sitting_Relaxed)
        {
            return Male_Sitting_Relaxed;
        }
        else if (anim == Animations.Running)
        {
            return Running;
        }
        else if (anim == Animations.Talking)
        {
            return Talking;
        }
        else if (anim == Animations.ThoughtfulHeadNod)
        {
            return ThoughtfulHeadNod;
        }
        else if (anim == Animations.Walking)
        {
            return Walking;
        }
        else
        {
            return Idle;
        }
    }
}
