Shader "CUSTOM/Lava"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM

            #define time _Time * 0.1

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

            sampler2D _MainTex;
            float4 _MainTex_ST;




			float hash21(in float2 n)
            {
	            return frac(sin(dot(n, float2(12.9898, 4.1414))) * 43758.5453);
            }
            
			float2x2 makem2(in float theta)
            {
	            float c = cos(theta);
				float s = sin(theta);
				return float2x2(c,-s,s,c);
            }
            
			float noise( in float2 i )
            {
            	return tex2D(_MainTex, float2(i * 0.01)).x;
            }

			float2 gradn(float2 p)
			{
				float ep = 0.09;
				float gradx = noise(float2(p.x+ep,p.y)) - noise(float2(p.x - ep, p.y));
				float grady = noise(float2(p.x,p.y+ep)) - noise(float2(p.x, p.y - ep));
				return float2(gradx,grady);
			}

			float flow(float2 p)
			{
				float z = 2.0;
				float rz = 0.0;
				float2 bp = p;
				for (float i= 1.;i < 7.;i++ )
				{
					//primary flow speed
					p += time*.6;
					
					//secondary flow speed (speed of the perceived flow)
					bp += time*1.9;
					
					//displacement field (try changing time multiplier)
					float2 gr = gradn(i * p * 0.34 + time * 1.0);
					
					//rotation of the displacement field
					gr = mul(gr, makem2(time * 6.0 - (0.05 * p.x + 0.03 * p.y) * 40.0));
					
					//displace the system
					p += gr * 0.5;
					
					//add noise octave
					rz += (sin(noise(p) * 7.0) * 0.5f + 0.5f) / z;
					
					//blend factor (blending displaced system with base system)
					//you could call this advection factor (.5 being low, .95 being high)
					p = lerp(bp, p, 0.77);
					
					//intensity scaling
					z *= 1.4;
					//octave scaling
					p *= 2.;
					bp *= 1.9;
				}
				return rz;	
			}

			float4 mainImage(float2 fragCoord)
			{
    //         	float2 iResolution = float2(1000, 1000);
				// float2 p = fragCoord.xy / iResolution.xy - 0.5;
				// p.x *= iResolution.x/iResolution.y;
				// p *= 3.0;
				float rz = flow(fragCoord);
				
				float3 col = float3(.2,0.07,0.01) / rz;
				col = pow(col,float3(1.4, 1.4, 1.4));
				return float4(col,1.0);
			}


            
            
            v2f vert (appdata i)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(i.vertex);
                o.uv = TRANSFORM_TEX(i.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // fixed4 col = tex2D(_MainTex, i.uv);
                float4 col = mainImage(i.uv);
                return col;
            }
            ENDCG
        }
    }
}
