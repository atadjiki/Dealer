using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class CharacterMarker : MonoBehaviour
{
    [SerializeField] private MeshRenderer PlateMesh;

    public void SetTeam(Enumerations.Team team)
    {
        PlateMesh.material = MaterialManager.Instance.GetMaterialByTeam(team);
    }

    private void OnMouseDown()
    {
        Debug.Log("clicked " + this.name);
    }
}
