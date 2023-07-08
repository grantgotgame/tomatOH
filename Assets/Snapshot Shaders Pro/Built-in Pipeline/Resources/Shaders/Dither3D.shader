Shader "Hidden/SnapshotPro/Dither3D"
{
	SubShader
	{
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			HLSLPROGRAM

			#pragma vertex vert
			#pragma fragment Frag

			#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

			TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
			TEXTURE2D_SAMPLER2D(_NoiseTex, sampler_NoiseTex);
			float4 _NoiseTex_TexelSize;
			float _NoiseSize;
			float _ThresholdOffset;
			float _Blend;

			float4 _LightColor;
			float4 _DarkColor;

			TEXTURE2D_SAMPLER2D(_CameraDepthTexture, sampler_CameraDepthTexture);
			TEXTURE2D_SAMPLER2D(_CameraDepthNormalsTexture, sampler_CameraDepthNormalsTexture);

			float4x4 clipToWorld;

			// Credit to https://alexanderameye.github.io/outlineshader.html:
			float3 DecodeNormal(float4 enc)
			{
				float kScale = 1.7777;
				float3 nn = enc.xyz*float3(2 * kScale, 2 * kScale, 0) + float3(-kScale, -kScale, 1);
				float g = 2.0 / dot(nn.xyz, nn.xyz);
				float3 n;
				n.xy = g * nn.xy;
				n.z = g - 1;
				return n;
			}

			struct v2f 
			{
				float4 vertex : SV_POSITION;
				float2 texcoord : TEXCOORD0;
				float2 texcoordStereo : TEXCOORD1;
				float3 worldDir : TEXCOORD2;
			};

			v2f vert(AttributesDefault i)
			{
				v2f o;
				o.vertex = float4(i.vertex.xy, 0.0, 1.0);
				o.texcoord = TransformTriangleVertexToUV(i.vertex.xy);

#if UNITY_UV_STARTS_AT_TOP
				o.texcoord = o.texcoord * float2(1.0, -1.0) + float2(0.0, 1.0);
#endif

				o.texcoordStereo = TransformStereoScreenSpaceTex(o.texcoord, 1.0);

				float4 clip = float4(o.texcoord.xy*2-1, 0.0, 1.0);
				o.worldDir = mul(clipToWorld, clip) - _WorldSpaceCameraPos;
				return o;
			}

			float4 Frag(v2f i) : SV_Target
			{
				float3 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);

				float depth = SAMPLE_TEXTURE2D(_CameraDepthTexture, sampler_CameraDepthTexture, i.texcoord).r;
				depth = LinearEyeDepth(depth);
				float3 worldPos = i.worldDir * depth + _WorldSpaceCameraPos;

				float3 noiseUV = worldPos / _NoiseSize;

				float4 noiseX = SAMPLE_TEXTURE2D(_NoiseTex, sampler_NoiseTex, noiseUV.zy);
				float4 noiseY = SAMPLE_TEXTURE2D(_NoiseTex, sampler_NoiseTex, noiseUV.xz);
				float4 noiseZ = SAMPLE_TEXTURE2D(_NoiseTex, sampler_NoiseTex, noiseUV.xy);

				float4 normalTex = SAMPLE_TEXTURE2D(_CameraDepthNormalsTexture, sampler_CameraDepthNormalsTexture, i.texcoordStereo);
				float3 normal = DecodeViewNormalStereo(normalTex);

				float3 blend = pow(abs(normal), _Blend);
				blend /= dot(blend, 1.0f);

				float lum = dot(col, float3(0.3f, 0.59f, 0.11f));

				float3 noiseColor = noiseX * blend.x + noiseY * blend.y + noiseZ * blend.z;
				float threshold = dot(noiseColor, float3(0.3f, 0.59f, 0.11f)) + _ThresholdOffset;
				col = lum < threshold ? _DarkColor.rgb : _LightColor.rgb;

				return float4(col, 1.0f);
			}

			ENDHLSL
		}
	}
}
