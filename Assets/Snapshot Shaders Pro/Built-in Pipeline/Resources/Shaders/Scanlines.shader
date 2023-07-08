Shader "Hidden/SnapshotPro/Scanlines"
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
			TEXTURE2D_SAMPLER2D(_ScanlineTex, sampler_ScanlineTex);

			float _Strength;
			int _Size;
			float _ScrollSpeed;

			float4 Frag(VaryingsDefault i) : SV_Target
			{
				float2 size = _ScreenParams.xy / _Size;
				float2 scanlineUV = i.texcoord * size;
				scanlineUV.y += _Time.y * _ScrollSpeed;

				float3 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord).rgb;
				float3 scanlines = SAMPLE_TEXTURE2D(_ScanlineTex, sampler_ScanlineTex, scanlineUV).rgb;

				col = lerp(col, col * scanlines, _Strength);

				return float4(col, 1.0f);
			}

			ENDHLSL
		}
	}
}
