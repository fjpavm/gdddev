#ifndef BONES_SHADER_INCLUDE
#define BONES_SHADER_INCLUDE

// Maximum number of bone matrices we can render using shader 2.0 in a single pass.
// If you change this, update SkinnedModelProcessor.cs to match.
#define MaxBones 49
shared float4x4 Bones[MaxBones];


#endif