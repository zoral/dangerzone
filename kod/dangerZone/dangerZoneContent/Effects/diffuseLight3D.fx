float4x4 World;
float4x4 View;
float4x4 Projection;

float4 AmbientColor;
float AmbientIntensity;

float3 DiffuseDirection;
float4 DiffuseColor;
float DiffuseIntensity;

struct VertexShaderInput
{
    float4 Position : POSITION0;

};

struct VertexShaderOutput
{
    float4 Position : POSITION0;

    float3 Normal : TEXCOOORD0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input, float3 Normal : NORMAL)
{
    VertexShaderOutput output;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);

	float3 normal = normalize(mul(Normal, World));
	output.Normal = normal;
    
    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    float4 norm = float4(input.Normal, 1.0);
	float4 diffuse = saturate(dot(-DiffuseDirection, norm));

	return AmbientColor * AmbientIntensity+DiffuseIntensity*DiffuseColor*diffuse;
}

technique Technique1
{
    pass Pass1
    {

        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
