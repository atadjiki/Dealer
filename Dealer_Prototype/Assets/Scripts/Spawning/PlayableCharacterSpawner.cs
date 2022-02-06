using System.Collections;
using Constants;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayableCharacterSpawner : CharacterSpawner
{
    [SerializeField] private bool PlayerLock = true;

    public override IEnumerator PerformSpawn()
    {
        yield return new WaitForSeconds(2.0f);

        State = CharacterSpawnerState.Spawning;

        DebugManager.Instance.Print(DebugManager.Log.LogSpawner, "Spawning playable character");

        GameObject Character = PrefabFactory.CreatePrefab(RegistryID.Player, this.transform);
        PlayableCharacterComponent playerComp = Character.GetComponent<PlayableCharacterComponent>();

        yield return new WaitWhile(() => playerComp == null);

        SpawnData playerSpawnData = new SpawnData()
        {
            ID = CharacterID,
            Team = CharacterConstants.Team.Ally
        };


        playerComp.Initialize(playerSpawnData);

        State = CharacterSpawnerState.Spawned;

        yield return new WaitWhile(() => !playerComp.HasInitialized());

        if (PlayerLock)
        {
            PlayableCharacterManager.Instance.LockToCharacter(playerComp);
        }

        yield return null;
    }
}
