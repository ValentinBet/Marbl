﻿Shader "Custom/MarbleSurf"
{
	Properties
	{
		[Header(Texture)]
		_Color("Color", Color) = (1,1,1,1)
		_DetailColor("Detail Color", Color) = (1,1,1,1)
		[NoScaleOffset]_MainTex("Albedo (RGB)", 2D) = "white" {}
		 // _Color1("Second Mix", Color) = (0, 1, 1, 1)

		 //[NoScaleOffset] _TexMask("Mask", 2D) = "black" {}

		  [NoScaleOffset] _MetaRough("MetaRough", 2D) = "black"
		  [NoScaleOffset] _Normal("Normal", 2D) = "black"


		_TilingMultiplier("TilingMultiplier", Float) = 1
		_TilingOffset("TilingOffset",Float) = 1
	}
		SubShader
		  {
			  Tags { "RenderType" = "Opaque" }
			  LOD 200

			  CGPROGRAM
			  // Physically based Standard lighting model, and enable shadows on all light types
			  #pragma surface surf Standard fullforwardshadows

			  // Use shader model 3.0 target, to get nicer looking lighting
			  #pragma target 3.0

			sampler2D _MainTex;
			struct Input
			{
				float2 uv_MainTex;
			};

			uniform sampler2D _MetaRough;
			uniform sampler2D _Normal;
			uniform float _TilingMultiplier;
			uniform float _TilingOffset;
			fixed4 _Color;
			fixed4 _DetailColor;
			// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
			// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
			// #pragma instancing_options assumeuniformscaling
			UNITY_INSTANCING_BUFFER_START(Props)
				// put more per-instance properties here
			UNITY_INSTANCING_BUFFER_END(Props)

			void surf(Input IN, inout SurfaceOutputStandard o)
			{
				IN.uv_MainTex = _TilingMultiplier * float2(IN.uv_MainTex.x, (1 - IN.uv_MainTex.y)) +_TilingOffset;
				// Albedo comes from a texture tinted by color
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
				c.rgb = lerp(c.rgb, _DetailColor, c.a);
				o.Albedo = c.rgb;
				// Metallic and smoothness come from slider variables
				o.Metallic = tex2D(_MetaRough, IN.uv_MainTex).r;
				o.Smoothness = 1.0f-tex2D(_MetaRough, IN.uv_MainTex).a;
				o.Normal = UnpackNormal(tex2D(_Normal, IN.uv_MainTex));
			}
			ENDCG
		}
			FallBack "Diffuse"
}
