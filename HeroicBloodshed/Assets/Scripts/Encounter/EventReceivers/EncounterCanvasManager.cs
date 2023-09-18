using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Constants;

public class EncounterCanvasManager : EncounterEventReceiver
{
    [Header("Prefabs")]
    [SerializeField] private List<GameObject> CanvasPrefabs;
 
    //collection
    private List<EncounterUIElement> _elements;

    private void Awake()
    {
        _elements = new List<EncounterUIElement>();

        foreach(GameObject prefab in CanvasPrefabs)
        {
            GenerateUIElement(prefab);
        }
    }

    public override IEnumerator Coroutine_Init(EncounterModel model)
    {
        yield return null;
    }

    public override IEnumerator Coroutine_StateUpdate(EncounterState stateID, EncounterModel model)
    {
        foreach(EncounterUIElement element in _elements)
        {
            element.HandleStateUpdate(stateID, model);
        }

        if(stateID == EncounterState.SETUP_COMPLETE)
        {
            foreach(CharacterComponent character in model.GetAllCharacters())
            {
                character.CreateEncounterOverhead();
            }
        }
        else if(stateID == EncounterState.DONE)
        {
            foreach (CharacterComponent character in model.GetAllCharacters())
            {
                character.DestroyEncounterOverhead();
            }
        }

        yield return null;
    }

    public void ShowEventBanner(string message, float duration = 1.5f)
    {
        foreach(EncounterUIElement uIElement in _elements)
        {
            if(uIElement is EncounterEventBanner)
            {
                EncounterEventBanner eventBanner = (EncounterEventBanner) uIElement;
                eventBanner.DisplayMessage(message, duration);
            }
        }
    }

    private void GenerateUIElement(GameObject prefab)
    {
        GameObject gameObject = Instantiate<GameObject>(prefab);
        EncounterUIElement component = gameObject.GetComponent<EncounterUIElement>();
        component.Hide();
        _elements.Add(component);
    }
}
