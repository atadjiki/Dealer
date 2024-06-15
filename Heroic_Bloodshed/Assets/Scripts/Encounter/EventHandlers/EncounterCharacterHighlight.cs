using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class EncounterCharacterHighlight : EncounterEventHandler
{
    [SerializeField] private ColorLibrary ColorLib;
    [SerializeField] private GameObject Quad;

    public void Setup(CharacterComponent character)
    {
        TeamID team = character.GetTeam();

        Color teamColor = ColorLib.Get(team);
        teamColor.a = 0.25f;

        Debug.Log("TEAM COLOR " + teamColor.ToString());

        MeshRenderer renderer = Quad.GetComponent<MeshRenderer>();

        renderer.material.color = teamColor;
    }

    private void Update()
    {
        Quad.transform.eulerAngles = new Vector3(90, 0, 0);
    }

    protected override void OnStateChangedCallback(EncounterStateData state)
    {
        if (state.GetCurrentState() == EncounterState.PERFORM_ACTION)
        {
            Destroy(this.gameObject);
        }
    }
}
