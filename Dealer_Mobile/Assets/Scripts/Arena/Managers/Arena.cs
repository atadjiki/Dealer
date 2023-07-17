using System;
using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public struct QueueData
{
    public CharacterConstants.TeamID team;
    public CharacterData characterData;
}

public class Arena : MonoBehaviour
{
    //setup phase
    [SerializeField] private ArenaData data;
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
        BuildCharacterQueue();

        PopulateArena();

        CameraManager.GoTo(CameraConstants.CameraID.CAM_ARENA_OVERVIEW);

        yield return new WaitForSeconds(0.5f);

        CameraManager.GoTo(CameraConstants.CameraID.CAM_ARENA_MAIN);

        yield return new WaitForSeconds(1.5f);

        ToggleCharacterUI(true);

        ProcessQueue();

        yield return null;
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
                PerformSpawn(characterData, teamData.ID);
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
        CharacterData target = PickTarget();

        EndTurn();

    }

    private CharacterData PickTarget()
    {

        foreach(SquadData squad in data.Squads)
        {
            if(squad.ID == GetOpposingTeam())
            {
                return squad.Characters[UnityEngine.Random.Range(0, squad.Characters.Count - 1)];
            }
        }

        Debug.Log("Couldnt find target");
        return new CharacterData();
    }

    private CharacterConstants.TeamID GetCurrentTeam()
    {
        return CharacterQueue.Peek().team;
    }

    private CharacterConstants.TeamID GetOpposingTeam()
    {
        foreach(SquadData squad in data.Squads)
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

    public void SelectCharacter(CharacterData character)
    {
        CharacterCameraOffset offset = character.Marker.GetComponentInChildren<CharacterCameraOffset>();

        ArenaCamera.SetTarget(offset.transform);
    }

    public void BuildCharacterQueue()
    {
        CharacterQueue = new Queue<QueueData>();

        Dictionary<CharacterConstants.TeamID, int> squadSizes = new Dictionary<CharacterConstants.TeamID, int>();

        foreach(SquadData squad in data.Squads)
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
            for(int j = 0; j < data.Squads.Count; j++)
            {
                if(data.Squads[j].Characters.Count > i)
                {
                    QueueData queueData = new QueueData();
                    queueData.team = data.Squads[j].ID;
                    queueData.characterData = data.Squads[j].Characters[i];

                    CharacterQueue.Enqueue(queueData);
                    Debug.Log(data.Squads[j].ID + " " + (i+1));
                }
            }
        }
    }

    private void ToggleCharacterUI(bool flag)
    {
        foreach(SquadData squad in data.Squads)
        {
            foreach(CharacterData character in squad.Characters)
            {
                CharacterMarker marker = character.Marker;
                if(marker != null)
                {
                    CharacterCombatCanvas canvas = marker.GetComponentInChildren<CharacterCombatCanvas>();
                    if (canvas != null)
                    {
                        canvas.Toggle(flag);
                    }
                    else
                    {
                        Debug.Log("Couldnt find canvas");
                    }
                }
            }
        }
    }

    private void PerformSpawn(CharacterData data, CharacterConstants.TeamID team)
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

        characterModel.transform.LookAt(this.transform.position);

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

        GameObject canvasObject = Instantiate(PrefabHelper.GetCharacterCombatCanvas(), data.Marker.transform);
        CharacterCombatCanvas combatCanvas = canvasObject.GetComponent<CharacterCombatCanvas>();
        combatCanvas.Refresh(0, team, data);
        combatCanvas.Toggle(false);
    }

    public CharacterData GetCurrentPlayerCharacter()
    {
        SquadData playerSquad = GetPlayerSquad();

        if (playerSquad.Characters.Count > 0)
        {
            return playerSquad.Characters[0];
        }

        Debug.Log("Couldnt find current player character?");
        return new CharacterData();
    }

    public SquadData GetPlayerSquad()
    {
        foreach (SquadData squadData in data.Squads)
        {
            if (squadData.ID == data.PlayerTeam)
            {
                return squadData;
            }
        }

        Debug.Log("Couldnt find player team?");
        return new SquadData();
    }
}
