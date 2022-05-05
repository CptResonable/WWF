Shader "Custom/BoomShader" {
	Properties {
		_BaseMap ("Base Texture", 2D) = "white" {}
		_TextureInfluence ("Texture Influence", Range(0, 1)) = 0.5
		_Color1 ("Color 1, using red chanel 0", Color) = (0, 0.66, 0.73, 1)
		_Color2 ("Color 2, using red chanel 1", Color) = (0, 0.66, 0.73, 1)
		_Color3 ("Color 3, using green chanel", Color) = (0, 0.66, 0.73, 1)
		_ShadedColor ("Shaded Colour", Color) = (0, 0.66, 0.73, 1)
		_CellShadePower ("Cell Shade Power", Range(0, 1)) = 0.5
		_GrassTexture ("Grass Texture", 2D) = "white" {}
		_GrassSize ("Grass Size", Float) = 0.5
		_GrassSizeVariance ("Grass Size Variance", Float) = 0.3
		_GrassColor ("Grass Color", Color) = (0, 0.66, 0.73, 1)
		_GrassPlanesPerTriangle ("GrassPlanesPerTriangle", Range(1, 10)) = 1
		_Tess("Tessellation", Range(0, 32)) = 20
		_MaxTessDistance("Max Tess Distance", Range(1, 360)) = 20
		
		_Noise ("Noise", 2D) = "white" {}
		_RndNoise ("Random noise", 2D) = "white" {}
	}
	SubShader {
		Tags {
			"RenderPipeline" = "UniversalPipeline"
			"RenderType" = "Opaque"
			"Queue" = "Geometry"
		}
		HLSLINCLUDE
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
 
			CBUFFER_START(UnityPerMaterial)
			float4 _BaseMap_ST;
			float _TextureInfluence;
			float4 _Color1;
			float4 _Color2;
			float4 _Color3;
			float4 _ShadedColor;
			float _CellShadePower;
			float _GrassSize;
			float _GrassSizeVariance;
			float4 _GrassColor;
			int _GrassPlanesPerTriangle;

			CBUFFER_END
		ENDHLSL
 
		Pass {
			Name "MainPass"
			Tags { "LightMode"="UniversalForward" }
			
			CULL Off
			ZWrite On
 
			HLSLPROGRAM
			#pragma require tessellation
            #pragma vertex TessellationVertexProgram // This line defines the name of the vertex shader. 
			//#pragma vertex vert
			#pragma fragment frag
			#pragma hull hull // This line defines the name of the hull shader. 
            #pragma domain domain // This line defines the name of the domain shader. 
			
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ _SHADOWS_SOFT
			 
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "NMGGeometryHelpers.hlsl"
			#include "HlslHelpers.hlsl"
			#include "NoiseSampler.hlsl"
			#include "CustomTessellation.cginc"
 
			struct Attributes {
				float4 vertex	: POSITION; // PositionOS
				float2 uv		: TEXCOORD0;
				float4 color		: COLOR;
				float4 normal     : NORMAL;  // NormalOS
			};
 		
			struct Varyings { // Vertex to geometry
				float4 positionCS 	: SV_POSITION;
				float2 uv		: TEXCOORD0;
				float4 color		: COLOR;
				float3 normalWS     : NORMAL;
				float3 positionWS   : TEXCOORD2;
			};

			TEXTURE2D(_BaseMap);
			SAMPLER(sampler_BaseMap);

			TEXTURE2D(_GrassTexture);
			SAMPLER(sampler_GrassTexture);

			// pre tesselation vertex program
            ControlPoint TessellationVertexProgram(Attributes v)
            {
                ControlPoint p;

                p.vertex = v.vertex;
                p.uv = v.uv;
                p.normal = v.normal.xyz;
                p.color = v.color;
         
                return p;
            }
 
			Varyings vert(Attributes IN) {
				Varyings OUT;
 
				VertexPositionInputs positionInputs = GetVertexPositionInputs(IN.vertex.xyz);
			    VertexNormalInputs normalInputs = GetVertexNormalInputs(IN.normal.xyz);
				
				OUT.positionCS = positionInputs.positionCS;
				OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
				OUT.color = IN.color;
			    OUT.positionWS = positionInputs.positionWS;
			    OUT.normalWS = normalInputs.normalWS;
				return OUT;
			}

            [UNITY_domain("tri")]
            Varyings domain(TessellationFactors factors, OutputPatch<ControlPoint, 3> patch, float3 barycentricCoordinates : SV_DomainLocation)
            {
                Attributes v;
         
                #define DomainPos(fieldName) v.fieldName = \
                patch[0].fieldName * barycentricCoordinates.x + \
                patch[1].fieldName * barycentricCoordinates.y + \
                patch[2].fieldName * barycentricCoordinates.z;
         
                DomainPos(vertex.xyz)
                DomainPos(uv)
                DomainPos(color)
                DomainPos(normal.xyz)
         
                return vert(v);
            }
			
			half4 frag(Varyings IN) : SV_Target {
		 
				// // Initialize some information for the lighting function
				// InputData lightingInput = (InputData)0;
				// lightingInput.positionWS = IN.positionWS;
				// lightingInput.normalWS = IN.normalWS; // No need to renormalize, since triangles all share normals
				// lightingInput.viewDirectionWS = GetViewDirectionFromPosition(IN.positionWS);
				// lightingInput.shadowCoord = CalculateShadowCoord(IN.positionWS, IN.positionCS);
			
				// half4 baseMap = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv);
			
				// // Calculate color
				// float3 colorSmooth = lerp(_Color1.rgb, _Color2.rgb, IN.color.r); // Use red chanel to lerp between base and grass ground color
				// //colorSmooth = lerp(colorSmooth, _Color3, IN.color.g);
				
				// float colorStep = floor((IN.color.r - 0.01f) * 3) / 3;
				// half3 colorCel = lerp(_Color1.rgb, _Color2.rgb, colorStep);
				// float3 color = lerp(colorSmooth, colorCel, 0.5);
			
				// float3 colorSmooth2 = lerp(color, _Color3.rgb, IN.color.g);
				// float3 colorStep2 = floor((IN.color.g) * 3) / 3;
				// half3 colorCel2 = lerp(color, _Color3.rgb, colorStep2);
				// color = lerp(colorSmooth2, colorCel2, 0.5);
			
				// // Grass stuff
			
				// // Calculate shading
				// float4 lit = UniversalFragmentBlinnPhong(lightingInput, float3(0.5, 0.5, 0.5), 1, 0, 0, 1);
				// float brightness = (lit.x + lit.y + lit.z) / 3; // * lerp(TexNoise(IN.positionWS.xz * 0.02), 1, 0.95); // Calculate brightness	
				// float3 rgbSmooth = lerp(_ShadedColor.rgb, color, brightness);
				// brightness = floor(brightness * 10) / 10;
				// float3 rgbCel = lerp(_ShadedColor.rgb, color, brightness);
				// float3 rgb = lerp(rgbSmooth, rgbCel, _CellShadePower);
			
				// // Apply texture
				// baseMap.rgb = lerp(float3(1, 1, 1), baseMap.rgb, _TextureInfluence);
				// rgb *= baseMap.rgb;
				
				return float4(1, 1, 1, 1);
			}
			ENDHLSL
		}
		
		Pass {
			Name "DepthOnly"
			Tags { "LightMode"="DepthOnly" }

			ColorMask 0
			ZWrite On
			ZTest LEqual

			HLSLPROGRAM
			#pragma vertex DepthOnlyVertex
			#pragma fragment DepthOnlyFragment

			// Material Keywords
			#pragma shader_feature _ALPHATEST_ON
			#pragma shader_feature _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

			// GPU Instancing
			#pragma multi_compile_instancing
			// #pragma multi_compile _ DOTS_INSTANCING_ON

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonMaterial.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Shaders/DepthOnlyPass.hlsl"
			ENDHLSL
		}
	}
}