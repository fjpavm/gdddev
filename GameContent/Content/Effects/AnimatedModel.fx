#include "includes\CommonParameters.inc"
#include "includes\Bones.inc" 
#include "includes\Lighting.inc" 
#include "includes\ShadowMapInclude.inc"
#include "includes\Grayscale.inc"
#include "includes\PointLight.inc"

texture Texture;

sampler MeshTextureSampler = 
sampler_state
{
    Texture = <Texture>;
//     MipFilter = LINEAR;
//     MinFilter = LINEAR;
//     MagFilter = LINEAR;
	MaxAnisotropy = 16;
    MinFilter = Anisotropic;
    MagFilter = Anisotropic;
    MipFilter = LINEAR;
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
    float4 BoneIndices : BLENDINDICES0;
    float4 BoneWeights : BLENDWEIGHT0;
};

struct PS_INPUT
{
    float2 TextureUV  : TEXCOORD0;  // vertex texture coords   
    float4 LightPosition : TEXCOORD2;  
    float3 pointLightColor : TEXCOORD3;  
    bool inTheDark    : TEXCOORD4;
};

VS_OUTPUT AnimatedModelVertexShader(VS_INPUT input)
{
     VS_OUTPUT Output = (VS_OUTPUT) 0;

     float4 position = input.Position;
     float4 normal = float4(input.Normal, 1.0f);
     
	 // Blend between the weighted bone matrices.
	 float4x4 skinTransform = 0;
    
 	 skinTransform += Bones[input.BoneIndices.x] * input.BoneWeights.x;
	 skinTransform += Bones[input.BoneIndices.y] * input.BoneWeights.y;
	 skinTransform += Bones[input.BoneIndices.z] * input.BoneWeights.z;
	 skinTransform += Bones[input.BoneIndices.w] * input.BoneWeights.w;
    
	 // Skin the vertex position.
	 position = mul(input.Position, skinTransform);	
	 normal = mul(mul(input.Normal, skinTransform), World);
	 
	 
	 //transform the input position to the output
	 Output.Position = mul(position, mul(mul(World, View), Projection));
	 
	 //do not transform the position needed for the
	 //shadow map determination
	 Output.LightPosition = mul(input.Position, mul(mul(World, LightView), LightProj));
          
     //pass the texture coordinate as-is
	 Output.TextureUV = input.TexCoord;

	 Output.pointLightColor = GetPointLightColor(normal, mul(position, World)) + GetLighting(position, normal);
	 
	 // check if the normal is facing away from the lightsource
	 float angle = dot(normalize(LightDir), normal);
	 
	 Output.inTheDark = (angle < 0.0f);
	 
	 return Output;
}

float4 AnimatedModelPixelShader(PS_INPUT input) : COLOR0
{   	
	float4 shadow;
	//if(!input.inTheDark){
		shadow = ConsultShadowMap(input.LightPosition);	
	/*}
	else{
		shadow = float4(0.6f, 0.6f, 0.6f, 1.0f);
	}*/
	
    float4 lightDiffuse = (float4(input.pointLightColor, 1.0f)) * shadow;		
	float4 outColor = tex2D(MeshTextureSampler, input.TextureUV);
	
    return Grayscale(outColor * lightDiffuse);
}

technique AnimatedModelTechnique
{
    pass Pass1
    {
		AlphaBlendEnable = false;
		ZWriteEnable = true;
	    ZEnable = true;
	    
	    CullMode = CCW;
	    
        VertexShader = compile vs_2_0 AnimatedModelVertexShader();
        PixelShader = compile ps_2_0 AnimatedModelPixelShader();
    }
}

