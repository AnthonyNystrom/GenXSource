// -------------------------------------------------------------
// Those settings are (and must be) set by the host program
// -------------------------------------------------------------
//#define TAPS 5
#define TAPS 7
//#define TAPS 9

// -------------------------------------------------------------
// Texture & Sampler
// -------------------------------------------------------------
texture texTexture : TEXTURE0;
sampler sampTexture = sampler_state {
	Texture = (texTexture);
    AddressU = Clamp;
    AddressV = Clamp;
};

// -------------------------------------------------------------
// Parameters
// -------------------------------------------------------------
const float centerTapWeight;
const float2 texelSize;
#if TAPS == 5
	const float tapOffsets[4] = {-2, -1, 1, 2};
	const float4 tapWeights;
#endif	
#if TAPS == 7
	const float tapOffsets[3] = {1, 2, 3};
	const float3 tapWeights;
#endif
#if TAPS == 9
	const float tapOffsets[4] = {1, 2, 3, 4};
	const float4 tapWeights;
#endif

// -------------------------------------------------------------
// Input/Output channels
// -------------------------------------------------------------
struct VS_INPUT
{
	float4 position : POSITION;
	float2 texCoord : TEXCOORD0;
};
struct VS_OUTPUT
{
	float4 position : POSITION;
	float2 centerTap : TEXCOORD0;	
	#if TAPS == 5
		float2 taps[4] : TEXCOORD1;
	#endif
	#if TAPS == 7
		float2 positiveTaps[3] : TEXCOORD1;
		float2 negativeTaps[3] : TEXCOORD4;		
	#endif
	#if TAPS == 9
		// Doesn't fit in float2 format!
		float4 positiveTaps[2] : TEXCOORD1;
		float4 negativeTaps[2] : TEXCOORD3;
	#endif		
};
#define PS_INPUT VS_OUTPUT

// -------------------------------------------------------------
// Vertex Shader
// -------------------------------------------------------------
VS_OUTPUT VS(const VS_INPUT IN, uniform float2 DIRECTION)
{
	VS_OUTPUT OUT;
	OUT.position = IN.position;
	OUT.centerTap = IN.texCoord;	
	
	#if TAPS == 5
		for(int i=0; i<4; i++)
			OUT.taps[i] = IN.texCoord + tapOffsets[i] * DIRECTION * texelSize;
	#endif
	#if TAPS == 7
		for(int i=0; i<3; i++)
		{
			OUT.positiveTaps[i] = IN.texCoord + tapOffsets[i] * DIRECTION * texelSize;
			OUT.negativeTaps[i] = IN.texCoord - tapOffsets[i] * DIRECTION * texelSize;
		}		
	#endif		
	#if TAPS == 9
		for(int i=0; i<2; i++)
		{
			OUT.positiveTaps[i].xy = IN.texCoord + tapOffsets[i*2] * DIRECTION * texelSize;
			OUT.negativeTaps[i].xy = IN.texCoord - tapOffsets[i*2] * DIRECTION * texelSize;
			OUT.positiveTaps[i].zw = IN.texCoord + tapOffsets[i*2+1] * DIRECTION * texelSize;
			OUT.negativeTaps[i].zw = IN.texCoord - tapOffsets[i*2+1] * DIRECTION * texelSize;			
		}		
	#endif
	
	return OUT;
}

// -------------------------------------------------------------
// Pixel Shader function
// -------------------------------------------------------------
float4 PS(PS_INPUT IN) : COLOR
{
	#if TAPS == 5
		float4x4 samples;
	#endif	
	#if TAPS == 7
		float3x4 positiveSamples;
		float3x4 negativeSamples;
	#endif
	#if TAPS == 9
		float4x4 positiveSamples;
		float4x4 negativeSamples;
	#endif
	
	#ifdef TAPS
		float4 color = tex2D(sampTexture, IN.centerTap) * centerTapWeight;
	#endif

	#if TAPS == 5
		for(int i=0; i<4; i++)
			samples[i] = tex2D(sampTexture, IN.taps[i]);
	#endif
	#if TAPS == 7
		for(int i=0; i<3; i++)
		{
			positiveSamples[i] = tex2D(sampTexture, IN.positiveTaps[i]);
			negativeSamples[i] = tex2D(sampTexture, IN.negativeTaps[i]);
		}
	#endif
	#if TAPS == 9
		for(int i=0; i<2; i++)
		{
			positiveSamples[i*2] = tex2D(sampTexture, IN.positiveTaps[i].xy);
			negativeSamples[i*2] = tex2D(sampTexture, IN.negativeTaps[i].xy);
			positiveSamples[i*2+1] = tex2D(sampTexture, IN.positiveTaps[i].zw);
			negativeSamples[i*2+1] = tex2D(sampTexture, IN.negativeTaps[i].zw);		
		}
	#endif
	
	#if TAPS == 5
		color += mul(tapWeights, samples);
	#else
		#ifdef TAPS
			color += mul(tapWeights, positiveSamples) + mul(tapWeights, negativeSamples);	
		#endif
	#endif
	
	#ifdef TAPS
		return color;
	#else
		return 1;
	#endif
}

// -------------------------------------------------------------
// Techniques
// -------------------------------------------------------------
technique TSM3
{
	pass HBlur
	{
		VertexShader = compile vs_3_0 VS(float2(1, 0));
		PixelShader = compile ps_3_0 PS();
	}
	pass VBlur
	{
		VertexShader = compile vs_3_0 VS(float2(0, 1));
		PixelShader = compile ps_3_0 PS();
	}		
}
technique TSM2a
{
	pass HBlur
	{
		VertexShader = compile vs_2_0 VS(float2(1, 0));
		PixelShader = compile ps_2_a PS();
	}
	pass VBlur
	{
		VertexShader = compile vs_2_0 VS(float2(0, 1));
		PixelShader = compile ps_2_a PS();
	}		
}
technique TSM2b
{
	pass HBlur
	{
		VertexShader = compile vs_2_0 VS(float2(1, 0));
		PixelShader = compile ps_2_b PS();
	}
	pass VBlur
	{
		VertexShader = compile vs_2_0 VS(float2(0, 1));
		PixelShader = compile ps_2_b PS();
	}		
}
technique TSM2
{
	pass HBlur
	{
		VertexShader = compile vs_2_0 VS(float2(1, 0));
		PixelShader = compile ps_2_0 PS();
	}
	pass VBlur
	{
		VertexShader = compile vs_2_0 VS(float2(0, 1));
		PixelShader = compile ps_2_0 PS();
	}		
}