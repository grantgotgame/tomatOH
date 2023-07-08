Shader "Hidden/SnapshotPro/GameBoy"
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
			float4 _GBDarkest;
			float4 _GBDark;
			float4 _GBLight;
			float4 _GBLightest;
			float _PowerRamp;

			float4 Frag(VaryingsDefault i) : SV_Target
			{
				float3 tex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
				float lum = dot(tex, float3(0.30, 0.59, 0.11));
				lum = pow(lum, _PowerRamp);
				int gb = lum * 4;

				float3 col = lerp(_GBDarkest, _GBDark, saturate(gb));
				col = lerp(col, _GBLight, saturate(gb - 1.0));
				col = lerp(col, _GBLightest, saturate(gb - 2.0));

				return float4(col, 1.0);
			}

			ENDHLSL
		}
	}
}
