using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TransitionPanel : MonoBehaviour
{
    [SerializeField] private Image image;

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

    public void Toggle(bool flag, float time)
    {
        if (flag)
        {
            StartCoroutine(LerpOpacity(1, time));
        }
        else
        {
            StartCoroutine(LerpOpacity(0, time));
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

    private IEnumerator LerpOpacity(float finalOpacity, float time)
    {
        float initialOpacity = image.color.a;
        Color initialColor = image.color;

        float currentTime = 0;

        while (currentTime < time)
        {
            float currentOpacity = Mathf.Lerp(initialOpacity, finalOpacity, currentTime / time);

            image.color = new Color(initialColor.r, initialColor.g, initialColor.b, currentOpacity);

            currentTime += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        image.color = new Color(initialColor.r, initialColor.g, initialColor.b, finalOpacity);
    }

    public void FadeInAndOut(float inTime, float holdTime,float outTime)
    {
        StartCoroutine(LerpInAndOut(inTime, holdTime, outTime));
    }

    private IEnumerator LerpInAndOut(float inTime, float holdTime, float outTime)
    {
        yield return LerpOpacity(1, inTime);

        yield return new WaitForSeconds(holdTime);

        yield return LerpOpacity(0, outTime);
    }
}
