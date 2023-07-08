Shader "Hidden/SnapshotPro/Kaleidoscope"
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
			float _SegmentCount;

			float4 Frag(VaryingsDefault i) : SV_Target
			{
				float2 shiftUV = _ScreenParams.xy * (i.texcoord - 0.5f);

				float radius = sqrt(dot(shiftUV, shiftUV));
				float angle = atan2(shiftUV.y, shiftUV.x);

				float segmentAngle = 3.14159265f * 2.0f / _SegmentCount;
				angle -= segmentAngle * floor(angle / segmentAngle);
				angle = min(angle, segmentAngle - angle);

				float2 uv = float2(cos(angle), sin(angle)) * radius + _ScreenParams.xy / 2.0f;
				uv = max(min(uv, _ScreenParams.xy * 2.0f - uv), -uv) / _ScreenParams.xy;

				return SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);
			}

			ENDHLSL
		}
	}
}
