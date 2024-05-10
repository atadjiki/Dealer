using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Haze;
using static Constants;

public class EnvironmentMovementRadius : MonoBehaviour
{
    private int distance = 12;
    private List<Vector3> tiles;
    private LineRenderer LineRenderer;

    private void Awake()
    {
        Vector3 origin = this.transform.position;

        tiles = EnvironmentUtil.GetTilesWithinDistance(origin, distance);

        //create a quad for each tile
        foreach(Vector3 tile in tiles)
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
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);

            i++;
        }

        Mesh mesh = new Mesh();
        mesh.CombineMeshes(combine);
        transform.GetComponent<MeshFilter>().mesh = mesh;
        transform.gameObject.SetActive(true);
    }

    private void OnDrawGizmosSelected()
    {
        if(Application.isPlaying)
        {
            foreach (Vector3 tile in tiles)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawCube(tile, GetTileSize());
            }
        }

    }
}
