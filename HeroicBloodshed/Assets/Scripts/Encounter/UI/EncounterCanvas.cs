using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Constants;

public class EncounterCanvas : EncounterEventReceiver
{
    [Header("Groups")]
    [SerializeField] private GameObject Panel_PlayerTurn;
    [SerializeField] private GameObject Panel_TargetSelection;
    [SerializeField] private GameObject Panel_CPUTurn;

    //list elements
    [Header("Containers")]
    [SerializeField] private EncounterAbilityList AbilityList;
    [SerializeField] private EncounterCharacterQueue PlayerQueue;
    [SerializeField] private EncounterCharacterQueue EnemyQueue;

    //cpu turn banner
    [Header("Team Banner")]
    [SerializeField] private EncounterTeamBanner TeamBanner;

    //state detail
    [Header("State Detail")]
    [SerializeField] private TextMeshProUGUI Text_StateDetail;

    //elements
    [Header("Fade Panel")]
    [SerializeField] private EncounterFadePanel FadePanel;

    private void Awake()
    {
        HideAll();
    }

    public override IEnumerator Coroutine_Init(EncounterModel model)
    {
        yield return FadePanel.Coroutine_PerformFadeToBlack();
    }

    public override IEnumerator Coroutine_StateUpdate(EncounterState stateID, EncounterModel model)
    {
        EncounterState state = model.GetState();

        switch (state)
        {
            case EncounterState.SETUP_COMPLETE:
                {
                    yield return FadePanel.Coroutine_PerformFadeToClear();
                    break;
                }
            case EncounterState.CHOOSE_ACTION:
                {
                    if(model.GetCurrentTeam() == TeamID.Player)
                    {
                        if (!model.IsCurrentTeamCPU())
                        {
                            PopulatePlayerTurnPanel(model);
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
                            PopulateTargetSelectionMode(model);
                        }
                    }
                    break;
                }
            case EncounterState.TEAM_UPDATED:
                {
                    yield return new WaitForSeconds(1.0f);
                    if (model.IsCurrentTeamCPU())
                    {
                        PopulateCPUTurnPanel(model);
                    }
                    break;
                }
            default:
                {
                    HideAll();
                    break;
                }
        }

        Text_StateDetail.text = GetDisplayString(state);

        yield return null;
    }

    private void PopulatePlayerTurnPanel(EncounterModel model)
    {
        Panel_CPUTurn.SetActive(false);
        Panel_TargetSelection.SetActive(false);
        Panel_PlayerTurn.SetActive(true);

        foreach (EncounterCanvasElement element in Panel_PlayerTurn.GetComponentsInChildren<EncounterCanvasElement>())
        {
            element.Populate(model);
        }
    }

    private void PopulateCPUTurnPanel(EncounterModel model)
    {
        Panel_PlayerTurn.SetActive(false);
        Panel_TargetSelection.SetActive(false);
        Panel_CPUTurn.SetActive(true);

        foreach (EncounterCanvasElement element in Panel_CPUTurn.GetComponentsInChildren<EncounterCanvasElement>())
        {
            element.Populate(model);
        }
    }

    private void PopulateTargetSelectionMode(EncounterModel model)
    {
        Panel_CPUTurn.SetActive(false);
        Panel_PlayerTurn.SetActive(false);
        Panel_TargetSelection.SetActive(true);

        foreach (EncounterCanvasElement element in Panel_TargetSelection.GetComponentsInChildren<EncounterCanvasElement>())
        {
            element.Populate(model);
        }
    }

    private void HideAll()
    {
        Panel_PlayerTurn.SetActive(false);
        Panel_TargetSelection.SetActive(false);
        Panel_CPUTurn.SetActive(false);

        ClearContainers();

        Text_StateDetail.text = string.Empty;
    }

    private void ClearContainers()
    {
        ClearPlayerTurnPanel();
        ClearCPUTurnPanel();
        ClearTargetPanel();
    }

    private void ClearPlayerTurnPanel()
    {
        foreach (EncounterCanvasElement element in Panel_PlayerTurn.GetComponentsInChildren<EncounterCanvasElement>())
        {
            element.Clear();
        }
    }

    private void ClearCPUTurnPanel()
    {
        foreach (EncounterCanvasElement element in Panel_CPUTurn.GetComponentsInChildren<EncounterCanvasElement>())
        {
            element.Clear();
        }
    }

    private void ClearTargetPanel()
    {
        foreach (EncounterCanvasElement element in Panel_TargetSelection.GetComponentsInChildren<EncounterCanvasElement>())
        {
            element.Clear();
        }
    }
}
