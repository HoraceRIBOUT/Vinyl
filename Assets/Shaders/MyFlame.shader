Shader "Custom/MyFlame"
{
    Properties
    {
        _NoiseTex("Noise", 2D) = "white" {}
		_Gradient("Gradient",2D) = "white" {}
		_Speed("Speed", Vector) = (1,1,0,0)
		_Color("Color base", Color) = (1,1,1,1)
		_ColorOnSource("Color on source", Color) = (1,1,1,1)
_Hard("Hard Cutoff", Range(1,40)) = 30
_Edge("Edge", Range(0,2)) = 1

_Offset("Offset gradientMain", Range(-2,2)) = 1


		_TintA("Edge Color A", Color) = (1,1,1)
		_TintB("Edge Color B", Color) = (1,1,1)

    }
    SubShader
    {
		Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }
		LOD 100
		Zwrite Off
		Blend One One // additive blending

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

#pragma multi_compile _ SHAPE_ON
#pragma multi_compile _ SHAPEX_ON

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
				float2 uv2 : TEXCOORD2;
                float4 vertex : SV_POSITION;
            };

			sampler2D _NoiseTex, _Gradient;
            float4 _NoiseTex_ST, _Gradient_ST;
			float4 _Color;

			float _Hard, _Edge, _Offset;

			float2 _Speed;

			float4 _ColorOnSource, _TintB, _TintA;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _NoiseTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
				o.uv2 = TRANSFORM_TEX(v.uv, _Gradient);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				fixed4 baseCol = _Color;
			
				
			
				/*float valueX = (sin(_Time.x * _Speed.x) + 1) * 0.5;
				float valueY = (cos(_Time.x * _Speed.y) + 1) * 0.5;
				float2 newUV = float2(i.uv.x * valueX, i.uv.x * valueY);

				
				
				//sin(_Time.x * _Speed * )*/
				float4 gradient = tex2D(_Gradient, fixed2(frac(i.uv2.x), i.uv2.y));
				
				fixed2 newUV = fixed2(i.uv.x, i.uv.y + (_Time.x) * _Speed.y);
				float4 noise = tex2D(_NoiseTex, frac(newUV));
				if (noise.r < 0.4 * gradient.r)
					baseCol *= 1;
				else if (noise.r < 0.8 * gradient.r) {
					float lerpVal = (noise.r - 0.4) * 0.25;
					baseCol = lerp(_TintA, _TintB, lerpVal) * gradient.r;
				}
				else
					baseCol *= 0;


				/*float4 gradientMain = lerp(_ColorOnSource, _Color, (i.uv.y + _Offset)); // gradient for color
				float4 gradientTint = lerp(_TintB, _TintA, (i.uv.y + _Offset)); // gradient for color

				float4 flame = saturate(baseCol.a * _Hard); //noise flame
				float4 flamecolored = flame * gradientMain; // coloured noise flame
				float4 flamerim = saturate((baseCol.a + _Edge) * _Hard) - flame; // noise flame edge

				float4 flamecolored2 = flamerim * gradientTint; // coloured flame edge
				float4 finalcolor = flamecolored + flamecolored2; // combined edge and flames					*/


				baseCol.r *= 1.0f + (gradient.r + gradient.r * noise.g) * _ColorOnSource.x;
				baseCol.g *= 1.0f + (gradient.r + gradient.r * noise.g) * _ColorOnSource.y;
				baseCol.b *= 1.0f + (gradient.r + gradient.r * noise.g) * _ColorOnSource.z;


				//baseCol.g *= 1.0f + gradient + gradient * noise

				//mask.x += gradient * sin(time + mask.y) * noise;

				//baseCol.a *= shape.a;
				
				
				//finalcolor *= baseCol.a;


                UNITY_APPLY_FOG(i.fogCoord, baseCol);
                return baseCol;
            }
            ENDCG
        }
    }
}
