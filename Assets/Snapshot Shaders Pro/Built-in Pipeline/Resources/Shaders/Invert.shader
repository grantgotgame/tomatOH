Shader "Hidden/SnapshotPro/Invert"
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
			float _Blend;

			float4 Frag(VaryingsDefault i) : SV_Target
			{
				float3 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord).xyz;
				col = lerp(col, 1.0f - col, _Blend);
				return float4(col, 1.0f);
			}

			ENDHLSL
		}
	}
}
