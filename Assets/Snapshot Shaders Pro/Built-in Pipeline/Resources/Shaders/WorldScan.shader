Shader "Hidden/SnapshotPro/WorldScan"
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
			#include "HLSLSupport.cginc"

			TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
			TEXTURE2D_SAMPLER2D(_OverlayRampTex, sampler_OverlayRampTex);
			TEXTURE2D_SAMPLER2D(_CameraDepthTexture, sampler_CameraDepthTexture);
			float3 _ScanOrigin;
			float _ScanDistance;
			float _ScanWidth;
			float4 _OverlayColor;
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

				float3 worldPos = pixelPositionWS.xyz / pixelPositionWS.w;

				float fragDist = distance(worldPos, _ScanOrigin);

				float4 scanColor = 0.0f;

				if (fragDist < _ScanDistance && fragDist > _ScanDistance - _ScanWidth)
				{
					float scanUV = (fragDist - _ScanDistance) / (_ScanWidth * 1.01f);

					scanColor = SAMPLE_TEXTURE2D(_OverlayRampTex, sampler_OverlayRampTex, float2(scanUV, 0.5f));
					scanColor.rgb *= _OverlayColor.rgb;
				}

				float4 textureSample = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);

				// Fix for weird issues with the skybox.
				if (depthSample < EPSILON)
				{
					scanColor = textureSample;
				}

				return lerp(textureSample, scanColor, scanColor.a);
			}

			ENDHLSL
		}
	}
}
