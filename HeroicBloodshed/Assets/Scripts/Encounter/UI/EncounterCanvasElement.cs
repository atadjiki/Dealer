using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterCanvasElement : MonoBehaviour
{
    public virtual void Populate(EncounterModel model)
    {
        Clear();
    }

    public virtual void Clear()
    {
    }
}
