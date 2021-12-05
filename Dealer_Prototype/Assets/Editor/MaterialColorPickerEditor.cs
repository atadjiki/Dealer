using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(MaterialColorPicker))]
public class MaterialColorPickerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MaterialColorPicker materialColorPicker = (MaterialColorPicker)target;

        materialColorPicker.UpdateColor();
    }
}