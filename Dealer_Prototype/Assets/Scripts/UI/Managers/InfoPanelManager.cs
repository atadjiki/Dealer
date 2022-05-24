using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoPanelManager : MonoBehaviour
{
    private static InfoPanelManager _instance;

    public static InfoPanelManager Instance { get { return _instance; } }

    [SerializeField] private GameObject interactablePanelPrefab;
    [SerializeField] private Camera uiCamera;
    [SerializeField] private Canvas uiCanvas;

    private Dictionary<Interactable, InteractableUIPanel> uiMap; //map interactable to panel

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

        uiMap = new Dictionary<Interactable, InteractableUIPanel>();
    }

    private void FixedUpdate()
    {
        foreach(Interactable interactable in uiMap.Keys)
        {
            InteractableUIPanel interactableUIPanel = uiMap[interactable];

            Vector3 anchorPoint = interactable.GetComponent<Collider>().bounds.max;

            Vector2 targetScreenPoint = WorldToCanvas(uiCanvas, anchorPoint, uiCamera);

            Vector2 offset = new Vector2(0, -100);

            ((RectTransform) interactableUIPanel.gameObject.transform).anchoredPosition = targetScreenPoint + offset;
        }
    }

    private InteractableUIPanel BuildUIForTarget(GameObject panelPrefab)
    {
        GameObject interactablePanelObject = Instantiate(panelPrefab, transform);
        InteractableUIPanel interactableUIPanel = interactablePanelObject.GetComponent<InteractableUIPanel>();

        return interactableUIPanel;
    }


    public static Vector2 WorldToCanvas(Canvas canvas, Vector3 worldPoint, Camera camera)
    {
        Vector2 viewportPos = camera.WorldToViewportPoint(worldPoint);
        RectTransform canvasRect = (RectTransform)canvas.transform;

        return new Vector2((viewportPos.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f),
            (viewportPos.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f));
    }


    public void RegisterInteractable(Interactable interactable, GameObject panelPrefab)
    {
        if (uiMap.ContainsKey(interactable) == false)
        {
            uiMap.Add(interactable, BuildUIForTarget(panelPrefab));
        }
    }

    public void UnRegisterInteractable(Interactable interactable)
    {
        Destroy(uiMap[interactable].gameObject);
        uiMap.Remove(interactable);
    }
}
