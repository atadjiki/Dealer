using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CharacterPortraitCamera : MonoBehaviour
{
    private Camera portraitCamera;
    private CharacterComponent character;

    private static CharacterPortraitCamera _instance;

    public static CharacterPortraitCamera Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        Build();
    }

    private void Build()
    {
        portraitCamera = GetComponent<Camera>();
        portraitCamera.enabled = false;
    }

    public void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }

    public void SetCharacter(CharacterComponent inCharacter)
    {
        character = inCharacter;

        SetLayerRecursively(inCharacter.GetModel(), LayerMask.NameToLayer("Speaker"));

        portraitCamera.enabled = true;

        portraitCamera.transform.parent = character.GetNavigatorComponent().transform;
        portraitCamera.transform.localPosition = new Vector3(0, 1.65f, 1.3f);
        portraitCamera.transform.localEulerAngles = new Vector3(8, 180, 0);
        portraitCamera.transform.localScale = new Vector3(1, 1, 1);
    }

    public void Reset()
    {
        SetLayerRecursively(character.GetModel(), LayerMask.NameToLayer("Character"));
        character = null;

        portraitCamera.transform.parent = ConversationManager.Instance.transform;
        portraitCamera.transform.position = Vector3.zero;
        portraitCamera.transform.eulerAngles = Vector3.zero;

        portraitCamera.enabled = false;
    }
}
