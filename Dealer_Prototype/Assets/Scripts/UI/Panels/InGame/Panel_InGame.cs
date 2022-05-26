using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel_InGame : UIPanel
{
    [SerializeField] private CharacterPanel panel_character;
    [SerializeField] private PartyPanelList panel_party;
    [SerializeField] private InteractablesPanel panel_interactables;
    [SerializeField] private Panel_InGame_Money panel_money;

    private List<UIPanel> panels;
    private int buildCount = 0;

    public override void Build()
    {
        panels = new List<UIPanel> { panel_character, panel_party, panel_interactables, panel_money };

        foreach(UIPanel panel in panels)
        {
            panel.onBuildComplete += OnChildPanelBuilt;
            panel.Build();
        }
    }

    public void OnChildPanelBuilt(UIPanel panel)
    {
        buildCount += 1;

        if (buildCount == panels.Count)
        {
            Debug.Log(this.name + " | Panels Built = " + buildCount + "/" + panels.Count);

            onBuildComplete(this);
        }
    }

    public override void UpdatePanel()
    {
        foreach(UIPanel panel in panels)
        {
            panel.UpdatePanel();
        }

        base.UpdatePanel();
    }

    public override void ShowPanel()
    {
        foreach(UIPanel panel in panels)
        {
            panel.ShowPanel();
        }

        base.ShowPanel();
    }

    public override void HidePanel()
    {
        foreach (UIPanel panel in panels)
        {
            panel.HidePanel();
        }    

        base.HidePanel();
    }

    public CharacterPanel GetCharacterPanel()
    {
        return panel_character;
    }

    public PartyPanelList GetPartyPanel()
    {
        return panel_party;
    }

    public InteractablesPanel GetInteractablesPanel()
    {
        return panel_interactables;
    }

    public Panel_InGame_Money GetMoneyPanel()
    {
        return panel_money;
    }
}
