using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class EnvironmentWall : MonoBehaviour
{
    [SerializeField] protected GameObject Mesh_Default;
    [SerializeField] protected GameObject Mesh_Alternate;

    [SerializeField] private bool CheckOverlap = true;

    private BoxCollider _collider;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();   
    }
    private void Update()
    {
        if (CheckOverlap_Mouse()) return;
        CheckOverlap_Camera();
    }
    private bool CheckOverlap_Mouse()
    {
        if (CheckOverlap == false) return false;
        if (Camera.main == null) return false;
        if (_collider == null) return false;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, LayerMask.GetMask("EnvironmentWall")))
        {
            if (hit.collider != null && hit.collider == _collider)
            {
                ToggleVisibility(false);
                return true;
            }
        }

        ToggleVisibility(true);
        return false;
    }

    private void CheckOverlap_Camera()
    {
        if (CheckOverlap == false) return;
        if (Camera.main == null) return;
        if (_collider == null) return;

        Vector3 origin = Camera.main.transform.position;
        Vector3 direction = Camera.main.transform.forward;

        Ray ray = new Ray(origin, direction);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, LayerMask.GetMask("EnvironmentWall")))
        {
            if (hit.collider != null && hit.collider == _collider)
            {
                ToggleVisibility(false);
                return;
            }
        }

        ToggleVisibility(true);
    }

    private void ToggleVisibility(bool flag)
    {
        if (flag)
        {
            Mesh_Default.SetActive(true);
            Mesh_Alternate.SetActive(false);
        }
        else
        {
            Mesh_Default.SetActive(false);
            Mesh_Alternate.SetActive(true);
        }
    }


}
