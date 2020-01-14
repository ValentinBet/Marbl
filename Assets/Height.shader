// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Height"
{
	Properties{
		  _Position("Position", Vector) = (.0, .0, .0)
		  _HaloColor("Halo Color", Color) = (1.0, 1.0, 1.0, 1.0)
	}

		SubShader{
			Tags { "Queue" = "Transparent" "Render" = "Transparent" "IgnoreProjector" = "True"}
			LOD 200

			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			Pass{
				CGPROGRAM

				#pragma target 3.0
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

				struct appdata {
					float4 vertex : POSITION;

				};

				struct v2f {
					float4 vertex : SV_POSITION;
					float3 worldPos : TEXCOORD0;
				};

				uniform float3 _Position;
				uniform float4 _HaloColor;


				v2f vert(appdata v) {
					v2f o;

					o.worldPos = mul(unity_ObjectToWorld, v.vertex);
					o.vertex = UnityObjectToClipPos(v.vertex);

					return o;
				}

				fixed4 frag(v2f i) : SV_Target {
					fixed4 col = _HaloColor;
					col.a = 1 - distance(i.worldPos, _Position);
					return col;
				}

				ENDCG
			}


	}
		FallBack "Diffuse"
}
