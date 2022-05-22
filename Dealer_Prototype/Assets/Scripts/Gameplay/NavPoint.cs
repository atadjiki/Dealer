using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class NavPoint : MonoBehaviour
{

    public static float CountdownSeconds = 2.5f;

    private void Awake()
    {
        StartCoroutine(Countdown());

        ColorManager.Instance.SetObjectToColor(this.gameObject, ColorManager.Instance.GetNavPointColor());
    }

    public IEnumerator Countdown()
    {
        yield return new WaitForSeconds(CountdownSeconds);

        Destroy(gameObject);
    }
}
