using System;
using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class Arena : MonoBehaviour
{
    [SerializeField] private ArenaData Data;
    [SerializeField] private CameraManager CameraManager;

    [SerializeField] private ArenaCamera ArenaCamera;


    private void Awake()
    {
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

        CameraManager.GoTo(CameraConstants.CameraID.CAM_ARENA);

        yield return new WaitForSeconds(2.5f);

        ArenaCamera.SetTarget(GetCurrentPlayerCharacter().marker.transform);
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
                CharacterHelper.PerformSpawn(characterData, teamData.ID, this.transform);
            }
        }
    }

    public CharacterData GetCurrentPlayerCharacter()
    {
        SquadData playerSquad = GetPlayerSquad();

        if(playerSquad.Characters.Count > 0)
        {
            return playerSquad.Characters[0];
        }

        Debug.Log("Couldnt find current player character?");
        return new CharacterData();
    }

    public SquadData GetPlayerSquad()
    {
        foreach(SquadData squadData in Data.Squads)
        {
            if(squadData.ID == Data.PlayerTeam)
            {
                return squadData;
            }
        }

        Debug.Log("Couldnt find player team?");
        return new SquadData();
    }
}
