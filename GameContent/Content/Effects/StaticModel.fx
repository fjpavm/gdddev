#include "includes\CommonParameters.inc"
#include "includes\PointLight.inc"
#include "includes\Lighting.inc" 
#include "includes\ShadowMapInclude.inc"
#include "includes\Grayscale.inc"

texture Texture;

sampler MeshTextureSampler = 
sampler_state
{
    Texture = <Texture>;
    MipFilter = LINEAR;
    MinFilter = LINEAR;
    MagFilter = LINEAR;
};

//------------------------------------------------------------------------------
// Vertex shader output structure
//------------------------------------------------------------------------------
struct VS_OUTPUT
{
    float4 Position   : POSITION0;   // vertex position 
    float2 TextureUV  : TEXCOORD0;  // vertex texture coords   
    float4 LightPosition : TEXCOORD2;  
    float3 pointLightColor : TEXCOORD3;  
    bool inTheDark    : TEXCOORD4;
};

// Vertex shader input structure.
struct VS_INPUT
{
    float4 Position : POSITION0;
    float3 Normal : NORMAL0;
    float2 TexCoord : TEXCOORD0;
};

struct PS_INPUT
{
    float2 TextureUV  : TEXCOORD0;  // vertex texture coords   
    float4 LightPosition : TEXCOORD2;  
    float3 pointLightColor : TEXCOORD3;  
    bool inTheDark    : TEXCOORD4;
};

VS_OUTPUT StaticModelVertexShader(VS_INPUT input)
{
	 VS_OUTPUT Output = (VS_OUTPUT) 0;
	 
	 //transform the input position to the output
	 Output.Position = mul(input.Position, mul(mul(World, View), Projection));
	 
	 //do not transform the position needed for the
	 //shadow map determination
	 Output.LightPosition = mul(input.Position, mul(mul(World, LightView), LightProj));
          
     //pass the texture coordinate as-is
	 Output.TextureUV = input.TexCoord;

	 Output.pointLightColor = GetPointLightColor(mul(input.Normal, World), mul(input.Position, World)) + GetLighting(input.Position, mul(input.Normal, World));
	 
	 // check if the normal is facing away from the lightsource
	 float angle = dot(normalize(LightDir), mul(input.Normal, World));
	 
	 Output.inTheDark = (angle < 0.0f);
	 
	 return Output;
}

float4 StaticModelPixelShader(PS_INPUT input) : COLOR0
{ 
    float4 shadow;
	if(!input.inTheDark){
		shadow = ConsultShadowMap(input.LightPosition);	
	}
	else{
		shadow = float4(0.6f, 0.6f, 0.6f, 1.0f);
	}
	
    float4 lightDiffuse = (float4(input.pointLightColor, 1.0f)) * shadow;
	float4 outColor = tex2D(MeshTextureSampler, input.TextureUV);
	
    return Grayscale(outColor * lightDiffuse);
}

technique StaticModelTechnique
{
    pass Pass1
    {
		AlphaBlendEnable = false;
		ZWriteEnable = true;
	    ZEnable = true;	    
    
        VertexShader = compile vs_2_0 StaticModelVertexShader();
        PixelShader = compile ps_2_0 StaticModelPixelShader();
    }
}

