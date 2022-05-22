using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCPanel : MonoBehaviour
{
    private Dictionary<BasicCharacter, TextMeshProUGUI> characterMap;

    [SerializeField] private GameObject TextMeshPrefab;
    [SerializeField] private Canvas uiCanvas;
    [SerializeField] private Camera uiCamera;

    private static NPCPanel _instance;

    public static NPCPanel Instance { get { return _instance; } }

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
        characterMap = new Dictionary<BasicCharacter, TextMeshProUGUI>();

    }

    private TextMeshProUGUI BuildTextMesh(BasicCharacter character)
    {
        if(character != null)
        {
            GameObject textMeshObject = Instantiate(TextMeshPrefab, this.transform);
            TextMeshProUGUI textMesh = textMeshObject.GetComponent<TextMeshProUGUI>();
            textMesh.text = character.GetID();
            return textMesh;
        }

        return null;
    }

    public void RegisterCharacter(BasicCharacter character)
    {
        if(characterMap.ContainsKey(character) == false)
        {
            characterMap.Add(character, BuildTextMesh(character));
        }
    }

    public void UnRegisterCharacter(BasicCharacter character)
    {
        if(characterMap.ContainsKey(character))
        {
            Destroy(characterMap[character].gameObject);
            characterMap.Remove(character);
        }
    }

    private void FixedUpdate()
    {
        foreach (BasicCharacter character in characterMap.Keys)
        {
            if(character.GetModel())
            {
                TextMeshProUGUI textMesh = characterMap[character];

                GameObject model = character.GetModel();

                UIAnchor anchor = model.GetComponentInChildren<UIAnchor>();

                Vector3 anchorPoint = anchor.transform.position;

                Vector2 targetScreenPoint = WorldToCanvas(uiCanvas, anchorPoint, uiCamera);

                Vector2 offset = new Vector2(0, 50);

                textMesh.rectTransform.anchoredPosition = targetScreenPoint + offset;
            }
 
        }
    }

    public static Vector2 WorldToCanvas(Canvas canvas, Vector3 worldPoint, Camera camera)
    {
        Vector2 viewportPos = camera.WorldToViewportPoint(worldPoint);
        RectTransform canvasRect = (RectTransform)canvas.transform;

        return new Vector2((viewportPos.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f),
            (viewportPos.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f));
    }

}
