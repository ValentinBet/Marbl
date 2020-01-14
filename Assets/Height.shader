Shader "Custom/Height"
{
		//show values to edit in inspector
		Properties{
			_Scale("Pattern Size", Range(0,10)) = 1
			_EvenColor("Checker Color 1", Color) = (0,0,0,1)
			_OddColor("Checker Color 2", Color) = (1,1,1,1)
			_Col1("Color 1", Color) = (1,0,0,1)
			_Col2("Color 2", Color) = (0,1,0,1)
			_Minimal1Height("_Color1Height",Float) = 3
			_Minimal2Height("_Color2Height",Float) = 1
		}

			SubShader{
			//the material is completely non-transparent and is rendered at the same time as the other opaque geometry
			Tags{ "RenderType" = "Opaque" "Queue" = "Geometry"}


			Pass{
				CGPROGRAM
				#include "UnityCG.cginc"

				#pragma vertex vert
				#pragma fragment frag

				float _Scale;

		float _Minimal1Height;
		float _Minimal2Height;
				float4 _EvenColor;
				float4 _OddColor;
				float4 _Col1;
				float4 _Col2;

				struct appdata {
					float4 vertex : POSITION;
				};

				struct v2f {
					float4 position : SV_POSITION;
					float3 worldPos : TEXCOORD0;
				};

				v2f vert(appdata v) {
					v2f o;
					//calculate the position in clip space to render the object
					o.position = UnityObjectToClipPos(v.vertex);
					//calculate the position of the vertex in the world
					o.worldPos = mul(unity_ObjectToWorld, v.vertex);
					return o;
				}

				fixed4 frag(v2f i) : SV_TARGET{
					//scale the position to adjust for shader input and floor the values so we have whole numbers
					float3 adjustedWorldPos = floor(i.worldPos / _Scale);
					//add different dimensions 
					float chessboard = adjustedWorldPos.x+adjustedWorldPos.z;
					//divide it by 2 and get the fractional part, resulting in a value of 0 for even and 0.5 for off numbers.
					chessboard = frac(chessboard * 0.5);
					//multiply it by 2 to make odd values white instead of grey
					chessboard *= 2;

					//interpolate between color for even fields (0) and color for odd fields (1)
					float4 color = (0,0,0,0);
					if (i.worldPos.y > _Minimal1Height)
					{
						color = lerp(_EvenColor, _OddColor, chessboard);
					}
					if(i.worldPos.y < _Minimal1Height)
					{
						color = _Col1;
					}
					if (i.worldPos.y < _Minimal2Height)
					{
						color = _Col2;
					}
					return color;
				}

				ENDCG
			}
		}
			FallBack "Standard" //fallback adds a shadow pass so we get shadows on other objects
	}