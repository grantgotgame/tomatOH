Shader "Hidden/SnapshotPro/SepiaTone"
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
				float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
				float3x3 sepia = float3x3
				(
					0.393, 0.349, 0.272,	// Red.
					0.769, 0.686, 0.534,	// Green.
					0.189, 0.168, 0.131		// Blue.
				);

				float3 sepiaTint = mul(color.rgb, sepia);

				color.rgb = lerp(color.rgb, sepiaTint, _Blend.xxx);
				return color;
			}

			ENDHLSL
		}
	}
}
