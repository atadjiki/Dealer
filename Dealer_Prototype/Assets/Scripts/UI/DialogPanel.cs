using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using GameDelegates;
using UnityEngine;

public class DialogPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI DialogTitle;
    [SerializeField] private TextMeshProUGUI DialogBlurb;
    [SerializeField] private Button Button_Cancel;
    [SerializeField] private Button Button_OK;

    private void Awake()
    {
        Button_OK.onClick.AddListener(delegate ()
        {
            if(OnOKPressed != null) OnOKPressed.Invoke();
         //   Destroy(this);
        }

        );

        Button_Cancel.onClick.AddListener(delegate ()
        {
            if (OnCancelPressed != null) OnCancelPressed.Invoke();
          //  Destroy(this);
        }
        );
    }


    public CancelButtonPressed OnCancelPressed;
    public OKButtonPressed OnOKPressed;

    public void Setup(string Title, string Blurb)
    {
        DialogTitle.text = Title;
        DialogBlurb.text = Blurb;
    }

}
