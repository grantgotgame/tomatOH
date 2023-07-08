Shader "Hidden/SnapshotPro/Mosaic"
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
			TEXTURE2D_SAMPLER2D(_OverlayTex, sampler_OverlayTex);
			float4 _OverlayColor;
			int _XTileCount;
			int _YTileCount;

			float4 Frag(VaryingsDefault i) : SV_Target
			{
				float4 tex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);

				float2 overlayUV = i.texcoord * float2(_XTileCount, _YTileCount);
				float4 overlayCol = SAMPLE_TEXTURE2D(_OverlayTex, sampler_OverlayTex, overlayUV) * _OverlayColor;

				return lerp(tex, overlayCol, overlayCol.a);
			}

			ENDHLSL
		}
	}
}
