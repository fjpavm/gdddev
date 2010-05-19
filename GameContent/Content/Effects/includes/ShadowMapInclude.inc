#ifndef SHADOWMAP_SHADER_INCLUDE
#define SHADOWMAP_SHADER_INCLUDE

shared float4x4 LightView;				// View matrix of light
shared float4x4 LightProj;				// Projection matrix of light
shared int ID;
texture ShadowMapTexture;			// Shadow map texture for lighting

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

shared float4 ConsultShadowMap(uniform float4 lightingPosition){
    
    // Get the shadow map depth value for this pixel   
    float2 ShadowTexC = 
		0.5 * lightingPosition.xy / lightingPosition.w + float2( 0.5, 0.5 );
    ShadowTexC.y = 1.0f - ShadowTexC.y;

    float shadowdepth = tex2D(ShadowMapSampler, ShadowTexC).r;    
      
    // Check our value against the depth value
    float ourdepth = 1 - (lightingPosition.z / lightingPosition.w);
    
    
    // Check the shadowdepth against the depth of this pixel
    // a fudge factor is added to account for floating-point error
    float4 vTotalLightDiffuse = float4(1,1,1,1);	
	if (shadowdepth-0.006f > ourdepth)
	{
	    // we're in shadow, cut the light
		vTotalLightDiffuse.xyz -= 0.4;
	}

    return vTotalLightDiffuse;
}

#endif