string description = "Basic Vertex Lighting with a Texture";

//------------------------------------
float4x4 worldViewProj : WorldViewProjection;
float4x4 world   : World;
float4x4 worldInverseTranspose : WorldInverseTranspose;
float4x4 viewInverse : ViewInverse;

texture diffuseTexture : Diffuse
<
	string ResourceName = "default_color.dds";
>;

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

float4 materialDiffuse : Diffuse
<
    string UIWidget = "Surface Color";
    string Space = "material";
> = {1.0f, 1.0f, 1.0f, 1.0f};

float4 materialSpecular : Specular
<
	string UIWidget = "Surface Specular";
	string Space = "material";
> = {1.0f, 1.0f, 1.0f, 1.0f};

float shininess : SpecularPower
<
    string UIWidget = "slider";
    float UIMin = 1.0;
    float UIMax = 128.0;
    float UIStep = 1.0;
    string UIName = "specular power";
> = 30.0;


//------------------------------------
struct viPNC0T0 {
    float3 position				: POSITION;
    float3 normal				: NORMAL;
    float4 texCoordDiffuse		: TEXCOORD0;
};

struct viPNC0 {
	float3 position				: POSITION;
    float3 normal				: NORMAL;
    float4 diffColor			: COLOR0;
};

struct voPT0C0C1 {
    float4 hPosition		: POSITION;
    float4 texCoordDiffuse	: TEXCOORD0;
    float4 diffAmbColor		: COLOR0;
    float4 specCol			: COLOR1;
};

struct voPT0C0 {
    float4 hPosition		: POSITION;
    float4 texCoordDiffuse	: TEXCOORD0;
    float4 diffAmbColor		: COLOR0;
};

struct voPC0 {
	float4 hPosition		: POSITION;
    float4 diffAmbColor		: COLOR0;
};


//------------------------------------
voPT0C0C1 VS_STD_TextureSpec(viPNC0T0 IN) 
{
    voPT0C0C1 OUT;
    OUT.hPosition = mul( float4(IN.position.xyz , 1.0) , worldViewProj);
    OUT.texCoordDiffuse = IN.texCoordDiffuse;

	//calculate our vectors N, E, L, and H
	float3 worldEyePos = viewInverse[3].xyz;
    float3 worldVertPos = mul(IN.position, world).xyz;
	float4 N = mul(IN.normal, worldInverseTranspose); //normal vector
    float3 E = normalize(worldEyePos - worldVertPos); //eye vector
    float3 L = normalize( -lightDir.xyz); //light vector
    float3 H = normalize(E + L); //half angle vector

	//calculate the diffuse and specular contributions
    float diff = max(0 , dot(N,L));
    float spec = pow( max(0 , dot(N,H) ) , shininess );
    if(diff <= 0 )
    {
        spec = 0;
    }

	//output diffuse
    float4 ambColor = materialDiffuse * lightAmbient;
    float4 diffColor = materialDiffuse * diff * lightColor ;
    OUT.diffAmbColor = diffColor + ambColor;

	//output specular
    float4 specColor = materialSpecular * lightColor * spec;
    OUT.specCol = specColor;

    return OUT;
}

voPT0C0 VS_STD_TextureNoSpec(viPNC0T0 IN) 
{
    voPT0C0 OUT;
    OUT.hPosition = mul(float4(IN.position.xyz , 1.0), worldViewProj);
    OUT.texCoordDiffuse = IN.texCoordDiffuse;

	//calculate our vectors N, E, L, and H
	float4 N = mul(IN.normal, worldInverseTranspose); //normal vector
    float3 L = normalize( -lightDir.xyz); //light vector

	//calculate the diffuse and specular contributions
    float diff = max(0 , dot(N,L));

	//output diffuse
    float4 ambColor = materialDiffuse * lightAmbient;
    float4 diffColor = materialDiffuse * diff * lightColor ;
    OUT.diffAmbColor = diffColor + ambColor;

    return OUT;
}

voPC0 VS_STD_NoTextureNoSpec(viPNC0 IN) 
{
    voPC0 OUT;
    OUT.hPosition = mul(float4(IN.position.xyz , 1.0), worldViewProj);

	//calculate our vectors N, E, L, and H
	float4 N = mul(IN.normal, worldInverseTranspose); //normal vector
    float3 L = normalize( -lightDir.xyz); //light vector

	//calculate the diffuse and specular contributions
    float diff = max(0, dot(N,L));

	//output diffuse
    float4 ambColor = IN.diffColor * lightAmbient;
    float4 diffColor = IN.diffColor * diff * lightColor ;
    OUT.diffAmbColor = diffColor + ambColor;

    return OUT;
}

voPC0 VS_STD_NoTextureSpec(viPNC0 IN) 
{
    voPC0 OUT;
    OUT.hPosition = mul(float4(IN.position.xyz , 1.0) , worldViewProj);

	//calculate our vectors N, E, L, and H
	float3 worldEyePos = viewInverse[3].xyz;
    float3 worldVertPos = mul(IN.position, world).xyz;
	float4 N = mul(IN.normal, worldInverseTranspose); //normal vector
    float3 E = normalize(worldEyePos - worldVertPos); //eye vector
    float3 L = normalize(-lightDir.xyz); //light vector
    float3 H = normalize(E + L); //half angle vector

	//calculate the diffuse and specular contributions
    float diff = max(0 , dot(N,L));
    float spec = pow(max(0 , dot(N,H) ), shininess );
    if( diff <= 0 )
    {
        spec = 0;
    }

	//output diffuse
    float4 ambColor = IN.diffColor * lightAmbient;
    float4 diffColor = IN.diffColor * diff * lightColor ;

	//output specular
    float4 specColor = IN.diffColor * lightColor * spec;
    OUT.diffAmbColor = diffColor + ambColor + specColor;

    return OUT;
}


//------------------------------------
sampler TextureSampler = sampler_state 
{
    texture = <diffuseTexture>;
    AddressU  = CLAMP;        
    AddressV  = CLAMP;
    AddressW  = CLAMP;
    MIPFILTER = LINEAR;
    MINFILTER = LINEAR;
    MAGFILTER = LINEAR;
};


//-----------------------------------
float4 PS_TextureSpec(voPT0C0C1 IN): COLOR
{
  float4 LuminanceConv = { 0.3f, 0.59f, 0.11f, 1.0f };
  float4 diffuseTexture = tex2D( TextureSampler, IN.texCoordDiffuse );
  float4 grayScale = dot(LuminanceConv, IN.diffAmbColor * diffuseTexture + IN.specCol);
  return grayScale;
}

float4 PS_TextureNoSpec(voPT0C0 IN): COLOR
{
  float4 LuminanceConv = { 0.3f, 0.59f, 0.11f, 1.0f };
  float4 diffuseTexture = tex2D( TextureSampler, IN.texCoordDiffuse );
  float4 grayScale = dot(LuminanceConv, IN.diffAmbColor * diffuseTexture);
  return grayScale;
}

float4 PS_NoTextureNoSpec(voPC0 IN): COLOR
{
  float4 LuminanceConv = { 0.3f, 0.59f, 0.11f, 1.0f };
  float4 grayScale = dot(LuminanceConv, IN.diffAmbColor);
  return grayScale;
}

float4 PS_NoTextureSpec(voPC0 IN): COLOR
{
  float4 LuminanceConv = { 0.3f, 0.59f, 0.11f, 1.0f };
  float4 grayScale = dot(LuminanceConv, IN.diffAmbColor);
  return grayScale;
}

//-----------------------------------
technique std_basicDirLightSpecTexture
{
    pass p0 
    {
		VertexShader = compile vs_1_1 VS_STD_TextureSpec();
		PixelShader  = compile ps_1_1 PS_TextureSpec();
    }
}

technique std_basicDirLightNoSpecTexture
{
	pass p0 
    {
		VertexShader = compile vs_1_1 VS_STD_TextureNoSpec();
		PixelShader  = compile ps_1_1 PS_TextureNoSpec();
    }
}

technique std_basicDirLightNoSpecNoTexture
{
	pass p0
	{
		VertexShader = compile vs_1_1 VS_STD_NoTextureNoSpec();
		PixelShader  = compile ps_1_1 PS_NoTextureNoSpec();
	}
}

technique std_basicDirLightSpecNoTexture
{
	pass p0
	{
		VertexShader = compile vs_1_1 VS_STD_NoTextureSpec();
		PixelShader  = compile ps_1_1 PS_NoTextureSpec();
	}
}