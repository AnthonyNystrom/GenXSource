//
//	Basic Illumination shader file
//	
//	Target: Vertex, Pixel shader 2.0
//	Constant reg: #256
//	Temp reg: #12
//	No dynamic flow control
//	Yes static flow control
//	No Geometry HW instancing
//	


//
float3	g_Light1Dir;                  // 1. Light's direction in world space
float3	g_Light2Dir;                  // 1. Light's direction in world space

float3  g_cameraPosInvWorld;				//	cameraPos * World_inverse, wireframe 에 사용.
float	g_wireframeLineWidth = 0.1f;	//	

float	g_Light1Intensity;
float	g_Light2Intensity;

float4	g_Light1Color;
float4	g_Light2Color;

float4 g_MaterialDiffuseColor;      // 2. Material's diffuse color

float4x4 g_mWorld;                  // 3. World matrix for object
float4x4 g_mWorldView;				// 3. World * View			--> used in FOG
float4x4 g_mWorldViewProjection;    // 3. World * View * Projection matrix

texture g_Texture;				    // 4. Color texture for mesh

float	g_alpha;					//	7. surface 등을 그릴때 alpha 값.

float4  g_indicateDiffuseColor;			//	9.  indacate color

bool	g_bIndicate;				//	10. 이것이 설정되면 g_indicateDiffuseColor로 선택된것 처럼 그린다.

float4	g_clipPlane0;				//	11. clip Plane의 a,b,c,d 이다.
float	g_clipPlane0Dir;

float4	g_clipPlane1;				//	11. clip Plane의 a,b,c,d 이다.
float	g_clipPlane1Dir;

float4	g_clipPlane2;				//	11. clip Plane의 a,b,c,d 이다.
float	g_clipPlane2Dir;

float2	g_vFog;						//	g_vFog.x = far/(far-near)
									//	g_vFog.y = -1/(far-near)

bool	g_bBackfaceColor;			//	13. surface 뒷면 컬러 On/OFF
float	g_bBlendBackfaceColor;		//  13. 기존 color와 blend value
float4	g_backfaceDiffuseColor;			//	13. backface color

float4	g_vecEye;					//	eye pos

float	g_intensityAmbient;			//	ambient Intensity
float	g_intensityDiffuse;			//	diffuse Intensity
float	g_intensitySpecular;		//	specular Intensity

// only used for vs_2_0 Shader Instancing
//	number 120 is sync to render source.
//	used for sphere and mesh

#define NumBatchInstance 75		//	 use 246 constant 

float4	g_vBatchInstancePosition[NumBatchInstance];
//	기본형이 float4이다. float 로 사용해도 float4로 만들어진다.
float4  g_vBatchInstanceSelectionRotationXYScale[NumBatchInstance];
float4  g_vBatchInstanceColor[NumBatchInstance];


//
//
//
texture g_MeshBackground;              // Color texture for mesh
sampler MeshBackgroundSampler = 
sampler_state
{
    Texture = <g_MeshBackground>;
    MipFilter = LINEAR;
    MinFilter = LINEAR;
    MagFilter = LINEAR;

    AddressU = CLAMP ;
    AddressV = CLAMP ;    

};

//
//	for ribbon
//	
sampler MeshTextureSampler = 
sampler_state
{
	Texture = <g_Texture>;
	MipFilter = LINEAR;
	MinFilter = LINEAR;
	MagFilter = LINEAR;
};

////////////////////////////////////////////////////////////////////////////////////////////

inline 
float	CalcClipPlaneVisible(float4 vPosWorld)
{
	float	result0 = 1.0f;
	float	result1 = 1.0f;
	float	result2 = 1.0f;

	//	사용하는 g_clipPlane0, g_clipPlane1, g_clipPlane2 는 Plane equ로 설정.
	//	사용하지 않는 g_clipPlane0, g_clipPlane1, g_clipPlane2 는 (0,0,0,1)로 설정.
	//	vPosWorld의 w 는 항상 1(+). 
	//	g_clipPlane0Dir 은 항상 (+) 로.
	
	result0 = dot(g_clipPlane0, vPosWorld) * (g_clipPlane0Dir);
	result1 = dot(g_clipPlane1, vPosWorld) * (g_clipPlane1Dir);
	result2 = dot(g_clipPlane2, vPosWorld) * (g_clipPlane2Dir);
	
	return result0*result1*result2;
}

////////////////////////////////////////////////////////////////////////////////////////////
//	
//	VS 2.0 Shader Instancing Sphere/cylinder Render
//	Phong Shading.
//	
////////////////////////////////////////////////////////////////////////////////////////////
struct VS_OUTPUT_SPHERE_VERTEX_PHONG
{
	float4	Pos				: POSITION;
	
	float4	DiffuseColor	: COLOR0;
	
	float4	PosWorld		: TEXCOORD0;		//	for clipping.
	float3	NormalWorld		: TEXCOORD1;		//	for specular light
	float3	PixelToEye		: TEXCOORD2;		//	Specular Light
	
	float1	bFog			: FOG;
};
						
VS_OUTPUT_SPHERE_VERTEX_PHONG SphereRenderingPhongVS(
				float4	inPos			: POSITION,
				float3	inNorm			: NORMAL,
				float	vBoxInstanceIndex : TEXCOORD0 , 
				uniform bool bRenderSphere
				)
{
	VS_OUTPUT_SPHERE_VERTEX_PHONG Out = (VS_OUTPUT_SPHERE_VERTEX_PHONG) 0; 

	float4	vSpherePosition = g_vBatchInstancePosition[(int)vBoxInstanceIndex];
	float	bSelect = g_vBatchInstanceSelectionRotationXYScale[(int)vBoxInstanceIndex].x;
	float4	vColor = g_vBatchInstanceColor[(int)vBoxInstanceIndex];

	float4 vRotatedPos = inPos;
	float3 vRotatedNorm = inNorm;
	
	if ( bRenderSphere == false )
	{	//	cylinder 일때 rotation을 추가한다.
		float	rotationY = g_vBatchInstanceSelectionRotationXYScale[(int)vBoxInstanceIndex].y;
		float	rotationX = g_vBatchInstanceSelectionRotationXYScale[(int)vBoxInstanceIndex].z;
		float	scale = g_vBatchInstanceSelectionRotationXYScale[(int)vBoxInstanceIndex].w;

		//	fist, sclae.
		inPos.z = inPos.z * scale;

		//	second, position rotation
		vRotatedPos.y = inPos.y * cos(rotationX) - inPos.z * sin(rotationX);
		vRotatedPos.z = inPos.y * sin(rotationX) + inPos.z * cos(rotationX);

		inPos = vRotatedPos;
		vRotatedPos.x = inPos.x * cos(rotationY) + inPos.z * sin(rotationY);
		vRotatedPos.z = -inPos.x * sin(rotationY) + inPos.z * cos(rotationY);

		//	Norm Rotation
		vRotatedNorm.y = inNorm.y * cos(rotationX) - inNorm.z * sin(rotationX);
		vRotatedNorm.z = inNorm.y * sin(rotationX) + inNorm.z * cos(rotationX);

		inNorm = vRotatedNorm;
		vRotatedNorm.x = inNorm.x * cos(rotationY) + inNorm.z * sin(rotationY);
		vRotatedNorm.z = -inNorm.x * sin(rotationY) + inNorm.z * cos(rotationY);
	}
	
	//	third, translation
	vRotatedPos += vSpherePosition;
		
	// Output transformed vertex position
	Out.Pos = mul( vRotatedPos, g_mWorldViewProjection );

	//    specular light
	Out.NormalWorld =	mul(vRotatedNorm, g_mWorld);						//	N    
	Out.PosWorld =		mul(vRotatedPos, g_mWorld);						//	clipping, Out.PosWorld의 w 값은 1.
	Out.PixelToEye =	g_vecEye - Out.PosWorld;					//	V

	//	Fog.
	float4 posView = mul(vRotatedPos, g_mWorldView);
	Out.bFog = g_vFog.x  - length(posView) * g_vFog.y;

	Out.DiffuseColor =	vColor * 
						( ( g_Light1Intensity * g_Light1Color * g_intensityDiffuse * max ( g_intensityAmbient, dot(Out.NormalWorld, g_Light1Dir) ) ) +
						  ( g_Light2Intensity * g_Light2Color * g_intensityDiffuse * max ( g_intensityAmbient, dot(Out.NormalWorld, g_Light2Dir) ) ) ) ;

	Out.DiffuseColor.a = bSelect;
	
	return Out;
}

float4 SphereRenderingPhongPS(  VS_OUTPUT_SPHERE_VERTEX_PHONG In , uniform bool bPhong) : COLOR
{
	clip ( CalcClipPlaneVisible(In.PosWorld) );

	float fSelect = In.DiffuseColor.a;
	float4 finalColor;
	if ( bPhong == true )
	{
		float3 H1 = normalize(g_Light1Dir + normalize(In.PixelToEye));
		float3 H2 = normalize(g_Light2Dir + normalize(In.PixelToEye));
		float3 N = normalize(In.NormalWorld);
		
		//	
		//  per pixel lighting.
		//	specular intensity.
		finalColor = In.DiffuseColor + 
					g_Light1Color * g_Light1Intensity * g_intensitySpecular * pow( max(0.0,dot(N,H1)), 30 ) + 
					g_Light2Color * g_Light2Intensity * g_intensitySpecular * pow( max(0.0,dot(N,H2)), 30 );
	}
	else
	{
		finalColor = In.DiffuseColor;
	}
	
	finalColor.a = fSelect;

	return finalColor;
}

//--------------------------------------------------------------//
// Technique Section for Effect Workspace.HLSL Illumination Effects.
//--------------------------------------------------------------//
technique SphereRenderingBatchPhong
{
	pass Single_Pass
	{
		VertexShader = compile vs_2_0 SphereRenderingPhongVS(true);
		PixelShader = compile ps_2_0 SphereRenderingPhongPS(true);
	}
}

technique SphereRenderingBatchGouraud
{
	pass Single_Pass
	{
		VertexShader = compile vs_2_0 SphereRenderingPhongVS(true);
		PixelShader = compile ps_2_0 SphereRenderingPhongPS(false);
	}
}

//--------------------------------------------------------------//
// Technique Section for Effect Workspace.HLSL Illumination Effects.
//--------------------------------------------------------------//
technique CylinderRenderingBatchPhong
{
	pass Single_Pass
	{
		VertexShader = compile vs_2_0 SphereRenderingPhongVS(false);
		PixelShader = compile ps_2_0 SphereRenderingPhongPS(true);
	}
}

technique CylinderRenderingBatchGouraud
{
	pass Single_Pass
	{
		VertexShader = compile vs_2_0 SphereRenderingPhongVS(false);
		PixelShader = compile ps_2_0 SphereRenderingPhongPS(false);
	}
}

////////////////////////////////////////////////////////////////////////////////////////////
//
//	Ribbon rendering
//
////////////////////////////////////////////////////////////////////////////////////////////

struct VS_OUTPUT_RIBBON_VERTEX_PHONG
{
	float4	Pos				: POSITION;
	
	float4	DiffuseColor	: COLOR0;
	
	float4	PosWorld		: TEXCOORD0;		//	for clipping.
	float3	NormalWorld		: TEXCOORD1;		//	for specular light
	float3	PixelToEye		: TEXCOORD2;		//	Specular Light
	
	float2 TextureCoord		: TEXCOORD3;
	
	float1	bFog			: FOG;
};

VS_OUTPUT_RIBBON_VERTEX_PHONG RibbonRenderingPhongVS(
				float4	inPos			: POSITION,
				float3	inNorm			: NORMAL,
				float2	inTex			: TEXCOORD0,
				float4	diffuseColor	: COLOR0)
{
	VS_OUTPUT_RIBBON_VERTEX_PHONG Out = (VS_OUTPUT_RIBBON_VERTEX_PHONG) 0; 

	float bSelect = diffuseColor.a;

	// Output transformed vertex position
	Out.Pos = mul( inPos, g_mWorldViewProjection );

	//    specular light
	Out.NormalWorld =	mul(inNorm, g_mWorld);						//	N    
	Out.PosWorld =		mul(inPos, g_mWorld);						//	clipping, Out.PosWorld의 w 값은 1.
	Out.PixelToEye =	g_vecEye - Out.PosWorld;					//	V

	//	Fog.
	float4 posView = mul(inPos, g_mWorldView);
	Out.bFog = g_vFog.x  - length(posView) * g_vFog.y;

	Out.DiffuseColor =	diffuseColor * 
						( ( g_Light1Intensity * g_Light1Color * g_intensityDiffuse * max ( g_intensityAmbient, dot(Out.NormalWorld, g_Light1Dir) ) ) +
						  ( g_Light2Intensity * g_Light2Color * g_intensityDiffuse * max ( g_intensityAmbient, dot(Out.NormalWorld, g_Light2Dir) ) ) );

	Out.DiffuseColor.a = bSelect;
	
	Out.TextureCoord = inTex;

	return Out;
}

float4 RibbonRenderingPhongPS(  VS_OUTPUT_RIBBON_VERTEX_PHONG In , uniform bool bTexture , uniform bool bPhong) : COLOR
{
	clip ( CalcClipPlaneVisible(In.PosWorld) );

	float fSelect = In.DiffuseColor.a;
	float4 finalColor;
	
	float4 textureColor = float4 (1,1,1,0);
	if ( bTexture == true )
	{
		textureColor = tex2D(MeshTextureSampler, In.TextureCoord);
	}
	
	if ( bPhong == true )
	{
		float3 H1 = normalize(g_Light1Dir + normalize(In.PixelToEye));
		float3 H2 = normalize(g_Light2Dir + normalize(In.PixelToEye));
		float3 N = normalize(In.NormalWorld);
		
		//	
		//  per pixel lighting.
		//	specular intensity.
		finalColor = In.DiffuseColor * textureColor + 
					g_Light1Color * g_Light1Intensity * g_intensitySpecular * pow( max(0.0,dot(N,H1)), 30 ) + 
					g_Light2Color * g_Light2Intensity * g_intensitySpecular * pow( max(0.0,dot(N,H2)), 30 );
	}
	else
	{
		finalColor = In.DiffuseColor * textureColor;
	}
	
	finalColor.a = fSelect;

	return finalColor;
}

//--------------------------------------------------------------//
// Technique Section for Effect Workspace.HLSL Illumination Effects.
//--------------------------------------------------------------//
technique RibbonRenderingPhong
{
	pass Single_Pass
	{
		VertexShader = compile vs_2_0 RibbonRenderingPhongVS();
		PixelShader = compile ps_2_0 RibbonRenderingPhongPS(true,true);
	}
}

technique RibbonRenderingNoTexturePhong
{
	pass Single_Pass
	{
		VertexShader = compile vs_2_0 RibbonRenderingPhongVS();
		PixelShader = compile ps_2_0 RibbonRenderingPhongPS(false,true);
	}
}

technique RibbonRenderingGouraud
{
	pass Single_Pass
	{
		VertexShader = compile vs_2_0 RibbonRenderingPhongVS();
		PixelShader = compile ps_2_0 RibbonRenderingPhongPS(true,false);
	}
}

technique RibbonRenderingNoTextureGouraud
{
	pass Single_Pass
	{
		VertexShader = compile vs_2_0 RibbonRenderingPhongVS();
		PixelShader = compile ps_2_0 RibbonRenderingPhongPS(false,false);
	}
}

////////////////////////////////////////////////////////////////////////////////////////////
//
//	WIREFRAME
//
////////////////////////////////////////////////////////////////////////////////////////////
struct VS_OUTPUT_DIFFUSE
{
	float4	Pos  : POSITION;			//
	float4	Diff : COLOR0;				//
	float4	PosWorld: TEXCOORD0;		//	for clip ps.
	float1	bFog : FOG;
};

VS_OUTPUT_DIFFUSE WireframeRenderingPhongVS( 
	float4	inPos		:	POSITION,
	float4	diffuseColor:	COLOR0 ,
	float3	linePos2:		TEXCOORD0,
	float2	lineIndex: 		TEXCOORD1,
	uniform bool			bLineWidth
)
{
	VS_OUTPUT_DIFFUSE Out = (VS_OUTPUT_DIFFUSE) 0; 

	if ( bLineWidth == true )
	{
		float3 vecLine = linePos2-inPos;
		float3 finalPosVec = cross( vecLine, g_cameraPosInvWorld );
	
		finalPosVec = normalize(finalPosVec);

		[flatten] if ( lineIndex.x == 1.0 )
		{
			inPos = float4 (inPos + finalPosVec * g_wireframeLineWidth , 1 );
		}
		else if ( lineIndex.x == 2.0 )
		{
			inPos = float4 (inPos + (-finalPosVec) * g_wireframeLineWidth, 1 );
		}
		else if ( lineIndex.x == 3.0 )
		{
			inPos = float4 (linePos2 + finalPosVec * g_wireframeLineWidth , 1 );
		}
		else if ( lineIndex.x == 4.0 )
		{
			inPos = float4 (linePos2 + (-finalPosVec) * g_wireframeLineWidth , 1);
		}
	}
	
	Out.Pos = mul( inPos, g_mWorldViewProjection ); 
	
	//	Fog.
		float4 posView = mul(inPos, g_mWorldView);
			
		//	Out.bFog is 1 : No Fog, transparent.
		//	Out.bFog is 0 : Full Fog, Opaque
		Out.bFog = g_vFog.x - length(posView) * g_vFog.y;
	//
		
	Out.PosWorld = mul(inPos, g_mWorld);	//	Out.PosWorld의 w 값은 1.

	Out.Diff = diffuseColor;
	Out.Diff.a = lineIndex.y;
	
	return Out;
}

float4 WireframeRenderingPhongPS ( VS_OUTPUT_DIFFUSE In ) : COLOR0
{
	clip( CalcClipPlaneVisible(In.PosWorld) );
	
	return In.Diff;
}

//--------------------------------------------------------------//
// Technique Section for Effect Workspace.HLSL Illumination Effects.
//--------------------------------------------------------------//
technique WireframeRenderingPhongLineWidth	
{
	pass Single_Pass
	{
		VertexShader = compile vs_2_0 WireframeRenderingPhongVS(true);
		PixelShader = compile ps_2_0 WireframeRenderingPhongPS();
	}
}

technique WireframeRenderingPhong	
{
	pass Single_Pass
	{
		VertexShader = compile vs_2_0 WireframeRenderingPhongVS(false);
		PixelShader = compile ps_2_0 WireframeRenderingPhongPS();
	}
}

////////////////////////////////////////////////////////////////////////////////////////////
//
//	 Surface Rendering
//
////////////////////////////////////////////////////////////////////////////////////////////

struct VS_OUTPUT_SURFACE_VERTEX_PHONG
{
	float4	Pos				: POSITION;
	float4	DiffuseColor	: COLOR0;
	float4	PosWorld		: TEXCOORD0;		//	for clipping.
	float3	NormalWorld		: TEXCOORD1;		//	for specular light
	float3	PixelToEye		: TEXCOORD2;		//	Specular Light
	
	//  In Shader Model 3, Fog register is obsoleted.
	//	Convert SM3->SM2
	float1	bFog			: FOG;
};

VS_OUTPUT_SURFACE_VERTEX_PHONG SurfaceRenderingPhongVS (	
														float4 inPos : POSITION, 
														float3 inNorm : NORMAL , 
														float4 diffuseColor: COLOR0 )
{
	VS_OUTPUT_SURFACE_VERTEX_PHONG Out = (VS_OUTPUT_SURFACE_VERTEX_PHONG)0;
	
	// Output transformed vertex position
	Out.Pos = mul( inPos, g_mWorldViewProjection );

	if ( g_bBackfaceColor == true )
	{
		diffuseColor = lerp(diffuseColor, g_backfaceDiffuseColor, g_bBlendBackfaceColor);
		inNorm = normalize(-inNorm);
	}

	//    specular light
	Out.NormalWorld =	mul(inNorm, g_mWorld);						//	N    
	Out.PosWorld =		mul(inPos, g_mWorld);						//	clipping, Out.PosWorld의 w 값은 1.
	Out.PixelToEye =	g_vecEye - Out.PosWorld;					//	V

	//	Fog.
	float4 posView = mul(inPos, g_mWorldView);
	Out.bFog = g_vFog.x  - length(posView) * g_vFog.y;

	if ( g_bIndicate == true )
		diffuseColor = g_indicateDiffuseColor;

	Out.DiffuseColor =	diffuseColor * 
						( ( g_Light1Intensity * g_Light1Color * g_intensityDiffuse * max ( g_intensityAmbient, dot(Out.NormalWorld, g_Light1Dir) ) ) +
					      ( g_Light2Intensity * g_Light2Color * g_intensityDiffuse * max ( g_intensityAmbient, dot(Out.NormalWorld, g_Light2Dir) ) ) );
	
	return Out;
}

float4 SurfaceRenderingPhongPS( VS_OUTPUT_SURFACE_VERTEX_PHONG In , uniform bool bUseAlpha, uniform bool bPhong ) : COLOR0
{
	clip ( CalcClipPlaneVisible(In.PosWorld) );
	
	float4 finalColor;
	if ( bPhong == true )
	{
		float3 H1 = normalize(g_Light1Dir + normalize(In.PixelToEye));
		float3 H2 = normalize(g_Light2Dir + normalize(In.PixelToEye));
		float3 N = normalize(In.NormalWorld);
		
		//	
		//  per pixel lighting.
		//	specular intensity.
		finalColor = In.DiffuseColor + 
					g_Light1Color * g_Light1Intensity * g_intensitySpecular * pow( max(0.0,dot(N,H1)), 30 ) + 
					g_Light2Color * g_Light2Intensity * g_intensitySpecular * pow( max(0.0,dot(N,H2)), 30 );
	}
	else
	{
		finalColor = In.DiffuseColor;
	}
	
	//    pixel transparency: uniform
	if ( bUseAlpha == true )
	{
		finalColor.a = g_alpha;
	}
	
	return finalColor;
}

technique SurfaceRenderingNoAlphaPhong
{
	pass P0
	{
		VertexShader = compile vs_2_0 SurfaceRenderingPhongVS();
		PixelShader  = compile ps_2_0 SurfaceRenderingPhongPS(false,true);
	}
}

technique SurfaceRenderingWithAlphaPhong
{
	pass P0
	{
		//    AlphaBlendEnable = true;
		SrcBlend = SrcAlpha;
		DestBlend = InvSrcAlpha;

		VertexShader = compile vs_2_0 SurfaceRenderingPhongVS();
		PixelShader  = compile ps_2_0 SurfaceRenderingPhongPS(true,true);
	}
}

technique SurfaceRenderingNoAlphaGouraud
{
	pass P0
	{
		VertexShader = compile vs_2_0 SurfaceRenderingPhongVS();
		PixelShader  = compile ps_2_0 SurfaceRenderingPhongPS(false,false);
	}
}

technique SurfaceRenderingWithAlphaGouraud
{
	pass P0
	{
		//    AlphaBlendEnable = true;
		SrcBlend = SrcAlpha;
		DestBlend = InvSrcAlpha;

		VertexShader = compile vs_2_0 SurfaceRenderingPhongVS();
		PixelShader  = compile ps_2_0 SurfaceRenderingPhongPS(true,false);
	}
}

////////////////////////////////////////////////////////////////////////////////////////////
//
//	Axis를 그린다.
//
////////////////////////////////////////////////////////////////////////////////////////////

struct VS_OUTPUT_AXIS
{
	float4  Position		: POSITION;   // vertex position
	float4  Diffuse			: COLOR0;     // vertex diffuse color
	float4  PosWorld		: TEXCOORD0;
	float1	bFog			: FOG;
};

VS_OUTPUT_AXIS	AxisRenderingVS( float4 vPos : POSITION, float3 inNorm : NORMAL )
{
	VS_OUTPUT_AXIS	Output;

	Output.Position = mul(vPos, g_mWorldViewProjection);

	float3 vNormal = normalize( mul( inNorm , (float3x3)g_mWorld) );
	Output.Diffuse = g_MaterialDiffuseColor * ( max(0.2,dot(vNormal, g_Light1Dir)) + max(0.2,dot(vNormal, g_Light2Dir)) );
	Output.Diffuse.a = 0.0f;

	Output.PosWorld =	mul(vPos, g_mWorld);						//	clipping, Out.PosWorld의 w 값은 1.
	
	//	Fog.
	float4 posView = mul(vPos, g_mWorldView);
	Output.bFog = g_vFog.x  - length(posView) * g_vFog.y;

	return Output;
}

float4 AxisRenderingPS ( VS_OUTPUT_AXIS	In ) : COLOR0
{
	clip ( CalcClipPlaneVisible(In.PosWorld) );
	
	return In.Diffuse;
}

technique AxisRendering
{
	pass P0
	{
		VertexShader = compile vs_2_0 AxisRenderingVS();
		PixelShader  = compile ps_2_0 AxisRenderingPS();
	}
}

////////////////////////////////////////////////////////////////////////////////////////////
//
//	LINE을 그린다.
//
////////////////////////////////////////////////////////////////////////////////////////////
struct VS_OUTPUT_LINE_WIREFRAME
{
	float4 Pos  : POSITION;
	float4 Diff : COLOR0;
};

VS_OUTPUT_LINE_WIREFRAME	HLSL_LineWireframeRendering_VS( float4 inPos  : POSITION )
{
	VS_OUTPUT_LINE_WIREFRAME Out;
	Out.Pos = mul( inPos, g_mWorldViewProjection ); 
	Out.Diff = g_MaterialDiffuseColor;
	return Out;
}

float4 HLSL_LineWireframeRendering_PS ( VS_OUTPUT_LINE_WIREFRAME In ) : COLOR0
{
	return In.Diff;
}

//--------------------------------------------------------------//
// Technique Section for Effect Workspace.HLSL Illumination Effects.
//--------------------------------------------------------------//
technique LineWireframeRendering
{
	pass Single_Pass
	{
		VertexShader = compile vs_2_0 HLSL_LineWireframeRendering_VS();
		PixelShader = compile ps_2_0 HLSL_LineWireframeRendering_PS();
	}
}

////////////////////////////////////////////////////////////////////////////////////////////
//
//	Draw Clip Plane
//
////////////////////////////////////////////////////////////////////////////////////////////
struct VS_OUTPUT_PLANE
{
	float4 Position		: POSITION;   // vertex position
	float4 Diffuse		: COLOR0;     // vertex diffuse color
	float1 bFog			: FOG;
};


VS_OUTPUT_PLANE	ClipPlaneVS ( float4 vPos : POSITION, float3 inNorm : NORMAL , uniform bool bAlpha )
{
	VS_OUTPUT_PLANE	Output = (VS_OUTPUT_PLANE)0;

	// Transform the position from object space to homogeneous projection space
	Output.Position = mul(vPos, g_mWorldViewProjection);
	
	float3 vNormal = normalize( mul( inNorm , (float3x3)g_mWorld) );
	Output.Diffuse = g_MaterialDiffuseColor * ( max(0.2,dot(vNormal, g_Light1Dir)) + max(0.2,dot(vNormal, g_Light2Dir)) );

	float4 posView = mul(vPos, g_mWorldView);
	Output.bFog = g_vFog.x  - length(posView) * g_vFog.y;

	if ( bAlpha == true )
		Output.Diffuse.a = g_alpha;
	else
		Output.Diffuse.a = 0.0f;

	return Output;
}

float4 ClipPlanePS( VS_OUTPUT_PLANE	In ) : COLOR0
{
	return In.Diffuse;
}

technique ClipPlaneRenderingNoAlpha
{
	pass P0
	{
		VertexShader = compile vs_2_0 ClipPlaneVS(false);
		PixelShader  = compile ps_2_0 ClipPlanePS();
	}
}

technique ClipPlaneRenderingWithAlpha
{
	pass P0
	{
		AlphaBlendEnable = true;
		SrcBlend = SrcAlpha;
		DestBlend = InvSrcAlpha;

		VertexShader = compile vs_2_0 ClipPlaneVS(true);
		PixelShader  = compile ps_2_0 ClipPlanePS();
	}
}

////////////////////////////////////////////////////////////////////////////////////////////
//
//	SKYBOX 을 그린다.
//
////////////////////////////////////////////////////////////////////////////////////////////
struct VS_OUTPUT_SKYBOX
{
    float4 Pos				: POSITION;		// vertex position 
	float4 Diffuse			: COLOR0;
    float2 TextureCoord		: TEXCOORD0;		// vertex texture coords 
};

VS_OUTPUT_SKYBOX SkyBoxRenderingVS(VS_OUTPUT_SKYBOX In )
{
	VS_OUTPUT_SKYBOX Out = (VS_OUTPUT_SKYBOX)0;
	
	Out.Pos = mul(In.Pos, g_mWorldViewProjection);
	Out.Diffuse = In.Diffuse;
	Out.TextureCoord = In.TextureCoord;
	
	return Out;
}

float4 SkyBoxRenderingPS ( VS_OUTPUT_SKYBOX In ) : COLOR0
{
	float4 finalColor = tex2D(MeshTextureSampler, In.TextureCoord) * In.Diffuse;
	finalColor.a = 0.0f;
	return finalColor;
}

//--------------------------------------------------------------//
// Technique Section for Effect Workspace.HLSL Illumination Effects.
//--------------------------------------------------------------//
technique SkyBoxRendering
{
	pass Single_Pass
	{
		VertexShader = compile vs_2_0 SkyBoxRenderingVS();
		PixelShader = compile ps_2_0 SkyBoxRenderingPS();
	}
}

////////////////////////////////////////////////////////////////////////////////////////////
//
//	DrawText3D 을 그린다.
//
////////////////////////////////////////////////////////////////////////////////////////////
struct VS_OUTPUT_TEXT3D
{
	float4 Position		: POSITION;   // vertex position
	float4 Diffuse		: COLOR0;     // vertex diffuse color
	float2 TextureCoord		: TEXCOORD0;		// vertex texture coords 
	float1 bFog			: FOG;
};

VS_OUTPUT_TEXT3D	Text3DVS ( 
								float4	inPos			: POSITION,
								float3	inNorm			: NORMAL,
								float4	diffuseColor	: COLOR0,
								float2	inTex			: TEXCOORD0,
								uniform bool bAlpha )
{
	VS_OUTPUT_TEXT3D	Output;

	// Transform the position from object space to homogeneous projection space
	Output.Position = mul(inPos, g_mWorldViewProjection);
	Output.Diffuse = diffuseColor;

	float4 posView = mul(inPos, g_mWorldView);
	Output.bFog = g_vFog.x  - length(posView) * g_vFog.y;

	Output.TextureCoord = inTex;
	
	if ( bAlpha == true )
 		Output.Diffuse.a = g_alpha;
 	else
 		Output.Diffuse.a = 1.0f;

	return Output;
}

float4 Text3DPS( VS_OUTPUT_TEXT3D	In ) : COLOR0
{
	float4 finalColor = tex2D(MeshTextureSampler, In.TextureCoord) * In.Diffuse;
	return finalColor;
}

technique Text3DRenderingNoAlpha
{
	pass P0
	{
		VertexShader = compile vs_2_0 Text3DVS(false);
		PixelShader  = compile ps_2_0 Text3DPS();
	}
}

technique Text3DRenderingWithAlpha
{
	pass P0
	{
		AlphaBlendEnable = true;
		SrcBlend = SrcAlpha;
		DestBlend = InvSrcAlpha;

		VertexShader = compile vs_2_0 Text3DVS(true);
		PixelShader  = compile ps_2_0 Text3DPS();
	}
}

////////////////////////////////////////////////////////////////////////////////////////////
//
//	Final Rendering with Border
//
////////////////////////////////////////////////////////////////////////////////////////////
float2		g_finalImageDelta;

struct VS_OUTPUT_FINAL
{
    float4 Position   : POSITION;		// vertex position 
    float2 TextureUV0  : TEXCOORD0;		// vertex texture coords 
    float2 TextureUV1  : TEXCOORD1;		// vertex texture coords 
    float2 TextureUV2  : TEXCOORD2;		// vertex texture coords 
    float2 TextureUV3  : TEXCOORD3;		// vertex texture coords 
};

struct PS_OUTPUT_FINAL
{
    float4 RGBColor : COLOR0;  // Pixel color    
};

//	
//	0..9 까지.
//	0 은 사용안함.
//	1 은 디폴트 selection
//	2..9 는 사용자 selection color.
//	

//
//	pixel shader 에서 array 의 index는 index 가 사용되지 않고 if then else 로 풀린다.
//	63 instruction.
//
float4 g_colorIndicate[7] = { {1,1,0,0}, {1,1,0,0}, {1,1,0,0}, {1,1,0,0}, {1,1,0,0} , {1,1,0,0}, {1,1,0,0} };

PS_OUTPUT_FINAL RenderFinalSceneBorderPS(VS_OUTPUT_FINAL In)
{
	PS_OUTPUT_FINAL Output;

	//	texture 값만 출력.
	float4 finalColor0 = tex2D(MeshBackgroundSampler, In.TextureUV0);
	float4 finalColor1 = tex2D(MeshBackgroundSampler, In.TextureUV1);
	float4 finalColor2 = tex2D(MeshBackgroundSampler, In.TextureUV2);
	float4 finalColor3 = tex2D(MeshBackgroundSampler, In.TextureUV3);
	
	float4  finalColorOrig = tex2D ( MeshBackgroundSampler, In.TextureUV0+float2(g_finalImageDelta.x , g_finalImageDelta.y) );
	float	finalColorAlpha = pow((finalColor0.a - finalColor1.a), 2) + pow((finalColor2.a - finalColor3.a), 2);

	float4 edgeMaskAND = float4(1.0f, 1.0f , 1.0f , 1.0f);
	float4 edgeMaskOR =  float4(0.0f, 0.0f , 0.0f , 0.0f);
	
	//	( a 값이 바뀌는곳 && a 값이 0 이상인곳 )
	//	0 은 외곽선이 없는곳.
	//	1 : selection color
	//	2,3,4,5,6 : custom selection color
	if ( finalColorAlpha > 0.0f && finalColorOrig.a > 0.06f )
	{
		edgeMaskAND = float4(0.0f, 0.0f,0.0f,0);
		int  index = round(finalColorOrig.a*10);
		edgeMaskOR = g_colorIndicate[index] ;						//	target color. constant register로 index를 만들어 color를 index로 넘겨준다.
	}
	
	Output.RGBColor = (finalColorOrig * edgeMaskAND)+edgeMaskOR;

	return Output;
}

technique RenderFinalSceneBorder
{
    pass P0
    {          
        PixelShader  = compile ps_2_0 RenderFinalSceneBorderPS();
    }
}
