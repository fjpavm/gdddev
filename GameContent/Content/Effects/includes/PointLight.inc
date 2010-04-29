
shared float3 CameraPos;
shared float3 PointLightPositionsW[4];
shared float4 PointLightColors[4];
shared int PointLightCount;
shared float4x4 InverseTransposeWorld;

shared float4 AmbientMaterial = {0.3f, 0.3f, 0.3f, 1.0f};
shared float4 DiffuseMaterial = {0.5f, 0.5f, 0.5f, 1.0f};
shared float4 SpecularMaterial = {0.3f, 0.3f, 0.3, 1.0f};
shared float  SpecularPower = 6.0f;

shared float4 AmbientLight = {0.5, 0.5, 0.5, 1.0f};
shared float4 SpecularLight = {0.5, 0.5, 0.5, 1.0f};
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

	return color;
}
