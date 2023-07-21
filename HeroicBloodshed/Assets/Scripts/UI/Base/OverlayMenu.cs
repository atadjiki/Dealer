using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class OverlayMenu : MonoBehaviour
{
    [SerializeField] protected Button Button_Cancel;

    private void Start()
    {
        Button_Cancel.onClick.AddListener(delegate
        {
            OnCancelButtonClicked();

        });
    }

    public Button GetCancelButton()
    {
        return Button_Cancel;
    }

    protected virtual void OnCancelButtonClicked()
    {
        Destroy(this.gameObject);
    }
}
