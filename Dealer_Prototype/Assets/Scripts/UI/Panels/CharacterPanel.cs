using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterPanel : MonoBehaviour
{
    private Dictionary<CharacterModel, TextMeshProUGUI> characterMap;

    [SerializeField] private GameObject TextMeshPrefab;
    [SerializeField] private Canvas uiCanvas;
    [SerializeField] private Camera uiCamera;

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
        characterMap = new Dictionary<CharacterModel, TextMeshProUGUI>();

    }

    private TextMeshProUGUI BuildTextMesh(CharacterModel characterModel)
    {
        if(characterModel != null)
        {
            GameObject textMeshObject = Instantiate(TextMeshPrefab, this.transform);
            TextMeshProUGUI textMesh = textMeshObject.GetComponent<TextMeshProUGUI>();
            textMesh.text = characterModel.GetID();
            return textMesh;
        }

        return null;
    }

    public void RegisterCharacter(CharacterModel characterModel)
    {
        if(characterMap.ContainsKey(characterModel) == false)
        {
            characterMap.Add(characterModel, BuildTextMesh(characterModel));
            Debug.Log("Registered model " + characterModel.gameObject.name);
        }
    }

    public void UnRegisterCharacter(CharacterModel characterModel)
    {
        if(characterMap.ContainsKey(characterModel))
        {
            Destroy(characterMap[characterModel].gameObject);
            characterMap.Remove(characterModel);
        }
    }

    private void FixedUpdate()
    {
        foreach (CharacterModel characterModel in characterMap.Keys)
        {
            if(characterModel.GetModel())
            {
                TextMeshProUGUI textMesh = characterMap[characterModel];

                GameObject model = characterModel.GetModel();

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
