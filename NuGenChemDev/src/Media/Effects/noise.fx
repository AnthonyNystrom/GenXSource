#include "inoise.fxh"

//------------------------------------
float4x4 worldViewProj : WorldViewProjection;
float4x4 world : World;

string description = "Perlin noise test";

float noiseScale
<
    string UIWidget = "slider";
    string UIName = "noise scale";
    float UIMin = 0.0; float UIMax = 20.0; float UIStep = 0.01;
> = 5.0;

float lacunarity
<
    string UIWidget = "slider";
    string UIName = "lacunarity";
    float UIMin = 0.0; float UIMax = 10.0; float UIStep = 0.01;
> = 2.0;

float gain
<
    string UIWidget = "slider";
    string UIName = "gain";
    float UIMin = 0.0; float UIMax = 1.0; float UIStep = 0.01;
> = 0.5;

float4 color1 : DIFFUSE
<
    string UIName = "color 1";
> = float4(0.0, 0.0, 0.5, 1.0);

float4 color2 : DIFFUSE
<
    string UIName = "color 2";
> = float4(0.0, 0.7, 0.0, 1.0);

/*float4 clrScale : DIFFUSE
<
    string UIName = "color 2";
> = float4(0, 0.7, -0.5, 0.0);*/

//------------------------------------
struct vertexInput {
    float4 position		: POSITION;
    //float2 texcoord     : TEXCOORD;
};

struct vertexOutput {
   float4 hPosition		: POSITION;
   //float2 texcoord      : TEXCOORD0;
   float3 wPosition		: TEXCOORD1;
};


//------------------------------------
vertexOutput VS(vertexInput IN) 
{
    vertexOutput OUT;
    OUT.hPosition = mul(IN.position, worldViewProj);
    //OUT.texcoord = IN.texcoord * noiseScale;
    OUT.wPosition = mul(IN.position, world).xyz * noiseScale;
    return OUT;
}

//-----------------------------------

float4 PS_turbulence(vertexOutput IN): COLOR
{
	float3 p = IN.wPosition;
	return tex2D(permSampler2d, float2(0.1, 0.6)); //color1 + ((color2 - color1) * turbulence(p, 4, lacunarity, gain));
}

//-----------------------------------
technique noiseTurbulence
{
    pass p0 
    {		
		VertexShader = compile vs_1_1 VS();
		PixelShader  = compile ps_2_a PS_turbulence();
    }
}