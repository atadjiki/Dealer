using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class CharacterComponent : MonoBehaviour
{
    public void PerformSpawn(CharacterSpawnData data)
    {
        if (data.Marker == null)
        {
            Debug.Log("Cannot spawn character, marker is null");
        }

        CharacterConstants.ModelID modelID = CharacterConstants.GetModelID(data.ClassID, data.Type, team);
        GameObject characterModel = Instantiate(PrefabHelper.GetCharacterModel(modelID), data.Marker.transform);

        CharacterWeaponAnchor anchor = characterModel.GetComponentInChildren<CharacterWeaponAnchor>();

        if (anchor != null)
        {
            CharacterConstants.WeaponID weapon = CharacterConstants.GetWeapon(data.ClassID, team);
            GameObject characterWeapon = Instantiate(PrefabHelper.GetWeaponByID(weapon), anchor.transform);
        }
        else
        {
            Debug.Log("Could not attach weapon, no anchor found on character");
        }

        GameObject decalPrefab = Instantiate(PrefabHelper.GetCharacterDecal(), data.Marker.transform);

        if (decalPrefab != null)
        {
            CharacterDecal decal = decalPrefab.GetComponent<CharacterDecal>();

            if (decal != null)
            {
                decal.SetColorByTeam(team);
            }
        }

        CharacterAnimator animator = characterModel.GetComponent<CharacterAnimator>();
        if (animator != null)
        {
            animator.Setup(data, team, AnimationConstants.State.Idle);
        }
    }
}
