using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class ColorConstants 
{
    public static string Team_Ally = "#6B8E23";
    public static string Team_Enemy = "#800000";
    public static string Team_NPC = "#708090";
    public static string Team_Player = "#F8F0E3";

    public static string NavPoint = "#00FF14";
    public static string Selection = "#808080";

    public static string GetColorByTeam(CharacterConstants.Team Team)
    {
        switch(Team)
        {
            case CharacterConstants.Team.Player:
                return Team_Player;
            case CharacterConstants.Team.Ally:
                return Team_Ally;
            case CharacterConstants.Team.NPC:
                return Team_NPC;
            case CharacterConstants.Team.Enemy:
                return Team_Enemy;
        }

        return "ERROR";
    }

    public static void SetObjectToColor(GameObject obj, string hexcode)
    {
        //get all mesh renderers
        HashSet<Renderer> renderers = new HashSet<Renderer>();

        foreach(Renderer child_renderer in obj.GetComponentsInChildren<Renderer>())
        {
            renderers.Add(child_renderer);
        }

        foreach(Renderer renderer in renderers)
        {
            Color color;

            if (ColorUtility.TryParseHtmlString(hexcode, out color))
            {
                renderer.material.color = color;
                renderer.material.SetColor("Color_42c7f5bfb6334aa5bbf8cc1a11a49afe", color);
            }
        }
    }
}
