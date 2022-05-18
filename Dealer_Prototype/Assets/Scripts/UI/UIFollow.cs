using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class UIFollow : MonoBehaviour
{
    public Vector2 offset;
    private GameObject target;

    [SerializeField] private Camera UICamera;

    private void Awake()
    {
        target = null;
    }

    public void SetTarget(GameObject _inTarget)
    {
        target = _inTarget;
        this.gameObject.SetActive(target != null);

        if(target != null)
            Debug.Log("Target = " + target.name);
    }

    private void FixedUpdate()
    {
        if(target != null)
        {

            Vector3 screenPoint = UICamera.WorldToScreenPoint(target.transform.position);

            this.transform.position = screenPoint;
        }
    }
}