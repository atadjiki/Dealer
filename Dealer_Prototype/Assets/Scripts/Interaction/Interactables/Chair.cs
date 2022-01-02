using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chair : Interactable
{
    [SerializeField] private Transform sittingPoseTransform;

    public Transform GetSittingPoseTransform() { return sittingPoseTransform; }

}
