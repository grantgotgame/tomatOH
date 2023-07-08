Shader "Hidden/SnapshotPro/Sharpen"
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
			float2 _MainTex_TexelSize;
			float _Intensity;

			float4 Frag(VaryingsDefault i) : SV_Target
			{
				float3 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord).xyz;
				col += 4.0f * col * _Intensity;

				float2 s = _MainTex_TexelSize;
				col -= SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(   0, -s.y)).xyz * _Intensity;
				col -= SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(-s.x,    0)).xyz * _Intensity;
				col -= SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2( s.x,    0)).xyz * _Intensity;
				col -= SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(   0,  s.y)).xyz * _Intensity;

				return float4(col, 1.0f);
			}

			ENDHLSL
		}
	}
}
