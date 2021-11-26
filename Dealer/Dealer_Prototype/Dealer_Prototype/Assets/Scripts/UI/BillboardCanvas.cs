using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardCanvas : MonoBehaviour
{
    private Quaternion originalRotation;
    private Canvas _canvas;

    void Awake()
    {
        originalRotation = transform.rotation;

        _canvas = GetComponent<Canvas>();
        _canvas.worldCamera = Camera.main;
    }

    private void Update()
    {
        UpdateRotation();
    }

    public void UpdateRotation()
    {
        transform.LookAt(Camera.main.transform);
    }

    public void Toggle(bool flag)
    {
        this.gameObject.SetActive(flag);
    }
}
