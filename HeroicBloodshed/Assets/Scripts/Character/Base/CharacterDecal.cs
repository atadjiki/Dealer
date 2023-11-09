using System.Collections;
using System.Collections.Generic;
using static Constants;
using UnityEngine;

public class CharacterDecal : MonoBehaviour
{
    private List<SpriteRenderer> _renderers;

    private void Awake()
    {
        _renderers = new List<SpriteRenderer>(GetComponentsInChildren<SpriteRenderer>());
    }

    public void SetColor(TeamID team)
    {
        Color teamColor = GetColorByTeam(team, 1.0f);
        teamColor.a = 0.2f;

        foreach(SpriteRenderer renderer in _renderers)
        {
            renderer.color = teamColor;
        }
    }
}
