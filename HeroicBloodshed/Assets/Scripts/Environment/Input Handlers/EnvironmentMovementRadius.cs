using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class EnvironmentMovementRadius : EnvironmentInputHandler
{
    [SerializeField] private GameObject Prefab_Tile;

    [SerializeField] private GameObject RadiusObject;

    private int _tilesLoaded;

    public override void Activate()
    {
        base.Activate();

        StartCoroutine(Coroutine_GenerateRadiusTiles());
    }

    public override void Deactivate()
    {
        base.Deactivate();
    }

    private IEnumerator Coroutine_GenerateRadiusTiles()
    {
        EnvironmentInputData inputData = EnvironmentManager.Instance.GetInputData();

        if(inputData.RadiusMap != null)
        {
            CharacterComponent currentCharacter = EncounterManager.Instance.GetCurrentCharacter();

            _tilesLoaded = 0;
            int tileCount = inputData.RadiusMap.Count;

            float startTime = Time.time;

            foreach (KeyValuePair<Vector3, int> pair in inputData.RadiusMap)
            {
                StartCoroutine(Coroutine_GenerateTile(pair.Key));
            }

            yield return new WaitWhile(() => _tilesLoaded < (tileCount-1));

            MeshFilter[] meshFilters = RadiusObject.GetComponentsInChildren<MeshFilter>();
            CombineInstance[] combine = new CombineInstance[meshFilters.Length];

            int i = 0;
            while (i < meshFilters.Length)
            {
                combine[i].mesh = meshFilters[i].mesh;
                combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
                meshFilters[i].gameObject.transform.gameObject.SetActive(false);

                i++;
            }

            Mesh mesh = new Mesh();
            mesh.CombineMeshes(combine, true, true);
            mesh.Optimize();
            UpdateMesh(mesh);

            Debug.Log("Combined " + i + " meshes to create radius in " + (Time.time-startTime) + " seconds");

            yield return null;
        }
    }

    private void UpdateMesh(Mesh mesh)
    {
        MeshFilter meshFilter = RadiusObject.GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        RadiusObject.SetActive(true);
    }

    private IEnumerator Coroutine_GenerateTile(Vector3 position)
    {
        GameObject tilePrefab = Instantiate(Prefab_Tile, RadiusObject.transform);

        tilePrefab.transform.position = position;

        _tilesLoaded++;

        yield return null;
    }
}
