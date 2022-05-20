using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InteractableUIPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Button Button_Interactable;
    [SerializeField] TextMeshProUGUI Title_TextMesh;
    [SerializeField] TextMeshProUGUI Description_TextMesh;

    private Interactable interactable;
    public bool expanded { get; set; }

    private Vector2 defaultSize = new Vector2(300.0f, 250.0f);

    private void Awake()
    {
        ((RectTransform)this.transform).sizeDelta = defaultSize;
        ToggleButtonPanel(false);

        interactable = GetComponentInParent<Interactable>();
    }

    public void OnButtonClick()
    {
        Debug.Log("on mouse clicked " + this.name);
        InfoPanelManager.Instance.UnRegisterInteractable(interactable);
    }

    private void ToggleButtonPanel(bool visible)
    {
        Button_Interactable.gameObject.SetActive(visible);
        expanded = visible;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("on mouse enter GUI " + this.name);
        ToggleButtonPanel(true);
        CursorManager.Instance.ToInteract();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("on mouse exit GUI " + this.name);
        ToggleButtonPanel(false);
        CursorManager.Instance.ToDefault();
    }
}
