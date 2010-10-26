string description = "Basic Vertex Lighting";

//------------------------------------
float4x4 worldViewProj : WorldViewProjection;
float4x4 world   : World;
float4x4 worldInverseTranspose : WorldInverseTranspose;
float4x4 viewInverse : ViewInverse;

texture diffuseTexture : Diffuse
<
	string ResourceName = "Volume3.dds";
>;

texture diffuseTexture2 : Diffuse
<
	string ResourceName = "Volume1.dds";
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

float noiseScale
<
> = 1.0;


//------------------------------------
struct vertexInput {
    float3 position				: POSITION;
    float3 normal				: NORMAL;
};

struct vertexOutput {
    float4 hPosition		: POSITION;
    float4 diffAmbColor		: COLOR0;
    float4 specCol			: COLOR1;
};

struct vertexOutput2 {
    float4 hPosition		: POSITION;
    float4 diffAmbColor		: COLOR0;
};

struct vertexOutput3 {
    float4 hPosition		: POSITION;
    float4 diffAmbColor		: COLOR0;
    float3 wPosition		: TEXCOORD0;
};


//------------------------------------
vertexOutput VS_TransAndLightAmbDifSpec(vertexInput IN) 
{
    vertexOutput OUT;
    OUT.hPosition = mul( float4(IN.position.xyz , 1.0) , worldViewProj);

	//calculate our vectors N, E, L, and H
	float3 worldEyePos = viewInverse[3].xyz;
    float3 worldVertPos = mul(IN.position, world).xyz;
	float4 N = mul(IN.normal, worldInverseTranspose); //normal vector
    float3 E = normalize(worldEyePos - worldVertPos); //eye vector
    float3 L = normalize( -lightDir.xyz); //light vector
    float3 H = normalize(E + L); //half angle vector

	//calculate the diffuse and specular contributions
    float  diff = max(0 , dot(N,L));
    float  spec = pow( max(0 , dot(N,H) ) , shininess );
    if( diff <= 0 )
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

vertexOutput2 VS_TransAndLightAmbDif(vertexInput IN)
{
	vertexOutput2 OUT;
    OUT.hPosition = mul( float4(IN.position.xyz , 1.0), worldViewProj);

	//calculate our vectors N, E, L, and H
	float3 worldEyePos = viewInverse[3].xyz;
    float3 worldVertPos = mul(IN.position, world).xyz;
	float4 N = mul(IN.normal, worldInverseTranspose); //normal vector
    float3 E = normalize(worldEyePos - worldVertPos); //eye vector
    float3 L = normalize( -lightDir.xyz); //light vector
    float3 H = normalize(E + L); //half angle vector

	//calculate the diffuse and specular contributions
    float  diff = max(0 , dot(N,L));

	//output diffuse
    float4 ambColor = materialDiffuse * lightAmbient;
    float4 diffColor = materialDiffuse * diff * lightColor ;
    OUT.diffAmbColor = diffColor + ambColor;

    return OUT;
}

vertexOutput3 VS_TransAndLightAmbDifNoise(vertexInput IN)
{
	vertexOutput3 OUT;
    OUT.hPosition = mul( float4(IN.position.xyz , 1.0), worldViewProj);

	//calculate our vectors N, E, L, and H
	float3 worldEyePos = viewInverse[3].xyz;
    float3 worldVertPos = mul(IN.position, world).xyz;
	float4 N = mul(IN.normal, worldInverseTranspose); //normal vector
    float3 E = normalize(worldEyePos - worldVertPos); //eye vector
    float3 L = normalize( -lightDir.xyz); //light vector
    float3 H = normalize(E + L); //half angle vector

	//calculate the diffuse and specular contributions
    float diff = max(0 , dot(N,L));

	//output diffuse
    float4 ambColor = materialDiffuse * lightAmbient;
    float4 diffColor = materialDiffuse * diff * lightColor ;
    OUT.diffAmbColor = diffColor + ambColor;
    
    OUT.wPosition = IN.position * noiseScale;//mul(float4(IN.position * noiseScale, 1.0), world);

    return OUT;
}

sampler TextureSampler = sampler_state 
{
    texture = <diffuseTexture>;
    AddressU  = MIRROR;        
    AddressV  = MIRROR;
    AddressW  = MIRROR;
    MIPFILTER = LINEAR;
    MINFILTER = LINEAR;
    MAGFILTER = LINEAR;
};

//-----------------------------------
float4 PS_Textured(vertexOutput IN): COLOR
{
  return IN.diffAmbColor + IN.specCol;
}

float4 PS_Noise(vertexOutput3 IN) : COLOR
{
	float4 noiseLookup = tex3D(TextureSampler, IN.wPosition);
	return IN.diffAmbColor * noiseLookup.x;
}


//-----------------------------------
technique vertexLitAmbientDiffuseSpec
{
    pass p0 
    {
		VertexShader = compile vs_1_1 VS_TransAndLightAmbDifSpec();
		PixelShader  = compile ps_1_1 PS_Textured();
    }
}

technique vertexLitAmbientDiffuse
{
	pass p0
	{
		VertexShader = compile vs_1_1 VS_TransAndLightAmbDif();
	}
}

technique vertexLitDiffuseWithNoise
{
	pass p0
	{
		VertexShader = compile vs_1_1 VS_TransAndLightAmbDifNoise();
		PixelShader  = compile ps_2_0 PS_Noise();
	}
}