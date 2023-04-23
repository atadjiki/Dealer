using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class OverlayMenu : MonoBehaviour
{
    [SerializeField] protected Button Button_Cancel;

    public Button GetCancelButton()
    {
        return Button_Cancel;
    }
}
