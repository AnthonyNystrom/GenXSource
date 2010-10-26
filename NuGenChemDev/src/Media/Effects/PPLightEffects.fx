float4x4 worldViewProj : WorldViewProjection;
float4x4 world   : World;
float4x4 worldInverseTranspose : WorldInverseTranspose;
float4x4 viewInverse : ViewInverse;
Texture diffuseTexture;

sampler DiffuseTexture = sampler_state {
	texture = <diffuseTexture>;
	magfilter = LINEAR;
	minfilter = LINEAR;
	mipfilter = LINEAR;
	AddressU = CLAMP;
	AddressV = CLAMP;
	AddressW  = CLAMP;
};

float4 lightDir : Direction
<
	string Object = "DirectionalLight";
    string Space = "World";
> = {1.0f, -1.0f, 1.0f, 0.0f};

float4 lightColor : Diffuse
<
    string UIName = "Diffuse Light Color";
    string Object = "DirectionalLight";
> = {1.0f, 1.0f, 1.0f, 1.0f};

float4 lightAmbient : Ambient
<
    string UIWidget = "Ambient Light Color";
    string Space = "material";
> = {0.0f, 0.0f, 0.0f, 1.0f};

float shininess : SpecularPower
<
    string UIWidget = "slider";
    float UIMin = 1.0;
    float UIMax = 128.0;
    float UIStep = 1.0;
    string UIName = "specular power";
> = 30.0;

struct viPNC0 {
	float3 position				: POSITION;
    float3 normal				: NORMAL;
    float4 diffColor			: COLOR0;
};

struct viPNCT0 {
	float3 position				: POSITION;
    float3 normal				: NORMAL;
    float4 diffColor			: COLOR0;
    float2 texCoord				: TEXCOORD0;
};

struct viPT {
	float4 position				: POSITION;
	float2 texCoord				: TEXCOORD0;
};

struct voPC0 {
	float4 hPosition		: POSITION;
	float4 pos				: TEXCOORD0;
    float4 diffAmbColor		: COLOR0;
    float4 diffColor		: COLOR1;
    float3 norm				: TEXCOORD1;
};

struct voPT {
	float4 hPosition		: POSITION;
	float2 texCoord			: TEXCOORD0;
};

struct voPCT0 {
	float4 hPosition		: POSITION;
	float2 texCoord			: TEXCOORD0;
	float4 pos				: TEXCOORD1;
    float4 diffAmbColor		: COLOR0;
    float4 diffColor		: COLOR1;
    float3 norm				: TEXCOORD2;
};

struct voPC02 {
	float4 position		: POSITION;
	float4 diffColor	: COLOR0;
};

voPT VS_STD_Texture(viPT IN) 
{
    voPT OUT;
    OUT.hPosition = mul(float4(IN.position.xyz , 1.0), worldViewProj);
    OUT.texCoord = IN.texCoord;
    return OUT;
}

voPC0 VS_STD_NoTextureNoSpec(viPNC0 IN) 
{
    voPC0 OUT;
    OUT.hPosition = OUT.pos = mul(float4(IN.position.xyz , 1.0), worldViewProj);

    float4 ambColor = IN.diffColor * lightAmbient;
    OUT.diffAmbColor = ambColor;
	OUT.norm = IN.normal;
	OUT.diffColor = IN.diffColor;

    return OUT;
}

voPCT0 VS_STD_NoTextureNoSpec(viPNCT0 IN) 
{
    voPCT0 OUT;
    OUT.hPosition = OUT.pos = mul(float4(IN.position.xyz , 1.0), worldViewProj);

    float4 ambColor = IN.diffColor * lightAmbient;
    OUT.diffAmbColor = ambColor;
	OUT.norm = IN.normal;
	OUT.diffColor = IN.diffColor;
	OUT.texCoord = IN.texCoord;

    return OUT;
}

voPC02 VS_STD_NoTextureNoSpecLine(viPNC0 IN) 
{
    voPC02 OUT;
    OUT.position = mul(float4(IN.position.xyz , 1.0), worldViewProj);
    OUT.diffColor =  IN.diffColor;

    return OUT;
}

float4 PS_NoTextureNoSpec(voPC0 IN): COLOR
{
	float3 P = IN.pos;
	float3 N = normalize(IN.norm);
	
	float3 L = normalize(lightDir - P);
	float diffuseLight = max(dot(N,L), 0);
	float3 diff = IN.diffColor * lightColor * diffuseLight;
	
	return float4(diff + IN.diffAmbColor, 1.0);
}

float4 PS_NoTextureSpec(voPC0 IN): COLOR
{
	float3 P = IN.pos;
	float3 N = normalize(IN.norm);
	
	float3 L = normalize(lightDir - P);
	float diffuseLight = max(dot(N,L), 0);
	float3 diff = IN.diffColor * lightColor * diffuseLight;
	
	float3 V = normalize(viewInverse[3].xyz - P);
	float3 H = normalize(L + V);
	float specularLight = pow(max(dot(N, H), 0), shininess);
	
	if (diffuseLight <= 0)
		specularLight = 0;
	float3 spec = lightColor * specularLight;
	
	return float4(diff + IN.diffAmbColor + spec, 1.0);
}

float4 PS_TextureSpec(voPCT0 IN): COLOR
{
	float4 tex = tex2D(DiffuseTexture, IN.texCoord);

	float3 P = IN.pos;
	float3 N = normalize(IN.norm);
	
	float3 L = normalize(lightDir - P);
	float diffuseLight = max(dot(N,L), 0);
	float3 diff = (IN.diffColor * tex.r) * lightColor * diffuseLight;
	
	float3 V = normalize(viewInverse[3].xyz - P);
	float3 H = normalize(L + V);
	float specularLight = pow(max(dot(N, H), 0), shininess);
	
	if (diffuseLight <= 0)
		specularLight = 0;
	float3 spec = lightColor * specularLight;
	
	return float4(diff + IN.diffAmbColor + spec, 1.0);
}

float4 PS_AlphaTexture(voPT IN): COLOR
{
	float4 tex = tex2D(DiffuseTexture, IN.texCoord);
	return float4(tex.rgb, 1);
}

technique std_basicDirLightNoSpecNoTexture
{
	pass p0
	{
		VertexShader = compile vs_1_1 VS_STD_NoTextureNoSpec();
		PixelShader  = compile ps_2_0 PS_NoTextureNoSpec();
	}
}

technique std_basicDirLightSpecNoTexture
{
	pass p0
	{
		VertexShader = compile vs_1_1 VS_STD_NoTextureNoSpec();
		PixelShader  = compile ps_2_0 PS_NoTextureSpec();
	}
}

technique std_basicDirLightNoSpecNoTextureLine
{
	pass p0
	{
		VertexShader = compile vs_1_1 VS_STD_NoTextureNoSpecLine();
	}
}

technique std_basicDirLightSpecNoTextureLine
{
	pass p0
	{
		VertexShader = compile vs_1_1 VS_STD_NoTextureNoSpecLine();
	}
}

technique std_basicDirLightSpecTexture
{
	pass p0
	{
		VertexShader = compile vs_1_1 VS_STD_NoTextureNoSpec();
		PixelShader  = compile ps_2_0 PS_TextureSpec();
	}
}

technique std_basicDirLightSpecTextureLine
{
	pass p0
	{
		VertexShader = compile vs_1_1 VS_STD_NoTextureNoSpecLine();
	}
}

technique std_basicAlphaTexture
{
	pass p0
	{
		VertexShader = compile vs_1_1 VS_STD_Texture();
		PixelShader  = compile ps_2_0 PS_AlphaTexture();
	}
}