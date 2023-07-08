Shader "Hidden/SnapshotPro/Blur"
{
	HLSLINCLUDE
	#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

	TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
	float2 _MainTex_TexelSize;
	uint _KernelSize;
	float _Spread;

	// Define Gaussian function constants.
	static const float E = 2.71828f;

	float gaussian(int x)
	{
		float sigmaSqu = _Spread * _Spread;
		return (1 / sqrt(TWO_PI * sigmaSqu)) * pow(E, -(x * x) / (2 * sigmaSqu));
	}
	ENDHLSL

	SubShader
	{
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			Name "HorizontalPass"
			HLSLPROGRAM

			#pragma vertex VertDefault
			#pragma fragment FragHorizontal

			float4 FragHorizontal(VaryingsDefault i) : SV_Target
			{
				float3 col = float3(0.0f, 0.0f, 0.0f);
				float kernelSum = 0.0f;

				int upper = ((_KernelSize - 1) / 2);
				int lower = -upper;

				float2 uv;

				for (int x = lower; x <= upper; ++x)
				{
					float gauss = gaussian(x);
					kernelSum += gauss;
					uv = i.texcoord + float2(_MainTex_TexelSize.x * x, 0.0f);
					col += gauss * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv).xyz;
				}

				col /= kernelSum;

				return float4(col, 1.0f);
			}

			ENDHLSL
		}

		Pass
		{
			Name "HorizontalPass"
			HLSLPROGRAM

			#pragma vertex VertDefault
			#pragma fragment FragVertical

			float4 FragVertical(VaryingsDefault i) : SV_Target
			{
				float3 col = float3(0.0f, 0.0f, 0.0f);
				float kernelSum = 0.0f;

				int upper = ((_KernelSize - 1) / 2);
				int lower = -upper;

				float2 uv;

				for (int y = lower; y <= upper; ++y)
				{
					float gauss = gaussian(y);
					kernelSum += gauss;
					uv = i.texcoord + float2(0.0f, _MainTex_TexelSize.y * y);
					col += gauss * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv).xyz;
				}

				col /= kernelSum;
				return float4(col, 1.0f);
			}

			ENDHLSL
		}
	}
}
