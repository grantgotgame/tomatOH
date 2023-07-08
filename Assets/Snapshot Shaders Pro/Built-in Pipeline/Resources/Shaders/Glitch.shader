Shader "Hidden/SnapshotPro/Glitch"
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
			TEXTURE2D_SAMPLER2D(_OffsetTex, sampler_OffsetTex);
			float _OffsetStrength;
			float _VerticalTiling;

			float4 Frag(VaryingsDefault i) : SV_Target
			{
				float2 uv = i.texcoord;

				float offset = SAMPLE_TEXTURE2D(_OffsetTex, sampler_OffsetTex, float2(uv.x, uv.y * _VerticalTiling));
				uv.x += (offset - 0.5f) * _OffsetStrength + 1.0f;
				uv.x %= 1.0f;

				return SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv); 
			}

			ENDHLSL
		}
	}
}
