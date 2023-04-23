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

    [Header("Buttons")]
    [SerializeField] private Button Button_Settings;

    private void Awake()
    {
        if(OptionsList.Count > 0)
        {
            for (int i = 0; i < OptionsList.Count; i++)
            {
                int index = i;
                OptionsList[i].button.onClick.AddListener(delegate { SelectListOption(index); });
            }
        }

        Button_Settings.onClick.AddListener(OnSettingsButtonClicked);
    }

    private void SelectListOption(int index)
    {
        GameObject prefab = Instantiate(OptionsList[index].prefab);
        OverlayMenu overlayMenu = prefab.GetComponent<OverlayMenu>();

        if(overlayMenu != null)
        {
            Button cancelButton = overlayMenu.GetCancelButton();

            if(cancelButton != null)
            {
                cancelButton.onClick.AddListener(delegate { OnCancelButtonCiicked(); });

                ToggleMenu(false);
                
            }
        }

    }

    private void OnSettingsButtonClicked()
    {
        
    }

    private void OnCancelButtonCiicked()
    {
        ToggleMenu(true);
    }

    private void ToggleMenu(bool flag)
    {
        Button_Settings.gameObject.SetActive(flag);

        foreach(SafehouseOptionData option in OptionsList)
        {
            option.button.gameObject.SetActive(flag);
        }
    }
}
