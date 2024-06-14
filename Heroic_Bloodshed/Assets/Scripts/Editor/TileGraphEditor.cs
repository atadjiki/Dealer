using UnityEditor;
using Pathfinding;
using UnityEngine;
using static Constants;

[CustomGraphEditor(typeof(TileGraph), "Tile Graph")]
public class TileGraphEditor : GraphEditor
{
    // Here goes the GUI
    public override void OnInspectorGUI(NavGraph target)
    {
        var graph = target as TileGraph;

        EditorGUILayout.PrefixLabel("Grid Settings:");
        EditorGUILayout.Space();

        graph.Width = EditorGUILayout.IntField("Width", graph.Width);

        EditorGUILayout.PrefixLabel("Gizmo Settings:");
        EditorGUILayout.Space();
        graph.ShowConnections = EditorGUILayout.Toggle("Show Connections", graph.ShowConnections);
        graph.ShowLayers = EditorGUILayout.Toggle("Show Layers", graph.ShowLayers);
        graph.ShowCover = EditorGUILayout.Toggle("Show Cover", graph.ShowCover);
        EditorGUILayout.Space();
        graph.LayerOpacity = EditorGUILayout.FloatField("Layer Opacity", graph.LayerOpacity);
        EditorGUILayout.Space();
        graph.CoverOpacity = EditorGUILayout.FloatField("Cover Opacity", graph.CoverOpacity);
        graph.LineWidth = EditorGUILayout.FloatField("Line Width", graph.LineWidth);
        EditorGUILayout.Space();
        graph.AllowedMovementTypes = (MovementPathType)EditorGUILayout.EnumFlagsField("Allowed Movement Types", graph.AllowedMovementTypes);
    }
}