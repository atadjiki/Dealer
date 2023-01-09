using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using GameDelegates;
using UnityEngine;
using System;

public class OkCancelDialog : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI DialogTitle;
    [SerializeField] private TextMeshProUGUI DialogBlurb;
    [SerializeField] private Button Button_Cancel;
    [SerializeField] private Button Button_OK;

    private Action _cancelCallback;
    private Action _okCallback;

    private void Awake()
    {
        Button_OK.onClick.AddListener(delegate ()
        {
            OnOkPressed();
        }

        );

        Button_Cancel.onClick.AddListener(delegate ()
        {
            OnCancelPressed();
        }
        );
    }

    public void Setup(string Title, string Blurb, Action _ok, Action _cancel)
    {
        DialogTitle.text = Title;
        DialogBlurb.text = Blurb;

        _okCallback = _ok;
        _cancelCallback = _cancel;
    }

    private void OnOkPressed()
    {
        _okCallback.Invoke();
        Destroy(this.gameObject);
    }

    private void OnCancelPressed()
    {
        _cancelCallback.Invoke();
        Destroy(this.gameObject);
    }

}
