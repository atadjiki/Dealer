using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Constants;

public class UIUtility : MonoBehaviour
{
    public static TransitionPanel RequestFadeToBlack(float transitionTime)
    {
        GameObject gameObject = Instantiate<GameObject>(PrefabLibrary.GetTransitionCanvas(), null);

        TransitionPanel panel = gameObject.GetComponent<TransitionPanel>();

        panel.Toggle(true, transitionTime);

        return panel;
    }

    public static void RequestFadeFromBlack(float transitionTime)
    {
        GameObject gameObject = Instantiate<GameObject>(PrefabLibrary.GetTransitionCanvas(), null);

        TransitionPanel panel = gameObject.GetComponent<TransitionPanel>();

        panel.Toggle(true, 0);

        panel.ToggleAndDestroy(false, transitionTime);
    }

    public static void RequestLoadingScreen()
    {
        GameObject gameObject = Instantiate<GameObject>(PrefabLibrary.GetLoadingScreen(), null);
    }

    public static void RefreshUI
    {

    }
}
