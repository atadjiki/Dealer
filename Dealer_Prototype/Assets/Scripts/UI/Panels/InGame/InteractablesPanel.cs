using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractablesPanel : UIPanel
{ 
    [SerializeField] private GameObject interactablePanelPrefab;
    [SerializeField] private Camera uiCamera;
    [SerializeField] private Canvas uiCanvas;

    private Dictionary<Interactable, InteractableUIPanel> uiMap; //map interactable to panel

    public override void Build()
    {
        uiMap = new Dictionary<Interactable, InteractableUIPanel>();

        base.Build();
    }

    public override void UpdatePanel()
    {
        foreach (Interactable interactable in uiMap.Keys)
        {
            InteractableUIPanel interactableUIPanel = uiMap[interactable];

            if(interactableUIPanel != null)
            {

                Vector3 anchorPoint = interactable.GetComponent<Collider>().bounds.max;

                Vector2 targetScreenPoint = WorldToCanvas(uiCanvas, anchorPoint, uiCamera);

                Vector2 offset = new Vector2(0, -100);

                ((RectTransform)interactableUIPanel.gameObject.transform).anchoredPosition = targetScreenPoint + offset;
            }
        }

        base.UpdatePanel();
    }

    private InteractableUIPanel BuildUIForTarget(Interactable interactable)
    {
        if(interactable.uiPanelPrefab)
        {
            GameObject interactablePanelObject = Instantiate(interactable.uiPanelPrefab, interactable.transform);
            InteractableUIPanel interactableUIPanel = interactablePanelObject.GetComponent<InteractableUIPanel>();

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


    public void RegisterInteractable(Interactable interactable)
    {
        if (uiMap.ContainsKey(interactable) == false)
        {
            uiMap.Add(interactable, BuildUIForTarget(interactable));
        }
    }

    public void UnRegisterInteractable(Interactable interactable)
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
