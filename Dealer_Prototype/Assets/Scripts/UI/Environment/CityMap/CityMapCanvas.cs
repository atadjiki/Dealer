using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Constants;
using GameDelegates;
using System;

public class CityMapCanvas : MonoBehaviour
{
    [SerializeField] private CityMapDescriptionPanel descriptionPanel;
    [SerializeField] private Button Button_Back;
    [SerializeField] private Button Button_Visit;

    [SerializeField] private Button Button_Downtown;

    public BackButtonPressed OnBackButtonPressed;

    private void Awake()
    {
        descriptionPanel.gameObject.SetActive(false);
        Button_Visit.gameObject.SetActive(false);

        Button_Downtown.onClick.AddListener(delegate () { OnDistrictClicked(Enumerations.DistrictName.Downtown); });
        Button_Back.onClick.AddListener(delegate () { OnBackClicked(); });
        Button_Visit.onClick.AddListener(delegate () { OnVisitClicked();  });
    }

    private void OnDistrictClicked(Enumerations.DistrictName district)
    {
        AudioUtility.ButtonClick();

        descriptionPanel.Populate(district);
        descriptionPanel.gameObject.SetActive(true);

        Button_Visit.gameObject.SetActive(true);
    }

    private void OnBackClicked()
    {
        AudioUtility.ButtonClick();

        if (OnBackButtonPressed != null)
        {
            OnBackButtonPressed.Invoke();
        }

        Destroy(this.gameObject);
    }

    private void OnVisitClicked()
    {
        AudioUtility.ButtonClick();

        GameObject dialogObject = Instantiate<GameObject>(PrefabLibrary.GetOKCancelCanvas(), null);
        DialogPanel dialogPanel = dialogObject.GetComponent<DialogPanel>();

        Action okAction = new Action(OnOKPressed);
        Action cancelAction = new Action(OnCancelPressed);

        dialogPanel.Setup("Head Outside?", "", new Action(OnOKPressed), OnCancelPressed);
    }

   private void OnOKPressed()
   {
        AudioUtility.ButtonClick();

        LevelUtility.GoToLoading(LevelUtility.PlayerLocation.City);
    }

    private void OnCancelPressed()
    {
        AudioUtility.ButtonClick();
    }
}

