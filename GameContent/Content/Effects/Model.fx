float4x4 World;
float4x4 View;
float4x4 Projection;
float4x4 InverseTransposeWorld;

float3 CameraPos;
float3 PointLightPositionsW[4];
float4 PointLightColors[4];
int PointLightCount;

float4 AmbientMaterial = {0.3f, 0.3f, 0.3f, 1.0f};
float4 DiffuseMaterial = {0.5f, 0.5f, 0.5f, 1.0f};
float4 SpecularMaterial = {0.8f, 0.8f, 0.8, 1.0f};
float  SpecularPower = 6.0f;
float4 AmbientLight = {0.3, 0.3, 0.3, 1.0f};
float4 SpecularLight = {0.8, 0.8, 0.8, 1.0f};
float3 gAttenuation012 = {0.01f, 0.2f, 0.0f};

struct VertexShaderInput
{
    float4 Position : POSITION0;    
    float3 Normal : NORMAL; 
    float4 Color: COLOR;   
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
    float4 Color: COLOR;   
};

float4 loopOneTime(float3 normalW, float3 posW, int index){
	float4 outColor = (float4)0;
		
    float3 lightVecW = normalize(PointLightPositionsW[index]-posW);

	// Diffuse Light Computation.
	float s = max(dot(normalW, lightVecW), 0.0f);
			
	float3 diffuse = s*(DiffuseMaterial*PointLightColors[index]).rgb;

	// Specular Light Computation.
	float3 toEyeW   = normalize(CameraPos - posW);
	float3 reflectW = reflect(lightVecW, normalW);
	float t = pow(max(dot(reflectW, toEyeW), 0.0f), SpecularPower);
	float3 spec = t*(SpecularMaterial*SpecularLight).rgb;

	// Attentuation.
	float d = distance(PointLightPositionsW[index], posW);
	
	float A = gAttenuation012.x + gAttenuation012.y*d +
								gAttenuation012.z*d*d;
								
	// Everything togethe
	float3 color = (diffuse + spec) / A;

	// Pass on color and diffuse material alpha.
	outColor = float4(color, PointLightColors[index].a);
	
	return outColor;
}

float4 loopFourTimes(float3 normalW, float3 posW){
	float4 color = (float4)0;
	for(int i = 0; i<4; i++){		
		color += loopOneTime(normalW, posW, i);
	}
	return color;
}

float4 loopThreeTimes(float3 normalW, float3 posW){
	float4 color = (float4)0;
	for(int i = 0; i<3; i++){
		color += loopOneTime(normalW, posW, i);
	}
	return color;
}

float4 loopTwoTimes(float3 normalW, float3 posW){
	float4 color = (float4)0;
	for(int i = 0; i<2; i++){
		color += loopOneTime(normalW, posW, i);
	}
	return color;	
}

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput) 0;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);
	float3 normalW = mul(float4(input.Normal, 0.0f), InverseTransposeWorld);
	normalW = normalize(normalW);
    
    if(PointLightCount == 3){
		output.Color = loopFourTimes(normalW, worldPosition);
	}
	else if(PointLightCount == 2){
		output.Color = loopThreeTimes(normalW, worldPosition);
	}
	else if(PointLightCount == 1){
		output.Color = loopTwoTimes(normalW, worldPosition);
	}
	else if(PointLightCount == 0){
		output.Color = loopOneTime(normalW, worldPosition, 0);   
	}
	
	// Ambient Light Computation.
	float3 ambient = (AmbientMaterial*AmbientLight).rgb;
    
    output.Color.xyz += ambient.xyz;

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    return input.Color;
}

technique Model
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
