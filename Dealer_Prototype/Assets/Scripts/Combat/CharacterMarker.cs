using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class CharacterMarker : MonoBehaviour
{
    [SerializeField] private MeshRenderer PlateMesh;

    private void OnMouseDown()
    {
        Debug.Log("clicked " + this.name);
    }

    public void SetTeam(Enumerations.Team team)
    {
        PlateMesh.material = MaterialManager.Instance.GetMaterialByTeam(team);
    }

    public void SetSide(Enumerations.ArenaSide side)
    {
        this.transform.localEulerAngles = CombatConstants.GetRotationBySide(side);
    }
}
