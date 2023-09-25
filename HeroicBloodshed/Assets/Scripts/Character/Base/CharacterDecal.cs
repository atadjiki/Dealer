using System.Collections;
using System.Collections.Generic;
using static Constants;
using UnityEngine;
using EPOOutline;

public class CharacterDecal : MonoBehaviour
{
    [SerializeField] private Outlinable Outline;

    public void SetColor(TeamID team)
    {
        Color teamColor = GetColorByTeam(team, 1.0f);

        Outline.OutlineParameters.Color = teamColor;

        teamColor.a = 0.15f;

        Outline.OutlineParameters.FillPass.SetColor("_PublicColor", teamColor);
    }
}
