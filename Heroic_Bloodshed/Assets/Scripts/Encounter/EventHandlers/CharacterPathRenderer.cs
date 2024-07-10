using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using CurveLib.Curves;
using static Constants;

[RequireComponent(typeof(LineRenderer))]
public class CharacterPathRenderer : EncounterEventHandler
{
    [Header("Color Library")]
    [SerializeField] private ColorLibrary ColorLib;

    [Header("Curve Params")]
    [SerializeField] private float Tension = 0.5f;
    [SerializeField] private SplineType Type;

    private LineRenderer _lineRenderer;

    private Vector3 _origin;
    private Dictionary<MovementRangeType, List<Vector3>> _rangeMap;

    public void Setup(CharacterComponent character)
    {
        //set the origin
        _origin = character.GetWorldLocation();

        _lineRenderer = GetComponent<LineRenderer>();

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
            MovementRangeType rangeType = GetRangeTypeFromPoint(node.GetGridPosition());

            if (node.layer == EnvironmentLayer.GROUND && rangeType != MovementRangeType.NONE)
            {
                Vector3 destination = node.GetTruePosition();

                HashSet<Vector3> vectorSet = new HashSet<Vector3>(EnvironmentUtil.CalculateVectorPath(_origin, destination));

                if(vectorSet.Count > 0)
                {
                    SplineCurve curve = new SplineCurve(new List<Vector3>(vectorSet).ToArray(), false, SplineType.Centripetal, Tension);

                    float length = curve.GetLength();
                    Vector3[] points = curve.GetPoints((int)(length * 4));

                    _lineRenderer.positionCount = points.Length;
                    _lineRenderer.SetPositions(points);
                    _lineRenderer.material.color = ColorLib.Get(rangeType);

                    return;
                }
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
