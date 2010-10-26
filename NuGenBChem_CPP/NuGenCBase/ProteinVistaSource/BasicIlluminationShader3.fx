//
//	Basic Illumination shader 3 file
//	
//	Target: Vertex, Pixel shader 3.0
//	Constant reg: #256
//	

float3	g_Light1Dir;                  // 1. Light's direction in world space
float3	g_Light2Dir;                  // 1. Light's direction in world space

float3  g_cameraPosInvWorld;				//	cameraPos * World_inverse, wireframe 에 사용.
float	g_wireframeLineWidth = 0.1f;	//	

float	g_Light1Intensity;
float	g_Light2Intensity;

float4	g_Light1Color;
float4	g_Light2Color;

int		g_numActiveLight = 0;

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

//	SM2
//	float2	g_vFog;						//	g_vFog.x = far/(far-near)
									//	g_vFog.y = -1/(far-near)

//	SM3
float3	g_fogParam;					//	x: enable.disable
									//	y: g_fogStart
									//	z: g_fogEnd
float4  g_fogColor;

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

float	g_farClipPlane;

#define SamplesSSAO	g_vBatchInstanceColor

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
	float3	oNormalView		: TEXCOORD3;		
	float3	oViewPos		: TEXCOORD4;		
	
	//	float1	bFog			: FOG;
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

	if ( g_numActiveLight == 0 )
		Out.DiffuseColor =	vColor;
	else
		Out.DiffuseColor =	vColor * 
							( ( g_Light1Intensity * g_Light1Color * g_intensityDiffuse * max ( g_intensityAmbient, dot(Out.NormalWorld, g_Light1Dir) ) ) +
							  ( g_Light2Intensity * g_Light2Color * g_intensityDiffuse * max ( g_intensityAmbient, dot(Out.NormalWorld, g_Light2Dir) ) ) ) ;

	Out.DiffuseColor.a = bSelect;
	
    float3 Normal = mul( vRotatedNorm, g_mWorldView );
	Out.oNormalView  = float4(0.5f*Normal+0.5f, 1.0f);
	Out.oViewPos = mul( vRotatedPos, g_mWorldView );
	
	return Out;
}

void SphereRenderingPhongPS(  VS_OUTPUT_SPHERE_VERTEX_PHONG In , 
							 out float4 OutColor: COLOR0, 
							 out float4 OutNormal: COLOR1, 
							 out float4 OutDepth: COLOR2 , uniform bool bPhong )
{
	clip ( CalcClipPlaneVisible(In.PosWorld) );

	float fSelect = In.DiffuseColor.a;
	float4 finalColor;
	if ( bPhong == true && g_numActiveLight != 0 )
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
	
	if ( g_fogParam.x == 1.0f )
	{
		//	Fog.
		float fogFLinear = saturate(g_fogParam.y  - length(In.oViewPos) * g_fogParam.z);
		finalColor.xyz = lerp( g_fogColor.xyz, finalColor.xyz , fogFLinear);
	}
	
	finalColor.a = fSelect;

	OutNormal = float4( In.oNormalView , 1.0f );

	//	z-value/(far-near)
	OutDepth = float4 ( In.oViewPos.zzz/(g_farClipPlane) , 1.0f );

	OutColor = finalColor;
}

//--------------------------------------------------------------//
// Technique Section for Effect Workspace.HLSL Illumination Effects.
//--------------------------------------------------------------//
technique SphereRenderingBatchPhong
{
	pass p0
	{
		VertexShader = compile vs_3_0 SphereRenderingPhongVS(true);
		PixelShader = compile ps_3_0 SphereRenderingPhongPS(true);
	}
}

technique SphereRenderingBatchGouraud
{
	pass p0
	{
		VertexShader = compile vs_3_0 SphereRenderingPhongVS(true);
		PixelShader = compile ps_3_0 SphereRenderingPhongPS(false);
	}
}

//--------------------------------------------------------------//
// Technique Section for Effect Workspace.HLSL Illumination Effects.
//--------------------------------------------------------------//
technique CylinderRenderingBatchPhong
{
	pass p0
	{
		VertexShader = compile vs_3_0 SphereRenderingPhongVS(false);
		PixelShader = compile ps_3_0 SphereRenderingPhongPS(true);
	}
}

technique CylinderRenderingBatchGouraud
{
	pass p0
	{
		VertexShader = compile vs_3_0 SphereRenderingPhongVS(false);
		PixelShader = compile ps_3_0 SphereRenderingPhongPS(false);
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
	
	float3	oNormalView		: TEXCOORD4;		
	float3	oViewPos		: TEXCOORD5;
	
	//	float1	bFog			: FOG;
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

	if ( g_numActiveLight == 0 )
		Out.DiffuseColor =	diffuseColor;
	else
		Out.DiffuseColor =	diffuseColor * 
							( ( g_Light1Intensity * g_Light1Color * g_intensityDiffuse * max ( g_intensityAmbient, dot(Out.NormalWorld, g_Light1Dir) ) ) +
							  ( g_Light2Intensity * g_Light2Color * g_intensityDiffuse * max ( g_intensityAmbient, dot(Out.NormalWorld, g_Light2Dir) ) ) );

	Out.DiffuseColor.a = bSelect;
	
	Out.TextureCoord = inTex;

    float3 Normal = mul( inNorm, g_mWorldView );
	Out.oNormalView  = float4(0.5f*Normal+0.5f, 1.0f);
	Out.oViewPos = mul( inPos, g_mWorldView );

	return Out;
}

void  RibbonRenderingPhongPS(  VS_OUTPUT_RIBBON_VERTEX_PHONG In , 
								out float4 OutColor: COLOR0, 
								out float4 OutNormal: COLOR1, 
								out float4 OutDepth: COLOR2 , 
								uniform bool bTexture , uniform bool bPhong)
{
	clip ( CalcClipPlaneVisible(In.PosWorld) );

	float fSelect = In.DiffuseColor.a;
	float4 finalColor;
	
	float4 textureColor = float4(1,1,1,0);
	if ( bTexture == true )
	{
		textureColor = tex2D(MeshTextureSampler, In.TextureCoord);
	}
	
	if ( bPhong == true && g_numActiveLight != 0 )
	{
		float3 H1 = normalize(g_Light1Dir + normalize(In.PixelToEye));
		float3 H2 = normalize(g_Light2Dir + normalize(In.PixelToEye));
		float3 N = normalize(In.NormalWorld);

		//	
		//  per pixel lighting.
		//	specular intensity.
		finalColor = In.DiffuseColor * textureColor+ 
					g_Light1Color * g_Light1Intensity * g_intensitySpecular * pow( max(0.0,dot(N,H1)), 30 ) + 
					g_Light2Color * g_Light2Intensity * g_intensitySpecular * pow( max(0.0,dot(N,H2)), 30 );
	}
	else
	{
		finalColor = In.DiffuseColor*textureColor;
	}
	
	if ( g_fogParam.x == 1.0f )
	{
		float fogFLinear = saturate(g_fogParam.y  - length(In.oViewPos) * g_fogParam.z);
		finalColor.xyz = lerp( g_fogColor.xyz , finalColor.xyz, fogFLinear );
	}
	
	finalColor.a = fSelect;

	OutNormal = float4( In.oNormalView , 1.0f );

	//	z-value/(far-near)
	OutDepth = float4 ( In.oViewPos.zzz/(g_farClipPlane) , 1.0f );

	OutColor = finalColor;
}

//--------------------------------------------------------------//
// Technique Section for Effect Workspace.HLSL Illumination Effects.
//--------------------------------------------------------------//
technique RibbonRenderingPhong
{
	pass p0
	{
		VertexShader = compile vs_3_0 RibbonRenderingPhongVS();
		PixelShader = compile ps_3_0 RibbonRenderingPhongPS(true,true);
	}
}

technique RibbonRenderingNoTexturePhong
{
	pass p0
	{
		VertexShader = compile vs_3_0 RibbonRenderingPhongVS();
		PixelShader = compile ps_3_0 RibbonRenderingPhongPS(false,true);
	}
}

technique RibbonRenderingGouraud
{
	pass p0
	{
		VertexShader = compile vs_3_0 RibbonRenderingPhongVS();
		PixelShader = compile ps_3_0 RibbonRenderingPhongPS(true,false);
	}
}

technique RibbonRenderingNoTextureGouraud
{
	pass p0
	{
		VertexShader = compile vs_3_0 RibbonRenderingPhongVS();
		PixelShader = compile ps_3_0 RibbonRenderingPhongPS(false,false);
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
	float3	oNormalView		: TEXCOORD1;		
	float3	oViewPos		: TEXCOORD2;
	
	//	float1	bFog : FOG;
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

		if ( lineIndex.x == 1.0 )
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

	Out.PosWorld = mul(inPos, g_mWorld);	//	Out.PosWorld의 w 값은 1.

	Out.Diff = diffuseColor;
	Out.Diff.a = lineIndex.y;
	
	Out.oNormalView  = float4(0,0,0,1.0f);
	Out.oViewPos = mul( inPos, g_mWorldView );
	
	if ( g_fogParam.x == 1.0f )
	{
		float fogFLinear = saturate(g_fogParam.y  - length(Out.oViewPos) * g_fogParam.z);
		Out.Diff.xyz = lerp( g_fogColor.xyz, Out.Diff.xyz, fogFLinear );
	}
	
	return Out;
}

void WireframeRenderingPhongPS (	VS_OUTPUT_DIFFUSE In,
									out float4 OutColor: COLOR0, 
									out float4 OutNormal: COLOR1, 
									out float4 OutDepth: COLOR2 )
{
	clip( CalcClipPlaneVisible(In.PosWorld) );
	

	OutNormal = float4( In.oNormalView , 1.0f );

	//	z-value/(far-near)
	OutDepth = float4 ( In.oViewPos.zzz/(g_farClipPlane) , 1.0f );
	
	OutColor = In.Diff;
}

//--------------------------------------------------------------//
// Technique Section for Effect Workspace.HLSL Illumination Effects.
//--------------------------------------------------------------//
technique WireframeRenderingPhongLineWidth	
{
	pass p0
	{
		VertexShader = compile vs_3_0 WireframeRenderingPhongVS(true);
		PixelShader = compile ps_3_0 WireframeRenderingPhongPS();
	}
}

technique WireframeRenderingPhong	
{
	pass p0
	{
		VertexShader = compile vs_3_0 WireframeRenderingPhongVS(false);
		PixelShader = compile ps_3_0 WireframeRenderingPhongPS();
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
	float3	oNormalView		: TEXCOORD3;		
	float3	oViewPos		: TEXCOORD4;		
	
	//  In Shader Model 3, Fog register is obsoleted.
	//	Convert SM3->SM2
	//	float1	bFog			: FOG;
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

	if ( g_bIndicate == true )
		diffuseColor = g_indicateDiffuseColor;

	if ( g_numActiveLight == 0 )
		Out.DiffuseColor =	diffuseColor;
	else
	{
		Out.DiffuseColor =	diffuseColor * 
							( ( g_Light1Intensity * g_Light1Color * g_intensityDiffuse * max ( g_intensityAmbient, dot(Out.NormalWorld, g_Light1Dir) ) ) +
							  ( g_Light2Intensity * g_Light2Color * g_intensityDiffuse * max ( g_intensityAmbient, dot(Out.NormalWorld, g_Light2Dir) ) ) );
	}
	
    float3 Normal = mul( inNorm, g_mWorldView );
	Out.oNormalView  = float4(0.5f*Normal+0.5f, 1.0f);
	Out.oViewPos = mul( inPos, g_mWorldView );
	
	return Out;
}

void SurfaceRenderingPhongPS(	VS_OUTPUT_SURFACE_VERTEX_PHONG In , 
								out float4 OutColor: COLOR0 , 
								out float4 OutNormal: COLOR1 , 
								out float4 OutDepth: COLOR2 , 
								uniform bool bUseAlpha, uniform bool bPhong )
{
	clip ( CalcClipPlaneVisible(In.PosWorld) );
	
	float4 finalColor;
	if ( bPhong == true && g_numActiveLight != 0 )
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
	
	if ( g_fogParam.x == 1.0f )
	{
		//	Fog.
		float fogFLinear = saturate(g_fogParam.y  - length(In.oViewPos) * g_fogParam.z);
		finalColor.xyz = lerp( g_fogColor.xyz, finalColor.xyz , fogFLinear);
	}
	
	//    pixel transparency: uniform
	if ( bUseAlpha == true )
	{
		finalColor.a = g_alpha;
	}
	
	OutNormal = float4( In.oNormalView , 1.0f );

	//	z-value/(far-near)
	OutDepth = float4 ( In.oViewPos.zzz/(g_farClipPlane) , 1.0f );
	
	OutColor = finalColor;
}

technique SurfaceRenderingNoAlphaPhong
{
	pass P0
	{
		VertexShader = compile vs_3_0 SurfaceRenderingPhongVS();
		PixelShader  = compile ps_3_0 SurfaceRenderingPhongPS(false,true);
	}
}

technique SurfaceRenderingWithAlphaPhong
{
	pass P0
	{
		//    AlphaBlendEnable = true;
		SrcBlend = SrcAlpha;
		DestBlend = InvSrcAlpha;

		VertexShader = compile vs_3_0 SurfaceRenderingPhongVS();
		PixelShader  = compile ps_3_0 SurfaceRenderingPhongPS(true,true);
	}
}

technique SurfaceRenderingNoAlphaGouraud
{
	pass P0
	{
		VertexShader = compile vs_3_0 SurfaceRenderingPhongVS();
		PixelShader  = compile ps_3_0 SurfaceRenderingPhongPS(false,false);
	}
}

technique SurfaceRenderingWithAlphaGouraud
{
	pass P0
	{
		//    AlphaBlendEnable = true;
		SrcBlend = SrcAlpha;
		DestBlend = InvSrcAlpha;

		VertexShader = compile vs_3_0 SurfaceRenderingPhongVS();
		PixelShader  = compile ps_3_0 SurfaceRenderingPhongPS(true,false);
	}
}

////////////////////////////////////////////////////////////////////////////////////////////
//
//	Axis를 그린다.
//
////////////////////////////////////////////////////////////////////////////////////////////

struct VS_OUTPUT_AXIS
{
	float4 Position		: POSITION;   // vertex position
	float4 Diffuse		: COLOR0;     // vertex diffuse color
	
	float3	oNormalView		: TEXCOORD0;
	float3	oViewPos		: TEXCOORD1;
	float4	PosWorld		: TEXCOORD2;
};

VS_OUTPUT_AXIS	AxisRenderingVS( float4 vPos : POSITION, float3 inNorm : NORMAL )
{
	VS_OUTPUT_AXIS	Output = (VS_OUTPUT_AXIS)0;

	Output.Position = mul(vPos, g_mWorldViewProjection);

	float3 vNormal = normalize( mul( inNorm , (float3x3)g_mWorld) );
	Output.Diffuse = g_MaterialDiffuseColor * ( max(0.2,dot(vNormal, g_Light1Dir)) + max(0.2,dot(vNormal, g_Light2Dir)) );
	Output.Diffuse.a = 0.0f;

    float3 Normal = mul( inNorm, g_mWorldView );
	Output.oNormalView  = float4(0.5f*Normal+0.5f, 1.0f);
	Output.oViewPos = mul( vPos, g_mWorldView );
	
	Output.PosWorld =	mul(vPos, g_mWorld);						//	clipping, Out.PosWorld의 w 값은 1.

	if ( g_fogParam.x == 1.0f )
	{
		float fogFLinear = saturate(g_fogParam.y  - length(Output.oViewPos) * g_fogParam.z);
		Output.Diffuse.xyz = lerp( g_fogColor.xyz, Output.Diffuse.xyz, fogFLinear );
	}

	return Output;
}

void AxisRenderingPS ( VS_OUTPUT_AXIS	In ,
						out float4 OutColor: COLOR0, 
						out float4 OutNormal: COLOR1, 
						out float4 OutDepth: COLOR2 )
{
	clip ( CalcClipPlaneVisible(In.PosWorld) );
	
	OutNormal = float4( In.oNormalView , 1.0f );

	//	z-value/(far-near)
	OutDepth = float4 ( In.oViewPos.zzz/(g_farClipPlane) , 1.0f );

	OutColor = In.Diffuse;
}

technique AxisRendering
{
	pass P0
	{
		VertexShader = compile vs_3_0 AxisRenderingVS();
		PixelShader  = compile ps_3_0 AxisRenderingPS();
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
	pass p0
	{
		VertexShader = compile vs_3_0 HLSL_LineWireframeRendering_VS();
		PixelShader = compile ps_3_0 HLSL_LineWireframeRendering_PS();
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
	//	float3	oNormalView		: TEXCOORD0;		
	//	float3	oViewPos		: TEXCOORD1;		
	//	float1 bFog			: FOG;
};

VS_OUTPUT_PLANE	ClipPlaneVS ( float4 vPos : POSITION, float3 inNorm : NORMAL , uniform bool bAlpha )
{
	VS_OUTPUT_PLANE	Output = (VS_OUTPUT_PLANE)0;

	// Transform the position from object space to homogeneous projection space
	Output.Position = mul(vPos, g_mWorldViewProjection);
	
	float3 vNormal = normalize( mul( inNorm , (float3x3)g_mWorld) );
	Output.Diffuse = g_MaterialDiffuseColor * ( max(0.2,dot(vNormal, g_Light1Dir)) + max(0.2,dot(vNormal, g_Light2Dir)) );

	if ( bAlpha == true )
		Output.Diffuse.a = g_alpha;
	else
		Output.Diffuse.a = 0.0f;

    //	float3 Normal = mul( inNorm, g_mWorldView );
	//	Output.oNormalView  = float4(0.5f*Normal+0.5f, 1.0f);
	//	Output.oViewPos = mul( vPos, g_mWorldView );

	if ( g_fogParam.x == 1.0f )
	{
		float4 posView = mul(vPos, g_mWorldView);
		float fogFLinear = saturate(g_fogParam.y  - length(posView) * g_fogParam.z);
		Output.Diffuse.xyz = lerp( g_fogColor.xyz, Output.Diffuse.xyz, fogFLinear );
	}

	return Output;
}

void ClipPlanePS( VS_OUTPUT_PLANE	In ,
					out float4 OutColor: COLOR0, 
					out float4 OutNormal: COLOR1, 
					out float4 OutDepth: COLOR2 ) 
{
	//	OutNormal = float4( In.oNormalView , 1.0f );

	//	z-value/(far-near)
	//	OutDepth = float4 ( In.oViewPos.zzz/(g_farClipPlane) , 1.0f );

	OutColor = In.Diffuse;
}

technique ClipPlaneRenderingNoAlpha
{
	pass P0
	{
		VertexShader = compile vs_3_0 ClipPlaneVS(false);
		PixelShader  = compile ps_3_0 ClipPlanePS();
	}
}

technique ClipPlaneRenderingWithAlpha
{
	pass P0
	{
		AlphaBlendEnable = true;
		SrcBlend = SrcAlpha;
		DestBlend = InvSrcAlpha;

		VertexShader = compile vs_3_0 ClipPlaneVS(true);
		PixelShader  = compile ps_3_0 ClipPlanePS();
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
	pass p0
	{
		VertexShader = compile vs_3_0 SkyBoxRenderingVS();
		PixelShader = compile ps_3_0 SkyBoxRenderingPS();
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
	//	float1 bFog			: FOG;
};

VS_OUTPUT_TEXT3D	Text3DVS ( 
								float4	inPos			: POSITION,
								float3	inNorm			: NORMAL,
								float4	diffuseColor	: COLOR0,
								float2	inTex			: TEXCOORD0,
								uniform bool bAlpha )
{
	VS_OUTPUT_TEXT3D	Output = (VS_OUTPUT_TEXT3D)0;

	// Transform the position from object space to homogeneous projection space
	Output.Position = mul(inPos, g_mWorldViewProjection);
	Output.Diffuse = diffuseColor;

	Output.TextureCoord = inTex;
	
	if ( bAlpha == true )
 		Output.Diffuse.a = g_alpha;
 	else
 		Output.Diffuse.a = 1.0f;

	if ( g_fogParam.x == 1.0f )
	{
		float4 posView = mul(inPos, g_mWorldView);
		float fogFLinear = saturate(g_fogParam.y  - length(posView) * g_fogParam.z);
		Output.Diffuse.xyz = lerp( g_fogColor.xyz, Output.Diffuse.xyz, fogFLinear );
	}

	return Output;
}

void Text3DPS( VS_OUTPUT_TEXT3D	In ,
				out float4 OutColor: COLOR0 )
{
	float4 finalColor = tex2D(MeshTextureSampler, In.TextureCoord) * In.Diffuse;
	OutColor = finalColor;
}

technique Text3DRenderingNoAlpha
{
	pass P0
	{
		VertexShader = compile vs_3_0 Text3DVS(false);
		PixelShader  = compile ps_3_0 Text3DPS();
	}
}

technique Text3DRenderingWithAlpha
{
	pass P0
	{
		AlphaBlendEnable = true;
		SrcBlend = SrcAlpha;
		DestBlend = InvSrcAlpha;

		VertexShader = compile vs_3_0 Text3DVS(true);
		PixelShader  = compile ps_3_0 Text3DPS();
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
        PixelShader  = compile ps_3_0 RenderFinalSceneBorderPS();
    }
}

//=========================================================================================================
//==SSAO=======================================================================================================
//=========================================================================================================

// Screen Space Ambient Occlusion shaders
// Ambient Occlusion Map pass

float		g_camFrustumTopLeft[2];
float4x4	g_matProj;

// uniform samplers
texture			textureDepth;
texture			textureNormal;
texture			textureNoise;
texture			textureColor;

//	SSAO param
float			g_ssaoParam[3];
//	float			g_aoRange;
//	float			g_occlusionScale;
//	int		g_numSampleSSAO = 16;

sampler2D ColorTex = sampler_state
{
	Texture = textureColor ;
	ADDRESSU = CLAMP;
	ADDRESSV = CLAMP;
	MAGFILTER = LINEAR;
	MINFILTER = LINEAR;
};

sampler2D Normal = sampler_state
{
	Texture = textureNormal ;
	ADDRESSU = CLAMP;
	ADDRESSV = CLAMP;
	MAGFILTER = LINEAR;
	MINFILTER = LINEAR;
};

sampler2D DepthMap = sampler_state
{
	Texture = textureDepth ;
	ADDRESSU = CLAMP;
	ADDRESSV = CLAMP;
	MAGFILTER = LINEAR;
	MINFILTER = LINEAR;
};

sampler2D NoiseTex = sampler_state
{
	Texture = textureNoise;
	ADDRESSU = WRAP;
	ADDRESSV = WRAP;
	MAGFILTER = LINEAR;
	MINFILTER = LINEAR;
};

void SSAO_VS (	in	float4 InPos  : POSITION ,
				in	float4 InTex  : TEXCOORD0,
			    out	float4 OutPos	: POSITION,
				out	float2 OutTex	: TEXCOORD0,
				out	float3 OutEyeRay : TEXCOORD1
				)
{
	OutPos = InPos;
	OutTex = InTex;
	
	//	[0..1]의 xy 값과 texture의 depth를 가지고 3차원 좌표를 복원
	//	[0..1] -> [-1..1] for x and y
    InPos.xy = sign(InPos.xy);
    
    // Eye ray is from origin to corner of camera frustum
    OutEyeRay.x = InPos.x * g_camFrustumTopLeft[0];
    OutEyeRay.y = InPos.y * g_camFrustumTopLeft[1];
    OutEyeRay.z = g_farClipPlane;
}

// Pixel shader that creates the Ambient Occlusion map
float4 SSAO_PS	( float2 uv0 : TEXCOORD0 , float3 eyeRay : TEXCOORD1 ) : COLOR
{
// 	float4 SamplesSSAO[16] =
// 	{
// 		float4(0.355512, 	-0.709318, 	-0.102371,	0.0 ),
// 		float4(0.534186, 	0.71511, 	-0.115167,	0.0 ),
// 		float4(-0.87866, 	0.157139, 	-0.115167,	0.0 ),
// 		float4(0.140679, 	-0.475516, 	-0.0639818,	0.0 ),
// 		float4(-0.0796121, 	0.158842, 	-0.677075,	0.0 ),
// 		float4(-0.0759516, 	-0.101676, 	-0.483625,	0.0 ),
// 		float4(0.12493, 	-0.0223423,	-0.483625,	0.0 ),
// 		float4(-0.0720074, 	0.243395, 	-0.967251,	0.0 ),
// 		float4(-0.207641, 	0.414286, 	0.187755,	0.0 ),
// 		float4(-0.277332, 	-0.371262, 	0.187755,	0.0 ),
// 		float4(0.63864, 	-0.114214, 	0.262857,	0.0 ),
// 		float4(-0.184051, 	0.622119, 	0.262857,	0.0 ),
// 		float4(0.110007, 	-0.219486, 	0.435574,	0.0 ),
// 		float4(0.235085, 	0.314707, 	0.696918,	0.0 ),
// 		float4(-0.290012, 	0.0518654, 	0.522688,	0.0 ),
// 		float4(0.0975089, 	-0.329594, 	0.609803,	0.0 )
// 	};

	//	Depth of pixel from eye as a percentage of camera far clip plane distance, so range is [0.0 , 1.0]
	//	near: 0, far: 1
	float depth = tex2D ( DepthMap , uv0 ).r;
	if ( depth >= 1.0f )
		return float4(1,1,1,1);

	// Pixel's position in eye space in world units
	float3 eyePos = depth * eyeRay;

	// Occlusion value to be summed up
	float occ = 0.0;
	
	// Normal sampler has normals in eyespace in compact form
	// Unpack normal components to lie between [-1.0, 1.0] range
	float3 origNorm = ( tex2D( Normal, uv0.xy ).rgb * 2.0 ) - float3( 1.0, 1.0, 1.0 );
			
	// Get a random normal from randomization texture, the multiplier (here 100.0) maybe could be selected more wisely?
	//	prime number:  2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97, 101, 103, 107, 109, 113, 127, 131, 137, 139, 149, 151, 157, 163, 167, 173, 179, 181, 191, 193, 197, 199, 211, 223, 227, 229, 233, 239, 241, 251, 257, 263, 269, 271 
	float3 randNormal = tex2D( NoiseTex, uv0.xy * 279.0f  ).rgb;
			
	// Unpack normal components to lie between [-1.0, 1.0] range
	randNormal = ( randNormal * 2.0 ) - float3( 1.0, 1.0, 1.0 );
		
	int i;
	float4 sn;
	float4 se;
	float4 ss;
	float sampleDepth;

	for ( i = 0; i < g_ssaoParam[2] ; i ++ )
	{
		float3 curSample;
		
		curSample = SamplesSSAO[ i ].xyz;
		
		// Get a sample by reflecting the constant SamplesSSAO[i].xyz value by a random normal vector for this pixel
		// Neighboring 128 pixels will get different random normal vectors
		curSample = reflect(curSample, randNormal.xyz);
		
		// If sample is in the lower hemisphere for the surface normal
		if( dot( curSample, origNorm.xyz) < 0.0 )
		{
			// Reflect it and put it in the upper hemisphere for the surface normal
			curSample = reflect(curSample, origNorm.xyz);
		}
		
		// Get this sample point's eye position in world space.
		se = float4( eyePos + curSample * g_ssaoParam[0] , 1.0 );
		
		//	
		//	Project back to screen space
		//	projection transform.. (xyz) = (-1..1),(-1,1),(0..1)
		//	
		sn = mul( se, g_matProj );
	
		//	
		//	[-1..1] --> [0..1]
		//	[-1..1] --> [1..0]
		// D3D textures require a flip in y-axis
		ss.xy = ( sn.xy/sn.w * float2( 0.5, -0.5 ) ) + float2( 0.5, 0.5 );
				
		// Take a depth sample for our random point
		//	sampleDepth = tex2D( DepthMap, ss.xy ).r * g_farClipPlane;
		sampleDepth = tex2Dlod( DepthMap, float4(ss.xy,0,1) ).r * g_farClipPlane;
					
		// Occlusion if sample vectors distance from the camera plane is larger
		// than stored value in depth buffer
		// Note that depth values are grow in -z direction 
		// therefore -->  ( se.z < sampleDepth )
				
		if ( se.z > sampleDepth )  
		{
			// Get depth difference
			float zd = se.z - sampleDepth;
			//	HORZ SSAO.
			//	float zd = atan((se.z - sampleDepth)/ length(curSample.xy * g_ssaoParam[0]) );

			// Quad falloff
			float falloff = 1.0 / ( 1.0 + ( zd * zd * 0.01 ) );
			
			// Sum
			occ += falloff;
		}
	}
	
	// Average
	float unOcclusion = 1.0 - min( (occ/g_ssaoParam[2])*g_ssaoParam[1], 1.0f );
		
	//	SSAO에 fog를 사용하지 않으면, 멀리 갔을때, SSAO의 명암만 두드러지게 나타난다.
	if ( g_fogParam.x == 1.0f )
	{
		float fogFLinear = saturate(g_fogParam.y  - length(eyePos) * g_fogParam.z);
		unOcclusion = lerp( 1.0f , unOcclusion, fogFLinear );
	}
		
	// Only first component matters
	return float4( unOcclusion, unOcclusion, unOcclusion , 1.0f );
}

technique SSAO_SM3
{
	pass P0
	{ 
		VertexShader =	compile vs_3_0		SSAO_VS();
		PixelShader =	compile ps_3_0		SSAO_PS();
		ZEnable = false;
	}
} 

//======================================================================================================================
//======================================================================================================================
//	Vert Blur

texture textureSSAO;

sampler2D TextureSSAO = sampler_state
{
	Texture = textureSSAO;
	ADDRESSU = CLAMP;
	ADDRESSV = CLAMP;
	MipFilter = NONE;
	MAGFILTER = LINEAR;
	MINFILTER = LINEAR;
};

//	16 box sampling.
//	blur texture size.
float g_blurTextureSize[2];	

// ------------------------------------------------------------
// 정점셰이더에서 픽셀셰이더로 넘기는 데이터
// ------------------------------------------------------------
struct VS_OUTPUT_BLUR_4
{
    float4 Pos			: POSITION;
	float2 Tex0			: TEXCOORD0;
	float2 Tex1			: TEXCOORD1;
	float2 Tex2			: TEXCOORD2;
	float2 Tex3			: TEXCOORD3;
};

// ------------------------------------------------------------
VS_OUTPUT_BLUR_4 Blur4VS ( float4 Pos    : POSITION ,float4 Tex    : TEXCOORD0	)
{
    VS_OUTPUT_BLUR_4 Out = (VS_OUTPUT_BLUR_4)0;        
    
    Out.Pos = Pos;

	Out.Tex0 = Tex + float2(-1.0f/g_blurTextureSize[0], -1.0f/g_blurTextureSize[1]);
	Out.Tex1 = Tex + float2(+1.0f/g_blurTextureSize[0], -1.0f/g_blurTextureSize[1]);
	Out.Tex2 = Tex + float2(-1.0f/g_blurTextureSize[0], +1.0f/g_blurTextureSize[1]);
	Out.Tex3 = Tex + float2(+1.0f/g_blurTextureSize[0], +1.0f/g_blurTextureSize[1]);
    	
    return Out;
}

// ------------------------------------------------------------
float4 Blur4PS(VS_OUTPUT_BLUR_4 In): COLOR0
{
	float4 t0 = tex2D(TextureSSAO, In.Tex0);
	float4 t1 = tex2D(TextureSSAO, In.Tex1);
	float4 t2 = tex2D(TextureSSAO, In.Tex2);
	float4 t3 = tex2D(TextureSSAO, In.Tex3);
	
	return (t0+t1+t2+t3)/4;
};

// ------------------------------------------------------------
// 정점셰이더에서 픽셀셰이더로 넘기는 데이터
// ------------------------------------------------------------
struct VS_OUTPUT_BLUR_16
{
    float4 Pos			: POSITION;
	float2 Tex0			: TEXCOORD0;
	float2 Tex1			: TEXCOORD1;
	float2 Tex2			: TEXCOORD2;
	float2 Tex3			: TEXCOORD3;
	float2 Tex4			: TEXCOORD4;
	float2 Tex5			: TEXCOORD5;
	float2 Tex6			: TEXCOORD6;
	float2 Tex7			: TEXCOORD7;	
};

// ------------------------------------------------------------
VS_OUTPUT_BLUR_16 Blur16VS ( float4 Pos    : POSITION ,float4 Tex    : TEXCOORD0	)
{
    VS_OUTPUT_BLUR_16 Out = (VS_OUTPUT_BLUR_16)0;        
    
    Out.Pos = Pos;

    Out.Tex0 = Tex + float2(3.0f/g_blurTextureSize[0], -3.0f/g_blurTextureSize[1]);
    Out.Tex1 = Tex + float2(3.0f/g_blurTextureSize[0], -1.0f/g_blurTextureSize[1]);
    Out.Tex2 = Tex + float2(3.0f/g_blurTextureSize[0], +1.0f/g_blurTextureSize[1]);
    Out.Tex3 = Tex + float2(3.0f/g_blurTextureSize[0], +3.0f/g_blurTextureSize[1]);
    Out.Tex4 = Tex + float2(1.0f/g_blurTextureSize[0], -3.0f/g_blurTextureSize[1]);
    Out.Tex5 = Tex + float2(1.0f/g_blurTextureSize[0], -1.0f/g_blurTextureSize[1]);
    Out.Tex6 = Tex + float2(1.0f/g_blurTextureSize[0], +1.0f/g_blurTextureSize[1]);
    Out.Tex7 = Tex + float2(1.0f/g_blurTextureSize[0], +3.0f/g_blurTextureSize[1]);

    return Out;
}

// ------------------------------------------------------------
float4 Blur16PS(VS_OUTPUT_BLUR_16 In): COLOR0
{
	float4 t0 = tex2D(TextureSSAO, In.Tex0);
	float4 t1 = tex2D(TextureSSAO, In.Tex1);
	float4 t2 = tex2D(TextureSSAO, In.Tex2);
	float4 t3 = tex2D(TextureSSAO, In.Tex3);
	
	float4 t4 = tex2D(TextureSSAO, In.Tex4);
	float4 t5 = tex2D(TextureSSAO, In.Tex5);
	float4 t6 = tex2D(TextureSSAO, In.Tex6);
	float4 t7 = tex2D(TextureSSAO, In.Tex7);
	
	float4 t8 = tex2D(TextureSSAO, In.Tex0 + float2(-4.0f/g_blurTextureSize[0], 0));
	float4 t9 = tex2D(TextureSSAO, In.Tex1 + float2(-4.0f/g_blurTextureSize[0], 0));
	float4 ta = tex2D(TextureSSAO, In.Tex2 + float2(-4.0f/g_blurTextureSize[0], 0));
	float4 tb = tex2D(TextureSSAO, In.Tex3 + float2(-4.0f/g_blurTextureSize[0], 0));
	
	float4 tc = tex2D(TextureSSAO, In.Tex4 + float2(-4.0f/g_blurTextureSize[0], 0));
	float4 td = tex2D(TextureSSAO, In.Tex5 + float2(-4.0f/g_blurTextureSize[0], 0));
	float4 te = tex2D(TextureSSAO, In.Tex6 + float2(-4.0f/g_blurTextureSize[0], 0));
	float4 tf = tex2D(TextureSSAO, In.Tex7 + float2(-4.0f/g_blurTextureSize[0], 0));
	
	return ((t0+t1+t2+t3)
		   +(t4+t5+t6+t7)
		   +(t8+t9+ta+tb)
		   +(tc+td+te+tf))/16;
};

// ------------------------------------------------------------
// ------------------------------------------------------------
technique Blur4pixel
{
    pass P0// 16박스필터 샘플링
    {
        // 셰이더
        VertexShader = compile vs_3_0 Blur4VS();
        PixelShader  = compile ps_3_0 Blur4PS();
    }
}

technique Blur16pixel
{
    pass P0// 16박스필터 샘플링
    {
        // 셰이더
        VertexShader = compile vs_3_0 Blur16VS();
        PixelShader  = compile ps_3_0 Blur16PS();
    }
}
//======================================================================================================================
//======================================================================================================================
//	FinalScene

texture textureColor1;
texture textureColor2;

sampler2D TextureColor1 = sampler_state
{
	Texture = textureColor1 ;
	ADDRESSU = CLAMP;
	ADDRESSV = CLAMP;
	MAGFILTER = LINEAR;
	MINFILTER = LINEAR;
};
sampler2D TextureColor2 = sampler_state
{
	Texture = textureColor2 ;
	ADDRESSU = CLAMP;
	ADDRESSV = CLAMP;
	MAGFILTER = LINEAR;
	MINFILTER = LINEAR;
};

float4 RenderFinalSceneTwoTexturePS ( float2 uv0: TEXCOORD0 ) : COLOR
{
    float4 vColor = tex2D( TextureColor1, uv0 ) * tex2D( TextureColor2, uv0 );
    return vColor;
}

float4 RenderFinalSceneOneTexturePS ( float2 uv0: TEXCOORD0 ) : COLOR
{
    float4 vColor = tex2D( TextureColor1, uv0 );
    return vColor;
}

technique RenderFinalSceneOneTexture
{
    pass p0
    {
        PixelShader = compile ps_3_0 RenderFinalSceneOneTexturePS();
    }
}

technique RenderFinalSceneTwoTexture
{
    pass p0
    {
        PixelShader = compile ps_3_0 RenderFinalSceneTwoTexturePS();
    }
}
