Shader "Custom/AudioSpectrum"
{
	Properties
	{
		_BackgroundTex("BackgroundTexture", 2D) = "black" {}
		_ForegroundTex("ForegroundTexture", 2D) = "black" {}
	}
		SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}

			sampler2D _BackgroundTex;
			sampler2D _ForegroundTex;

			fixed4 frag(v2f i) : SV_Target
			{
				float2 pos = i.vertex.xy / _ScreenParams.xy;
				/*pos.y = -1. + 2. * pos.y;*/
				pos.y = -pos.y;
				pos.y += 1.0f;

				float fft = tex2D(_BackgroundTex, float2(pos.x, 0.25)).x;
				float wave = tex2D(_ForegroundTex, float2(pos.x, 0.75)).x;

				float3 orange = float3(1., 0.647, 0.);
				float3 blue = float3(0.008, 0.031, 0.341);

				float intensity = step(pos.y, fft);
				float3 col = float3(intensity, intensity, intensity);// *float3(fft, fft, 3. * fft) * fft;

				/*pos.x *= 128.;
				col *= step(frac(pos.x), .8);
				pos.x /= 128.;*/

				return float4(col, 1.0);
			}
			ENDCG
		}
	}
}
