Shader "Hidden/SnapshotPro/SobelNeon"
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

			float _SaturationFloor;
			float _LightnessFloor;

			float3 sobel(float2 uv)
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

			// Credit: http://lolengine.net/blog/2013/07/27/rgb-to-hsv-in-glsl
			float3 rgb2hsv(float3 c)
			{
				float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
				float4 p = c.g < c.b ? float4(c.bg, K.wz) : float4(c.gb, K.xy);
				float4 q = c.r < p.x ? float4(p.xyw, c.r) : float4(c.r, p.yzx);

				float d = q.x - min(q.w, q.y);
				float e = 1.0e-10;
				return float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
			}

			// Credit: http://lolengine.net/blog/2013/07/27/rgb-to-hsv-in-glsl
			float3 hsv2rgb(float3 c)
			{
				float4 K = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
				float3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
				return c.z * lerp(K.xxx, saturate(p - K.xxx), c.y);
			}

			float4 Frag(VaryingsDefault i) : SV_Target
			{
				float3 s = sobel(i.texcoord);
				float3 tex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord).xyz;

				float3 hsvTex = rgb2hsv(tex);
				hsvTex.y = max(hsvTex.y, _SaturationFloor);	// Modify saturation.
				hsvTex.z = max(hsvTex.z, _LightnessFloor);	// Modify lightness/value.
				float3 col = hsv2rgb(hsvTex);

				return float4(col * s, 1.0f);
			}

			ENDHLSL
		}
	}
}
