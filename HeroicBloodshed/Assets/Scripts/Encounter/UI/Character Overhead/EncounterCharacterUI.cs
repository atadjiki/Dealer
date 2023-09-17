using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Constants;

public class EncounterCharacterUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Text_Name;

    [SerializeField] private Transform Container_HP;

    [SerializeField] private GameObject Prefab_Health_Item;

    public void Populate(CharacterComponent character)
    {
        UIHelper.ClearTransformChildren(Container_HP);

        Text_Name.text = GetDisplayString(character.GetID());

        int health = character.GetHealth();

        float parentWidth = Container_HP.GetComponent<RectTransform>().rect.width;
        Debug.Log("parentWidth " + parentWidth);

        float childWidth = parentWidth / health;
        Debug.Log("childWidth" + childWidth);

        for (int i = 0; i < health; i++)
        {
            GameObject healthItem = Instantiate<GameObject>(Prefab_Health_Item, Container_HP);
            RectTransform rectTransform = healthItem.GetComponent<RectTransform>();
            rectTransform.rect.Set(rectTransform.rect.x, rectTransform.rect.y, childWidth, rectTransform.rect.height);

        }
    }
}
