Shader "Hidden/SnapshotPro/Underwater"
{
	SubShader
	{
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			HLSLPROGRAM

			#pragma vertex VertDefault
			#pragma fragment Frag

			#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
			#include "HLSLSupport.cginc"

			TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
			TEXTURE2D_SAMPLER2D(_BumpMap, sampler_BumpMap);
			float _Strength;
			float4 _WaterColor;
			float _FogStrength;
			TEXTURE2D_SAMPLER2D(_CameraDepthTexture, sampler_CameraDepthTexture);

			float4 Frag(VaryingsDefault i) : SV_Target 
			{
				float2 timeUV = (i.texcoord + _Time.x) % 1.0f;
				float4 bumpTex = SAMPLE_TEXTURE2D(_BumpMap, sampler_BumpMap, timeUV);
				float2 normal = bumpTex.wy * 2 - 1;

				float2 normalUV = i.texcoord + normal.xy * _Strength;
				float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, normalUV);

				float4 depthTex = SAMPLE_TEXTURE2D(_CameraDepthTexture, sampler_CameraDepthTexture, normalUV);
				float depth = UNITY_SAMPLE_DEPTH(depthTex);
				depth = Linear01Depth(depthTex.r);

				col = lerp(col, _WaterColor, depth * _FogStrength);

				return col;
			}

			ENDHLSL
		}
	}
}
