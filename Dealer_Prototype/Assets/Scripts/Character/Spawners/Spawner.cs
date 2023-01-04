using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using GameDelegates;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] protected CharacterSpawnData spawnData;

    public bool SpawnOnClosestPoint;

    public virtual string GetSpawning()
    {
        return null;
    }


#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(this.transform.position, 0.1f);
        Gizmos.DrawRay(new Ray(this.transform.position, this.transform.forward));

        Handles.Label(this.transform.position + new Vector3(-0.5f, -0.5f, 0), GetSpawning());
    }
#endif
}
