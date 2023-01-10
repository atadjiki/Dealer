using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ActionCanvas : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Text_Action;
    [SerializeField] private Button Button_Advance;

    private Action _advanceCallback;

    private void Awake()
    {
        Button_Advance.onClick.AddListener(delegate ()
        {
            OnAdvancePressed();
        });

        Button_Advance.Select();
    }

    public void Setup(string Action, Action OnAdvance)
    {
        Text_Action.text = Action;

        _advanceCallback = OnAdvance;
    }

    private void OnDestroy()
    {
        _advanceCallback = null;
    }

    private void OnAdvancePressed()
    {
        _advanceCallback.Invoke();
        Destroy(this.gameObject);
    }
}
