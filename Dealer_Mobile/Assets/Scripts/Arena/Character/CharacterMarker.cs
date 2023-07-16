using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class CharacterMarker : MonoBehaviour
{
    private GameObject _model;
    private CharacterDecal _decal;
    private CharacterConstants.Team _team;
    private CharacterMarkerData _data;

    public void SetupMarker(CharacterConstants.Team team, CharacterMarkerData data)
    {
        _team = team;
        _data = data;
    }

    public void PerformSpawn()
    {
        StartCoroutine(Coroutine_PerformSpawn());
    }

    private IEnumerator Coroutine_PerformSpawn()
    {
        _model = Instantiate(ArenaPrefabHelper.GetCharacterModelByTeam(_team, _data.type), this.transform);

        GameObject decalPrefab = Instantiate(ArenaPrefabHelper.GetCharacterDecal(), this.transform);

        if (decalPrefab != null)
        {
            _decal = decalPrefab.GetComponent<CharacterDecal>();

            if (_decal != null)
            {
                _decal.SetColorByTeam(_team);
            }
        }
        yield return null;
    }

    private void OnMouseDown()
    {
        Debug.Log("Marker clicked");
    }
}
