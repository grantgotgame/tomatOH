Shader "Hidden/SnapshotPro/BarrelDistortion"
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

			TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
			float4 _BackgroundColor;
			float _Strength;

			float4 Frag(VaryingsDefault i) : SV_Target
			{
				float2 UVs = i.texcoord - 0.5f;

				UVs = UVs * (1 + _Strength * length(UVs) * length(UVs)) + 0.5f;

				float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, UVs);

				col = (UVs.x >= 0.0f && UVs.x <= 1.0f && UVs.y >= 0.0f && UVs.y <= 1.0f) ? col : _BackgroundColor;

				return col;
			}

			ENDHLSL
		}
	}
}
