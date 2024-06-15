using CurveLib.Curves;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SplineSceneController : MonoBehaviour
{
    public bool dirty = true;

    public float tension = 0.5f;
    public List<LineRenderer> generatedCubic;
    public LineRenderer generatedSpline;
    public LineRenderer generatedSplineCatmullrom;
    public LineRenderer generatedSplineChordal;

    public Material red;
    public Material blue;
    public Material green;
    public Material yellow;

    private void OnEnable()
    {
        if (generatedCubic == null)
        {
            generatedCubic = new List<LineRenderer>();

        }

        if (generatedSpline == null)
        {
            var go = new GameObject("CentripetalSpline");
            generatedSpline = go.AddComponent<LineRenderer>();
        }

        if (generatedSplineChordal == null)
        {
            var go = new GameObject("ChordalSpline");
            generatedSplineChordal = go.AddComponent<LineRenderer>();
            generatedSplineChordal.startWidth = 0.2f;
            generatedSplineChordal.endWidth = 0.2f;
        }
        if (generatedSplineCatmullrom == null)
        {
            var go = new GameObject("CatmullromSpline");
            generatedSplineCatmullrom = go.AddComponent<LineRenderer>();
            generatedSplineCatmullrom.startWidth = 0.2f;
            generatedSplineCatmullrom.endWidth = 0.2f;
        }
        dirty = true;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (dirty)
        {
            var count = transform.childCount;
            var points = new Vector3[count];
            for (int i = 0; i < count; i++)
            {
                points[i] = transform.GetChild(i).position;
            }

            if (count > 4)
            {
                int countCubic = (count - 1) / 3;
                for (int i = 0; i < countCubic; i++)
                {
                    CubicBezierCurve cubicBezierCurve = new CubicBezierCurve(points[i * 3], points[i * 3 + 1], points[i * 3 + 2], points[i * 3 + 3]);
                    var len = cubicBezierCurve.GetLength();
                    var curvePoints = cubicBezierCurve.GetPoints((int)(len * 4));
                    Debug.Log($"divisions {curvePoints.Length}");

                    if (generatedCubic.Count < i + 1)
                    {
                        var go = new GameObject("CubicBezierCurve_" + i, typeof(LineRenderer));
                        LineRenderer line = go.GetComponent<LineRenderer>();
                        generatedCubic.Add(line);
                        line.startWidth = 0.2f;
                        line.endWidth = 0.2f;
                        line.material = blue;
                    }

                    generatedCubic[i].positionCount = curvePoints.Length;
                    generatedCubic[i].SetPositions(curvePoints);

                    
                }
            }

            {
                var curve1 = new SplineCurve(points, tension: tension);
                var len = curve1.GetLength();
                var ps = curve1.GetPoints((int)(len * 4));
                generatedSpline.positionCount = ps.Length;
                generatedSpline.SetPositions(ps);
            }

            {
                var curve3 = new SplineCurve(points, false, SplineType.Chordal, tension: tension);

                var len = curve3.GetLength();
                var ps = curve3.GetPoints((int)(len * 4));
                generatedSplineChordal.positionCount = ps.Length;
                generatedSplineChordal.SetPositions(ps);
            }
            {
                var curve2 = new SplineCurve(points, false, SplineType.Catmullrom, tension: tension);

                var len = curve2.GetLength();
                var ps = curve2.GetPoints((int)(len * 4));
                generatedSplineCatmullrom.positionCount = ps.Length;
                generatedSplineCatmullrom.SetPositions(ps);
            }

            dirty = false;
        }


    }
}
