using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class EnvironmentPathDisplay : EncounterEventHandler
{
    [SerializeField] private Material LineMaterial;

    private LineRenderer _lineRenderer;

    protected override void Setup()
    {
        base.Setup();

        this.gameObject.layer = LAYER_DECAL;

        SetupLineRenderer();
    }

    protected override void OnStateChangedCallback(EncounterState state)
    {
        if(state == EncounterState.PERFORM_ACTION)
        {
            Destroy(this.gameObject);
        }
    }

    private void SetupLineRenderer()
    {
        _lineRenderer = this.gameObject.AddComponent<LineRenderer>();
        _lineRenderer.material = LineMaterial;
        _lineRenderer.startWidth = 0.1f;
        _lineRenderer.endWidth = 0.1f;

    }

    private void Update()
    {
        Vector3 origin = this.transform.position;
        EnvironmentTileRaycastInfo info;
        if(EnvironmentUtil.GetTileBeneathMouse(out info))
        {
            if(info.layer == EnvironmentLayer.GROUND)
            {
                List<Vector3> nodes = EnvironmentUtil.CalculatePath(origin, info.position);

                _lineRenderer.positionCount = nodes.Count;
                _lineRenderer.SetPositions(nodes.ToArray());
                return;
            }
        }

        _lineRenderer.positionCount = 0;
    }
}
