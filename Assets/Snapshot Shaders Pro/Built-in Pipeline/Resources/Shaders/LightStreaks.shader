Shader "Hidden/SnapshotPro/LightStreaks"
{
	HLSLINCLUDE
	#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

	TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
	float2 _MainTex_TexelSize;
	
	ENDHLSL

	SubShader
	{
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			Name "HorizontalBlur"
			HLSLPROGRAM

			#pragma vertex VertDefault
			#pragma fragment FragHorizontal

			uint _KernelSize;
			float _Spread;

			float _LuminanceThreshold;

			// Define Gaussian function constants.
			static const float E = 2.71828f;

			float gaussian(int x)
			{
				float sigmaSqu = _Spread * _Spread;
				return (1 / sqrt(TWO_PI * sigmaSqu)) * pow(E, -(x * x) / (2 * sigmaSqu));
			}

			float4 FragHorizontal(VaryingsDefault i) : SV_Target
			{
				float3 light = 0.0f;
				float kernelSum = 0.0f;

				int upper = ((_KernelSize - 1) / 2);
				int lower = -upper;

				float2 uv;

				for (int x = lower; x <= upper; ++x)
				{
					float gauss = gaussian(x);
					kernelSum += gauss;
					uv = i.texcoord + float2(_MainTex_TexelSize.x * x, 0.0f);

					float3 newLight = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv).xyz;
					float lum = dot(newLight, float3(0.3f, 0.59f, 0.11f));
					light += step(_LuminanceThreshold, lum) * newLight * gauss;
				}

				light /= kernelSum;

				return float4(light, 1.0f);
			}

			ENDHLSL
		}

		Pass
		{
			Name "Overlay"
			HLSLPROGRAM

			#pragma vertex VertDefault
			#pragma fragment FragOverlay

			TEXTURE2D_SAMPLER2D(_BlurTex, sampler_BlurTex);

			float4 FragOverlay(VaryingsDefault i) : SV_Target
			{
				float4 mainCol = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
				float4 blurCol = SAMPLE_TEXTURE2D(_BlurTex, sampler_BlurTex, i.texcoord);

				return mainCol + blurCol;
			}

			ENDHLSL
		}
	}
}
