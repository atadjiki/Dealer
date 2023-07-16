using System;
using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class Arena : MonoBehaviour
{
    [SerializeField] private ArenaData Data;
    [SerializeField] private CameraManager CameraManager;

    [SerializeField] private CameraFollowTarget FollowTarget;

    private Transform defaultFollowTargetParent;

    private void Awake()
    {
        defaultFollowTargetParent = FollowTarget.transform.parent;

        PerformOverviewScene();
    }

    private void PerformOverviewScene()
    {
        StartCoroutine(Coroutine_PerformOverviewScene());
    }

    private IEnumerator Coroutine_PerformOverviewScene()
    {
        PopulateArena();

        CameraManager.GoTo(CameraConstants.CameraID.CAM_ARENA_OVERVIEW);

        yield return new WaitForSeconds(1.5f);

        CameraManager.GoTo(CameraConstants.CameraID.CAM_ARENA_FOLLOW_TARGET);

        yield return null;
    }

    public void PopulateArena()
    {
        if (Data.Squads == null || Data.Squads.Count == 0)
        {
            Debug.Log("Could not populate arena, no teams defined.");
            return;
        }

        foreach (SquadData teamData in Data.Squads)
        {
            foreach (CharacterData characterData in teamData.Characters)
            {
                PerformSpawn(characterData, teamData.ID);
            }
        }
    }

    public void PerformSpawn(CharacterData data, CharacterConstants.Team team)
    {
        if(data.marker == null)
        {
            Debug.Log("Cannot spawn character, marker is null");
        }
        StartCoroutine(Coroutine_PerformSpawn(data, team));
    }

    private IEnumerator Coroutine_PerformSpawn(CharacterData data, CharacterConstants.Team team)
    {
        Debug.Log("Spawn: " + data.type + " " + team);
        GameObject characterModel = Instantiate(ArenaPrefabHelper.GetCharacterModelByTeam(team, data.type), data.marker.transform);

        characterModel.transform.LookAt(this.transform.position);

        GameObject decalPrefab = Instantiate(ArenaPrefabHelper.GetCharacterDecal(), data.marker.transform);

        if (decalPrefab != null)
        {
            CharacterDecal decal = decalPrefab.GetComponent<CharacterDecal>();

            if (decal != null)
            {
                decal.SetColorByTeam(team);
            }
        }
        yield return null;
    }

    public void SetFollowTarget(Transform newParent)
    {
        FollowTarget.transform.parent = newParent;
        FollowTarget.transform.localPosition = Vector3.zero;
    }

    public void ResetFollowTarget()
    {
        SetFollowTarget(defaultFollowTargetParent);
    }
}
