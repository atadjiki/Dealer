using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using static Constants;

[Serializable]
public struct SafehouseOptionData
{
    public Button button;
    public string sceneName;
}

public class SafehouseMenu : MonoBehaviour
{
    [Header("Options")]
    [SerializeField] private List<SafehouseOptionData> OptionsList;

    [Header("Buttons")]
    [SerializeField] private Button Button_Settings;

    private void Awake()
    {
        Global.OnSafehouseMenuComplete += OnSafehouseMenuComplete;

        if(OptionsList.Count > 0)
        {
            for (int i = 0; i < OptionsList.Count; i++)
            {
                int index = i;
                OptionsList[i].button.onClick.AddListener(delegate { SelectListOption(index); });
            }
        }
    }

    private void SelectListOption(int index)
    {
        StartCoroutine(Coroutine_LoadMenu(index));
    }

    private IEnumerator Coroutine_LoadMenu(int optionIndex)
    {
        string sceneName = OptionsList[optionIndex].sceneName;

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        yield return new WaitUntil(() => operation.isDone);

        ToggleMenu(false);

    }

    private void OnSafehouseMenuComplete(SafehouseMenuID MenuID)
    {
        ToggleMenu(true);
    }

    private void ToggleMenu(bool flag)
    {
        this.gameObject.SetActive(flag);
    }
}
