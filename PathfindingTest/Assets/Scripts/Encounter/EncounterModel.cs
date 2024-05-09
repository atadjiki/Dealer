using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class EncounterModel : MonoBehaviour
{
    [SerializeField] private EncounterSetupData SetupData;

    [SerializeField] private List<CharacterData> Characters;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Coroutine_Init());
    }

    private IEnumerator Coroutine_Init()
    {
        foreach (CharacterData data in Characters)
        {
            if (SetupData.ToSpawn == data.ID)
            {
                CharacterComponent character = CharacterUtil.BuildCharacterObject(data, this.transform);

                Vector3 spawnLocation = EncounterUtil.GetRandomTile();

                character.transform.position = spawnLocation;

                character.Setup(data);
            }
        }

        yield return null;
    }
}
