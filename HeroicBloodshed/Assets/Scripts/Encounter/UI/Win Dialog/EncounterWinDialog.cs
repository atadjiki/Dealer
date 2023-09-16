using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EncounterWinDialog : MonoBehaviour
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

    private void Populate(EncounterModel model)
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
