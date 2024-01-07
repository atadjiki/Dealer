using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class EnvironmentCoverDisplay : EnvironmentInputHandler
{
    [Header("Prefab")]
    [SerializeField] private GameObject Prefab_CoverDecal;

    private List<EnvironmentCoverDecal> _coverDecals;
    private List<Vector3> _validNodes;

    public override void Activate()
    {
        base.Activate();

        StartCoroutine(Coroutine_GenerateCoverDecals());
    }

    public override void Deactivate()
    {
        base.Deactivate();

        _coverDecals.Clear();
        _validNodes.Clear();

        for (int i = 0; i < this.transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    public override IEnumerator PerformInputUpdate(EnvironmentInputData InputData)
    {
        foreach(EnvironmentCoverDecal coverDecal in _coverDecals)
        {
            Vector3 decalPos = coverDecal.transform.position;

            bool flag = _validNodes.Contains(decalPos) || decalPos == InputData.NodePosition;

            coverDecal.gameObject.SetActive(flag);
        }

        yield return null;
    }

    private IEnumerator Coroutine_GenerateCoverDecals()
    {
        Dictionary<Vector3, List<EnvironmentCoverData>> coverMap = EnvironmentManager.Instance.GetCoverMap();

        if (coverMap == null) { yield break; }

        _validNodes = new List<Vector3>();

        CharacterComponent currentCharacter = EncounterManager.Instance.GetCurrentCharacter();

        //first gather all the valid nodes near the character
        Vector3 characterPosition = currentCharacter.GetWorldLocation();

        float characterRange = currentCharacter.GetMovementRange();

        Bounds bounds = new Bounds(characterPosition, new Vector3(characterRange, 0, characterRange));

        foreach(GraphNode graphNode in EnvironmentUtil.GetGraphNodesInBounds(bounds))
        {
            if(graphNode.Walkable)
            {
                _validNodes.Add((Vector3)graphNode.position);
            }
        }

        _coverDecals = new List<EnvironmentCoverDecal>();

        foreach (KeyValuePair<Vector3, List<EnvironmentCoverData>> pair in coverMap)
        {
            Vector3 origin = pair.Key;

            foreach (EnvironmentCoverData coverData in pair.Value)
            {
                GameObject decalObject = Instantiate<GameObject>(Prefab_CoverDecal, this.transform);

                EnvironmentCoverDecal coverDecal = decalObject.GetComponent<EnvironmentCoverDecal>();

                coverDecal.Setup(coverData.ObstacleType);

                decalObject.transform.position = origin;
                decalObject.transform.localEulerAngles = GetEulerAnglesFromDirection(coverData.Direction);

                if(EnvironmentUtil.IsNodeExposed(origin))
                {
                    coverDecal.SetExposed();
                }

                decalObject.SetActive(false);

                _coverDecals.Add(coverDecal);
            }
        }

    }

    private Vector3 GetEulerAnglesFromDirection(Vector3 direction)
    {
        if(direction == Vector3.forward)
        {
            return new Vector3(0, 0, 0);
        }
        else if (direction == Vector3.forward * -1)
        {
            return new Vector3(0, -180, 0);
        }
        else if (direction == Vector3.right)
        {
            return new Vector3(0, 90, 0);
        }
        else if (direction == Vector3.right * -1)
        {
            return new Vector3(0, -90, 0);
        }

        return Vector3.zero;
    }
}
