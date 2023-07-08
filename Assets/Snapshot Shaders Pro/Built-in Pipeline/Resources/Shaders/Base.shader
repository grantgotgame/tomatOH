Shader "Hidden/SnapshotPro/Base"
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

			float4 Frag(VaryingsDefault i) : SV_Target
			{
				return SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
			}

			ENDHLSL
		}
	}
}
