using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class CharacterHelper : MonoBehaviour
{
    public static void PerformSpawn(CharacterData data, CharacterConstants.Team team, Transform lookAt)
    {
        if (data.marker == null)
        {
            Debug.Log("Cannot spawn character, marker is null");
        }

        GameObject characterModel = Instantiate(PrefabHelper.GetCharacterModelByTeam(team, data.type), data.marker.transform);

        CharacterWeaponAnchor anchor = characterModel.GetComponentInChildren<CharacterWeaponAnchor>();

        if (anchor != null)
        {
            GameObject characterWeapon = Instantiate(PrefabHelper.GetWeaponByID(data.weapon), anchor.transform);
        }
        else
        {
            Debug.Log("Could not attach weapon, no anchor found on character");
        }

        characterModel.transform.LookAt(lookAt.position);

        GameObject decalPrefab = Instantiate(PrefabHelper.GetCharacterDecal(), data.marker.transform);

        if (decalPrefab != null)
        {
            CharacterDecal decal = decalPrefab.GetComponent<CharacterDecal>();

            if (decal != null)
            {
                decal.SetColorByTeam(team);
            }
        }

        CharacterAnimator animator = characterModel.GetComponent<CharacterAnimator>();
        if(animator != null)
        {
            animator.Setup(data, AnimationConstants.State.Idle);
        }
    }
}
