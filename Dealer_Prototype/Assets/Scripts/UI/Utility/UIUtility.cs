using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Constants;

public class UIUtility : MonoBehaviour
{
    private static TransitionPanel _transitionPanel;

    private static void InitTransitionPanel()
    {
        GameObject gameObject = Instantiate<GameObject>(PrefabLibrary.GetTransitionCanvas(), null);

        _transitionPanel = gameObject.GetComponent<TransitionPanel>();

        _transitionPanel.SetInitialState(true);
    }

    public static void FadeToBlack(float time)
    {
        if(_transitionPanel == null)
        {
            InitTransitionPanel();
        }

        _transitionPanel.Toggle(true, time);

        Debug.Log("Fade To Black " + time + "s");
    }

    public static void FadeToTransparent(float time)
    {
        if (_transitionPanel == null)
        {
            InitTransitionPanel();
        }

        _transitionPanel.SetInitialState(true);
        _transitionPanel.Toggle(false, time);

        Debug.Log("Fade To Transparent " + time + "s");
    }

    public static void FadeInAndOut(float inTime, float holdTime, float outTime)
    {
        if (_transitionPanel == null)
        {
            InitTransitionPanel();
        }

        _transitionPanel.FadeInAndOut(inTime, holdTime, outTime);
    }
}

