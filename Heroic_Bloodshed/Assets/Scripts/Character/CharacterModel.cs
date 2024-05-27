using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterModel : MonoBehaviour
{
    [SerializeField] private List<GameObject> Meshes;

    public void ToggleVisibility(bool flag)
    {
        foreach(GameObject mesh in Meshes)
        {
            mesh.SetActive(flag);
        }
    }
}
