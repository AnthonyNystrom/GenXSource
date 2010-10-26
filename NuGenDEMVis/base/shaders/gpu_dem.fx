/*

% Renders a plane mesh into a DEM using a texture lookup

date: 10-01-2007

*/

float4x4 WorldViewProj : WorldViewProjection;
int dem_level;
float quarterSample = 0.031111111111111111111111111111111;//0.03125;
float shift = 0;
float scale = 1;
int axis = 0;

float diffuseTexShift = -0.03125;
float diffuseTexScale = 1.066666666666666666666666666667;

texture DiffuseTexture : Diffuse;
texture HeightTexture : Diffuse;
texture NextLevelHeightTexture : Diffuse;
texture NormalMapTexture : Diffuse;

sampler DiffuseSampler = sampler_state 
{
    texture = <DiffuseTexture>;
    AddressU  = CLAMP;
    AddressV  = CLAMP;
    AddressW  = CLAMP;
    MIPFILTER = NONE;
    MINFILTER = NONE;
    MAGFILTER = NONE;
};

sampler HeightSampler = sampler_state 
{
    texture = <HeightTexture>;
    AddressU  = CLAMP;
    AddressV  = CLAMP;
    AddressW  = CLAMP;
    MIPFILTER = LINEAR;
    MINFILTER = LINEAR;
    MAGFILTER = LINEAR;
};

sampler NextLevelHeightSampler = sampler_state 
{
    texture = <NextLevelHeightTexture>;
    AddressU  = CLAMP;
    AddressV  = CLAMP;
    AddressW  = CLAMP;
    MIPFILTER = NONE;
    MINFILTER = NONE;
    MAGFILTER = NONE;
};

sampler NormalMapSampler = sampler_state 
{
    texture = <NormalMapTexture>;
    AddressU  = CLAMP;
    AddressV  = CLAMP;
    AddressW  = CLAMP;
    MIPFILTER = NONE;
    MINFILTER = NONE;
    MAGFILTER = NONE;
};

struct vInTextured {
    float3 position				: POSITION;
    float4 texCoord				: TEXCOORD0;
};

struct vInTextured_Patch {
    float3 position				: POSITION;
    float4 texCoord				: TEXCOORD0;
    float4 isJoin         : COLOR;
};


struct vOutTextured {
    float4 hPosition		: POSITION;
    float4 texCoord			: TEXCOORD0;
};

struct vOutTextured_Patch {
    float4 hPosition		: POSITION;
    float4 texCoord			: TEXCOORD0;
    float4 color : COLOR;
};

struct vOutTextured_Patch_Int {
    float4 hPosition		: POSITION;
    float4 texCoord			: TEXCOORD0;
    float4 color : COLOR;
};


vOutTextured mainVS(vInTextured IN)
{
    vOutTextured OUT;
    float3 heightClr = tex2Dlod(DiffuseSampler, IN.texCoord).rgb;
    float height = (heightClr.r + heightClr.g + heightClr.b) / 6;
    OUT.hPosition = mul(float4(IN.position.x, height, IN.position.z, 1.0), WorldViewProj);
    OUT.texCoord = (IN.texCoord.xyzw + diffuseTexShift) * diffuseTexScale;
    return OUT;
}

vOutTextured_Patch mainVS_Patch(vInTextured_Patch IN)
{
    vOutTextured_Patch OUT;
    
    if (IN.isJoin.r == 1 || (shift < 0 && IN.isJoin.r > 0)) // join edge directly, even value
    {
        // use the next level up only
        float4 rTexCoord;
        if (axis == 0)
            rTexCoord = float4(shift + (IN.texCoord.x * scale), 1 - IN.texCoord.y, IN.texCoord.zw);
        else
            rTexCoord = float4(1 - IN.texCoord.x, shift + (IN.texCoord.y * scale), IN.texCoord.zw);
            
        float3 texel1 = tex2Dlod(NextLevelHeightSampler, rTexCoord).rgb;
        
        float height1 = (texel1.r + texel1.g + texel1.b) / 6;
        
        OUT.hPosition = mul(float4(IN.position.x, height1, IN.position.z, 1.0), WorldViewProj);
    }
    else if (IN.isJoin.r > 0)   // average 2 values from next level, odd value
    {
        // use the next level up
        float4 rTexCoord;
        if (axis == 0)
            rTexCoord = float4(shift + (IN.texCoord.x * scale) - quarterSample, 1 - IN.texCoord.y, IN.texCoord.zw);
        else
            rTexCoord = float4(1 - IN.texCoord.x, shift + (IN.texCoord.y * scale) - quarterSample, IN.texCoord.zw);
            
        // get first pos height
        float3 texel1 = tex2Dlod(NextLevelHeightSampler, rTexCoord).rgb;
        
        float height = (texel1.r + texel1.g + texel1.b) / 6;
        
        if (axis == 0)
            rTexCoord = float4(shift + (IN.texCoord.x * scale) + quarterSample, 1 - IN.texCoord.y, IN.texCoord.zw);
        else
            rTexCoord = float4(1 - IN.texCoord.x, shift + (IN.texCoord.y * scale) + quarterSample, IN.texCoord.zw);
            
        // get first pos height
        texel1 = tex2Dlod(NextLevelHeightSampler, rTexCoord).rgb;
        
        height += (texel1.r + texel1.g + texel1.b) / 6;
        height /= 2;
        
        // blend from 2 nearest points on the next level up
        OUT.hPosition = mul(float4(IN.position.x, height, IN.position.z, 1.0), WorldViewProj);
    }
    else
    {
        float3 heightClr = tex2Dlod(DiffuseSampler, IN.texCoord).rgb;
        float height = (heightClr.r + heightClr.g + heightClr.b) / 6;
        OUT.hPosition = mul(float4(IN.position.x, height, IN.position.z, 1.0), WorldViewProj);
    }
    OUT.color = IN.isJoin;
    OUT.texCoord = (IN.texCoord.xyzw + diffuseTexShift) * diffuseTexScale;
    return OUT;
}

vOutTextured_Patch mainVS_Patch_Inv(vInTextured_Patch IN)
{
    vOutTextured_Patch OUT;
    
    if (IN.isJoin.r == 1 || (shift < 0 && IN.isJoin.r > 0)) // join edge directly, even value
    {
        // use the next level up
        float4 rTexCoord;
        if (axis == 0)
            rTexCoord = float4(shift + (IN.texCoord.x * scale) - quarterSample, 1 - IN.texCoord.y, IN.texCoord.zw);
        else
            rTexCoord = float4(1 - IN.texCoord.x, shift + (IN.texCoord.y * scale) - quarterSample, IN.texCoord.zw);
            
        // get first pos height
        float3 texel1 = tex2Dlod(NextLevelHeightSampler, rTexCoord).rgb;
        
        float height = (texel1.r + texel1.g + texel1.b) / 6;
        
        if (axis == 0)
            rTexCoord = float4(shift + (IN.texCoord.x * scale) + quarterSample, 1 - IN.texCoord.y, IN.texCoord.zw);
        else
            rTexCoord = float4(1 - IN.texCoord.x, shift + (IN.texCoord.y * scale) + quarterSample, IN.texCoord.zw);
            
        // get first pos height
        texel1 = tex2Dlod(NextLevelHeightSampler, rTexCoord).rgb;
        
        height += (texel1.r + texel1.g + texel1.b) / 6;
        height /= 2;
        
        // blend from 2 nearest points on the next level up
        OUT.hPosition = mul(float4(IN.position.x, height, IN.position.z, 1.0), WorldViewProj);
    }
    else if (IN.isJoin.r > 0)   // use value from next level, odd value
    {
        // use the next level up only
        float4 rTexCoord;
        if (axis == 0)
            rTexCoord = float4(shift + (IN.texCoord.x * scale), 1 - IN.texCoord.y, IN.texCoord.zw);
        else
            rTexCoord = float4(1 - IN.texCoord.x, shift + (IN.texCoord.y * scale), IN.texCoord.zw);
            
        float3 texel1 = tex2Dlod(NextLevelHeightSampler, rTexCoord).rgb;
        
        float height1 = (texel1.r + texel1.g + texel1.b) / 6;
        
        OUT.hPosition = mul(float4(IN.position.x, height1, IN.position.z, 1.0), WorldViewProj);
    }
    else
    {
        float3 heightClr = tex2Dlod(DiffuseSampler, IN.texCoord).rgb;
        float height = (heightClr.r + heightClr.g + heightClr.b) / 6;
        OUT.hPosition = mul(float4(IN.position.x, height, IN.position.z, 1.0), WorldViewProj);
    }
    OUT.color = float4(IN.isJoin.r, 0, 0, 1);
    OUT.texCoord = (IN.texCoord.xyzw + diffuseTexShift) * diffuseTexScale;
    return OUT;
}

vOutTextured_Patch_Int mainVS_Patch_Int(vInTextured_Patch IN)
{
    vOutTextured_Patch_Int OUT;
    
    if (IN.isJoin.r > 0) // join edge directly, even value
    {
        // use the next level up
        float4 rTexCoord;
        if (axis == 0)
            rTexCoord = float4(IN.texCoord.x, 1 - IN.texCoord.y, IN.texCoord.zw);
        else
            rTexCoord = float4(1 - IN.texCoord.x, IN.texCoord.y, IN.texCoord.zw);
            
        float3 texel1 = tex2Dlod(NextLevelHeightSampler, rTexCoord).rgb;
        float3 texel2 = tex2Dlod(DiffuseSampler, IN.texCoord).rgb;
        
        float height1 = (texel1.r + texel1.g + texel1.b) / 6;
        float height2 = (texel2.r + texel2.g + texel2.b) / 6;
        
        // blend from 2 nearest points on the next level up
        OUT.hPosition = mul(float4(IN.position.x, (height1 + height2) / 2, IN.position.z, 1.0), WorldViewProj);
    }
    else
    {
        float3 heightClr = tex2Dlod(DiffuseSampler, IN.texCoord).rgb;
        float height = (heightClr.r + heightClr.g + heightClr.b) / 6;
        OUT.hPosition = mul(float4(IN.position.x, height, IN.position.z, 1.0), WorldViewProj);
    }
    OUT.texCoord = (IN.texCoord.xyzw + diffuseTexShift) * diffuseTexScale;
    OUT.color = IN.isJoin;
    return OUT;
}

float4 mainPS_Debug(vOutTextured IN) : COLOR
{
    return float4(0, dem_level * 0.125, 0, 1);
}

float4 mainPatchPS(vOutTextured_Patch IN) : COLOR
{
    return tex2D(NormalMapSampler, IN.texCoord);
}

float4 mainPatchPS_Int(vOutTextured_Patch_Int IN) : COLOR
{
    return tex2D(NormalMapSampler, IN.texCoord);
}

float4 mainPS(vOutTextured IN) : COLOR
{
    return tex2D(NormalMapSampler, IN.texCoord);
}

technique Debug
{
    pass p0
    {
        VertexShader = compile vs_3_0 mainVS();
        PixelShader = compile ps_3_0 mainPS_Debug();
    }
}

technique Basic_Patch
{
    pass p0
    {
        VertexShader = compile vs_3_0 mainVS_Patch();
        PixelShader = compile ps_3_0 mainPatchPS();
    }
}

technique Basic_Patch_Inv
{
    pass p0
    {
        VertexShader = compile vs_3_0 mainVS_Patch_Inv();
        PixelShader = compile ps_3_0 mainPatchPS();
    }
}

technique Basic_Patch_Int
{
    pass p0
    {
        VertexShader = compile vs_3_0 mainVS_Patch_Int();
        PixelShader = compile ps_3_0 mainPatchPS_Int();
    }
}

technique Basic
{
    pass p0
    {
        VertexShader = compile vs_3_0 mainVS();
        PixelShader = compile ps_3_0 mainPS();
    }
}