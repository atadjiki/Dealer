using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class CharacterComponent : MonoBehaviour
{
    protected CharacterConstants.CharacterID _ID;
    protected CharacterModel _model;
    protected CharacterWeapon _weapon;

    public delegate void OnCharacterSetupComplete(CharacterComponent character);
    public OnCharacterSetupComplete onSetupComplete;

    public void PerformSpawn(CharacterConstants.CharacterID ID)
    {
        StartCoroutine(Coroutine_PerformSpawn(ID));
    }

    protected virtual IEnumerator Coroutine_PerformSpawn(CharacterConstants.CharacterID ID)
    {
        _ID = ID;

        _model = SetupModel();

        yield return new WaitWhile(() => _model == null);

        Debug.Log("model created");
    
        _weapon = SetupWeapon();

        yield return new WaitWhile(() => _weapon == null);

        Debug.Log("weapon created");

        SetupAnimator();

        if(onSetupComplete != null)
        {
            onSetupComplete.Invoke(this);
        }

        yield return null;
    }

    protected virtual CharacterModel SetupModel()
    {
        CharacterDefinition def = CharacterDefinition.Get(_ID);

        CharacterConstants.ModelID modelID = def.AllowedModels[Random.Range(0, def.AllowedModels.Length - 1)];
        GameObject characterModelObject = Instantiate(Resources.Load<GameObject>(PrefabPaths.GetCharacterModel(modelID)), this.transform);

        return characterModelObject.GetComponent<CharacterModel>();
    }

    protected virtual CharacterWeapon SetupWeapon()
    {
        CharacterWeaponAnchor anchor = _model.GetComponentInChildren<CharacterWeaponAnchor>();

        if (anchor != null)
        {
            CharacterDefinition def = CharacterDefinition.Get(_ID);

            if(def.AllowedWeapons.Length > 0)
            {
                CharacterConstants.WeaponID weaponID = def.AllowedWeapons[Random.Range(0, def.AllowedWeapons.Length-1)];
                GameObject characterWeaponObject = Instantiate(Resources.Load<GameObject>(PrefabPaths.GetWeaponByID(weaponID)), anchor.transform);

                CharacterWeapon weaponComponent = characterWeaponObject.GetComponent<CharacterWeapon>();

                if(weaponComponent != null)
                {
                    weaponComponent.SetID(weaponID);
                }

                return weaponComponent;
            }
        }

        Debug.Log("Weapon was null!");
        return null;
    }

    protected virtual void SetupAnimator()
    {
        CharacterAnimator animator = _model.GetComponent<CharacterAnimator>();
        if (animator != null)
        {
            animator.Setup(AnimationConstants.State.Idle, _weapon.GetID());
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
                decal.SetColorByTeam(CharacterConstants.GetTeamByID(_ID));
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
}