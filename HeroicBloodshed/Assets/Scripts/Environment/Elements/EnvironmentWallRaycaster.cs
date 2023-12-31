using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class EnvironmentWallRaycaster : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Camera.main == null) return;

        Vector3 origin = Camera.main.transform.position;
        Vector3 destination = this.transform.position;

        RaycastHit hitinfo;

        if (Physics.Linecast(origin, destination, out hitinfo, LayerMask.GetMask(LAYER_ENV_WALL)))
        {
            EnvironmentTransparencyComponent transparency = hitinfo.transform.parent.gameObject.GetComponent<EnvironmentTransparencyComponent>();

            if(transparency != null)
            {
                transparency.ForceTransparency();
            }
        }
    }
}
