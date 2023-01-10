using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Constants;
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

        Button_Advance.Select();
    }

    public void Setup(Enumerations.CharacterID Speaking, string Dialogue, Action OnAdvance)
    {
        if(Speaking == Enumerations.CharacterID.None)
        {
            Text_Speaking.gameObject.SetActive(false);
        }
        else
        {
            Text_Speaking.text = Speaking.ToString();
        }

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
