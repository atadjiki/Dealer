using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractableUIPanel : MonoBehaviour
{

    [SerializeField] GameObject Panel_Main;
    [SerializeField] Button Button_Interactable;
    [SerializeField] TextMeshProUGUI Button_TextMesh;
    [SerializeField] TextMeshProUGUI Description_TextMesh;

    private Interactable interactable;

    private Vector2 defaultSize = new Vector2(300.0f, 250.0f);
    public void Build(Interactable toBuild)
    {
        interactable = toBuild;

        ((RectTransform)this.transform).sizeDelta = defaultSize;

        Button_TextMesh.text = toBuild.GetID();
        Description_TextMesh.text = "click to interact with " + toBuild.GetID();
    }

    public void OnButtonClick()
    {
        InfoPanelManager.Instance.UnRegisterInteractable(interactable);
    }
}
