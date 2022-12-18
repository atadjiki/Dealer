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

        yield return null;
    }

    protected virtual void Highlight()
    {
        MaterialHelper.SetNeutralOutline(model);
    }

    protected virtual void Unhighlight()
    {
        MaterialHelper.ResetCharacterOutline(model);
    }

    private void OnMouseEnter()
    {
        Debug.Log("mouse over " + _modelID.ToString());

        Highlight();
    }

    private void OnMouseExit()
    {
        Unhighlight();
    }
}
