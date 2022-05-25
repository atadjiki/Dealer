using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{ 
    private SpawnConstants.CharacterSpawnerState State = SpawnConstants.CharacterSpawnerState.WaitingToSpawn;
    public void SetState(SpawnConstants.CharacterSpawnerState newState) { State = newState; }
    public SpawnConstants.CharacterSpawnerState GetState() { return State; }

    private void Awake()  { SpawnManager.Instance.RegisterSpawnPoint(this); }

    private void OnDestroy() {  SpawnManager.Instance.UnRegisterSpawnPoint(this); }
}
