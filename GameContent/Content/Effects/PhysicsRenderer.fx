#include "includes\\CommonParameters.inc"
#include "includes\\Bones.inc"

bool usingBones;

//------------------------------------------------------------------------------
// Vertex shader output structure
//------------------------------------------------------------------------------
struct VS_OUTPUT
{
    float4 Position : POSITION0;
};

// Vertex shader input structure.
struct VS_INPUT
{
    float4 Position : POSITION0;
    float4 BoneIndices : BLENDINDICES0;
    float4 BoneWeights : BLENDWEIGHT0;
};

VS_OUTPUT VertexShaderFunction(VS_INPUT input)
{
    VS_OUTPUT output;
    
    float4 position = input.Position;
    
    if(usingBones){
		// Blend between the weighted bone matrices.
		float4x4 skinTransform = 0;

		skinTransform += Bones[input.BoneIndices.x] * input.BoneWeights.x;
		skinTransform += Bones[input.BoneIndices.y] * input.BoneWeights.y;
		skinTransform += Bones[input.BoneIndices.z] * input.BoneWeights.z;
		skinTransform += Bones[input.BoneIndices.w] * input.BoneWeights.w;

		// Skin the vertex position.
		position = mul(position, skinTransform);	
    }

    float4 worldPosition = mul(position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);
    
    return output;
}

float4 PixelShaderFunction(VS_OUTPUT input) : COLOR0
{
    // TODO: add your pixel shader code here.
    return float4(1, 0, 0, 1);
}

technique PhysicsRenderer
{
    pass Pass1
    {
        // TODO: set renderstates here.

        VertexShader = compile vs_1_1 VertexShaderFunction();
        PixelShader = compile ps_1_1 PixelShaderFunction();
    }
}
