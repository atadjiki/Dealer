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

        ModelID modelID = def.AllowedModels[Random.Range(0, def.AllowedModels.Length - 1)];

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
            WeaponID weaponID = def.AllowedWeapons[Random.Range(0, def.AllowedWeapons.Length - 1)];

            ResourceRequest weaponRequest = Resources.LoadAsync<GameObject>(PrefabPaths.GetWeaponByID(weaponID));

            yield return new WaitUntil(() => weaponRequest.isDone);

            GameObject weaponPrefab = Instantiate((GameObject)weaponRequest.asset, _weaponAnchor.transform);

            _weapon = weaponPrefab.GetComponent<CharacterWeapon>();

            yield return new WaitWhile(() => _weapon == null);

        //    Debug.Log("Setup " + _weapon);

            _weapon.SetID(weaponID);
        }

        _overheadAnchor = modelPrefab.GetComponentInChildren<CharacterOverheadAnchor>();

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

    public virtual void HandleDamage(int amount)
    {
        _health -= Mathf.Abs(amount);

        _health = Mathf.Clamp(_health, 0, _health);

        if(IsDead())
        {
            Kill();
        }
        else
        {
            _animator.PerformOneOff(AnimState.Hit, 0.25f);
        }
    }

    public virtual CharacterOverheadAnchor GetOverheadAnchor()
    {
        return _overheadAnchor;
    }
}