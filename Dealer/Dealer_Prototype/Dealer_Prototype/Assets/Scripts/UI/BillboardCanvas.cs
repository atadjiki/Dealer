using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardCanvas : MonoBehaviour
{
    private Quaternion originalRotation;
    private Transform camTransform;
    private Canvas _canvas;
    public bool _enabled = true;


    void Awake()
    {
        originalRotation = transform.rotation;
        camTransform = CameraManager.Instance.GetMainCamera().transform;

        _canvas = GetComponent<Canvas>();

        if(_canvas != null)
        {
            _canvas.worldCamera = CameraManager.Instance.GetMainCamera();
        }


    }

    private void Update()
    {
        if (_enabled)
        {
            UpdateRotation();
        }
    }

    public void UpdateRotation()
    {
        transform.rotation = camTransform.rotation * originalRotation;
    }

    public void Toggle(bool flag)
    {
        this.gameObject.SetActive(flag);
    }
}
