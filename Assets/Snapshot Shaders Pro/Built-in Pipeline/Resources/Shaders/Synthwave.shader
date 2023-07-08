Shader "Hidden/SnapshotPro/Synthwave"
{
	SubShader
	{
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			HLSLPROGRAM

			#pragma vertex VertDefault
			#pragma fragment Frag
			#pragma multi_compile __ USE_SCENE_TEXTURE_ON

			#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

			TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
			TEXTURE2D_SAMPLER2D(_CameraDepthTexture, sampler_CameraDepthTexture);
			float4 _BackgroundColor;
			float4 _LineColor1;
			float4 _LineColor2;
			float _LineColorMix;
			float _LineWidth;
			float _LineFalloff;
			float3 _GapWidth;
			float3 _Offset;
			float3 _AxisMask;
			float4x4 _ClipToWorld;

			float4 Frag(VaryingsDefault i) : SV_Target
			{
				float depthSample = SAMPLE_TEXTURE2D(_CameraDepthTexture, sampler_CameraDepthTexture, i.texcoord).r;

#if UNITY_REVERSED_Z
				float depth = depthSample;
#else
				float depth = lerp(UNITY_NEAR_CLIP_VALUE, 1, depthSample);
#endif

				float4 pixelPositionCS = float4(i.texcoord * 2.0f - 1.0f, depth, 1.0f);

#if UNITY_UV_STARTS_AT_TOP
				pixelPositionCS.y = -pixelPositionCS.y;
#endif

				float4 pixelPositionWS = mul(_ClipToWorld, pixelPositionCS);

				float3 worldPos = pixelPositionWS.xyz / pixelPositionWS.w + _Offset;

				float3 modWorldPos = fmod(abs(worldPos) + _GapWidth / 2.0f, _GapWidth);

				float3 distWorldPos = abs((_GapWidth / 2.0f) - modWorldPos);

				float3 stepWorldPos = 1.0f - smoothstep(_LineWidth, _LineWidth + _LineFalloff, distWorldPos);
				stepWorldPos *= _AxisMask;

				float lines = saturate(dot(float3(1.0f, 1.0f, 1.0f), stepWorldPos));

				// Fix for weird issues with the skybox.
				if (depthSample < EPSILON)
				{
					lines = 0.0f;
				}

#ifdef USE_SCENE_TEXTURE_ON
				float4 background = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
#else
				float4 background = _BackgroundColor;
#endif
				float4 lineColor = lerp(_LineColor1, _LineColor2, pow(i.texcoord.y, _LineColorMix));
				return lerp(background, lineColor, lines);
			}

			ENDHLSL
		}
	}
}
