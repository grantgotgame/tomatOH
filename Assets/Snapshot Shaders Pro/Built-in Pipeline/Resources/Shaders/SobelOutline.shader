Shader "Hidden/SnapshotPro/SobelOutline"
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
			float2 _MainTex_TexelSize;

			float _Threshold;
			float4 _OutlineColor;
			float4 _BackgroundColor;

			float sobel(float2 uv)
			{
				float3 x = 0;
				float3 y = 0;

				float2 texelSize = _MainTex_TexelSize;

				// Values are hardcoded for simplicity. Kernel values with
				// zeroes are missed out for efficiency.
				x += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + float2(-texelSize.x, -texelSize.y)).xyz * -1.0;
				x += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + float2(-texelSize.x, 0)).xyz * -2.0;
				x += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + float2(-texelSize.x, texelSize.y)).xyz * -1.0;

				x += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + float2(texelSize.x, -texelSize.y)).xyz *  1.0;
				x += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + float2(texelSize.x, 0)).xyz *  2.0;
				x += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + float2(texelSize.x, texelSize.y)).xyz *  1.0;

				y += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + float2(-texelSize.x, -texelSize.y)).xyz * -1.0;
				y += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + float2(0, -texelSize.y)).xyz * -2.0;
				y += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + float2(texelSize.x, -texelSize.y)).xyz * -1.0;

				y += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + float2(-texelSize.x, texelSize.y)).xyz *  1.0;
				y += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + float2(0, texelSize.y)).xyz *  2.0;
				y += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + float2(texelSize.x, texelSize.y)).xyz *  1.0;

				float xLum = dot(x, float3(0.2126729, 0.7151522, 0.0721750));
				float yLum = dot(y, float3(0.2126729, 0.7151522, 0.0721750));

				return sqrt(xLum * xLum + yLum * yLum);
			}

			float4 Frag(VaryingsDefault i) : SV_Target
			{
				float s = sobel(i.texcoord);
				float4 sobelCol = s > _Threshold ? _OutlineColor : _BackgroundColor;
				float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);

				return lerp(col, sobelCol, sobelCol.a);
			}

			ENDHLSL
		}
	}
}
