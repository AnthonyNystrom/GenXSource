/// UNIFORMS

float4x4 worldViewProj : WorldViewProjection;
float4x4 world   : World;
float4x4 worldInverseTranspose : WorldInverseTranspose;
float4x4 viewInverse : ViewInverse;
texture g_txScene;
texture g_txBrightSource;
texture g_hBlurMap;

/// VARIABLES

// The Y luminance transformation used follows that used by TIFF and JPEG (Rec 601-1)
const float3 luminanceFilter = { 0.2989, 0.5866, 0.1145 };

float SceneIntensity <
    string UIName = "Scene intensity";
    string UIWidget = "slider";
    float UIMin = 0.0f;
    float UIMax = 2.0f;
    float UIStep = 0.1f;
> = 0.5f;

float GlowIntensity <
    string UIName = "Glow intensity";
    string UIWidget = "slider";
    float UIMin = 0.0f;
    float UIMax = 2.0f;
    float UIStep = 0.1f;
> = 0.5f;

float HighlightThreshold <
    string UIName = "Highlight threshold";
    string UIWidget = "slider";
    float UIMin = 0.0f;
    float UIMax = 1.0f;
    float UIStep = 0.1f;
> = 0.9f;

float HighlightIntensity <
    string UIName = "Highlight intensity";
    string UIWidget = "slider";
    float UIMin = 0.0f;
    float UIMax = 10.0f;
    float UIStep = 0.1f;
> = 0.5f;

float2 texelSize;// : VIEWPORTPIXELSIZE;

float BlurWidth <
    string UIName = "Blur width";
    string UIWidget = "slider";
    float UIMin = 0.0f;
    float UIMax = 10.0f;
    float UIStep = 0.5f;
> = 1.0f;

/// SAMPLERS

sampler2D g_sourceSample =
sampler_state
{
    Texture = <g_txScene>;
    MinFilter = NONE;
    MagFilter = NONE;
    MipFilter = None;
    AddressU = Clamp;
    AddressV = Clamp;
};

sampler BrightSourceSampler = sampler_state 
{
    texture = <g_txBrightSource>;
    AddressU  = CLAMP;        
    AddressV  = CLAMP;
    AddressW  = CLAMP;
    MIPFILTER = NONE;
    MINFILTER = LINEAR;
    MAGFILTER = LINEAR;
};

sampler HBlurSampler = sampler_state 
{
    texture = <g_hBlurMap>;
    AddressU  = CLAMP;        
    AddressV  = CLAMP;
    AddressW  = CLAMP;
    MIPFILTER = NONE;
    MINFILTER = LINEAR;
    MAGFILTER = LINEAR;
};

/// STRUCTURES

struct brightPassIn {
	float3 position				: POSITION;
    float3 texCoord				: TEXCOORD0;
};

struct brightPassVOut {
	float4 position		: POSITION;
	float2 texCoord		: TEXCOORD0;
};

struct VS_OUTPUT_BLUR
{
    float4 Position   : POSITION;
    float2 TexCoord0 : TEXCOORD0;
};

struct finalVSOutput {
	float4 position   : POSITION;
	float2 texCoord   : TEXCOORD0;
};

const half weights7[7] = {
	0.05,
	0.1,
	0.2,
	0.3,
	0.2,
	0.1,
	0.05,
};	

/// VERTEX SHADERS

brightPassVOut BrightVertexPass(brightPassIn IN)
{
	brightPassVOut OUT;
	OUT.position = mul(float4(IN.position.xyz , 1.0), worldViewProj);
	OUT.texCoord = IN.texCoord;
	return OUT;
}

VS_OUTPUT_BLUR VS_Blur(float4 Position : POSITION, 
					   float2 TexCoord : TEXCOORD0,
					   uniform int nsamples,
					   uniform float2 direction
					   )
{
    VS_OUTPUT_BLUR OUT;// = (VS_OUTPUT_BLUR)0;
    OUT.Position = Position;
	//float2 texelSize = BlurWidth / WindowSize;
    /*float2 s = TexCoord - (texelSize * (nsamples-1) * 0.5 * direction);
    for(int i=0; i < nsamples; i++) {
    	OUT.TexCoord[i] = s + (texelSize * i * direction);
    }*/
    OUT.TexCoord0 = TexCoord;
    return OUT;
}

finalVSOutput FinalVertexPass(brightPassIn IN)
{
	finalVSOutput OUT;
	OUT.position = mul(float4(IN.position.xyz , 1.0), worldViewProj);
	OUT.texCoord = IN.texCoord;
	return OUT;
}

/// PIXEL SHADERS

float4 BrightPixelPass(brightPassVOut IN): COLOR
{
	float normalizationFactor = 1 / (1 - 0.5);

	float4 fullSample = tex2D(g_sourceSample, IN.texCoord);
	float3 sample = fullSample.bbb;
	float greyLevel = saturate(mul(sample, luminanceFilter));
	float3 desaturated = lerp(sample, greyLevel.rrr, 0.5);

	return float4((desaturated - 0.5) * normalizationFactor, 1);
}

half4 PS_Blur7x(VS_OUTPUT_BLUR IN,
			   uniform sampler2D tex,
			   uniform half weight[7]
			   ) : COLOR
{
    half4 c = 0;
    
   	c += tex2D(tex, float2(IN.TexCoord0.x - (3 * texelSize.x), IN.TexCoord0.y)) * 0.05;
   	c += tex2D(tex, float2(IN.TexCoord0.x - (2 * texelSize.x), IN.TexCoord0.y)) * 0.1;
   	c += tex2D(tex, float2(IN.TexCoord0.x - (1 * texelSize.x), IN.TexCoord0.y)) * 0.2;
   	c += tex2D(tex, IN.TexCoord0) * 0.3;
   	c += tex2D(tex, float2(IN.TexCoord0.x + (1 * texelSize.x), IN.TexCoord0.y)) * 0.2;
   	c += tex2D(tex, float2(IN.TexCoord0.x + (2 * texelSize.x), IN.TexCoord0.y)) * 0.1;
   	c += tex2D(tex, float2(IN.TexCoord0.x + (3 * texelSize.x), IN.TexCoord0.y)) * 0.05;
   	
    return c;
}

half4 PS_Blur7y(VS_OUTPUT_BLUR IN,
			   uniform sampler2D tex,
			   uniform half weight[7]
			   ) : COLOR
{
    half4 c = 0;

   	c += tex2D(tex, float2(IN.TexCoord0.x, IN.TexCoord0.y - (3 * texelSize.x))) * 0.05;
   	c += tex2D(tex, float2(IN.TexCoord0.x, IN.TexCoord0.y - (2 * texelSize.x))) * 0.1;
   	c += tex2D(tex, float2(IN.TexCoord0.x, IN.TexCoord0.y - (1 * texelSize.x))) * 0.2;
   	c += tex2D(tex, IN.TexCoord0) * 0.3;
   	c += tex2D(tex, float2(IN.TexCoord0.x, IN.TexCoord0.y + (1 * texelSize.x))) * 0.2;
   	c += tex2D(tex, float2(IN.TexCoord0.x, IN.TexCoord0.y + (2 * texelSize.x))) * 0.1;
   	c += tex2D(tex, float2(IN.TexCoord0.x, IN.TexCoord0.y + (3 * texelSize.x))) * 0.05;
   	
    return c;
}

float4 FinalPixelPass(finalVSOutput IN): COLOR
{
	return tex2D(HBlurSampler, IN.texCoord) + tex2D(g_sourceSample, IN.texCoord);
}

/// TECHNIQUES

technique std_BloomBrightPass
{
	pass p0
	{
		VertexShader = compile vs_1_1 BrightVertexPass();
		PixelShader = compile ps_2_0 BrightPixelPass();
	}
}

technique std_BlurPass
{
	pass p0
	{
		VertexShader = compile vs_2_0 VS_Blur(7, float2(1, 0));
		PixelShader  = compile ps_2_0 PS_Blur7x(BrightSourceSampler, weights7);
	}
	pass p1
	{
		VertexShader = compile vs_2_0 VS_Blur(7, float2(0, 1));
		PixelShader  = compile ps_2_0 PS_Blur7y(HBlurSampler, weights7);
	}
}

technique std_FinalPass
{
	pass p0
	{
		VertexShader = compile vs_1_1 FinalVertexPass();
		PixelShader = compile ps_2_0 FinalPixelPass();
	}
}