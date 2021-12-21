using System;
using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    [Header("Teams")]
    [SerializeField] private Color Team_Ally;
    [SerializeField] private Color Team_Enemy;
    [SerializeField] private Color Team_NPC;
    [SerializeField] private Color Team_Player;

    [Header("Decals")]
    [SerializeField] private Color NavPoint;
    [SerializeField] private Color Selection;
    [SerializeField] private Color Behavior_Inactive;
    [SerializeField] private Color Behavior_Active;
    [SerializeField] private Color InteractionTransform;


    private static ColorManager _instance;

    public static ColorManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public Color GetPlayerColor()
    {
        return Team_Player;
    }

    public Color GetColorByTeam(CharacterConstants.Team Team)
    {
        switch (Team)
        {
            case CharacterConstants.Team.Ally:
                return Team_Ally;
            case CharacterConstants.Team.NPC:
                return Team_NPC;
            case CharacterConstants.Team.Enemy:
                return Team_Enemy;
        }

        return Color.clear;
    }

    public void SetObjectToColor(GameObject _object, Color color)
    {
        if (_object is null)
        {
            throw new ArgumentNullException(nameof(_object));
        }

        //get all mesh renderers
        HashSet<Renderer> renderers = new HashSet<Renderer>();

        foreach (Renderer child_renderer in _object.GetComponentsInChildren<Renderer>())
        {
            renderers.Add(child_renderer);
        }

        foreach (Renderer renderer in renderers)
        {
            renderer.material.color = color;
            renderer.material.SetColor("Color_42c7f5bfb6334aa5bbf8cc1a11a49afe", color);
        }
    }

    public Color GetNavPointColor()
    {
        return NavPoint;
    }

    public Color GetSelectionColor()
    {
        return Selection;
    }

    public Color GetBehaviorDecalColor(bool active)
    {
        if(active)
        {
            return Behavior_Active;
        }
        else
        {
            return Behavior_Inactive;
        }
    }

    public Color GetInteractionTransformColor()
    {
        return InteractionTransform;
    }
}
