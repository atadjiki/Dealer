using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class CharacterComponent : MonoBehaviour
{
    [SerializeField] private CharacterConstants.CharacterState state = CharacterConstants.CharacterState.None;

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
        updateTime = Random.Range(1, 5);

        characterInfo = _characterInfo;

        StartCoroutine(DoInitialize());

        CharacterManager.Instance.RegisterCharacter(this);
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

    private void OnDestroy()
    {
        CharacterManager.Instance.UnRegisterCharacter(this);
    }

    public bool HasInitialized() { return bHasInitialized; }

    public virtual void OnNewDestination(Vector3 destination) { }

    public virtual void OnDestinationReached(Vector3 destination) { }

    public string GetID() { return characterInfo.ID.ToString(); }

    public CharacterConstants.CharacterID GetCharacterID() { return characterInfo.ID; }

    public NavigatorComponent GetNavigatorComponent() { return _navigator; }

    public CharacterAnimationComponent GetAnimationComponent() { return _animationComponent; }

    public CharacterTaskComponent GetTaskComponent() { return _taskComponent; }

    public CharacterInfo GetCharacterInfo() { return characterInfo; }

    public CharacterConstants.CharacterState GetState() { return state; }

    private void SetState(CharacterConstants.CharacterState _state)
    {
        Debug.Log(characterInfo.name + " state changed from " + state + " to " + _state);
        state = _state;
    }

    public void ToWaitingForUpdate()
    {
        SetState(CharacterConstants.CharacterState.WaitingForUpdate);
    }

    public void ToMoving()
    {
        SetState(CharacterConstants.CharacterState.Moving);
    }

    public void ToWaiting()
    {
        SetState(CharacterConstants.CharacterState.Waiting);
    }
}