Shader "Hidden/SnapshotPro/Vortex"
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
			float2 _Center;
			float _Strength;
			float2 _Offset;

			float4 Frag(VaryingsDefault i) : SV_Target
			{
				float2 distance = i.texcoord - _Center;
				float angle = length(distance) * _Strength;
				float sinAngle = sin(angle);
				float cosAngle = cos(angle);
				float x = cosAngle * distance.x - sinAngle * distance.y;
				float y = sinAngle * distance.x + cosAngle * distance.y;
				float2 uv = float2(x + _Center.x + _Offset.x, y + _Center.y + _Offset.y);

				float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);
				return col;
			}

			ENDHLSL
		}
	}
}
