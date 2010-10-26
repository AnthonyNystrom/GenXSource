float4x4 mvpMatrix;

struct v2p {
	float4 Position	: POSITION;
	float3 Normal	: NORMAL;
};

struct p2f {
	float4 Position		: POSITION;
	float3 ObjectPos	: TEXCOORD0;
	float3 Normal		: TEXCOORD1;
};

void main(in v2p IN, out p2f OUT)
{
	OUT.Position = mul(IN.Position, mvpMatrix);
	OUT.ObjectPos = IN.Position.xyz;
	OUT.Normal = IN.Normal;
}