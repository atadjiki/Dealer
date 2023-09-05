using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Constants;

public class EncounterCanvas : MonoBehaviour, IEncounter
{
    //elements
    [Header("Elements")]
    [SerializeField] private Image Panel_Fade;
    [SerializeField] private GameObject Panel_PlayerTurn;

    //the containers for prefabs to be spawned in
    [Header("Containers")]
    [SerializeField] private GameObject Panel_PlayerQueue;
    [SerializeField] private GameObject Panel_EnemyQueue;
    [SerializeField] private GameObject Panel_AbilityList;

    [SerializeField] private TextMeshProUGUI Text_CurrentTeam;
    [SerializeField] private TextMeshProUGUI Text_TurnCount;
    [SerializeField] private TextMeshProUGUI Text_StateDetail;

    [Header("Prefabs")]
    [SerializeField] private GameObject Prefab_PlayerQueue_Item;
    [SerializeField] private GameObject Prefab_EnemyQueue_Item;
    [SerializeField] private GameObject Prefab_Ability_Button;

    private void Awake()
    {
        HideAll();
    }

    public IEnumerator HandleInit()
    {
        Panel_PlayerTurn.SetActive(false);

        yield return Coroutine_PerformFadeToBlack();
    }

    public void UpdateCanvas(Encounter encounter)
    {
        StartCoroutine(Coroutine_UpdateCanvas(encounter));
    }

    private IEnumerator Coroutine_UpdateCanvas(Encounter encounter)
    {
        EncounterState state = encounter.GetState();

        switch (state)
        {
            case EncounterState.SETUP_COMPLETE:
            {
                yield return Coroutine_PerformFadeToClear();
                break;
            }
            case EncounterState.WAIT_FOR_PLAYER_INPUT:
            {
                PopulatePlayerTurnPanel(encounter);
                break;
            }
            case EncounterState.DONE:
            {
                encounter.OnStateChanged -= this.UpdateCanvas;
                Destroy(this.gameObject);
                yield break;
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

    private void PopulatePlayerTurnPanel(Encounter encounter)
    {
        Panel_PlayerTurn.SetActive(true);

        Text_CurrentTeam.text = (encounter.GetCurrentTeam().ToString() + " team").ToLower(); ;
        Text_CurrentTeam.color = Constants.GetColorByTeam(encounter.GetCurrentTeam());

        Text_TurnCount.text = ("Turn " + encounter.GetTurnCount().ToString()).ToLower();

        PopulateAbilityList(encounter);

        PopulateQueues(encounter);
    }

    private void PopulateAbilityList(Encounter encounter)
    {
        CharacterComponent character = encounter.GetCurrentCharacter();

        Debug.Log(GetAllowedAbilities(character.GetID()).Count + " abilities counted");

        foreach (AbilityID abilityID in GetAllowedAbilities(character.GetID()))
        {

            GameObject ButtonObject = Instantiate(Prefab_Ability_Button, Panel_AbilityList.transform);
            EncounterAbilityButton abilityButton = ButtonObject.GetComponent<EncounterAbilityButton>();
            abilityButton.Populate(abilityID);
            abilityButton.onClick.AddListener(() => OnAbilityButtonClicked(abilityButton));

            if (GetAllowedAbilities(character.GetID()).IndexOf(abilityID) == 0)
            {
                abilityButton.Select();
            }
        }
    }

    private void OnAbilityButtonClicked(EncounterAbilityButton button)
    {
        if(EncounterManager.Instance != null)
        {
            EncounterManager.Instance.SelectAbility(button.GetAbilityID());
        }
    }

    private void PopulateQueues(Encounter encounter)
    { 

        //add a portrait for each character in the player queue
        foreach (CharacterComponent character in encounter.GetAllCharactersInTeam(TeamID.Player))
        {
            GameObject queueItemObject = Instantiate(Prefab_PlayerQueue_Item, Panel_PlayerQueue.transform);
            EncounterPlayerQueueItem playerQueueItem = queueItemObject.GetComponent<EncounterPlayerQueueItem>();
            playerQueueItem.Setup(character);

            if(character == encounter.GetCurrentCharacter())
            {
                playerQueueItem.SetActive();
            }
            else if(character.IsDead())
            {
                playerQueueItem.SetDead();
            }
            else
            {
                playerQueueItem.SetInactive();
            }
        }

        //add a line of text for reach character in the enemy queue
        foreach(CharacterComponent character in encounter.GetAllCharactersInTeam(TeamID.Enemy))
        {
            GameObject queueItemObject = Instantiate(Prefab_EnemyQueue_Item, Panel_EnemyQueue.transform);
            EncounterEnemyQueueItem enemyQueueItem = queueItemObject.GetComponent<EncounterEnemyQueueItem>();
            enemyQueueItem.Setup(character);

            if (character == encounter.GetCurrentCharacter())
            {
                enemyQueueItem.SetActive();
            }
            else if (character.IsDead())
            {
                enemyQueueItem.SetDead();
            }
            else
            {
                enemyQueueItem.SetInactive();
            }
        }
    }

    private void HideAll()
    {
        Panel_PlayerTurn.SetActive(false);

        //first, make sure queues are empty
        DestroyTransformChildren(Panel_PlayerQueue.transform);
        DestroyTransformChildren(Panel_EnemyQueue.transform);

        //clear our the ability list
        DestroyTransformChildren(Panel_AbilityList.transform);

        //clear text
        Text_StateDetail.text = string.Empty;
        Text_TurnCount.text = string.Empty;
        Text_CurrentTeam.text = string.Empty;
    }

    private IEnumerator Coroutine_PerformFadeToBlack()
    {
        StopCoroutine("Coroutine_PerformFadeToClear");
        yield return Coroutine_PerformFadeBetween(Color.clear, Color.black, 0.25f);
    }

    private IEnumerator Coroutine_PerformFadeToClear()
    {
        StopCoroutine("Coroutine_PerformFadeToClear");
        yield return Coroutine_PerformFadeBetween(Color.black, Color.clear, 0.25f);
    }

    private IEnumerator Coroutine_PerformFadeBetween(Color to, Color from, float duration)
    {
        Debug.Log("Fade from " + to.ToString() + " to " + from.ToString());
        Panel_Fade.color = to;

        float currentTime = 0;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            Panel_Fade.color = Color.Lerp(to, from, currentTime / duration);
            yield return null;
        }
    }

    private void DestroyTransformChildren(Transform parentTransform)
    {
        for (int i = 0; i < parentTransform.childCount; i++)
        {
            DestroyImmediate(parentTransform.GetChild(i).gameObject);
        }
    }
}
