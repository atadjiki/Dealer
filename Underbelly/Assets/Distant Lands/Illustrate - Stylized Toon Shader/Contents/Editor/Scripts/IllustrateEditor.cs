using UnityEngine;
using UnityEditor.EditorTools;
using UnityEditor;

public class IllustrateEditor : ShaderGUI
{


    static bool outlineSettings;
    static bool normalsSettings;
    static bool gradientSettings;
    static bool glintSettings;
    static bool halftoneSettings;
    static bool triplanarSettings;
    static bool lightingSettings;
    static bool emissionSettings;
    static bool precipitationSettings;
    static bool flutterSettings;
    static bool swirlSettings;
    static bool swaySettings;
    static bool wavingSettings;
    static bool noiseSettings;
    static bool colorSettings;
    static bool advancedSettings;
    static bool dissolveSettings;

    public enum ShaderStyle { opaque, alphaclip, outline, transparent, simple }
    public ShaderStyle shaderStyle = ShaderStyle.opaque;



    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {

        switch ((materialEditor.target as Material).shader.name)
        {
            case ("Distant Lands/Illustrate/Opaque"):
                shaderStyle = ShaderStyle.opaque;
                break;
            case ("Distant Lands/Illustrate/Alpha Clipped"):
                shaderStyle = ShaderStyle.alphaclip;
                break;
            case ("Distant Lands/Illustrate/Outline"):
                shaderStyle = ShaderStyle.outline;
                break;
            case ("Distant Lands/Illustrate/Transparent"):
                shaderStyle = ShaderStyle.transparent;
                break;
            case ("Distant Lands/Illustrate/Simple"):
                shaderStyle = ShaderStyle.simple;
                break;

        }

        GUIStyle foldoutStyle = new GUIStyle(GUI.skin.GetStyle("MiniPopup"));
        foldoutStyle.fontStyle = FontStyle.Bold;
        foldoutStyle.margin = new RectOffset(30, 10, 5, 5);
        foldoutStyle.padding = new RectOffset(7, 5, 5, 5);
        foldoutStyle.fixedHeight = 30;

        GUIStyle toggleStyle = new GUIStyle(GUI.skin.GetStyle("Toggle"));
        toggleStyle.margin = new RectOffset(5, 5, 5, 5);

        GUIStyle titleStyle = new GUIStyle(GUI.skin.GetStyle("boldLabel"));
        foldoutStyle.fontStyle = FontStyle.Bold;


        colorSettings = EditorGUILayout.BeginFoldoutHeaderGroup(colorSettings, new GUIContent("    Coloring", (Texture)Resources.Load("Editor Textures/Coloring")), foldoutStyle);
        EditorGUILayout.EndFoldoutHeaderGroup();
        if (colorSettings)
        {
            EditorGUI.indentLevel++;
            MaterialProperty colorProperty = ShaderGUI.FindProperty("_MainColor", properties);
            materialEditor.ShaderProperty(colorProperty, colorProperty.displayName);
            EditorGUI.indentLevel++;
            MaterialProperty texture = ShaderGUI.FindProperty("_Texture", properties);
            materialEditor.TextureProperty(texture, "Main Texture");
            EditorGUI.indentLevel--;
            EditorGUILayout.Space();

            if (shaderStyle != ShaderStyle.simple)
            {
                EditorGUILayout.LabelField("Color Adjustments", titleStyle);
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_ClampAdjustments", properties), "Clamp Adjustments");
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_HueShift", properties), "Hue");
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_SaturationShift", properties), "Saturation");
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_ValueShift", properties), "Value");
                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Color Variation", titleStyle);
                MaterialProperty variation = ShaderGUI.FindProperty("_UseHSVVariation", properties);
                materialEditor.ShaderProperty(variation, "Use Variation");
                EditorGUI.indentLevel++;
                EditorGUI.BeginDisabledGroup(variation.floatValue == 0);
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_VariationScale", properties), "Variation Scale");
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_VariationSource", properties), "Variation Source");
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_HueVariation", properties), "Hue");
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_SaturationVariation", properties), "Saturation");
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_ValueVariation", properties), "Value");
                EditorGUI.EndDisabledGroup();
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
                MaterialProperty posterizeColors = ShaderGUI.FindProperty("_PosterizeColors", properties);
                materialEditor.ShaderProperty(posterizeColors, posterizeColors.displayName);
                EditorGUI.BeginDisabledGroup(posterizeColors.floatValue == 0);
                EditorGUI.indentLevel++;
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_ColorNumbers", properties), "Steps");
                EditorGUI.indentLevel--;
                EditorGUI.EndDisabledGroup();
            }

            EditorGUI.indentLevel--;
            EditorGUILayout.Space();
        }

        normalsSettings = EditorGUILayout.BeginFoldoutHeaderGroup(normalsSettings, new GUIContent("    Normals", (Texture)Resources.Load("Editor Textures/Normals")), foldoutStyle);
        EditorGUILayout.EndFoldoutHeaderGroup();
        if (normalsSettings)
        {

            EditorGUI.indentLevel++;
            MaterialProperty normalMode = ShaderGUI.FindProperty("_NormalMode", properties);
            materialEditor.ShaderProperty(normalMode, normalMode.displayName);
            EditorGUI.indentLevel++;
            switch (normalMode.floatValue)
            {
                case (0):
                    MaterialProperty normalMap = ShaderGUI.FindProperty("_NormalMap", properties);
                    materialEditor.ShaderProperty(normalMap, normalMap.displayName);
                    break;
                case (1):
                    DisplayVector3("_CustomNormalEllipseSize", properties, "Normal Ellipse Size");
                    break;
                case (2):
                    DisplayVector3("_CustomNormalDirection", properties, "Normal Direction");
                    break;
                case (4):
                    materialEditor.ShaderProperty(ShaderGUI.FindProperty("_BlendStrength", properties), "Blend Amount");
                    materialEditor.ShaderProperty(ShaderGUI.FindProperty("_NormalMap", properties), "Normal Map");
                    DisplayVector3("_CustomNormalEllipseSize", properties, "Normal Ellipse Size");
                    break;

            }
            EditorGUI.indentLevel--;
            EditorGUI.indentLevel--;
            EditorGUILayout.Space();

        }


        lightingSettings = EditorGUILayout.BeginFoldoutHeaderGroup(lightingSettings, new GUIContent("    Lighting", (Texture)Resources.Load("Editor Textures/Light")), foldoutStyle);
        EditorGUILayout.EndFoldoutHeaderGroup();
        if (lightingSettings)
        {
            EditorGUI.indentLevel++;
            MaterialProperty mode = ShaderGUI.FindProperty("_LightingMode", properties);
            materialEditor.ShaderProperty(mode, mode.displayName);
            if (mode.floatValue == 1)
            {

                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_LightRamp", properties), new GUIContent("Light Ramp Multiplier"));
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_LightRampOffset", properties), new GUIContent("Light Ramp Offset"));
                EditorGUILayout.Space();

                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_LightColor", properties), new GUIContent("Light Color"));
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_ShadowColor", properties), new GUIContent("Shadow Color"));
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_MultiplyByLightColor", properties), new GUIContent("Multiply By Light Color"));
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_UseShadows", properties), new GUIContent("Use Shadows"));
                EditorGUILayout.Space();

                MaterialProperty prop = ShaderGUI.FindProperty("_PosterizeLight", properties);
                materialEditor.ShaderProperty(prop, new GUIContent("Posterize Light"));
                EditorGUI.indentLevel++;
                EditorGUI.BeginDisabledGroup(prop.floatValue == 0);
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_LightSteps", properties), new GUIContent("Light Steps"));
                EditorGUI.EndDisabledGroup();
                EditorGUI.indentLevel--;

                EditorGUILayout.Space();
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_AdditionalLightRamp", properties), new GUIContent("Additional Light Ramp"));


                EditorGUILayout.Space();
                MaterialProperty useSpecular = ShaderGUI.FindProperty("_UseSpecular", properties);
                materialEditor.ShaderProperty(useSpecular, new GUIContent("Use Specular"));
                EditorGUI.indentLevel++;
                EditorGUI.BeginDisabledGroup(useSpecular.floatValue == 0);
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_SpecularColor", properties), new GUIContent("Specular Color"));
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_SpecularRamp", properties), new GUIContent("Specular Ramp"));
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_SpecularRampOffset", properties), new GUIContent("Specular Ramp Offset"));
                EditorGUI.EndDisabledGroup();
                EditorGUI.indentLevel--;

                if (shaderStyle != ShaderStyle.simple)
                {
                    EditorGUILayout.Space();
                    MaterialProperty useRim = ShaderGUI.FindProperty("_UseRimLighting", properties);
                    materialEditor.ShaderProperty(useRim, new GUIContent("Use Rim Lighting"));
                    EditorGUI.indentLevel++;
                    EditorGUI.BeginDisabledGroup(useRim.floatValue == 0);
                    materialEditor.ShaderProperty(ShaderGUI.FindProperty("_RimLightColor", properties), new GUIContent("Rim Light Color"));
                    materialEditor.ShaderProperty(ShaderGUI.FindProperty("_RimRamp", properties), new GUIContent("Rim Light Ramp"));
                    materialEditor.ShaderProperty(ShaderGUI.FindProperty("_RimLightRampOffset", properties), new GUIContent("Rim Light Ramp Offset"));
                    materialEditor.ShaderProperty(ShaderGUI.FindProperty("_RimLightLitIntensity", properties), new GUIContent("Rim Light Intensity Lit"));
                    materialEditor.ShaderProperty(ShaderGUI.FindProperty("_RimLightShadowIntensity", properties), new GUIContent("Rim Light Intensity Shadow"));
                    EditorGUI.EndDisabledGroup();
                    EditorGUI.indentLevel--;
                }

                EditorGUILayout.Space();
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_UseModifiedNormals", properties), new GUIContent("Use Modified Normals For Specular/Rim Lighting"));



            }
            EditorGUI.indentLevel--;
            EditorGUILayout.Space();

        }

        if (shaderStyle != ShaderStyle.simple)
        {
            EditorGUILayout.BeginHorizontal();
            MaterialProperty useGradients = ShaderGUI.FindProperty("_UseGradientShading", properties);
            gradientSettings = EditorGUILayout.BeginFoldoutHeaderGroup(gradientSettings, new GUIContent("    Gradient Shading", (Texture)Resources.Load("Editor Textures/Gradient")), foldoutStyle);
            useGradients.floatValue = EditorGUILayout.Toggle(useGradients.floatValue == 1, toggleStyle, GUILayout.Width(20), GUILayout.Height(35)) ? 1 : 0;
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndFoldoutHeaderGroup();
            if (gradientSettings)
            {

                EditorGUI.BeginDisabledGroup(useGradients.floatValue == 0);
                EditorGUI.indentLevel++;
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_GradientSource", properties), new GUIContent("Gradient Source", "What value is used to interpolate the gradient?"));
                EditorGUI.indentLevel++;
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_Space", properties), new GUIContent("Space", "Should position be calculated in local or world space?"));
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_GradientSensitivity", properties), new GUIContent("Sensitivity"));
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_GradientOffset", properties), new GUIContent("Offset"));
                DisplayVector3("_GradientPositionalOffset", properties, new GUIContent("Position Offset", "Applies an offset to the inital position that the distance is calculated from."));
                EditorGUI.indentLevel--;
                DisplayVector3("_GradientChannelMask", properties, new GUIContent("Mask", "What channel (vertex color or axial position) should be considered when calculating the gradient?"));
                EditorGUILayout.Space();

                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_NearColor", properties), new GUIContent("Gradient Color 1"));
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_FarColor", properties), new GUIContent("Gradient Color 2"));
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
                EditorGUI.EndDisabledGroup();

            }

            if (shaderStyle == ShaderStyle.alphaclip || shaderStyle == ShaderStyle.transparent)
            {
                EditorGUILayout.BeginHorizontal();
                MaterialProperty useDissolve = ShaderGUI.FindProperty("_UseDissolve", properties);
                dissolveSettings = EditorGUILayout.BeginFoldoutHeaderGroup(dissolveSettings, new GUIContent("    Dissolve & Materialize", (Texture)Resources.Load("Editor Textures/Dissolve")), foldoutStyle);
                useDissolve.floatValue = EditorGUILayout.Toggle(useDissolve.floatValue == 1, toggleStyle, GUILayout.Width(20), GUILayout.Height(35)) ? 1 : 0;
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndFoldoutHeaderGroup();
                if (dissolveSettings)
                {
                    EditorGUI.BeginDisabledGroup(useDissolve.floatValue == 0);

                    EditorGUI.indentLevel++;
                    materialEditor.ShaderProperty(ShaderGUI.FindProperty("_DissolveAmount", properties), new GUIContent("Dissolve Amount"));
                    materialEditor.ShaderProperty(ShaderGUI.FindProperty("_DissolveScale", properties), new GUIContent("Scale"));
                    EditorGUI.indentLevel--;
                    EditorGUILayout.Space();

                    EditorGUI.EndDisabledGroup();

                }
            }

            EditorGUILayout.BeginHorizontal();
            MaterialProperty useTriplanar = ShaderGUI.FindProperty("_UseTriplanar", properties);
            triplanarSettings = EditorGUILayout.BeginFoldoutHeaderGroup(triplanarSettings, new GUIContent("    Triplanar", (Texture)Resources.Load("Editor Textures/Triplanar")), foldoutStyle);
            useTriplanar.floatValue = EditorGUILayout.Toggle(useTriplanar.floatValue == 1, toggleStyle, GUILayout.Width(20), GUILayout.Height(35)) ? 1 : 0;
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndFoldoutHeaderGroup();
            if (triplanarSettings)
            {
                EditorGUI.BeginDisabledGroup(useTriplanar.floatValue == 0);

                EditorGUI.indentLevel++;
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_TriplanarTexture", properties), new GUIContent("Texture"));
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_TriplanarColor", properties), new GUIContent("Color"));
                EditorGUILayout.Space();
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_TriplanarSpace", properties), new GUIContent("Space"));
                EditorGUI.indentLevel++;
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_TriplanarDirection", properties), new GUIContent("Direction"));
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_TriplanarMultiplier", properties), new GUIContent("Multiplier"));
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_TriplanarOffset", properties), new GUIContent("Offset"));
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_ClipTriplanar", properties), new GUIContent("Clip"));
                EditorGUI.indentLevel--;
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();

                EditorGUI.EndDisabledGroup();

            }

            EditorGUILayout.BeginHorizontal();
            MaterialProperty usePrecip = ShaderGUI.FindProperty("_UseCOZYPrecipitation", properties);
            precipitationSettings = EditorGUILayout.BeginFoldoutHeaderGroup(precipitationSettings, new GUIContent("    COZY Precipitation", (Texture)Resources.Load("Editor Textures/Precipitation")), foldoutStyle);
            usePrecip.floatValue = EditorGUILayout.Toggle(usePrecip.floatValue == 1, toggleStyle, GUILayout.Width(20), GUILayout.Height(35)) ? 1 : 0;
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndFoldoutHeaderGroup();
            if (precipitationSettings)
            {
                EditorGUI.BeginDisabledGroup(usePrecip.floatValue == 0);

                EditorGUI.indentLevel++;
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_PuddleScale", properties), new GUIContent("Puddle Scale"));
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_PuddleColor", properties), new GUIContent("Puddle Color"));
                EditorGUILayout.Space();
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_SnowScale", properties), new GUIContent("Snow Scale"));
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_SnowColor", properties), new GUIContent("Snow Color"));
                MaterialProperty texture = ShaderGUI.FindProperty("_SnowTexture", properties);
                materialEditor.TextureProperty(texture, "Snow Texture");

                EditorGUI.indentLevel--;
                EditorGUILayout.Space();

                EditorGUI.EndDisabledGroup();


            }

            EditorGUILayout.BeginHorizontal();
            MaterialProperty useHalftone = ShaderGUI.FindProperty("_UseHalftone", properties);
            halftoneSettings = EditorGUILayout.BeginFoldoutHeaderGroup(halftoneSettings, new GUIContent("    Halftone", (Texture)Resources.Load("Editor Textures/Halftone")), foldoutStyle);
            useHalftone.floatValue = EditorGUILayout.Toggle(useHalftone.floatValue == 1, toggleStyle, GUILayout.Width(20), GUILayout.Height(35)) ? 1 : 0;
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndFoldoutHeaderGroup();
            if (halftoneSettings)
            {
                EditorGUI.BeginDisabledGroup(useHalftone.floatValue == 0);

                EditorGUI.indentLevel++;
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_HalftoneScale", properties), new GUIContent("Scale"));
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_HalftoneMultiplier", properties), new GUIContent("Multiplier"));

                EditorGUI.indentLevel--;
                EditorGUILayout.Space();

                EditorGUI.EndDisabledGroup();


            }

            EditorGUILayout.BeginHorizontal();
            MaterialProperty useGlint = ShaderGUI.FindProperty("_UseGlint", properties);
            glintSettings = EditorGUILayout.BeginFoldoutHeaderGroup(glintSettings, new GUIContent("    Glint", (Texture)Resources.Load("Editor Textures/Glint")), foldoutStyle);
            useGlint.floatValue = EditorGUILayout.Toggle(useGlint.floatValue == 1, toggleStyle, GUILayout.Width(20), GUILayout.Height(35)) ? 1 : 0;
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndFoldoutHeaderGroup();
            if (glintSettings)
            {
                EditorGUI.BeginDisabledGroup(useGlint.floatValue == 0);

                EditorGUI.indentLevel++;
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_GlintColor", properties), new GUIContent("Glint Color"));
                EditorGUI.indentLevel++;
                MaterialProperty texture = ShaderGUI.FindProperty("_GlintTexture", properties);
                materialEditor.TexturePropertySingleLine(new GUIContent("Glint Texture"), texture);
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_GlintScale", properties), new GUIContent("Glint Scale"));
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_MultiplyByLightRatio", properties), new GUIContent("Multiply by Light Ratio"));
                EditorGUILayout.Space();
                EditorGUI.indentLevel--;
                EditorGUI.indentLevel--;

                EditorGUI.EndDisabledGroup();


            }

            EditorGUILayout.BeginHorizontal();
            MaterialProperty useEmission = ShaderGUI.FindProperty("_UseEmission", properties);
            emissionSettings = EditorGUILayout.BeginFoldoutHeaderGroup(emissionSettings, new GUIContent("    Emission", (Texture)Resources.Load("Editor Textures/Emission")), foldoutStyle);
            useEmission.floatValue = EditorGUILayout.Toggle(useEmission.floatValue == 1, toggleStyle, GUILayout.Width(20), GUILayout.Height(35)) ? 1 : 0;
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndFoldoutHeaderGroup();
            if (emissionSettings)
            {
                EditorGUI.BeginDisabledGroup(useEmission.floatValue == 0);

                EditorGUI.indentLevel++;
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_EmissionColor", properties), new GUIContent("Emission Color"));
                EditorGUI.indentLevel++;
                MaterialProperty texture = ShaderGUI.FindProperty("_EmissionTexture", properties);
                materialEditor.TexturePropertySingleLine(new GUIContent("Emission Texture"), texture);
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_EmissionEffectScale", properties), new GUIContent("Scale"));
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_EmissionUVSource", properties), new GUIContent("Source", "Should the emission render using screenspace UV or the models UV maps"));
                EditorGUILayout.Space();
                EditorGUI.indentLevel--;

                MaterialProperty scrollEmission = ShaderGUI.FindProperty("_ScrollEmission", properties);
                materialEditor.ShaderProperty(scrollEmission, new GUIContent("Scroll Emission Texture"));
                EditorGUI.indentLevel++;
                EditorGUI.BeginDisabledGroup(scrollEmission.floatValue == 0);
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_EmissionScrolling1", properties), new GUIContent("Scroll Direction 1"));
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_EmissionScrolling2", properties), new GUIContent("Scroll Direction 2"));
                EditorGUI.EndDisabledGroup();
                EditorGUI.indentLevel--;

                EditorGUILayout.Space();
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_EmissionLightRatio", properties), new GUIContent("Intensity (Lit)"));
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_EmissionShadowRatio", properties), new GUIContent("Intensity (Shadow)"));
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();

                EditorGUI.EndDisabledGroup();


            }

            EditorGUILayout.BeginHorizontal();
            MaterialProperty useNoise = ShaderGUI.FindProperty("_UseScreenNoise", properties);
            noiseSettings = EditorGUILayout.BeginFoldoutHeaderGroup(noiseSettings, new GUIContent("    Noise", (Texture)Resources.Load("Editor Textures/Noise")), foldoutStyle);
            useNoise.floatValue = EditorGUILayout.Toggle(useNoise.floatValue == 1, toggleStyle, GUILayout.Width(20), GUILayout.Height(35)) ? 1 : 0;
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndFoldoutHeaderGroup();
            if (noiseSettings)
            {
                EditorGUI.BeginDisabledGroup(useNoise.floatValue == 0);

                EditorGUI.indentLevel++;
                MaterialProperty texture = ShaderGUI.FindProperty("_ScreenNoiseTexture", properties);
                materialEditor.TexturePropertySingleLine(new GUIContent("Noise Texture"), texture);
                EditorGUI.indentLevel++;
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_NoiseScale", properties), new GUIContent("Scale"));
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_NoiseUVSource", properties), new GUIContent("Source", "Should the noise render using screenspace UV or the models UV maps"));

                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_NoiseFramerate", properties), new GUIContent("Framerate"));
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_NoiseOffset", properties), new GUIContent("Fresnel Effect"));
                EditorGUILayout.Space();
                EditorGUI.indentLevel--;

                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_NoiseAmountLight", properties), new GUIContent("Intensity (Lit)"));
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_NoiseAmountShadow", properties), new GUIContent("Intensity (Shadow)"));
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();

                EditorGUI.EndDisabledGroup();




            }


            EditorGUILayout.BeginHorizontal();
            MaterialProperty useFlutteringOffset = ShaderGUI.FindProperty("_UseFlutter", properties);
            flutterSettings = EditorGUILayout.BeginFoldoutHeaderGroup(flutterSettings, new GUIContent("    Fluttering Vertex Offset", (Texture)Resources.Load("Editor Textures/Leaf"), "Applies a small perlin noise effect to your vertices."), foldoutStyle);
            useFlutteringOffset.floatValue = EditorGUILayout.Toggle(useFlutteringOffset.floatValue == 1, toggleStyle, GUILayout.Width(20), GUILayout.Height(35)) ? 1 : 0;
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndFoldoutHeaderGroup();
            if (flutterSettings)
            {
                EditorGUI.BeginDisabledGroup(useFlutteringOffset.floatValue == 0);

                EditorGUI.indentLevel++;
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_FlutterAmount", properties), new GUIContent("Amount"));
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_FlutterNoiseScale", properties), new GUIContent("Scale"));
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_FlutterDirection", properties), new GUIContent("Direction"));

                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_FlutterFramerate", properties), new GUIContent("Framerate"));
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_FlutterSpeed", properties), new GUIContent("Movement Speed"));
                EditorGUILayout.Space();

                MaterialProperty prop = ShaderGUI.FindProperty("_FlutterSource", properties);
                materialEditor.ShaderProperty(prop, new GUIContent("Source", "What areas of the model should be most impacted by this vertex offset effect?"));
                if (prop.floatValue != 3)
                {
                    EditorGUI.indentLevel++;
                    materialEditor.ShaderProperty(ShaderGUI.FindProperty("_FlutterMask", properties), new GUIContent("Mask"));
                    materialEditor.ShaderProperty(ShaderGUI.FindProperty("_FlutterSensitivity", properties), new GUIContent("Sensitivity"));
                    materialEditor.ShaderProperty(ShaderGUI.FindProperty("_FlutterOffset", properties), new GUIContent("Offset"));
                    EditorGUI.indentLevel--;
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();

                EditorGUI.EndDisabledGroup();

            }

            EditorGUILayout.BeginHorizontal();
            MaterialProperty useSwirlingOffset = ShaderGUI.FindProperty("_UseSwirl", properties);
            swirlSettings = EditorGUILayout.BeginFoldoutHeaderGroup(swirlSettings, new GUIContent("    Swirling Vertex Offset", (Texture)Resources.Load("Editor Textures/Spiral"), "Applies a rotational offset to your vertices."), foldoutStyle);
            useSwirlingOffset.floatValue = EditorGUILayout.Toggle(useSwirlingOffset.floatValue == 1, toggleStyle, GUILayout.Width(20), GUILayout.Height(35)) ? 1 : 0;
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndFoldoutHeaderGroup();
            if (swirlSettings)
            {
                EditorGUI.BeginDisabledGroup(useSwirlingOffset.floatValue == 0);

                EditorGUI.indentLevel++;
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_SwirlAmount", properties), new GUIContent("Amount"));
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_SwirlNoiseScale", properties), new GUIContent("Scale"));
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_SwirlDirection", properties), new GUIContent("Direction"));

                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_SwirlFramerate", properties), new GUIContent("Framerate"));
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_SwirlSpeed", properties), new GUIContent("Movement Speed"));
                EditorGUILayout.Space();

                MaterialProperty prop = ShaderGUI.FindProperty("_SwirlSource", properties);
                materialEditor.ShaderProperty(prop, new GUIContent("Source", "What areas of the model should be most impacted by this vertex offset effect?"));
                if (prop.floatValue != 3)
                {
                    EditorGUI.indentLevel++;
                    materialEditor.ShaderProperty(ShaderGUI.FindProperty("_SwirlMask", properties), new GUIContent("Mask"));
                    materialEditor.ShaderProperty(ShaderGUI.FindProperty("_SwirlSensitivity", properties), new GUIContent("Sensitivity"));
                    materialEditor.ShaderProperty(ShaderGUI.FindProperty("_SwirlOffset", properties), new GUIContent("Offset"));
                    EditorGUI.indentLevel--;
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();

                EditorGUI.EndDisabledGroup();

            }

            EditorGUILayout.BeginHorizontal();
            MaterialProperty useSwayingOffset = ShaderGUI.FindProperty("_UseSway", properties);
            swaySettings = EditorGUILayout.BeginFoldoutHeaderGroup(swaySettings, new GUIContent("    Sway Vertex Offset", (Texture)Resources.Load("Editor Textures/Sway"), "Applies a large scale perlin offset to your vertices."), foldoutStyle);
            useSwayingOffset.floatValue = EditorGUILayout.Toggle(useSwayingOffset.floatValue == 1, toggleStyle, GUILayout.Width(20), GUILayout.Height(35)) ? 1 : 0;
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndFoldoutHeaderGroup();
            if (swaySettings)
            {
                EditorGUI.BeginDisabledGroup(useSwayingOffset.floatValue == 0);

                EditorGUI.indentLevel++;
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_SwayAmount", properties), new GUIContent("Amount"));
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_SwayNoiseScale", properties), new GUIContent("Scale"));
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_SwayDirection", properties), new GUIContent("Direction"));

                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_SwayFramerate", properties), new GUIContent("Framerate"));
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_SwaySpeed", properties), new GUIContent("Movement Speed"));
                EditorGUILayout.Space();

                MaterialProperty prop = ShaderGUI.FindProperty("_SwaySource", properties);
                materialEditor.ShaderProperty(prop, new GUIContent("Source", "What areas of the model should be most impacted by this vertex offset effect?"));
                if (prop.floatValue != 3)
                {
                    EditorGUI.indentLevel++;
                    materialEditor.ShaderProperty(ShaderGUI.FindProperty("_SwayMask", properties), new GUIContent("Mask"));
                    materialEditor.ShaderProperty(ShaderGUI.FindProperty("_SwaySensitivity", properties), new GUIContent("Sensitivity"));
                    materialEditor.ShaderProperty(ShaderGUI.FindProperty("_SwayOffset", properties), new GUIContent("Offset"));
                    EditorGUI.indentLevel--;
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();

                EditorGUI.EndDisabledGroup();

            }


            EditorGUILayout.BeginHorizontal();
            MaterialProperty useWavingOffset = ShaderGUI.FindProperty("_UseWave", properties);
            wavingSettings = EditorGUILayout.BeginFoldoutHeaderGroup(wavingSettings, new GUIContent("    Wave Vertex Offset", (Texture)Resources.Load("Editor Textures/Wave"), "Applies a waving effect to your vertices."), foldoutStyle);
            useWavingOffset.floatValue = EditorGUILayout.Toggle(useWavingOffset.floatValue == 1, toggleStyle, GUILayout.Width(20), GUILayout.Height(35)) ? 1 : 0;
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndFoldoutHeaderGroup();
            if (wavingSettings)
            {
                EditorGUI.BeginDisabledGroup(useWavingOffset.floatValue == 0);

                EditorGUI.indentLevel++;
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_WaveAmount", properties), new GUIContent("Amount"));
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_WaveNoiseScale", properties), new GUIContent("Scale"));
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_WaveInfluenceDirection", properties), new GUIContent("Offset Direction"));
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_WaveDirection1", properties), new GUIContent("Primary Movement Direction"));
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_WaveDirection2", properties), new GUIContent("Secondary Movement Direction"));

                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_WaveFramerate", properties), new GUIContent("Framerate"));
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_WaveSpeed", properties), new GUIContent("Movement Speed"));
                EditorGUILayout.Space();

                MaterialProperty prop = ShaderGUI.FindProperty("_WaveSource", properties);
                materialEditor.ShaderProperty(prop, new GUIContent("Source", "What areas of the model should be most impacted by this vertex offset effect?"));
                if (prop.floatValue != 3)
                {
                    EditorGUI.indentLevel++;
                    materialEditor.ShaderProperty(ShaderGUI.FindProperty("_WaveMask", properties), new GUIContent("Mask"));
                    materialEditor.ShaderProperty(ShaderGUI.FindProperty("_WaveSensitivity", properties), new GUIContent("Sensitivity"));
                    materialEditor.ShaderProperty(ShaderGUI.FindProperty("_WaveOffset", properties), new GUIContent("Offset"));
                    EditorGUI.indentLevel--;
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();

                EditorGUI.EndDisabledGroup();

            }


            if (shaderStyle == ShaderStyle.outline)
            {
                EditorGUILayout.BeginHorizontal();
                MaterialProperty useOutlines = ShaderGUI.FindProperty("_UseOutlines", properties);
                outlineSettings = EditorGUILayout.BeginFoldoutHeaderGroup(outlineSettings, new GUIContent("    Outlines", (Texture)Resources.Load("Editor Textures/Outline"), "Applies traditional outlines to your model."), foldoutStyle);
                useOutlines.floatValue = EditorGUILayout.Toggle(useOutlines.floatValue == 1, toggleStyle, GUILayout.Width(20), GUILayout.Height(35)) ? 1 : 0;
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndFoldoutHeaderGroup();
                if (outlineSettings)
                {
                    EditorGUI.BeginDisabledGroup(useOutlines.floatValue == 0);

                    EditorGUI.indentLevel++;
                    materialEditor.ShaderProperty(ShaderGUI.FindProperty("_OutlineColor", properties), new GUIContent("Color"));
                    materialEditor.ShaderProperty(ShaderGUI.FindProperty("_OutlineWidth", properties), new GUIContent("Width"));
                    materialEditor.ShaderProperty(ShaderGUI.FindProperty("_NormalSurfaceOutline", properties), new GUIContent("Use Normal Surface", "Enable to fix breakup on sharp edges."));
                    EditorGUI.indentLevel--;
                    EditorGUILayout.Space();

                    EditorGUI.EndDisabledGroup();

                }
            }
        }

        advancedSettings = EditorGUILayout.BeginFoldoutHeaderGroup(advancedSettings, new GUIContent("    Advanced Settings", (Texture)Resources.Load("Editor Textures/More")), foldoutStyle);
        EditorGUILayout.EndFoldoutHeaderGroup();
        if (advancedSettings)
        {
            EditorGUI.indentLevel++;
            if (shaderStyle == ShaderStyle.alphaclip)
            {
                materialEditor.ShaderProperty(ShaderGUI.FindProperty("_AlphaClipThreshold", properties), new GUIContent("Alpha Clip Threshold"));
                EditorGUILayout.Space();
            }
            materialEditor.ShaderProperty(ShaderGUI.FindProperty("_CullMode", properties), new GUIContent("Cull Mode"));
            EditorGUILayout.Space();

            materialEditor.RenderQueueField();
            materialEditor.EnableInstancingField();
            materialEditor.DoubleSidedGIField();
            materialEditor.LightmapEmissionProperty();

            EditorGUI.indentLevel--;
            EditorGUILayout.Space();
        }

    }

    public void DrawColoringWindow()
    {

    }

    public void DisplayVector3(string name, MaterialProperty[] properties)
    {
        MaterialProperty prop = ShaderGUI.FindProperty(name, properties);
        Vector3 display = new Vector3(prop.vectorValue.x, prop.vectorValue.y, prop.vectorValue.z);
        display = EditorGUILayout.Vector3Field(prop.displayName, display);
        prop.vectorValue = display;

    }
    public void DisplayVector3(string name, MaterialProperty[] properties, string displayName)
    {
        MaterialProperty prop = ShaderGUI.FindProperty(name, properties);
        Vector3 display = new Vector3(prop.vectorValue.x, prop.vectorValue.y, prop.vectorValue.z);
        display = EditorGUILayout.Vector3Field(displayName, display);
        prop.vectorValue = display;

    }
    public void DisplayVector3(string name, MaterialProperty[] properties, GUIContent displayName)
    {
        MaterialProperty prop = ShaderGUI.FindProperty(name, properties);
        Vector3 display = new Vector3(prop.vectorValue.x, prop.vectorValue.y, prop.vectorValue.z);
        display = EditorGUILayout.Vector3Field(displayName, display);
        prop.vectorValue = display;

    }



}