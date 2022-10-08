using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CharacterMarker : MonoBehaviour
{
    private CharacterInfo _info;
    private CharacterModel _model;
    private CharacterCanvas _canvas;

    public void Setup(CharacterInfo characterInfo)
    {
        _info = characterInfo;

        SetupCharacterModel();
        SetupCharacterCanvas();
    }

    private void SetupCharacterModel()
    {
        GameObject spawnedCharacter = Instantiate(PrefabManager.Instance.GetCharacterModel(_info.CharacterModelID), this.transform);
        spawnedCharacter.transform.eulerAngles = this.transform.eulerAngles;

        CharacterModel model = spawnedCharacter.GetComponent<CharacterModel>();
        model.ApplyCharacterInfo(_info);
    }

    private void SetupCharacterCanvas()
    {
        GameObject characterCanvasPrefab = Instantiate(
    PrefabManager.Instance.GetUIElement(Enumerations.UIID.CharacterCanvas), this.transform);

        characterCanvasPrefab.GetComponentInChildren<RectTransform>().localEulerAngles = this.transform.eulerAngles * -1;

        _canvas = characterCanvasPrefab.GetComponent<CharacterCanvas>();
        _canvas.SetName(_info.CharacterName); 
    }

    private void OnMouseDown()
    {
        if(_info != null)
        {
            Debug.Log("Clicked on " + _info.CharacterName);
        }
    }
}
