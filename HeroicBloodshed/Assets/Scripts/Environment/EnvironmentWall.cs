using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class EnvironmentWall : MonoBehaviour
{
    private List<MeshRenderer> _renderers;

    private float fadeDistance = 10; //if the camera is farther away than this, opacity is 100%

    private void Awake()
    {
        _renderers = new List<MeshRenderer>(GetComponentsInChildren<MeshRenderer>());
    }

    private void Update()
    {
        if (Camera.main == null) return;

        Vector3 cameraPos = Camera.main.transform.position;

        float distanceToCamera = Vector3.Distance(cameraPos, this.transform.position);

        if(distanceToCamera < fadeDistance)
        {
            float fadePercentage = distanceToCamera / fadeDistance;

            foreach(MeshRenderer renderer in _renderers)
            {
                if(renderer.material != null)
                {
                    Color color = renderer.material.color;

                    color.a = fadePercentage;

                    renderer.material.color = color;
                }
            }
        }
    }
}
