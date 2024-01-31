using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class IllustrateWizard : EditorWindow
{

    public Texture titleWindow;
    public Shader illustrateShader;
    public Shader illustrateSimpleShader;
    public Shader illustrateTransparentShader;
    public Shader illustrateAlphaClipShader;
    public Shader illustrateOutlineShader;
    public Material handDrawn;
    public Material toonRamp;
    public Material unlit;
    public Material comicBook;
    public Material soft;
    public Material metallic;
    public Material psychedelic;
    public Material materialPreset;
    Vector2 scrollPos;

    [SerializeField]
    Material[] materialsToChange;
    SerializedObject so;
    [IllustrateProperty(UnityEngine.Rendering.ShaderPropertyType.Color)]
    public string color;
    [IllustrateProperty(UnityEngine.Rendering.ShaderPropertyType.Texture)]
    public string texture;
    [IllustrateProperty(UnityEngine.Rendering.ShaderPropertyType.Texture)]
    public string normalMap;
    public enum Style { handDrawn, toonRamp, unlit, comicBook, soft, metallic, psychedelic, custom };
    public Style style;
    public enum ShaderType { opaque, simpleOpaque, alphaClipped, transparent, traditionalOutline };
    public ShaderType shaderType;


    SerializedProperty mats;

    [MenuItem("Distant Lands/Illustrate/Material Conversion Wizard", false, 0)]
    static void Init()
    {

        IllustrateWizard window = (IllustrateWizard)EditorWindow.GetWindow(typeof(IllustrateWizard), false, "Illustrate Wizard");
        window.minSize = new Vector2(400, 500);
        window.Show();

    }
    void OnEnable()
    {
        ScriptableObject target = this;
        so = new SerializedObject(target);
        mats = so.FindProperty("materialsToChange");

    }

    private void OnGUI()
    {



        GUI.DrawTexture(new Rect(0, 0, position.width, position.width * 1 / 4), titleWindow);
        EditorGUILayout.Space(position.width * 1 / 4);
        EditorGUILayout.Separator();
        EditorGUI.indentLevel = 1;

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        so.Update();
        EditorGUILayout.LabelField("Materials");
        EditorGUILayout.PropertyField(mats);
        CheckMaterials();

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(so.FindProperty("color"));
        EditorGUILayout.PropertyField(so.FindProperty("texture"));
        EditorGUILayout.PropertyField(so.FindProperty("normalMap"));
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(so.FindProperty("style"));
        if (style == Style.custom)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(so.FindProperty("materialPreset"));
            EditorGUI.indentLevel--;
        }
        EditorGUILayout.PropertyField(so.FindProperty("shaderType"));


        EditorGUILayout.EndScrollView();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Convert Materials"))
            ConvertMaterials();
        so.ApplyModifiedProperties();

    }

    private void ConvertMaterials()
    {
        if (EditorUtility.DisplayDialog("Convert Materials?", "This operation cannot be undone.", "Yes", "No"))
        {
            foreach (Material mat in materialsToChange)
            {
                Color col = Color.white;
                Texture mainTex = null;
                Texture normals = null;
                if (color != "" && color != "None")
                    col = mat.GetColor(color);
                if (texture != "" && texture != "None")
                    mainTex = mat.GetTexture(texture);
                if (normalMap != "" && normalMap != "None")
                    normals = mat.GetTexture(normalMap);

                switch (shaderType)
                {
                    case (ShaderType.opaque):
                        mat.shader = illustrateShader;
                        break;
                    case (ShaderType.simpleOpaque):
                        mat.shader = illustrateSimpleShader;
                        break;
                    case (ShaderType.alphaClipped):
                        mat.shader = illustrateAlphaClipShader;
                        break;
                    case (ShaderType.transparent):
                        mat.shader = illustrateTransparentShader;
                        break;
                    case (ShaderType.traditionalOutline):
                        mat.shader = illustrateOutlineShader;
                        break;
                }


                switch (style)
                {
                    case (Style.comicBook):
                        mat.CopyPropertiesFromMaterial(comicBook);
                        break;
                    case (Style.handDrawn):
                        mat.CopyPropertiesFromMaterial(handDrawn);
                        break;
                    case (Style.metallic):
                        mat.CopyPropertiesFromMaterial(metallic);
                        break;
                    case (Style.soft):
                        mat.CopyPropertiesFromMaterial(soft);
                        break;
                    case (Style.toonRamp):
                        mat.CopyPropertiesFromMaterial(toonRamp);
                        break;
                    case (Style.unlit):
                        mat.CopyPropertiesFromMaterial(unlit);
                        break;
                    case (Style.psychedelic):
                        mat.CopyPropertiesFromMaterial(psychedelic);
                        break;
                    case (Style.custom):
                        mat.CopyPropertiesFromMaterial(materialPreset);
                        break;
                }

                mat.SetColor("_MainColor", col);
                mat.SetTexture("_Texture", mainTex);
                mat.SetTexture("_NormalMap", normals);

            }
        }
    }

    private void CheckMaterials()
    {

        if (mats.arraySize == 0 || mats.GetArrayElementAtIndex(0).objectReferenceValue == null)
            return;

        Shader shader = (mats.GetArrayElementAtIndex(0).objectReferenceValue as Material).shader;

        for (int i = 1; i < mats.arraySize; i++)
        {

            if (mats.GetArrayElementAtIndex(i).objectReferenceValue != null && (mats.GetArrayElementAtIndex(i).objectReferenceValue as Material).shader != shader)
            {
                EditorGUILayout.HelpBox($"WARNING: {mats.GetArrayElementAtIndex(i).objectReferenceValue.name} does not have the same shader as other materials in this list. The conversion process may not work properly for all materials if you proceed. \nFor best results, please convert different materials at different times.", MessageType.Warning);
            }
        }
    }

}

public class IllustratePropertyAttribute : PropertyAttribute
{

    public UnityEngine.Rendering.ShaderPropertyType propertyType;


    public IllustratePropertyAttribute(UnityEngine.Rendering.ShaderPropertyType _propertyType)
    {
        propertyType = _propertyType;
    }
}

[UnityEditor.CustomPropertyDrawer(typeof(IllustratePropertyAttribute))]
public class IllustratePropertyDrawer : PropertyDrawer
{


    IllustratePropertyAttribute _attribute;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {

        _attribute = (IllustratePropertyAttribute)attribute;

        List<string> names = new List<string>() { "None" };
        int selected = -1;
        SerializedProperty materials = property.serializedObject.FindProperty("materialsToChange");

        if (materials.arraySize == 0 || materials.GetArrayElementAtIndex(0).objectReferenceValue == null)
        {
            DrawStandardProperty(position, property, label);
            return;
        }

        Shader shader = (materials.GetArrayElementAtIndex(0).objectReferenceValue as Material).shader;

        EditorGUI.BeginProperty(position, label, property);
        for (int i = 0; i < shader.GetPropertyCount(); i++)
            if (shader.GetPropertyType(i) == _attribute.propertyType)
                names.Add(shader.GetPropertyName(i));


        if (names.Contains(property.stringValue))
            selected = names.IndexOf(property.stringValue);
        else
            selected = 0;

        int propName = EditorGUI.Popup(position, label.text, selected, names.ToArray());

        property.stringValue = propName < names.Count ? names[propName] : "";


        EditorGUI.EndProperty();
    }

    public void DrawStandardProperty(Rect position, SerializedProperty property, GUIContent label)
    {

        EditorGUI.BeginProperty(position, label, property);


        EditorGUI.PropertyField(position, property, label);

        EditorGUI.EndProperty();

    }
}
