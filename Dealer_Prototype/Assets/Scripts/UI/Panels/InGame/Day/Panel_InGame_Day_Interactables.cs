using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Panel_InGame_Day_Interactables : UIPanel
{ 
    [SerializeField] private GameObject interactablePanelPrefab;
    [SerializeField] private Camera uiCamera;
    [SerializeField] private Canvas uiCanvas;

    private Dictionary<Interactable, Panel_InGame_Day_Interactable> uiMap; //map interactable to panel

    public override void Build()
    {
        uiMap = new Dictionary<Interactable, Panel_InGame_Day_Interactable>();

        base.Build();
    }

    public override void UpdatePanel()
    {
        if(allowUpdate)
        {
            foreach (Interactable interactable in uiMap.Keys)
            {
                Panel_InGame_Day_Interactable interactableUIPanel = uiMap[interactable];

                if (interactableUIPanel != null)
                {

                    Vector3 anchorPoint = interactable.GetComponent<Collider>().bounds.max;

                    Vector2 targetScreenPoint = WorldToCanvas(uiCanvas, anchorPoint, uiCamera);

                    Vector2 offset = new Vector2(0, -100);

                    ((RectTransform)interactableUIPanel.gameObject.transform).anchoredPosition = targetScreenPoint + offset;
                }
            }
        }
    }

    private Panel_InGame_Day_Interactable BuildUIForTarget(Interactable interactable)
    {
        if(interactable.uiPanelPrefab)
        {
            GameObject interactablePanelObject = Instantiate(interactable.uiPanelPrefab, interactable.transform);
            Panel_InGame_Day_Interactable interactableUIPanel = interactablePanelObject.GetComponent<Panel_InGame_Day_Interactable>();

            return interactableUIPanel;
        }

        return null;
    }

    public static Vector2 WorldToCanvas(Canvas canvas, Vector3 worldPoint, Camera camera)
    {
        Vector2 viewportPos = camera.WorldToViewportPoint(worldPoint);
        RectTransform canvasRect = (RectTransform)canvas.transform;

        return new Vector2((viewportPos.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f),
            (viewportPos.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f));
    }


    public override void RegisterInteractable(Interactable interactable)
    {
        if (uiMap.ContainsKey(interactable) == false)
        {
            uiMap.Add(interactable, BuildUIForTarget(interactable));
        }
    }

    public override void UnRegisterInteractable(Interactable interactable)
    {
        if(uiMap.ContainsKey(interactable))
        {
            if(uiMap[interactable] != null)
            {
                Destroy(uiMap[interactable].gameObject);
            }

            uiMap.Remove(interactable);
        }
    }
}
