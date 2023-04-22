using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

[Serializable]
public struct CityMapOptionData
{
    public Button button;
    public GameObject prefab;
}

public class CityMapMenu : MonoBehaviour
{
    [Header("Options")]
    [SerializeField] private List<CityMapOptionData> OptionsList;

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
        Instantiate(OptionsList[index].prefab);
    }

    private void OnSettingsButtonClicked()
    { 
    }
}
