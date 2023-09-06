using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class EncounterCharacterQueueItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    protected CharacterComponent _characterComponent;

    [Header("Highlights")]
    [SerializeField] protected GameObject Panel_Active;
    [SerializeField] protected GameObject Panel_Inactive;
    [SerializeField] protected GameObject Panel_Dead;

    protected virtual void OnEnable()
    {
        Panel_Active.SetActive(false);
        Panel_Inactive.SetActive(false);
        Panel_Dead.SetActive(false);
    }

    public virtual void Setup(CharacterComponent character)
    {
        _characterComponent = character;
    }

    public virtual void SetActive()
    {
        Panel_Dead.SetActive(false);
        Panel_Active.SetActive(true);
        Panel_Inactive.SetActive(false);
    }

    public virtual void SetInactive()
    {

        Panel_Dead.SetActive(false);
        Panel_Active.SetActive(false);
        Panel_Inactive.SetActive(true);
    }

    public virtual void SetDead()
    {
        Panel_Dead.SetActive(true);
        Panel_Active.SetActive(false);
        Panel_Inactive.SetActive(false);
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
