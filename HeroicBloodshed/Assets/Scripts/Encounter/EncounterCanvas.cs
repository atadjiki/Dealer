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

    [SerializeField] private TextMeshProUGUI Text_StateDetail;

    [Header("Prefabs")]
    [SerializeField] private GameObject Prefab_Portrait;
    [SerializeField] private GameObject Prefab_EnemyText;
    [SerializeField] private GameObject Prefab_AbilityButton;

    private void Awake()
    {
        HideAll();
    }

    public IEnumerator HandleInit()
    {
        Panel_PlayerTurn.SetActive(false);

        yield return Coroutine_PerformFadeToBlack();
    }

    public void EncounterStateCallback(Encounter encounter)
    {
        StartCoroutine(Coroutine_EncounterStateCallback(encounter));
    }

    private IEnumerator Coroutine_EncounterStateCallback(Encounter encounter)
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
                encounter.OnStateChanged -= this.EncounterStateCallback;
                Destroy(this.gameObject);
                yield break;
            }
            default:
            {
                HideAll();
                break;
            }
        }

        Text_StateDetail.text = PopulateStateDetailText(state);

        yield return null;
    }

    private string PopulateStateDetailText(EncounterState state)
    {
        switch(state)
        {
            case EncounterState.PERFORM_ACTION:
                return "performing action...";
            case EncounterState.WAIT_FOR_PLAYER_INPUT:
                return "waiting for input...";
            case EncounterState.CHOOSE_AI_ACTION:
                return "AI choosing action...";
            default:
                return string.Empty;
        }
    }

    private void PopulatePlayerTurnPanel(Encounter encounter)
    {
        Panel_PlayerTurn.SetActive(true);

        PopulateAbilityList(encounter);

        PopulateQueues(encounter);
    }

    private void PopulateAbilityList(Encounter encounter)
    {
        CharacterComponent character = encounter.GetCurrentCharacter();

        Debug.Log(GetAllowedAbilities(character.GetID()).Count + " abilities counted");

        foreach (AbilityID abilityID in GetAllowedAbilities(character.GetID()))
        {
            GameObject ButtonObject = Instantiate(Prefab_AbilityButton, Panel_AbilityList.transform);
            EncounterAbilityButton abilityButton = ButtonObject.GetComponent<EncounterAbilityButton>();
            abilityButton.Populate(abilityID);
            abilityButton.onClick.AddListener(() => OnAbilityButtonClicked(abilityButton));
        }
    }

    private void OnAbilityButtonClicked(EncounterAbilityButton button)
    {
        Debug.Log("ability selected " + button.GetAbilityID());
    }

    private void PopulateQueues(Encounter encounter)
    { 

        //add a portrait for each character in the player queue
        foreach (CharacterComponent character in encounter.GetAllCharactersInTeam(TeamID.Player))
        {
            GameObject portraitObject = Instantiate(Prefab_Portrait, Panel_PlayerQueue.transform);
            EncounterPortraitPanel portraitPanel = portraitObject.GetComponent<EncounterPortraitPanel>();
            portraitPanel.Setup(character);

            if(character == encounter.GetCurrentCharacter())
            {
                portraitPanel.SetActive();
            }
            else if(character.IsDead())
            {
                portraitPanel.SetDead();
            }
            else
            {
                portraitPanel.SetInactive();
            }
        }

        //add a line of text for reach character in the enemy queue
        foreach(CharacterComponent character in encounter.GetAllCharactersInTeam(TeamID.Enemy))
        {
            GameObject textObject = Instantiate(Prefab_EnemyText, Panel_EnemyQueue.transform);
            TextMeshProUGUI textMesh = textObject.GetComponent<TextMeshProUGUI>();
            textMesh.text = character.GetID().ToString();

            if(character.IsDead())
            {
                textMesh.color = Color.grey;
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
