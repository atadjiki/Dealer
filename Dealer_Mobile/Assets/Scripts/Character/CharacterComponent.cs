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

        CharacterDefinition def = CharacterDefinition.GetCharacterDefinition(data.ID);

        CharacterConstants.ModelID modelID = def.AllowedModels[0];
        GameObject characterModel = Instantiate(PrefabHelper.GetCharacterModel(modelID), data.Marker.transform);

        CharacterWeaponAnchor anchor = characterModel.GetComponentInChildren<CharacterWeaponAnchor>();

        if (anchor != null)
        {
            CharacterConstants.WeaponID weapon = def.AllowedWeapons[0];
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
                decal.SetColorByTeam(CharacterConstants.TeamID.Enemy);
            }
        }

        CharacterAnimator animator = characterModel.GetComponent<CharacterAnimator>();
        if (animator != null)
        {
            animator.Setup(AnimationConstants.State.Idle);
        }
    }
}
