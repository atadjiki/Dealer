using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

[ExecuteAlways]
public class CharacterComponent : MonoBehaviour
{
    protected Enumerations.CharacterModelID _modelID = Enumerations.CharacterModelID.Model_Male1;

    protected CharacterModel model;

    public virtual void ProcessSpawnData(object _data)
    {
        _modelID = ((CharacterSpawnData)_data).ModelID;
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
            GameObject spawnedCharacter = Instantiate(PrefabLibrary.GetCharacterModelByID(_modelID), this.transform);
            model = spawnedCharacter.GetComponent<CharacterModel>();
            yield return new WaitUntil(() => model != null);
        }

        yield return null;
    }
}

