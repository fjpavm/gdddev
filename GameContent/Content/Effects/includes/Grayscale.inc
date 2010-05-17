#ifndef GRAYSCALE_SHADER_INCLUDE
#define GRAYSCALE_SHADER_INCLUDE

// grayscaling taken from http://bursjootech.blogspot.com/2008/06/grayscale-effect-pixel-shader-effect-in.html
shared float4 Grayscale(float4 inputColor){
	float gray = inputColor.r * 0.3 + inputColor.g * 0.59 + inputColor.b *0.11;    
	
	float4 outColor;
	    
	outColor.r = (inputColor.r - gray) * life + gray;
	outColor.g = (inputColor.g - gray) * life + gray;
	outColor.b = (inputColor.b - gray) * life + gray;
	outColor.a = inputColor.a;
	
	return outColor;
}

#endif