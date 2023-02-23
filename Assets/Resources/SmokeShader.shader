Shader "CUSTOM/SmokeShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    	_Color("Color", Color) = (0,0,0,0)
    	_HighlightColor("HighlightColor", Color) = (1,1,1,1)

	    _Resolution("Resolution", Vector) = (1200, 1000, 0,0) 
    	_Speed("Speed", Float) = 0.1
    	_TimeOffset("TimeOffset", Float) = 23
    	_ClearnessFactor("ClearnessFactor", Float) = 0.8
    	_FoamRadMin("FoamRadMin", Float) = 0.4
    	_FoamRadMax("FoamRadMax", Float) = 1.8
    	_ClearRadMin("ClearRadMin", Float) = 0.1
    	_ClearRadMax("ClearRadMax", Float) = 0.5
    	_FoamFactorMin("FoamFactorMin", Float) = 1
    	_FoamFactorMax("FoamFactorMax", Float) = 6
    	
    	_Step("Step", Float) = 0
    	_Power2("Power2", Float) = 10
    	_FilterStep("FilterStep", Float) = 0
    	
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
			#define TAU 6.28318530718
			#define TILING_FACTOR 1.0
			#define MAX_ITER 3

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _Color;
            float4 _HighlightColor;
            float4 _MainTex_ST;
            float4 _Resolution;
            float _Speed;
            float _TimeOffset;
            float _ClearnessFactor;
            float _FoamRadMin;
            float _FoamRadMax;
            float _ClearRadMin;
            float _ClearRadMax;
            float _Step;
            float _Power2;
			float _FoamFactorMin;
            float _FoamFactorMax;
            float _FilterStep;

			float waterHighlight(float2 p, float time, float foaminess)
			{
			    float2 i = float2(p);
				float c = 0.0;
			    float foaminess_factor = lerp(_FoamFactorMin, _FoamFactorMax, foaminess);
				float inten = .005 * foaminess_factor;

				for (int n = 0; n < MAX_ITER; n++) 
				{
					float t = time * (1.0 - (1 / float(n + 1) ));
					i = p + float2(cos(t - i.x) + sin(t + i.y), sin(t - i.y) + cos(t + i.x));
					c += 1.0 / length( float2(p.x / (sin(i.x+t)),p.y / (cos(i.y+t))) );
				}
				c = c / (inten * float(MAX_ITER));
				c = 1 - pow(c, 2.3);
			    c = pow(abs(c), _Power2);
				return c / sqrt(foaminess_factor);
			}


			void mainImage( out float4 fragColor, in float2 fragCoord, in float2 uvOr ) 
			{
				float time = _Time.y * _Speed + _TimeOffset;
				float2 uv = float2(fragCoord.x / _Resolution.x, fragCoord.y / _Resolution.y);
				float2 uv_square = float2(uv.x * _Resolution.x / _Resolution.y, uv.y);
			    float dist_center = pow( 3 * length(uv - 0.5), 2.0);
			    
			    float foaminess = smoothstep(_FoamRadMin, _FoamRadMax, dist_center);
			    float clearness = 0.1 + _ClearnessFactor * smoothstep(_ClearRadMin, _ClearRadMax, dist_center);
			    
				float2 p = fmod(uv_square * TAU * TILING_FACTOR, TAU) - 250.0;
			    
			    float c = waterHighlight(p, time, foaminess);
			    
				float3 mainColor = float3(_Color.xyz);
				c = step(_Step, c) * c;
				float3 highlightColor = float3(_HighlightColor.xyz) * c;
			    highlightColor = clamp(highlightColor + mainColor, 0.0, 1.0);
			    
			    highlightColor = lerp(mainColor, highlightColor, clearness);
				
				fragColor = float4(highlightColor, 1.0);
			}

            

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
            	float4 other = float4(1,1,1,1);
            	mainImage(other, i.vertex, i.uv);
            	// float filt = (other.r + other.g + other.b);
            	// filt = step(_FilterStep, filt);
            	// col = col * filt;
                return other ;
            }
            ENDCG
        }
    }
}
