using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterPanel : MonoBehaviour
{
    private Dictionary<CharacterComponent, TextMeshProUGUI> characterMap;

    [SerializeField] private GameObject TextMeshPrefab;
    [SerializeField] private Canvas uiCanvas;
    [SerializeField] private Camera uiCamera;
    [SerializeField] private Vector2 offset;

    [SerializeField] private bool displayName;
    [SerializeField] private bool displayID;
    [SerializeField] private bool displayState;
    [SerializeField] private bool displayTask;
    [SerializeField] private bool displayTime;

    private static CharacterPanel _instance;

    public static CharacterPanel Instance { get { return _instance; } }

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
        characterMap = new Dictionary<CharacterComponent, TextMeshProUGUI>();

    }

    private TextMeshProUGUI BuildTextMesh(CharacterComponent characterComponent)
    {
        if (characterComponent != null)
        {
            GameObject textMeshObject = Instantiate(TextMeshPrefab, this.transform);
            TextMeshProUGUI textMesh = textMeshObject.GetComponent<TextMeshProUGUI>();
            return textMesh;
        }

        return null;
    }

    public void RegisterCharacter(CharacterComponent characterModel)
    {
        if (characterMap.ContainsKey(characterModel) == false)
        {
            characterMap.Add(characterModel, BuildTextMesh(characterModel));
            Debug.Log("Registered character " + characterModel.gameObject.name);
        }
    }

    public void UnRegisterCharacter(CharacterComponent characterComponent)
    {
        if (characterMap.ContainsKey(characterComponent))
        {
            Destroy(characterMap[characterComponent].gameObject);
            characterMap.Remove(characterComponent);
        }
    }

    private void FixedUpdate()
    {
        foreach (CharacterComponent character in characterMap.Keys)
        {
            int lines;

            TextMeshProUGUI textMesh = characterMap[character];
            textMesh.text = BuildCharacterString(character, out lines);

            if (character.GetAnimationComponent().IsVisible())
            {
                textMesh.color = Color.yellow;
                textMesh.alpha = 0.80f;
        }
            else
            {
                textMesh.color = Color.grey;
                textMesh.alpha = 0.25f;
            }

            GameObject model = character.GetAnimationComponent().gameObject;

            UIAnchor anchor = model.GetComponentInChildren<UIAnchor>();

            Vector3 anchorPoint = anchor.transform.position;

            Vector2 targetScreenPoint = WorldToCanvas(uiCanvas, anchorPoint, uiCamera);

            textMesh.rectTransform.anchoredPosition = targetScreenPoint + (offset * lines);
        }
    }

    private string BuildCharacterString(CharacterComponent character, out int lines)
    {
        lines = 0;
        string characterString = "";

        if (displayName)
        {
            characterString += character.GetCharacterInfo().name;
            lines++;
        }

        if (displayID && character.GetAnimationComponent().IsVisible())
        {
            characterString += "\n" + character.GetCharacterInfo().ID;
            lines++;
        }
        if (displayState)
        {
            characterString += "\n" + Constants.CharacterConstants.StateToString(character.GetState());

            lines++;
        }
        if (displayTask)
        {
            characterString += "\n" + character.GetTaskComponent().GetTask().Type;

            lines++;
        }
        if (displayTime && character.GetAnimationComponent().IsVisible())
        {
            if (character.timeSinceLastUpdate > 0)
            {
                characterString += "\n" + string.Format("{0:0.#}", character.timeSinceLastUpdate) + "/" + ((int)character.updateTime);
                lines++;
            }
        }

        return characterString;
    }


    public static Vector2 WorldToCanvas(Canvas canvas, Vector3 worldPoint, Camera camera)
    {
        Vector2 viewportPos = camera.WorldToViewportPoint(worldPoint);
        RectTransform canvasRect = (RectTransform)canvas.transform;

        return new Vector2((viewportPos.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f),
            (viewportPos.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f));
    }

}
