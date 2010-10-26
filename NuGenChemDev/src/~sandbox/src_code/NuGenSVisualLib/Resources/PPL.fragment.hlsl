float4 lightClr	:register(c0);
float4 lightDir	:register(c1);
float4 lightA	:register(c2);
float4 eyePos	:register(c3);

float4 matA		:register(c4);
float4 matE		:register(c5);
float4 matD		:register(c6);
float4 matS		:register(c7);

struct p2f {
	float4 Position		: TEXCOORD0;
	float4 Normal		: TEXCOORD1;
};

float4 main(in p2f IN)	: COLOR0
{
	float3 P = IN.Position;
	float3 N = normalize(IN.Normal);
	
	// compute emissive
	float3 emissive = matE.xyz;
	
	// compute ambient
	float3 ambient = matA.xyz * lightA.xyz;
	
	// compute diffuse
	float3 L = lightDir;
	float diffuseLight = max(dot(N,L), 0);
	float3 diffuse = matD.xyz * lightClr * diffuseLight;
	
	// compute specular
	float3 V = normalize(eyePos.xyz - P);
	float3 H = normalize(L + V);
	float specularLight = pow(max(dot(N,H), 0), matS.w);
	if (diffuseLight <= 0) specularLight = 0;
	float3 specular = matS.xyz * lightClr.xyz * specularLight;
	
	return float4(emissive + ambient + diffuse + specular, 1);
}