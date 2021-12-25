using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayableCharacterComponent : NPCComponent
{
    internal override void Initialize(SpawnData spawnData)
    {
        base.Initialize(spawnData);

        if (NPCManager.Instance.Register(this) == false)
        {
            Destroy(this.gameObject);
        }
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
    }

    public override void PerformUnselect()
    {
        CharacterMode = CharacterConstants.Mode.Stationary;
        _selection.SetUnposessed();
    }

    public override void OnMouseClicked()
    {
        PlayableCharacterManager.Instance.HandleCharacterSelection(this);
    }
}
