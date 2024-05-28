using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HighlightPlus;
using static Constants;

public class EnvironmentMovementRadius : EncounterEventHandler
{
    private Dictionary<MovementRangeType, List<Vector3>> _rangeMap;

    private MaterialLibrary matLib;

    protected override void OnAwake()
    {
        base.OnAwake();

        matLib = MaterialLibrary.Get();
    }

    protected override void OnStateChangedCallback(EncounterStateData stateData)
    {
        if (stateData.GetCurrentState() == EncounterState.PERFORM_ACTION)
        {
            Destroy(this.gameObject);
        }
    }

    public void Setup(CharacterComponent character)
    {
        _rangeMap = EnvironmentUtil.GetCharacterRangeMap(character);

        //cull any movement half tiles from the movement full list

        foreach(Vector3 point in _rangeMap[MovementRangeType.HALF])
        {
            if(_rangeMap[MovementRangeType.FULL].Contains(point))
            {
                _rangeMap[MovementRangeType.FULL].Remove(point);
            }
        }

        foreach(MovementRangeType ID in _rangeMap.Keys)
        {
            CreateRadiusMesh(ID, _rangeMap[ID]);
        }
    }

    private void CreateRadiusMesh(MovementRangeType rangeType, List<Vector3> nodes)
    {
        GameObject container = new GameObject("Container " + rangeType.ToString());
        container.transform.parent = this.transform;
        MeshRenderer renderer = container.AddComponent<MeshRenderer>();
        renderer.material = matLib.MovementRadius;

        MeshFilter filter = container.AddComponent<MeshFilter>();

        //create a quad for each tile
        foreach (Vector3 tile in nodes)
        {
            GameObject quadObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
            quadObject.transform.parent = container.transform;
            quadObject.transform.localPosition = tile + new Vector3(0, 0.1f, 0);
            quadObject.transform.localEulerAngles = new Vector3(90, 0, 0);
            quadObject.transform.localScale = new Vector3(ENV_TILE_SIZE, ENV_TILE_SIZE, ENV_TILE_SIZE);

        }

        //grab all the meshes from the quads
        MeshFilter[] meshFilters = container.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].mesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);

            i++;
        }
        
        Mesh mesh = new Mesh();
        mesh.CombineMeshes(combine);
        filter.mesh = mesh;

        HighlightProfileLibrary highlightLib = HighlightProfileLibrary.Get();

        HighlightEffect effect = container.AddComponent<HighlightEffect>();
        HighlightProfile profile = highlightLib.GetMovementRadiusProfile(rangeType);

        if(rangeType == MovementRangeType.HALF)
        {
            effect.subMeshMask = 1;
        }

        effect.profile = profile;
        effect.ProfileLoad(profile);
        effect.highlighted = true;

        container.SetActive(true);
    }
}
