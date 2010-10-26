//--------------------------------------------------------------------------------------
// File: MarchingCubes.fx
//
// The effect file for the MarchingCubes sample.  
// 
// Copyright (c) Jens Krüger. All rights reserved.
//--------------------------------------------------------------------------------------


//--------------------------------------------------------------------------------------
// Global variables
//--------------------------------------------------------------------------------------
float4x4 g_mWorldViewProjection;	// World * View * Projection matrix
float4x4 g_mWorldView;				// World * View matrix

struct app2Vertex {
	float4 Position : POSITION;
	float4 Normal	: NORMAL0;
};

struct vertex2pixel {
	float4 Position : POSITION;
	float4 Color	: COLOR0;
};

vertex2pixel vsMain(app2Vertex IN) {
	vertex2pixel OUT;
	OUT.Position = mul(IN.Position,g_mWorldViewProjection);
	IN.Normal.w  = 0;
	
	float diffuse = dot(float3(0,0,1),mul(IN.Normal,g_mWorldView).xyz );
	OUT.Color     = saturate( diffuse*float4(0,0,1,1))+
	                saturate(-diffuse*float4(1,1,0,1));
	return OUT;
}

float4 psMain(vertex2pixel IN) : COLOR  {
	return IN.Color;
}

//--------------------------------------------------------------------------------------
// Techniques
//--------------------------------------------------------------------------------------
technique RenderScene
{
    pass P0
    { 
		VertexShader = compile vs_1_1 vsMain();
		PixelShader  = compile ps_1_1 psMain();
		
		CullMode			= NONE; 
    }
}
