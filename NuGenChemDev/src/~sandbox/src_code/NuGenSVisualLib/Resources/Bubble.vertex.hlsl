struct VS_OUTPUT 
{
   float4 pos:		POSITION;
   float2 texCoord:	TEXCOORD0;
   float4 reflection:	TEXCOORD1;
   float4 NdotV: 	TEXCOORD2;
};

VS_OUTPUT main(float4 position: POSITION, float4 normal: NORMAL, float2 texCoord: TEXCOORD,
				float4 tangent: TANGENT,
				uniform float4x4 viewProjMatrix,
				uniform float4x4 worldMatrix,
				uniform float4 cameraPos,
				uniform float time)
{
	VS_OUTPUT Out;
	
	const float TWO_PI = 6.2831853072;
	
	const float4 wave_directions_in_X	= {0, 2, 0, 4};		// relative to u
	const float4 wave_directions_in_Y	= {2, 0, 4, 0};		// relative to v

	const float4 waveSpeed			= {0.6, 0.7, 1.2, 1.4};
	const float4 waveHeights 		= {0.5, 0.5, 0.25, 0.25};

	// use texture coordinates as inputs to sinusoidal warp
	float4 wave_vec = frac(	wave_directions_in_X*texCoord.x +
				wave_directions_in_Y*texCoord.y + waveSpeed*time );
   	
   	// shift the texture coordinates to be in (pi, -pi) range
   	wave_vec = (wave_vec - 0.5) * TWO_PI;
   	
   	float4 wave_vec_sin = sin(wave_vec);
   	float4 wave_vec_cos = (2 - cos(wave_vec))*0.04;		// multiply by 0.04 as fix up factor
								
	// dot with waveHeights and then apply deformation in the direction of the normal
	wave_vec = dot(wave_vec_sin,waveHeights) * normal + position;
	wave_vec.w = 1; 	// homogeneous component
	
	// transform wave vector 
	Out.pos = mul(viewProjMatrix, wave_vec);
	
	// compute the binomial
	float4 binomial = float4(cross(tangent.xyz, normal.xyz), 1);

	// warp normal based on tangent and binomial vectors
	float4 warpedNormal = tangent  * dot(-(wave_vec_cos * waveHeights), wave_directions_in_Y) +
					binomial * dot(-(wave_vec_cos * waveHeights), wave_directions_in_X);
	warpedNormal = warpedNormal + normal;

	// transform and normalize the normal
	warpedNormal = normalize(mul(worldMatrix, warpedNormal));
	
	// compute a normalized view vector
	float4 viewVector = normalize(cameraPos - mul(worldMatrix, wave_vec));
	
	// compute the reflection vector: R = 2*N(N.V) - V
	Out.reflection = 2*dot(warpedNormal.xyz, viewVector.xyz)*warpedNormal - viewVector;
	
	// pass to the pixel shader N.V
	Out.NdotV = dot(warpedNormal.xyz, viewVector.xyz);
	
	// pass along texture coordinates
	Out.texCoord = texCoord;

	return Out;
}