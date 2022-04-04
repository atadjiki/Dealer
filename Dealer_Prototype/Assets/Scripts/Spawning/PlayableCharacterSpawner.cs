using System.Collections;
using Constants;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayableCharacterSpawner : CharacterSpawner
{
    [SerializeField] private bool PlayerLock = true;

    internal override IEnumerator WaitForManager()
    {
        yield return new WaitUntil(() => PlayableCharacterManager.Instance != null);

        yield return new WaitUntil(() => PlayableCharacterManager.Instance.IsInitialized());

        AttemptSpawn();

        yield return null;
    }

    public override void AttemptSpawn()
    {
        base.AttemptSpawn();

        if (State == CharacterSpawnerState.WaitingToSpawn && Mode == SpawnMode.AutoActivate)
        {
            StartCoroutine(PerformSpawn());
        }
    }

    internal override IEnumerator PerformSpawn()
    {
        if(PlayableCharacterManager.Instance)
        {
            State = CharacterSpawnerState.Spawning;

            DebugManager.Instance.Print(DebugManager.Log.LogSpawner, "Spawning playable character");

            GameObject Character = PrefabFactory.CreatePrefab(RegistryID.Player, this.transform);
            PlayableCharacterComponent playerComp = Character.GetComponent<PlayableCharacterComponent>();

            yield return new WaitWhile(() => playerComp == null);

            spawnData.SetTeam(CharacterConstants.Team.Ally);

            playerComp.Initialize(spawnData);

            State = CharacterSpawnerState.Spawned;

            yield return new WaitWhile(() => !playerComp.HasInitialized());

            if (PlayerLock)
            {
                PlayableCharacterManager.Instance.LockToCharacter(playerComp);
            }
        }

        yield return null;
    }
}
