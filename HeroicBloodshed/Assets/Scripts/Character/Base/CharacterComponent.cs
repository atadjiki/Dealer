using System.Collections;
using System.Collections.Generic;
using static Constants;
using UnityEngine;

public class CharacterComponent : MonoBehaviour
{
    protected CharacterID _ID;
    protected CharacterModel _model;
    protected CharacterWeapon _weapon;
    protected int _health = 0;

    public delegate void OnCharacterSetupComplete(CharacterComponent character);
    public OnCharacterSetupComplete onSetupComplete;

    public void SetID(CharacterID ID)
    {
        _ID = ID;
    }

    public void PerformSetup()
    {
        StartCoroutine(Coroutine_PerformSetup());
    }

    protected virtual IEnumerator Coroutine_PerformSetup()
    {
        CharacterDefinition def = CharacterDefinition.Get(_ID);

        _health = def.BaseHealth;

        SetupModel();

        yield return new WaitWhile(() => _model == null);

        Debug.Log("model created");
    
        SetupWeapon();

        yield return new WaitWhile(() => _weapon == null);

        Debug.Log("weapon created");

        SetupAnimator();

        if(onSetupComplete != null)
        {
            onSetupComplete.Invoke(this);
        }

        yield return null;
    }

    public virtual void SetupModel()
    {
        CharacterDefinition def = CharacterDefinition.Get(_ID);

        ModelID modelID = def.AllowedModels[Random.Range(0, def.AllowedModels.Length - 1)];
        GameObject characterModelObject = Instantiate(Resources.Load<GameObject>(PrefabPaths.GetCharacterModel(modelID)), this.transform);

        _model = characterModelObject.GetComponent<CharacterModel>();
    }

    public virtual void DestroyModel()
    {
        GameObject modelObject = _model.gameObject;

        _model = null;

        GameObject.Destroy(modelObject);
    }

    public virtual void SetupWeapon()
    {
        CharacterWeaponAnchor anchor = _model.GetComponentInChildren<CharacterWeaponAnchor>();

        if (anchor != null)
        {
            CharacterDefinition def = CharacterDefinition.Get(_ID);

            if(def.AllowedWeapons.Length > 0)
            {
                WeaponID weaponID = def.AllowedWeapons[Random.Range(0, def.AllowedWeapons.Length-1)];
                GameObject characterWeaponObject = Instantiate(Resources.Load<GameObject>(PrefabPaths.GetWeaponByID(weaponID)), anchor.transform);

                CharacterWeapon weaponComponent = characterWeaponObject.GetComponent<CharacterWeapon>();

                if(weaponComponent != null)
                {
                    weaponComponent.SetID(weaponID);
                }

                _weapon = weaponComponent;
            }
        }
    }

    public virtual void DestroyWeapon()
    {
        GameObject weaponObject = _weapon.gameObject;

        _weapon = null;

        GameObject.Destroy(weaponObject);
    }

    protected virtual void SetupAnimator()
    {
        CharacterAnimator animator = _model.GetComponent<CharacterAnimator>();
        if (animator != null)
        {
            animator.Setup(AnimState.Idle, _weapon.GetID());
        }
    }

    public virtual void CreateDecal()
    {
        GameObject decalPrefab = Instantiate(Resources.Load<GameObject>(PrefabPaths.Path_Character_Decal), this.transform);

        if (decalPrefab != null)
        {
            CharacterDecal decal = decalPrefab.GetComponent<CharacterDecal>();

            if (decal != null)
            {
                decal.SetColorByTeam(GetTeamByID(_ID));
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

    public virtual bool IsAlive()
    {
        return _health > 0;
    }

    public virtual void SubtractHealth(int amount)
    {
        _health -= Mathf.Abs(amount);

        _health = Mathf.Clamp(_health, 0, _health);
    }
}