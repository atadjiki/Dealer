using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class MaterialManager : Singleton<MaterialManager>
{
    [Header("Debug")]
    [SerializeField] private Material Mat_Debug;

    [Header("Team")]
    [SerializeField] private Material Mat_Team_Player;
    [SerializeField] private Material Mat_Team_Enemy;
    [SerializeField] private Material Mat_Team_Neutral;

    public Material GetMaterialByTeam(Enumerations.Team team)
    {
        switch(team)
        {
            case Enumerations.Team.Player:
                return Mat_Team_Player;
            case Enumerations.Team.Enemy:
                return Mat_Team_Enemy;
            case Enumerations.Team.Neutral:
                return Mat_Team_Neutral;
        }

        return Mat_Debug;
    }
}
