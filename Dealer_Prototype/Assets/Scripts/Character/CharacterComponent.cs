using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEditor;
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

    protected CharacterModel model;

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

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(this.transform.position, 0.1f);
        Gizmos.DrawRay(new Ray(this.transform.position, this.transform.forward));

        Handles.Label(this.transform.position + new Vector3(-0.5f,-0.5f,0), data.ModelID.ToString());
    }
#endif
}

