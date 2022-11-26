using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

[System.Serializable]
public struct CharacterComponentData
{
    public Enumerations.CharacterModelID ModelID;
}

public class CharacterComponent : RunTimeComponent
{
    [SerializeField] private CharacterComponentData data;

    [SerializeField] private CharacterModel model;
    [SerializeField] private NavigatorComponent navigator;

    private void Awake()
    {
        Initialize();
    }

    protected override IEnumerator PerformInitialize()
    {
        //create a navigator to move the model
        if(navigator == null)
        {
            GameObject navigatorObject = new GameObject("Navigator");
            navigatorObject.transform.parent = transform;
            navigator = navigatorObject.AddComponent<NavigatorComponent>();
            navigator.Initialize();
            yield return new WaitUntil(() => navigator.HasInitialized());
        }

        if(model == null)
        {
            //load our associated model
            GameObject spawnedCharacter = Instantiate(PrefabLibrary.GetCharacterModelByID(data.ModelID), navigator.transform);
            model = spawnedCharacter.GetComponent<CharacterModel>();
            yield return new WaitUntil(() => model != null);
        }

        yield return base.PerformInitialize();
    }
}
