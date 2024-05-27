using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class EncounterCharacterHighlight : EncounterEventHandler
{
    private GameObject _quad;

    protected override void OnAwake()
    {
        base.OnAwake();

        MaterialLibrary matLib = MaterialLibrary.Get();

        _quad = EnvironmentUtil.CreateTileQuad(matLib.CharacterSelect, this.transform, Vector3.one);
    }

    private void Update()
    {
        _quad.transform.eulerAngles = new Vector3(90, 0, 0);
    }

    protected override void OnStateChangedCallback(EncounterStateData state)
    {
        if (state.GetCurrentState() == EncounterState.DESELECT_CURRENT_CHARACTER)
        {
            Destroy(this.gameObject);
        }
    }
}
