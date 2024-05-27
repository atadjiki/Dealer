using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

[RequireComponent(typeof(EnvironmentWallRaycaster))]
public class CharacterModel : MonoBehaviour, ICharacterEventReceiver
{
    [SerializeField] private GameObject MeshGroup_Main;

    private List<CharacterBodyPartAnchor> _bodyParts;
    private EnvironmentWallRaycaster _wallRaycaster;

    private void Awake()
    {
        _bodyParts = new List<CharacterBodyPartAnchor>(GetComponentsInChildren<CharacterBodyPartAnchor>());
        _wallRaycaster = GetComponent<EnvironmentWallRaycaster>();
    }

    public void ToggleModel(bool flag)
    {
        MeshGroup_Main.SetActive(flag);
    }

    public void HandleEvent(CharacterEvent characterEvent, object eventData)
    {
        switch (characterEvent)
        {
            case CharacterEvent.DEATH:
                {
                    _wallRaycaster.enabled = false;
                    break;
                }
            default:
                break;
        }
    }

    public Transform GetRandomBodyPart()
    {
        int index = Random.Range(0, _bodyParts.Count);

        return _bodyParts[index].gameObject.transform;
    }

    public Transform GetBodyPart(BodyPartID ID)
    {
        foreach(CharacterBodyPartAnchor bodypart in _bodyParts)
        {
            if(bodypart.GetID() == ID)
            {
                return bodypart.gameObject.transform;
            }
        }

        return this.transform;
    }

    public bool CanReceiveCharacterEvents()
    {
        return true;
    }
}
