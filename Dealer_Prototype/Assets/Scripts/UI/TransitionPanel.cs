using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TransitionPanel : MonoBehaviour
{
    [SerializeField] private Image image;

    [SerializeField] private bool panelState = false;

    [SerializeField] private float transitionTime = 2f;

    public void SetInitialState(bool flag)
    {
        if (flag)
        {
            image.color = Color.black;
        }
        else
        {
            image.color = new Color(0, 0, 0, 0);
        }
    }

    public void Toggle(bool flag)
    {
        if(flag)
        {
            StartCoroutine(LerpOpacity(1));
        }
        else
        {
            StartCoroutine(LerpOpacity(0));
        }
    }

    private IEnumerator LerpOpacity(float finalOpacity)
    {
        float initialOpacity = image.color.a;
        Color initialColor = image.color;

        float currentTime = 0;

        while(currentTime < transitionTime)
        {
            float currentOpacity = Mathf.Lerp(initialOpacity, finalOpacity, currentTime / transitionTime);

            image.color = new Color(initialColor.r, initialColor.g, initialColor.b, currentOpacity);

            currentTime += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        image.color = new Color(initialColor.r, initialColor.g, initialColor.b, finalOpacity);
    }
}
