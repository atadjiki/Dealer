using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class EnvironmentDoorSpawnPoint : EnvironmentSpawnPoint
{
    [Header("Door Mechanism")]
    [SerializeField] private GameObject DoorMesh;
    [SerializeField] private GameObject BackingMesh;

    private BoxCollider _collider;

    private void Awake()
    {
        DoorMesh.transform.localEulerAngles = Vector3.zero;
        BackingMesh.SetActive(false);

        _collider = GetComponent<BoxCollider>();
    }

    public override void Activate()
    {
        base.Activate();

        StartCoroutine(Coroutine_ActivateDoor());
    }

    private IEnumerator Coroutine_ActivateDoor()
    {
        DoorMesh.transform.localEulerAngles = new Vector3(0, 90, 0);
        BackingMesh.SetActive(true);

        yield return new WaitForSeconds(1.0f);

        DoorMesh.transform.localEulerAngles = Vector3.zero;
        BackingMesh.SetActive(false);
    }

}
