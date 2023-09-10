using System.Collections;
using System.Collections.Generic;
using static Constants;
using UnityEngine;

public class CharacterDecal : MonoBehaviour
{
    public void SetColor(TeamID team)
    {
        Color teamColor = GetColorByTeam(team, 0.25f);

        Renderer renderer = this.GetComponent<Renderer>();

        if(renderer != null)
        {
            if(renderer.material != null)
            {
                renderer.material.color = teamColor;
            }
        }
    }
}
