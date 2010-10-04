#include "includes\Bones.inc"
#include "includes\CommonParameters.inc"
//------------------------------------------------------------------------------
// File: BasicRender.fx
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//------------------------------------------------------------------------------


//------------------------------------------------------------------------------
// Global variables
//------------------------------------------------------------------------------

float4x4 LightView;				// View matrix of light
float4x4 LightProj;				// Projection matrix of light

float4 LightDiffuse;				// Light's diffuse color
float4 LightAmbient;              // Light's ambient color

texture Texture;              // Color texture for mesh
texture ShadowMapTexture;			// Shadow map texture for lighting


//------------------------------------------------------------------------------
// Texture samplers
//------------------------------------------------------------------------------
sampler MeshTextureSampler = 
sampler_state
{
    Texture = <Texture>;
    MipFilter = LINEAR;
    MinFilter = LINEAR;
    MagFilter = LINEAR;
};
sampler ShadowMapSampler =
sampler_state
{
	Texture = <ShadowMapTexture>;
    MinFilter = LINEAR;
    MagFilter = LINEAR;
    MipFilter = LINEAR;
    AddressU = Clamp;
    AddressV = Clamp;
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

struct VS_SHADOW_OUTPUT
{
	float4 Position : POSITION;
	float Depth		: TEXCOORD0;
};
//------------------------------------------------------------------------------
// Utility function(s)
//------------------------------------------------------------------------------
float4x4 CreateLookAt(float3 cameraPos, float3 target, float3 up)
{
	float3 zaxis = normalize(cameraPos - target);
	float3 xaxis = normalize(cross(up, zaxis));
	float3 yaxis = cross(zaxis, xaxis);
	
	float4x4 view = { xaxis.x, yaxis.x, zaxis.x, 0,
		xaxis.y, yaxis.y, zaxis.y, 0,
		xaxis.z, yaxis.z, zaxis.z, 0,
		-dot(xaxis, cameraPos), -dot(yaxis, cameraPos),
		-dot(zaxis, cameraPos), 1
	};

	return view;
}

float4 GetPositionFromLight(float4 position)
{
	float4x4 WorldViewProjection =
	 mul(mul(World, LightView), LightProj);
	return mul(position, WorldViewProjection);  
}

VS_SHADOW_OUTPUT RenderShadowMapVS(VS_INPUT input, uniform bool skinTransform)
{
	VS_SHADOW_OUTPUT Out;
	float4 position = (float4)0;
	if(skinTransform)
	{
		float4x4 skinTransform = 0;
	    
		skinTransform += Bones[input.BoneIndices.x] * input.BoneWeights.x;
		skinTransform += Bones[input.BoneIndices.y] * input.BoneWeights.y;
		skinTransform += Bones[input.BoneIndices.z] * input.BoneWeights.z;
		skinTransform += Bones[input.BoneIndices.w] * input.BoneWeights.w;
	    
		// Skin the vertex position.
		position = mul(input.Position, skinTransform);
	}
	else{
		position = input.Position;
	}		
    
	Out.Position = GetPositionFromLight(position); 
	
	// Depth is Z/W.  This is returned by the pixel shader.
	// Subtracting from 1 gives us more precision in floating point.
	Out.Depth.x = /*1-*/(Out.Position.z/*/Out.Position.w*/);
	
	return Out;
}

float4 RenderShadowMapPS( VS_SHADOW_OUTPUT In ) : COLOR
{ 
	// The depth is Z divided by W. We return
	// this value entirely in a 32-bit red channel
	// using SurfaceFormat.Single.  This preserves the
	// floating-point data for finer detail.
	    
    return float4(In.Depth.x,0,0,1);
}

technique ShadowMapRenderAnimation
{
	pass P0
	{
		// These render states are necessary to get a shadow map.
		// You should consider resetting CullMode and AlphaBlendEnable
		// before you render your main scene.	
		CullMode = NONE;
		ZEnable = TRUE;
		ZWriteEnable = TRUE;
		AlphaBlendEnable = FALSE;
		
        VertexShader = compile vs_2_0 RenderShadowMapVS(true);
        PixelShader  = compile ps_2_0 RenderShadowMapPS();
	}
}

technique ShadowMapRenderStatic
{
	pass P0
	{
		// These render states are necessary to get a shadow map.
		// You should consider resetting CullMode and AlphaBlendEnable
		// before you render your main scene.	
		CullMode = NONE;
		ZEnable = TRUE;
		ZWriteEnable = TRUE;
		AlphaBlendEnable = FALSE;
		
        VertexShader = compile vs_2_0 RenderShadowMapVS(false);
        PixelShader  = compile ps_2_0 RenderShadowMapPS();	
	}
}