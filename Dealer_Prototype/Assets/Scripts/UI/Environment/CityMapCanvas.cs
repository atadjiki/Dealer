using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Constants;
using GameDelegates;

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
        descriptionPanel.Populate(district);
        descriptionPanel.gameObject.SetActive(true);

        Button_Visit.gameObject.SetActive(true);
    }

    private void OnBackClicked()
    {
        if(OnBackButtonPressed != null)
        {
            OnBackButtonPressed.Invoke();
        }

        Destroy(this.gameObject);
    }

    private void OnVisitClicked()
    {
        GameObject dialogObject = Instantiate<GameObject>(PrefabLibrary.GetOKCancelCanvas(), null);
        DialogPanel dialogPanel = dialogObject.GetComponent<DialogPanel>();

        dialogPanel.OnOKPressed += () => OnOKPressed(dialogPanel);
        dialogPanel.OnCancelPressed += () => OnCancelPressed(dialogPanel);

        dialogPanel.Setup("Head Outside?", "");
    }

   private void OnOKPressed(DialogPanel dialogPanel)
   {
        Destroy(dialogPanel.gameObject);
        Debug.Log("loading dealer level");
    }

    private void OnCancelPressed(DialogPanel dialogPanel)
    {
        Destroy(dialogPanel.gameObject);
    }
}

