using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class FlickeringLight : MonoBehaviour
{
    private Light _light;

    public int loops = 5;

    private int _currentLoops = 0;

    private void Awake()
    {
        _light = GetComponent<Light>();

        StartCoroutine(PerformFlicker());
    }

    private IEnumerator PerformFlicker()
    {
        while(_currentLoops <= loops)
        {
            float delayTime = Random.Range(1, 3);

            yield return new WaitForSeconds(delayTime);

            _light.enabled = false;

            yield return new WaitForSeconds(Random.Range(0.05f, 0.1f));

            _light.enabled = true;

            yield return new WaitForSeconds(Random.Range(0.05f, 0.1f));

            _light.enabled = false;

            yield return new WaitForSeconds(Random.Range(0.05f, 0.1f));

            _light.enabled = true;

            delayTime = Random.Range(1, 2);

            yield return new WaitForSeconds(delayTime);

            yield return new WaitForSeconds(Random.Range(0.05f, 0.1f));

            _light.enabled = true;

            yield return new WaitForSeconds(Random.Range(0.05f, 0.1f));

            _light.enabled = false;

            yield return new WaitForSeconds(delayTime);

            _light.enabled = true;

            _currentLoops++;
        }
    }
}
