Shader "Hidden/SnapshotPro/Silhouette"
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

			TEXTURE2D_SAMPLER2D(_CameraDepthTexture, sampler_CameraDepthTexture);

			float4 _NearColor;
			float4 _FarColor;
			float _PowerRamp;

			float4 Frag(VaryingsDefault i) : SV_Target
			{
				float depthSample = SAMPLE_TEXTURE2D(_CameraDepthTexture, sampler_CameraDepthTexture, i.texcoord).r;

#if UNITY_REVERSED_Z
				float depth = depthSample;
#else
				float depth = lerp(UNITY_NEAR_CLIP_VALUE, 1, depthSample);
#endif
				depth = pow(Linear01Depth(depth), _PowerRamp);

				return lerp(_NearColor, _FarColor, depth);
			}

			ENDHLSL
		}
	}
}
