#include "includes\CommonParameters.inc"
#include "includes\Grayscale.inc"

texture tex;

sampler TextureSampler = 
sampler_state
{
    Texture = <tex>;
    MipFilter = LINEAR;
    MinFilter = LINEAR;
    MagFilter = LINEAR;
};

struct VertexShaderInput
{
    float4 Position : POSITION0;
    float2 TextureUV  : TEXCOORD0;  
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
    float2 TextureUV  : TEXCOORD0;  
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput) 0;

    output.Position = mul(input.Position, mul(mul(World, View), Projection));
	output.TextureUV = input.TextureUV;

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    return Grayscale(tex2D(TextureSampler, input.TextureUV));    
}

technique TextureTechnique
{
    pass Pass1
    {
        AlphablendEnable = true;
       			
		SrcBlend = SrcAlpha;
		DestBlend = InvSrcAlpha;
		
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
