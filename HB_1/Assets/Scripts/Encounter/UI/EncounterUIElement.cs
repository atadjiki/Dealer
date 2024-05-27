using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class EncounterUIElement : MonoBehaviour
{
    [SerializeField] protected GameObject Panel_Main;

    public virtual void Populate(EncounterModel model) { }

    public virtual void HandleStateUpdate(EncounterState stateID, EncounterModel model) { }

    public virtual void Show()
    {
        Panel_Main.SetActive(true);
    }

    public virtual void Hide()
    {
        Panel_Main.SetActive(false);
    }
}
