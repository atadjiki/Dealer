using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class CharacterDecal : MonoBehaviour
{
    public void SetColorByTeam(CharacterConstants.Team team)
    {
        switch(team)
        {
            case CharacterConstants.Team.DEA:
                SetColor(Color.blue);
                break;
            case CharacterConstants.Team.Mafia:
                SetColor(Color.red);
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
