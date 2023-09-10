using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EncounterFadePanel : MonoBehaviour
{
    [SerializeField] private Image Panel_Fade;

    public IEnumerator Coroutine_PerformFadeToBlack()
    {
        StopCoroutine("Coroutine_PerformFadeToClear");
        yield return Coroutine_PerformFadeBetween(Color.clear, Color.black, 0.25f);
    }

    public IEnumerator Coroutine_PerformFadeToClear()
    {
        StopCoroutine("Coroutine_PerformFadeToClear");
        yield return Coroutine_PerformFadeBetween(Color.black, Color.clear, 0.25f);
    }

    private IEnumerator Coroutine_PerformFadeBetween(Color to, Color from, float duration)
    {
        Panel_Fade.color = to;

        float currentTime = 0;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            Panel_Fade.color = Color.Lerp(to, from, currentTime / duration);
            yield return null;
        }
    }
}
