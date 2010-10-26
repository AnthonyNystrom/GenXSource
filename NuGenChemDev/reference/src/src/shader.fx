float4x4 WorldViewProj;
float3 light;
float ambient;

Texture Texture1;
Texture Texture2;

sampler samp1 = sampler_state { texture = <Texture1>; 
    minfilter = LINEAR; mipfilter = LINEAR; magfilter = LINEAR;};
sampler samp2 = sampler_state { texture = <Texture2>; 
    minfilter = LINEAR; mipfilter = LINEAR; magfilter = LINEAR;};
    
void Transform(
    in float4 inPos			: POSITION0,
    in float2 inCoord		: TEXCOORD0,    
    in float4 blend			: TEXCOORD1,
    in float3 normal		: NORMAL,    
    out float4 outPos		: POSITION0,
    out float2 outCoord		: TEXCOORD0,
    out float4 Blend		: TEXCOORD1,
    out float3 Normal		: TEXCOORD2,
    out float3 lightDir		: TEXCOORD3  )
{
    outPos = mul(inPos, WorldViewProj);				//transform position
    outCoord = inCoord;								
    Blend = blend;
    Normal = normalize(mul(normal,WorldViewProj));	// transform normal
    lightDir = inPos.xyz - light;					// calculate the direction of the light
}

float4 TextureColor(
 in float2 texCoord			: TEXCOORD0,
 in float4 blend			: TEXCOORD1,
 in float3 normal			: TEXCOORD2,
 in float3 lightDir			: TEXCOORD3) : COLOR0
{
    float4 texCol1 = tex2D(samp1, texCoord*4) * blend[0];
    float4 texCol2 = tex2D(samp2, texCoord) * blend[1];
    return (texCol1 + texCol2) * (saturate(dot(normalize(normal), normalize(light)))* (1-ambient) + ambient);
}

technique TransformTexture
{
    pass P0
    {
		CullMode = None;
    
        VertexShader = compile vs_1_1 Transform();
        PixelShader  = compile ps_2_0 TextureColor();
    }
}

