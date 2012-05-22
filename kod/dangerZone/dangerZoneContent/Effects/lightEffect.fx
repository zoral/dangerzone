texture pLightRenderTarget;
sampler mainSampler : register(s0);
sampler lightSampler = sampler_state{Texture = pLightRenderTarget;};

float BlurDistance = 0.002f;
float Threshold = 0.00;

struct PixelShaderInput
{
    float4 TextureCoords: TEXCOORD0;
};

float4 PixelShaderFunction(PixelShaderInput input) : COLOR0
{
        float2 texCoord = input.TextureCoords;

		
        float4 mainColor = tex2D(mainSampler, texCoord);
        float4 lightColor = tex2D(lightSampler, texCoord);
		
		return float4(mainColor.r, mainColor.g, mainColor.b, lightColor.r);

		float4 color = mainColor * lightColor;

		return saturate((color - Threshold) / (1-Threshold));

		
		//return color;
		


}

float4 BlurFunction(PixelShaderInput input) : COLOR0
{
		float4 color;
		float2 texCoord = input.TextureCoords;

		color = tex2D(mainSampler, texCoord);

		color += tex2D(mainSampler, float2(texCoord.x+BlurDistance, texCoord.y+BlurDistance));
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
	pass Pass2
	{
		PixelShader = compile ps_2_0 BlurFunction();
	}
}