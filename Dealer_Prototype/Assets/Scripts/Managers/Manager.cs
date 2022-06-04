using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Manager : MonoBehaviour
{
    public delegate void OnBuildComplete(Manager manager);
    public OnBuildComplete onBuildComplete;
    internal bool activated = false;

    public virtual void Build()
    {
        onBuildComplete(this);
    }

    public virtual int AssignDelegates() { return 0; }

    public virtual void Activate()
    {
        activated = true;
    }

    public virtual void DeActivate()
    {
        activated = false;
    }

    public virtual bool PerformUpdate(float tick)
    {
        return activated;
    }
}
