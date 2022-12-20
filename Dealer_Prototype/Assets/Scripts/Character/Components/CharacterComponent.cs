using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

[ExecuteAlways]
public class CharacterComponent : MonoBehaviour
{
    public delegate void OnShowDecal(Enumerations.Team team);
    public OnShowDecal OnShowDecalDelegate;

    public delegate void OnHideDecal();
    public OnHideDecal OnHideDecalDelegate;

    public delegate void OnToggleCharacterCanvas(bool flag);
    public OnToggleCharacterCanvas OnToggleCanvasDelegate;

    protected Enumerations.CharacterModelID _modelID = Enumerations.CharacterModelID.Model_Male1;
    protected Enumerations.Team _team = Enumerations.Team.Neutral;

    protected CharacterModel model;

    public virtual void ProcessSpawnData(object _data)
    {
        _modelID = ((CharacterSpawnData)_data).ModelID;
    }

    public void Initialize()
    {
        if (Application.isPlaying)
        {
            StartCoroutine(PerformInitialize());
        }
    }

    public virtual IEnumerator PerformInitialize()
    {

        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }


        //load our associated model
        GameObject spawnedCharacter = Instantiate(PrefabLibrary.GetCharacterModelByID(_modelID), this.transform);
        model = spawnedCharacter.GetComponent<CharacterModel>();
        model.SetTeam(_team);

        model.OnModelClickedDelegate += OnCharacterClicked;


        yield return new WaitUntil(() => model != null);

        //attempt to add a collider
        CapsuleCollider inputCollider = model.gameObject.AddComponent<CapsuleCollider>();
        inputCollider.isTrigger = true;
        inputCollider.height = 1.75f;
        inputCollider.radius = 0.5f;
        inputCollider.center = new Vector3(0, 0.75f);

        //add a ground decal
        GameObject groundDecalObject = Instantiate(PrefabLibrary.GetCharacterGroundDecal(), model.gameObject.transform);
        CharacterGroundDecal groundDecal = groundDecalObject.GetComponent<CharacterGroundDecal>();
        OnShowDecalDelegate += groundDecal.Show;
        OnHideDecalDelegate += groundDecal.Hide;

        OnShowDecalDelegate.Invoke(_team);

        //add a character canvas
        GameObject canvasObject = Instantiate(PrefabLibrary.GetCharacterCanvas(), model.gameObject.transform);
        CharacterCanvas characterCanvas = canvasObject.GetComponent<CharacterCanvas>();
        characterCanvas.SetName(_modelID.ToString());
        OnToggleCanvasDelegate += characterCanvas.Toggle;
        OnToggleCanvasDelegate.Invoke(false);

        yield return null;
    }

    protected virtual void ShowGroundDecal()
    {
        OnShowDecalDelegate.Invoke(_team);
    }

    protected virtual void HideGroundDecal()
    {
        OnHideDecalDelegate.Invoke();
    }

    protected virtual void OnCharacterClicked()
    {
        Debug.Log(_modelID.ToString() + " clicked");
    }
}
