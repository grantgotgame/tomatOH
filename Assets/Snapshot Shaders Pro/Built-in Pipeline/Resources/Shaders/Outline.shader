Shader "Hidden/SnapshotPro/Outline"
{
	SubShader
	{
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			HLSLPROGRAM

			#pragma vertex VertDefault
			#pragma fragment Frag
			#pragma multi_compile_local_fragment __ USE_SCENE_TEXTURE_ON

			#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

			TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
			float2 _MainTex_TexelSize;

			TEXTURE2D_SAMPLER2D(_CameraDepthTexture, sampler_CameraDepthTexture);
			TEXTURE2D_SAMPLER2D(_CameraDepthNormalsTexture, sampler_CameraDepthNormalsTexture);

			float4 _OutlineColor;
			float _ColorSensitivity;
			float _ColorStrength;
			float _DepthSensitivity;
			float _DepthStrength;
			float _NormalsSensitivity;
			float _NormalsStrength;
			float _DepthThreshold;
			float4 _BackgroundColor;

			float4 Frag(VaryingsDefault i) : SV_Target
			{
				float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);

				// Determine plus-shaped sampling positions.
				float2 leftUV = i.texcoord + float2(-_MainTex_TexelSize.x, 0);
				float2 rightUV = i.texcoord + float2(_MainTex_TexelSize.x, 0);
				float2 bottomUV = i.texcoord + float2(0, -_MainTex_TexelSize.y);
				float2 topUV = i.texcoord + float2(0, _MainTex_TexelSize.y);

				// Calculate edges based on colour data.
				float3 col0 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, leftUV).r;
				float3 col1 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, rightUV).r;
				float3 col2 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, bottomUV).r;
				float3 col3 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, topUV).r;

				float3 c0 = col1 - col0;
				float3 c1 = col3 - col2;

				float edgeCol = sqrt(dot(c0, c0) + dot(c1, c1));
				edgeCol = edgeCol > _ColorSensitivity ? _ColorStrength : 0;

				// Calculate edges based on depth data.
				float depth0 = SAMPLE_TEXTURE2D(_CameraDepthTexture, sampler_CameraDepthTexture, leftUV).r;
				float depth1 = SAMPLE_TEXTURE2D(_CameraDepthTexture, sampler_CameraDepthTexture, rightUV).r;
				float depth2 = SAMPLE_TEXTURE2D(_CameraDepthTexture, sampler_CameraDepthTexture, bottomUV).r;
				float depth3 = SAMPLE_TEXTURE2D(_CameraDepthTexture, sampler_CameraDepthTexture, topUV).r;

				depth0 = Linear01Depth(depth0);
				depth1 = Linear01Depth(depth1);
				depth2 = Linear01Depth(depth2);
				depth3 = Linear01Depth(depth3);

				float d0 = depth1 - depth0;
				float d1 = depth3 - depth2;

				float edgeDepth = sqrt(d0 * d0 + d1 * d1);
				edgeDepth = edgeDepth > _DepthSensitivity ? _DepthStrength : 0;

				// Calculate edges based on normal data.
				float3 normal0 = SAMPLE_TEXTURE2D(_CameraDepthNormalsTexture, sampler_CameraDepthNormalsTexture, leftUV).rgb;
				float3 normal1 = SAMPLE_TEXTURE2D(_CameraDepthNormalsTexture, sampler_CameraDepthNormalsTexture, rightUV).rgb;
				float3 normal2 = SAMPLE_TEXTURE2D(_CameraDepthNormalsTexture, sampler_CameraDepthNormalsTexture, bottomUV).rgb;
				float3 normal3 = SAMPLE_TEXTURE2D(_CameraDepthNormalsTexture, sampler_CameraDepthNormalsTexture, topUV).rgb;

				float3 n0 = normal1 - normal0;
				float3 n1 = normal3 - normal2;

				float edgeNormal = sqrt(dot(n0, n0) + dot(n1, n1));
				edgeNormal = edgeNormal > _NormalsSensitivity ? _NormalsStrength : 0;

				// Combine edge data.
				float edge = max(max(edgeCol, edgeDepth), edgeNormal);

#if UNITY_REVERSED_Z
				float depth = SAMPLE_TEXTURE2D(_CameraDepthTexture, sampler_CameraDepthTexture, i.texcoord).r;
#else
				float depth = lerp(UNITY_NEAR_CLIP_VALUE, 1, SAMPLE_TEXTURE2D(_CameraDepthTexture, sampler_CameraDepthTexture, i.texcoord).r);
#endif
				depth = Linear01Depth(depth);
				edge = depth > _DepthThreshold ? 0.0f : edge;

#ifdef USE_SCENE_TEXTURE_ON
				float4 background = col;
#else
				float4 background = _BackgroundColor;
#endif

				return lerp(background, _OutlineColor, edge);
			}

			ENDHLSL
		}
	}
}
