using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialManager : Manager
{ 
    private static MaterialManager _instance;

    public static MaterialManager Instance { get { return _instance; } }

    public override void Build()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        base.Build();
    }

    [Header("Character Materials")]
    [SerializeField] private Material OutlineMaterial;

    public Material GetOutlineMaterial() { return OutlineMaterial; }

    public void ApplyOutlineMaterialToMesh(MeshRenderer renderer)
    {
        if (renderer != null)
        {
            List<Material> materials = new List<Material>(renderer.materials);

            if (materials.Count == 1)
            {
                materials.Add(OutlineMaterial);

                renderer.materials = materials.ToArray();
            }
        }
    }

    public void RemoveOutlineMaterialFromMesh(MeshRenderer renderer)
    {
        if (renderer != null)
        {
            List<Material> materials = new List<Material>(renderer.materials);

            if (materials.Count == 2)
            {
                materials.RemoveAt(1);

                renderer.materials = materials.ToArray();
            }
        }
    }

    public void ApplyOutlineMaterialToMesh(SkinnedMeshRenderer renderer)
    {
        if (renderer != null)
        {
            List<Material> materials = new List<Material>(renderer.materials);

            if (materials.Count == 3)
            {
                materials.Add(OutlineMaterial);

                renderer.materials = materials.ToArray();
            }
        }
    }

    public void RemoveOutlineMaterialFromMesh(SkinnedMeshRenderer renderer)
    {
        if (renderer != null)
        {
            List<Material> materials = new List<Material>(renderer.materials);

            if (materials.Count == 4)
            {
                materials.RemoveAt(3);

                renderer.materials = materials.ToArray();
            }
        }
    }
}
