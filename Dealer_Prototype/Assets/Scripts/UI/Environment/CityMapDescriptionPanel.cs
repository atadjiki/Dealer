using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using Constants;
using UnityEngine;

public class CityMapDescriptionPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Text_DistrictName;
    [SerializeField] private TextMeshProUGUI Text_DangerValue;
    [SerializeField] private TextMeshProUGUI Text_DemandValue;
    [SerializeField] private TextMeshProUGUI Text_DistrictBlurb;

    public void Populate(Enumerations.DistrictName district)
    {
        DistrictInfo districtInfo = CityConstants.GetInfo(district);

        Text_DistrictName.text = districtInfo.District.ToString();
        Text_DangerValue.text = districtInfo.Danger.ToString();
        Text_DemandValue.text = districtInfo.Demand.ToString();
        Text_DistrictBlurb.text = districtInfo.Blurb;
    }
}
