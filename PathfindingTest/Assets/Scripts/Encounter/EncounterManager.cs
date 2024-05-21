using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class EncounterManager : MonoBehaviour
{
    //who should we spawn, etc
    [SerializeField] private EncounterSetupData SetupData;

    private EncounterPrefabData _prefabData;
    private EncounterModel _model;

    private void Awake()
    {
        _prefabData = ResourceUtil.GetEncounterPrefabs();

        Instantiate<GameObject>(_prefabData.CameraRig);

        _model = new EncounterModel();
    }

    private IEnumerator Coroutine_HandleInit()
    {
        foreach (CharacterID ID in SetupData.Characters)
        {
            CharacterData data = ResourceUtil.GetCharacterData(ID);

            if (data != null)
            {
                CharacterComponent character = CharacterUtil.BuildCharacterObject(data, this.transform);

                yield return character.Coroutine_Setup(data);

                Vector3 spawnLocation = EnvironmentUtil.GetRandomTile();
                character.Teleport(spawnLocation);
                Vector3 destination = EnvironmentUtil.GetClosestTileWithCover(character.GetWorldLocation());
                character.MoveTo(destination);
                Debug.Log("Character " + data.ID + " spawned at " + spawnLocation.ToString());
            }
        }
    }
}
