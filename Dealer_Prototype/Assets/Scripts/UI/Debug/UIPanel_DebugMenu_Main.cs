using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIPanel_DebugMenu_Main : MonoBehaviour
{
    public void Button_OnExitPressed()
    {
        LevelManager.Instance.CloseDebugMenu();
    }
}
