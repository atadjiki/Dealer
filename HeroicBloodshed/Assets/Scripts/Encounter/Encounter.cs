using System;
using System.Collections;
using System.Collections.Generic;
using Constants;
using Cinemachine;
using UnityEngine;

public class Encounter : MonoBehaviour
{
    //setup phase
    [SerializeField] private EncounterSetupData setupData;

    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    public EncounterSetupData GetSetupData()
    {
        return setupData;
    }

    public CinemachineVirtualCamera GetCamera()
    {
        return virtualCamera;
    }
}
