using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentTransparencyComponent : MonoBehaviour
{
    private List<MeshRenderer> _renderers;
    private AnimationCurve _curve;

    private float MinFadeDistance = 0;
    private float MaxFadeDistance = 16; //if the camera is farther away than this, opacity is 100%

    private void Awake()
    {
        _renderers = new List<MeshRenderer>(GetComponentsInChildren<MeshRenderer>());

        _curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    }

    private void Update()
    {
        if (Camera.main == null) return;
        if (_renderers == null) return;

        Vector3 cameraPos = Camera.main.transform.position;

        float distanceToCamera = Vector3.Distance(cameraPos, this.transform.position);

        float fadePercentage = Mathf.InverseLerp(MinFadeDistance, MaxFadeDistance, distanceToCamera);

        float opacity = _curve.Evaluate(fadePercentage);

        foreach (MeshRenderer renderer in _renderers)
        {
            if (renderer.material != null)
            {
                Color color = renderer.material.color;

                color.a = opacity;

                renderer.material.color = color;
            }
        }
    }
}
