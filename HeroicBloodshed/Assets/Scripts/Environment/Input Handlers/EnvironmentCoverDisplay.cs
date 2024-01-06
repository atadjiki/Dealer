using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentCoverDisplay : EnvironmentInputHandler
{
    [Header("Prefab")]
    [SerializeField] private GameObject Prefab_CoverDecal;

    public override void Activate()
    {
        base.Activate();

        StartCoroutine(Coroutine_GenerateCoverDecals());
    }

    public override void Deactivate()
    {
        base.Deactivate();

        for (int i = 0; i < this.transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    private IEnumerator Coroutine_GenerateCoverDecals()
    {
        Dictionary<Vector3, List<Vector3>> coverMap = EnvironmentManager.Instance.GetCoverMap();

        if (coverMap == null) { yield break; }

        foreach (KeyValuePair<Vector3, List<Vector3>> pair in coverMap)
        {
            Vector3 origin = pair.Key;

            foreach(Vector3 direction in pair.Value)
            {
                GameObject decalObject = Instantiate<GameObject>(Prefab_CoverDecal, this.transform);

                EnvironmentCoverDecal coverDecal = decalObject.GetComponent<EnvironmentCoverDecal>();
                coverDecal.Setup(Constants.EnvironmentObstacleType.FullCover);

                decalObject.transform.position = origin;
                decalObject.transform.localEulerAngles = GetEulerAnglesFromDirection(direction);
            }
        }

        yield return null;
    }

    private Vector3 GetEulerAnglesFromDirection(Vector3 direction)
    {
        if(direction == Vector3.forward)
        {
            return new Vector3(0, 0, 0);
        }
        else if (direction == Vector3.forward * -1)
        {
            return new Vector3(0, 0, 0);
        }
        else if (direction == Vector3.right)
        {
            return new Vector3(0, 90, 0);
        }
        else if (direction == Vector3.right * -1)
        {
            return new Vector3(0, 90, 0);
        }

        return Vector3.zero;
    }
}
