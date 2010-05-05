#include "includes\CommonParameters.inc"
#include "AmbientLight.fx"

shared float3 CameraPos;
shared float3 PointLightPositionsW[4];
shared float4 PointLightColors[4];
shared int PointLightCount;
shared float4 LightAmbient;
shared float4x4 InverseTransposeWorld;

//shared float4 AmbientMaterial = {0.3f, 0.3f, 0.3f, 1.0f};
shared float4 DiffuseMaterial = {0.5f, 0.5f, 0.5f, 1.0f};
shared float4 SpecularMaterial = {0.3f, 0.3f, 0.3, 1.0f};
shared float  SpecularPower = 6.0f;
//shared float4 AmbientLight = {0.3, 0.3, 0.3, 1.0f};
shared float4 SpecularLight = {0.3, 0.3, 0.3, 1.0f};
shared float3 gAttenuation012 = {0.1f, 0.02f, 0.01f};

shared float3 loopOneTime(float3 normalW, float3 posW, int index){
	float3 outColor = (float3)0;
	
    float3 lightVecW = normalize(PointLightPositionsW[index] - posW);

	// Diffuse Light Computation.
	float s = max(dot(normalW, lightVecW), 0.0f);
			
	float3 diffuse = s*(DiffuseMaterial*PointLightColors[index]).rgb;
	
	// Attentuation.
	float d = distance(PointLightPositionsW[index], posW);

	float A = gAttenuation012.x + gAttenuation012.y*d +	gAttenuation012.z*d*d;

	// Specular Light Computation.
	float3 toEyeW = normalize(CameraPos - posW);
	float3 reflectW = reflect(lightVecW, normalW);
	float t = pow(max(dot(reflectW, toEyeW), 0.0f), SpecularPower);
	float3 spec = t*(SpecularMaterial*SpecularLight).rgb;
	

	return (spec + diffuse) / A;
}


shared float3 loopFourTimes(float3 normalW, float3 posW){
	float3 color = (float3)0;
	for(int i = 0; i<4; i++){		
		color += loopOneTime(normalW, posW, i);
	}
	return color;
}

shared float3 loopThreeTimes(float3 normalW, float3 posW){
	float3 color = (float3)0;
	for(int i = 0; i<3; i++){
		color += loopOneTime(normalW, posW, i);
	}
	return color;
}	

shared float3 loopTwoTimes(float3 normalW, float3 posW){
	float3 color = (float3)0;
	for(int i = 0; i<2; i++){
		color += loopOneTime(normalW, posW, i);
	}
	return color;	
}

shared float4 GetAmbientLight(){
	// Ambient Light Computation.
	return AmbientMaterial*AmbientLight;    
}

shared float3 GetPointLightColor(float3 normalW, float3 worldPosition){
	float3 color = (float3)0;

	normalW = mul(float4(normalW, 0.0f), InverseTransposeWorld);
	
	if(PointLightCount == 3){
		color = loopFourTimes(normalW, worldPosition);
	}
	else if(PointLightCount == 2){
		color = loopThreeTimes(normalW, worldPosition);
	}
	else if(PointLightCount == 1){
		color = loopTwoTimes(normalW, worldPosition);
	}
	else if(PointLightCount == 0){
		color = loopOneTime(normalW, worldPosition, 0);   
	}
	color += GetAmbientLight();

	return color;
}

// Vertex shader input structure.
struct POINT_LIGHT_VS_INPUT
{
    float4 Position : POSITION0;
    float3 color : COLOR0;
    float3 Normal : NORMAL;
};


// Vertex shader output structure.
struct POINT_LIGHT_VS_OUTPUT
{
    float4 Position : POSITION0;
    float4 Color : COLOR0;
};


// Vertex shader program.
POINT_LIGHT_VS_OUTPUT PointLightVertexShader(POINT_LIGHT_VS_INPUT input)
{
    POINT_LIGHT_VS_OUTPUT output;
    
    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);
	float3 normalW = mul(float4(input.Normal, 0.0f), InverseTransposeWorld).rgb;
	normalW = normalize(normalW);
    
    output.Color.rgb = GetPointLightColor(normalW, output.Position);
    output.Color.a = 1.0f;
    
    return output;
}

// Pixel shader program.
float4 PointLightPixelShader(POINT_LIGHT_VS_OUTPUT input) : COLOR0
{
    return input.Color;
}

technique PointLightsTechnique
{
    pass PointLightTechnique
    {
        VertexShader = compile vs_2_0 PointLightVertexShader();
        PixelShader = compile ps_2_0 PointLightPixelShader();
    }
}