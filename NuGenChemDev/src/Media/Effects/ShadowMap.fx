float4x4 xWorldViewProjection;
float4x4 xLightWorldViewProjection;
float4x4 xWorld;
float4 xLightPos;
float xLightPower = 1;
float xMaxDepth;
bool xUseBrownInsteadOfTextures;
float4 xLightDir;
float g_fCosTheta;

#define SMAP_SIZE 1024


#define SHADOW_EPSILON 0.02f;//0005f

Texture xColoredTexture;
Texture xShadowMap;

float4x4 viewInverse : ViewInverse;

float4 lightAmbient : Ambient
<
    string UIWidget = "Ambient Light Color";
    string Space = "material";
> = {0.0f, 0.0f, 0.0f, 1.0f};

float4 lightColor : Diffuse
<
    string UIName = "Diffuse Light Color";
    string Object = "DirectionalLight";
> = {1.0f, 1.0f, 1.0f, 1.0f};

float shininess : SpecularPower
<
    string UIWidget = "slider";
    float UIMin = 1.0;
    float UIMax = 128.0;
    float UIStep = 1.0;
    string UIName = "specular power";
> = 30.0;

/// SAMPLERS

sampler2D ShadowMapSampler = sampler_state {
	texture = <xShadowMap>;
	magfilter = Point;
	minfilter = Point;
	mipfilter = Point;
	AddressU = clamp;
	AddressV = clamp;
};

/// STRUCTURES

struct SMapVertexToPixel
{
    float4 Position     : POSITION;
    float3 Position2D    : TEXCOORD0;
};

struct SMapPixelToFrame
{
    float4 Color : COLOR0;
};

struct PixelToFrame
{
    float4 Color	: COLOR0;
};

struct SSceneVertexToPixel
{
    float4 Position             : POSITION;
    float4 ShadowMapSamplingPos : TEXCOORD0;
    float4 RealDistance            : TEXCOORD1;
    float3 Normal                : TEXCOORD2;
    float3 Position3D            : TEXCOORD3;
    float4 diffAmbColor		: COLOR0;
    float4 diffColor		: COLOR1;
};

struct SScenePixelToFrame
{
	float4 Color : COLOR0;
};

/// VERTEX SHADERS

SMapVertexToPixel ShadowMapVertexShader( float4 inPos : POSITION)
{
    SMapVertexToPixel Output = (SMapVertexToPixel)0;

    Output.Position = mul(inPos, xLightWorldViewProjection);
    Output.Position2D = Output.Position;

    return Output;
}

SSceneVertexToPixel ShadowedSceneVertexShader(float4 inPos : POSITION,
										      float3 inNormal : NORMAL,
										      float4 diffColor : COLOR0)
{
    SSceneVertexToPixel Output = (SSceneVertexToPixel)0;

    Output.Position = mul(inPos, xWorldViewProjection);
    Output.ShadowMapSamplingPos = mul(inPos, xLightWorldViewProjection);


    Output.RealDistance = Output.ShadowMapSamplingPos.z / xMaxDepth;
    Output.Normal = normalize(mul(inNormal, (float3x3)xWorld));
    Output.Position3D = mul(inPos, xWorld);
     
    float4 ambColor = diffColor * lightAmbient;
    Output.diffAmbColor = ambColor;
	Output.diffColor = diffColor;

    return Output;
}

/// PIXEL SHADERS

SMapPixelToFrame ShadowMapPixelShader(SMapVertexToPixel PSIn)
{
    SMapPixelToFrame Output = (SMapPixelToFrame)0;

    Output.Color = PSIn.Position2D.z / xMaxDepth;

    return Output;
}

float DotProduct(float4 LightPos, float3 Pos3D, float3 Normal)
{
	float3 LightDir = normalize(LightPos - Pos3D);
    return dot(LightDir, Normal);
}
 
SScenePixelToFrame ShadowedScenePixelShader(SSceneVertexToPixel PSIn)
{
	SScenePixelToFrame Output = (SScenePixelToFrame)0;
	
	float3 vLight = normalize(float3(PSIn.Position3D - xLightPos));

    // Compute diffuse from the light
    //if(dot(vLight, xLightDir) > g_fCosTheta) // Light must face the pixel (within Theta)
    //{
    	float2 ProjectedTexCoords;
    	ProjectedTexCoords[0] = PSIn.ShadowMapSamplingPos.x / PSIn.ShadowMapSamplingPos.w / 2.0f + 0.5f;
    	ProjectedTexCoords[1] = -PSIn.ShadowMapSamplingPos.y / PSIn.ShadowMapSamplingPos.w / 2.0f + 0.5f;
    	
    	float texelSize = 1.0f / SMAP_SIZE;
	
		//Output.Color = float4(0.2,0.2,0.2,1);
    	if ((saturate(ProjectedTexCoords.x) == ProjectedTexCoords.x) && (saturate(ProjectedTexCoords.y) == ProjectedTexCoords.y))
    	{
        	//float StoredDepthInShadowMap = tex2D(ShadowMapSampler, ProjectedTexCoords).x;
        	float2 lerps = frac(SMAP_SIZE * ProjectedTexCoords);
        	float sourcevals[4];
        	sourcevals[0] = (PSIn.RealDistance.x <= tex2D(ShadowMapSampler, ProjectedTexCoords).x + 0.02f) ? 0 : 1;
        	sourcevals[1] = (PSIn.RealDistance.x <= tex2D(ShadowMapSampler, ProjectedTexCoords + float2(1.0/SMAP_SIZE, 0)).x + 0.02f) ? 0.0f: 1.0f;  
        	sourcevals[2] = (PSIn.RealDistance.x <= tex2D(ShadowMapSampler, ProjectedTexCoords + float2(0, 1.0/SMAP_SIZE)).x + 0.02f) ? 0.0f: 1.0f;  
        	sourcevals[3] = (PSIn.RealDistance.x <= tex2D(ShadowMapSampler, ProjectedTexCoords + float2(1.0/SMAP_SIZE, 1.0/SMAP_SIZE)).x + 0.02f) ? 0.0f: 1.0f;  
	
			float LightAmount = 1-(  lerp( lerp( sourcevals[0], sourcevals[1], lerps.x ),
                                  lerp( sourcevals[2], sourcevals[3], lerps.x ),
                                  lerps.y ));
        	
        	//Output.Color = float4(0, 0, 0, 1);
        	//if (LightAmount > 0)//(PSIn.RealDistance.x - 1.0f / 50.0f) <= StoredDepthInShadowMap)
			//{
        		/*float DiffuseLightingFactor = DotProduct(xLightPos, PSIn.Position3D, PSIn.Normal);
            	float4 ColorComponent = PSIn.diffColor;
            	Output.Color = ColorComponent * DiffuseLightingFactor * xLightPower;*/
            	
            	float3 P = PSIn.Position3D;
				float3 N = normalize(PSIn.Normal);
				
				float3 L = normalize(xLightPos - PSIn.Position3D);
				float diffuseLight = max(dot(L,N), 0);
				float3 diff = PSIn.diffColor * lightColor * diffuseLight;
		
				/*float3 V = normalize(viewInverse[3].xyz - PSIn.Position3D);
				float3 H = normalize(L + V);
				float specularLight = pow(max(dot(N, H), 0), shininess);
		
				if (diffuseLight <= 0)
					specularLight = 0;
				float3 spec = lightColor * specularLight;*/
		
				Output.Color = float4(diff + PSIn.diffAmbColor /*+ spec*/, 1.0) * LightAmount;
    		//}
		}
	//}
 
    return Output;
}

SScenePixelToFrame ShadowedScenePixelShaderLine(SSceneVertexToPixel PSIn)
{
	SScenePixelToFrame Output = (SScenePixelToFrame)0;

    float2 ProjectedTexCoords;
    ProjectedTexCoords[0] = PSIn.ShadowMapSamplingPos.x / PSIn.ShadowMapSamplingPos.w / 2.0f + 0.5f;
    ProjectedTexCoords[1] = -PSIn.ShadowMapSamplingPos.y / PSIn.ShadowMapSamplingPos.w / 2.0f + 0.5f;

    if ((saturate(ProjectedTexCoords.x) == ProjectedTexCoords.x) && (saturate(ProjectedTexCoords.y) == ProjectedTexCoords.y))
    {
        float StoredDepthInShadowMap = tex2D(ShadowMapSampler, ProjectedTexCoords).x;
        Output.Color = float4(0, 0, 0, 1);
        if ((PSIn.RealDistance.x - 1.0f/200.0f) <= StoredDepthInShadowMap)
		{
			Output.Color = PSIn.diffColor;
    	}
	}
 
    return Output;
}
 
/// TECHNIQUES
 
technique ShadowMap
{
    pass Pass0
	{
    	VertexShader = compile vs_2_0 ShadowMapVertexShader();
    	PixelShader = compile ps_2_0 ShadowMapPixelShader();
    }
}

technique std_basicDirLightSpecNoTexture
{
	pass p0
	{
		VertexShader = compile vs_2_0 ShadowedSceneVertexShader();
        PixelShader = compile ps_2_0 ShadowedScenePixelShader();
	}
}

technique std_basicDirLightSpecNoTextureLine
{
	pass p0
	{
		VertexShader = compile vs_2_0 ShadowedSceneVertexShader();
        PixelShader = compile ps_2_0 ShadowedScenePixelShaderLine();
	}
}