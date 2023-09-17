using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Constants;

public class EncounterCanvasManager : EncounterEventReceiver
{
    [Header("Prefabs")]
    [SerializeField] private GameObject Prefab_CurrentCharacter;
    [SerializeField] private GameObject Prefab_AbilitySelect;
    [SerializeField] private GameObject Prefab_TargetSelect;
    [SerializeField] private GameObject Prefab_TeamBanner;
    [SerializeField] private GameObject Prefab_StateDetail;
    [SerializeField] private GameObject Prefab_WinDialog;
    [SerializeField] private GameObject Prefab_LoseDialog;
 
    //collection
    private List<EncounterUIElement> _elements;


    public override IEnumerator Coroutine_Init(EncounterModel model)
    {
        yield break;
    }

    private void Awake()
    {
        _elements = new List<EncounterUIElement>();

        GenerateUIElement(Prefab_CurrentCharacter);
        GenerateUIElement(Prefab_AbilitySelect);
        GenerateUIElement(Prefab_TargetSelect);
        GenerateUIElement(Prefab_TeamBanner);
        GenerateUIElement(Prefab_StateDetail);
        GenerateUIElement(Prefab_WinDialog);
        GenerateUIElement(Prefab_LoseDialog);
    }

    public override IEnumerator Coroutine_StateUpdate(EncounterState stateID, EncounterModel model)
    {
        foreach(EncounterUIElement element in _elements)
        {
            element.HandleStateUpdate(stateID, model);
        }

        switch (stateID)
        {
            case EncounterState.CHOOSE_ACTION:
                {
                    if(model.GetCurrentTeam() == TeamID.Player)
                    {
                        if (!model.IsCurrentTeamCPU())
                        {

                        }
                    }
                    break;
                }
            case EncounterState.CHOOSE_TARGET:
                {
                    if (model.GetCurrentTeam() == TeamID.Player)
                    {
                        if (!model.IsCurrentTeamCPU())
                        {

                        }
                    }
                    break;
                }
            case EncounterState.TEAM_UPDATED:
                {
                    if (model.IsCurrentTeamCPU())
                    {

                    }
                    break;
                }
            default:
                {
                    break;
                }
        }

        yield return null;
    }

    private void GenerateUIElement(GameObject prefab)
    {
        GameObject gameObject = Instantiate<GameObject>(prefab);
        EncounterUIElement component = gameObject.GetComponent<EncounterUIElement>();
        component.Hide();
        _elements.Add(component);
    }
}
