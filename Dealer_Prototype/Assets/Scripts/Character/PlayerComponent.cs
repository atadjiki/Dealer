using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerComponent : NPCComponent
{
    private static PlayerComponent _instance;

    public static PlayerComponent Instance { get { return _instance; } }

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

    internal override void Initialize(SpawnData spawnData)
    {
        base.Initialize(spawnData);
    }

    internal override IEnumerator DoInitialize()
    {
        yield return base.DoInitialize();

        ColorManager.Instance.SetObjectToColor(_animator.gameObject, ColorManager.Instance.GetPlayerColor());
    }

    public override void PerformSelect()
    {
        CharacterMode = CharacterConstants.Mode.Selected;
        _selection.SetPossesed();
        ToIdle();
    }

    public override void PerformUnselect()
    {
        CharacterMode = CharacterConstants.Mode.Stationary;
        _selection.SetUnposessed();
        ToIdle();
    }

    public override void OnMouseClicked()
    {
        NPCManager.Instance.HandleNPCSelection(this);
    }

    public override void OnMouseEnter()
    {
        base.OnMouseEnter();
    }

    public override void OnMouseExit()
    {
        base.OnMouseExit();
    }
}
