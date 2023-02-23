Shader "CUSTOM/PureFog"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color("Color", Color) = (1,1,1,1)
        _CloudDensity("_CloudDensity", float) = 1
        _Noisiness("_Noisiness", float) = 0.35
        _speed("_speed", float) = 0.1
        _CloudHeight("_CloudHeight", float) = 2.5
        _FallOff("FallOff", float) = 1
        _AlphaAdd("AddedAlpha", float) = 0
        
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" }
        LOD 100
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            #define ITERATIONS 8
            #define CENTER 0.5
            
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
            float4 _Color;

            float _CloudDensity = 1.0; 	// overall density [0,1]
            float _Noisiness = 0.35; 	// overall strength of the noise effect [0,1]
            float _speed = 0.1;			// controls the animation speed [0, 0.1 ish)
            float _CloudHeight = 2.5; 	// (inverse) height of the input gradient [0,...)
            float _FallOff;
            float _AlphaAdd;
             /// Cloud stuff:
            static float maximum = 1.0/1.0 + 1.0/2.0 + 1.0/3.0 + 1.0/4.0 + 1.0/5.0 + 1.0/6.0 + 1.0/7.0 + 1.0/8.0;
            // static float maximum = 1.0/1.0 + 1.0/2.0 + 1.0/3.0 + 1.0/4.0 + 1.0/5.0 + 1.0/6.0 + 1.0/7.0 + 1.0/8.0;

            
            // Simplex noise below = ctrl+c, ctrl+v:
            // Description : Array and textureless GLSL 2D/3D/4D simplex 
            //               noise functions.
            //      Author : Ian McEwan, Ashima Arts.
            //  Maintainer : ijm
            //     Lastmod : 20110822 (ijm)
            //     License : Copyright (C) 2011 Ashima Arts. All rights reserved.
            //               Distributed under the MIT License. See LICENSE file.
            //               https://github.com/ashima/webgl-noise
            // 

            float3 mod289(float3 x)
            {
              return x - floor(x * (1.0 / 289.0)) * 289.0;
            }

            float4 mod289(float4 x)
            {
              return x - floor(x * (1.0 / 289.0)) * 289.0;
            }

            float4 permute(float4 x)
            {
                 return mod289(((x*34.0)+1.0)*x);
            }

            float4 taylorInvSqrt(float4 r)
            {
              return 1.79284291400159 - 0.85373472095314 * r;
            }

            float snoise(float3 v)
            { 
              const float2  C = float2(1.0/6.0, 1.0/3.0) ;
              const float4  D = float4(0.0, 0.5, 1.0, 2.0);

            // First corner
              float3 i  = floor(v + dot(v, C.yyy) );
              float3 x0 =   v - i + dot(i, C.xxx) ;

            // Other corners
              float3 g = step(x0.yzx, x0.xyz);
              float3 l = 1.0 - g;
              float3 i1 = min( g.xyz, l.zxy );
              float3 i2 = max( g.xyz, l.zxy );

              //   x0 = x0 - 0.0 + 0.0 * C.xxx;
              //   x1 = x0 - i1  + 1.0 * C.xxx;
              //   x2 = x0 - i2  + 2.0 * C.xxx;
              //   x3 = x0 - 1.0 + 3.0 * C.xxx;
              float3 x1 = x0 - i1 + C.xxx;
              float3 x2 = x0 - i2 + C.yyy; // 2.0*C.x = 1/3 = C.y
              float3 x3 = x0 - D.yyy;      // -1.0+3.0*C.x = -0.5 = -D.y
                // Permutations
              i = mod289(i); 
              float4 p = permute( permute( permute( 
                         i.z + float4(0.0, i1.z, i2.z, 1.0 ))
                       + i.y + float4(0.0, i1.y, i2.y, 1.0 )) 
                       + i.x + float4(0.0, i1.x, i2.x, 1.0 ));

              // Gradients: 7x7 points over a square, mapped onto an octahedron.
              // The ring size 17*17 = 289 is close to a multiple of 49 (49*6 = 294)
              float n_ = 0.142857142857; // 1.0/7.0
              float3  ns = n_ * D.wyz - D.xzx;

              float4 j = p - 49.0 * floor(p * ns.z * ns.z);  //  mod(p,7*7)

              float4 x_ = floor(j * ns.z);
              float4 y_ = floor(j - 7.0 * x_ );    // mod(j,N)

              float4 x = x_ *ns.x + ns.yyyy;
              float4 y = y_ *ns.x + ns.yyyy;
              float4 h = 1.0 - abs(x) - abs(y);

              float4 b0 = float4( x.xy, y.xy );
              float4 b1 = float4( x.zw, y.zw );

              //float4 s0 = float4(lessThan(b0,0.0))*2.0 - 1.0;
              //float4 s1 = float4(lessThan(b1,0.0))*2.0 - 1.0;
              float4 s0 = floor(b0)*2.0 + 1.0;
              float4 s1 = floor(b1)*2.0 + 1.0;
              float4 sh = -step(h, float4(0.0, 0.0, 0.0, 0.0));

              float4 a0 = b0.xzyw + s0.xzyw*sh.xxyy ;
              float4 a1 = b1.xzyw + s1.xzyw*sh.zzww ;

              float3 p0 = float3(a0.xy,h.x);
              float3 p1 = float3(a0.zw,h.y);
              float3 p2 = float3(a1.xy,h.z);
              float3 p3 = float3(a1.zw,h.w);

              //Normalise gradients
              float4 norm = taylorInvSqrt(float4(dot(p0,p0), dot(p1,p1), dot(p2, p2), dot(p3,p3)));
              p0 *= norm.x;
              p1 *= norm.y;
              p2 *= norm.z;
              p3 *= norm.w;

              // Mix final noise value
              float4 m = max(0.6 - float4(dot(x0,x0), dot(x1,x1), dot(x2,x2), dot(x3,x3)), 0.0);
              m = m * m;
              return 42.0 * dot( m*m, float4( dot(p0,x0), dot(p1,x1), 
                                            dot(p2,x2), dot(p3,x3) ) );
            }
            

            // Fractal Brownian motion, or something that passes for it anyway: range [-1, 1]
            float FractalBrownian(float3 uv)
            {
                float sum = 0.0;
                for (int i = 0; i < ITERATIONS; ++i) {
                    float f = float(i+1);
                    sum += snoise(uv * f) / f;
                }
                return sum / maximum;
            }

            // Simple vertical gradient:
            float Gradient(float2 uv)
            {
                float d = length( uv - float2(CENTER, CENTER)  );
                return saturate(1 - _FallOff * d);
            }

            void FogEffect( out float4 fragColor, in float2 uv )
            {
	            // float2 uv = fragCoord.xy / iResolution.xy;
                float3 p = float3(uv, _Time.y * _speed);
                float3 someRandomOffset = float3(0.1, 0.3, 0.2);
                someRandomOffset = float3(0,0,0);
                float2 duv = float2(FractalBrownian(p), FractalBrownian(p + someRandomOffset)) * _Noisiness;
                float q = Gradient(uv + duv) * _CloudDensity;
	            fragColor = float4(q,q,q, q);
            }

            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                
                return o;
            }

            static float4 unitary4 = (1,1,1,1);
            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float4 col;
                FogEffect(col, i.uv);
                col.w += _AlphaAdd;
                col = _Color * col;
                return col;
            }
            ENDCG
        }
    }
}
