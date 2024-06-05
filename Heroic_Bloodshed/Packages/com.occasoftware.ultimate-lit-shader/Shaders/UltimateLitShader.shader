Shader"OccaSoftware/UltimateLitShader"
{
    Properties
    {

        //////////////////////////////////////
        //  Shared Material Settings        //
        //////////////////////////////////////

        // Subsurface
        _SubsurfaceFalloff ("Subsurface Falloff", Range(1, 8)) = 2.0
        [HDR] _SubsurfaceColor ("Subsurface Color", Color) = (1, 1, 1)
        _SubsurfaceDistortion ("Subsurface Distortion", Range(0.0, 1.0)) = 0.3
        _SubsurfaceThicknessMap ("Subsurface Thickness Map", 2D) = "black" {}
        _SubsurfaceThickness ("Subsurface Thickness", Range(0.0, 1.0)) = 0.8
        [Toggle(_HasSubsurfaceMap)] _HasSubsurfaceMap("Has Subsurface Map", Float) = 0
        _SubsurfaceAmbient ("Subsurface Ambient", Range(0, 1)) = 0.2
        [Toggle(_SubsurfaceEnabled)] _SubsurfaceEnabled("Subsurface Enabled", Float) = 0

        // Surface
        _Surface("Surface", Float) = 0.0
        _Blend("Blend", Float) = 0.0
        _AlphaClip("Alpha Clip", Range(0.0, 1.0)) = 0.0
        [Toggle(_AlphaClipEnabled)] _AlphaClipEnabled ("Alpha Clip Enabled", Float) = 0.0
        [HideInInspector] _SrcBlend("Source Blending", Float) = 1.0
        [HideInInspector] _DstBlend("Dest Blending", Float) = 0.0

        _SortPriority("Sort Priority", Range(-50.0, 50.0)) = 0.0

        // Options
        [Toggle(_ReceiveShadowsEnabled)] _ReceiveShadowsEnabled("Receive Shadows", Float) = 1
        [Toggle(_ReceiveFogEnabled)] _ReceiveFogEnabled ("Receive Fog", Float) = 1
        [Toggle(_UseVertexColors)] _UseVertexColors("Use Vertex Colors", Float) = 0

        //Advanced
        [Enum(Off, 0, On, 1)]_ZWrite ("ZWrite", Float) = 1.0 // Default to "ZWrite On"
        [Enum(UnityEngine.Rendering.CompareFunction)] _ZTest("Depth Test", Float) = 4 // Default to "LEqual"
        [Enum(UnityEngine.Rendering.CullMode)] _Culling ("Culling", Float) = 2 // Default to "Cull Back"

        

        ////////////////////////
        //  Material 1        //
        ////////////////////////

        // Albedo
        [MainTexture] _MainTex ("Base Color", 2D) = "white" {}
        [MainColor] _BaseColor("Base Color", Color) = (0.5, 0.5, 0.5, 1.0)
        
        // Rough
        _RoughnessMap ("Roughness Texture", 2D) = "black" {}
        _Roughness("Roughness", Range(0.0, 1.0)) = 0.5
        _RoughnessMapExposure ("Roughness Map Exposure", Range(-1, 1)) = 0.0
        [Toggle(_HasRoughnessMap)] _HasRoughnessMap("Has Roughness Map", Float) = 0
        

        // Normal
        [Normal] _NormalMap ("Normal Texture", 2D) = "bump" {}
        _NormalStrength ("Normal Strength", Float) = 1
        [Toggle(_HasNormalMap)] _HasNormalMap("Has Normal Map", Float) = 0
        

        // Specularity
        _Specularity ("Specularity", Range(0.0, 1.0)) = 0.5
        

        // Occlusion
        _OcclusionMap ("Occlusion", 2D) = "white" {}
        _OcclusionStrength("Occlusion Strength", Range(0.0, 1.0)) = 1
        

        // Emission
        _EmissionMap("Emission Map", 2D) = "black" {}
        [HDR] _Emission("Emission", Color) = (0,0,0)
        [Toggle(_HasEmissionMap)] _HasEmissionMap("Has Emission Map", Float) = 0
        

        // Metal
        _MetalnessMap ("Metalness", 2D) = "white" {}
        _Metalness ("Metalness", Range(0.0, 1.0)) = 0
        _MetalnessMapExposure ("Metalness Map Exposure", Range(-1, 1)) = 0.0
        [Toggle(_HasMetalnessMap)] _HasMetalnessMap("Has Metalness Map", Float) = 0


        // Height
        _HeightMap ("Height Map", 2D) = "black" {}
        _HeightStrength ("Height Strength", Range(0.0, 0.2)) = 0.02
        [Toggle(_HasHeightMap)] _HasHeightMap("Has Height Map", Float) = 0


        ////////////////////////
        //  Material 2        //
        ////////////////////////

        // Albedo
        _MainTex2 ("Base Color", 2D) = "white" {}
        _BaseColor2("Base Color", Color) = (0.5, 0.5, 0.5, 1.0)
        
        // Rough
        _RoughnessMap2 ("Roughness Texture", 2D) = "black" {}
        _Roughness2("Roughness", Range(0.0, 1.0)) = 0.5
        _RoughnessMapExposure2 ("Roughness Map Exposure", Range(-1, 1)) = 0.0
        [Toggle(_HasRoughnessMap2)] _HasRoughnessMap2("Has Roughness Map", Float) = 0
        

        // Normal
        [Normal] _NormalMap2 ("Normal Texture", 2D) = "bump" {}
        _NormalStrength2 ("Normal Strength", Float) = 1
        [Toggle(_HasNormalMap2)] _HasNormalMap2("Has Normal Map", Float) = 0
        

        // Specularity
        _Specularity2 ("Specularity", Range(0.0, 1.0)) = 0.5
        

        // Occlusion
        _OcclusionMap2 ("Occlusion", 2D) = "white" {}
        _OcclusionStrength2("Occlusion Strength", Range(0.0, 1.0)) = 1
        

        // Emission
        _EmissionMap2("Emission Map", 2D) = "black" {}
        [HDR] _Emission2("Emission", Color) = (0,0,0)
        [Toggle(_HasEmissionMap2)] _HasEmissionMap2("Has Emission Map", Float) = 0
        

        // Metal
        _MetalnessMap2 ("Metalness", 2D) = "white" {}
        _Metalness2 ("Metalness", Range(0.0, 1.0)) = 0
        _MetalnessMapExposure2 ("Metalness Map Exposure", Range(-1, 1)) = 0.0
        [Toggle(_HasMetalnessMap2)] _HasMetalnessMap2("Has Metalness Map", Float) = 0


        // Height
        _HeightMap2 ("Height Map", 2D) = "black" {}
        _HeightStrength2 ("Height Strength", Range(0.0, 0.2)) = 0.02
        [Toggle(_HasHeightMap2)] _HasHeightMap2("Has Height Map", Float) = 0


    }
    
    
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" }
        
        
        Pass
        {
            Name "ForwardLit"
            Tags {"LightMode" = "UniversalForwardOnly"}
            
            Blend [_SrcBlend] [_DstBlend]
            Cull [_Culling]
            ZWrite [_ZWrite]
            ZTest LEqual
            ZClip Off
            
            HLSLPROGRAM


            // Render Paths
            #pragma multi_compile _ _FORWARD_PLUS

            // Fog, Decals, SSAO
            #pragma multi_compile_fog
            #pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
            #pragma multi_compile _ _SCREEN_SPACE_OCCLUSION

            // Transparency
            #pragma shader_feature_local_fragment _SURFACE_TYPE_TRANSPARENT
            #pragma shader_feature_local_fragment _ALPHATEST_ON
            #pragma shader_feature_local_fragment _ALPHAPREMULTIPLY_ON
            
            // Lighting
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile _ _SHADOWS_SOFT

            // Unity stuff
            #pragma multi_compile_fragment _ _LIGHT_LAYERS
            #pragma multi_compile_fragment _ _LIGHT_COOKIES
            #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RenderingLayers.hlsl"
            #pragma multi_compile_fragment _ LOD_FADE_CROSSFADE
            
            // Lightmapping
            #pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
            #pragma multi_compile _ SHADOWS_SHADOWMASK
            #pragma multi_compile _ DIRLIGHTMAP_COMBINED
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile _ DYNAMICLIGHTMAP_ON

            // Instancing
            #pragma multi_compile_instancing
            #pragma instancing_options renderinglayer
            #pragma multi_compile _ DOTS_INSTANCING_ON
            
            #pragma vertex Vert
            #pragma fragment Frag
            
            #include "UltimateLitPass.hlsl"
            ENDHLSL
        }
        
        Pass
        {
            Name "ShadowCaster"
            
            Tags {"LightMode" = "ShadowCaster"}
            Cull [_Culling]
            ZWrite On
            ZTest LEqual
            ZClip Off
            
            
            HLSLPROGRAM

            #pragma shader_feature_local_fragment _ALPHATEST_ON
            #pragma multi_compile_instancing
            #pragma multi_compile _ DOTS_INSTANCING_ON
            #pragma multi_compile_fragment _ LOD_FADE_CROSSFADE
            
            #pragma vertex Vert
            #pragma fragment FragmentDepthOnly
            #define CAST_SHADOWS_PASS
            #pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW
            
            #include "UltimateLitPass.hlsl"
            

            ENDHLSL
        }
        
        Pass
        {
            Name "DepthOnly"
            
            Tags {"LightMode" = "DepthOnly"}
            Cull [_Culling]
            ZWrite On
            ZTest LEqual
            ZClip Off
            
            
            HLSLPROGRAM

            #pragma shader_feature_local_fragment _ALPHATEST_ON
            #pragma multi_compile_instancing
            #pragma multi_compile _ DOTS_INSTANCING_ON
            #pragma multi_compile_fragment _ LOD_FADE_CROSSFADE
            
            #pragma vertex Vert
            #pragma fragment FragmentDepthOnly
            
            #include "UltimateLitPass.hlsl"
            
            ENDHLSL
        }
        
        Pass
        {
            Name "DepthNormalsOnly"
            
            Tags {"LightMode" = "DepthNormalsOnly"}
            Cull [_Culling]
            ZWrite On
            ZTest LEqual
            ZClip Off
            
            HLSLPROGRAM

            #pragma shader_feature_local_fragment _ALPHATEST_ON
            #pragma multi_compile_instancing
            #pragma multi_compile _ DOTS_INSTANCING_ON
            #pragma multi_compile_fragment _ LOD_FADE_CROSSFADE
            
            #pragma vertex Vert
            #pragma fragment FragmentDepthNormalsOnly
            
            #include "UltimateLitPass.hlsl"
            
            ENDHLSL
        }
        
    }
    CustomEditor "OccaSoftware.UltimateLitShader.Editor.LitMaterialEditorGUI"
}
