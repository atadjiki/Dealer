using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ObjectSpawner : MonoBehaviour
{

    public CharacterConstants.CharacterID CharacterID;

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

    public void SpawnCharacter(CharacterConstants.CharacterID ID)
    {
        State = ObjectSpawnerState.Spawning;

        Debug.Log("Spawning character - " + ID.ToString());

        GameObject NPC = PrefabFactory.Instance.CreatePrefab(RegistryID.NPC, this.transform);

        NPCComponent npcComp = NPC.GetComponent<NPCComponent>();

        if(npcComp != null)
        {
            npcComp.CharacterID = ID;
            npcComp.Initialize();
        }

        State = ObjectSpawnerState.Spawned;

    }

}
