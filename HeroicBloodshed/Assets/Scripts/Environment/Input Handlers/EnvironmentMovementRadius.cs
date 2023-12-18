using EPOOutline;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class EnvironmentMovementRadius : EnvironmentInputHandler
{
    [SerializeField] private Material TileMaterial;

    public override void Activate()
    {
        base.Activate();

        StartCoroutine(Coroutine_GenerateRadiusTiles());
    }

    public override void Deactivate()
    {
        base.Deactivate();

        for(int i = 0; i < this.transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    private IEnumerator Coroutine_GenerateRadiusTiles()
    {
        EnvironmentInputData inputData = EnvironmentManager.Instance.GetInputData();

        Dictionary<MovementRangeType, List<Vector3>> borderMap = new Dictionary<MovementRangeType, List<Vector3>>();

        CharacterNavigator characterNavigator = EncounterManager.Instance.GetCurrentCharacter().GetNavigator();

        //generate our border nodes for reach movement range type
        foreach (KeyValuePair<MovementRangeType, Dictionary<Vector3, int>> KeyPair in inputData.RadiusMaps)
        {
            MovementRangeType rangeType = KeyPair.Key;
            Dictionary<Vector3, int> radiusMap = KeyPair.Value;

            Color rangeColor = GetColor(rangeType);
           // rangeColor.a = 0.2f;

            borderMap.Add(rangeType, new List<Vector3>());

            if (radiusMap != null)
            {
                float startTime = Time.time;

                GameObject RadiusObject = new GameObject(GetDisplayString(rangeType));
                RadiusObject.transform.parent = this.transform;

                //gather a list of every corner vertex
                List<Vector3> corners = new List<Vector3>();
                foreach (KeyValuePair<Vector3, int> pair in radiusMap)
                {
                    corners.AddRange(CalculateCorners(pair.Key));
                }

                //for each corner, count how many times it occurs, and extract border nodes
                foreach (Vector3 node in corners)
                {
                    int occurences = 0;

                    foreach (Vector3 comparison in corners)
                    {
                        float distance = Vector3.Distance(node, comparison);

                        if (distance < 0.01f)
                        {
                            occurences++;
                        }
                    }

                    if (occurences <= 2)
                    {
                        borderMap[rangeType].Add(node);
                    }
                }

                List<Vector3> allowed = new List<Vector3>();

                int count = 0;
                foreach (Vector3 node in borderMap[rangeType])
                {
                    bool allow = true;
                    if (rangeType == MovementRangeType.Full)
                    {
                        foreach (Vector3 comparison in borderMap[MovementRangeType.Half])
                        {
                            float distance = Vector3.Distance(node, comparison);

                            if (distance < 0.01f)
                            {
                                allow = false;
                                break;
                            }
                        }
                    }

                    Vector3 point = RadiusObject.transform.InverseTransformPoint(node);

                    if (allow && !allowed.Contains(point))
                    {
                        allowed.Add(point);
                        count++;
                    }
                }

               // allowed = allowed.OrderBy(point => Mathf.Atan2(point.z - allowed[0].z, point.x - allowed[0].x)).ToList();

                foreach (Vector3 node in inputData.RadiusMaps[rangeType].Keys)
                {
                    GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
                    quad.name = node.ToString();

                    Destroy(quad.GetComponent<Collider>());
                    quad.transform.parent = RadiusObject.transform;
                    quad.transform.localPosition = node;
                    quad.transform.localPosition += new Vector3(0, 0.15f, 0);
                    quad.transform.localEulerAngles = new Vector3(90, 0, 0);
                    quad.transform.localScale = GetTileScaleVector();

                    MeshRenderer meshRenderer = quad.GetComponent<MeshRenderer>();
                    meshRenderer.material = TileMaterial;
                    meshRenderer.material.color = rangeColor;

                    //Outlinable outline = quad.AddComponent<Outlinable>();
                    //outline.TryAddTarget(new OutlineTarget(meshRenderer));
                    //outline.OutlineParameters.Color = Color.white;
                    //outline.OutlineParameters.BlurShift = 0.5f;
                    //outline.OutlineParameters.DilateShift = 0.5f;
                    //outline.OutlineLayer = (int)rangeType;
                  //  meshRenderer.SetMaterials(new List<Material>());
                }

                Debug.Log("created " + count + " quads to create radius in " + (Time.time - startTime) + " seconds");

                yield return null;
            }
        }
    }

    private Vector3[] CalculateCorners(Vector3 center)
    {
        float radius = TILE_SIZE / 2;

        return new Vector3[]
        {
            center + new Vector3(radius, 0, radius),
            center + new Vector3(-radius, 0, radius),
            center + new Vector3(radius, 0, -radius),
            center + new Vector3(-radius, 0, -radius),

        };
    }
}
