using UnityEditor;
using Pathfinding;

[CustomGraphEditor(typeof(TileGraph), "Tile Graph")]
public class TileGraphEditor : GraphEditor
{
    // Here goes the GUI
    public override void OnInspectorGUI(NavGraph target)
    {
        var graph = target as TileGraph;

        graph.width = EditorGUILayout.IntField("Width", graph.width);
    }
}