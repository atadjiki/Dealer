using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class CharacterMarker : MonoBehaviour
{
    //what team am i on?
    [SerializeField] private CharacterConstants.Team team;

    [SerializeField] private CharacterConstants.CharacterType type;

    //what weapon do i have?
    [SerializeField] private CharacterConstants.Weapon weapon;

    //privates
    private GameObject _model;
    private CharacterDecal _decal;


    private void Awake()
    {
        _model = Instantiate(ArenaPrefabHelper.GetCharacterModelByTeam(team, type), this.transform);

        GameObject decalPrefab = Instantiate(ArenaPrefabHelper.GetCharacterDecal(), this.transform);

        if (decalPrefab != null)
        {
            _decal = decalPrefab.GetComponent<CharacterDecal>();

            if (_decal != null)
            {
                _decal.SetColorByTeam(team);
            }
        }

    }

    private void OnMouseDown()
    {
        Debug.Log("Marker clicked");
    }
}
