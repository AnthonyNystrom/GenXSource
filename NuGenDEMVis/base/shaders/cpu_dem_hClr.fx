float4x4 WorldViewProj : WorldViewProjection;
float MinHeight;
float HeightScale;
float4 HeightClr;

texture DiffuseTexture : Diffuse;
texture HeightTexture : Diffuse;


struct vIn {
    float3 position     : POSITION;
};

struct vInTextured {
    float3 position     : POSITION;
    float4 texCoord     : TEXCOORD0;
};

struct vOut {
    float4 hPosition		: POSITION;
    float4 clr          : COLOR;
};

struct vOutTextured {
    float4 hPosition		: POSITION;
    float4 texCoord     : TEXCOORD0;
};

sampler HeightTextureSampler = sampler_state 
{
    texture = <HeightTexture>;
    AddressU  = CLAMP;        
    AddressV  = CLAMP;
    AddressW  = CLAMP;
    MIPFILTER = LINEAR;
    MINFILTER = LINEAR;
    MAGFILTER = LINEAR;
};


// just a height value from the vertex into clr
vOut VS_HeightClr(vIn IN) {
    vOut OUT;
    OUT.hPosition = mul(float4(IN.position.xyz , 1.0) , WorldViewProj);
    float height = (IN.position.y - MinHeight) * HeightScale;
    OUT.clr = HeightClr * height;
    return OUT;
}

float4 PS_HeightClr(vOut IN): COLOR {
    return IN.clr;
}

// height value from height map into clr
vOutTextured VS_HeightMappedClr(vInTextured IN) {
    vOutTextured OUT;
    OUT.hPosition = mul(float4(IN.position.xyz , 1.0) , WorldViewProj);
    OUT.texCoord = IN.texCoord;
    return OUT;
}

float4 PS_HeightMappedClr(vOutTextured IN): COLOR {
    float hValue = 1-tex2D(HeightTextureSampler, IN.texCoord).x;
    float height = (hValue - MinHeight) * HeightScale;
    return HeightClr * height;
}


technique CPU_DEM_HeightClr {
    pass p0 {
		    VertexShader = compile vs_1_1 VS_HeightClr();
		    PixelShader  = compile ps_1_1 PS_HeightClr();
    }
}

technique CPU_DEM_HeightMappedClr {
    pass p0 {
		    VertexShader = compile vs_1_1 VS_HeightMappedClr();
		    PixelShader  = compile ps_1_1 PS_HeightMappedClr();
    }
}