Shader "Custom/MinimalShader2"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		[HDR] _Emissive("Emissive", Color) = (1,1,1,1)
		_MainTex("Text",2D) = "white" {}
		_Glossinnes("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
	}

		SubShader
		{
			Tags
			{
				"RenderType" = "Opaque"
				//       "Queue" = "Transparent+0"

				   }

			/* Pass
			 {
				 Blend One OneMinusSrcAlpha
				 Cull Off
				 Lighting Off
				 ZWrite On
				 ZTest LEqual
			 */
				 CGPROGRAM
				 #pragma target 3.0
				 #pragma surface surf Standard fullforwardsshadows

			//#pragma vertex vert
			//#pragma fragment frag

			uniform sampler2D _MainTex;
			struct Input {
				float2 uv_MainTex;
			};

			uniform float4 _Color;

			half _Glossiness;
			float4 _Emissive;

			half _Metallic;
			/*
			uniform float4 _Color1;
			uniform sampler2D _TexMask;
			uniform float _TilingMultiplier;
			uniform float _TilingOffset;
			uniform float _TilingMultiplierMask;*/
			/*
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : POSITION;

				float2 texoord : TEX0;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);

				o.texoord = _TilingMultiplier * float2(v.uv.x,(1-  v.uv.y));
				return o;
			}

			float4 frag (v2f IN) : COLOR
			{
				float4 mask = tex2D(_TexMask, IN.texoord *_TilingMultiplierMask);

				//float4 result1 = tex2D(_Tex1, IN.texoord + _TilingOffset)*(_Color0);
				float4 result2 = tex2D(_Tex2, IN.texoord + _TilingOffset)* _Color1;

				float4 result = lerp(_Color0, result2, mask);

				return result;
			}
			ENDCG*/
			void surf(Input IN, inout SurfaceOutputStandard o) {
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex)* _Color;
				//float grayscale = (c.r + c.g + c.b) / 3;
				o.Albedo = c.rgb;
				o.Metallic = _Metallic;
				o.Emission = _Emissive.rgb;
				o.Smoothness = _Glossiness;
				o.Alpha = c.a;
			}
			ENDCG
		}
			FallBack "Diffuse"
}

