using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class SelectionComponent : MonoBehaviour
{
    [SerializeField] private Color Clear;
    [SerializeField] private Color Unpossesed;
    [SerializeField] private Color Possesed;

    private MeshRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
        SetClear();
    }

    public void Toggle(bool flag)
    {
        this.gameObject.SetActive(flag);
    }

    public void SetPossesed()
    {
        _renderer.material.color = Possesed;
    }

    public void SetUnposessed()
    {
        _renderer.material.color = Unpossesed;
    }

    public void SetClear()
    {
        _renderer.material.color = Clear;
    }
}
