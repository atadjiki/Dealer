using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class CharacterComponent : MonoBehaviour
{

    [SerializeField] private NavigatorComponent _navigator;
    [SerializeField] private CharacterTaskComponent _taskComponent;

    [Header("Character Setup")]

    private CharacterInfo characterInfo;
    private CharacterAnimationComponent _animationComponent;

    private bool bHasInitialized = false;

    public float updateTime { get; set; } //update when we hit this threshhold
    public float timeSinceLastUpdate { get; set; } //current time

    internal virtual void Initialize(CharacterInfo _characterInfo)
    {
        characterInfo = _characterInfo;

        StartCoroutine(DoInitialize());

        if (CharacterManager.Instance)
        {
            CharacterManager.Instance.RegisterCharacter(this);
        }

    }

    private void OnDestroy()
    {
        if (CharacterManager.Instance)
        {
            CharacterManager.Instance.UnRegisterCharacter(this);
        }
    }

    internal virtual IEnumerator DoInitialize()
    {
        GameObject ModelPrefab = PrefabFactory.GetCharacterPrefab(characterInfo.ID.ToString(), _navigator.gameObject.transform);

        _animationComponent = ModelPrefab.GetComponent<CharacterAnimationComponent>();
        _animationComponent.FadeToAnimation(characterInfo.InitialAnim, 0.0f, true);

        _navigator.ToggleMovement(true);

        bHasInitialized = true;

        yield return null;
    }

    public bool HasInitialized() { return bHasInitialized; }

    public virtual void OnNewDestination(Vector3 destination) { }

    public virtual void OnDestinationReached(Vector3 destination) { }

    public string GetID()
    {
        return characterInfo.ID.ToString();
    }

    public CharacterConstants.CharacterID GetCharacterID()
    {
        return characterInfo.ID;
    }

    public NavigatorComponent GetNavigatorComponent()
    {
        return _navigator;
    }

    public CharacterAnimationComponent GetAnimationComponent()
    {
        return _animationComponent;
    }

    public CharacterTaskComponent GetTaskComponent()
    {
        return _taskComponent;
    }

    public CharacterInfo GetCharacterInfo()
    {
        return characterInfo;
    }
}
