using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class ObjectStateComponent : MonoBehaviour
{ 
    [SerializeField] private RegistryID RegistryID;

    public virtual void SetRegistryID(RegistryID _ID) { RegistryID = _ID; }

    public virtual string GetID() { return RegistryID.ToString(); }
}
