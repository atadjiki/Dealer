using System.Collections;
using System.Collections.Generic;
using static Constants;
using UnityEngine;

public class CharacterComponent : MonoBehaviour, ICharacterEventReceiver
{
    protected CharacterID _ID;

    //Event Receivers
    protected CharacterModel _model;
    protected CharacterAbilityHandler _abilityHandler;
    protected CharacterNavigator _navigator;
    protected CharacterWeapon _weapon;
    protected CharacterAnimator _animator;
    protected CharacterAudioSource _audioSource;
    protected CharacterOutlineController _outline;
    protected EncounterCharacterUI _encounterUI;

    private List<ICharacterEventReceiver> _eventReceivers;

    protected CharacterOverheadAnchor _overheadAnchor;
    protected CharacterWeaponAnchor _weaponAnchor;

    protected CharacterDecal _decal;

    protected int _health = 0;
    protected int _baseHealth = 0;

    protected int _actionPoints = 0;
    protected int _baseActionPoints = 0;

    protected int _movementRange = 0;

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

        _baseActionPoints = characterDefinition.BaseActionPoints;
        _actionPoints = characterDefinition.BaseActionPoints;

        _movementRange = characterDefinition.MovementRange;

        _eventReceivers = new List<ICharacterEventReceiver>();

        //create an ability handler for this character
        _abilityHandler = this.gameObject.AddComponent<CharacterAbilityHandler>();
        _abilityHandler.Setup(this);
        _eventReceivers.Add(_abilityHandler);

        //create a navigator for the character
        ResourceRequest navigatorRequest = GetCharacterComponent(PrefabID.Character_Navigator);
        yield return new WaitUntil(() => navigatorRequest.isDone);
        GameObject navigatorObject = Instantiate<GameObject>((GameObject)navigatorRequest.asset, this.transform);
        _navigator = navigatorObject.GetComponent<CharacterNavigator>();

        //add a colider for mouse interaction
        _collider = navigatorObject.AddComponent<CapsuleCollider>();
        yield return new WaitWhile(() => _collider == null);
        _collider.isTrigger = true;
        _collider.radius = 0.5f;
        _collider.height = 2.0f;
        _collider.center = new Vector3(0, 1.0f, 0);

        //get model from character ID and attach to navigator
        ModelID modelID = characterDefinition.AllowedModels[Random.Range(0, characterDefinition.AllowedModels.Length)];
        ResourceRequest modelRequest = GetCharacterModel(modelID);
        yield return new WaitUntil(() => modelRequest.isDone);
        GameObject modelPrefab = Instantiate((GameObject)modelRequest.asset, navigatorObject.transform);
        _model = modelPrefab.GetComponent<CharacterModel>();
        _eventReceivers.Add(_model);
        yield return new WaitUntil(() => _model != null);

        //grab UI anchor from model
        _overheadAnchor = GetComponentInChildren<CharacterOverheadAnchor>();

        //get weapon model for character 
        _weaponAnchor = modelPrefab.GetComponentInChildren<CharacterWeaponAnchor>();
        Transform weaponAnchorOffset = null;

        yield return new WaitWhile(() => _weaponAnchor == null);
        if (characterDefinition.AllowedWeapons.Length > 0)
        {
            WeaponID weaponID = characterDefinition.AllowedWeapons[Random.Range(0, characterDefinition.AllowedWeapons.Length)];

            ResourceRequest weaponRequest = GetWeaponModel(weaponID);

            yield return new WaitUntil(() => weaponRequest.isDone);

            weaponAnchorOffset = _weaponAnchor.GetOffset(weaponID);

            GameObject weaponPrefab = Instantiate((GameObject)weaponRequest.asset, weaponAnchorOffset);

            weaponPrefab.transform.parent = weaponAnchorOffset;

            weaponPrefab.transform.localPosition = Vector3.zero;
            weaponPrefab.transform.localRotation = Quaternion.identity;

            _weapon = weaponPrefab.GetComponent<CharacterWeapon>();

            _weapon.Setup(_ID, weaponID);

            _eventReceivers.Add(_weapon);
        }

        //create a decal for this character
        ResourceRequest decalRequest = GetCharacterComponent(PrefabID.Character_Decal);
        yield return new WaitUntil(() => decalRequest.isDone);
        GameObject decalObject = Instantiate<GameObject>((GameObject)decalRequest.asset, navigatorObject.transform);
        _decal = decalObject.GetComponent<CharacterDecal>();
        _decal.SetColor(GetTeam());

        //create an outline container for the model 
        ResourceRequest outlineRequest = GetCharacterComponent(PrefabID.Character_Outliner);
        yield return new WaitUntil(() => outlineRequest.isDone);
        GameObject outlineObject = Instantiate<GameObject>((GameObject)outlineRequest.asset, this.transform);
        _outline = outlineObject.GetComponent<CharacterOutlineController>();
        _outline.Setup(characterDefinition, _model.gameObject);
        _eventReceivers.Add(_outline);

        //create an audio source
        ResourceRequest audioSourceRequest = GetAudioSource(_ID);
        yield return new WaitUntil(()=>audioSourceRequest.isDone);
        GameObject audioSourceObject = Instantiate<GameObject>((GameObject)audioSourceRequest.asset, navigatorObject.transform);
        _audioSource = audioSourceObject.GetComponentInChildren<CharacterAudioSource>();
        _eventReceivers.Add(_audioSource);

        //grab animator from model 
        _animator = modelPrefab.GetComponent<CharacterAnimator>();
        yield return new WaitUntil(() => _animator != null);
        _animator.Setup(AnimID.Idle, _weapon.GetID(), weaponAnchorOffset);
        _eventReceivers.Add(_animator);

        //place the character at an appropriate spawn location
        if(EnvironmentManager.IsActive())
        {
            EnvironmentManager.Instance.FindSpawnLocationForCharacter(this);
        }

        if (onSetupComplete != null)
        {
            onSetupComplete.Invoke(this);
        }

        yield return null;
    }

    public void HandleEvent(CharacterEvent characterEvent, object eventData = null)
    {
        if (!_canReceive) { return; }

        switch (characterEvent)
        {
            case CharacterEvent.DAMAGE:
            {
                if (IsAlive())
                {
                    DamageInfo damageInfo = (DamageInfo)eventData;

                    _health -= Mathf.Abs(damageInfo.ActualDamage);

                    _health = Mathf.Clamp(_health, 0, _health);

                    SetHealth(_health);
                }
                break;
            }
            case CharacterEvent.DEATH:
            {
                _health = 0;
                break;
            }
            case CharacterEvent.HEAL:
            {
                int amount = _baseHealth / 4;
                _health += amount;
                _health = Mathf.Clamp(_health, _health, _baseHealth);
                break;
            }
            default:
                break;
        }

        BroadcastEvent(eventData, characterEvent);
    }
    
    private void BroadcastEvent(object eventData, CharacterEvent characterEvent)
    {
        foreach(ICharacterEventReceiver eventReceiver in _eventReceivers)
        {
            eventReceiver.HandleEvent(characterEvent, eventData);
        }
    }

    //Getters and Setters
    public bool HasAbility(AbilityID abilityID)
    {
        return GetAllowedAbilities(_ID).Contains(abilityID);
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

    public bool IsHealthFull()
    {
        return _baseHealth == _health;
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
        _outline.ToggleHighlight(flag);
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

        Destroy(modelObject);

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
        ResourceRequest request = GetCharacterComponent(PrefabID.Character_Decal);

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
            Destroy(decal.gameObject);
        }
    }

    public virtual void CreateEncounterOverhead()
    {
        StartCoroutine(Coroutine_CreateEncounterOverhead());
    }

    private IEnumerator Coroutine_CreateEncounterOverhead()
    {
        ResourceRequest request = GetEncounterUI(PrefabID.Encounter_UI_CharacterUI_Canvas);

        yield return new WaitUntil(() => request.isDone);

        GameObject overheadPrefab = Instantiate((GameObject)request.asset, this.transform);

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

    public CharacterNavigator GetNavigator()
    {
        return _navigator;
    }

    public Vector3 GetWorldLocation()
    {
        return _navigator.transform.position;
    }

    public Quaternion GetWorldRotation()
    {
        return _navigator.transform.rotation;
    }

    public Vector3 GetWorldEulerAngles()
    {
        return _navigator.transform.eulerAngles;
    }

    public void SetWorldEulerAngles(Vector3 eulerAngles)
    {
        _navigator.transform.transform.eulerAngles = eulerAngles;
    }

    public void SetWorldRotation(Quaternion rotation)
    {
        _navigator.transform.rotation = rotation;
    }

    public void ReplenishActionPoints()
    {
        _actionPoints = _baseActionPoints;
    }

    public int DecrementActionPoints(int cost)
    {
        _actionPoints -= cost;

        return (int) Mathf.Clamp(_actionPoints, 0, _baseActionPoints);
    }

    public bool HasActionPoints()
    {
        return _actionPoints > 0;
    }

    public int GetActionPoints()
    {
        return _actionPoints;
    }

    public float GetMovementRange()
    {
        return _movementRange * TILE_SIZE;
    }

    public WeaponAttackDefinition GetWeaponAttackDefinition()
    {
        return _weapon.GetWeaponAttackDefinition();
    }

    public void RotateTowards(CharacterComponent target)
    {
        _abilityHandler.RotateTowards(target);
    }

    public void PerformAbility(AbilityID abilityID, CharacterComponent target = null, Vector3 destination = new Vector3())
    {
        StartCoroutine(_abilityHandler.PerformAbility(abilityID, target, destination));
    }

    public IEnumerator Coroutine_PerformAbility(AbilityID abilityID, CharacterComponent target = null, Vector3 destination = new Vector3())
    {
        yield return _abilityHandler.PerformAbility(abilityID, target, destination);
    }

    public Transform GetRandomBodyPart()
    {
        return _model.GetRandomBodyPart();
    }

    public void FaceNearestEnemy()
    {
        CharacterComponent closestEnemy = GetClosestEnemy();

        if (closestEnemy != null)
        {
            RotateTowards(closestEnemy);
        }
    }

    public CharacterComponent GetClosestEnemy()
    {
        List<CharacterComponent> enemies = EncounterManager.Instance.GetAllCharactersInTeam(GetOpposingTeam(GetTeam()));

        if (enemies.Count > 0)
        {
            CharacterComponent closestEnemy = enemies[0];

            float minDistance = Vector3.Distance(GetWorldLocation(), closestEnemy.GetWorldLocation());

            foreach (CharacterComponent enemy in enemies)
            {
                float distance = Vector3.Distance(GetWorldLocation(), enemy.GetWorldLocation());

                if (distance < minDistance)
                {
                    closestEnemy = enemy;
                }
            }

            return closestEnemy;
        }

        return null;
    }

    public float GetWeaponRange()
    {
        return WeaponDefinition.Get(_weapon.GetID()).Range * TILE_SIZE;
    }
}