using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class CharacterModel : MonoBehaviour
{
    [SerializeField] private GameObject MeshGroup_Main;

    private CapsuleCollider _collider;

    public void ToggleModel(bool flag)
    {
        MeshGroup_Main.SetActive(flag);
    }
}
