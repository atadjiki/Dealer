using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;

public class DialogueCanvas : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Text_Speaking;
    [SerializeField] private TextMeshProUGUI Text_Dialogue;
    [SerializeField] private Button Button_Advance;

    private Action _advanceCallback;

    private void Awake()
    {
        Button_Advance.onClick.AddListener(delegate ()
        {
            OnAdvancePressed();
        });
    }

    public void Setup(string Speaking, string Dialogue, Action OnAdvance)
    {
        Text_Speaking.text = Speaking;
        Text_Dialogue.text = Dialogue;

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
