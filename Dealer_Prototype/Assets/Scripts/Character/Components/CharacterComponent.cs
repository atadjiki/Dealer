using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

[ExecuteAlways]
public class CharacterComponent : MonoBehaviour
{
    protected Enumerations.CharacterModelID _modelID = Enumerations.CharacterModelID.Model_Male1;

    protected CharacterModel model;

    protected CapsuleCollider inputCollider;

    protected CharacterGroundDecal groundDecal;

    protected CharacterCanvas characterCanvas;

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

        if (model == null)
        {
            //load our associated model
            GameObject spawnedCharacter = Instantiate(PrefabLibrary.GetCharacterModelByID(_modelID), this.transform);
            model = spawnedCharacter.GetComponent<CharacterModel>();
            yield return new WaitUntil(() => model != null);
        }

        //attempt to add a collider
        inputCollider = this.gameObject.AddComponent<CapsuleCollider>();
        inputCollider.isTrigger = true;
        inputCollider.height = 1.75f;
        inputCollider.radius = 0.5f;
        inputCollider.center = new Vector3(0, 0.75f);

        //add a ground decal
        GameObject groundDecalObject = Instantiate(PrefabLibrary.GetCharacterGroundDecal(), model.gameObject.transform);
        groundDecal = groundDecalObject.GetComponent<CharacterGroundDecal>();
        ShowGroundDecal();

        //add a character canvas
        GameObject canvasObject = Instantiate(PrefabLibrary.GetCharacterCanvas(), model.gameObject.transform);
        characterCanvas = canvasObject.GetComponent<CharacterCanvas>();
        characterCanvas.Toggle(false);


        yield return null;
    }

    protected virtual void ShowGroundDecal()
    {
        MaterialHelper.SetNeutralGroundDecal(groundDecal);
    }

    protected virtual void HideGroundDecal()
    {
        MaterialHelper.HideGroundDecal(groundDecal);
    }

    protected virtual void Highlight()
    {
        MaterialHelper.SetNeutralOutline(model);
    }

    protected virtual void Unhighlight()
    {
        MaterialHelper.ResetCharacterOutline(model);
    }

    private void OnMouseDown()
    {
        Debug.Log("mouse click on " + _modelID.ToString());
    }

    private void OnMouseEnter()
    {
        Highlight();
    }

    private void OnMouseExit()
    {
        Unhighlight();
    }
}
