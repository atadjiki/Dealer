using System.Collections;
using System.Collections.Generic;
using static Constants;
using UnityEngine;

public class CharacterDecal : MonoBehaviour
{

    private static Color Color_Player = new Color(0, 0.501668f, 0.9716981f);
    private static Color Color_Enemy = new Color(1, 0.08050716f, 0);

    public void SetColorByTeam(TeamID team)
    {
        switch(team)
        {
            case TeamID.Player:
                SetColor(Color_Player);
                break;
            case TeamID.Enemy:
                SetColor(Color_Enemy);
                break;
            default:
                SetColor(Color.grey);
                break;
        }
    }

    private void SetColor(Color color)
    {
        Renderer renderer = this.GetComponent<Renderer>();

        if(renderer != null)
        {
            if(renderer.material != null)
            {
                renderer.material.color = color;
            }
        }
    }
}
