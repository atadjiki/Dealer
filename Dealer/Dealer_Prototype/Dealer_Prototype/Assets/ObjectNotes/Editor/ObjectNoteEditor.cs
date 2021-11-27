using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(ObjectNote)), CanEditMultipleObjects]
public class ObjectNoteEditor : Editor
{
    ObjectNote note;

    [DrawGizmo(GizmoType.InSelectionHierarchy)]
    static void DrawSelectedNote(ObjectNote note, GizmoType gizmoType)
    {
        DrawObjectNote(note, gizmoType, true);
    }
    [DrawGizmo(GizmoType.NotInSelectionHierarchy)]
    static void DrawUnselectedNote(ObjectNote note, GizmoType gizmoType)
    {
        DrawObjectNote(note, gizmoType, false);
    }

    static void DrawObjectNote(ObjectNote onote, GizmoType gizmoType, bool selected)
    {
        Transform transform = onote.transform;

        if (onote != null && onote.enabled)
        {
            if (onote.Style == null || onote.Size.x == 0f) {
                onote.SetStyle(false);
            }

            if ((selected && onote.ShowWhenSelected) || (!selected && onote.ShowWhenUnselected))
            {
                Handles.BeginGUI();
                GUI.backgroundColor = onote.Color;
                Vector2 guiPosition = HandleUtility.WorldToGUIPoint(transform.position - onote.AnchorOffset);
                guiPosition += onote.PixelOffset;
                if (!string.IsNullOrEmpty(onote.NoteText))
                {
                    GUI.DrawTexture(new Rect(guiPosition.x - onote.Size.x / 2, guiPosition.y - onote.Size.y / 2, onote.Size.x, onote.Size.y), onote.ColorTex, ScaleMode.StretchToFill);
                    GUI.Label(new Rect(guiPosition.x - onote.Size.x / 2, guiPosition.y - onote.Size.y / 2, onote.Size.x, onote.Size.y), onote.NoteText, onote.Style);
                }
                guiPosition = HandleUtility.WorldToGUIPoint(transform.position);
                GUI.backgroundColor = Color.white;
                if(onote.Image != null) GUI.Label(new Rect(guiPosition.x - onote.Image.width * onote.ImageScale.x / 2 + onote.ImageOffset.x, guiPosition.y - onote.Image.height * onote.ImageScale.y / 2 + onote.ImageOffset.y, onote.Image.width * onote.ImageScale.x, onote.Image.height * onote.ImageScale.y), "", onote.ImgStyle);
                Handles.EndGUI();
            }
        }
    }

    public void OnEnable()
    {
        note = (ObjectNote)target;
        if (note != null && note.IsNew) {
            note.NoteText = target.name;
            if (!PrefabUtility.IsAddedComponentOverride(note))
            {
                int comps = note.gameObject.GetComponents<Component>().Length;
                for (int i = 0; i < comps; i++)
                {
                    UnityEditorInternal.ComponentUtility.MoveComponentUp(note);
                }
            }
            note.IsNew = false;
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        bool changed = false;
        if (note != null)
        {
            GUILayout.BeginVertical();
            if (note is ObjectNoteInGame)
            {
                EditorGUILayout.HelpBox("This component uses simple GUI calls and is not recommended for use in final game builds, but rather for debugging and testing purposes.", MessageType.Warning);
            }
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Text"));
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(note, "Changed Object Note");
                changed = true;
            }
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Image"));
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(note, "Changed Object Note Image");
                changed = true;
            }
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("ImageScale"));
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(note, "Changed Object Note Image Scale");
                changed = true;
            }
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("ImageOffset"));
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(note, "Changed Object Note Image Offset");
                changed = true;
            }
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("ShowWhenSelected"));
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(note, "Changed Object Note Visibility");
                changed = true;
            }
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("ShowWhenUnselected"));
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(note, "Changed Object Note Unselected Visibility");
                changed = true;
            }
            if (note is ObjectNoteInGame) {
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(serializedObject.FindProperty("ShowInGameEditor"));
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(note, "Changed Object Note In Game Editor Visibility");
                    changed = true;
                }
                if(!serializedObject.FindProperty("ShowInGameEditor").boolValue) {
                    EditorGUILayout.HelpBox("Does still show in built game!", MessageType.Warning);
                }
            }
            else {
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(serializedObject.FindProperty("ShowInGameEditor"));
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(note, "Changed Object Note Game View Visibility");
                    changed = true;
                }
                if (serializedObject.FindProperty("ShowInGameEditor").boolValue)
                {
                    EditorGUILayout.HelpBox("Does not show in built game!", MessageType.Warning);
                }
            }
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("AnchorOffset"));
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(note, "Changed Object Note Anchor Offset");
                changed = true;
            }
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("PixelOffset"));
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(note, "Changed Object Note Pixel Offset");
                changed = true;
            }
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Color"));
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(note, "Changed Object Note Color");
                changed = true;
            }
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("FontSize"));
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(note, "Changed Object Note Font Size");
                changed = true;
            }
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Bold"));
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(note, "Changed Object Note Bold text");
                changed = true;
            }
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Alignment"));
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(note, "Changed Object Note Alignment");
                changed = true;
            }
            GUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }

        if(changed)
        {
            foreach (ObjectNote item in targets) item.SetStyle(false);
            SceneView.RepaintAll();
            UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
        }
    }
}
