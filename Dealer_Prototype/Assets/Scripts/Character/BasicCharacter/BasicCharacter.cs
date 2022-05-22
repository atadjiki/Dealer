using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class BasicCharacter : MonoBehaviour
{
    protected Animator _animator;
    protected CharacterAnimationComponent _characterAnimation;
    protected NavigatorComponent _navigator;
    protected Light _light;

    private GameObject _model;

    public GameObject GetModel()
    {
        return _model;
    }

    [Header("Character Setup")]

    private CharacterInfo characterInfo;
    private AnimationConstants.Anim CurrentAnimation = AnimationConstants.Anim.Idle;

    private bool bHasInitialized = false;

    internal virtual void Initialize(CharacterInfo _characterInfo)
    {
        characterInfo = _characterInfo;
        StartCoroutine(DoInitialize());
    }

    internal virtual IEnumerator DoInitialize()
    {
        //setup navigator
        GameObject NavigatorPrefab = PrefabFactory.CreatePrefab(RegistryID.Navigator, this.transform);
        _navigator = NavigatorPrefab.GetComponent<NavigatorComponent>();
        _navigator.SetCanMove(true);

        yield return new WaitWhile(() => _navigator == null);

        //setup character model and attach to navigator
        _model = PrefabFactory.GetCharacterPrefab(characterInfo.ID.ToString(), NavigatorPrefab.transform);
        // ModelPrefab.transform.parent = NavigtorPrefab.transform;
        _animator = _model.GetComponent<Animator>();

        _characterAnimation = _model.GetComponent<CharacterAnimationComponent>();
        _characterAnimation.SetSocket(_navigator.gameObject);

        yield return new WaitWhile(() => _animator == null);

        GameObject CharacterLightPrefab = PrefabFactory.CreatePrefab(RegistryID.CharacterLight, _model.transform);
        _light = CharacterLightPrefab.GetComponent<Light>();

        yield return new WaitWhile(() => _light == null);

        FadeToAnimation(characterInfo.InitialAnim, 0.0f, false);

        bHasInitialized = true;
    }

    public bool HasInitialized() { return bHasInitialized; }

    public void SetPositionRotation(Vector3 Position, Quaternion Rotation)
    {
        _navigator.gameObject.transform.position = Position;
        _navigator.gameObject.transform.rotation = Rotation;

    }

    public virtual void OnNewDestination(Vector3 destination) { }

    public virtual void OnDestinationReached(Vector3 destination) { }

    public void FadeToAnimation(AnimationConstants.Anim anim, float time, bool canMove)
    {
        string animString = AnimationConstants.FetchAnimString(characterInfo.ID, anim);

        if (_animator != null) _animator.CrossFade(animString, time);
        if (_navigator != null) _navigator.SetCanMove(canMove);
        SetCurrentAnimation(anim);

        DebugManager.Instance.Print(DebugManager.Log.LogCharacter, "Fading to anim " + animString);
    }

    public AnimationConstants.Anim GetCurrentAnimation() { return CurrentAnimation; }

    public void SetCurrentAnimation(AnimationConstants.Anim anim)
    {
        CurrentAnimation = anim;
        if (UIManager.Instance) UIManager.Instance.HandleEvent(UI.Events.SetAnimText, anim);
    }

    public string GetID()
    {
        return characterInfo.ID.ToString();
    }

    public NavigatorComponent GetNavigatorComponent()
    {
        return _navigator;
    }
}
