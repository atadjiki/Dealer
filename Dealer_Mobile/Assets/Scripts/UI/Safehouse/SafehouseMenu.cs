using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

[Serializable]
public struct SafehouseOptionData
{
    public Button button;
    public GameObject prefab;
}

public class SafehouseMenu : MonoBehaviour
{
    [Header("Options")]
    [SerializeField] private List<SafehouseOptionData> OptionsList;

    [Header("Elements")]
    [SerializeField] private Transform Transform_ContextPanel;

    [Header("Buttons")]
    [SerializeField] private Button Button_Settings;

    private int currentOption;

    private void Awake()
    {
        if(OptionsList.Count > 0)
        {
            for (int i = 0; i < OptionsList.Count; i++)
            {
                int index = i;
                OptionsList[i].button.onClick.AddListener(delegate { SelectListOption(index); });
            }

            //SelectListOption(0);
            //OptionsList[0].button.Select();
        }

        Button_Settings.onClick.AddListener(OnSettingsButtonClicked);
    }

    private void SelectListOption(int index)
    {
        if(currentOption == index && Transform_ContextPanel.childCount > 0) { return; }

        currentOption = index;

        ClearContextPanel();

        Instantiate(OptionsList[currentOption].prefab, Transform_ContextPanel);
    }

    private void ClearContextPanel()
    {
        var children = new List<GameObject>();
        foreach (Transform child in Transform_ContextPanel) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));
    }

    private void OnSettingsButtonClicked()
    { 
    }
}
