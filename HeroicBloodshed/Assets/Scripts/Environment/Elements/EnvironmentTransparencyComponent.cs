using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentTransparencyComponent : MonoBehaviour
{
    [Header("Materials")]
    [SerializeField] private Material Mat_Opaque;
    [SerializeField] private Material Mat_Transparent;

    private List<MeshRenderer> _renderers;
    private AnimationCurve _curve;

    private float MinFadeDistance = 0;
    private float MaxFadeDistance = 20; //if the camera is farther away than this, opacity is 100%

    private bool forceTransparent = false;

    private void Awake()
    {
        _renderers = new List<MeshRenderer>(GetComponentsInChildren<MeshRenderer>());

        _curve = AnimationCurve.EaseInOut(0.25f, 0.25f, 1, 1);
    }

    private void Update()
    {
        if (Camera.main == null) return;
        if (_renderers == null) return;

        if(forceTransparent)
        {
            SetOpacity(0.5f);
        }
        else
        {
            Vector3 cameraPos = Camera.main.transform.position;

            float distanceToCamera = Vector3.Distance(cameraPos, this.transform.position);

            float fadePercentage = Mathf.InverseLerp(MinFadeDistance, MaxFadeDistance, distanceToCamera);

            SetOpacity(_curve.Evaluate(fadePercentage));
        }

        forceTransparent = false;
    }

    private void SetOpacity(float opacity)
    {
        if(opacity > 0.5f)
        {
            if (Mat_Opaque != null)
            {
                foreach (MeshRenderer renderer in _renderers)
                {
                    if (renderer.material != null)
                    {
                        renderer.material = Mat_Opaque;
                    }
                }
            }
        }
        else
        {
            if (Mat_Transparent != null)
            {
                foreach (MeshRenderer renderer in _renderers)
                {
                    if (renderer.material != null)
                    {
                        renderer.material = Mat_Transparent;

                        Color color = renderer.material.color;

                        color.a = opacity;

                        renderer.material.color = color;
                    }
                }
            }
        }
    }

    public void ForceTransparency()
    {
        forceTransparent = true;
    }
}
