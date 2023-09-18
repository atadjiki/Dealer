using System.Collections;
using System.Collections.Generic;
using static Constants;
using UnityEngine;

public class CharacterComponent : MonoBehaviour
{
    protected CharacterID _ID;
    protected CharacterModel _model;
    protected CharacterOverheadAnchor _overheadAnchor;
    protected CharacterWeaponAnchor _weaponAnchor;
    protected CharacterWeapon _weapon;
    protected CharacterAnimator _animator;
    protected int _health = 0;
    protected int _baseHealth = 0;

    protected CapsuleCollider _collider;

    public delegate void OnCharacterSetupComplete(CharacterComponent character);
    public OnCharacterSetupComplete onSetupComplete;

    public void SetID(CharacterID ID)
    {
        _ID = ID;
    }

    public virtual IEnumerator SpawnCharacter()
    {
        CharacterDefinition def = CharacterDefinition.Get(_ID);

        _health = def.BaseHealth;
        _baseHealth = def.BaseHealth;

        ModelID modelID = def.AllowedModels[Random.Range(0, def.AllowedModels.Length)];

        ResourceRequest modelRequest = Resources.LoadAsync<GameObject>(PrefabPaths.GetCharacterModel(modelID));

        yield return new WaitUntil(() => modelRequest.isDone);

        GameObject modelPrefab = Instantiate((GameObject)modelRequest.asset, this.transform);

        _model = modelPrefab.GetComponent<CharacterModel>();

        _model.Setup(def);

        yield return new WaitWhile(() => _model == null);
        
        _weaponAnchor = modelPrefab.GetComponentInChildren<CharacterWeaponAnchor>();

        yield return new WaitWhile(() => _weaponAnchor == null);

        if (def.AllowedWeapons.Length > 0)
        {
            WeaponID weaponID = def.AllowedWeapons[Random.Range(0, def.AllowedWeapons.Length)];

            ResourceRequest weaponRequest = Resources.LoadAsync<GameObject>(PrefabPaths.GetWeaponByID(weaponID));

            yield return new WaitUntil(() => weaponRequest.isDone);

            GameObject weaponPrefab = Instantiate((GameObject)weaponRequest.asset, _weaponAnchor.transform);

            _weapon = weaponPrefab.GetComponent<CharacterWeapon>();

            yield return new WaitWhile(() => _weapon == null);

            _weapon.Setup(weaponID);
        }

        _overheadAnchor = GetComponentInChildren<CharacterOverheadAnchor>();

        yield return new WaitWhile(() => _overheadAnchor == null);

        _animator = modelPrefab.GetComponent<CharacterAnimator>();

        yield return new WaitWhile(() => _animator == null);

        _animator.Setup(AnimState.Idle, _weapon.GetID());

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

    public void Kill()
    {
        _health = 0;
        _animator.GoTo(AnimState.Dead);
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

        yield return new WaitUntil( () => modelObject == null );
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
        GameObject decalPrefab = Instantiate(Resources.Load<GameObject>(PrefabPaths.Path_Character_Decal), this.transform);

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

        if(decal != null)
        {
            GameObject.Destroy(decal.gameObject);
        }
    }

    public virtual void CreateEncounterOverhead()
    {
        GameObject overheadPrefab = Instantiate(Resources.Load<GameObject>(PrefabPaths.Path_Character_Encounter_Overhead), null);

        if (overheadPrefab != null)
        {
            EncounterCharacterUI overhead = overheadPrefab.GetComponent<EncounterCharacterUI>();

            if (overhead != null)
            {
                overhead.Setup(this);
            }
        }
    }

    public virtual void DestroyEncounterOverhead()
    {
        EncounterCharacterUI characterUI = GetComponentInChildren<EncounterCharacterUI>();

        if (characterUI != null)
        {
            GameObject.Destroy(characterUI.gameObject);
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

        if(_health < 1)
        {
            Kill();
        }
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

    public IEnumerator Coroutine_FireWeaponAt(CharacterComponent target)
    {
        //rotate and pause momentarily
        yield return Coroutine_RotateTowards(target);
        yield return new WaitForSeconds(0.5f);
        _weapon.OnAttack();
        _animator.GoTo(AnimState.Attack_Single);
    }

    public IEnumerator Coroutine_RotateTowards(CharacterComponent target)
    {
        float currentTime = 0;
        float duration = 0.15f;

        Quaternion casterRotation = this.transform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(casterRotation, targetRotation, currentTime / duration);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            yield return null;
        }

        transform.rotation = targetRotation;
    }

    public virtual IEnumerator Coroutine_PerformReload()
    {
        _weapon.Reload();

        _animator.GoTo(AnimState.Reload);

        yield return new WaitForSeconds(1.0f);
    }

    public virtual IEnumerator Coroutine_HandleDamage(DamageInfo damageInfo)
    {
        //calculate how much damage we should deal
        _health -= Mathf.Abs(damageInfo.ActualDamage);

        _health = Mathf.Clamp(_health, 0, _health);

        if(IsDead())
        {
            Kill();
            yield return new WaitForSeconds(0.5f);
        }
        else
        {
            if (damageInfo.ActualDamage < damageInfo.BaseDamage)
            {
                _animator.GoTo(AnimState.Hit_Light);
            }
            else if(damageInfo.ActualDamage == damageInfo.BaseDamage)
            {
                _animator.GoTo(AnimState.Hit_Medium);
            }
            else
            {
                _animator.GoTo(AnimState.Hit_Heavy);
            }

            yield return new WaitForSeconds(0.5f);
        }
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
}