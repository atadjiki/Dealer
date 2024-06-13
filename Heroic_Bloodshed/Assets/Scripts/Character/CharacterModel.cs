using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HighlightPlus;
using static Constants;

[RequireComponent(typeof(HighlightEffect))]
public class CharacterModel : MonoBehaviour, ICharacterEventHandler
{
    [SerializeField] private List<GameObject> Meshes;

    private HighlightEffect _highlight;

    public void CreateHighlight(TeamID teamID)
    {
        _highlight = GetComponent<HighlightEffect>();

        ToggleHighlighted(false);
        ToggleOverlay(false);
        ToggleTargetUI(false);
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

    public void HandleEvent(CharacterEvent characterEvent, object eventData)
    {
        switch(characterEvent)
        {
            case CharacterEvent.SELECTED:
                ToggleHighlighted(true);
                break;
            case CharacterEvent.DESELECTED:
                ToggleHighlighted(true);
                break;
        }
    }
}
