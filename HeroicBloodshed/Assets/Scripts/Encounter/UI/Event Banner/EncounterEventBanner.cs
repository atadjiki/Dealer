using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Constants;
public class EncounterEventBanner : EncounterUIElement
{
    [SerializeField] private TextMeshProUGUI Text_EventDetail;

    private void Awake()
    {
        Hide();
    }

    public void DisplayMessage(string message, float time)
    {
        StopAllCoroutines();
        StartCoroutine(Coroutine_DisplayMessage(message, time));
    }

    private IEnumerator Coroutine_DisplayMessage(string message, float time)
    {
        Text_EventDetail.text = message;
        Show();
        yield return new WaitForSeconds(time);
        Hide();
    }
}
