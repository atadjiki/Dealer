using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Constants;

public class CityMapCanvas : MonoBehaviour
{
    [SerializeField] private CityMapDescriptionPanel descriptionPanel;
    [SerializeField] private Button Button_Back;
    [SerializeField] private Button Button_Visit;

    [SerializeField] private Button Button_Downtown;

    private void Awake()
    {
        descriptionPanel.gameObject.SetActive(false);
        Button_Visit.gameObject.SetActive(false);

        Button_Downtown.onClick.AddListener(delegate () { OnDistrictClicked(Enumerations.DistrictName.Downtown); });
        Button_Back.onClick.AddListener(delegate () { OnBackClicked(); });
    }

    private void OnDistrictClicked(Enumerations.DistrictName district)
    {
        descriptionPanel.Populate(district);
        descriptionPanel.gameObject.SetActive(true);

        Button_Visit.gameObject.SetActive(true);
    }

    private void OnBackClicked()
    {
        Destroy(this.gameObject);
    }
}
