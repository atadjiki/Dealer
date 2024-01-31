// Made with Amplify Shader Editor v1.9.1.8
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Distant Lands/Illustrate/Opaque"
{
	Properties
	{
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		[HideInInspector]_SnowTexture("Snow Texture", 2D) = "white" {}
		_PuddleScale("Puddle Scale", Float) = 0.4
		_SnowScale("Snow Scale", Float) = 10
		_PuddleColor("Puddle Color", Color) = (0.1947566,0.3448693,0.4339623,0)
		[HDR]_SnowColor("Snow Color", Color) = (1,1,1,0)
		_UseCOZYPrecipitation("Use COZY Precipitation", Float) = 0
		[Header(Main Color Settings)]_Texture("Main Texture", 2D) = "white" {}
		_MainColor("Main Color", Color) = (1,1,1,1)
		[Enum(Local Space,0,World Space,1)]_TriplanarSpace("Triplanar Space", Float) = 1
		[Toggle]_ClipTriplanar("Clip Triplanar", Float) = 1
		_TriplanarDirection("TriplanarDirection", Vector) = (0,1,0,0)
		_TriplanarColor("Triplanar Color", Color) = (1,1,1,0)
		_TriplanarMultiplier("Triplanar Multiplier", Float) = 2
		_TriplanarOffset("Triplanar Offset", Float) = 0
		_TriplanarTexture("Triplanar Texture", 2D) = "white" {}
		_UseTriplanar("Use Triplanar", Float) = 0
		[Enum(World Position,0,Object Position,1,Vertex Position,2,UV Space,3)]_VariationSource("Variation Source", Float) = 0
		[Toggle]_UseHSVVariation("Use HSV Variation", Float) = 0
		_VariationScale("Variation Scale", Float) = 1
		_SaturationVariation("Saturation Variation", Range( 0 , 1)) = 0
		_ValueVariation("Value Variation", Range( 0 , 1)) = 0
		_HueVariation("Hue Variation", Range( 0 , 1)) = 0
		_WaveOffset("Wave Offset", Float) = 0
		_WaveAmount("Wave Amount", Float) = 0
		[IntRange]_WaveFramerate("Wave Framerate", Range( 0 , 60)) = 10
		_WaveSpeed("Wave Speed", Range( 0 , 10)) = 3.004473
		_WaveNoiseScale("Wave Noise Scale", Range( 0 , 10)) = 0.5
		[Enum(Distance From Center,0,Position Axis,1,Vertex Colors,2,Uniform,3)]_WaveSource("Wave Source", Float) = 3
		_WaveInfluenceDirection("Wave Influence Direction", Vector) = (0,1,0,0)
		_WaveDirection1("Wave Direction 1", Vector) = (0,1,0,0)
		_WaveDirection2("Wave Direction 2", Vector) = (0,1,0,0)
		_WaveMask("Wave Mask", Vector) = (1,0,0,0)
		_WaveSensitivity("Wave Sensitivity", Float) = 1
		[Toggle]_UseWave("UseWave", Float) = 0
		[Header(Glint)][Toggle]_UseGlint("Use Glint", Float) = 0
		[Toggle]_MultiplyByLightRatio("Multiply By Light Ratio", Float) = 0
		_GlintTexture("Glint Texture", 2D) = "black" {}
		_GlintScale("Glint Scale", Range( 0.1 , 5)) = 0.4
		_GlintColor("Glint Color", Color) = (1,1,1,1)
		[Header(Gradient Shading)][Toggle]_UseGradientShading("Use Gradient Shading", Float) = 0
		[Enum(Distance From Center,0,Position Axis,1,Vertex Colors,2)]_GradientSource("Gradient Source", Float) = 1
		[Enum(World Space,0,Local Space,1)]_Space("Space", Float) = 1
		_GradientPositionalOffset("Gradient Positional Offset", Vector) = (0,0,0,0)
		[HDR]_NearColor("Near Color", Color) = (1,1,1,1)
		[HDR]_FarColor("Far Color", Color) = (0,0,0,1)
		_GradientOffset("Gradient Offset", Float) = 0
		_GradientSensitivity("Gradient Sensitivity", Float) = 1
		_GradientChannelMask("Gradient Channel Mask", Vector) = (0,1,0,0)
		[Enum(Two Sided,0,Cull Front,1,Cull Back,2)]_CullMode("Cull Mode", Float) = 2
		[Header(Normals)][Enum(World Normals,0,Spherize Normals,1,Custom Normal Vector,2,Align With View Direction,3,Smooth Normals,4)]_NormalMode("Normal Mode", Float) = 0
		_NormalMap("Normal Map", 2D) = "bump" {}
		_BlendStrength("Blend Strength", Range( 0 , 1)) = 0.5
		_CustomNormalEllipseSize("Custom Normal Ellipse Size", Vector) = (1,1,1,0)
		[DistantLands.Soften.ShowAsVector3Field]_CustomNormalDirection("Custom Normal Direction", Vector) = (0,1,0,0)
		[Header(Posterize Colors)][Toggle]_PosterizeColors("Posterize Colors", Float) = 0
		[IntRange]_ColorNumbers("Color Numbers", Range( 1 , 20)) = 4
		[Header(Color Adjustment)][Toggle]_UseColorAdjustments("Use Color Adjustments", Float) = 0
		[Toggle]_ClampAdjustments("Clamp Adjustments", Float) = 1
		_SaturationShift("Saturation Shift", Range( -1 , 1)) = 0
		_ValueShift("Value Shift", Range( -1 , 1)) = 0
		_HueShift("Hue Shift", Range( -1 , 1)) = 0
		[Header(Lighting)][Enum(Unlit,0,Lit,1)]_LightingMode("Lighting Mode", Float) = 1
		_LightRampOffset("Light Ramp Offset", Range( 0 , 1)) = 0.5
		_LightRamp("Light Ramp", Range( 0 , 2)) = 1
		[HDR]_LightColor("Light Color", Color) = (1,1,1,1)
		[HDR]_ShadowColor("Shadow Color", Color) = (0.5188679,0.5188679,0.5188679,1)
		[Toggle]_PosterizeLight("Posterize Light", Float) = 0
		[IntRange]_LightSteps("Light Steps", Range( 0 , 20)) = 5
		_AdditionalLightRamp("Additional Light Ramp", Range( 0 , 1)) = 1
		[Toggle]_MultiplyByLightColor("Multiply By Light Color", Float) = 1
		[Toggle]_UseSpecular("UseSpecular", Float) = 1
		[HDR]_SpecularColor("Specular Color", Color) = (1,1,1,1)
		_SpecularRamp("Specular Ramp", Range( 0 , 2)) = 0
		_SpecularRampOffset("Specular Ramp Offset", Range( 0 , 1)) = 0.93
		[Toggle]_UseRimLighting("UseRimLighting", Float) = 0
		[HDR]_RimLightColor("Rim Light Color", Color) = (0.3921569,0.3921569,0.3921569,1)
		_RimRamp("Rim Ramp", Range( 0 , 2)) = 1
		_HalftoneScale("Halftone Scale", Range( 0 , 5)) = 1
		_RimLightRampOffset("Rim Light Ramp Offset", Range( 0 , 1)) = 0.5
		_HalftoneMultiplier("Halftone Multiplier", Range( 0 , 2)) = 0.7961575
		_HalftoneOffset("Halftone Offset", Range( -1 , 1)) = 0
		_RimLightShadowIntensity("Rim Light Shadow Intensity", Range( 0 , 1)) = 1
		_RimLightLitIntensity("Rim Light Lit Intensity", Range( 0 , 1)) = 1
		[Toggle]_UseShadows("Use Shadows", Float) = 1
		[Toggle]_UseModifiedNormals("Use Modified Normals", Float) = 0
		[Toggle]_UseHalftone("UseHalftone", Float) = 0
		[Header(Emission Settings)][Toggle]_UseEmission("Use Emission", Float) = 0
		[NoScaleOffset]_EmissionTexture("Emission Texture", 2D) = "white" {}
		[Enum(UV Space,0,Screen Space,1)]_EmissionUVSource("Emission UV Source", Float) = 0
		_EmissionEffectScale("Emission Effect Scale", Float) = 1
		[HDR]_EmissionColor("Emission Color", Color) = (0,0,0,0)
		_EmissionLightRatio("Emission Light Ratio", Range( 0 , 1)) = 1
		_EmissionShadowRatio("Emission Shadow Ratio", Range( 0 , 1)) = 1
		[Toggle]_ScrollEmission("Scroll Emission", Float) = 0
		_EmissionScrolling1("Emission Scrolling 1", Vector) = (1,1,0,0)
		_EmissionScrolling2("Emission Scrolling 2", Vector) = (0.1,-0.5,0,0)
		[Toggle]_UseScreenNoise("Use Screen Noise", Float) = 0
		[Header(Noise Texture Settings)][NoScaleOffset]_ScreenNoiseTexture("Screen Noise Texture", 2D) = "white" {}
		_NoiseScale("Noise Scale", Float) = 2
		[Enum(UV Space,0,Screen Space,1)]_NoiseUVSource("Noise UV Source", Float) = 0
		_NoiseAmountLight("Noise Amount Light", Range( 0 , 1)) = 0
		_NoiseAmountShadow("Noise Amount Shadow", Range( 0 , 1)) = 0.5
		_NoiseOffset("Noise Offset", Range( 0 , 1)) = 0
		[IntRange]_NoiseFramerate("Noise Framerate", Range( 0 , 60)) = 8
		_SwayOffset("Sway Offset", Float) = 0
		_SwayAmount("Sway Amount", Float) = 0
		[IntRange]_SwayFramerate("Sway Framerate", Range( 0 , 60)) = 10
		_SwaySpeed("Sway Speed", Range( 0 , 10)) = 1
		_SwayNoiseScale("Sway Noise Scale", Range( 0 , 10)) = 1
		[Enum(Distance From Center,0,Position Axis,1,Vertex Colors,2,Uniform,3)]_SwaySource("Sway Source", Float) = 3
		_SwayDirection("Sway Direction", Vector) = (0,1,0,0)
		_SwayMask("Sway Mask", Vector) = (1,0,0,0)
		_SwaySensitivity("Sway Sensitivity", Float) = 1
		[Toggle]_UseSway("UseSway", Float) = 0
		_FlutterOffset("Flutter Offset", Float) = 0
		_FlutterAmount("Flutter Amount", Float) = 0
		[IntRange]_FlutterFramerate("Flutter Framerate", Range( 0 , 60)) = 10
		[Enum(Distance From Center,0,Position Axis,1,Vertex Colors,2,Uniform,3)]_FlutterSource("Flutter Source", Float) = 3
		_FlutterSpeed("Flutter Speed", Range( 0 , 10)) = 1
		_FlutterNoiseScale("Flutter Noise Scale", Range( 0 , 10)) = 1
		_FlutterDirection("Flutter Direction", Vector) = (0,1,0,0)
		_FlutterMask("Flutter Mask", Vector) = (1,0,0,0)
		_FlutterSensitivity("Flutter Sensitivity", Float) = 1
		[Toggle]_UseFlutter("UseFlutter", Float) = 0
		_SwirlOffset("Swirl Offset", Float) = 0
		_SwirlAmount("Swirl Amount", Float) = 0
		[IntRange]_SwirlFramerate("Swirl Framerate", Range( 0 , 60)) = 10
		[Enum(Distance From Center,0,Position Axis,1,Vertex Colors,2,Uniform,3)]_SwirlSource("Swirl Source", Float) = 3
		_SwirlSpeed("Swirl Speed", Range( 0 , 10)) = 1
		_SwirlNoiseScale("Swirl Noise Scale", Range( 0 , 10)) = 1
		_SwirlDirection("Swirl Direction", Vector) = (0,1,0,0)
		_SwirlMask("Swirl Mask", Vector) = (1,0,0,0)
		_SwirlSensitivity("Swirl Sensitivity", Float) = 1
		[ASEEnd][Toggle]_UseSwirl("UseSwirl", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}


		//_TessPhongStrength( "Tess Phong Strength", Range( 0, 1 ) ) = 0.5
		//_TessValue( "Tess Max Tessellation", Range( 1, 32 ) ) = 16
		//_TessMin( "Tess Min Distance", Float ) = 10
		//_TessMax( "Tess Max Distance", Float ) = 25
		//_TessEdgeLength ( "Tess Edge length", Range( 2, 50 ) ) = 16
		//_TessMaxDisp( "Tess Max Displacement", Float ) = 25

		[HideInInspector] _QueueOffset("_QueueOffset", Float) = 0
        [HideInInspector] _QueueControl("_QueueControl", Float) = -1

        [HideInInspector][NoScaleOffset] unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset] unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset] unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
	}

	SubShader
	{
		LOD 0

		

		Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Opaque" "Queue"="Geometry" "UniversalMaterialType"="Unlit" }

		Cull [_CullMode]
		AlphaToMask Off

		

		HLSLINCLUDE
		#pragma target 4.5
		#pragma prefer_hlslcc gles
		// ensure rendering platforms toggle list is visible

		#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
		#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Filtering.hlsl"

		#ifndef ASE_TESS_FUNCS
		#define ASE_TESS_FUNCS
		float4 FixedTess( float tessValue )
		{
			return tessValue;
		}

		float CalcDistanceTessFactor (float4 vertex, float minDist, float maxDist, float tess, float4x4 o2w, float3 cameraPos )
		{
			float3 wpos = mul(o2w,vertex).xyz;
			float dist = distance (wpos, cameraPos);
			float f = clamp(1.0 - (dist - minDist) / (maxDist - minDist), 0.01, 1.0) * tess;
			return f;
		}

		float4 CalcTriEdgeTessFactors (float3 triVertexFactors)
		{
			float4 tess;
			tess.x = 0.5 * (triVertexFactors.y + triVertexFactors.z);
			tess.y = 0.5 * (triVertexFactors.x + triVertexFactors.z);
			tess.z = 0.5 * (triVertexFactors.x + triVertexFactors.y);
			tess.w = (triVertexFactors.x + triVertexFactors.y + triVertexFactors.z) / 3.0f;
			return tess;
		}

		float CalcEdgeTessFactor (float3 wpos0, float3 wpos1, float edgeLen, float3 cameraPos, float4 scParams )
		{
			float dist = distance (0.5 * (wpos0+wpos1), cameraPos);
			float len = distance(wpos0, wpos1);
			float f = max(len * scParams.y / (edgeLen * dist), 1.0);
			return f;
		}

		float DistanceFromPlane (float3 pos, float4 plane)
		{
			float d = dot (float4(pos,1.0f), plane);
			return d;
		}

		bool WorldViewFrustumCull (float3 wpos0, float3 wpos1, float3 wpos2, float cullEps, float4 planes[6] )
		{
			float4 planeTest;
			planeTest.x = (( DistanceFromPlane(wpos0, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[0]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.y = (( DistanceFromPlane(wpos0, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[1]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.z = (( DistanceFromPlane(wpos0, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[2]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.w = (( DistanceFromPlane(wpos0, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[3]) > -cullEps) ? 1.0f : 0.0f );
			return !all (planeTest);
		}

		float4 DistanceBasedTess( float4 v0, float4 v1, float4 v2, float tess, float minDist, float maxDist, float4x4 o2w, float3 cameraPos )
		{
			float3 f;
			f.x = CalcDistanceTessFactor (v0,minDist,maxDist,tess,o2w,cameraPos);
			f.y = CalcDistanceTessFactor (v1,minDist,maxDist,tess,o2w,cameraPos);
			f.z = CalcDistanceTessFactor (v2,minDist,maxDist,tess,o2w,cameraPos);

			return CalcTriEdgeTessFactors (f);
		}

		float4 EdgeLengthBasedTess( float4 v0, float4 v1, float4 v2, float edgeLength, float4x4 o2w, float3 cameraPos, float4 scParams )
		{
			float3 pos0 = mul(o2w,v0).xyz;
			float3 pos1 = mul(o2w,v1).xyz;
			float3 pos2 = mul(o2w,v2).xyz;
			float4 tess;
			tess.x = CalcEdgeTessFactor (pos1, pos2, edgeLength, cameraPos, scParams);
			tess.y = CalcEdgeTessFactor (pos2, pos0, edgeLength, cameraPos, scParams);
			tess.z = CalcEdgeTessFactor (pos0, pos1, edgeLength, cameraPos, scParams);
			tess.w = (tess.x + tess.y + tess.z) / 3.0f;
			return tess;
		}

		float4 EdgeLengthBasedTessCull( float4 v0, float4 v1, float4 v2, float edgeLength, float maxDisplacement, float4x4 o2w, float3 cameraPos, float4 scParams, float4 planes[6] )
		{
			float3 pos0 = mul(o2w,v0).xyz;
			float3 pos1 = mul(o2w,v1).xyz;
			float3 pos2 = mul(o2w,v2).xyz;
			float4 tess;

			if (WorldViewFrustumCull(pos0, pos1, pos2, maxDisplacement, planes))
			{
				tess = 0.0f;
			}
			else
			{
				tess.x = CalcEdgeTessFactor (pos1, pos2, edgeLength, cameraPos, scParams);
				tess.y = CalcEdgeTessFactor (pos2, pos0, edgeLength, cameraPos, scParams);
				tess.z = CalcEdgeTessFactor (pos0, pos1, edgeLength, cameraPos, scParams);
				tess.w = (tess.x + tess.y + tess.z) / 3.0f;
			}
			return tess;
		}
		#endif //ASE_TESS_FUNCS
		ENDHLSL

		
		Pass
		{
			
			Name "Forward"
			Tags { "LightMode"="UniversalForwardOnly" }

			Blend One Zero, One Zero
			ZWrite On
			ZTest LEqual
			Offset 0 , 0
			ColorMask RGBA

			

			HLSLPROGRAM

			#define ASE_SRP_VERSION 140003


			#pragma instancing_options renderinglayer

			#pragma multi_compile _ LIGHTMAP_ON
			#pragma multi_compile _ DIRLIGHTMAP_COMBINED
			#pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
        	#pragma multi_compile_fragment _ DEBUG_DISPLAY
        	#pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
        	#pragma multi_compile_fragment _ _WRITE_RENDERING_LAYERS

			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS SHADERPASS_UNLIT

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Debug/Debugging3D.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceData.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/LODCrossFade.hlsl"

			#define ASE_NEEDS_VERT_POSITION
			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#define ASE_NEEDS_VERT_NORMAL
			#define ASE_NEEDS_FRAG_POSITION
			#define ASE_NEEDS_FRAG_SHADOWCOORDS
			#pragma multi_compile_instancing
			#pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ _SHADOWS_SOFT
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
			#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
			#pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
			#pragma multi_compile_fragment _ _SHADOWS_SOFT
			#pragma multi_compile _ _FORWARD_PLUS


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_color : COLOR;
				uint ase_vertexID : SV_VertexID;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_tangent : TANGENT;
				float4 texcoord1 : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
					float3 worldPos : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					float4 shadowCoord : TEXCOORD1;
				#endif
				#ifdef ASE_FOG
					float fogFactor : TEXCOORD2;
				#endif
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_color : COLOR;
				float4 ase_texcoord4 : TEXCOORD4;
				float4 ase_texcoord5 : TEXCOORD5;
				float4 ase_texcoord6 : TEXCOORD6;
				float4 ase_texcoord7 : TEXCOORD7;
				float4 lightmapUVOrVertexSH : TEXCOORD8;
				float4 ase_texcoord9 : TEXCOORD9;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _TriplanarColor;
			float4 _NearColor;
			float4 _LightColor;
			float4 _ShadowColor;
			float4 _MainColor;
			float4 _GlintColor;
			float4 _RimLightColor;
			float4 _EmissionColor;
			float4 _SnowColor;
			float4 _FarColor;
			float4 _SpecularColor;
			float4 _PuddleColor;
			float3 _SwirlDirection;
			float3 _GradientChannelMask;
			float3 _SwayMask;
			float3 _FlutterMask;
			float3 _WaveInfluenceDirection;
			float3 _FlutterDirection;
			float3 _CustomNormalEllipseSize;
			float3 _CustomNormalDirection;
			float3 _WaveDirection2;
			float3 _SwayDirection;
			float3 _SwirlMask;
			float3 _WaveDirection1;
			float3 _TriplanarDirection;
			float3 _GradientPositionalOffset;
			float3 _WaveMask;
			float2 _EmissionScrolling1;
			float2 _EmissionScrolling2;
			float _LightRamp;
			float _UseShadows;
			float _PosterizeLight;
			float _UseFlutter;
			float _UseHalftone;
			float _MultiplyByLightColor;
			float _LightingMode;
			float _SnowScale;
			float _PuddleScale;
			float _ColorNumbers;
			float _ValueVariation;
			float _SaturationVariation;
			float _LightRampOffset;
			float _LightSteps;
			float _SpecularRampOffset;
			float _HalftoneOffset;
			float _NoiseScale;
			float _NoiseUVSource;
			float _NoiseAmountLight;
			float _NoiseAmountShadow;
			float _UseScreenNoise;
			float _EmissionEffectScale;
			float _EmissionUVSource;
			float _ScrollEmission;
			float _EmissionLightRatio;
			float _EmissionShadowRatio;
			float _UseEmission;
			float _GlintScale;
			float _MultiplyByLightRatio;
			float _UseGlint;
			float _UseRimLighting;
			float _RimLightLitIntensity;
			float _RimLightShadowIntensity;
			float _RimRamp;
			float _RimLightRampOffset;
			float _UseSpecular;
			float _UseModifiedNormals;
			float _SpecularRamp;
			float _HueVariation;
			float _AdditionalLightRamp;
			float _HalftoneMultiplier;
			float _HalftoneScale;
			float _UseHSVVariation;
			float _TriplanarSpace;
			float _VariationSource;
			float _SwaySpeed;
			float _SwayFramerate;
			float _SwayNoiseScale;
			float _SwayAmount;
			float _SwaySensitivity;
			float _SwayOffset;
			float _SwaySource;
			float _UseSway;
			float _SwirlSpeed;
			float _SwirlFramerate;
			float _SwirlNoiseScale;
			float _SwirlAmount;
			float _SwirlSensitivity;
			float _SwirlOffset;
			float _SwirlSource;
			float _UseSwirl;
			float _FlutterSpeed;
			float _FlutterFramerate;
			float _FlutterNoiseScale;
			float _FlutterAmount;
			float _FlutterSensitivity;
			float _FlutterOffset;
			float _FlutterSource;
			float _UseWave;
			float _VariationScale;
			float _WaveSource;
			float _WaveSensitivity;
			float _ValueShift;
			float _SaturationShift;
			float _HueShift;
			float _ClipTriplanar;
			float _TriplanarOffset;
			float _TriplanarMultiplier;
			float _NoiseFramerate;
			float _BlendStrength;
			float _NormalMode;
			float _GradientSensitivity;
			float _Space;
			float _GradientOffset;
			float _GradientSource;
			float _UseGradientShading;
			float _UseTriplanar;
			float _ClampAdjustments;
			float _UseColorAdjustments;
			float _PosterizeColors;
			float _UseCOZYPrecipitation;
			float _WaveSpeed;
			float _WaveFramerate;
			float _WaveNoiseScale;
			float _WaveAmount;
			float _WaveOffset;
			float _NoiseOffset;
			#ifdef ASE_TESSELLATION
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END

			sampler2D _Texture;
			sampler2D _TriplanarTexture;
			sampler2D _NormalMap;
			float CZY_WetnessAmount;
			sampler2D _SnowTexture;
			float CZY_SnowAmount;
			sampler2D _GlintTexture;
			sampler2D _EmissionTexture;
			sampler2D _ScreenNoiseTexture;
			UNITY_INSTANCING_BUFFER_START(DistantLandsIllustrateOpaque)
				UNITY_DEFINE_INSTANCED_PROP(float4, _Texture_ST)
				UNITY_DEFINE_INSTANCED_PROP(float4, _NormalMap_ST)
				UNITY_DEFINE_INSTANCED_PROP(float4, _SnowTexture_ST)
				UNITY_DEFINE_INSTANCED_PROP(float, _CullMode)
			UNITY_INSTANCING_BUFFER_END(DistantLandsIllustrateOpaque)


			float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }
			float snoise( float2 v )
			{
				const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
				float2 i = floor( v + dot( v, C.yy ) );
				float2 x0 = v - i + dot( i, C.xx );
				float2 i1;
				i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
				float4 x12 = x0.xyxy + C.xxzz;
				x12.xy -= i1;
				i = mod2D289( i );
				float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
				float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
				m = m * m;
				m = m * m;
				float3 x = 2.0 * frac( p * C.www ) - 1.0;
				float3 h = abs( x ) - 0.5;
				float3 ox = floor( x + 0.5 );
				float3 a0 = x - ox;
				m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
				float3 g;
				g.x = a0.x * x0.x + h.x * x0.y;
				g.yz = a0.yz * x12.xz + h.yz * x12.yw;
				return 130.0 * dot( m, g );
			}
			
			float3 RotateAroundAxis( float3 center, float3 original, float3 u, float angle )
			{
				original -= center;
				float C = cos( angle );
				float S = sin( angle );
				float t = 1 - C;
				float m00 = t * u.x * u.x + C;
				float m01 = t * u.x * u.y - S * u.z;
				float m02 = t * u.x * u.z + S * u.y;
				float m10 = t * u.x * u.y + S * u.z;
				float m11 = t * u.y * u.y + C;
				float m12 = t * u.y * u.z - S * u.x;
				float m20 = t * u.x * u.z - S * u.y;
				float m21 = t * u.y * u.z + S * u.x;
				float m22 = t * u.z * u.z + C;
				float3x3 finalMatrix = float3x3( m00, m01, m02, m10, m11, m12, m20, m21, m22 );
				return mul( finalMatrix, original ) + center;
			}
			
			float3 HSVToRGB( float3 c )
			{
				float4 K = float4( 1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0 );
				float3 p = abs( frac( c.xxx + K.xyz ) * 6.0 - K.www );
				return c.z * lerp( K.xxx, saturate( p - K.xxx ), c.y );
			}
			
			float3 RGBToHSV(float3 c)
			{
				float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
				float4 p = lerp( float4( c.bg, K.wz ), float4( c.gb, K.xy ), step( c.b, c.g ) );
				float4 q = lerp( float4( p.xyw, c.r ), float4( c.r, p.yzx ), step( p.x, c.r ) );
				float d = q.x - min( q.w, q.y );
				float e = 1.0e-10;
				return float3( abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
			}
					float2 voronoihash31_g1212( float2 p )
					{
						
						p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
						return frac( sin( p ) *43758.5453);
					}
			
					float voronoi31_g1212( float2 v, float time, inout float2 id, inout float2 mr, float smoothness, inout float2 smoothId )
					{
						float2 n = floor( v );
						float2 f = frac( v );
						float F1 = 8.0;
						float F2 = 8.0; float2 mg = 0;
						for ( int j = -1; j <= 1; j++ )
						{
							for ( int i = -1; i <= 1; i++ )
						 	{
						 		float2 g = float2( i, j );
						 		float2 o = voronoihash31_g1212( n + g );
								o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
								float d = 0.5 * dot( r, r );
						 		if( d<F1 ) {
						 			F2 = F1;
						 			F1 = d; mg = g; mr = r; id = o;
						 		} else if( d<F2 ) {
						 			F2 = d;
						
						 		}
						 	}
						}
						return (F2 + F1) * 0.5;
					}
			
					float2 voronoihash39_g1212( float2 p )
					{
						
						p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
						return frac( sin( p ) *43758.5453);
					}
			
					float voronoi39_g1212( float2 v, float time, inout float2 id, inout float2 mr, float smoothness, inout float2 smoothId )
					{
						float2 n = floor( v );
						float2 f = frac( v );
						float F1 = 8.0;
						float F2 = 8.0; float2 mg = 0;
						for ( int j = -1; j <= 1; j++ )
						{
							for ( int i = -1; i <= 1; i++ )
						 	{
						 		float2 g = float2( i, j );
						 		float2 o = voronoihash39_g1212( n + g );
								o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
								float d = 0.5 * dot( r, r );
						 		if( d<F1 ) {
						 			F2 = F1;
						 			F1 = d; mg = g; mr = r; id = o;
						 		} else if( d<F2 ) {
						 			F2 = d;
						
						 		}
						 	}
						}
						return F1;
					}
			
					float2 voronoihash19_g1212( float2 p )
					{
						
						p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
						return frac( sin( p ) *43758.5453);
					}
			
					float voronoi19_g1212( float2 v, float time, inout float2 id, inout float2 mr, float smoothness, inout float2 smoothId )
					{
						float2 n = floor( v );
						float2 f = frac( v );
						float F1 = 8.0;
						float F2 = 8.0; float2 mg = 0;
						for ( int j = -1; j <= 1; j++ )
						{
							for ( int i = -1; i <= 1; i++ )
						 	{
						 		float2 g = float2( i, j );
						 		float2 o = voronoihash19_g1212( n + g );
								o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
								float d = 0.5 * dot( r, r );
						 		if( d<F1 ) {
						 			F2 = F1;
						 			F1 = d; mg = g; mr = r; id = o;
						 		} else if( d<F2 ) {
						 			F2 = d;
						
						 		}
						 	}
						}
						return F1;
					}
			
			float3 ASEIndirectDiffuse( float2 uvStaticLightmap, float3 normalWS )
			{
			#ifdef LIGHTMAP_ON
				return SampleLightmap( uvStaticLightmap, normalWS );
			#else
				return SampleSH(normalWS);
			#endif
			}
			
					float2 voronoihash288_g1189( float2 p )
					{
						p = p - 1 * floor( p / 1 );
						p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
						return frac( sin( p ) *43758.5453);
					}
			
					float voronoi288_g1189( float2 v, float time, inout float2 id, inout float2 mr, float smoothness, inout float2 smoothId )
					{
						float2 n = floor( v );
						float2 f = frac( v );
						float F1 = 8.0;
						float F2 = 8.0; float2 mg = 0;
						for ( int j = -1; j <= 1; j++ )
						{
							for ( int i = -1; i <= 1; i++ )
						 	{
						 		float2 g = float2( i, j );
						 		float2 o = voronoihash288_g1189( n + g );
								o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
								float d = 0.5 * dot( r, r );
						 		if( d<F1 ) {
						 			F2 = F1;
						 			F1 = d; mg = g; mr = r; id = o;
						 		} else if( d<F2 ) {
						 			F2 = d;
						
						 		}
						 	}
						}
						return F1;
					}
			
			half4 CalculateShadowMask1_g1192( half2 LightmapUV )
			{
				#if defined(SHADOWS_SHADOWMASK) && defined(LIGHTMAP_ON)
				return SAMPLE_SHADOWMASK( LightmapUV.xy );
				#elif !defined (LIGHTMAP_ON)
				return unity_ProbesOcclusion;
				#else
				return half4( 1, 1, 1, 1 );
				#endif
			}
			
			float3 AdditionalLightsFlatMask14x( float3 WorldPosition, float2 ScreenUV, float4 ShadowMask, float Ramp )
			{
				float3 Color = 0;
				#if defined(_ADDITIONAL_LIGHTS)
					#define SUM_LIGHT(Light)\
						Color += Light.color * smoothstep(0, Ramp,  ( Light.distanceAttenuation * Light.shadowAttenuation ) );
					InputData inputData = (InputData)0;
					inputData.normalizedScreenSpaceUV = ScreenUV;
					inputData.positionWS = WorldPosition;
					uint meshRenderingLayers = GetMeshRenderingLayer();
					uint pixelLightCount = GetAdditionalLightsCount();	
					#if USE_FORWARD_PLUS
					for (uint lightIndex = 0; lightIndex < min(URP_FP_DIRECTIONAL_LIGHTS_COUNT, MAX_VISIBLE_LIGHTS); lightIndex++)
					{
						FORWARD_PLUS_SUBTRACTIVE_LIGHT_CHECK
						Light light = GetAdditionalLight(lightIndex, WorldPosition, ShadowMask);
						#ifdef _LIGHT_LAYERS
						if (IsMatchingLightLayer(light.layerMask, meshRenderingLayers))
						#endif
						{
							SUM_LIGHT( light );
						}
					}
					#endif
					LIGHT_LOOP_BEGIN( pixelLightCount )
						Light light = GetAdditionalLight(lightIndex, WorldPosition, ShadowMask);
						#ifdef _LIGHT_LAYERS
						if (IsMatchingLightLayer(light.layerMask, meshRenderingLayers))
						#endif
						{
							SUM_LIGHT( light );
						}
					LIGHT_LOOP_END
				#endif
				return Color;
			}
			

			VertexOutput VertexFunction ( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float temp_output_21_0_g1173 = _FlutterSource;
				float3 temp_output_23_0_g1173 = v.vertex.xyz;
				float temp_output_1_0_g1173 = distance( ( -_FlutterOffset + temp_output_23_0_g1173 ) , float3( 0,0,0 ) );
				float3 temp_output_22_0_g1173 = _FlutterMask;
				float temp_output_31_0_g1173 = length( ( ( -_FlutterOffset + temp_output_23_0_g1173 ) * temp_output_22_0_g1173 ) );
				float temp_output_30_0_g1173 = length( ( float4( temp_output_22_0_g1173 , 0.0 ) * v.ase_color ) );
				float temp_output_2_0_g1172 = _FlutterFramerate;
				float2 temp_cast_1 = (v.ase_vertexID*( _FlutterNoiseScale * 0.1 ) + ( ( round( ( _TimeParameters.x * temp_output_2_0_g1172 ) ) / temp_output_2_0_g1172 ) * 0.1 * _FlutterSpeed ));
				float simplePerlin2D12_g1171 = snoise( temp_cast_1*8.91 );
				float3 worldToObjDir34_g1171 = normalize( mul( GetWorldToObjectMatrix(), float4( _FlutterDirection, 0 ) ).xyz );
				float temp_output_21_0_g1170 = _SwirlSource;
				float3 temp_output_23_0_g1170 = v.vertex.xyz;
				float temp_output_1_0_g1170 = distance( ( -_SwirlOffset + temp_output_23_0_g1170 ) , float3( 0,0,0 ) );
				float3 temp_output_22_0_g1170 = _SwirlMask;
				float temp_output_31_0_g1170 = length( ( ( -_SwirlOffset + temp_output_23_0_g1170 ) * temp_output_22_0_g1170 ) );
				float temp_output_30_0_g1170 = length( ( float4( temp_output_22_0_g1170 , 0.0 ) * v.ase_color ) );
				float3 worldToObjDir40_g1168 = normalize( mul( GetWorldToObjectMatrix(), float4( _SwirlDirection, 0 ) ).xyz );
				float temp_output_2_0_g1169 = _SwirlFramerate;
				float simplePerlin2D12_g1168 = snoise( (v.vertex.xyz*( _SwirlNoiseScale * 0.1 ) + ( ( round( ( _TimeParameters.x * temp_output_2_0_g1169 ) ) / temp_output_2_0_g1169 ) * 0.1 * _SwirlSpeed )).xy*8.91 );
				float3 rotatedValue35_g1168 = RotateAroundAxis( float3( 0,0,0 ), v.vertex.xyz, worldToObjDir40_g1168, ( _SwirlAmount * 0.01 * simplePerlin2D12_g1168 ) );
				float temp_output_21_0_g1176 = _SwaySource;
				float3 temp_output_23_0_g1176 = v.vertex.xyz;
				float temp_output_1_0_g1176 = distance( ( -_SwayOffset + temp_output_23_0_g1176 ) , float3( 0,0,0 ) );
				float3 temp_output_22_0_g1176 = _SwayMask;
				float temp_output_31_0_g1176 = length( ( ( -_SwayOffset + temp_output_23_0_g1176 ) * temp_output_22_0_g1176 ) );
				float temp_output_30_0_g1176 = length( ( float4( temp_output_22_0_g1176 , 0.0 ) * v.ase_color ) );
				float temp_output_2_0_g1175 = _SwayFramerate;
				float simplePerlin2D5_g1174 = snoise( (v.vertex.xyz*( _SwayNoiseScale * 0.1 ) + ( ( round( ( _TimeParameters.x * temp_output_2_0_g1175 ) ) / temp_output_2_0_g1175 ) * 0.1 * _SwaySpeed )).xy*8.91 );
				simplePerlin2D5_g1174 = simplePerlin2D5_g1174*0.5 + 0.5;
				float3 worldToObjDir18_g1174 = normalize( mul( GetWorldToObjectMatrix(), float4( _SwayDirection, 0 ) ).xyz );
				float temp_output_21_0_g1179 = _WaveSource;
				float3 temp_output_23_0_g1179 = v.vertex.xyz;
				float temp_output_1_0_g1179 = distance( ( -_WaveOffset + temp_output_23_0_g1179 ) , float3( 0,0,0 ) );
				float3 temp_output_22_0_g1179 = _WaveMask;
				float temp_output_31_0_g1179 = length( ( ( -_WaveOffset + temp_output_23_0_g1179 ) * temp_output_22_0_g1179 ) );
				float temp_output_30_0_g1179 = length( ( float4( temp_output_22_0_g1179 , 0.0 ) * v.ase_color ) );
				float3 ase_worldPos = TransformObjectToWorld( (v.vertex).xyz );
				float3 normalizeResult37_g1177 = normalize( _WaveDirection1 );
				float3 break35_g1177 = ( ase_worldPos * normalizeResult37_g1177 );
				float temp_output_2_0_g1178 = _WaveFramerate;
				float Time40_g1177 = ( ( round( ( _TimeParameters.x * temp_output_2_0_g1178 ) ) / temp_output_2_0_g1178 ) * _WaveSpeed );
				float3 normalizeResult52_g1177 = normalize( _WaveDirection2 );
				float3 break49_g1177 = ( ase_worldPos * normalizeResult52_g1177 );
				float3 worldToObjDir18_g1177 = normalize( mul( GetWorldToObjectMatrix(), float4( _WaveInfluenceDirection, 0 ) ).xyz );
				float3 ase_objectScale = float3( length( GetObjectToWorldMatrix()[ 0 ].xyz ), length( GetObjectToWorldMatrix()[ 1 ].xyz ), length( GetObjectToWorldMatrix()[ 2 ].xyz ) );
				
				float3 ase_worldTangent = TransformObjectToWorldDir(v.ase_tangent.xyz);
				o.ase_texcoord4.xyz = ase_worldTangent;
				float3 ase_worldNormal = TransformObjectToWorldNormal(v.ase_normal);
				o.ase_texcoord5.xyz = ase_worldNormal;
				float ase_vertexTangentSign = v.ase_tangent.w * ( unity_WorldTransformParams.w >= 0.0 ? 1.0 : -1.0 );
				float3 ase_worldBitangent = cross( ase_worldNormal, ase_worldTangent ) * ase_vertexTangentSign;
				o.ase_texcoord6.xyz = ase_worldBitangent;
				OUTPUT_LIGHTMAP_UV( v.texcoord1, unity_LightmapST, o.lightmapUVOrVertexSH.xy );
				OUTPUT_SH( ase_worldNormal, o.lightmapUVOrVertexSH.xyz );
				float4 ase_clipPos = TransformObjectToHClip((v.vertex).xyz);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord9 = screenPos;
				
				o.ase_texcoord3.xy = v.ase_texcoord.xy;
				o.ase_color = v.ase_color;
				o.ase_texcoord7 = v.vertex;
				o.ase_texcoord3.zw = v.texcoord1.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord4.w = 0;
				o.ase_texcoord5.w = 0;
				o.ase_texcoord6.w = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = ( ( _UseFlutter == 1.0 ? ( saturate( ( ( ( temp_output_21_0_g1173 == 3.0 ? 1.0 : 0.0 ) + ( temp_output_21_0_g1173 == 0.0 ? temp_output_1_0_g1173 : 0.0 ) + ( temp_output_21_0_g1173 == 1.0 ? temp_output_31_0_g1173 : 0.0 ) + ( temp_output_21_0_g1173 == 2.0 ? temp_output_30_0_g1173 : 0.0 ) ) * _FlutterSensitivity ) ) * ( _FlutterAmount * float3( 0.001,0.001,0.001 ) * simplePerlin2D12_g1171 * worldToObjDir34_g1171 ) ) : float3( 0,0,0 ) ) + ( _UseSwirl == 1.0 ? ( saturate( ( ( ( temp_output_21_0_g1170 == 3.0 ? 1.0 : 0.0 ) + ( temp_output_21_0_g1170 == 0.0 ? temp_output_1_0_g1170 : 0.0 ) + ( temp_output_21_0_g1170 == 1.0 ? temp_output_31_0_g1170 : 0.0 ) + ( temp_output_21_0_g1170 == 2.0 ? temp_output_30_0_g1170 : 0.0 ) ) * _SwirlSensitivity ) ) * ( rotatedValue35_g1168 - v.vertex.xyz ) ) : float3( 0,0,0 ) ) + ( _UseSway == 1.0 ? ( saturate( ( ( ( temp_output_21_0_g1176 == 3.0 ? 1.0 : 0.0 ) + ( temp_output_21_0_g1176 == 0.0 ? temp_output_1_0_g1176 : 0.0 ) + ( temp_output_21_0_g1176 == 1.0 ? temp_output_31_0_g1176 : 0.0 ) + ( temp_output_21_0_g1176 == 2.0 ? temp_output_30_0_g1176 : 0.0 ) ) * _SwaySensitivity ) ) * ( _SwayAmount * float3( 0.01,0.01,0.01 ) * simplePerlin2D5_g1174 * worldToObjDir18_g1174 ) ) : float3( 0,0,0 ) ) + ( _UseWave == 1.0 ? ( ( saturate( ( ( ( temp_output_21_0_g1179 == 3.0 ? 1.0 : 0.0 ) + ( temp_output_21_0_g1179 == 0.0 ? temp_output_1_0_g1179 : 0.0 ) + ( temp_output_21_0_g1179 == 1.0 ? temp_output_31_0_g1179 : 0.0 ) + ( temp_output_21_0_g1179 == 2.0 ? temp_output_30_0_g1179 : 0.0 ) ) * _WaveSensitivity ) ) * ( _WaveAmount * ( sin( (( break35_g1177.x + break35_g1177.y + break35_g1177.z )*_WaveNoiseScale + Time40_g1177) ) + sin( (( break49_g1177.x + break49_g1177.y + break49_g1177.z )*( _WaveNoiseScale * 0.5 ) + ( Time40_g1177 * 0.7 )) ) ) * worldToObjDir18_g1177 ) ) / ase_objectScale ) : float3( 0,0,0 ) ) );

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				float4 positionCS = TransformWorldToHClip( positionWS );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
					o.worldPos = positionWS;
				#endif

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = positionCS;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif

				#ifdef ASE_FOG
					o.fogFactor = ComputeFogFactor( positionCS.z );
				#endif

				o.clipPos = positionCS;

				return o;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_color : COLOR;
				uint ase_vertexID : SV_VertexID;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_tangent : TANGENT;
				float4 texcoord1 : TEXCOORD1;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.vertex = v.vertex;
				o.ase_color = v.ase_color;
				o.ase_vertexID = v.ase_vertexID;
				o.ase_texcoord = v.ase_texcoord;
				o.ase_tangent = v.ase_tangent;
				o.texcoord1 = v.texcoord1;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				o.ase_vertexID = patch[0].ase_vertexID * bary.x + patch[1].ase_vertexID * bary.y + patch[2].ase_vertexID * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				o.ase_tangent = patch[0].ase_tangent * bary.x + patch[1].ase_tangent * bary.y + patch[2].ase_tangent * bary.z;
				o.texcoord1 = patch[0].texcoord1 * bary.x + patch[1].texcoord1 * bary.y + patch[2].texcoord1 * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag ( VertexOutput IN
				#ifdef _WRITE_RENDERING_LAYERS
				, out float4 outRenderingLayers : SV_Target1
				#endif
				 ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
					float3 WorldPosition = IN.worldPos;
				#endif

				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				float _CullMode_Instance = UNITY_ACCESS_INSTANCED_PROP(DistantLandsIllustrateOpaque,_CullMode);
				
				float4 _Texture_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(DistantLandsIllustrateOpaque,_Texture_ST);
				float2 uv_Texture = IN.ase_texcoord3.xy * _Texture_ST_Instance.xy + _Texture_ST_Instance.zw;
				float4 temp_output_14_0_g1204 = ( _MainColor * tex2D( _Texture, uv_Texture ) );
				float temp_output_21_0_g1205 = _GradientSource;
				float3 worldToObj9_g1204 = mul( GetWorldToObjectMatrix(), float4( WorldPosition, 1 ) ).xyz;
				float3 AdjustedPosition10_g1204 = ( ( _Space == 0.0 ? WorldPosition : worldToObj9_g1204 ) - _GradientPositionalOffset );
				float3 temp_output_23_0_g1205 = AdjustedPosition10_g1204;
				float temp_output_1_0_g1205 = distance( ( -_GradientOffset + temp_output_23_0_g1205 ) , float3( 0,0,0 ) );
				float3 temp_output_22_0_g1205 = _GradientChannelMask;
				float temp_output_31_0_g1205 = length( ( ( -_GradientOffset + temp_output_23_0_g1205 ) * temp_output_22_0_g1205 ) );
				float temp_output_30_0_g1205 = length( ( float4( temp_output_22_0_g1205 , 0.0 ) * IN.ase_color ) );
				float Distance15_g1204 = saturate( ( ( ( temp_output_21_0_g1205 == 3.0 ? 1.0 : 0.0 ) + ( temp_output_21_0_g1205 == 0.0 ? temp_output_1_0_g1205 : 0.0 ) + ( temp_output_21_0_g1205 == 1.0 ? temp_output_31_0_g1205 : 0.0 ) + ( temp_output_21_0_g1205 == 2.0 ? temp_output_30_0_g1205 : 0.0 ) ) * _GradientSensitivity ) );
				float4 lerpResult32_g1204 = lerp( _NearColor , _FarColor , Distance15_g1204);
				float4 AdjustedGradient34_g1204 = lerpResult32_g1204;
				float3 lerpResult28_g1204 = lerp( (temp_output_14_0_g1204).rgb , (AdjustedGradient34_g1204).rgb , (AdjustedGradient34_g1204).a);
				float4 appendResult57_g1204 = (float4(lerpResult28_g1204 , (temp_output_14_0_g1204).a));
				float4 temp_output_1_0_g1213 = ( _UseGradientShading == 1.0 ? appendResult57_g1204 : temp_output_14_0_g1204 );
				float2 appendResult40_g1213 = (float2(WorldPosition.x , WorldPosition.z));
				float4 Color23_g1213 = ( tex2D( _TriplanarTexture, appendResult40_g1213 ) * _TriplanarColor );
				float4 _NormalMap_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(DistantLandsIllustrateOpaque,_NormalMap_ST);
				float2 uv_NormalMap = IN.ase_texcoord3.xy * _NormalMap_ST_Instance.xy + _NormalMap_ST_Instance.zw;
				float3 ase_worldTangent = IN.ase_texcoord4.xyz;
				float3 ase_worldNormal = IN.ase_texcoord5.xyz;
				float3 ase_worldBitangent = IN.ase_texcoord6.xyz;
				float3 tanToWorld0 = float3( ase_worldTangent.x, ase_worldBitangent.x, ase_worldNormal.x );
				float3 tanToWorld1 = float3( ase_worldTangent.y, ase_worldBitangent.y, ase_worldNormal.y );
				float3 tanToWorld2 = float3( ase_worldTangent.z, ase_worldBitangent.z, ase_worldNormal.z );
				float3 tanNormal12_g1167 = UnpackNormalScale( tex2D( _NormalMap, uv_NormalMap ), 1.0f );
				float3 worldNormal12_g1167 = float3(dot(tanToWorld0,tanNormal12_g1167), dot(tanToWorld1,tanNormal12_g1167), dot(tanToWorld2,tanNormal12_g1167));
				float3 worldToObj22_g1167 = mul( GetWorldToObjectMatrix(), float4( WorldPosition, 1 ) ).xyz;
				float3 normalizeResult6_g1167 = normalize( ( worldToObj22_g1167 / _CustomNormalEllipseSize ) );
				float3 objToWorldDir37_g1167 = mul( GetObjectToWorldMatrix(), float4( normalizeResult6_g1167, 0 ) ).xyz;
				float3 objToWorldDir31_g1167 = mul( GetObjectToWorldMatrix(), float4( _CustomNormalDirection, 0 ) ).xyz;
				float3 worldSpaceViewDir40_g1167 = ( _WorldSpaceCameraPos.xyz - mul(GetObjectToWorldMatrix(), float4( 0,0,0,1 ) ).xyz );
				float3 lerpResult25_g1167 = lerp( worldNormal12_g1167 , objToWorldDir37_g1167 , _BlendStrength);
				float3 temp_output_887_0 = ( ( _NormalMode == 0.0 ? worldNormal12_g1167 : float3( 0,0,0 ) ) + ( _NormalMode == 1.0 ? objToWorldDir37_g1167 : float3( 0,0,0 ) ) + ( _NormalMode == 2.0 ? objToWorldDir31_g1167 : float3( 0,0,0 ) ) + ( _NormalMode == 3.0 ? worldSpaceViewDir40_g1167 : float3( 0,0,0 ) ) + ( _NormalMode == 4.0 ? lerpResult25_g1167 : float3( 0,0,0 ) ) );
				float3 Normals490 = temp_output_887_0;
				float3 objToWorldDir5_g1213 = mul( GetObjectToWorldMatrix(), float4( _TriplanarDirection, 0 ) ).xyz;
				float dotResult11_g1213 = dot( Normals490 , ( ( _TriplanarSpace * _TriplanarDirection ) + ( ( 1.0 - _TriplanarSpace ) * objToWorldDir5_g1213 ) ) );
				float temp_output_14_0_g1213 = saturate( (dotResult11_g1213*_TriplanarMultiplier + _TriplanarOffset) );
				float4 lerpResult17_g1213 = lerp( temp_output_1_0_g1213 , Color23_g1213 , saturate( ( ( temp_output_14_0_g1213 * ( 1.0 - _ClipTriplanar ) ) + ( _ClipTriplanar * ( temp_output_14_0_g1213 > 0.5 ? 1.0 : 0.0 ) ) ) ));
				float4 temp_output_1_0_g1194 = ( ( ( 1.0 - _UseTriplanar ) * temp_output_1_0_g1213 ) + ( _UseTriplanar * lerpResult17_g1213 ) );
				float3 hsvTorgb3_g1194 = RGBToHSV( temp_output_1_0_g1194.xyz );
				float temp_output_1_0_g1197 = hsvTorgb3_g1194.x;
				float temp_output_2_0_g1197 = abs( _HueShift );
				float temp_output_7_0_g1197 = ( temp_output_1_0_g1197 - 0.5 );
				float temp_output_1_0_g1195 = hsvTorgb3_g1194.y;
				float temp_output_2_0_g1195 = _SaturationShift;
				float temp_output_7_0_g1195 = ( temp_output_1_0_g1195 - 0.5 );
				float temp_output_1_0_g1196 = hsvTorgb3_g1194.z;
				float temp_output_2_0_g1196 = _ValueShift;
				float temp_output_7_0_g1196 = ( temp_output_1_0_g1196 - 0.5 );
				float3 hsvTorgb12_g1194 = HSVToRGB( float3(saturate( ( _ClampAdjustments == 0.0 ? ( temp_output_1_0_g1197 + temp_output_2_0_g1197 ) : ( temp_output_1_0_g1197 + ( ( ( 0.25 - ( temp_output_7_0_g1197 * temp_output_7_0_g1197 ) ) * 4.0 ) * temp_output_2_0_g1197 ) ) ) ),saturate( ( _ClampAdjustments == 0.0 ? ( temp_output_1_0_g1195 + temp_output_2_0_g1195 ) : ( temp_output_1_0_g1195 + ( ( ( 0.25 - ( temp_output_7_0_g1195 * temp_output_7_0_g1195 ) ) * 4.0 ) * temp_output_2_0_g1195 ) ) ) ),saturate( ( _ClampAdjustments == 0.0 ? ( temp_output_1_0_g1196 + temp_output_2_0_g1196 ) : ( temp_output_1_0_g1196 + ( ( ( 0.25 - ( temp_output_7_0_g1196 * temp_output_7_0_g1196 ) ) * 4.0 ) * temp_output_2_0_g1196 ) ) ) )) );
				float4 appendResult4_g1194 = (float4(saturate( hsvTorgb12_g1194 ) , (temp_output_1_0_g1194).w));
				float4 temp_output_1_0_g1198 = ( _UseColorAdjustments == 1.0 ? appendResult4_g1194 : temp_output_1_0_g1194 );
				float3 hsvTorgb3_g1198 = RGBToHSV( temp_output_1_0_g1198.xyz );
				float3 objToWorld17_g1201 = mul( GetObjectToWorldMatrix(), float4( float3( 0,0,0 ), 1 ) ).xyz;
				float2 texCoord23_g1201 = IN.ase_texcoord3.xy * float2( 1,1 ) + float2( 0,0 );
				float2 Source25_g1201 = ( ( _VariationSource == 0.0 ? (WorldPosition).xz : float2( 0,0 ) ) + ( _VariationSource == 1.0 ? (objToWorld17_g1201).xz : float2( 0,0 ) ) + ( _VariationSource == 2.0 ? (IN.ase_texcoord7.xyz).xz : float2( 0,0 ) ) + ( _VariationSource == 3.0 ? texCoord23_g1201 : float2( 0,0 ) ) );
				float simplePerlin2D4_g1201 = snoise( Source25_g1201*_VariationScale );
				float3 objToWorld17_g1199 = mul( GetObjectToWorldMatrix(), float4( float3( 0,0,0 ), 1 ) ).xyz;
				float2 texCoord23_g1199 = IN.ase_texcoord3.xy * float2( 1,1 ) + float2( 0,0 );
				float2 Source25_g1199 = ( ( _VariationSource == 0.0 ? (WorldPosition).xz : float2( 0,0 ) ) + ( _VariationSource == 1.0 ? (objToWorld17_g1199).xz : float2( 0,0 ) ) + ( _VariationSource == 2.0 ? (IN.ase_texcoord7.xyz).xz : float2( 0,0 ) ) + ( _VariationSource == 3.0 ? texCoord23_g1199 : float2( 0,0 ) ) );
				float simplePerlin2D4_g1199 = snoise( Source25_g1199*_VariationScale );
				float3 objToWorld17_g1200 = mul( GetObjectToWorldMatrix(), float4( float3( 0,0,0 ), 1 ) ).xyz;
				float2 texCoord23_g1200 = IN.ase_texcoord3.xy * float2( 1,1 ) + float2( 0,0 );
				float2 Source25_g1200 = ( ( _VariationSource == 0.0 ? (WorldPosition).xz : float2( 0,0 ) ) + ( _VariationSource == 1.0 ? (objToWorld17_g1200).xz : float2( 0,0 ) ) + ( _VariationSource == 2.0 ? (IN.ase_texcoord7.xyz).xz : float2( 0,0 ) ) + ( _VariationSource == 3.0 ? texCoord23_g1200 : float2( 0,0 ) ) );
				float simplePerlin2D4_g1200 = snoise( Source25_g1200*_VariationScale );
				float3 hsvTorgb12_g1198 = HSVToRGB( float3(saturate( ( hsvTorgb3_g1198.x + ( simplePerlin2D4_g1201 * _UseHSVVariation * _HueVariation ) ) ),saturate( ( hsvTorgb3_g1198.y + ( simplePerlin2D4_g1199 * _UseHSVVariation * _SaturationVariation ) ) ),saturate( ( hsvTorgb3_g1198.z + ( simplePerlin2D4_g1200 * _UseHSVVariation * _ValueVariation ) ) )) );
				float4 appendResult4_g1198 = (float4(saturate( hsvTorgb12_g1198 ) , (temp_output_1_0_g1198).w));
				float4 temp_output_15_0_g1202 = appendResult4_g1198;
				float3 hsvTorgb6_g1202 = RGBToHSV( temp_output_15_0_g1202.xyz );
				float3 hsvTorgb1_g1202 = HSVToRGB( float3(hsvTorgb6_g1202.x,hsvTorgb6_g1202.y,( round( ( hsvTorgb6_g1202.z * _ColorNumbers ) ) / _ColorNumbers )) );
				float4 appendResult8_g1202 = (float4(hsvTorgb1_g1202 , (temp_output_15_0_g1202).w));
				float4 temp_output_8_0_g1212 = ( _PosterizeColors == 1.0 ? appendResult8_g1202 : temp_output_15_0_g1202 );
				float3 Normal69_g1212 = Normals490;
				float temp_output_43_0_g1212 = ( 1.0 / _PuddleScale );
				float time31_g1212 = 0.0;
				float2 voronoiSmoothId31_g1212 = 0;
				float2 temp_output_77_0_g1212 = (WorldPosition).xz;
				float2 coords31_g1212 = temp_output_77_0_g1212 * temp_output_43_0_g1212;
				float2 id31_g1212 = 0;
				float2 uv31_g1212 = 0;
				float voroi31_g1212 = voronoi31_g1212( coords31_g1212, time31_g1212, id31_g1212, uv31_g1212, 0, voronoiSmoothId31_g1212 );
				float time39_g1212 = 2.16;
				float2 voronoiSmoothId39_g1212 = 0;
				float2 coords39_g1212 = temp_output_77_0_g1212 * ( temp_output_43_0_g1212 * 3.0 );
				float2 id39_g1212 = 0;
				float2 uv39_g1212 = 0;
				float voroi39_g1212 = voronoi39_g1212( coords39_g1212, time39_g1212, id39_g1212, uv39_g1212, 0, voronoiSmoothId39_g1212 );
				float Rain45_g1212 = ( ( (Normal69_g1212).y * 2.0 * ( (1.0 + (voroi31_g1212 - 0.0) * (0.0 - 1.0) / (0.4 - 0.0)) + (0.1 + (voroi39_g1212 - 0.0) * (-0.3 - 0.1) / (0.21 - 0.0)) ) * (0.3 + (CZY_WetnessAmount - 0.0) * (1.0 - 0.3) / (1.0 - 0.0)) ) > 0.5 ? 1.0 : 0.0 );
				float4 lerpResult58_g1212 = lerp( temp_output_8_0_g1212 , _PuddleColor , ( _PuddleColor.a * Rain45_g1212 ));
				float4 _SnowTexture_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(DistantLandsIllustrateOpaque,_SnowTexture_ST);
				float2 uv_SnowTexture = IN.ase_texcoord3.xy * _SnowTexture_ST_Instance.xy + _SnowTexture_ST_Instance.zw;
				float2 temp_output_84_0_g1212 = (WorldPosition).xz;
				float temp_output_15_0_g1212 = ( 1.0 / _SnowScale );
				float simplePerlin2D12_g1212 = snoise( temp_output_84_0_g1212*temp_output_15_0_g1212 );
				simplePerlin2D12_g1212 = simplePerlin2D12_g1212*0.5 + 0.5;
				float time19_g1212 = 0.0;
				float2 voronoiSmoothId19_g1212 = 0;
				float2 coords19_g1212 = temp_output_84_0_g1212 * ( temp_output_15_0_g1212 / 0.1 );
				float2 id19_g1212 = 0;
				float2 uv19_g1212 = 0;
				float voroi19_g1212 = voronoi19_g1212( coords19_g1212, time19_g1212, id19_g1212, uv19_g1212, 0, voronoiSmoothId19_g1212 );
				float Snow44_g1212 = ( pow( ( pow( (Normal69_g1212).y , 7.0 ) * ( simplePerlin2D12_g1212 * ( 1.0 - voroi19_g1212 ) ) ) , 0.5 ) > ( 1.0 - CZY_SnowAmount ) ? 1.0 : 0.0 );
				float4 lerpResult48_g1212 = lerp( lerpResult58_g1212 , ( _SnowColor * tex2D( _SnowTexture, uv_SnowTexture ) ) , Snow44_g1212);
				float lightMode277_g1189 = _LightingMode;
				float multiplyByLightColor201_g1189 = _MultiplyByLightColor;
				float3 bakedGI361_g1189 = ASEIndirectDiffuse( IN.lightmapUVOrVertexSH.xy, ase_worldNormal);
				Light ase_mainLight = GetMainLight( ShadowCoords );
				MixRealtimeAndBakedGI(ase_mainLight, ase_worldNormal, bakedGI361_g1189, half4(0,0,0,0));
				float useHalftone324_g1189 = _UseHalftone;
				float temp_output_8_0_g1189 = ( _LightRamp * 0.5 );
				float3 temp_output_191_0_g1189 = temp_output_887_0;
				float3 normals338_g1189 = temp_output_191_0_g1189;
				float dotResult1_g1189 = dot( _MainLightPosition.xyz , normals338_g1189 );
				float ase_lightAtten = 0;
				ase_lightAtten = ase_mainLight.distanceAttenuation * ase_mainLight.shadowAttenuation;
				float LightAttenuation100_g1189 = ( _UseShadows == 1.0 ? ase_lightAtten : 1.0 );
				float temp_output_12_0_g1189 = ( (dotResult1_g1189*0.5 + 0.5) * LightAttenuation100_g1189 );
				float smoothstepResult10_g1189 = smoothstep( ( _LightRampOffset - temp_output_8_0_g1189 ) , ( _LightRampOffset + temp_output_8_0_g1189 ) , ( _PosterizeLight == 1.0 ? ( round( ( temp_output_12_0_g1189 * _LightSteps ) ) / _LightSteps ) : temp_output_12_0_g1189 ));
				float preLightRatio308_g1189 = saturate( smoothstepResult10_g1189 );
				float time288_g1189 = 0.0;
				float2 voronoiSmoothId288_g1189 = 0;
				float4 screenPos = IN.ase_texcoord9;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float2 coords288_g1189 = (( ase_screenPosNorm * _ScreenParams )*_HalftoneScale + 0.0).xy * 0.02;
				float2 id288_g1189 = 0;
				float2 uv288_g1189 = 0;
				float voroi288_g1189 = voronoi288_g1189( coords288_g1189, time288_g1189, id288_g1189, uv288_g1189, 0, voronoiSmoothId288_g1189 );
				float halftone295_g1189 = (0.0 + (( voroi288_g1189 - _HalftoneOffset ) - 0.0) * (( _HalftoneMultiplier * 2.0 ) - 0.0) / (1.0 - 0.0));
				float lightRatio37_g1189 = ( useHalftone324_g1189 == 0.0 ? preLightRatio308_g1189 : ( ( preLightRatio308_g1189 - halftone295_g1189 ) > 0.5 ? 1.0 : 0.0 ) );
				float4 lerpResult22_g1189 = lerp( ( multiplyByLightColor201_g1189 == 1.0 ? ( float4( bakedGI361_g1189 , 0.0 ) * _ShadowColor ) : _ShadowColor ) , ( multiplyByLightColor201_g1189 == 1.0 ? ( _MainLightColor * _LightColor ) : _LightColor ) , lightRatio37_g1189);
				float4 FinalLighting134_g1189 = lerpResult22_g1189;
				float3 worldPosValue102_g1191 = WorldPosition;
				float3 WorldPosition118_g1191 = worldPosValue102_g1191;
				float2 ScreenUV105_g1191 = (ase_screenPosNorm).xy;
				float2 ScreenUV118_g1191 = ScreenUV105_g1191;
				half2 LightmapUV1_g1192 = (IN.ase_texcoord3.zw*(unity_LightmapST).xy + (unity_LightmapST).zw);
				half4 localCalculateShadowMask1_g1192 = CalculateShadowMask1_g1192( LightmapUV1_g1192 );
				float4 shadowMaskValue113_g1191 = localCalculateShadowMask1_g1192;
				float4 ShadowMask118_g1191 = shadowMaskValue113_g1191;
				float temp_output_123_0_g1191 = _AdditionalLightRamp;
				float Ramp118_g1191 = temp_output_123_0_g1191;
				float3 localAdditionalLightsFlatMask14x118_g1191 = AdditionalLightsFlatMask14x( WorldPosition118_g1191 , ScreenUV118_g1191 , ShadowMask118_g1191 , Ramp118_g1191 );
				float3 normalizeResult129_g1191 = normalize( localAdditionalLightsFlatMask14x118_g1191 );
				float3 temp_output_126_0_g1191 = ( length( localAdditionalLightsFlatMask14x118_g1191 ) > 1.0 ? normalizeResult129_g1191 : localAdditionalLightsFlatMask14x118_g1191 );
				float temp_output_133_0_g1191 = _LightSteps;
				float3 FlatResult131_g1191 = ( _PosterizeLight == 1.0 ? ( round( ( temp_output_126_0_g1191 * temp_output_133_0_g1191 ) ) / temp_output_133_0_g1191 ) : temp_output_126_0_g1191 );
				float3 posterizedLight236_g1189 = FlatResult131_g1191;
				float4 appendResult281_g1189 = (float4((( lightMode277_g1189 == 1.0 ? ( FinalLighting134_g1189 + float4( posterizedLight236_g1189 , 0.0 ) ) : float4( 1,1,1,1 ) )).rgb , 1.0));
				float4 SpecularColor103_g1189 = _SpecularColor;
				float temp_output_167_0_g1189 = ( ( _SpecularRampOffset + 1.0 ) / 2.0 );
				float temp_output_111_0_g1189 = ( _SpecularRamp * 2 );
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - WorldPosition );
				ase_worldViewDir = normalize(ase_worldViewDir);
				float3 normalizeResult4_g1190 = normalize( ( ase_worldViewDir + _MainLightPosition.xyz ) );
				float3 ModifiedNormal155_g1189 = ( _UseModifiedNormals == 0.0 ? ase_worldNormal : temp_output_191_0_g1189 );
				float3 normalizeResult84_g1189 = normalize( ModifiedNormal155_g1189 );
				float dotResult80_g1189 = dot( normalizeResult4_g1190 , normalizeResult84_g1189 );
				float smoothstepResult113_g1189 = smoothstep( ( temp_output_167_0_g1189 - temp_output_111_0_g1189 ) , ( temp_output_167_0_g1189 + temp_output_111_0_g1189 ) , max( dotResult80_g1189 , 0.0 ));
				float Specular102_g1189 = ( _SpecularColor.a * smoothstepResult113_g1189 );
				float4 FinalSpecular133_g1189 = ( ( multiplyByLightColor201_g1189 == 1.0 ? _MainLightColor : float4( 1,1,1,0 ) ) * SpecularColor103_g1189 * ( Specular102_g1189 * lightRatio37_g1189 ) * _UseSpecular );
				float4 RimColor119_g1189 = _RimLightColor;
				float temp_output_127_0_g1189 = ( _RimRamp * 0.5 );
				float dotResult124_g1189 = dot( ModifiedNormal155_g1189 , ase_worldViewDir );
				float smoothstepResult129_g1189 = smoothstep( ( _RimLightRampOffset - temp_output_127_0_g1189 ) , ( _RimLightRampOffset + temp_output_127_0_g1189 ) , max( ( 1.0 - dotResult124_g1189 ) , 0.0 ));
				float Rim117_g1189 = ( _RimLightColor.a * smoothstepResult129_g1189 );
				float lerpResult148_g1189 = lerp( _RimLightShadowIntensity , _RimLightLitIntensity , lightRatio37_g1189);
				float temp_output_142_0_g1189 = ( Rim117_g1189 * lerpResult148_g1189 );
				float4 FinalRim145_g1189 = ( RimColor119_g1189 * ( useHalftone324_g1189 == 0.0 ? temp_output_142_0_g1189 : ( ( temp_output_142_0_g1189 - halftone295_g1189 ) > 0.5 ? 1.0 : 0.0 ) ) * _UseRimLighting );
				float temp_output_921_32 = ( lightMode277_g1189 == 1.0 ? saturate( ( length( posterizedLight236_g1189 ) + lightRatio37_g1189 ) ) : 1.0 );
				float3x3 ase_worldToTangent = float3x3(ase_worldTangent,ase_worldBitangent,ase_worldNormal);
				float3 worldToTangentPos15_g1188 = mul( ase_worldToTangent, ( WorldPosition - _WorldSpaceCameraPos ));
				float cos19_g1188 = cos( radians( 45.0 ) );
				float sin19_g1188 = sin( radians( 45.0 ) );
				float2 rotator19_g1188 = mul( worldToTangentPos15_g1188.xy - float2( 0,0 ) , float2x2( cos19_g1188 , -sin19_g1188 , sin19_g1188 , cos19_g1188 )) + float2( 0,0 );
				float4 Glint23_g1188 = ( tex2D( _GlintTexture, (rotator19_g1188*( _GlintScale * 1.0 ) + 0.0) ) * _GlintColor );
				float lerpResult20_g1166 = lerp( _EmissionShadowRatio , _EmissionLightRatio , temp_output_921_32);
				float2 temp_cast_14 = (_EmissionEffectScale).xx;
				float2 texCoord5_g1166 = IN.ase_texcoord3.xy * temp_cast_14 + float2( 0,0 );
				float4 UVs26_g1166 = ( _EmissionUVSource == 0.0 ? float4( texCoord5_g1166, 0.0 , 0.0 ) : (ase_screenPosNorm*_EmissionEffectScale + 0.0) );
				float4 DefaultEmission31_g1166 = tex2D( _EmissionTexture, UVs26_g1166.xy );
				float4 ScrolledEmission25_g1166 = max( tex2D( _EmissionTexture, (UVs26_g1166*1.0 + float4( ( _EmissionScrolling1 * sin( _TimeParameters.x * 0.25 ) ), 0.0 , 0.0 )).xy ) , tex2D( _EmissionTexture, (UVs26_g1166*0.9 + float4( ( sin( _TimeParameters.x * 0.5 ) * _EmissionScrolling2 ), 0.0 , 0.0 )).xy ) );
				float lerpResult37_g1153 = lerp( ( 1.0 - _NoiseAmountShadow ) , ( 1.0 - _NoiseAmountLight ) , temp_output_921_32);
				float2 texCoord48_g1153 = IN.ase_texcoord3.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult20_g1153 = (float2(ase_screenPosNorm.xy));
				float2 ScreenspaceUV21_g1153 = ( _NoiseUVSource == 0.0 ? texCoord48_g1153 : appendResult20_g1153 );
				float temp_output_5_0_g1153 = ( floor( ( _TimeParameters.x * _NoiseFramerate ) ) / _NoiseFramerate );
				float3 normalizedWorldNormal = normalize( ase_worldNormal );
				float dotResult33_g1153 = dot( normalizedWorldNormal , ase_worldViewDir );
				float4 lerpResult27_g1153 = lerp( max( tex2D( _ScreenNoiseTexture, (ScreenspaceUV21_g1153*( 0.55 * _NoiseScale ) + temp_output_5_0_g1153) ) , tex2D( _ScreenNoiseTexture, (ScreenspaceUV21_g1153*_NoiseScale + ( -1.56 * temp_output_5_0_g1153 )) ) ) , float4( 1,1,1,1 ) , saturate( ( dotResult33_g1153 * _NoiseOffset ) ));
				float lerpResult24_g1153 = lerp( lerpResult37_g1153 , 1.0 , lerpResult27_g1153.r);
				float ScreenspaceNoise23_g1153 = lerpResult24_g1153;
				
				float3 BakedAlbedo = 0;
				float3 BakedEmission = 0;
				float3 Color = ( ( ( ( ( _UseCOZYPrecipitation * lerpResult48_g1212 ) + ( ( 1.0 - _UseCOZYPrecipitation ) * temp_output_8_0_g1212 ) ) * appendResult281_g1189 ) + float4( (( lightMode277_g1189 == 1.0 ? ( FinalSpecular133_g1189 + FinalRim145_g1189 ) : float4( 0,0,0,0 ) )).rgb , 0.0 ) + ( _UseGlint * ( _MultiplyByLightRatio == 1.0 ? ( temp_output_921_32 * Glint23_g1188 ) : Glint23_g1188 ) ) + ( _UseEmission == 0.0 ? float4( 0,0,0,0 ) : ( lerpResult20_g1166 * _EmissionColor * ( _ScrollEmission == 0.0 ? DefaultEmission31_g1166 : ScrolledEmission25_g1166 ) ) ) ) * ( _UseScreenNoise == 1.0 ? ScreenspaceNoise23_g1153 : 1.0 ) ).rgb;
				float Alpha = 1;
				float AlphaClipThreshold = 0.5;
				float AlphaClipThresholdShadow = 0.5;

				#ifdef _ALPHATEST_ON
					clip( Alpha - AlphaClipThreshold );
				#endif

				#if defined(_DBUFFER)
					ApplyDecalToBaseColor(IN.clipPos, Color);
				#endif

				#if defined(_ALPHAPREMULTIPLY_ON)
				Color *= Alpha;
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODFadeCrossFade( IN.clipPos );
				#endif

				#ifdef ASE_FOG
					Color = MixFog( Color, IN.fogFactor );
				#endif

				#ifdef _WRITE_RENDERING_LAYERS
					uint renderingLayers = GetMeshRenderingLayer();
					outRenderingLayers = float4( EncodeMeshRenderingLayer( renderingLayers ), 0, 0, 0 );
				#endif

				return half4( Color, Alpha );
			}
			ENDHLSL
		}

		
		Pass
		{
			
			Name "ShadowCaster"
			Tags { "LightMode"="ShadowCaster" }

			ZWrite On
			ZTest LEqual
			AlphaToMask Off
			ColorMask 0

			HLSLPROGRAM

			#define ASE_SRP_VERSION 140003


			#pragma vertex vert
			#pragma fragment frag

			#pragma multi_compile _ _CASTING_PUNCTUAL_LIGHT_SHADOW

			#define SHADERPASS SHADERPASS_SHADOWCASTER

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/LODCrossFade.hlsl"

			#define ASE_NEEDS_VERT_POSITION
			#pragma multi_compile_instancing


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_color : COLOR;
				uint ase_vertexID : SV_VertexID;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
					float3 worldPos : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					float4 shadowCoord : TEXCOORD1;
				#endif
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _TriplanarColor;
			float4 _NearColor;
			float4 _LightColor;
			float4 _ShadowColor;
			float4 _MainColor;
			float4 _GlintColor;
			float4 _RimLightColor;
			float4 _EmissionColor;
			float4 _SnowColor;
			float4 _FarColor;
			float4 _SpecularColor;
			float4 _PuddleColor;
			float3 _SwirlDirection;
			float3 _GradientChannelMask;
			float3 _SwayMask;
			float3 _FlutterMask;
			float3 _WaveInfluenceDirection;
			float3 _FlutterDirection;
			float3 _CustomNormalEllipseSize;
			float3 _CustomNormalDirection;
			float3 _WaveDirection2;
			float3 _SwayDirection;
			float3 _SwirlMask;
			float3 _WaveDirection1;
			float3 _TriplanarDirection;
			float3 _GradientPositionalOffset;
			float3 _WaveMask;
			float2 _EmissionScrolling1;
			float2 _EmissionScrolling2;
			float _LightRamp;
			float _UseShadows;
			float _PosterizeLight;
			float _UseFlutter;
			float _UseHalftone;
			float _MultiplyByLightColor;
			float _LightingMode;
			float _SnowScale;
			float _PuddleScale;
			float _ColorNumbers;
			float _ValueVariation;
			float _SaturationVariation;
			float _LightRampOffset;
			float _LightSteps;
			float _SpecularRampOffset;
			float _HalftoneOffset;
			float _NoiseScale;
			float _NoiseUVSource;
			float _NoiseAmountLight;
			float _NoiseAmountShadow;
			float _UseScreenNoise;
			float _EmissionEffectScale;
			float _EmissionUVSource;
			float _ScrollEmission;
			float _EmissionLightRatio;
			float _EmissionShadowRatio;
			float _UseEmission;
			float _GlintScale;
			float _MultiplyByLightRatio;
			float _UseGlint;
			float _UseRimLighting;
			float _RimLightLitIntensity;
			float _RimLightShadowIntensity;
			float _RimRamp;
			float _RimLightRampOffset;
			float _UseSpecular;
			float _UseModifiedNormals;
			float _SpecularRamp;
			float _HueVariation;
			float _AdditionalLightRamp;
			float _HalftoneMultiplier;
			float _HalftoneScale;
			float _UseHSVVariation;
			float _TriplanarSpace;
			float _VariationSource;
			float _SwaySpeed;
			float _SwayFramerate;
			float _SwayNoiseScale;
			float _SwayAmount;
			float _SwaySensitivity;
			float _SwayOffset;
			float _SwaySource;
			float _UseSway;
			float _SwirlSpeed;
			float _SwirlFramerate;
			float _SwirlNoiseScale;
			float _SwirlAmount;
			float _SwirlSensitivity;
			float _SwirlOffset;
			float _SwirlSource;
			float _UseSwirl;
			float _FlutterSpeed;
			float _FlutterFramerate;
			float _FlutterNoiseScale;
			float _FlutterAmount;
			float _FlutterSensitivity;
			float _FlutterOffset;
			float _FlutterSource;
			float _UseWave;
			float _VariationScale;
			float _WaveSource;
			float _WaveSensitivity;
			float _ValueShift;
			float _SaturationShift;
			float _HueShift;
			float _ClipTriplanar;
			float _TriplanarOffset;
			float _TriplanarMultiplier;
			float _NoiseFramerate;
			float _BlendStrength;
			float _NormalMode;
			float _GradientSensitivity;
			float _Space;
			float _GradientOffset;
			float _GradientSource;
			float _UseGradientShading;
			float _UseTriplanar;
			float _ClampAdjustments;
			float _UseColorAdjustments;
			float _PosterizeColors;
			float _UseCOZYPrecipitation;
			float _WaveSpeed;
			float _WaveFramerate;
			float _WaveNoiseScale;
			float _WaveAmount;
			float _WaveOffset;
			float _NoiseOffset;
			#ifdef ASE_TESSELLATION
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END

			UNITY_INSTANCING_BUFFER_START(DistantLandsIllustrateOpaque)
				UNITY_DEFINE_INSTANCED_PROP(float, _CullMode)
			UNITY_INSTANCING_BUFFER_END(DistantLandsIllustrateOpaque)


			float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }
			float snoise( float2 v )
			{
				const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
				float2 i = floor( v + dot( v, C.yy ) );
				float2 x0 = v - i + dot( i, C.xx );
				float2 i1;
				i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
				float4 x12 = x0.xyxy + C.xxzz;
				x12.xy -= i1;
				i = mod2D289( i );
				float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
				float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
				m = m * m;
				m = m * m;
				float3 x = 2.0 * frac( p * C.www ) - 1.0;
				float3 h = abs( x ) - 0.5;
				float3 ox = floor( x + 0.5 );
				float3 a0 = x - ox;
				m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
				float3 g;
				g.x = a0.x * x0.x + h.x * x0.y;
				g.yz = a0.yz * x12.xz + h.yz * x12.yw;
				return 130.0 * dot( m, g );
			}
			
			float3 RotateAroundAxis( float3 center, float3 original, float3 u, float angle )
			{
				original -= center;
				float C = cos( angle );
				float S = sin( angle );
				float t = 1 - C;
				float m00 = t * u.x * u.x + C;
				float m01 = t * u.x * u.y - S * u.z;
				float m02 = t * u.x * u.z + S * u.y;
				float m10 = t * u.x * u.y + S * u.z;
				float m11 = t * u.y * u.y + C;
				float m12 = t * u.y * u.z - S * u.x;
				float m20 = t * u.x * u.z - S * u.y;
				float m21 = t * u.y * u.z + S * u.x;
				float m22 = t * u.z * u.z + C;
				float3x3 finalMatrix = float3x3( m00, m01, m02, m10, m11, m12, m20, m21, m22 );
				return mul( finalMatrix, original ) + center;
			}
			

			float3 _LightDirection;
			float3 _LightPosition;

			VertexOutput VertexFunction( VertexInput v )
			{
				VertexOutput o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				float temp_output_21_0_g1173 = _FlutterSource;
				float3 temp_output_23_0_g1173 = v.vertex.xyz;
				float temp_output_1_0_g1173 = distance( ( -_FlutterOffset + temp_output_23_0_g1173 ) , float3( 0,0,0 ) );
				float3 temp_output_22_0_g1173 = _FlutterMask;
				float temp_output_31_0_g1173 = length( ( ( -_FlutterOffset + temp_output_23_0_g1173 ) * temp_output_22_0_g1173 ) );
				float temp_output_30_0_g1173 = length( ( float4( temp_output_22_0_g1173 , 0.0 ) * v.ase_color ) );
				float temp_output_2_0_g1172 = _FlutterFramerate;
				float2 temp_cast_1 = (v.ase_vertexID*( _FlutterNoiseScale * 0.1 ) + ( ( round( ( _TimeParameters.x * temp_output_2_0_g1172 ) ) / temp_output_2_0_g1172 ) * 0.1 * _FlutterSpeed ));
				float simplePerlin2D12_g1171 = snoise( temp_cast_1*8.91 );
				float3 worldToObjDir34_g1171 = normalize( mul( GetWorldToObjectMatrix(), float4( _FlutterDirection, 0 ) ).xyz );
				float temp_output_21_0_g1170 = _SwirlSource;
				float3 temp_output_23_0_g1170 = v.vertex.xyz;
				float temp_output_1_0_g1170 = distance( ( -_SwirlOffset + temp_output_23_0_g1170 ) , float3( 0,0,0 ) );
				float3 temp_output_22_0_g1170 = _SwirlMask;
				float temp_output_31_0_g1170 = length( ( ( -_SwirlOffset + temp_output_23_0_g1170 ) * temp_output_22_0_g1170 ) );
				float temp_output_30_0_g1170 = length( ( float4( temp_output_22_0_g1170 , 0.0 ) * v.ase_color ) );
				float3 worldToObjDir40_g1168 = normalize( mul( GetWorldToObjectMatrix(), float4( _SwirlDirection, 0 ) ).xyz );
				float temp_output_2_0_g1169 = _SwirlFramerate;
				float simplePerlin2D12_g1168 = snoise( (v.vertex.xyz*( _SwirlNoiseScale * 0.1 ) + ( ( round( ( _TimeParameters.x * temp_output_2_0_g1169 ) ) / temp_output_2_0_g1169 ) * 0.1 * _SwirlSpeed )).xy*8.91 );
				float3 rotatedValue35_g1168 = RotateAroundAxis( float3( 0,0,0 ), v.vertex.xyz, worldToObjDir40_g1168, ( _SwirlAmount * 0.01 * simplePerlin2D12_g1168 ) );
				float temp_output_21_0_g1176 = _SwaySource;
				float3 temp_output_23_0_g1176 = v.vertex.xyz;
				float temp_output_1_0_g1176 = distance( ( -_SwayOffset + temp_output_23_0_g1176 ) , float3( 0,0,0 ) );
				float3 temp_output_22_0_g1176 = _SwayMask;
				float temp_output_31_0_g1176 = length( ( ( -_SwayOffset + temp_output_23_0_g1176 ) * temp_output_22_0_g1176 ) );
				float temp_output_30_0_g1176 = length( ( float4( temp_output_22_0_g1176 , 0.0 ) * v.ase_color ) );
				float temp_output_2_0_g1175 = _SwayFramerate;
				float simplePerlin2D5_g1174 = snoise( (v.vertex.xyz*( _SwayNoiseScale * 0.1 ) + ( ( round( ( _TimeParameters.x * temp_output_2_0_g1175 ) ) / temp_output_2_0_g1175 ) * 0.1 * _SwaySpeed )).xy*8.91 );
				simplePerlin2D5_g1174 = simplePerlin2D5_g1174*0.5 + 0.5;
				float3 worldToObjDir18_g1174 = normalize( mul( GetWorldToObjectMatrix(), float4( _SwayDirection, 0 ) ).xyz );
				float temp_output_21_0_g1179 = _WaveSource;
				float3 temp_output_23_0_g1179 = v.vertex.xyz;
				float temp_output_1_0_g1179 = distance( ( -_WaveOffset + temp_output_23_0_g1179 ) , float3( 0,0,0 ) );
				float3 temp_output_22_0_g1179 = _WaveMask;
				float temp_output_31_0_g1179 = length( ( ( -_WaveOffset + temp_output_23_0_g1179 ) * temp_output_22_0_g1179 ) );
				float temp_output_30_0_g1179 = length( ( float4( temp_output_22_0_g1179 , 0.0 ) * v.ase_color ) );
				float3 ase_worldPos = TransformObjectToWorld( (v.vertex).xyz );
				float3 normalizeResult37_g1177 = normalize( _WaveDirection1 );
				float3 break35_g1177 = ( ase_worldPos * normalizeResult37_g1177 );
				float temp_output_2_0_g1178 = _WaveFramerate;
				float Time40_g1177 = ( ( round( ( _TimeParameters.x * temp_output_2_0_g1178 ) ) / temp_output_2_0_g1178 ) * _WaveSpeed );
				float3 normalizeResult52_g1177 = normalize( _WaveDirection2 );
				float3 break49_g1177 = ( ase_worldPos * normalizeResult52_g1177 );
				float3 worldToObjDir18_g1177 = normalize( mul( GetWorldToObjectMatrix(), float4( _WaveInfluenceDirection, 0 ) ).xyz );
				float3 ase_objectScale = float3( length( GetObjectToWorldMatrix()[ 0 ].xyz ), length( GetObjectToWorldMatrix()[ 1 ].xyz ), length( GetObjectToWorldMatrix()[ 2 ].xyz ) );
				

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = ( ( _UseFlutter == 1.0 ? ( saturate( ( ( ( temp_output_21_0_g1173 == 3.0 ? 1.0 : 0.0 ) + ( temp_output_21_0_g1173 == 0.0 ? temp_output_1_0_g1173 : 0.0 ) + ( temp_output_21_0_g1173 == 1.0 ? temp_output_31_0_g1173 : 0.0 ) + ( temp_output_21_0_g1173 == 2.0 ? temp_output_30_0_g1173 : 0.0 ) ) * _FlutterSensitivity ) ) * ( _FlutterAmount * float3( 0.001,0.001,0.001 ) * simplePerlin2D12_g1171 * worldToObjDir34_g1171 ) ) : float3( 0,0,0 ) ) + ( _UseSwirl == 1.0 ? ( saturate( ( ( ( temp_output_21_0_g1170 == 3.0 ? 1.0 : 0.0 ) + ( temp_output_21_0_g1170 == 0.0 ? temp_output_1_0_g1170 : 0.0 ) + ( temp_output_21_0_g1170 == 1.0 ? temp_output_31_0_g1170 : 0.0 ) + ( temp_output_21_0_g1170 == 2.0 ? temp_output_30_0_g1170 : 0.0 ) ) * _SwirlSensitivity ) ) * ( rotatedValue35_g1168 - v.vertex.xyz ) ) : float3( 0,0,0 ) ) + ( _UseSway == 1.0 ? ( saturate( ( ( ( temp_output_21_0_g1176 == 3.0 ? 1.0 : 0.0 ) + ( temp_output_21_0_g1176 == 0.0 ? temp_output_1_0_g1176 : 0.0 ) + ( temp_output_21_0_g1176 == 1.0 ? temp_output_31_0_g1176 : 0.0 ) + ( temp_output_21_0_g1176 == 2.0 ? temp_output_30_0_g1176 : 0.0 ) ) * _SwaySensitivity ) ) * ( _SwayAmount * float3( 0.01,0.01,0.01 ) * simplePerlin2D5_g1174 * worldToObjDir18_g1174 ) ) : float3( 0,0,0 ) ) + ( _UseWave == 1.0 ? ( ( saturate( ( ( ( temp_output_21_0_g1179 == 3.0 ? 1.0 : 0.0 ) + ( temp_output_21_0_g1179 == 0.0 ? temp_output_1_0_g1179 : 0.0 ) + ( temp_output_21_0_g1179 == 1.0 ? temp_output_31_0_g1179 : 0.0 ) + ( temp_output_21_0_g1179 == 2.0 ? temp_output_30_0_g1179 : 0.0 ) ) * _WaveSensitivity ) ) * ( _WaveAmount * ( sin( (( break35_g1177.x + break35_g1177.y + break35_g1177.z )*_WaveNoiseScale + Time40_g1177) ) + sin( (( break49_g1177.x + break49_g1177.y + break49_g1177.z )*( _WaveNoiseScale * 0.5 ) + ( Time40_g1177 * 0.7 )) ) ) * worldToObjDir18_g1177 ) ) / ase_objectScale ) : float3( 0,0,0 ) ) );

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
					o.worldPos = positionWS;
				#endif

				float3 normalWS = TransformObjectToWorldDir( v.ase_normal );

				#if _CASTING_PUNCTUAL_LIGHT_SHADOW
					float3 lightDirectionWS = normalize(_LightPosition - positionWS);
				#else
					float3 lightDirectionWS = _LightDirection;
				#endif

				float4 clipPos = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, lightDirectionWS));

				#if UNITY_REVERSED_Z
					clipPos.z = min(clipPos.z, UNITY_NEAR_CLIP_VALUE);
				#else
					clipPos.z = max(clipPos.z, UNITY_NEAR_CLIP_VALUE);
				#endif

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = clipPos;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif

				o.clipPos = clipPos;

				return o;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_color : COLOR;
				uint ase_vertexID : SV_VertexID;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.vertex = v.vertex;
				o.ase_color = v.ase_color;
				o.ase_vertexID = v.ase_vertexID;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				o.ase_vertexID = patch[0].ase_vertexID * bary.x + patch[1].ase_vertexID * bary.y + patch[2].ase_vertexID * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(VertexOutput IN  ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
					float3 WorldPosition = IN.worldPos;
				#endif

				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				float _CullMode_Instance = UNITY_ACCESS_INSTANCED_PROP(DistantLandsIllustrateOpaque,_CullMode);
				

				float Alpha = 1;
				float AlphaClipThreshold = 0.5;
				float AlphaClipThresholdShadow = 0.5;

				#ifdef _ALPHATEST_ON
					#ifdef _ALPHATEST_SHADOW_ON
						clip(Alpha - AlphaClipThresholdShadow);
					#else
						clip(Alpha - AlphaClipThreshold);
					#endif
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODFadeCrossFade( IN.clipPos );
				#endif
				return 0;
			}
			ENDHLSL
		}

		
		Pass
		{
			
			Name "DepthOnly"
			Tags { "LightMode"="DepthOnly" }

			ZWrite On
			ColorMask 0
			AlphaToMask Off

			HLSLPROGRAM

			#define ASE_SRP_VERSION 140003


			#pragma vertex vert
			#pragma fragment frag

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/LODCrossFade.hlsl"

			#define ASE_NEEDS_VERT_POSITION
			#pragma multi_compile_instancing


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_color : COLOR;
				uint ase_vertexID : SV_VertexID;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 worldPos : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				float4 shadowCoord : TEXCOORD1;
				#endif
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _TriplanarColor;
			float4 _NearColor;
			float4 _LightColor;
			float4 _ShadowColor;
			float4 _MainColor;
			float4 _GlintColor;
			float4 _RimLightColor;
			float4 _EmissionColor;
			float4 _SnowColor;
			float4 _FarColor;
			float4 _SpecularColor;
			float4 _PuddleColor;
			float3 _SwirlDirection;
			float3 _GradientChannelMask;
			float3 _SwayMask;
			float3 _FlutterMask;
			float3 _WaveInfluenceDirection;
			float3 _FlutterDirection;
			float3 _CustomNormalEllipseSize;
			float3 _CustomNormalDirection;
			float3 _WaveDirection2;
			float3 _SwayDirection;
			float3 _SwirlMask;
			float3 _WaveDirection1;
			float3 _TriplanarDirection;
			float3 _GradientPositionalOffset;
			float3 _WaveMask;
			float2 _EmissionScrolling1;
			float2 _EmissionScrolling2;
			float _LightRamp;
			float _UseShadows;
			float _PosterizeLight;
			float _UseFlutter;
			float _UseHalftone;
			float _MultiplyByLightColor;
			float _LightingMode;
			float _SnowScale;
			float _PuddleScale;
			float _ColorNumbers;
			float _ValueVariation;
			float _SaturationVariation;
			float _LightRampOffset;
			float _LightSteps;
			float _SpecularRampOffset;
			float _HalftoneOffset;
			float _NoiseScale;
			float _NoiseUVSource;
			float _NoiseAmountLight;
			float _NoiseAmountShadow;
			float _UseScreenNoise;
			float _EmissionEffectScale;
			float _EmissionUVSource;
			float _ScrollEmission;
			float _EmissionLightRatio;
			float _EmissionShadowRatio;
			float _UseEmission;
			float _GlintScale;
			float _MultiplyByLightRatio;
			float _UseGlint;
			float _UseRimLighting;
			float _RimLightLitIntensity;
			float _RimLightShadowIntensity;
			float _RimRamp;
			float _RimLightRampOffset;
			float _UseSpecular;
			float _UseModifiedNormals;
			float _SpecularRamp;
			float _HueVariation;
			float _AdditionalLightRamp;
			float _HalftoneMultiplier;
			float _HalftoneScale;
			float _UseHSVVariation;
			float _TriplanarSpace;
			float _VariationSource;
			float _SwaySpeed;
			float _SwayFramerate;
			float _SwayNoiseScale;
			float _SwayAmount;
			float _SwaySensitivity;
			float _SwayOffset;
			float _SwaySource;
			float _UseSway;
			float _SwirlSpeed;
			float _SwirlFramerate;
			float _SwirlNoiseScale;
			float _SwirlAmount;
			float _SwirlSensitivity;
			float _SwirlOffset;
			float _SwirlSource;
			float _UseSwirl;
			float _FlutterSpeed;
			float _FlutterFramerate;
			float _FlutterNoiseScale;
			float _FlutterAmount;
			float _FlutterSensitivity;
			float _FlutterOffset;
			float _FlutterSource;
			float _UseWave;
			float _VariationScale;
			float _WaveSource;
			float _WaveSensitivity;
			float _ValueShift;
			float _SaturationShift;
			float _HueShift;
			float _ClipTriplanar;
			float _TriplanarOffset;
			float _TriplanarMultiplier;
			float _NoiseFramerate;
			float _BlendStrength;
			float _NormalMode;
			float _GradientSensitivity;
			float _Space;
			float _GradientOffset;
			float _GradientSource;
			float _UseGradientShading;
			float _UseTriplanar;
			float _ClampAdjustments;
			float _UseColorAdjustments;
			float _PosterizeColors;
			float _UseCOZYPrecipitation;
			float _WaveSpeed;
			float _WaveFramerate;
			float _WaveNoiseScale;
			float _WaveAmount;
			float _WaveOffset;
			float _NoiseOffset;
			#ifdef ASE_TESSELLATION
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END

			UNITY_INSTANCING_BUFFER_START(DistantLandsIllustrateOpaque)
				UNITY_DEFINE_INSTANCED_PROP(float, _CullMode)
			UNITY_INSTANCING_BUFFER_END(DistantLandsIllustrateOpaque)


			float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }
			float snoise( float2 v )
			{
				const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
				float2 i = floor( v + dot( v, C.yy ) );
				float2 x0 = v - i + dot( i, C.xx );
				float2 i1;
				i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
				float4 x12 = x0.xyxy + C.xxzz;
				x12.xy -= i1;
				i = mod2D289( i );
				float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
				float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
				m = m * m;
				m = m * m;
				float3 x = 2.0 * frac( p * C.www ) - 1.0;
				float3 h = abs( x ) - 0.5;
				float3 ox = floor( x + 0.5 );
				float3 a0 = x - ox;
				m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
				float3 g;
				g.x = a0.x * x0.x + h.x * x0.y;
				g.yz = a0.yz * x12.xz + h.yz * x12.yw;
				return 130.0 * dot( m, g );
			}
			
			float3 RotateAroundAxis( float3 center, float3 original, float3 u, float angle )
			{
				original -= center;
				float C = cos( angle );
				float S = sin( angle );
				float t = 1 - C;
				float m00 = t * u.x * u.x + C;
				float m01 = t * u.x * u.y - S * u.z;
				float m02 = t * u.x * u.z + S * u.y;
				float m10 = t * u.x * u.y + S * u.z;
				float m11 = t * u.y * u.y + C;
				float m12 = t * u.y * u.z - S * u.x;
				float m20 = t * u.x * u.z - S * u.y;
				float m21 = t * u.y * u.z + S * u.x;
				float m22 = t * u.z * u.z + C;
				float3x3 finalMatrix = float3x3( m00, m01, m02, m10, m11, m12, m20, m21, m22 );
				return mul( finalMatrix, original ) + center;
			}
			

			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float temp_output_21_0_g1173 = _FlutterSource;
				float3 temp_output_23_0_g1173 = v.vertex.xyz;
				float temp_output_1_0_g1173 = distance( ( -_FlutterOffset + temp_output_23_0_g1173 ) , float3( 0,0,0 ) );
				float3 temp_output_22_0_g1173 = _FlutterMask;
				float temp_output_31_0_g1173 = length( ( ( -_FlutterOffset + temp_output_23_0_g1173 ) * temp_output_22_0_g1173 ) );
				float temp_output_30_0_g1173 = length( ( float4( temp_output_22_0_g1173 , 0.0 ) * v.ase_color ) );
				float temp_output_2_0_g1172 = _FlutterFramerate;
				float2 temp_cast_1 = (v.ase_vertexID*( _FlutterNoiseScale * 0.1 ) + ( ( round( ( _TimeParameters.x * temp_output_2_0_g1172 ) ) / temp_output_2_0_g1172 ) * 0.1 * _FlutterSpeed ));
				float simplePerlin2D12_g1171 = snoise( temp_cast_1*8.91 );
				float3 worldToObjDir34_g1171 = normalize( mul( GetWorldToObjectMatrix(), float4( _FlutterDirection, 0 ) ).xyz );
				float temp_output_21_0_g1170 = _SwirlSource;
				float3 temp_output_23_0_g1170 = v.vertex.xyz;
				float temp_output_1_0_g1170 = distance( ( -_SwirlOffset + temp_output_23_0_g1170 ) , float3( 0,0,0 ) );
				float3 temp_output_22_0_g1170 = _SwirlMask;
				float temp_output_31_0_g1170 = length( ( ( -_SwirlOffset + temp_output_23_0_g1170 ) * temp_output_22_0_g1170 ) );
				float temp_output_30_0_g1170 = length( ( float4( temp_output_22_0_g1170 , 0.0 ) * v.ase_color ) );
				float3 worldToObjDir40_g1168 = normalize( mul( GetWorldToObjectMatrix(), float4( _SwirlDirection, 0 ) ).xyz );
				float temp_output_2_0_g1169 = _SwirlFramerate;
				float simplePerlin2D12_g1168 = snoise( (v.vertex.xyz*( _SwirlNoiseScale * 0.1 ) + ( ( round( ( _TimeParameters.x * temp_output_2_0_g1169 ) ) / temp_output_2_0_g1169 ) * 0.1 * _SwirlSpeed )).xy*8.91 );
				float3 rotatedValue35_g1168 = RotateAroundAxis( float3( 0,0,0 ), v.vertex.xyz, worldToObjDir40_g1168, ( _SwirlAmount * 0.01 * simplePerlin2D12_g1168 ) );
				float temp_output_21_0_g1176 = _SwaySource;
				float3 temp_output_23_0_g1176 = v.vertex.xyz;
				float temp_output_1_0_g1176 = distance( ( -_SwayOffset + temp_output_23_0_g1176 ) , float3( 0,0,0 ) );
				float3 temp_output_22_0_g1176 = _SwayMask;
				float temp_output_31_0_g1176 = length( ( ( -_SwayOffset + temp_output_23_0_g1176 ) * temp_output_22_0_g1176 ) );
				float temp_output_30_0_g1176 = length( ( float4( temp_output_22_0_g1176 , 0.0 ) * v.ase_color ) );
				float temp_output_2_0_g1175 = _SwayFramerate;
				float simplePerlin2D5_g1174 = snoise( (v.vertex.xyz*( _SwayNoiseScale * 0.1 ) + ( ( round( ( _TimeParameters.x * temp_output_2_0_g1175 ) ) / temp_output_2_0_g1175 ) * 0.1 * _SwaySpeed )).xy*8.91 );
				simplePerlin2D5_g1174 = simplePerlin2D5_g1174*0.5 + 0.5;
				float3 worldToObjDir18_g1174 = normalize( mul( GetWorldToObjectMatrix(), float4( _SwayDirection, 0 ) ).xyz );
				float temp_output_21_0_g1179 = _WaveSource;
				float3 temp_output_23_0_g1179 = v.vertex.xyz;
				float temp_output_1_0_g1179 = distance( ( -_WaveOffset + temp_output_23_0_g1179 ) , float3( 0,0,0 ) );
				float3 temp_output_22_0_g1179 = _WaveMask;
				float temp_output_31_0_g1179 = length( ( ( -_WaveOffset + temp_output_23_0_g1179 ) * temp_output_22_0_g1179 ) );
				float temp_output_30_0_g1179 = length( ( float4( temp_output_22_0_g1179 , 0.0 ) * v.ase_color ) );
				float3 ase_worldPos = TransformObjectToWorld( (v.vertex).xyz );
				float3 normalizeResult37_g1177 = normalize( _WaveDirection1 );
				float3 break35_g1177 = ( ase_worldPos * normalizeResult37_g1177 );
				float temp_output_2_0_g1178 = _WaveFramerate;
				float Time40_g1177 = ( ( round( ( _TimeParameters.x * temp_output_2_0_g1178 ) ) / temp_output_2_0_g1178 ) * _WaveSpeed );
				float3 normalizeResult52_g1177 = normalize( _WaveDirection2 );
				float3 break49_g1177 = ( ase_worldPos * normalizeResult52_g1177 );
				float3 worldToObjDir18_g1177 = normalize( mul( GetWorldToObjectMatrix(), float4( _WaveInfluenceDirection, 0 ) ).xyz );
				float3 ase_objectScale = float3( length( GetObjectToWorldMatrix()[ 0 ].xyz ), length( GetObjectToWorldMatrix()[ 1 ].xyz ), length( GetObjectToWorldMatrix()[ 2 ].xyz ) );
				

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = ( ( _UseFlutter == 1.0 ? ( saturate( ( ( ( temp_output_21_0_g1173 == 3.0 ? 1.0 : 0.0 ) + ( temp_output_21_0_g1173 == 0.0 ? temp_output_1_0_g1173 : 0.0 ) + ( temp_output_21_0_g1173 == 1.0 ? temp_output_31_0_g1173 : 0.0 ) + ( temp_output_21_0_g1173 == 2.0 ? temp_output_30_0_g1173 : 0.0 ) ) * _FlutterSensitivity ) ) * ( _FlutterAmount * float3( 0.001,0.001,0.001 ) * simplePerlin2D12_g1171 * worldToObjDir34_g1171 ) ) : float3( 0,0,0 ) ) + ( _UseSwirl == 1.0 ? ( saturate( ( ( ( temp_output_21_0_g1170 == 3.0 ? 1.0 : 0.0 ) + ( temp_output_21_0_g1170 == 0.0 ? temp_output_1_0_g1170 : 0.0 ) + ( temp_output_21_0_g1170 == 1.0 ? temp_output_31_0_g1170 : 0.0 ) + ( temp_output_21_0_g1170 == 2.0 ? temp_output_30_0_g1170 : 0.0 ) ) * _SwirlSensitivity ) ) * ( rotatedValue35_g1168 - v.vertex.xyz ) ) : float3( 0,0,0 ) ) + ( _UseSway == 1.0 ? ( saturate( ( ( ( temp_output_21_0_g1176 == 3.0 ? 1.0 : 0.0 ) + ( temp_output_21_0_g1176 == 0.0 ? temp_output_1_0_g1176 : 0.0 ) + ( temp_output_21_0_g1176 == 1.0 ? temp_output_31_0_g1176 : 0.0 ) + ( temp_output_21_0_g1176 == 2.0 ? temp_output_30_0_g1176 : 0.0 ) ) * _SwaySensitivity ) ) * ( _SwayAmount * float3( 0.01,0.01,0.01 ) * simplePerlin2D5_g1174 * worldToObjDir18_g1174 ) ) : float3( 0,0,0 ) ) + ( _UseWave == 1.0 ? ( ( saturate( ( ( ( temp_output_21_0_g1179 == 3.0 ? 1.0 : 0.0 ) + ( temp_output_21_0_g1179 == 0.0 ? temp_output_1_0_g1179 : 0.0 ) + ( temp_output_21_0_g1179 == 1.0 ? temp_output_31_0_g1179 : 0.0 ) + ( temp_output_21_0_g1179 == 2.0 ? temp_output_30_0_g1179 : 0.0 ) ) * _WaveSensitivity ) ) * ( _WaveAmount * ( sin( (( break35_g1177.x + break35_g1177.y + break35_g1177.z )*_WaveNoiseScale + Time40_g1177) ) + sin( (( break49_g1177.x + break49_g1177.y + break49_g1177.z )*( _WaveNoiseScale * 0.5 ) + ( Time40_g1177 * 0.7 )) ) ) * worldToObjDir18_g1177 ) ) / ase_objectScale ) : float3( 0,0,0 ) ) );

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
					o.worldPos = positionWS;
				#endif

				o.clipPos = TransformWorldToHClip( positionWS );
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = o.clipPos;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif

				return o;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_color : COLOR;
				uint ase_vertexID : SV_VertexID;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.vertex = v.vertex;
				o.ase_color = v.ase_color;
				o.ase_vertexID = v.ase_vertexID;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				o.ase_vertexID = patch[0].ase_vertexID * bary.x + patch[1].ase_vertexID * bary.y + patch[2].ase_vertexID * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(VertexOutput IN  ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
					float3 WorldPosition = IN.worldPos;
				#endif

				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				float _CullMode_Instance = UNITY_ACCESS_INSTANCED_PROP(DistantLandsIllustrateOpaque,_CullMode);
				

				float Alpha = 1;
				float AlphaClipThreshold = 0.5;

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODFadeCrossFade( IN.clipPos );
				#endif
				return 0;
			}
			ENDHLSL
		}

		
		Pass
		{
			
            Name "SceneSelectionPass"
            Tags { "LightMode"="SceneSelectionPass" }

			Cull Off

			HLSLPROGRAM

			#define ASE_SRP_VERSION 140003


			#pragma vertex vert
			#pragma fragment frag

			#define ATTRIBUTES_NEED_NORMAL
			#define ATTRIBUTES_NEED_TANGENT
			#define SHADERPASS SHADERPASS_DEPTHONLY

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

			#define ASE_NEEDS_VERT_POSITION
			#pragma multi_compile_instancing


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_color : COLOR;
				uint ase_vertexID : SV_VertexID;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _TriplanarColor;
			float4 _NearColor;
			float4 _LightColor;
			float4 _ShadowColor;
			float4 _MainColor;
			float4 _GlintColor;
			float4 _RimLightColor;
			float4 _EmissionColor;
			float4 _SnowColor;
			float4 _FarColor;
			float4 _SpecularColor;
			float4 _PuddleColor;
			float3 _SwirlDirection;
			float3 _GradientChannelMask;
			float3 _SwayMask;
			float3 _FlutterMask;
			float3 _WaveInfluenceDirection;
			float3 _FlutterDirection;
			float3 _CustomNormalEllipseSize;
			float3 _CustomNormalDirection;
			float3 _WaveDirection2;
			float3 _SwayDirection;
			float3 _SwirlMask;
			float3 _WaveDirection1;
			float3 _TriplanarDirection;
			float3 _GradientPositionalOffset;
			float3 _WaveMask;
			float2 _EmissionScrolling1;
			float2 _EmissionScrolling2;
			float _LightRamp;
			float _UseShadows;
			float _PosterizeLight;
			float _UseFlutter;
			float _UseHalftone;
			float _MultiplyByLightColor;
			float _LightingMode;
			float _SnowScale;
			float _PuddleScale;
			float _ColorNumbers;
			float _ValueVariation;
			float _SaturationVariation;
			float _LightRampOffset;
			float _LightSteps;
			float _SpecularRampOffset;
			float _HalftoneOffset;
			float _NoiseScale;
			float _NoiseUVSource;
			float _NoiseAmountLight;
			float _NoiseAmountShadow;
			float _UseScreenNoise;
			float _EmissionEffectScale;
			float _EmissionUVSource;
			float _ScrollEmission;
			float _EmissionLightRatio;
			float _EmissionShadowRatio;
			float _UseEmission;
			float _GlintScale;
			float _MultiplyByLightRatio;
			float _UseGlint;
			float _UseRimLighting;
			float _RimLightLitIntensity;
			float _RimLightShadowIntensity;
			float _RimRamp;
			float _RimLightRampOffset;
			float _UseSpecular;
			float _UseModifiedNormals;
			float _SpecularRamp;
			float _HueVariation;
			float _AdditionalLightRamp;
			float _HalftoneMultiplier;
			float _HalftoneScale;
			float _UseHSVVariation;
			float _TriplanarSpace;
			float _VariationSource;
			float _SwaySpeed;
			float _SwayFramerate;
			float _SwayNoiseScale;
			float _SwayAmount;
			float _SwaySensitivity;
			float _SwayOffset;
			float _SwaySource;
			float _UseSway;
			float _SwirlSpeed;
			float _SwirlFramerate;
			float _SwirlNoiseScale;
			float _SwirlAmount;
			float _SwirlSensitivity;
			float _SwirlOffset;
			float _SwirlSource;
			float _UseSwirl;
			float _FlutterSpeed;
			float _FlutterFramerate;
			float _FlutterNoiseScale;
			float _FlutterAmount;
			float _FlutterSensitivity;
			float _FlutterOffset;
			float _FlutterSource;
			float _UseWave;
			float _VariationScale;
			float _WaveSource;
			float _WaveSensitivity;
			float _ValueShift;
			float _SaturationShift;
			float _HueShift;
			float _ClipTriplanar;
			float _TriplanarOffset;
			float _TriplanarMultiplier;
			float _NoiseFramerate;
			float _BlendStrength;
			float _NormalMode;
			float _GradientSensitivity;
			float _Space;
			float _GradientOffset;
			float _GradientSource;
			float _UseGradientShading;
			float _UseTriplanar;
			float _ClampAdjustments;
			float _UseColorAdjustments;
			float _PosterizeColors;
			float _UseCOZYPrecipitation;
			float _WaveSpeed;
			float _WaveFramerate;
			float _WaveNoiseScale;
			float _WaveAmount;
			float _WaveOffset;
			float _NoiseOffset;
			#ifdef ASE_TESSELLATION
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END

			UNITY_INSTANCING_BUFFER_START(DistantLandsIllustrateOpaque)
				UNITY_DEFINE_INSTANCED_PROP(float, _CullMode)
			UNITY_INSTANCING_BUFFER_END(DistantLandsIllustrateOpaque)


			float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }
			float snoise( float2 v )
			{
				const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
				float2 i = floor( v + dot( v, C.yy ) );
				float2 x0 = v - i + dot( i, C.xx );
				float2 i1;
				i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
				float4 x12 = x0.xyxy + C.xxzz;
				x12.xy -= i1;
				i = mod2D289( i );
				float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
				float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
				m = m * m;
				m = m * m;
				float3 x = 2.0 * frac( p * C.www ) - 1.0;
				float3 h = abs( x ) - 0.5;
				float3 ox = floor( x + 0.5 );
				float3 a0 = x - ox;
				m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
				float3 g;
				g.x = a0.x * x0.x + h.x * x0.y;
				g.yz = a0.yz * x12.xz + h.yz * x12.yw;
				return 130.0 * dot( m, g );
			}
			
			float3 RotateAroundAxis( float3 center, float3 original, float3 u, float angle )
			{
				original -= center;
				float C = cos( angle );
				float S = sin( angle );
				float t = 1 - C;
				float m00 = t * u.x * u.x + C;
				float m01 = t * u.x * u.y - S * u.z;
				float m02 = t * u.x * u.z + S * u.y;
				float m10 = t * u.x * u.y + S * u.z;
				float m11 = t * u.y * u.y + C;
				float m12 = t * u.y * u.z - S * u.x;
				float m20 = t * u.x * u.z - S * u.y;
				float m21 = t * u.y * u.z + S * u.x;
				float m22 = t * u.z * u.z + C;
				float3x3 finalMatrix = float3x3( m00, m01, m02, m10, m11, m12, m20, m21, m22 );
				return mul( finalMatrix, original ) + center;
			}
			

			int _ObjectId;
			int _PassValue;

			struct SurfaceDescription
			{
				float Alpha;
				float AlphaClipThreshold;
			};

			VertexOutput VertexFunction(VertexInput v  )
			{
				VertexOutput o;
				ZERO_INITIALIZE(VertexOutput, o);

				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float temp_output_21_0_g1173 = _FlutterSource;
				float3 temp_output_23_0_g1173 = v.vertex.xyz;
				float temp_output_1_0_g1173 = distance( ( -_FlutterOffset + temp_output_23_0_g1173 ) , float3( 0,0,0 ) );
				float3 temp_output_22_0_g1173 = _FlutterMask;
				float temp_output_31_0_g1173 = length( ( ( -_FlutterOffset + temp_output_23_0_g1173 ) * temp_output_22_0_g1173 ) );
				float temp_output_30_0_g1173 = length( ( float4( temp_output_22_0_g1173 , 0.0 ) * v.ase_color ) );
				float temp_output_2_0_g1172 = _FlutterFramerate;
				float2 temp_cast_1 = (v.ase_vertexID*( _FlutterNoiseScale * 0.1 ) + ( ( round( ( _TimeParameters.x * temp_output_2_0_g1172 ) ) / temp_output_2_0_g1172 ) * 0.1 * _FlutterSpeed ));
				float simplePerlin2D12_g1171 = snoise( temp_cast_1*8.91 );
				float3 worldToObjDir34_g1171 = normalize( mul( GetWorldToObjectMatrix(), float4( _FlutterDirection, 0 ) ).xyz );
				float temp_output_21_0_g1170 = _SwirlSource;
				float3 temp_output_23_0_g1170 = v.vertex.xyz;
				float temp_output_1_0_g1170 = distance( ( -_SwirlOffset + temp_output_23_0_g1170 ) , float3( 0,0,0 ) );
				float3 temp_output_22_0_g1170 = _SwirlMask;
				float temp_output_31_0_g1170 = length( ( ( -_SwirlOffset + temp_output_23_0_g1170 ) * temp_output_22_0_g1170 ) );
				float temp_output_30_0_g1170 = length( ( float4( temp_output_22_0_g1170 , 0.0 ) * v.ase_color ) );
				float3 worldToObjDir40_g1168 = normalize( mul( GetWorldToObjectMatrix(), float4( _SwirlDirection, 0 ) ).xyz );
				float temp_output_2_0_g1169 = _SwirlFramerate;
				float simplePerlin2D12_g1168 = snoise( (v.vertex.xyz*( _SwirlNoiseScale * 0.1 ) + ( ( round( ( _TimeParameters.x * temp_output_2_0_g1169 ) ) / temp_output_2_0_g1169 ) * 0.1 * _SwirlSpeed )).xy*8.91 );
				float3 rotatedValue35_g1168 = RotateAroundAxis( float3( 0,0,0 ), v.vertex.xyz, worldToObjDir40_g1168, ( _SwirlAmount * 0.01 * simplePerlin2D12_g1168 ) );
				float temp_output_21_0_g1176 = _SwaySource;
				float3 temp_output_23_0_g1176 = v.vertex.xyz;
				float temp_output_1_0_g1176 = distance( ( -_SwayOffset + temp_output_23_0_g1176 ) , float3( 0,0,0 ) );
				float3 temp_output_22_0_g1176 = _SwayMask;
				float temp_output_31_0_g1176 = length( ( ( -_SwayOffset + temp_output_23_0_g1176 ) * temp_output_22_0_g1176 ) );
				float temp_output_30_0_g1176 = length( ( float4( temp_output_22_0_g1176 , 0.0 ) * v.ase_color ) );
				float temp_output_2_0_g1175 = _SwayFramerate;
				float simplePerlin2D5_g1174 = snoise( (v.vertex.xyz*( _SwayNoiseScale * 0.1 ) + ( ( round( ( _TimeParameters.x * temp_output_2_0_g1175 ) ) / temp_output_2_0_g1175 ) * 0.1 * _SwaySpeed )).xy*8.91 );
				simplePerlin2D5_g1174 = simplePerlin2D5_g1174*0.5 + 0.5;
				float3 worldToObjDir18_g1174 = normalize( mul( GetWorldToObjectMatrix(), float4( _SwayDirection, 0 ) ).xyz );
				float temp_output_21_0_g1179 = _WaveSource;
				float3 temp_output_23_0_g1179 = v.vertex.xyz;
				float temp_output_1_0_g1179 = distance( ( -_WaveOffset + temp_output_23_0_g1179 ) , float3( 0,0,0 ) );
				float3 temp_output_22_0_g1179 = _WaveMask;
				float temp_output_31_0_g1179 = length( ( ( -_WaveOffset + temp_output_23_0_g1179 ) * temp_output_22_0_g1179 ) );
				float temp_output_30_0_g1179 = length( ( float4( temp_output_22_0_g1179 , 0.0 ) * v.ase_color ) );
				float3 ase_worldPos = TransformObjectToWorld( (v.vertex).xyz );
				float3 normalizeResult37_g1177 = normalize( _WaveDirection1 );
				float3 break35_g1177 = ( ase_worldPos * normalizeResult37_g1177 );
				float temp_output_2_0_g1178 = _WaveFramerate;
				float Time40_g1177 = ( ( round( ( _TimeParameters.x * temp_output_2_0_g1178 ) ) / temp_output_2_0_g1178 ) * _WaveSpeed );
				float3 normalizeResult52_g1177 = normalize( _WaveDirection2 );
				float3 break49_g1177 = ( ase_worldPos * normalizeResult52_g1177 );
				float3 worldToObjDir18_g1177 = normalize( mul( GetWorldToObjectMatrix(), float4( _WaveInfluenceDirection, 0 ) ).xyz );
				float3 ase_objectScale = float3( length( GetObjectToWorldMatrix()[ 0 ].xyz ), length( GetObjectToWorldMatrix()[ 1 ].xyz ), length( GetObjectToWorldMatrix()[ 2 ].xyz ) );
				

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = ( ( _UseFlutter == 1.0 ? ( saturate( ( ( ( temp_output_21_0_g1173 == 3.0 ? 1.0 : 0.0 ) + ( temp_output_21_0_g1173 == 0.0 ? temp_output_1_0_g1173 : 0.0 ) + ( temp_output_21_0_g1173 == 1.0 ? temp_output_31_0_g1173 : 0.0 ) + ( temp_output_21_0_g1173 == 2.0 ? temp_output_30_0_g1173 : 0.0 ) ) * _FlutterSensitivity ) ) * ( _FlutterAmount * float3( 0.001,0.001,0.001 ) * simplePerlin2D12_g1171 * worldToObjDir34_g1171 ) ) : float3( 0,0,0 ) ) + ( _UseSwirl == 1.0 ? ( saturate( ( ( ( temp_output_21_0_g1170 == 3.0 ? 1.0 : 0.0 ) + ( temp_output_21_0_g1170 == 0.0 ? temp_output_1_0_g1170 : 0.0 ) + ( temp_output_21_0_g1170 == 1.0 ? temp_output_31_0_g1170 : 0.0 ) + ( temp_output_21_0_g1170 == 2.0 ? temp_output_30_0_g1170 : 0.0 ) ) * _SwirlSensitivity ) ) * ( rotatedValue35_g1168 - v.vertex.xyz ) ) : float3( 0,0,0 ) ) + ( _UseSway == 1.0 ? ( saturate( ( ( ( temp_output_21_0_g1176 == 3.0 ? 1.0 : 0.0 ) + ( temp_output_21_0_g1176 == 0.0 ? temp_output_1_0_g1176 : 0.0 ) + ( temp_output_21_0_g1176 == 1.0 ? temp_output_31_0_g1176 : 0.0 ) + ( temp_output_21_0_g1176 == 2.0 ? temp_output_30_0_g1176 : 0.0 ) ) * _SwaySensitivity ) ) * ( _SwayAmount * float3( 0.01,0.01,0.01 ) * simplePerlin2D5_g1174 * worldToObjDir18_g1174 ) ) : float3( 0,0,0 ) ) + ( _UseWave == 1.0 ? ( ( saturate( ( ( ( temp_output_21_0_g1179 == 3.0 ? 1.0 : 0.0 ) + ( temp_output_21_0_g1179 == 0.0 ? temp_output_1_0_g1179 : 0.0 ) + ( temp_output_21_0_g1179 == 1.0 ? temp_output_31_0_g1179 : 0.0 ) + ( temp_output_21_0_g1179 == 2.0 ? temp_output_30_0_g1179 : 0.0 ) ) * _WaveSensitivity ) ) * ( _WaveAmount * ( sin( (( break35_g1177.x + break35_g1177.y + break35_g1177.z )*_WaveNoiseScale + Time40_g1177) ) + sin( (( break49_g1177.x + break49_g1177.y + break49_g1177.z )*( _WaveNoiseScale * 0.5 ) + ( Time40_g1177 * 0.7 )) ) ) * worldToObjDir18_g1177 ) ) / ase_objectScale ) : float3( 0,0,0 ) ) );

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				o.clipPos = TransformWorldToHClip(positionWS);

				return o;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_color : COLOR;
				uint ase_vertexID : SV_VertexID;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.vertex = v.vertex;
				o.ase_color = v.ase_color;
				o.ase_vertexID = v.ase_vertexID;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
				return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				o.ase_vertexID = patch[0].ase_vertexID * bary.x + patch[1].ase_vertexID * bary.y + patch[2].ase_vertexID * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(VertexOutput IN ) : SV_TARGET
			{
				SurfaceDescription surfaceDescription = (SurfaceDescription)0;

				float _CullMode_Instance = UNITY_ACCESS_INSTANCED_PROP(DistantLandsIllustrateOpaque,_CullMode);
				

				surfaceDescription.Alpha = 1;
				surfaceDescription.AlphaClipThreshold = 0.5;

				#if _ALPHATEST_ON
					float alphaClipThreshold = 0.01f;
					#if ALPHA_CLIP_THRESHOLD
						alphaClipThreshold = surfaceDescription.AlphaClipThreshold;
					#endif
					clip(surfaceDescription.Alpha - alphaClipThreshold);
				#endif

				half4 outColor = half4(_ObjectId, _PassValue, 1.0, 1.0);
				return outColor;
			}
			ENDHLSL
		}

		
		Pass
		{
			
            Name "ScenePickingPass"
            Tags { "LightMode"="Picking" }

			HLSLPROGRAM

			#define ASE_SRP_VERSION 140003


			#pragma vertex vert
			#pragma fragment frag

			#define ATTRIBUTES_NEED_NORMAL
			#define ATTRIBUTES_NEED_TANGENT
			#define SHADERPASS SHADERPASS_DEPTHONLY

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

			#define ASE_NEEDS_VERT_POSITION
			#pragma multi_compile_instancing


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_color : COLOR;
				uint ase_vertexID : SV_VertexID;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _TriplanarColor;
			float4 _NearColor;
			float4 _LightColor;
			float4 _ShadowColor;
			float4 _MainColor;
			float4 _GlintColor;
			float4 _RimLightColor;
			float4 _EmissionColor;
			float4 _SnowColor;
			float4 _FarColor;
			float4 _SpecularColor;
			float4 _PuddleColor;
			float3 _SwirlDirection;
			float3 _GradientChannelMask;
			float3 _SwayMask;
			float3 _FlutterMask;
			float3 _WaveInfluenceDirection;
			float3 _FlutterDirection;
			float3 _CustomNormalEllipseSize;
			float3 _CustomNormalDirection;
			float3 _WaveDirection2;
			float3 _SwayDirection;
			float3 _SwirlMask;
			float3 _WaveDirection1;
			float3 _TriplanarDirection;
			float3 _GradientPositionalOffset;
			float3 _WaveMask;
			float2 _EmissionScrolling1;
			float2 _EmissionScrolling2;
			float _LightRamp;
			float _UseShadows;
			float _PosterizeLight;
			float _UseFlutter;
			float _UseHalftone;
			float _MultiplyByLightColor;
			float _LightingMode;
			float _SnowScale;
			float _PuddleScale;
			float _ColorNumbers;
			float _ValueVariation;
			float _SaturationVariation;
			float _LightRampOffset;
			float _LightSteps;
			float _SpecularRampOffset;
			float _HalftoneOffset;
			float _NoiseScale;
			float _NoiseUVSource;
			float _NoiseAmountLight;
			float _NoiseAmountShadow;
			float _UseScreenNoise;
			float _EmissionEffectScale;
			float _EmissionUVSource;
			float _ScrollEmission;
			float _EmissionLightRatio;
			float _EmissionShadowRatio;
			float _UseEmission;
			float _GlintScale;
			float _MultiplyByLightRatio;
			float _UseGlint;
			float _UseRimLighting;
			float _RimLightLitIntensity;
			float _RimLightShadowIntensity;
			float _RimRamp;
			float _RimLightRampOffset;
			float _UseSpecular;
			float _UseModifiedNormals;
			float _SpecularRamp;
			float _HueVariation;
			float _AdditionalLightRamp;
			float _HalftoneMultiplier;
			float _HalftoneScale;
			float _UseHSVVariation;
			float _TriplanarSpace;
			float _VariationSource;
			float _SwaySpeed;
			float _SwayFramerate;
			float _SwayNoiseScale;
			float _SwayAmount;
			float _SwaySensitivity;
			float _SwayOffset;
			float _SwaySource;
			float _UseSway;
			float _SwirlSpeed;
			float _SwirlFramerate;
			float _SwirlNoiseScale;
			float _SwirlAmount;
			float _SwirlSensitivity;
			float _SwirlOffset;
			float _SwirlSource;
			float _UseSwirl;
			float _FlutterSpeed;
			float _FlutterFramerate;
			float _FlutterNoiseScale;
			float _FlutterAmount;
			float _FlutterSensitivity;
			float _FlutterOffset;
			float _FlutterSource;
			float _UseWave;
			float _VariationScale;
			float _WaveSource;
			float _WaveSensitivity;
			float _ValueShift;
			float _SaturationShift;
			float _HueShift;
			float _ClipTriplanar;
			float _TriplanarOffset;
			float _TriplanarMultiplier;
			float _NoiseFramerate;
			float _BlendStrength;
			float _NormalMode;
			float _GradientSensitivity;
			float _Space;
			float _GradientOffset;
			float _GradientSource;
			float _UseGradientShading;
			float _UseTriplanar;
			float _ClampAdjustments;
			float _UseColorAdjustments;
			float _PosterizeColors;
			float _UseCOZYPrecipitation;
			float _WaveSpeed;
			float _WaveFramerate;
			float _WaveNoiseScale;
			float _WaveAmount;
			float _WaveOffset;
			float _NoiseOffset;
			#ifdef ASE_TESSELLATION
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END

			UNITY_INSTANCING_BUFFER_START(DistantLandsIllustrateOpaque)
				UNITY_DEFINE_INSTANCED_PROP(float, _CullMode)
			UNITY_INSTANCING_BUFFER_END(DistantLandsIllustrateOpaque)


			float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }
			float snoise( float2 v )
			{
				const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
				float2 i = floor( v + dot( v, C.yy ) );
				float2 x0 = v - i + dot( i, C.xx );
				float2 i1;
				i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
				float4 x12 = x0.xyxy + C.xxzz;
				x12.xy -= i1;
				i = mod2D289( i );
				float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
				float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
				m = m * m;
				m = m * m;
				float3 x = 2.0 * frac( p * C.www ) - 1.0;
				float3 h = abs( x ) - 0.5;
				float3 ox = floor( x + 0.5 );
				float3 a0 = x - ox;
				m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
				float3 g;
				g.x = a0.x * x0.x + h.x * x0.y;
				g.yz = a0.yz * x12.xz + h.yz * x12.yw;
				return 130.0 * dot( m, g );
			}
			
			float3 RotateAroundAxis( float3 center, float3 original, float3 u, float angle )
			{
				original -= center;
				float C = cos( angle );
				float S = sin( angle );
				float t = 1 - C;
				float m00 = t * u.x * u.x + C;
				float m01 = t * u.x * u.y - S * u.z;
				float m02 = t * u.x * u.z + S * u.y;
				float m10 = t * u.x * u.y + S * u.z;
				float m11 = t * u.y * u.y + C;
				float m12 = t * u.y * u.z - S * u.x;
				float m20 = t * u.x * u.z - S * u.y;
				float m21 = t * u.y * u.z + S * u.x;
				float m22 = t * u.z * u.z + C;
				float3x3 finalMatrix = float3x3( m00, m01, m02, m10, m11, m12, m20, m21, m22 );
				return mul( finalMatrix, original ) + center;
			}
			

			float4 _SelectionID;


			struct SurfaceDescription
			{
				float Alpha;
				float AlphaClipThreshold;
			};

			VertexOutput VertexFunction(VertexInput v  )
			{
				VertexOutput o;
				ZERO_INITIALIZE(VertexOutput, o);

				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float temp_output_21_0_g1173 = _FlutterSource;
				float3 temp_output_23_0_g1173 = v.vertex.xyz;
				float temp_output_1_0_g1173 = distance( ( -_FlutterOffset + temp_output_23_0_g1173 ) , float3( 0,0,0 ) );
				float3 temp_output_22_0_g1173 = _FlutterMask;
				float temp_output_31_0_g1173 = length( ( ( -_FlutterOffset + temp_output_23_0_g1173 ) * temp_output_22_0_g1173 ) );
				float temp_output_30_0_g1173 = length( ( float4( temp_output_22_0_g1173 , 0.0 ) * v.ase_color ) );
				float temp_output_2_0_g1172 = _FlutterFramerate;
				float2 temp_cast_1 = (v.ase_vertexID*( _FlutterNoiseScale * 0.1 ) + ( ( round( ( _TimeParameters.x * temp_output_2_0_g1172 ) ) / temp_output_2_0_g1172 ) * 0.1 * _FlutterSpeed ));
				float simplePerlin2D12_g1171 = snoise( temp_cast_1*8.91 );
				float3 worldToObjDir34_g1171 = normalize( mul( GetWorldToObjectMatrix(), float4( _FlutterDirection, 0 ) ).xyz );
				float temp_output_21_0_g1170 = _SwirlSource;
				float3 temp_output_23_0_g1170 = v.vertex.xyz;
				float temp_output_1_0_g1170 = distance( ( -_SwirlOffset + temp_output_23_0_g1170 ) , float3( 0,0,0 ) );
				float3 temp_output_22_0_g1170 = _SwirlMask;
				float temp_output_31_0_g1170 = length( ( ( -_SwirlOffset + temp_output_23_0_g1170 ) * temp_output_22_0_g1170 ) );
				float temp_output_30_0_g1170 = length( ( float4( temp_output_22_0_g1170 , 0.0 ) * v.ase_color ) );
				float3 worldToObjDir40_g1168 = normalize( mul( GetWorldToObjectMatrix(), float4( _SwirlDirection, 0 ) ).xyz );
				float temp_output_2_0_g1169 = _SwirlFramerate;
				float simplePerlin2D12_g1168 = snoise( (v.vertex.xyz*( _SwirlNoiseScale * 0.1 ) + ( ( round( ( _TimeParameters.x * temp_output_2_0_g1169 ) ) / temp_output_2_0_g1169 ) * 0.1 * _SwirlSpeed )).xy*8.91 );
				float3 rotatedValue35_g1168 = RotateAroundAxis( float3( 0,0,0 ), v.vertex.xyz, worldToObjDir40_g1168, ( _SwirlAmount * 0.01 * simplePerlin2D12_g1168 ) );
				float temp_output_21_0_g1176 = _SwaySource;
				float3 temp_output_23_0_g1176 = v.vertex.xyz;
				float temp_output_1_0_g1176 = distance( ( -_SwayOffset + temp_output_23_0_g1176 ) , float3( 0,0,0 ) );
				float3 temp_output_22_0_g1176 = _SwayMask;
				float temp_output_31_0_g1176 = length( ( ( -_SwayOffset + temp_output_23_0_g1176 ) * temp_output_22_0_g1176 ) );
				float temp_output_30_0_g1176 = length( ( float4( temp_output_22_0_g1176 , 0.0 ) * v.ase_color ) );
				float temp_output_2_0_g1175 = _SwayFramerate;
				float simplePerlin2D5_g1174 = snoise( (v.vertex.xyz*( _SwayNoiseScale * 0.1 ) + ( ( round( ( _TimeParameters.x * temp_output_2_0_g1175 ) ) / temp_output_2_0_g1175 ) * 0.1 * _SwaySpeed )).xy*8.91 );
				simplePerlin2D5_g1174 = simplePerlin2D5_g1174*0.5 + 0.5;
				float3 worldToObjDir18_g1174 = normalize( mul( GetWorldToObjectMatrix(), float4( _SwayDirection, 0 ) ).xyz );
				float temp_output_21_0_g1179 = _WaveSource;
				float3 temp_output_23_0_g1179 = v.vertex.xyz;
				float temp_output_1_0_g1179 = distance( ( -_WaveOffset + temp_output_23_0_g1179 ) , float3( 0,0,0 ) );
				float3 temp_output_22_0_g1179 = _WaveMask;
				float temp_output_31_0_g1179 = length( ( ( -_WaveOffset + temp_output_23_0_g1179 ) * temp_output_22_0_g1179 ) );
				float temp_output_30_0_g1179 = length( ( float4( temp_output_22_0_g1179 , 0.0 ) * v.ase_color ) );
				float3 ase_worldPos = TransformObjectToWorld( (v.vertex).xyz );
				float3 normalizeResult37_g1177 = normalize( _WaveDirection1 );
				float3 break35_g1177 = ( ase_worldPos * normalizeResult37_g1177 );
				float temp_output_2_0_g1178 = _WaveFramerate;
				float Time40_g1177 = ( ( round( ( _TimeParameters.x * temp_output_2_0_g1178 ) ) / temp_output_2_0_g1178 ) * _WaveSpeed );
				float3 normalizeResult52_g1177 = normalize( _WaveDirection2 );
				float3 break49_g1177 = ( ase_worldPos * normalizeResult52_g1177 );
				float3 worldToObjDir18_g1177 = normalize( mul( GetWorldToObjectMatrix(), float4( _WaveInfluenceDirection, 0 ) ).xyz );
				float3 ase_objectScale = float3( length( GetObjectToWorldMatrix()[ 0 ].xyz ), length( GetObjectToWorldMatrix()[ 1 ].xyz ), length( GetObjectToWorldMatrix()[ 2 ].xyz ) );
				
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = ( ( _UseFlutter == 1.0 ? ( saturate( ( ( ( temp_output_21_0_g1173 == 3.0 ? 1.0 : 0.0 ) + ( temp_output_21_0_g1173 == 0.0 ? temp_output_1_0_g1173 : 0.0 ) + ( temp_output_21_0_g1173 == 1.0 ? temp_output_31_0_g1173 : 0.0 ) + ( temp_output_21_0_g1173 == 2.0 ? temp_output_30_0_g1173 : 0.0 ) ) * _FlutterSensitivity ) ) * ( _FlutterAmount * float3( 0.001,0.001,0.001 ) * simplePerlin2D12_g1171 * worldToObjDir34_g1171 ) ) : float3( 0,0,0 ) ) + ( _UseSwirl == 1.0 ? ( saturate( ( ( ( temp_output_21_0_g1170 == 3.0 ? 1.0 : 0.0 ) + ( temp_output_21_0_g1170 == 0.0 ? temp_output_1_0_g1170 : 0.0 ) + ( temp_output_21_0_g1170 == 1.0 ? temp_output_31_0_g1170 : 0.0 ) + ( temp_output_21_0_g1170 == 2.0 ? temp_output_30_0_g1170 : 0.0 ) ) * _SwirlSensitivity ) ) * ( rotatedValue35_g1168 - v.vertex.xyz ) ) : float3( 0,0,0 ) ) + ( _UseSway == 1.0 ? ( saturate( ( ( ( temp_output_21_0_g1176 == 3.0 ? 1.0 : 0.0 ) + ( temp_output_21_0_g1176 == 0.0 ? temp_output_1_0_g1176 : 0.0 ) + ( temp_output_21_0_g1176 == 1.0 ? temp_output_31_0_g1176 : 0.0 ) + ( temp_output_21_0_g1176 == 2.0 ? temp_output_30_0_g1176 : 0.0 ) ) * _SwaySensitivity ) ) * ( _SwayAmount * float3( 0.01,0.01,0.01 ) * simplePerlin2D5_g1174 * worldToObjDir18_g1174 ) ) : float3( 0,0,0 ) ) + ( _UseWave == 1.0 ? ( ( saturate( ( ( ( temp_output_21_0_g1179 == 3.0 ? 1.0 : 0.0 ) + ( temp_output_21_0_g1179 == 0.0 ? temp_output_1_0_g1179 : 0.0 ) + ( temp_output_21_0_g1179 == 1.0 ? temp_output_31_0_g1179 : 0.0 ) + ( temp_output_21_0_g1179 == 2.0 ? temp_output_30_0_g1179 : 0.0 ) ) * _WaveSensitivity ) ) * ( _WaveAmount * ( sin( (( break35_g1177.x + break35_g1177.y + break35_g1177.z )*_WaveNoiseScale + Time40_g1177) ) + sin( (( break49_g1177.x + break49_g1177.y + break49_g1177.z )*( _WaveNoiseScale * 0.5 ) + ( Time40_g1177 * 0.7 )) ) ) * worldToObjDir18_g1177 ) ) / ase_objectScale ) : float3( 0,0,0 ) ) );
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif
				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				o.clipPos = TransformWorldToHClip(positionWS);
				return o;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_color : COLOR;
				uint ase_vertexID : SV_VertexID;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.vertex = v.vertex;
				o.ase_color = v.ase_color;
				o.ase_vertexID = v.ase_vertexID;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
				return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				o.ase_vertexID = patch[0].ase_vertexID * bary.x + patch[1].ase_vertexID * bary.y + patch[2].ase_vertexID * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(VertexOutput IN ) : SV_TARGET
			{
				SurfaceDescription surfaceDescription = (SurfaceDescription)0;

				float _CullMode_Instance = UNITY_ACCESS_INSTANCED_PROP(DistantLandsIllustrateOpaque,_CullMode);
				

				surfaceDescription.Alpha = 1;
				surfaceDescription.AlphaClipThreshold = 0.5;

				#if _ALPHATEST_ON
					float alphaClipThreshold = 0.01f;
					#if ALPHA_CLIP_THRESHOLD
						alphaClipThreshold = surfaceDescription.AlphaClipThreshold;
					#endif
					clip(surfaceDescription.Alpha - alphaClipThreshold);
				#endif

				half4 outColor = 0;
				outColor = _SelectionID;

				return outColor;
			}

			ENDHLSL
		}

		
		Pass
		{
			
            Name "DepthNormals"
            Tags { "LightMode"="DepthNormalsOnly" }

			ZTest LEqual
			ZWrite On


			HLSLPROGRAM

			#define ASE_SRP_VERSION 140003


			#pragma vertex vert
			#pragma fragment frag

			#pragma multi_compile_fragment _ _WRITE_RENDERING_LAYERS
        	#pragma multi_compile_fragment _ _GBUFFER_NORMALS_OCT

			#define ATTRIBUTES_NEED_NORMAL
			#define ATTRIBUTES_NEED_TANGENT
			#define VARYINGS_NEED_NORMAL_WS

			#define SHADERPASS SHADERPASS_DEPTHNORMALSONLY

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/LODCrossFade.hlsl"

			#define ASE_NEEDS_VERT_POSITION
			#pragma multi_compile_instancing


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_color : COLOR;
				uint ase_vertexID : SV_VertexID;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				float3 normalWS : TEXCOORD0;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _TriplanarColor;
			float4 _NearColor;
			float4 _LightColor;
			float4 _ShadowColor;
			float4 _MainColor;
			float4 _GlintColor;
			float4 _RimLightColor;
			float4 _EmissionColor;
			float4 _SnowColor;
			float4 _FarColor;
			float4 _SpecularColor;
			float4 _PuddleColor;
			float3 _SwirlDirection;
			float3 _GradientChannelMask;
			float3 _SwayMask;
			float3 _FlutterMask;
			float3 _WaveInfluenceDirection;
			float3 _FlutterDirection;
			float3 _CustomNormalEllipseSize;
			float3 _CustomNormalDirection;
			float3 _WaveDirection2;
			float3 _SwayDirection;
			float3 _SwirlMask;
			float3 _WaveDirection1;
			float3 _TriplanarDirection;
			float3 _GradientPositionalOffset;
			float3 _WaveMask;
			float2 _EmissionScrolling1;
			float2 _EmissionScrolling2;
			float _LightRamp;
			float _UseShadows;
			float _PosterizeLight;
			float _UseFlutter;
			float _UseHalftone;
			float _MultiplyByLightColor;
			float _LightingMode;
			float _SnowScale;
			float _PuddleScale;
			float _ColorNumbers;
			float _ValueVariation;
			float _SaturationVariation;
			float _LightRampOffset;
			float _LightSteps;
			float _SpecularRampOffset;
			float _HalftoneOffset;
			float _NoiseScale;
			float _NoiseUVSource;
			float _NoiseAmountLight;
			float _NoiseAmountShadow;
			float _UseScreenNoise;
			float _EmissionEffectScale;
			float _EmissionUVSource;
			float _ScrollEmission;
			float _EmissionLightRatio;
			float _EmissionShadowRatio;
			float _UseEmission;
			float _GlintScale;
			float _MultiplyByLightRatio;
			float _UseGlint;
			float _UseRimLighting;
			float _RimLightLitIntensity;
			float _RimLightShadowIntensity;
			float _RimRamp;
			float _RimLightRampOffset;
			float _UseSpecular;
			float _UseModifiedNormals;
			float _SpecularRamp;
			float _HueVariation;
			float _AdditionalLightRamp;
			float _HalftoneMultiplier;
			float _HalftoneScale;
			float _UseHSVVariation;
			float _TriplanarSpace;
			float _VariationSource;
			float _SwaySpeed;
			float _SwayFramerate;
			float _SwayNoiseScale;
			float _SwayAmount;
			float _SwaySensitivity;
			float _SwayOffset;
			float _SwaySource;
			float _UseSway;
			float _SwirlSpeed;
			float _SwirlFramerate;
			float _SwirlNoiseScale;
			float _SwirlAmount;
			float _SwirlSensitivity;
			float _SwirlOffset;
			float _SwirlSource;
			float _UseSwirl;
			float _FlutterSpeed;
			float _FlutterFramerate;
			float _FlutterNoiseScale;
			float _FlutterAmount;
			float _FlutterSensitivity;
			float _FlutterOffset;
			float _FlutterSource;
			float _UseWave;
			float _VariationScale;
			float _WaveSource;
			float _WaveSensitivity;
			float _ValueShift;
			float _SaturationShift;
			float _HueShift;
			float _ClipTriplanar;
			float _TriplanarOffset;
			float _TriplanarMultiplier;
			float _NoiseFramerate;
			float _BlendStrength;
			float _NormalMode;
			float _GradientSensitivity;
			float _Space;
			float _GradientOffset;
			float _GradientSource;
			float _UseGradientShading;
			float _UseTriplanar;
			float _ClampAdjustments;
			float _UseColorAdjustments;
			float _PosterizeColors;
			float _UseCOZYPrecipitation;
			float _WaveSpeed;
			float _WaveFramerate;
			float _WaveNoiseScale;
			float _WaveAmount;
			float _WaveOffset;
			float _NoiseOffset;
			#ifdef ASE_TESSELLATION
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END

			UNITY_INSTANCING_BUFFER_START(DistantLandsIllustrateOpaque)
				UNITY_DEFINE_INSTANCED_PROP(float, _CullMode)
			UNITY_INSTANCING_BUFFER_END(DistantLandsIllustrateOpaque)


			float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }
			float snoise( float2 v )
			{
				const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
				float2 i = floor( v + dot( v, C.yy ) );
				float2 x0 = v - i + dot( i, C.xx );
				float2 i1;
				i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
				float4 x12 = x0.xyxy + C.xxzz;
				x12.xy -= i1;
				i = mod2D289( i );
				float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
				float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
				m = m * m;
				m = m * m;
				float3 x = 2.0 * frac( p * C.www ) - 1.0;
				float3 h = abs( x ) - 0.5;
				float3 ox = floor( x + 0.5 );
				float3 a0 = x - ox;
				m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
				float3 g;
				g.x = a0.x * x0.x + h.x * x0.y;
				g.yz = a0.yz * x12.xz + h.yz * x12.yw;
				return 130.0 * dot( m, g );
			}
			
			float3 RotateAroundAxis( float3 center, float3 original, float3 u, float angle )
			{
				original -= center;
				float C = cos( angle );
				float S = sin( angle );
				float t = 1 - C;
				float m00 = t * u.x * u.x + C;
				float m01 = t * u.x * u.y - S * u.z;
				float m02 = t * u.x * u.z + S * u.y;
				float m10 = t * u.x * u.y + S * u.z;
				float m11 = t * u.y * u.y + C;
				float m12 = t * u.y * u.z - S * u.x;
				float m20 = t * u.x * u.z - S * u.y;
				float m21 = t * u.y * u.z + S * u.x;
				float m22 = t * u.z * u.z + C;
				float3x3 finalMatrix = float3x3( m00, m01, m02, m10, m11, m12, m20, m21, m22 );
				return mul( finalMatrix, original ) + center;
			}
			

			struct SurfaceDescription
			{
				float Alpha;
				float AlphaClipThreshold;
			};

			VertexOutput VertexFunction(VertexInput v  )
			{
				VertexOutput o;
				ZERO_INITIALIZE(VertexOutput, o);

				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float temp_output_21_0_g1173 = _FlutterSource;
				float3 temp_output_23_0_g1173 = v.vertex.xyz;
				float temp_output_1_0_g1173 = distance( ( -_FlutterOffset + temp_output_23_0_g1173 ) , float3( 0,0,0 ) );
				float3 temp_output_22_0_g1173 = _FlutterMask;
				float temp_output_31_0_g1173 = length( ( ( -_FlutterOffset + temp_output_23_0_g1173 ) * temp_output_22_0_g1173 ) );
				float temp_output_30_0_g1173 = length( ( float4( temp_output_22_0_g1173 , 0.0 ) * v.ase_color ) );
				float temp_output_2_0_g1172 = _FlutterFramerate;
				float2 temp_cast_1 = (v.ase_vertexID*( _FlutterNoiseScale * 0.1 ) + ( ( round( ( _TimeParameters.x * temp_output_2_0_g1172 ) ) / temp_output_2_0_g1172 ) * 0.1 * _FlutterSpeed ));
				float simplePerlin2D12_g1171 = snoise( temp_cast_1*8.91 );
				float3 worldToObjDir34_g1171 = normalize( mul( GetWorldToObjectMatrix(), float4( _FlutterDirection, 0 ) ).xyz );
				float temp_output_21_0_g1170 = _SwirlSource;
				float3 temp_output_23_0_g1170 = v.vertex.xyz;
				float temp_output_1_0_g1170 = distance( ( -_SwirlOffset + temp_output_23_0_g1170 ) , float3( 0,0,0 ) );
				float3 temp_output_22_0_g1170 = _SwirlMask;
				float temp_output_31_0_g1170 = length( ( ( -_SwirlOffset + temp_output_23_0_g1170 ) * temp_output_22_0_g1170 ) );
				float temp_output_30_0_g1170 = length( ( float4( temp_output_22_0_g1170 , 0.0 ) * v.ase_color ) );
				float3 worldToObjDir40_g1168 = normalize( mul( GetWorldToObjectMatrix(), float4( _SwirlDirection, 0 ) ).xyz );
				float temp_output_2_0_g1169 = _SwirlFramerate;
				float simplePerlin2D12_g1168 = snoise( (v.vertex.xyz*( _SwirlNoiseScale * 0.1 ) + ( ( round( ( _TimeParameters.x * temp_output_2_0_g1169 ) ) / temp_output_2_0_g1169 ) * 0.1 * _SwirlSpeed )).xy*8.91 );
				float3 rotatedValue35_g1168 = RotateAroundAxis( float3( 0,0,0 ), v.vertex.xyz, worldToObjDir40_g1168, ( _SwirlAmount * 0.01 * simplePerlin2D12_g1168 ) );
				float temp_output_21_0_g1176 = _SwaySource;
				float3 temp_output_23_0_g1176 = v.vertex.xyz;
				float temp_output_1_0_g1176 = distance( ( -_SwayOffset + temp_output_23_0_g1176 ) , float3( 0,0,0 ) );
				float3 temp_output_22_0_g1176 = _SwayMask;
				float temp_output_31_0_g1176 = length( ( ( -_SwayOffset + temp_output_23_0_g1176 ) * temp_output_22_0_g1176 ) );
				float temp_output_30_0_g1176 = length( ( float4( temp_output_22_0_g1176 , 0.0 ) * v.ase_color ) );
				float temp_output_2_0_g1175 = _SwayFramerate;
				float simplePerlin2D5_g1174 = snoise( (v.vertex.xyz*( _SwayNoiseScale * 0.1 ) + ( ( round( ( _TimeParameters.x * temp_output_2_0_g1175 ) ) / temp_output_2_0_g1175 ) * 0.1 * _SwaySpeed )).xy*8.91 );
				simplePerlin2D5_g1174 = simplePerlin2D5_g1174*0.5 + 0.5;
				float3 worldToObjDir18_g1174 = normalize( mul( GetWorldToObjectMatrix(), float4( _SwayDirection, 0 ) ).xyz );
				float temp_output_21_0_g1179 = _WaveSource;
				float3 temp_output_23_0_g1179 = v.vertex.xyz;
				float temp_output_1_0_g1179 = distance( ( -_WaveOffset + temp_output_23_0_g1179 ) , float3( 0,0,0 ) );
				float3 temp_output_22_0_g1179 = _WaveMask;
				float temp_output_31_0_g1179 = length( ( ( -_WaveOffset + temp_output_23_0_g1179 ) * temp_output_22_0_g1179 ) );
				float temp_output_30_0_g1179 = length( ( float4( temp_output_22_0_g1179 , 0.0 ) * v.ase_color ) );
				float3 ase_worldPos = TransformObjectToWorld( (v.vertex).xyz );
				float3 normalizeResult37_g1177 = normalize( _WaveDirection1 );
				float3 break35_g1177 = ( ase_worldPos * normalizeResult37_g1177 );
				float temp_output_2_0_g1178 = _WaveFramerate;
				float Time40_g1177 = ( ( round( ( _TimeParameters.x * temp_output_2_0_g1178 ) ) / temp_output_2_0_g1178 ) * _WaveSpeed );
				float3 normalizeResult52_g1177 = normalize( _WaveDirection2 );
				float3 break49_g1177 = ( ase_worldPos * normalizeResult52_g1177 );
				float3 worldToObjDir18_g1177 = normalize( mul( GetWorldToObjectMatrix(), float4( _WaveInfluenceDirection, 0 ) ).xyz );
				float3 ase_objectScale = float3( length( GetObjectToWorldMatrix()[ 0 ].xyz ), length( GetObjectToWorldMatrix()[ 1 ].xyz ), length( GetObjectToWorldMatrix()[ 2 ].xyz ) );
				
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = ( ( _UseFlutter == 1.0 ? ( saturate( ( ( ( temp_output_21_0_g1173 == 3.0 ? 1.0 : 0.0 ) + ( temp_output_21_0_g1173 == 0.0 ? temp_output_1_0_g1173 : 0.0 ) + ( temp_output_21_0_g1173 == 1.0 ? temp_output_31_0_g1173 : 0.0 ) + ( temp_output_21_0_g1173 == 2.0 ? temp_output_30_0_g1173 : 0.0 ) ) * _FlutterSensitivity ) ) * ( _FlutterAmount * float3( 0.001,0.001,0.001 ) * simplePerlin2D12_g1171 * worldToObjDir34_g1171 ) ) : float3( 0,0,0 ) ) + ( _UseSwirl == 1.0 ? ( saturate( ( ( ( temp_output_21_0_g1170 == 3.0 ? 1.0 : 0.0 ) + ( temp_output_21_0_g1170 == 0.0 ? temp_output_1_0_g1170 : 0.0 ) + ( temp_output_21_0_g1170 == 1.0 ? temp_output_31_0_g1170 : 0.0 ) + ( temp_output_21_0_g1170 == 2.0 ? temp_output_30_0_g1170 : 0.0 ) ) * _SwirlSensitivity ) ) * ( rotatedValue35_g1168 - v.vertex.xyz ) ) : float3( 0,0,0 ) ) + ( _UseSway == 1.0 ? ( saturate( ( ( ( temp_output_21_0_g1176 == 3.0 ? 1.0 : 0.0 ) + ( temp_output_21_0_g1176 == 0.0 ? temp_output_1_0_g1176 : 0.0 ) + ( temp_output_21_0_g1176 == 1.0 ? temp_output_31_0_g1176 : 0.0 ) + ( temp_output_21_0_g1176 == 2.0 ? temp_output_30_0_g1176 : 0.0 ) ) * _SwaySensitivity ) ) * ( _SwayAmount * float3( 0.01,0.01,0.01 ) * simplePerlin2D5_g1174 * worldToObjDir18_g1174 ) ) : float3( 0,0,0 ) ) + ( _UseWave == 1.0 ? ( ( saturate( ( ( ( temp_output_21_0_g1179 == 3.0 ? 1.0 : 0.0 ) + ( temp_output_21_0_g1179 == 0.0 ? temp_output_1_0_g1179 : 0.0 ) + ( temp_output_21_0_g1179 == 1.0 ? temp_output_31_0_g1179 : 0.0 ) + ( temp_output_21_0_g1179 == 2.0 ? temp_output_30_0_g1179 : 0.0 ) ) * _WaveSensitivity ) ) * ( _WaveAmount * ( sin( (( break35_g1177.x + break35_g1177.y + break35_g1177.z )*_WaveNoiseScale + Time40_g1177) ) + sin( (( break49_g1177.x + break49_g1177.y + break49_g1177.z )*( _WaveNoiseScale * 0.5 ) + ( Time40_g1177 * 0.7 )) ) ) * worldToObjDir18_g1177 ) ) / ase_objectScale ) : float3( 0,0,0 ) ) );

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				float3 normalWS = TransformObjectToWorldNormal(v.ase_normal);

				o.clipPos = TransformWorldToHClip(positionWS);
				o.normalWS.xyz =  normalWS;

				return o;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_color : COLOR;
				uint ase_vertexID : SV_VertexID;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.vertex = v.vertex;
				o.ase_color = v.ase_color;
				o.ase_vertexID = v.ase_vertexID;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
				return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				o.ase_vertexID = patch[0].ase_vertexID * bary.x + patch[1].ase_vertexID * bary.y + patch[2].ase_vertexID * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			void frag( VertexOutput IN
				, out half4 outNormalWS : SV_Target0
			#ifdef _WRITE_RENDERING_LAYERS
				, out float4 outRenderingLayers : SV_Target1
			#endif
				 )
			{
				SurfaceDescription surfaceDescription = (SurfaceDescription)0;

				float _CullMode_Instance = UNITY_ACCESS_INSTANCED_PROP(DistantLandsIllustrateOpaque,_CullMode);
				

				surfaceDescription.Alpha = 1;
				surfaceDescription.AlphaClipThreshold = 0.5;

				#if _ALPHATEST_ON
					clip(surfaceDescription.Alpha - surfaceDescription.AlphaClipThreshold);
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODFadeCrossFade( IN.clipPos );
				#endif

				#if defined(_GBUFFER_NORMALS_OCT)
					float3 normalWS = normalize(IN.normalWS);
					float2 octNormalWS = PackNormalOctQuadEncode(normalWS);           // values between [-1, +1], must use fp32 on some platforms
					float2 remappedOctNormalWS = saturate(octNormalWS * 0.5 + 0.5);   // values between [ 0,  1]
					half3 packedNormalWS = PackFloat2To888(remappedOctNormalWS);      // values between [ 0,  1]
					outNormalWS = half4(packedNormalWS, 0.0);
				#else
					float3 normalWS = IN.normalWS;
					outNormalWS = half4(NormalizeNormalPerPixel(normalWS), 0.0);
				#endif

				#ifdef _WRITE_RENDERING_LAYERS
					uint renderingLayers = GetMeshRenderingLayer();
					outRenderingLayers = float4(EncodeMeshRenderingLayer(renderingLayers), 0, 0, 0);
				#endif
			}

			ENDHLSL
		}

	
	}
	
	CustomEditor "IllustrateEditor"
	FallBack "Hidden/Shader Graph/FallbackError"
	
	Fallback Off
}
/*ASEBEGIN
Version=19108
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;469;924,-991;Float;False;False;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;13;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;ShadowCaster;0;2;ShadowCaster;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;False;False;False;True;4;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;UniversalMaterialType=Unlit;True;5;True;12;all;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;False;False;True;False;False;False;False;0;False;;False;False;False;False;False;False;False;False;False;True;1;False;;True;3;False;;False;True;1;LightMode=ShadowCaster;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;470;924,-991;Float;False;False;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;13;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;DepthOnly;0;3;DepthOnly;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;False;False;False;True;4;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;UniversalMaterialType=Unlit;True;5;True;12;all;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;False;False;True;False;False;False;False;0;False;;False;False;False;False;False;False;False;False;False;True;1;False;;False;False;True;1;LightMode=DepthOnly;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;471;924,-991;Float;False;False;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;13;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;Meta;0;4;Meta;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;False;False;False;True;4;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;UniversalMaterialType=Unlit;True;5;True;12;all;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=Meta;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.RangedFloatNode;664;-959,-1680;Inherit;False;InstancedProperty;_CullMode;Cull Mode;64;1;[Enum];Create;True;0;3;Two Sided;0;Cull Front;1;Cull Back;2;0;True;0;False;2;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;299;288,-1408;Inherit;False;4;4;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;755;-48,-1120;Inherit;False;Screenspace Noise;117;;1153;5e9a786262a092f488efb11845fd6e36;0;1;40;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;490;-400,-1264;Inherit;False;Normals;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;338;32,-1488;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;115;512,-1408;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;467;704,-1136;Float;False;False;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;13;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;ExtraPrePass;0;0;ExtraPrePass;5;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;False;False;False;True;4;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;UniversalMaterialType=Unlit;True;5;True;12;all;0;False;True;1;1;False;;0;False;;0;1;False;;0;False;;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;1;False;;True;3;False;;True;True;0;False;;0;False;;True;0;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.SimpleAddOpNode;716;736,-1232;Inherit;False;4;4;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.FunctionNode;886;-48,-1216;Inherit;False;Emission;106;;1166;8bfb35ab5d7b99742a36e8551a29259e;0;1;21;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;887;-656,-1392;Inherit;False;Custom Normals;65;;1167;b77cb78b6acab4a48b84461b1772ff9e;0;0;1;FLOAT3;0
Node;AmplifyShaderEditor.FunctionNode;888;512,-1216;Inherit;False;Swirling Noise;148;;1168;9a3d91a82277abc46966b9af21c17a04;0;0;1;FLOAT3;0
Node;AmplifyShaderEditor.FunctionNode;889;512,-1296;Inherit;False;Fluttering Noise;137;;1171;b38a8c9eef50ed746b29c9f61485ae0d;0;0;1;FLOAT3;0
Node;AmplifyShaderEditor.FunctionNode;890;512,-1136;Inherit;False;Swaying Noise;126;;1174;883d0da31ba823541b98d418e4516d72;0;0;1;FLOAT3;16
Node;AmplifyShaderEditor.FunctionNode;891;512,-1056;Inherit;False;Waving Noise;35;;1177;cadadbf7b0bd28f4e978f83999c65368;0;0;1;FLOAT3;16
Node;AmplifyShaderEditor.FunctionNode;896;-48,-1312;Inherit;False;Glint;48;;1188;b3529c5fbdfb23848a365c71df3e9f0e;0;1;4;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;899;1008,-1358;Float;False;False;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;13;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;Universal2D;0;5;Universal2D;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;False;False;False;True;4;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;UniversalMaterialType=Unlit;True;5;True;12;all;0;False;True;1;1;False;;0;False;;0;1;False;;0;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;1;False;;True;3;False;;True;True;0;False;;0;False;;True;1;LightMode=Universal2D;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;900;1008,-1358;Float;False;False;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;13;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;SceneSelectionPass;0;6;SceneSelectionPass;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;False;False;False;True;4;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;UniversalMaterialType=Unlit;True;5;True;12;all;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=SceneSelectionPass;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;901;1008,-1358;Float;False;False;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;13;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;ScenePickingPass;0;7;ScenePickingPass;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;False;False;False;True;4;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;UniversalMaterialType=Unlit;True;5;True;12;all;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=Picking;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;902;1008,-1358;Float;False;False;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;13;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;DepthNormals;0;8;DepthNormals;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;False;False;False;True;4;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;UniversalMaterialType=Unlit;True;5;True;12;all;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;False;;True;3;False;;False;True;1;LightMode=DepthNormalsOnly;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;903;1008,-1358;Float;False;False;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;13;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;DepthNormalsOnly;0;9;DepthNormalsOnly;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;False;False;False;True;4;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;UniversalMaterialType=Unlit;True;5;True;12;all;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;False;;True;3;False;;False;True;1;LightMode=DepthNormalsOnly;False;True;9;d3d11;metal;vulkan;xboxone;xboxseries;playstation;ps4;ps5;switch;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.FunctionNode;921;-398,-1390;Inherit;False;Lighting;80;;1189;686dba7250013234d892aef0d32c104d;0;1;191;FLOAT3;0,0,0;False;3;FLOAT4;0;FLOAT3;205;FLOAT;32
Node;AmplifyShaderEditor.FunctionNode;883;-800,-1488;Inherit;False;Adjust Color;74;;1194;3c1e998ab8b1a3843a23a5fe34c8bbc3;0;1;1;FLOAT4;1,1,1,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.FunctionNode;880;-608,-1488;Inherit;False;HSV Variation;19;;1198;ca5d2276ff902f6439e6cdd3685031b4;0;1;1;FLOAT4;1,1,1,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.FunctionNode;892;-416,-1488;Inherit;False;Posterize Texture;71;;1202;2cee7f234007b2a458c94bbdaf6405dc;0;1;15;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.FunctionNode;882;-1456,-1488;Inherit;False;Texture Block;7;;1203;4918127142e778a4a9e764dde15ce8c2;0;0;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;884;-1296,-1488;Inherit;False;Gradient;54;;1204;fda248e61446ae246bc4b3d542b7072c;0;1;14;COLOR;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;908;-1280,-1408;Inherit;False;490;Normals;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WireNode;934;-694.1666,-1419.327;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.FunctionNode;940;-224,-1488;Inherit;False;COZY Precipitation;0;;1212;9bd035274d51a2048868dde6cf785195;0;2;8;COLOR;0,0,0,0;False;70;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;941;-1088,-1488;Inherit;False;Triplanar Mapping;10;;1213;cf656a340a01c8c49a6ac127e833481e;0;2;1;COLOR;1,1,1,0;False;32;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;468;928,-1408;Float;False;True;-1;2;IllustrateEditor;0;13;Distant Lands/Illustrate/Opaque;2992e84f91cbeb14eab234972e07ea9d;True;Forward;0;1;Forward;8;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;True;True;0;True;_CullMode;False;False;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;False;False;False;True;4;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;UniversalMaterialType=Unlit;True;5;True;12;all;0;False;True;1;1;False;;0;False;;1;1;False;;0;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;1;False;;True;3;False;;True;True;0;False;;0;False;;True;1;LightMode=UniversalForwardOnly;False;False;0;;0;0;Standard;23;Surface;0;638201235824325068;  Blend;0;0;Two Sided;1;0;Forward Only;0;0;Cast Shadows;1;0;  Use Shadow Threshold;0;0;Receive Shadows;1;0;GPU Instancing;1;0;LOD CrossFade;0;0;Built-in Fog;0;0;DOTS Instancing;0;0;Meta Pass;0;0;Extra Pre Pass;0;0;Tessellation;0;0;  Phong;0;0;  Strength;0.5,False,;0;  Type;0;0;  Tess;16,False,;0;  Min;10,False,;0;  Max;25,False,;0;  Edge Length;16,False,;0;  Max Displacement;25,False,;0;Vertex Position,InvertActionOnDeselection;1;638201042370953364;0;10;False;True;True;True;False;False;True;True;True;False;False;;False;0
WireConnection;299;0;338;0
WireConnection;299;1;921;205
WireConnection;299;2;896;0
WireConnection;299;3;886;0
WireConnection;755;40;921;32
WireConnection;490;0;887;0
WireConnection;338;0;940;0
WireConnection;338;1;921;0
WireConnection;115;0;299;0
WireConnection;115;1;755;0
WireConnection;716;0;889;0
WireConnection;716;1;888;0
WireConnection;716;2;890;16
WireConnection;716;3;891;16
WireConnection;886;21;921;32
WireConnection;896;4;921;32
WireConnection;921;191;887;0
WireConnection;883;1;941;0
WireConnection;880;1;883;0
WireConnection;892;15;880;0
WireConnection;884;14;882;0
WireConnection;934;0;908;0
WireConnection;940;8;892;0
WireConnection;940;70;934;0
WireConnection;941;1;884;0
WireConnection;941;32;908;0
WireConnection;468;2;115;0
WireConnection;468;5;716;0
ASEEND*/
//CHKSM=1BE46B039AA73BF1E8F57AC2162F8B36D11D61E6