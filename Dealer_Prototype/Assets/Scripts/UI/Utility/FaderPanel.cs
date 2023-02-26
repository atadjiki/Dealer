using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FaderPanel : MonoBehaviour
{
    private Image image;
    private float transitionTime = 1.0f;

    private void Awake()
    {
        image = GetComponent<Image>();

        image.color = Color.black;

        PerformFade();
        
    }

    public void PerformFade()
    {
        StartCoroutine(Coroutine_PerformFade());
    }

    private IEnumerator Coroutine_PerformFade()
    {

        yield return new WaitForSeconds(1.0f);

        float initialOpacity = image.color.a;
        Color initialColor = image.color;

        float currentTime = 0;

        while (currentTime < transitionTime)
        {
            float currentOpacity = Mathf.Lerp(initialOpacity, 0, currentTime / transitionTime);

            image.color = new Color(initialColor.r, initialColor.g, initialColor.b, currentOpacity);

            currentTime += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        image.color = new Color(initialColor.r, initialColor.g, initialColor.b, 0);
    }
}
