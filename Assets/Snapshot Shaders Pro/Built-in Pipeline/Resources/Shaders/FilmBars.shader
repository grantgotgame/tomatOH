Shader "Hidden/SnapshotPro/FilmBars"
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
			float _Aspect;

			float4 Frag(VaryingsDefault i) : SV_Target
			{
				float4 tex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);

				float aspect = _ScreenParams.x / _ScreenParams.y;
				float bars = step(abs(0.5f - i.texcoord.y) * 2.0f, aspect / _Aspect);

				return tex * bars;
			}

			ENDHLSL
		}
	}
}
