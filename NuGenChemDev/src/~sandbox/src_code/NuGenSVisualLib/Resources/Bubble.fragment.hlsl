sampler baseMap: register(s0);
sampler cubeMap: register(s1);

float4 main(float2 texCoord: TEXCOORD0, float4 reflection: TEXCOORD1, float4 NdotV: TEXCOORD2 ) : COLOR
{
	float4 modulatedCubeMap, result;

	float4 base = tex2D(baseMap, texCoord);			// sample base texture
	float4 cube = texCUBE(cubeMap, reflection);		// sample cubemap
	
	modulatedCubeMap.rgb = saturate(2 * base * cube);	// modulate cube map with base map
	modulatedCubeMap.a = (1-abs(NdotV))*0.6 - 0.01;		// compute fresnel term by scaling alpha,
								// multipling by N.V, and adding some bias
	// compute opacity from glow map
	float opacity = saturate(4*(cube.a*cube.a - 0.75));

	// linearly interpolate between (cubemap) and (base*cubemap) based on glow map
	result.rgb = lerp(modulatedCubeMap, cube, opacity);
	result.a = modulatedCubeMap.a + opacity;		// add fresnel term and glow map for alpha
	
	return result;
}