sampler mainSampler : register(s0);


float BlurDistance = 0.2f;

struct PixelShaderInput
{
    float4 TextureCoords: TEXCOORD0;
};

float4 PixelShaderFunction(PixelShaderInput input) : COLOR0
{
		float4 color;
		float2 texCoord = input.TextureCoords;

		//color = tex2D(mainSampler, texCoord);
		
		color = tex2D(mainSampler, float2(texCoord.x+BlurDistance, texCoord.y+BlurDistance));
		color += tex2D(mainSampler, float2(texCoord.x+BlurDistance, texCoord.y-BlurDistance));
		color += tex2D(mainSampler, float2(texCoord.x-BlurDistance, texCoord.y+BlurDistance));
		color += tex2D(mainSampler, float2(texCoord.x-BlurDistance, texCoord.y-BlurDistance));

		color = color / 4;

		return color;
}



technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }

}