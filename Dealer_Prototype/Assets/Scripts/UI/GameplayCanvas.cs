using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class GameplayCanvas : MonoBehaviour
{
    private static GameplayCanvas _instance;

    public static GameplayCanvas Instance { get { return _instance; } }

    [SerializeField] internal GameObject GameplayPanel;
    [SerializeField] internal TextMeshProUGUI Text_Interaction_Tip;

    public enum InteractionContext { Select, Deselect, Move, Approach, Interact, None };

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
        Toggle(true);
    }

    public void Toggle(bool flag)
    {
        this.gameObject.SetActive(flag);
    }

    public void SetInteractionTipTextContext(InteractionContext context)
    {
        string text = "";

        switch(context)
        {
            case InteractionContext.Select:
                text = "Select";
                break;
            case InteractionContext.Deselect:
                text = "Deselect";
                break;
            case InteractionContext.Move:
                text = "Move";
                break;
            case InteractionContext.Interact:
                text = "Interact";
                break;
            case InteractionContext.Approach:
                text = "Approach";
                break;
        }

        Text_Interaction_Tip.text = text;
    }

    public void ClearInteractionTipText()
    {
        Text_Interaction_Tip.text = "";
    }
}
