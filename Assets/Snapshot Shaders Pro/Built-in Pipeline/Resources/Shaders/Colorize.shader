Shader "Hidden/SnapshotPro/Colorize"
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
			float4 _TintColor;

			float4 Frag(VaryingsDefault i) : SV_Target
			{
				float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
				return lerp(col, col * _TintColor, _TintColor.a);
			}

			ENDHLSL
		}
	}
}
