using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIPanel_RosterPicker_RosterSlot : UIPanel
{
    [SerializeField] private GameObject Group_Active;
    [SerializeField] private GameObject Group_Inactive;

    public void FillSlot()
    {
        Group_Inactive.SetActive(false);
        Group_Active.SetActive(true);
    }

    public void ClearSlot()
    {
        Group_Active.SetActive(false);
        Group_Inactive.SetActive(true);
    }
}
