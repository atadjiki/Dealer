using System.Collections;
using System.Collections.Generic;
using static Constants;
using UnityEngine;

public class CharacterComponent : MonoBehaviour, ICharacterEventReceiver
{
    protected CharacterID _ID;

    //Event Receivers
    protected CharacterModel _model;
    protected CharacterWeapon _weapon;
    protected CharacterAnimator _animator;
    protected CharacterAudioSource _audioSource;
    protected EncounterCharacterUI _encounterUI;

    private List<ICharacterEventReceiver> _eventReceivers;

    protected CharacterOverheadAnchor _overheadAnchor;
    protected CharacterWeaponAnchor _weaponAnchor;

    protected int _health = 0;
    protected int _baseHealth = 0;

    protected CapsuleCollider _collider;

    private bool _canReceive = true;

    public delegate void OnCharacterSetupComplete(CharacterComponent character);
    public OnCharacterSetupComplete onSetupComplete;

    public virtual IEnumerator Coroutine_Spawn()
    {
        CharacterDefinition characterDefinition = CharacterDefinition.Get(_ID);

        yield return Coroutine_Spawn(characterDefinition);
    }

    public virtual IEnumerator Coroutine_Spawn(CharacterID ID)
    {
        CharacterDefinition characterDefinition = CharacterDefinition.Get(_ID);

        yield return Coroutine_Spawn(characterDefinition);
    }

    public virtual IEnumerator Coroutine_Spawn(CharacterDefinition characterDefinition)
    {
        UIHelper.ClearTransformChildren(this.transform);

        _ID = characterDefinition.ID;
        _health = characterDefinition.BaseHealth;
        _baseHealth = characterDefinition.BaseHealth;

        _eventReceivers = new List<ICharacterEventReceiver>();

        ModelID modelID = characterDefinition.AllowedModels[UnityEngine.Random.Range(0, characterDefinition.AllowedModels.Length)];

        ResourceRequest modelRequest = GetPrefab(modelID);

        yield return new WaitUntil(() => modelRequest.isDone);

        GameObject modelPrefab = Instantiate((GameObject)modelRequest.asset, this.transform);

        _model = modelPrefab.GetComponent<CharacterModel>();

        _model.Setup(characterDefinition);

        _eventReceivers.Add(_model);

        _weaponAnchor = modelPrefab.GetComponentInChildren<CharacterWeaponAnchor>();

        yield return new WaitWhile(() => _weaponAnchor == null);

        if (characterDefinition.AllowedWeapons.Length > 0)
        {
            WeaponID weaponID = characterDefinition.AllowedWeapons[UnityEngine.Random.Range(0, characterDefinition.AllowedWeapons.Length)];

            ResourceRequest weaponRequest = GetPrefab(weaponID);

            yield return new WaitUntil(() => weaponRequest.isDone);

            GameObject weaponPrefab = Instantiate((GameObject)weaponRequest.asset, _weaponAnchor.transform);

            _weapon = weaponPrefab.GetComponent<CharacterWeapon>();

            _weapon.Setup(_ID, weaponID);

            _eventReceivers.Add(_weapon);
        }

        _overheadAnchor = GetComponentInChildren<CharacterOverheadAnchor>();

        _animator = modelPrefab.GetComponent<CharacterAnimator>();

        _animator.Setup(AnimState.Idle, _weapon.GetID());

        _eventReceivers.Add(_animator);

        ResourceRequest audioSourceRequest = GetAudioSource(_ID);

        yield return new WaitUntil(()=>audioSourceRequest.isDone);

        GameObject audioSourceObject = Instantiate<GameObject>((GameObject)audioSourceRequest.asset, this.transform);

        _audioSource = audioSourceObject.GetComponentInChildren<CharacterAudioSource>();

        _eventReceivers.Add(_audioSource);

        _collider = this.gameObject.AddComponent<CapsuleCollider>();

        yield return new WaitWhile(() => _collider == null);

        _collider.isTrigger = true;
        _collider.radius = 0.5f;
        _collider.height = 2.0f;
        _collider.center = new Vector3(0, 1.0f, 0);

        if (onSetupComplete != null)
        {
            onSetupComplete.Invoke(this);
        }

        yield return null;
    }

    public void HandleEvent(object eventData, CharacterEvent characterEvent)
    {
        if (!_canReceive) { return; }

        switch (characterEvent)
        {
            case CharacterEvent.SELECTED:
            {
                HandleEvent_Selected();
                break;
            }
            case CharacterEvent.DESELECTED:
            {
                HandleEvent_Deselected();
                break;
            }
            case CharacterEvent.TARGETED:
            {
                HandleEvent_Targeted();
                break;
            }
            case CharacterEvent.UNTARGETED:
            {
                HandleEvent_Untargeted();
                break;
            }
            case CharacterEvent.DAMAGE:
            {
                HandleEvent_HandleDamage((DamageInfo)eventData);
                break;
            }
            case CharacterEvent.HIT:
            {
                HandleEvent_Hit((DamageInfo)eventData);
                break;
            }
            case CharacterEvent.ABILITY:
            {
                HandleEvent_Ability((AbilityID) eventData);
                break;
            }
            case CharacterEvent.KILLED:
            {
                HandleEvent_Killed();
            }
            break;
            default:
                break;
        }

        BroadcastEvent(eventData, characterEvent);

        if(characterEvent == CharacterEvent.KILLED)
        {
            _canReceive = false;
        }
    }

    private void HandleEvent_Selected()
    {
        CreateDecal();
        _audioSource.Play(CharacterAudioType.Await);
    }

    private void HandleEvent_Deselected()
    {
        DestroyDecal();
    }

    private void HandleEvent_Targeted()
    {

    }

    private void HandleEvent_Untargeted()
    {

    }


    private void HandleEvent_HandleDamage(DamageInfo damageInfo)
    {
        if (IsDead()) { return; }

        _health -= Mathf.Abs(damageInfo.ActualDamage);

        _health = Mathf.Clamp(_health, 0, _health);

        SetHealth(_health);

        if (IsDead())
        {
            HandleEvent(damageInfo, CharacterEvent.KILLED);
        }
        else
        {
            HandleEvent(damageInfo, CharacterEvent.HIT);
        }
    }

    private void HandleEvent_Hit(DamageInfo damageInfo)
    { 
        if (damageInfo.ActualDamage < damageInfo.BaseDamage)
        {
            _animator.GoTo(AnimState.Hit_Light);
        }
        else if (damageInfo.ActualDamage == damageInfo.BaseDamage)
        {
            _animator.GoTo(AnimState.Hit_Medium);
        }
        else
        {
            _animator.GoTo(AnimState.Hit_Heavy);
        }
    }

    private void HandleEvent_Killed()
    {
        _health = 0;
    }

    private void HandleEvent_Ability(AbilityID abilityID)
    {
        switch (abilityID)
        {
            case AbilityID.Attack:
            {
                _weapon.OnAbility(AbilityID.Attack);
                if (_weapon.HasAmmo())
                {
                    _animator.GoTo(AnimState.Attack_Single);
                }
                break;
            }
            case AbilityID.Reload:
            {
                _weapon.OnAbility(AbilityID.Reload);
                _animator.GoTo(AnimState.Reload);
                break;
            }
            default:
            {
                break;
            }
        }
    }

    private void BroadcastEvent(object eventData, CharacterEvent characterEvent)
    {
        foreach(ICharacterEventReceiver eventReceiver in _eventReceivers)
        {
            eventReceiver.HandleEvent(eventData, characterEvent);
        }
    }

    //Getters and Setters
    public bool HasAbility(AbilityID abilityID)
    {
        return Constants.GetAllowedAbilities(_ID).Contains(abilityID);
    }

    public void SetID(CharacterID ID)
    {
        _ID = ID;
    }

    public CharacterID GetID()
    {
        return _ID;
    }

    public virtual void SetHealth(int health)
    {
        _health = health;
    }

    public virtual int GetHealth()
    {
        return _health;
    }

    public virtual int GetBaseHealth()
    {
        return _baseHealth;
    }

    public virtual bool IsDead()
    {
        return _health == 0;
    }

    public virtual bool IsAlive()
    {
        return _health > 0;
    }

    public WeaponID GetWeaponID()
    {
        return _weapon.GetID();
    }

    public int GetRemainingAmmo()
    {
        return _weapon.GetRemainingAmmo();
    }

    public bool IsAmmoFull()
    {
        return _weapon.GetRemainingAmmo() == _weapon.GetMaxAmmo();
    }

    public TeamID GetTeam()
    {
        return GetTeamByID(GetID());
    }

    public CharacterOverheadAnchor GetOverheadAnchor()
    {
        return _overheadAnchor;
    }

    public void PlayAudio(CharacterAudioType audioType)
    {
        _audioSource.Play(audioType);
    }

    public void ToggleHighlight(bool flag)
    {
        _model.ToggleHighlight(flag);
    }

    private void OnMouseOver()
    {
        if(IsAlive())
        {
            ToggleHighlight(true);
        }
    }

    private void OnMouseExit()
    {
        ToggleHighlight(false);
    }

    //creators and destroyers
    public virtual IEnumerator PerformCleanup()
    {
        yield return DestroyModel();

        yield return DestroyWeapon();

        yield return null;
    }

    protected virtual IEnumerator DestroyModel()
    {
        GameObject modelObject = _model.gameObject;

        _model = null;

        GameObject.Destroy(modelObject);

        yield return new WaitUntil(() => modelObject == null);
    }

    protected virtual IEnumerator DestroyWeapon()
    {
        GameObject weaponObject = _weapon.gameObject;

        _weapon = null;

        GameObject.Destroy(weaponObject);

        yield return new WaitUntil(() => weaponObject == null);
    }

    protected virtual void CreateDecal()
    {
        StartCoroutine(Coroutine_CreateDecal());
    }

    private IEnumerator Coroutine_CreateDecal()
    {
        ResourceRequest request = GetPrefab(PrefabID.Character_Decal);

        yield return new WaitUntil(() => request.isDone);

        GameObject decalPrefab = Instantiate((GameObject)request.asset, this.transform);

        if (decalPrefab != null)
        {
            CharacterDecal decal = decalPrefab.GetComponent<CharacterDecal>();

            if (decal != null)
            {
                decal.SetColor(GetTeamByID(GetID()));
            }
        }
    }

    protected virtual void DestroyDecal()
    {
        CharacterDecal decal = GetComponentInChildren<CharacterDecal>();

        if (decal != null)
        {
            GameObject.Destroy(decal.gameObject);
        }
    }

    public virtual void CreateEncounterOverhead()
    {
        StartCoroutine(Coroutine_CreateEncounterOverhead());
    }

    private IEnumerator Coroutine_CreateEncounterOverhead()
    {
        ResourceRequest request = GetPrefab(PrefabID.Encounter_UI_CharacterUI_Canvas);

        yield return new WaitUntil(() => request.isDone);

        GameObject overheadPrefab = Instantiate((GameObject)request.asset, null);

        if (overheadPrefab != null)
        {
            _encounterUI = overheadPrefab.GetComponent<EncounterCharacterUI>();

            if (_encounterUI != null)
            {
                _eventReceivers.Add(_encounterUI);
                _encounterUI.Setup(this);
            }
        }
    }

    public virtual void DestroyEncounterOverhead()
    {
        if (_encounterUI != null)
        {
            _eventReceivers.Remove(_encounterUI);
            Destroy(_encounterUI.gameObject);
        }
    }

    public bool CanReceiveCharacterEvents()
    {
        return _canReceive;
    }
}