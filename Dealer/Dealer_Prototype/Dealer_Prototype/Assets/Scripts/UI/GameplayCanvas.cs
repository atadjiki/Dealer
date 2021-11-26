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

    public void SetInteractionTipText(NPCComponent npc)
    {
        Text_Interaction_Tip.text = npc.CharacterID.ToString();
    }

    public void ClearInteractionTipText()
    {
        Text_Interaction_Tip.text = "";
    }
}
