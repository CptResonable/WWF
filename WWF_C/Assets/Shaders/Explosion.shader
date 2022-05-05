// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/Explosion"
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
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 normal : NORMAL;  // NormalOS
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float noise3D(float3 p)
            {
                p.z = frac(p.z)*256.0;
                float iz = floor(p.z);
                float fz = frac(p.z);
                float2 a_off = (23.0, 29.0)*(iz)/256.0;
                float2 b_off = (23.0, 29.0)*(iz+1.0)/256.0;
                //tex2Dlod(_MainTex, float4(vertex.texcoord.xy, 0.0, 0.0));
                float a = tex2Dlod(_MainTex, float4(p, 0.0));
                float b = tex2Dlod(_MainTex, float4(p, 0.0));
                // float a = texture(iChannel0, p.xy + a_off, -999.0).r;
                // float b = texture(iChannel0, p.xy + b_off, -999.0).r;
                return lerp(a, b, fz) - 0.5;
            }
            
            float perlin(float3 p)
            {
                float v = 0.0;
                for (float i = 0.0; i < 6.0; i += 1.0)
                    v += noise3D(p * pow(2.0, i)) * pow(0.35, i);
                return v;
            }
        
            float snoise(float3 x) {
                float3 p = floor(x);
                float3 f = frac(x);
                f = f*f*(3.0-2.0*f);
                
                float2 uv = (p.xy+(37.0,17.0)*p.z) + f.xy;
                //float2 rg = tex2Dlod( _MainTex, (uv+ 0.5) /256.0, 0.0 ).yx;
                //vec2 rg = textureLod( iChannel0, (uv+ 0.5)/256.0, 0.0 ).yx;
                float2 rg = tex2Dlod( _MainTex, float4(p, 0.0)).yx;
                return lerp( rg.x, rg.y, f.z );
            }
            fixed noise(fixed3 x) // iq's 3D noise
            {
                fixed3 f = frac(x);
                fixed3 p = x - f;
                f = f*f*(3.0 - 2.0*f);
                fixed2 uv = (p.xy + fixed2(37.0, 17.0) * p.z) + f.xy;
                fixed2 rg = tex2D(_MainTex, (uv + 0.5)/256.0, 0, 0).rg;
                return lerp(rg.y, rg.x, f.z);
            }


            float fbm(float3 x)
            {
                float r = 0.0;
                float w = 1.0, s = 1.0;
                for (int i=0; i<5; i++)
                {
                    w *= 0.5;
                    s *= 2.0;
                    r += w * noise(s * x);
                }
                return r;
            }

            v2f vert (appdata v)
            {
                // v.vertex.rgb = v.vertex.rgb + v.normal * lerp(perlin(v.vertex.rgb) * 0.2, 1, 0.1);
                //output.WorldPos = mul(input.Pos,World);
                float3 worldPos = mul (v.vertex.xyz, unity_ObjectToWorld).xyz;
                //float3 worldPos = mul(UNITY_MATRIX_MV, v.vertex);
                //v.vertex.rgb = v.vertex.rgb + v.normal * lerp(worldPos, 1, 0.2);

               // v.vertex.rgb = v.vertex.rgb + v.normal * (noise(worldPos * 3) * 3);
                
                //v.vertex.rgb = v.vertex.rgb + v.normal * 0.2;
                 v.vertex.rgb = v.vertex.rgb + v.normal * noise(((v.vertex.rgb + float3(40, 32, 23)) * 1) * 2) * 8;
                //v.vertex.rgb = v.vertex.rgb + v.normal * lerp(noise((v.vertex.rgb + float3(40, 32, 23)) * 5) * 2, 1, 0.1);

                // v.vertex.rgb = v.vertex.rgb + v.normal * lerp(noise((v.vertex.rgb) * 0.2f), 1, 0.2);
                //v.vertex.rgb = v.vertex.rgb + v.normal * lerp(perlin((v.vertex.rgb + float3(432, 10, 40)) * 0.2f), 1, 0.2);
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                //fixed4 col = float4(0.5, 0.5, 0.5, 0);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
