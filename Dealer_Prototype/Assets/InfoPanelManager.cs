using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoPanelManager : MonoBehaviour
{
    private static InfoPanelManager _instance;

    public static InfoPanelManager Instance { get { return _instance; } }

    [SerializeField] private GameObject textMeshPrefab;
    [SerializeField] private Camera uiCamera;
    [SerializeField] private Canvas uiCanvas;

    private Dictionary<GameObject, GameObject> targetMap; //map game object to panel

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

        targetMap = new Dictionary<GameObject, GameObject>();
    }

    private void FixedUpdate()
    {
        foreach(GameObject target in targetMap.Keys)
        {
            TextMeshProUGUI textMesh = targetMap[target].GetComponent<TextMeshProUGUI>();
            Vector2 targetScreenPoint = WorldToCanvas(uiCanvas, target.transform.position, uiCamera);

            textMesh.rectTransform.anchoredPosition = targetScreenPoint;
        }
    }

    private GameObject BuildUIForTarget(GameObject target)
    {
        GameObject textMeshObject = Object.Instantiate<GameObject>(textMeshPrefab, this.transform);
        TextMeshProUGUI textMesh = textMeshObject.GetComponent<TextMeshProUGUI>();
        textMesh.text = target.name;

        return textMeshObject;
    }


    public static Vector2 WorldToCanvas(Canvas canvas, Vector3 worldPoint, Camera camera)
    {
        Vector2 viewportPos = camera.WorldToViewportPoint(worldPoint);
        RectTransform canvasRect = (RectTransform)canvas.transform;

        return new Vector2((viewportPos.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f),
            (viewportPos.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f));
    }


    public void RegisterTarget(GameObject target)
    {
        if (targetMap.ContainsKey(target) == false)
        {
            targetMap.Add(target, BuildUIForTarget(target));
        }
    }

    public void UnRegisterTarget(GameObject target)
    {
        Destroy(targetMap[target]);
        targetMap.Remove(target);
    }
}
