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

    public delegate void OnCharacterSetupComplete(CharacterComponent character);
    public OnCharacterSetupComplete onSetupComplete;

    public void Debug_Spawn()
    {
        SetID(CharacterID.HENCHMAN);
        StartCoroutine(SpawnCharacter());
    }

    public void SetID(CharacterID ID)
    {
        _ID = ID;
    }

    public virtual IEnumerator SpawnCharacter()
    {
        CharacterDefinition def = CharacterDefinition.Get(_ID);

        _health = def.BaseHealth;
        _baseHealth = def.BaseHealth;

        _eventReceivers = new List<ICharacterEventReceiver>();

        ModelID modelID = def.AllowedModels[Random.Range(0, def.AllowedModels.Length)];

        ResourceRequest modelRequest = GetPrefab(modelID);

        yield return new WaitUntil(() => modelRequest.isDone);

        GameObject modelPrefab = Instantiate((GameObject)modelRequest.asset, this.transform);

        _model = modelPrefab.GetComponent<CharacterModel>();

        _model.Setup(def);

        _eventReceivers.Add(_model);

        _weaponAnchor = modelPrefab.GetComponentInChildren<CharacterWeaponAnchor>();

        yield return new WaitWhile(() => _weaponAnchor == null);

        if (def.AllowedWeapons.Length > 0)
        {
            WeaponID weaponID = def.AllowedWeapons[Random.Range(0, def.AllowedWeapons.Length)];

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

        _audioSource = GetComponentInChildren<CharacterAudioSource>();

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

    private void OnMouseOver()
    {
        ToggleHighlight(true);
    }

    private void OnMouseExit()
    {
        ToggleHighlight(false);
    }

    private void OnMouseDown()
    {
        if (EncounterManager.Instance != null)
        {
            EncounterManager.Instance.SelectTarget(this);
        }
    }

    public void ToggleHighlight(bool flag)
    {
        _model.ToggleHighlight(flag);
    }

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

    public virtual void CreateDecal()
    {
        StartCoroutine(Coroutine_CreateDecal());
    }

    private IEnumerator Coroutine_CreateDecal()
    {
        ResourceRequest request = GetPrefab(PrefabID.Character_Decal);

        yield return new WaitUntil(() => request.isDone);

        GameObject decalPrefab = Instantiate((GameObject) request.asset, this.transform);

        if (decalPrefab != null)
        {
            CharacterDecal decal = decalPrefab.GetComponent<CharacterDecal>();

            if (decal != null)
            {
                decal.SetColor(GetTeamByID(GetID()));
            }
        }
    }

    public virtual void DestroyDecal()
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

        GameObject overheadPrefab = Instantiate((GameObject) request.asset, null);

        if (overheadPrefab != null)
        {
            _encounterUI = overheadPrefab.GetComponent<EncounterCharacterUI>();

            if (_encounterUI != null)
            {
                _encounterUI.Setup(this);
            }
        }
    }

    public virtual void DestroyEncounterOverhead()
    {
        if (_encounterUI != null)
        {
            GameObject.Destroy(_encounterUI.gameObject);
        }
    }

    public bool HasAbility(AbilityID abilityID)
    {
        return Constants.GetAllowedAbilities(_ID).Contains(abilityID);
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

    public void PerformSelect()
    {
        CreateDecal();
        _audioSource.Play(CharacterAudioType.Await);
    }

    public void PerformAbility(AbilityID abilityID)
    {
        switch(abilityID)
        {
            case AbilityID.Attack:
            {
                _weapon.OnAbility(AbilityID.Attack);
                _animator.GoTo(AnimState.Attack_Single);
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

    public void PerformDamageHit(DamageInfo damageInfo)
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

    public void PlayAudio(CharacterAudioType audioType)
    {
        _audioSource.Play(audioType);
    }

    public void HandleEvent(object eventData, CharacterEvent characterEvent)
    {
        switch (characterEvent)
        {
            case CharacterEvent.DEAD:
            {
                _health = 0;

            }
            break;
            default:
                break;
        }

        BroadcastEvent(eventData, characterEvent) ;
    }

    private void BroadcastEvent(object eventData, CharacterEvent characterEvent)
    {
        foreach(ICharacterEventReceiver eventReceiver in _eventReceivers)
        {
            eventReceiver.HandleEvent(eventData, characterEvent);
        }
    }
}