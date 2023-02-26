using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class CharacterGroundDecal : MonoBehaviour
{
    public virtual void Show(Enumerations.Team team)
    {
        switch(team)
        {
            case Enumerations.Team.Player:
                MaterialHelper.SetPlayerGroundDecal(this);
                break;
            case Enumerations.Team.Neutral:
                MaterialHelper.SetNeutralGroundDecal(this);
                break;
            case Enumerations.Team.Enemy:
                MaterialHelper.SetEnemyGroundDecal(this);
                break;
        }
    }

    public virtual void Hide()
    {
        MaterialHelper.HideGroundDecal(this);
    }
}
