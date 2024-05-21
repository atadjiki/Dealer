using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HighlightPlus;
using static Constants;

public class EnvironmentMovementRadius : MonoBehaviour
{
    [SerializeField] private Material OutlineMaterial;
    [SerializeField] private HighlightProfile OutlineSettings;

    private int distance = 12;
    private List<Vector3> tiles;

    private MeshRenderer _renderer;
    private MeshFilter _filter;
    private HighlightEffect _outline;

    private void Awake()
    {
        _renderer = this.gameObject.AddComponent<MeshRenderer>();
        _renderer.material = OutlineMaterial;
        _filter = this.gameObject.AddComponent<MeshFilter>();

        CreateRadiusMesh();
    }

    private void CreateRadiusMesh()
    {
        //TODO: eventually change this to current characters position
        Vector3 origin = this.transform.position;

        tiles = EnvironmentUtil.GetTilesWithinDistance(origin, distance);

        //create a quad for each tile
        foreach (Vector3 tile in tiles)
        {
            GameObject quadObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
            quadObject.transform.parent = this.transform;
            quadObject.transform.localPosition = tile + new Vector3(0, 0.1f, 0);
            quadObject.transform.localEulerAngles = new Vector3(90, 0, 0);
            quadObject.transform.localScale = new Vector3(ENV_TILE_SIZE, ENV_TILE_SIZE, ENV_TILE_SIZE);

        }

        //grab all the meshes from the quads
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
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
        _filter.mesh = mesh;

        _outline = this.gameObject.AddComponent<HighlightEffect>();
        _outline.profile = OutlineSettings;
        _outline.ProfileLoad(OutlineSettings);
        _outline.highlighted = true;

        transform.gameObject.SetActive(true);
    }
}
