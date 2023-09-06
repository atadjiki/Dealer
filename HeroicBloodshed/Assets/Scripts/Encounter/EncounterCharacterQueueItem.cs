using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class EncounterCharacterQueueItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    protected CharacterComponent _characterComponent;

    public virtual void Setup(CharacterComponent character)
    {
        _characterComponent = character;
    }

    public virtual void SetActive()
    {
    }

    public virtual void SetInactive()
    {
    }

    public virtual void SetDead()
    {
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        EncounterManager.Instance.FollowCharacter(_characterComponent);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        EncounterManager.Instance.UnfollowCharacter();
    }
}
