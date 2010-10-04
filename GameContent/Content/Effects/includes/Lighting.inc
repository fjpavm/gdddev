#ifndef LIGHTING_SHADER_INCLUDE
#define LIGHTING_SHADER_INCLUDE

shared float3 LightDir;
shared float4 LightAmbient;
shared float4 LightDiffuse;

shared float4 GetLighting(float4 Position, float3 Normal){
	
	// Standard lighting equation
    float4 vTotalLightDiffuse = LightAmbient;
    //float3 lightDir = normalize(LightPos-Position);  // direction of light
    vTotalLightDiffuse += LightDiffuse * max(0,dot(Normal, LightDir)); 
    vTotalLightDiffuse.a = 1.0f;
    
    return vTotalLightDiffuse;
}

#endif