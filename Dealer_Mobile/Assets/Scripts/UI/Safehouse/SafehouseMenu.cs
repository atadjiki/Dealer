using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SafehouseMenu : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject Prefab_SummaryMenu;
    [SerializeField] private GameObject Prefab_StashMenu;
    [SerializeField] private GameObject Prefab_InboxMenu;
    [SerializeField] private GameObject Prefab_MapMenu;

    [Header("Elements")]
    [SerializeField] private Transform Transform_ContextPanel;

    [Header("Buttons")]
    [SerializeField] private Button Button_Summary;
    [SerializeField] private Button Button_Stash;
    [SerializeField] private Button Button_Inbox;
    [SerializeField] private Button Button_Map;

    [SerializeField] private Button Button_Settings;
    [SerializeField] private Button Button_Continue;

    private enum SummaryListSection { Summary, Stash, Inbox, Map, None };
    private SummaryListSection currentSection = SummaryListSection.None;

    private void Awake()
    {
        Button_Summary.onClick.AddListener(     delegate { OnListButtonClicked(SummaryListSection.Summary);     });
        Button_Stash.onClick.AddListener(       delegate { OnListButtonClicked(SummaryListSection.Stash);       });
        Button_Inbox.onClick.AddListener(       delegate { OnListButtonClicked(SummaryListSection.Inbox);       });
        Button_Map.onClick.AddListener(         delegate { OnListButtonClicked(SummaryListSection.Map);         });

        Button_Settings.onClick.AddListener(OnSettingsButtonClicked);
        Button_Continue.onClick.AddListener(OnContinueButtonClicked);
    }

    private void OnListButtonClicked(SummaryListSection section)
    {
        if(section == currentSection) { return; }

        if(currentSection != SummaryListSection.None)
        {
            ClearContextPanel();
        }

        switch(section)
        {
            case SummaryListSection.Summary:
                Instantiate(Prefab_SummaryMenu, Transform_ContextPanel);
                break;
            case SummaryListSection.Stash:
                Instantiate(Prefab_StashMenu, Transform_ContextPanel);
                break;
            case SummaryListSection.Inbox:
                Instantiate(Prefab_InboxMenu, Transform_ContextPanel);
                break;
            case SummaryListSection.Map:
                Instantiate(Prefab_MapMenu, Transform_ContextPanel);
                break;
            default:
                break;
        }

        currentSection = section;
    }

    private void ClearContextPanel()
    {
        var children = new List<GameObject>();
        foreach (Transform child in Transform_ContextPanel) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));
    }

    private void OnContinueButtonClicked()
    {
    }

    private void OnSettingsButtonClicked()
    { 
    }
}
