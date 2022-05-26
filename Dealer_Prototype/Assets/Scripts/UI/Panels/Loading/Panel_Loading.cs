using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel_Loading : UIPanel
{
    public GameObject SubPanel;

    public override void ShowPanel()
    {
        SubPanel.gameObject.SetActive(true);

        base.ShowPanel();
    }

    public override void HidePanel()
    {
        SubPanel.gameObject.SetActive(false);
        base.HidePanel();
    }
}
