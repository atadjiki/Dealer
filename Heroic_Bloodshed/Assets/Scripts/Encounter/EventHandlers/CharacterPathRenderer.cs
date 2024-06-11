using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using static Constants;

[RequireComponent(typeof(LineRenderer))]
public class CharacterPathRenderer : EncounterEventHandler
{
    [Header("Color Library")]
    [SerializeField] private ColorLibrary ColorLib;

    private LineRenderer _lineRenderer;

    private Vector3 _origin;
    private Dictionary<MovementRangeType, List<Vector3>> _rangeMap;

    public void Setup(CharacterComponent character)
    {
        //set the origin
        _origin = character.GetWorldLocation();

        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.startWidth = 0.1f;
        _lineRenderer.endWidth = 0.1f;

        //figure out what the range is for this character
        _rangeMap = EnvironmentUtil.GetCharacterRangeMap(character);
    }

    protected override void OnStateChangedCallback(EncounterStateData stateData)
    {
        if (stateData.GetCurrentState() == EncounterState.PERFORM_ACTION)
        {
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        if (_rangeMap == null) return;

        TileNode node;
        if(EnvironmentUtil.GetNodeBeneathMouse(out node))
        {
            MovementRangeType rangeType = GetRangeTypeFromPoint((Vector3)node.position);

            if (node.layer == EnvironmentLayer.GROUND && rangeType != MovementRangeType.NONE)
            {
                Vector3 destination = (Vector3)node.position;

                List<Vector3> vectors = EnvironmentUtil.CalculateVectorPath(_origin, destination);

                _lineRenderer.positionCount = vectors.Count;
                _lineRenderer.SetPositions(vectors.ToArray());

                _lineRenderer.material.color = ColorLib.Get(rangeType);

                return;
            }
        }

        _lineRenderer.positionCount = 0;
    }

    private MovementRangeType GetRangeTypeFromPoint(Vector3 point)
    {
        if (_rangeMap[MovementRangeType.HALF].Contains(point))
        {
            return MovementRangeType.HALF;
        }
        else if (_rangeMap[MovementRangeType.FULL].Contains(point))
        {
            return MovementRangeType.FULL;
        }
        else
        {
            return MovementRangeType.NONE;
        }
    }
}
