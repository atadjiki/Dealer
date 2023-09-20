using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Constants;

public class EncounterWinDialog : EncounterUIElement
{ 
    [Header("Variables")]
    [SerializeField] private Button Button_Exit;
    [SerializeField] private GameObject Container;

    [Header("Prefab")]
    [SerializeField] private GameObject Prefab_DetailItem;

    private void Awake()
    {
        //assign delegates
        Button_Exit.onClick.AddListener(() => OnExitButtonPressed());  
    }

    public override void Populate(EncounterModel model)
    {
        //player casualties
        int casualties = model.GetDeadCount(Constants.TeamID.Player);
        GenerateDetailItem("Casualties", casualties.ToString());

        //enemies killed
        int killCount = model.GetDeadCount(Constants.TeamID.Enemy);
        GenerateDetailItem("Enemies Killed", killCount.ToString());

        //turns taken
        int turnCount = model.GetTurnCount();
        GenerateDetailItem("Turns Taken", turnCount.ToString());
    }

    public override void HandleStateUpdate(Constants.EncounterState stateID, EncounterModel model)
    {
        switch (stateID)
        {
            case EncounterState.DONE:
                {
                    if (model.DidPlayerWin())
                    {
                        Populate(model);
                        Show();
                    }
                    else
                    {
                        Hide();
                    }
                    break;
                }
            default:
                {
                    Hide();
                    break;
                }
        }
    }

    private void GenerateDetailItem(string detail, string value)
    {
        GameObject detailObject = Instantiate<GameObject>(Prefab_DetailItem, Container.transform);

        EncounterWinDetailItem detailItem = detailObject.GetComponent<EncounterWinDetailItem>();

        detailItem.Populate(detail, value);
    }

    private void OnExitButtonPressed()
    {
        //tell encounter manager to clean up 
        GameObject.Destroy(this.gameObject);
    }
}
