using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{ 
    private SpawnConstants.CharacterSpawnerState State = SpawnConstants.CharacterSpawnerState.WaitingToSpawn;
    public void SetState(SpawnConstants.CharacterSpawnerState newState) { State = newState; }
    public SpawnConstants.CharacterSpawnerState GetState() { return State; }

    private void Awake()
    {
        if(LevelManager.IsManagerLoaded())
        {
            SpawnManager.Instance.RegisterSpawnPoint(this);
        }
        else
        {
            this.enabled = false;
        }
       
    }

    private void OnDestroy()
    {
        if (LevelManager.IsManagerLoaded())
        {
            SpawnManager.Instance.UnRegisterSpawnPoint(this);
        } 
    }
}
