using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanel : MonoBehaviour
{
    public delegate void OnBuildComplete(UIPanel panel);
    public OnBuildComplete onBuildComplete;

    public virtual void Build() 
    {
        onBuildComplete(this);
    }
    public virtual void UpdatePanel() { }
    public virtual void ShowPanel() { }
    public virtual void HidePanel() { }
}
