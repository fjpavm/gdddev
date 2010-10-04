#include "includes\CommonParameters.inc"
#include "includes\Grayscale.inc"

// TODO: add effect parameters here.
texture BackgroundTexture;

sampler BackgroundTextureSampler = 
sampler_state
{
    Texture = <BackgroundTexture>;
    MipFilter = LINEAR;
    MinFilter = LINEAR;
    MagFilter = LINEAR;
};

struct VertexShaderOutput
{
	float2 TextureUV  : TEXCOORD0;  // vertex texture coords	
};

float4 PixelShaderFunction(float2 texCoord: TEXCOORD0) : COLOR
{
	return Grayscale(tex2D(BackgroundTextureSampler, texCoord));    
}

technique BackgroundTechnique
{
    pass Pass1
    {
        // TODO: set renderstates here.
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
