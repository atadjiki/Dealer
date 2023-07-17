using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class CharacterDecal : MonoBehaviour
{

    private static Color Color_DEA = new Color(0, 0.501668f, 0.9716981f);
    private static Color Color_Mafia = new Color(1, 0.08050716f, 0);

    public void SetColorByTeam(CharacterConstants.Team team)
    {
        switch(team)
        {
            case CharacterConstants.Team.DEA:
                SetColor(Color_DEA);
                break;
            case CharacterConstants.Team.Mafia:
                SetColor(Color_Mafia);
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
