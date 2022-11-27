using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

[System.Serializable]
public struct CharacterComponentData
{
    public Enumerations.CharacterModelID ModelID;
}

[ExecuteAlways]
public class CharacterComponent : MonoBehaviour, IGameplayInitializer
{
    [SerializeField] protected CharacterComponentData data;

    [SerializeField] protected CharacterModel model;

    protected bool _initialized = false;

    private void Awake()
    {
        Initialize();
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

    ////DEBUG
    ///
    private void OnEnable()
    {
        ClearMarker();

        if(Application.isEditor && Application.isPlaying == false)
        {
            Instantiate(PrefabLibrary.GetDirectionalMarker(), this.transform);
        }
    }

    private void ClearMarker()
    {
        foreach (DirectionalMarker marker in GetComponentsInChildren<DirectionalMarker>())
        {
            DestroyImmediate(marker.gameObject, true);
        }
    }

    private void OnDisable()
    {
        ClearMarker();
    }
}
