using System;
using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public struct QueueData
{
    public CharacterConstants.TeamID team;
    public CharacterSpawnData characterData;
}

public class Arena : MonoBehaviour
{
    //setup phase
    [SerializeField] private EncounterData data;
    [SerializeField] private CameraManager CameraManager;

    [SerializeField] private ArenaCamera ArenaCamera;

    //combat phase
    public Queue<QueueData> CharacterQueue;

    private enum State { NONE, WAITING, BUSY };

    private State _state = State.NONE;

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
        //BuildCharacterQueue();

        PopulateArena();

        //CameraManager.GoTo(CameraConstants.CameraID.CAM_ARENA_OVERVIEW);

        //yield return new WaitForSeconds(0.5f);

        //CameraManager.GoTo(CameraConstants.CameraID.CAM_ARENA_MAIN);

        //yield return new WaitForSeconds(1.5f);

        //ToggleCharacterUI(true);

        //ProcessQueue();

        yield return null;
    }

    public void PopulateArena()
    {
        if (data.Waves == null || data.Waves.Count == 0)
        {
            Debug.Log("Could not populate arena, no teams defined.");
            return;
        }

        foreach (WaveData teamData in data.Waves)
        {
            foreach (CharacterSpawnData characterData in teamData.Characters)
            {
                GameObject characterObject = new GameObject();
                characterObject.transform.parent = characterData.Marker.transform;
                CharacterComponent characterComponent = characterObject.AddComponent<CharacterComponent>();
                characterComponent.PerformSpawn(characterData, teamData.ID);
            }
        }
    }

    public void ProcessQueue()
    {
        if(CharacterQueue.Count == 0)
        {
            BuildCharacterQueue();
            ProcessQueue();
        }
        else
        {
            //update all positions

            QueueData queueData = CharacterQueue.Peek();

            Debug.Log("Processing queue - Turn " + queueData.team + " - Character - " + queueData.characterData.Marker.name);
            
            SelectCharacter(queueData.characterData);

            _state = State.WAITING;
        }
    }

    public void ProcessTurn_AI()
    {
        //pick a target
        CharacterSpawnData target = PickTarget();

        EndTurn();

    }

    private CharacterSpawnData PickTarget()
    {

        foreach(WaveData squad in data.Waves)
        {
            if(squad.ID == GetOpposingTeam())
            {
                return squad.Characters[UnityEngine.Random.Range(0, squad.Characters.Count - 1)];
            }
        }

        Debug.Log("Couldnt find target");
        return new CharacterSpawnData();
    }

    private CharacterConstants.TeamID GetCurrentTeam()
    {
        return CharacterQueue.Peek().team;
    }

    private CharacterConstants.TeamID GetOpposingTeam()
    {
        foreach(WaveData squad in data.Waves)
        {
            if(squad.ID != GetCurrentTeam())
            {
                return squad.ID;
            }
        }

        return CharacterConstants.TeamID.NONE;
    }

    public void EndTurn()
    {
        CharacterQueue.Dequeue();
        ProcessQueue();
    }

    public void SelectCharacter(CharacterSpawnData character)
    {
        CharacterCameraOffset offset = character.Marker.GetComponentInChildren<CharacterCameraOffset>();

        ArenaCamera.SetTarget(offset.transform);
    }

    public void BuildCharacterQueue()
    {
        CharacterQueue = new Queue<QueueData>();

        Dictionary<CharacterConstants.TeamID, int> squadSizes = new Dictionary<CharacterConstants.TeamID, int>();

        foreach(WaveData squad in data.Waves)
        {
            squadSizes.Add(squad.ID, squad.Characters.Count);
        }

        int totalCharacters = 0;

        foreach(KeyValuePair<CharacterConstants.TeamID, int> pair in squadSizes)
        {

            totalCharacters += pair.Value;
        }

        //iterate through squads and form queue
        for(int i = 0; i < totalCharacters; i++)
        {
            for(int j = 0; j < data.Waves.Count; j++)
            {
                if(data.Waves[j].Characters.Count > i)
                {
                    QueueData queueData = new QueueData();
                    queueData.team = data.Waves[j].ID;
                    queueData.characterData = data.Waves[j].Characters[i];

                    CharacterQueue.Enqueue(queueData);
                    Debug.Log(data.Waves[j].ID + " " + (i+1));
                }
            }
        }
    }
}
