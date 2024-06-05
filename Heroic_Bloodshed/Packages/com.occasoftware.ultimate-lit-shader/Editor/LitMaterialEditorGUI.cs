using UnityEngine;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine.Rendering;
using System;

namespace OccaSoftware.UltimateLitShader.Editor
{
    public class LitMaterialEditorGUI : ShaderGUI
    {
        public static readonly string[] _CullOptions = new string[] { "Both", "Back", "Front" };
        public static readonly int[] _CullValues = new int[] { 0, 1, 2 };

        public static readonly string[] _SurfaceOptions = new string[] { "Opaque", "Transparent" };
        public static readonly int[] _SurfaceValues = new int[] { 0, 1 };

        bool showSurfaceOptions = true;
        bool showSubsurfaceInputs = true;
        bool showSurfaceInputs = true;
        bool showSurfaceInputs2 = true;
        bool showAdvancedOptions = true;

        Material t;

        public enum AlphaOptions
        {
            Alpha,
            Premultiply,
            Additive,
            Multiply
        }

        private AlphaOptions GetAlphaBlendMode(MaterialProperty blend) => (AlphaOptions)blend.floatValue;

        MaterialEditor e;

        public override void OnGUI(MaterialEditor e, MaterialProperty[] properties)
        {
            this.e = e;
            t = e.target as Material;

            /////////////////////////////
            // Material 1              //
            /////////////////////////////
            MaterialProperty _MainTex = FindProperty("_MainTex", properties);
            MaterialProperty _BaseColor = FindProperty("_BaseColor", properties);

            MaterialProperty _MetalnessMap = FindProperty("_MetalnessMap", properties);
            MaterialProperty _Metalness = FindProperty("_Metalness", properties);
            MaterialProperty _MetalnessMapExposure = FindProperty("_MetalnessMapExposure", properties);
            MaterialProperty _HasMetalnessMap = FindProperty("_HasMetalnessMap", properties);

            MaterialProperty _HeightMap = FindProperty("_HeightMap", properties);
            MaterialProperty _HeightStrength = FindProperty("_HeightStrength", properties);
            MaterialProperty _HasHeightMap = FindProperty("_HasHeightMap", properties);

            MaterialProperty _RoughnessMap = FindProperty("_RoughnessMap", properties);
            MaterialProperty _Roughness = FindProperty("_Roughness", properties);
            MaterialProperty _RoughnessMapExposure = FindProperty("_RoughnessMapExposure", properties);
            MaterialProperty _HasRoughnessMap = FindProperty("_HasRoughnessMap", properties);

            MaterialProperty _NormalMap = FindProperty("_NormalMap", properties);
            MaterialProperty _NormalStrength = FindProperty("_NormalStrength", properties);
            MaterialProperty _HasNormalMap = FindProperty("_HasNormalMap", properties);

            MaterialProperty _Specularity = FindProperty("_Specularity", properties);

            MaterialProperty _OcclusionMap = FindProperty("_OcclusionMap", properties);
            MaterialProperty _OcclusionStrength = FindProperty("_OcclusionStrength", properties);

            MaterialProperty _EmissionMap = FindProperty("_EmissionMap", properties);
            MaterialProperty _Emission = FindProperty("_Emission", properties);
            MaterialProperty _HasEmissionMap = FindProperty("_HasEmissionMap", properties);

            /////////////////////////////
            // Material 2              //
            /////////////////////////////
            MaterialProperty _MainTex2 = FindProperty("_MainTex2", properties);
            MaterialProperty _BaseColor2 = FindProperty("_BaseColor2", properties);

            MaterialProperty _MetalnessMap2 = FindProperty("_MetalnessMap2", properties);
            MaterialProperty _Metalness2 = FindProperty("_Metalness2", properties);
            MaterialProperty _MetalnessMapExposure2 = FindProperty("_MetalnessMapExposure2", properties);
            MaterialProperty _HasMetalnessMap2 = FindProperty("_HasMetalnessMap2", properties);

            MaterialProperty _HeightMap2 = FindProperty("_HeightMap2", properties);
            MaterialProperty _HeightStrength2 = FindProperty("_HeightStrength2", properties);
            MaterialProperty _HasHeightMap2 = FindProperty("_HasHeightMap2", properties);

            MaterialProperty _RoughnessMap2 = FindProperty("_RoughnessMap2", properties);
            MaterialProperty _Roughness2 = FindProperty("_Roughness2", properties);
            MaterialProperty _RoughnessMapExposure2 = FindProperty("_RoughnessMapExposure2", properties);
            MaterialProperty _HasRoughnessMap2 = FindProperty("_HasRoughnessMap2", properties);

            MaterialProperty _NormalMap2 = FindProperty("_NormalMap2", properties);
            MaterialProperty _NormalStrength2 = FindProperty("_NormalStrength2", properties);
            MaterialProperty _HasNormalMap2 = FindProperty("_HasNormalMap2", properties);

            MaterialProperty _Specularity2 = FindProperty("_Specularity2", properties);

            MaterialProperty _OcclusionMap2 = FindProperty("_OcclusionMap2", properties);
            MaterialProperty _OcclusionStrength2 = FindProperty("_OcclusionStrength2", properties);

            MaterialProperty _EmissionMap2 = FindProperty("_EmissionMap2", properties);
            MaterialProperty _Emission2 = FindProperty("_Emission2", properties);
            MaterialProperty _HasEmissionMap2 = FindProperty("_HasEmissionMap2", properties);

            /////////////////////////////
            // Shared Material         //
            /////////////////////////////
            MaterialProperty _SubsurfaceEnabled = FindProperty("_SubsurfaceEnabled", properties);
            MaterialProperty _SubsurfaceThicknessMap = FindProperty("_SubsurfaceThicknessMap", properties);
            MaterialProperty _SubsurfaceThickness = FindProperty("_SubsurfaceThickness", properties);
            MaterialProperty _SubsurfaceFalloff = FindProperty("_SubsurfaceFalloff", properties);
            MaterialProperty _SubsurfaceColor = FindProperty("_SubsurfaceColor", properties);
            MaterialProperty _SubsurfaceDistortion = FindProperty("_SubsurfaceDistortion", properties);
            MaterialProperty _HasSubsurfaceMap = FindProperty("_HasSubsurfaceMap", properties);
            MaterialProperty _SubsurfaceAmbient = FindProperty("_SubsurfaceAmbient", properties);

            MaterialProperty _Culling = FindProperty("_Culling", properties);
            MaterialProperty _ReceiveShadowsEnabled = FindProperty("_ReceiveShadowsEnabled", properties);

            MaterialProperty _ReceiveFogEnabled = FindProperty("_ReceiveFogEnabled", properties);

            MaterialProperty _Surface = FindProperty("_Surface", properties);
            MaterialProperty _AlphaClip = FindProperty("_AlphaClip", properties);
            MaterialProperty _AlphaClipEnabled = FindProperty("_AlphaClipEnabled", properties);
            MaterialProperty _Blend = FindProperty("_Blend", properties);

            MaterialProperty _SortPriority = FindProperty("_SortPriority", properties);

            MaterialProperty _UseVertexColors = FindProperty("_UseVertexColors", properties);

            DrawSurfaceOptions();
            DrawSurfaceInputs();
            DrawSurfaceInputs2();
            DrawSubsurfaceInputs();
            DrawAdvancedOptions();

            if (EditorGUILayout.LinkButton("Docs"))
            {
                Application.OpenURL("https://docs.occasoftware.com/ultimate-lit-shader");
            }

            bool IsTransparent() => _Surface.floatValue > 0.0f ? true : false;
            bool AlphaClipEnabled() => _AlphaClipEnabled.floatValue > 0.0f ? true : false;

            void DrawSurfaceOptions()
            {
                showSurfaceOptions = EditorGUILayout.BeginFoldoutHeaderGroup(showSurfaceOptions, "Surface Options");
                if (showSurfaceOptions)
                {
                    EditorGUI.indentLevel++;
                    e.IntPopupShaderProperty(_Surface, "Surface", _SurfaceOptions, _SurfaceValues);

                    t.DisableKeyword("_ALPHAPREMULTIPLY_ON");

                    bool depthWrite = true;
                    if (IsTransparent())
                    {
                        t.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
                        t.SetOverrideTag("RenderType", "Transparent");
                        depthWrite = false;

                        DrawEnumProperty(GetAlphaBlendMode(_Blend), _Blend, new GUIContent("Blend"));

                        switch (GetAlphaBlendMode(_Blend))
                        {
                            case AlphaOptions.Alpha:
                                SetSrcDestProperties(t, BlendMode.SrcAlpha, BlendMode.OneMinusSrcAlpha);
                                break;
                            case AlphaOptions.Premultiply:
                                SetSrcDestProperties(t, BlendMode.One, BlendMode.OneMinusSrcAlpha);
                                t.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                                break;
                            case AlphaOptions.Additive:
                                SetSrcDestProperties(t, BlendMode.One, BlendMode.One);
                                break;
                            case AlphaOptions.Multiply:
                                SetSrcDestProperties(t, BlendMode.DstColor, BlendMode.Zero);
                                break;
                        }
                    }
                    else
                    {
                        t.DisableKeyword("_SURFACE_TYPE_TRANSPARENT");
                        t.SetOverrideTag("RenderType", "Opaque");
                        SetSrcDestProperties(t, BlendMode.One, BlendMode.Zero);
                    }

                    e.IntPopupShaderProperty(_Culling, "Render Face", _CullOptions, _CullValues);

                    e.ShaderProperty(_AlphaClipEnabled, new GUIContent("Alpha Clip"));
                    if (AlphaClipEnabled())
                    {
                        EditorGUI.indentLevel++;
                        e.ShaderProperty(_AlphaClip, new GUIContent("Threshold"));
                        EditorGUI.indentLevel--;
                    }

                    if (AlphaClipEnabled())
                    {
                        t.EnableKeyword("_ALPHATEST_ON");
                    }
                    else
                    {
                        t.DisableKeyword("_ALPHATEST_ON");
                    }

                    if (IsTransparent())
                    {
                        t.renderQueue = (int)RenderQueue.Transparent + _SortPriority.intValue;
                    }
                    else
                    {
                        if (AlphaClipEnabled())
                        {
                            t.renderQueue = (int)RenderQueue.AlphaTest + _SortPriority.intValue;
                        }
                        else
                        {
                            t.renderQueue = (int)RenderQueue.Geometry + _SortPriority.intValue;
                        }
                    }

                    SetupDepthWriting(t, depthWrite);

                    DrawToggleProperty(
                        _ReceiveShadowsEnabled,
                        new GUIContent(
                            "Receive Shadows",
                            "A setting that determines whether or not an object will receive shadows from other objects in the scene. When enabled, the object will appear to receive shadows, adding depth and realism to the scene."
                        )
                    );
                    EditorGUI.indentLevel--;
                    EditorGUILayout.Space();
                }

                EditorGUILayout.EndFoldoutHeaderGroup();
            }

            void DrawAdvancedOptions()
            {
                showAdvancedOptions = EditorGUILayout.BeginFoldoutHeaderGroup(showAdvancedOptions, "Advanced Inputs");
                if (showAdvancedOptions)
                {
                    EditorGUI.indentLevel++;
                    e.ShaderProperty(_ReceiveFogEnabled, new GUIContent("Receive Fog"));
                    e.ShaderProperty(_SortPriority, new GUIContent("Sort Priority"));
                    EditorGUI.indentLevel--;
                    EditorGUILayout.Space();
                }
                EditorGUILayout.EndFoldoutHeaderGroup();
            }

            void DrawSubsurfaceInputs()
            {
                showSubsurfaceInputs = EditorGUILayout.BeginFoldoutHeaderGroup(showSubsurfaceInputs, "Subsurface Inputs");
                if (showSubsurfaceInputs)
                {
                    EditorGUI.indentLevel++;
                    e.ShaderProperty(_SubsurfaceEnabled, "Enabled");
                    if (_SubsurfaceEnabled.floatValue > 0)
                    {
                        EditorGUI.BeginChangeCheck();
                        e.TexturePropertySingleLine(
                            new GUIContent("Thickness", "A property that defines the subsurface thickness of the surface."),
                            _SubsurfaceThicknessMap,
                            _SubsurfaceThickness
                        );
                        if (EditorGUI.EndChangeCheck())
                        {
                            _HasSubsurfaceMap.floatValue = (_SubsurfaceThicknessMap.textureValue == null ? 0 : 1);
                        }

                        e.ShaderProperty(_SubsurfaceColor, "Color");
                        e.ShaderProperty(_SubsurfaceDistortion, "Distortion");
                        e.ShaderProperty(_SubsurfaceFalloff, "Falloff");
                        e.ShaderProperty(_SubsurfaceAmbient, "Ambient");
                    }

                    EditorGUI.indentLevel--;
                }

                EditorGUILayout.EndFoldoutHeaderGroup();
            }

            void DrawSurfaceInputs()
            {
                showSurfaceInputs = EditorGUILayout.BeginFoldoutHeaderGroup(showSurfaceInputs, "Surface Inputs");
                if (showSurfaceInputs)
                {
                    EditorGUI.indentLevel++;
                    TexturePropertyWithColor(
                        new GUIContent(
                            "Base Color",
                            "A property that defines its overall color. It serves as a starting point for calculating the final color of the surface after taking into account lighting, shadows, reflections, and other effects."
                        ),
                        _MainTex,
                        _BaseColor,
                        true,
                        false
                    );

                    // Metalness
                    EditorGUI.BeginChangeCheck();
                    MaterialProperty p = _MetalnessMap.textureValue == null ? _Metalness : null;
                    e.TexturePropertySingleLine(
                        new GUIContent(
                            "Metalness",
                            "A property that defines how metallic it appears. It affects the way light interacts with the surface, creating the illusion of materials like metal, chrome, or gold, as well as non-metallic materials like plastic or glass."
                        ),
                        _MetalnessMap,
                        p
                    );
                    if (_MetalnessMap.textureValue != null)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUI.indentLevel++;
                        e.DefaultShaderProperty(_MetalnessMapExposure, "Exposure");
                        EditorGUI.indentLevel--;
                        EditorGUI.indentLevel--;
                    }
                    if (EditorGUI.EndChangeCheck())
                    {
                        _HasMetalnessMap.floatValue = (_MetalnessMap.textureValue == null ? 0 : 1);
                    }

                    // Roughness
                    EditorGUI.BeginChangeCheck();
                    p = _RoughnessMap.textureValue == null ? _Roughness : null;
                    e.TexturePropertySingleLine(
                        new GUIContent(
                            "Roughness",
                            "A property that defines how rough or smooth it appears. It affects the way light scatters and spreads across the surface, creating the illusion of materials like matte paint, sandpaper, or satin."
                        ),
                        _RoughnessMap,
                        p
                    );
                    if (_RoughnessMap.textureValue != null)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUI.indentLevel++;
                        e.DefaultShaderProperty(_RoughnessMapExposure, "Exposure");
                        EditorGUI.indentLevel--;
                        EditorGUI.indentLevel--;
                    }
                    if (EditorGUI.EndChangeCheck())
                    {
                        _HasRoughnessMap.floatValue = (_RoughnessMap.textureValue == null ? 0 : 1);
                    }

                    e.ShaderProperty(
                        _Specularity,
                        new GUIContent(
                            "Specularity",
                            "Refers to the glossy reflections on shiny surfaces. More specular materials are more reflective and create brighter highlights."
                        )
                    );

                    EditorGUI.BeginChangeCheck();
                    DrawConditionalTextureProperty(
                        new GUIContent(
                            "Normal",
                            "A property that defines the direction the surface is facing. It affects the way light interacts with the surface, creating the illusion of bumps, ridges, and other surface details, as well as the appearance of shadows and reflections."
                        ),
                        _NormalMap,
                        _NormalStrength
                    );
                    if (EditorGUI.EndChangeCheck())
                    {
                        _HasNormalMap.floatValue = (_NormalMap.textureValue == null ? 0 : 1);
                    }

                    EditorGUI.BeginChangeCheck();
                    DrawConditionalTextureProperty(
                        new GUIContent("Height", "A property that defines the displacment of the surface."),
                        _HeightMap,
                        _HeightStrength
                    );
                    if (EditorGUI.EndChangeCheck())
                    {
                        _HasHeightMap.floatValue = (_HeightMap.textureValue == null ? 0 : 1);
                    }

                    DrawConditionalTextureProperty(
                        new GUIContent(
                            "Occlusion",
                            "A property that defines how much of a surface is blocked from receiving light. It affects the appearance of shadows and the overall brightness of a surface, creating a more realistic and visually appealing environment."
                        ),
                        _OcclusionMap,
                        _OcclusionStrength
                    );

                    EditorGUI.BeginChangeCheck();
                    TexturePropertyWithColor(
                        new GUIContent(
                            "Emission",
                            "A property of an object that defines how much light it emits on its own, independent of external light sources. It creates the illusion of glowing objects and surfaces, such as light bulbs, lamps, or neon signs."
                        ),
                        _EmissionMap,
                        _Emission,
                        false,
                        true
                    );
                    if (EditorGUI.EndChangeCheck())
                    {
                        _HasEmissionMap.floatValue = (_EmissionMap.textureValue == null ? 0 : 1);
                    }

                    e.TextureScaleOffsetProperty(_MainTex);
                    EditorGUI.indentLevel--;
                    EditorGUILayout.Space();
                }
                EditorGUILayout.EndFoldoutHeaderGroup();
            }

            void DrawSurfaceInputs2()
            {
                showSurfaceInputs2 = EditorGUILayout.BeginFoldoutHeaderGroup(showSurfaceInputs2, "Surface Inputs 2");
                if (showSurfaceInputs2)
                {
                    //EditorGUILayout.HelpBox("Paint Vertex Colors to show Surface Inputs 2. See docs for more info.", MessageType.Info);
                    EditorGUI.indentLevel++;
                    DrawToggleProperty(_UseVertexColors, new GUIContent("Enabled"));
                    EditorGUILayout.LabelField("Paint Vertex Colors to show Surface Inputs 2. See docs for more info.");
                    EditorGUILayout.Space();
                    TexturePropertyWithColor(
                        new GUIContent(
                            "Base Color",
                            "A property that defines its overall color. It serves as a starting point for calculating the final color of the surface after taking into account lighting, shadows, reflections, and other effects."
                        ),
                        _MainTex2,
                        _BaseColor2,
                        true,
                        false
                    );

                    // Metalness
                    EditorGUI.BeginChangeCheck();
                    MaterialProperty p = _MetalnessMap2.textureValue == null ? _Metalness2 : null;
                    e.TexturePropertySingleLine(
                        new GUIContent(
                            "Metalness",
                            "A property that defines how metallic it appears. It affects the way light interacts with the surface, creating the illusion of materials like metal, chrome, or gold, as well as non-metallic materials like plastic or glass."
                        ),
                        _MetalnessMap2,
                        p
                    );
                    if (_MetalnessMap2.textureValue != null)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUI.indentLevel++;
                        e.DefaultShaderProperty(_MetalnessMapExposure2, "Exposure");
                        EditorGUI.indentLevel--;
                        EditorGUI.indentLevel--;
                    }
                    if (EditorGUI.EndChangeCheck())
                    {
                        _HasMetalnessMap2.floatValue = (_MetalnessMap2.textureValue == null ? 0 : 1);
                    }

                    // Roughness
                    EditorGUI.BeginChangeCheck();
                    p = _RoughnessMap2.textureValue == null ? _Roughness2 : null;
                    e.TexturePropertySingleLine(
                        new GUIContent(
                            "Roughness",
                            "A property that defines how rough or smooth it appears. It affects the way light scatters and spreads across the surface, creating the illusion of materials like matte paint, sandpaper, or satin."
                        ),
                        _RoughnessMap2,
                        p
                    );
                    if (_RoughnessMap2.textureValue != null)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUI.indentLevel++;
                        e.DefaultShaderProperty(_RoughnessMapExposure2, "Exposure");
                        EditorGUI.indentLevel--;
                        EditorGUI.indentLevel--;
                    }
                    if (EditorGUI.EndChangeCheck())
                    {
                        _HasRoughnessMap2.floatValue = (_RoughnessMap2.textureValue == null ? 0 : 1);
                    }

                    e.ShaderProperty(
                        _Specularity2,
                        new GUIContent(
                            "Specularity",
                            "Refers to the glossy reflections on shiny surfaces. More specular materials are more reflective and create brighter highlights."
                        )
                    );

                    EditorGUI.BeginChangeCheck();
                    DrawConditionalTextureProperty(
                        new GUIContent(
                            "Normal",
                            "A property that defines the direction the surface is facing. It affects the way light interacts with the surface, creating the illusion of bumps, ridges, and other surface details, as well as the appearance of shadows and reflections."
                        ),
                        _NormalMap2,
                        _NormalStrength2
                    );
                    if (EditorGUI.EndChangeCheck())
                    {
                        _HasNormalMap2.floatValue = (_NormalMap2.textureValue == null ? 0 : 1);
                    }

                    EditorGUI.BeginChangeCheck();
                    DrawConditionalTextureProperty(
                        new GUIContent("Height", "A property that defines the displacment of the surface."),
                        _HeightMap2,
                        _HeightStrength2
                    );
                    if (EditorGUI.EndChangeCheck())
                    {
                        _HasHeightMap2.floatValue = (_HeightMap2.textureValue == null ? 0 : 1);
                    }

                    DrawConditionalTextureProperty(
                        new GUIContent(
                            "Occlusion",
                            "A property that defines how much of a surface is blocked from receiving light. It affects the appearance of shadows and the overall brightness of a surface, creating a more realistic and visually appealing environment."
                        ),
                        _OcclusionMap2,
                        _OcclusionStrength2
                    );

                    EditorGUI.BeginChangeCheck();
                    TexturePropertyWithColor(
                        new GUIContent(
                            "Emission",
                            "A property of an object that defines how much light it emits on its own, independent of external light sources. It creates the illusion of glowing objects and surfaces, such as light bulbs, lamps, or neon signs."
                        ),
                        _EmissionMap2,
                        _Emission2,
                        false,
                        true
                    );
                    if (EditorGUI.EndChangeCheck())
                    {
                        _HasEmissionMap2.floatValue = (_EmissionMap2.textureValue == null ? 0 : 1);
                    }

                    e.TextureScaleOffsetProperty(_MainTex2);

                    EditorGUI.indentLevel--;
                    EditorGUILayout.Space();
                }
                EditorGUILayout.EndFoldoutHeaderGroup();
            }
        }

        void DrawToggleProperty(MaterialProperty p, GUIContent c)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = p.hasMixedValue;
            bool v = EditorGUILayout.Toggle(c, p.floatValue == 1.0f);
            if (EditorGUI.EndChangeCheck())
            {
                p.floatValue = v ? 1.0f : 0.0f;
            }
            EditorGUI.showMixedValue = false;
        }

        void TexturePropertyWithColor(
            GUIContent label,
            MaterialProperty textureProp,
            MaterialProperty colorProperty,
            bool showAlpha = true,
            bool showHdr = true
        )
        {
            Rect controlRectForSingleLine = EditorGUILayout.GetControlRect(true, MaterialEditor.GetDefaultPropertyHeight(colorProperty));

            e.TexturePropertyMiniThumbnail(controlRectForSingleLine, textureProp, label.text, label.tooltip);

            int indentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = colorProperty.hasMixedValue;
            Color colorValue = EditorGUI.ColorField(
                MaterialEditor.GetRectAfterLabelWidth(controlRectForSingleLine),
                GUIContent.none,
                colorProperty.colorValue,
                showEyedropper: true,
                showAlpha,
                showHdr
            );
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
            {
                colorProperty.colorValue = colorValue;
            }

            EditorGUI.indentLevel = indentLevel;
        }

        void DrawConditionalTextureProperty(GUIContent content, MaterialProperty a, MaterialProperty b)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = a.hasMixedValue || b.hasMixedValue;

            if (a.textureValue == null)
                b = null;

            e.TexturePropertySingleLine(content, a, b);
            if (EditorGUI.EndChangeCheck())
            {
                if (b != null)
                {
                    b.floatValue = Mathf.Max(0, b.floatValue);
                }
            }
            EditorGUI.showMixedValue = false;
        }

        private static void DrawEnumProperty(Enum e, MaterialProperty p, GUIContent c)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = p.hasMixedValue;
            var v = EditorGUILayout.EnumPopup(c, e);
            if (EditorGUI.EndChangeCheck())
            {
                p.floatValue = Convert.ToInt32(v);
            }
            EditorGUI.showMixedValue = false;
        }

        private static void SetSrcDestProperties(Material t, BlendMode src, BlendMode dst)
        {
            t.SetFloat("_SrcBlend", (float)src);
            t.SetFloat("_DstBlend", (float)dst);
        }

        private static void SetupDepthWriting(Material t, bool depthWrite)
        {
            t.SetFloat("_ZWrite", depthWrite ? 1.0f : 0.0f);
            t.SetShaderPassEnabled("DepthOnly", depthWrite);
        }
    }
}
