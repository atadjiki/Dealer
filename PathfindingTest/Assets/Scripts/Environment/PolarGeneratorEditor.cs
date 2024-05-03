using UnityEditor;
using Pathfinding;

[CustomGraphEditor(typeof(PolarGraph), "Polar Graph")]
public class PolarGeneratorEditor : GraphEditor
{
    // Here goes the GUI
    public override void OnInspectorGUI(NavGraph target)
    {
        var graph = target as PolarGraph;

        graph.circles = EditorGUILayout.IntField("Circles", graph.circles);
        graph.steps = EditorGUILayout.IntField("Steps", graph.steps);
        graph.scale = EditorGUILayout.FloatField("Scale", graph.scale);
        graph.center = EditorGUILayout.Vector3Field("Center", graph.center);
    }
}