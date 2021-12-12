using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class NavPoint : MonoBehaviour
{

    public static float CountdownSeconds = 10.0f;

    private void Awake()
    {
       StartCoroutine(Countdown());

        ColorConstants.SetObjectToColor(this.gameObject, ColorConstants.NavPoint);
    }

    public IEnumerator Countdown()
    {
        yield return new WaitForSeconds(CountdownSeconds);

        Destroy(this.gameObject);
    }
}
