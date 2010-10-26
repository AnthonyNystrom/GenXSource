float4x4 WorldViewProj : WorldViewProjection;
float4x4 WorldInverseTranspose : WorldInverseTranspose;

texture DiffuseTexture : Diffuse
<
	string ResourceName = "default_color.dds";
>;

texture OverlayTexture : Diffuse
<
	string ResourceName = "default_color.dds";
>;

struct DirLight
{
	float3 Direction;
	float3 Clr;
};

DirLight Light = 
{
	0.0f, -1.0f, 0.0f,
	1.0f, 1.0f, 1.0f
};

float3 Ambient = float3(0.0f, 0.0f, 0.0f);


//------------------------------------
struct vInTextured {
    float3 position				: POSITION;
    float4 texCoord				: TEXCOORD0;
};

struct vInTexturedLit {
	float3 position				: POSITION;
	float4 texCoord				: TEXCOORD0;
	float4 normal				: NORMAL;
};

struct vOutTextured {
    float4 hPosition		: POSITION;
    float4 texCoord			: TEXCOORD0;
};

struct vOutTexturedLit {
    float4 hPosition		: POSITION;
    float4 texCoord			: TEXCOORD0;
    float4 color			: COLOR;
};


//------------------------------------
vOutTextured VS_TransformAndTexture(vInTextured IN) 
{
    vOutTextured OUT;
    OUT.hPosition = mul(float4(IN.position.xyz , 1.0) , WorldViewProj);
    OUT.texCoord = IN.texCoord;
    return OUT;
}

vOutTexturedLit VS_TransformAndTextureLit(vInTexturedLit IN)
{
    vOutTexturedLit OUT;
    OUT.hPosition = mul(float4(IN.position.xyz , 1.0) , WorldViewProj);
    OUT.texCoord = IN.texCoord;
    
    // calc lighting
	  float4 N = mul(IN.normal, WorldInverseTranspose);
    float3 L = normalize(-Light.Direction);

	  //calculate the diffuse contribution
    float diff = max(0, dot(N,L));

    float4 diffColor = diff * float4(Light.Clr, 1.0f);
    OUT.color = diffColor + float4(Ambient, 1);
    
    return OUT;
}

//------------------------------------
sampler TextureSampler = sampler_state 
{
    texture = <DiffuseTexture>;
    AddressU  = CLAMP;        
    AddressV  = CLAMP;
    AddressW  = CLAMP;
    MIPFILTER = LINEAR;
    MINFILTER = LINEAR;
    MAGFILTER = LINEAR;
};

sampler OverlayTextureSampler = sampler_state 
{
    texture = <OverlayTexture>;
    AddressU  = CLAMP;        
    AddressV  = CLAMP;
    AddressW  = CLAMP;
    MIPFILTER = LINEAR;
    MINFILTER = LINEAR;
    MAGFILTER = LINEAR;
};


//-----------------------------------
float4 PS_MTextured(vOutTextured IN): COLOR
{
	float4 diffuseTexture = tex2D(TextureSampler, IN.texCoord);
	float4 overlayTexture = tex2D(OverlayTextureSampler, IN.texCoord);
	return float4((diffuseTexture * (1 - overlayTexture.a)) + (overlayTexture.rgb * overlayTexture.a), 1);
}

float4 PS_Textured(vOutTextured IN): COLOR
{
	return tex2D(TextureSampler, IN.texCoord);
}

float4 PS_TexturedLit(vOutTexturedLit IN): COLOR
{
	float4 diffuseTexture = tex2D(TextureSampler, IN.texCoord);
	return IN.color * diffuseTexture;
}

float4 PS_MTexturedLit(vOutTexturedLit IN): COLOR
{
	float4 diffuseTexture = tex2D(TextureSampler, IN.texCoord);
	float4 overlayTexture = tex2D(OverlayTextureSampler, IN.texCoord);
	return IN.color * float4((diffuseTexture * (1 - overlayTexture.a)) + (overlayTexture.rgb * overlayTexture.a), 1);
}

//-----------------------------------
technique Textured
{
    pass p0
    {
		VertexShader = compile vs_1_1 VS_TransformAndTexture();
		PixelShader  = compile ps_2_0 PS_Textured();
    }
}

technique MultiTextured
{
    pass p0
    {
		VertexShader = compile vs_1_1 VS_TransformAndTexture();
		PixelShader  = compile ps_2_0 PS_MTextured();
    }
}

technique LitTextured
{
	pass p0
    {
		VertexShader = compile vs_1_1 VS_TransformAndTextureLit();
		PixelShader  = compile ps_2_0 PS_TexturedLit();
    }
}

technique LitMTextured
{
	pass p0
    {
		VertexShader = compile vs_1_1 VS_TransformAndTextureLit();
		PixelShader  = compile ps_2_0 PS_MTexturedLit();
    }
}