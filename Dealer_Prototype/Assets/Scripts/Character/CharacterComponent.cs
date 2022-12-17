using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public struct CharacterSpawnData
{
    public Enumerations.CharacterModelID ModelID;

    public List<NPC.TaskID> AllowedTasks;

    public bool AllowNavigation;

    public bool SpawnOnClosestPoint;
}

[ExecuteAlways]
public class CharacterComponent : MonoBehaviour, IGameplayInitializer
{
    [SerializeField] protected CharacterSpawnData data;

    protected CharacterModel model;

    protected bool _initialized = false;

    public void SetData(CharacterSpawnData _data)
    {
        data = _data;
    }

    public void Initialize()
    {
        if (Application.isPlaying)
        {
            StartCoroutine(PerformInitialize());
        }
    }

    public virtual IEnumerator PerformInitialize()
    {

        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        if (model == null)
        {
            //load our associated model
            GameObject spawnedCharacter = Instantiate(PrefabLibrary.GetCharacterModelByID(data.ModelID), this.transform);
            model = spawnedCharacter.GetComponent<CharacterModel>();
            yield return new WaitUntil(() => model != null);
        }

        _initialized = true;

        yield return null;
    }

    public bool HasInitialized()
    {
        return _initialized;
    }
}

