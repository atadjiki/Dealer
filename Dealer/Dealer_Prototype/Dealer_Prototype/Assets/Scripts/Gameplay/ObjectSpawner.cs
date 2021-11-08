using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ObjectSpawner : MonoBehaviour
{

    public Constants.CharacterConstants.Characters CharacterID;

    public enum ObjectSpawnerState { WaitingToSpawn, Spawning, Spawned };
    private ObjectSpawnerState State = ObjectSpawnerState.WaitingToSpawn;

    public enum ActivationMode { Trigger, AutoActivate, None };
    public ActivationMode activationMode = ActivationMode.AutoActivate;

    private void Awake()
    {
        if(activationMode == ActivationMode.AutoActivate)
        {
            if(State == ObjectSpawnerState.WaitingToSpawn)
            {
                SpawnCharacter(CharacterID);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (activationMode == ActivationMode.Trigger)
        {
            if (State == ObjectSpawnerState.WaitingToSpawn)
            {
                SpawnCharacter(CharacterID);
            }
        }
    }

    public void SpawnCharacter(CharacterConstants.Characters ID)
    {
        State = ObjectSpawnerState.Spawning;

        GameObject Model = GetCharacterPrefab(ID);
        GameObject NPC = PrefabFactory.Instance.CreatePrefab(Prefab.NPC, this.transform);
        Model.transform.parent = NPC.GetComponentInChildren<NavigatorComponent>().transform;
        NPC.GetComponent<NPCComponent>().Initialize();

        State = ObjectSpawnerState.Spawned;

    }

    private GameObject GetCharacterPrefab(CharacterConstants.Characters ID)
    {
        if(ID == CharacterConstants.Characters.Male_1)
        {
            return PrefabFactory.Instance.CreatePrefab(Prefab.Model_Male, this.transform);
        }
        else if(ID == CharacterConstants.Characters.Female_1)
        {
            return PrefabFactory.Instance.CreatePrefab(Prefab.Model_Female, this.transform);
        }

        return null;
    }

}
