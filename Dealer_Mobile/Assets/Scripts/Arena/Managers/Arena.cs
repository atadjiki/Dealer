using System;
using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class Arena : MonoBehaviour
{
    //setup phase
    [SerializeField] private ArenaData data;
    [SerializeField] private CameraManager CameraManager;

    [SerializeField] private ArenaCamera ArenaCamera;

    //combat phase
    public Queue<CharacterData> CharacterQueue;

    private void Awake()
    {
        Launch();
    }

    public void Launch()
    {
        StartCoroutine(Coroutine_SetupArena());
    }

    private IEnumerator Coroutine_SetupArena()
    {
        BuildCharacterQueue();

        PopulateArena();

        CameraManager.GoTo(CameraConstants.CameraID.CAM_ARENA_OVERVIEW);

        yield return new WaitForSeconds(1.5f);

        CameraManager.GoTo(CameraConstants.CameraID.CAM_ARENA_MAIN);

        yield return new WaitForSeconds(1.5f);

        ArenaCamera.SetTarget(GetCurrentPlayerCharacter().marker.transform);
    }

    public void PopulateArena()
    {
        if (data.Squads == null || data.Squads.Count == 0)
        {
            Debug.Log("Could not populate arena, no teams defined.");
            return;
        }

        foreach (SquadData teamData in data.Squads)
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
        foreach(SquadData squadData in data.Squads)
        {
            if(squadData.ID == data.PlayerTeam)
            {
                return squadData;
            }
        }

        Debug.Log("Couldnt find player team?");
        return new SquadData();
    }

    public void BuildCharacterQueue()
    {
        CharacterQueue = new Queue<CharacterData>();

        Dictionary<CharacterConstants.Team, int> squadSizes = new Dictionary<CharacterConstants.Team, int>();

        foreach(SquadData squad in data.Squads)
        {
            squadSizes.Add(squad.ID, squad.Characters.Count);
        }

        int totalCharacters = 0;

        foreach(KeyValuePair<CharacterConstants.Team, int> pair in squadSizes)
        {

            totalCharacters += pair.Value;
        }

        //iterate through squads and form queue
        for(int i = 0; i < totalCharacters; i++)
        {
            for(int j = 0; j < data.Squads.Count; j++)
            {
                if(data.Squads[j].Characters.Count > i)
                {
                    CharacterQueue.Enqueue(data.Squads[j].Characters[i]);
                    Debug.Log(data.Squads[j].Characters[i].marker.gameObject.name);
                }
            }
        }
    }
}
