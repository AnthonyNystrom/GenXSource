string description = "Basic Vertex Lighting with a Texture";

//------------------------------------
float4x4 worldViewProj : WorldViewProjection;
float4x4 world   : World;
float4x4 worldInverseTranspose : WorldInverseTranspose;
float4x4 viewInverse : ViewInverse;


//------------------------------------
struct vertexInput {
    float3 position				: POSITION;
    float3 normal				: NORMAL;
    float3 diffuse				: COLOR0;
};

struct vertexOutput {
    float4 hPosition		: POSITION;
    float4 diffAmbColor		: COLOR0;
};


//------------------------------------
vertexOutput VS_TransformAndTexture(vertexInput IN) 
{
    vertexOutput OUT;
    OUT.hPosition = mul( float4(IN.position.xyz , 1.0) , worldViewProj);

	float3 worldEyePos = viewInverse[3].xyz;
    float3 worldVertPos = mul(IN.position, world).xyz;
	float4 N = mul(IN.normal, worldInverseTranspose); //normal vector
    float3 E = normalize(worldEyePos - worldVertPos); //eye vector

    OUT.diffAmbColor = float4(IN.diffuse, 1);
    OUT.diffAmbColor.w = min(0.5, dot(E,N));

    return OUT;
}

struct PPOut {
	float4 hPosition		: POSITION;
	float3 diffuse			: COLOR;
    float3 normal		: TEXCOORD0;
    float3 worldVertPos : TEXCOORD1;
};

PPOut VS_OutlinePP(vertexInput IN)
{
	PPOut OUT;
	
	OUT.hPosition = mul(float4(IN.position.xyz , 1.0), worldViewProj);
	
	float3 worldEyePos = viewInverse[3].xyz;
    float3 worldVertPos = mul(IN.position, world).xyz;
	OUT.normal = /*mul(*/IN.normal;/*, worldInverseTranspose); //normal vector*/
	OUT.worldVertPos = worldVertPos;
	OUT.diffuse = IN.diffuse;
	
	return OUT;
}

//-----------------------------------
float4 PS_Textured( vertexOutput IN): COLOR
{
	float4 clr = IN.diffAmbColor;
	if (IN.diffAmbColor.w < 0.3)
  		clr = float4(1, 0, 0, 1);
  	else if (IN.diffAmbColor.w < 0.45)
  		clr = float4(1, 1, 0.2, 1);
  	return clr;
}

float4 PS_OutlinePP(PPOut IN) : COLOR
{
	float3 normal = mul(IN.normal, worldInverseTranspose);

	float3 worldEyePos = viewInverse[3].xyz;
	float3 E = normalize(worldEyePos - IN.worldVertPos); //eye vector

    float4 diffAmbColor = float4(IN.diffuse, 1);
    diffAmbColor.w = min(0.5, dot(E,normal));

	float4 clr = diffAmbColor;
	if (diffAmbColor.w < 0.3)
  		clr = float4(1, 0, 0, 1);
  	else if (diffAmbColor.w < 0.45)
  		clr = float4(1, 1, 0.2, 1);
  	return clr;
}


//-----------------------------------
technique outlinePerVertex
{
    pass p0 
    {
		VertexShader = compile vs_1_1 VS_TransformAndTexture();
		PixelShader  = compile ps_2_0 PS_Textured();
    }
}

technique outlinePerPixel
{
	pass p0
	{
		VertexShader = compile vs_1_1 VS_OutlinePP();
		PixelShader  = compile ps_2_0 PS_OutlinePP();
	}
}