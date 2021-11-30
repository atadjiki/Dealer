using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavPoint : MonoBehaviour
{

    public static float CountdownSeconds = 10.0f;

    private void Awake()
    {
       StartCoroutine(Countdown());
    }

    public IEnumerator Countdown()
    {
        yield return new WaitForSeconds(CountdownSeconds);

        Destroy(this.gameObject);
    }
}
