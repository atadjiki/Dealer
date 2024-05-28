using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HighlightPlus;
using static Constants;

public class CharacterModel : MonoBehaviour
{
    [SerializeField] private List<GameObject> Meshes;

    private HighlightEffect _highlight;

    public void CreateHighlight(TeamID teamID)
    {
        RemoveHighlight();

        HighlightProfileLibrary lib = HighlightProfileLibrary.Get();

        HighlightProfile profile = lib.GetTeamProfile(teamID);

        _highlight = this.gameObject.AddComponent<HighlightEffect>();

        _highlight.profile = profile;
        _highlight.ProfileLoad(profile);

        ToggleHighlighted(false);
        ToggleOverlay(false);
        ToggleTargetUI(false);
    }

    public void RemoveHighlight()
    {
        if (_highlight != null)
        {
            Destroy(_highlight);
        }
    }

    public void ToggleTargetUI(bool flag)
    {
        if (_highlight != null)
        {
            _highlight.targetFX = flag;
        }
    }

    public void ToggleOverlay(bool flag)
    {
        if(flag)
        {
            _highlight.overlay = 1;
        }
        else
        {
            _highlight.overlay = 0;
        }
    }

    public void ToggleHighlighted(bool flag)
    {
        if(_highlight != null)
        {
            _highlight.highlighted = flag;
        }
    }

    public void ToggleVisibility(bool flag)
    {
        foreach(GameObject mesh in Meshes)
        {
            mesh.SetActive(flag);
        }
    }
}
