// float noise3D(float3 p)
// {
//     p.z = frac(p.z)*256.0;
//     float iz = floor(p.z);
//     float fz = frac(p.z);
//     float2 a_off = (23.0, 29.0)*(iz)/256.0;
//     float2 b_off = (23.0, 29.0)*(iz+1.0)/256.0;
//     //tex2Dlod(_MainTex, float4(vertex.texcoord.xy, 0.0, 0.0));
//     float a = tex2Dlod(_MainTex, float4(p, 0.0));
//     float b = tex2Dlod(_MainTex, float4(p, 0.0));
//     // float a = texture(iChannel0, p.xy + a_off, -999.0).r;
//     // float b = texture(iChannel0, p.xy + b_off, -999.0).r;
//     return lerp(a, b, fz) - 0.5;
// }

// float perlin(float3 p)
// {
//     float v = 0.0;
//     for (float i = 0.0; i < 6.0; i += 1.0)
//         v += noise3D(p * pow(2.0, i)) * pow(0.35, i);
//     return v;
// }

// float snoise(float3 x) {
//     float3 p = floor(x);
//     float3 f = frac(x);
//     f = f*f*(3.0-2.0*f);
    
//     float2 uv = (p.xy+(37.0,17.0)*p.z) + f.xy;
//     //float2 rg = tex2Dlod( _MainTex, (uv+ 0.5) /256.0, 0.0 ).yx;
//     //float2 rg = textureLod( iChannel0, (uv+ 0.5)/256.0, 0.0 ).yx;
//     float2 rg = tex2Dlod(_MainTex, float4(p, 0.0)).yx;
//     return lerp( rg.x, rg.y, f.z );
// }

// fixed noise(fixed3 x) // iq's 3D noise
// {
//     fixed3 f = frac(x);
//     fixed3 p = x - f;
//     f = f*f*(3.0 - 2.0*f);
//     fixed2 uv = (p.xy + fixed2(37.0, 17.0) * p.z) + f.xy;
//     fixed2 rg = tex2D(_MainTex, (uv + 0.5)/256.0, 0, 0).rg;
//     return lerp(rg.y, rg.x, f.z);
// }


// float fbm(float3 x)
// {
//     float r = 0.0;
//     float w = 1.0, s = 1.0;
//     for (int i=0; i<5; i++)
//     {
//         w *= 0.5;
//         s *= 2.0;
//         r += w * noise(s * x);
//     }
//     return r;
// }

// float2 hash( float2 p ) // replace this by something better
// {
// 	p = float2( dot(p,float2(127.1,311.7)), dot(p,float2(269.5,183.3)) );
// 	return -1.0 + 2.0*frac(sin(p)*43758.5453123);
// }

// float noise( in float2 p )
// {
//     const float K1 = 0.366025404; // (sqrt(3)-1)/2;
//     const float K2 = 0.211324865; // (3-sqrt(3))/6;

// 	float2  i = floor( p + (p.x+p.y)*K1 );
//     float2  a = p - i + (i.x+i.y)*K2;
//     float m = step(a.y,a.x); 
//     float2  o = float2(m,1.0-m);
//     float2  b = a - o + K2;
// 	float2  c = a - 1.0 + 2.0*K2;
//     float3  h = max( 0.5-float3(dot(a,a), dot(b,b), dot(c,c) ), 0.0 );
// 	float3  n = h*h*h*h*float3( dot(a,hash(i+0.0)), dot(b,hash(i+o)), dot(c,hash(i+1.0)));
//     return dot( n, float3(70.0) );
// }

float hash( float n )
{
    return frac(sin(n)*43758.5453);
}

float noise( float3 x )
{
    // The noise function returns a value in the range -1.0f -> 1.0f

    float3 p = floor(x);
    float3 f = frac(x);

    f       = f*f*(3.0-2.0*f);
    float n = p.x + p.y*57.0 + 113.0*p.z;

    return lerp(lerp(lerp( hash(n+0.0), hash(n+1.0),f.x),
                   lerp( hash(n+57.0), hash(n+58.0),f.x),f.y),
               lerp(lerp( hash(n+113.0), hash(n+114.0),f.x),
                   lerp( hash(n+170.0), hash(n+171.0),f.x),f.y),f.z);
}
