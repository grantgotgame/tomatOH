Shader "Hidden/SnapshotPro/SNES"
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
			int _BandingLevels;
			float _PowerRamp;

			static const float EPS = 1e-10;

			float4 Frag(VaryingsDefault i) : SV_Target
			{
				float3 tex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord).rgb;
				tex = pow(tex, _PowerRamp);

				int r = (tex.r - EPS) * _BandingLevels;
				int g = (tex.g - EPS) * _BandingLevels;
				int b = (tex.b - EPS) * _BandingLevels;

				float divisor = _BandingLevels - 1.0f;

				return float4(r / divisor, g / divisor, b / divisor, 1.0f);
			}

			ENDHLSL
		}
	}
}
