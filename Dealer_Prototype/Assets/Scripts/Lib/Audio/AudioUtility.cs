using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioUtility : MonoBehaviour
{
    private static string Path_ButtonClick = "Audio/SFX/sfx_ui_click";

    private static string Path_DoorOpen = "Audio/SFX/sfx_door_open";

    private static string Path_DoorClose= "Audio/SFX/sfx_door_close";

    public static void ButtonClick()
    {
        PlayClip(Path_ButtonClick, 0.85f);
    }

    public static void DoorOpen()
    {
        PlayClip(Path_DoorOpen, 0.85f);
    }

    public static void DoorClose()
    {
        PlayClip(Path_DoorClose, 0.85f);
    }

    private static void PlayClip(string path, float volume)
    {
        AudioClip clip = Instantiate<AudioClip>(Resources.Load<AudioClip>(path));
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, volume);
    }
}
