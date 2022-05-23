using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class CharacterModel : MonoBehaviour
{
    protected Animator _animator;
    protected CharacterAnimationComponent _characterAnimation;
    protected NavigatorComponent _navigator;
    protected Light _light;

    [Header("Character Setup")]

    private CharacterInfo characterInfo;
    private AnimationConstants.Anim CurrentAnimation = AnimationConstants.Anim.Idle;

    private bool bHasInitialized = false;

    internal virtual void Initialize(CharacterInfo _characterInfo)
    {
        characterInfo = _characterInfo;
        StartCoroutine(DoInitialize());
    }

    private void OnDestroy()
    {
        if (CharacterPanel.Instance)
        {
            CharacterPanel.Instance.UnRegisterCharacter(this);
        }
        if(PartyManager.Instance)
        {
            PartyManager.Instance.UnRegisterCharacterModel(characterInfo);
        }
    }

    internal virtual IEnumerator DoInitialize()
    {
        //setup navigator
        GameObject NavigatorPrefab = PrefabFactory.CreatePrefab(RegistryID.Navigator, this.transform);
        _navigator = NavigatorPrefab.GetComponent<NavigatorComponent>();
        _navigator.SetCanMove(true);

        yield return new WaitWhile(() => _navigator == null);

        // ModelPrefab.transform.parent = NavigtorPrefab.transform;
        _animator = this.GetComponent<Animator>();

        _characterAnimation = GetComponent<CharacterAnimationComponent>();
        _characterAnimation.SetSocket(_navigator.gameObject);

        yield return new WaitWhile(() => _animator == null);

        GameObject CharacterLightPrefab = PrefabFactory.CreatePrefab(RegistryID.CharacterLight, this.transform);
        _light = CharacterLightPrefab.GetComponent<Light>();

        yield return new WaitWhile(() => _light == null);

        FadeToAnimation(characterInfo.InitialAnim, 0.0f, true);

        if (CharacterPanel.Instance)
        {
            Debug.Log("Attempting to register " + this.name);
            CharacterPanel.Instance.RegisterCharacter(this);
        }

        if(PartyManager.Instance)
        {
            PartyManager.Instance.RegisterCharacterModel(characterInfo, this);
        }

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
