using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class EncounterOverheads : EncounterUIElement
{
    [SerializeField] private GameObject Container_Panel;

    [SerializeField] private GameObject Prefab_CharacterUI;

    private Dictionary<CharacterComponent, EncounterCharacterUI> _dictionary;

    private bool _allowUpdate;

    private void Awake()
    {
        _dictionary = new Dictionary<CharacterComponent, EncounterCharacterUI>();    
    }

    public override void Populate(EncounterModel model)
    {
        foreach(CharacterComponent character in model.GetAllCharacters())
        {
            GameObject uiObject = Instantiate<GameObject>(Prefab_CharacterUI, Container_Panel.transform);

            EncounterCharacterUI characterUI = uiObject.GetComponent<EncounterCharacterUI>();
            characterUI.Populate(character);
            _dictionary.Add(character, characterUI);
        }
    }

    private void FixedUpdate()
    {
        if (!_allowUpdate) { return; }

        foreach(KeyValuePair<CharacterComponent, EncounterCharacterUI> pair in _dictionary)
        {
            CharacterComponent character = pair.Key;
            EncounterCharacterUI uiPanel = pair.Value;

            RectTransform rectTransform = uiPanel.GetComponent<RectTransform>();

            Vector3 screenPos = Camera.main.WorldToScreenPoint(character.GetOverheadAnchor().transform.position);

            screenPos.y += 25f;

            rectTransform.position = screenPos;
        }
    }

    public override void HandleStateUpdate(EncounterState stateID, EncounterModel model)
    {
        switch (stateID)
        {
            case EncounterState.SETUP_COMPLETE:
                {
                    Populate(model);
                    break;
                }
            case EncounterState.DONE:
                {
                    Hide();
                    _allowUpdate = false;
                    break;
                }
            default:
                {
                    _allowUpdate = true;
                    Show();
                    break;
                }
        }
    }
}
