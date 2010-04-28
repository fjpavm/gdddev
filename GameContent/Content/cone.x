xof 0303txt 0032


template VertexDuplicationIndices { 
 <b8d65549-d7c9-4995-89cf-53a9a8b031e3>
 DWORD nIndices;
 DWORD nOriginalVertices;
 array DWORD indices[nIndices];
}
template XSkinMeshHeader {
 <3cf169ce-ff7c-44ab-93c0-f78f62d172e2>
 WORD nMaxSkinWeightsPerVertex;
 WORD nMaxSkinWeightsPerFace;
 WORD nBones;
}
template SkinWeights {
 <6f0d123b-bad2-4167-a0d0-80224f25fabb>
 STRING transformNodeName;
 DWORD nWeights;
 array DWORD vertexIndices[nWeights];
 array float weights[nWeights];
 Matrix4x4 matrixOffset;
}

Frame RootFrame {

  FrameTransformMatrix {
    1.000000,0.000000,0.000000,0.000000,
    0.000000,1.000000,0.000000,0.000000,
    0.000000,0.000000,-1.000000,0.000000,
    0.000000,0.000000,0.000000,1.000000;;
  }
  Frame Cone {

    FrameTransformMatrix {
      1.000000,0.000000,0.000000,0.000000,
      0.000000,1.000000,0.000000,0.000000,
      0.000000,0.000000,1.000000,0.000000,
      -0.864700,0.085300,0.617200,1.000000;;
    }
Mesh {
192;
0.831500; 0.555600; -1.000000;,
0.707100; 0.707100; -1.000000;,
0.000000; 0.000000; 1.000000;,
0.000000; 0.000000; 1.000000;,
0.923900; 0.382700; -1.000000;,
0.831500; 0.555600; -1.000000;,
0.000000; 0.000000; 1.000000;,
0.980800; 0.195100; -1.000000;,
0.923900; 0.382700; -1.000000;,
0.000000; 0.000000; 1.000000;,
1.000000; 0.000000; -1.000000;,
0.980800; 0.195100; -1.000000;,
0.000000; 0.000000; 1.000000;,
0.980800; -0.195100; -1.000000;,
1.000000; 0.000000; -1.000000;,
0.000000; 0.000000; 1.000000;,
0.923900; -0.382700; -1.000000;,
0.980800; -0.195100; -1.000000;,
0.000000; 0.000000; 1.000000;,
0.831500; -0.555600; -1.000000;,
0.923900; -0.382700; -1.000000;,
0.000000; 0.000000; 1.000000;,
0.707100; -0.707100; -1.000000;,
0.831500; -0.555600; -1.000000;,
0.000000; 0.000000; 1.000000;,
0.555600; -0.831500; -1.000000;,
0.707100; -0.707100; -1.000000;,
0.000000; 0.000000; 1.000000;,
0.382700; -0.923900; -1.000000;,
0.555600; -0.831500; -1.000000;,
0.000000; 0.000000; 1.000000;,
0.195100; -0.980800; -1.000000;,
0.382700; -0.923900; -1.000000;,
0.000000; 0.000000; 1.000000;,
-0.000000; -1.000000; -1.000000;,
0.195100; -0.980800; -1.000000;,
0.000000; 0.000000; 1.000000;,
-0.195100; -0.980800; -1.000000;,
-0.000000; -1.000000; -1.000000;,
0.000000; 0.000000; 1.000000;,
-0.382700; -0.923900; -1.000000;,
-0.195100; -0.980800; -1.000000;,
0.000000; 0.000000; 1.000000;,
-0.555600; -0.831500; -1.000000;,
-0.382700; -0.923900; -1.000000;,
0.000000; 0.000000; 1.000000;,
-0.707100; -0.707100; -1.000000;,
-0.555600; -0.831500; -1.000000;,
0.000000; 0.000000; 1.000000;,
-0.831500; -0.555600; -1.000000;,
-0.707100; -0.707100; -1.000000;,
0.000000; 0.000000; 1.000000;,
-0.923900; -0.382700; -1.000000;,
-0.831500; -0.555600; -1.000000;,
0.000000; 0.000000; 1.000000;,
-0.980800; -0.195100; -1.000000;,
-0.923900; -0.382700; -1.000000;,
0.000000; 0.000000; 1.000000;,
-1.000000; 0.000000; -1.000000;,
-0.980800; -0.195100; -1.000000;,
0.000000; 0.000000; 1.000000;,
-0.980800; 0.195100; -1.000000;,
-1.000000; 0.000000; -1.000000;,
0.000000; 0.000000; 1.000000;,
-0.923900; 0.382700; -1.000000;,
-0.980800; 0.195100; -1.000000;,
0.000000; 0.000000; 1.000000;,
-0.831500; 0.555600; -1.000000;,
-0.923900; 0.382700; -1.000000;,
0.000000; 0.000000; 1.000000;,
-0.707100; 0.707100; -1.000000;,
-0.831500; 0.555600; -1.000000;,
0.000000; 0.000000; 1.000000;,
-0.555600; 0.831500; -1.000000;,
-0.707100; 0.707100; -1.000000;,
0.000000; 0.000000; 1.000000;,
-0.382700; 0.923900; -1.000000;,
-0.555600; 0.831500; -1.000000;,
0.000000; 0.000000; 1.000000;,
-0.195100; 0.980800; -1.000000;,
-0.382700; 0.923900; -1.000000;,
0.000000; 0.000000; 1.000000;,
0.000000; 1.000000; -1.000000;,
-0.195100; 0.980800; -1.000000;,
0.000000; 0.000000; 1.000000;,
0.195100; 0.980800; -1.000000;,
0.000000; 1.000000; -1.000000;,
0.000000; 0.000000; 1.000000;,
0.382700; 0.923900; -1.000000;,
0.195100; 0.980800; -1.000000;,
0.000000; 0.000000; 1.000000;,
0.555600; 0.831500; -1.000000;,
0.382700; 0.923900; -1.000000;,
0.000000; 0.000000; 1.000000;,
0.707100; 0.707100; -1.000000;,
0.555600; 0.831500; -1.000000;,
0.000000; 0.000000; -1.000000;,
0.707100; 0.707100; -1.000000;,
0.831500; 0.555600; -1.000000;,
0.000000; 0.000000; -1.000000;,
0.831500; 0.555600; -1.000000;,
0.923900; 0.382700; -1.000000;,
0.000000; 0.000000; -1.000000;,
0.923900; 0.382700; -1.000000;,
0.980800; 0.195100; -1.000000;,
0.000000; 0.000000; -1.000000;,
0.980800; 0.195100; -1.000000;,
1.000000; 0.000000; -1.000000;,
0.000000; 0.000000; -1.000000;,
1.000000; 0.000000; -1.000000;,
0.980800; -0.195100; -1.000000;,
0.000000; 0.000000; -1.000000;,
0.980800; -0.195100; -1.000000;,
0.923900; -0.382700; -1.000000;,
0.000000; 0.000000; -1.000000;,
0.923900; -0.382700; -1.000000;,
0.831500; -0.555600; -1.000000;,
0.000000; 0.000000; -1.000000;,
0.831500; -0.555600; -1.000000;,
0.707100; -0.707100; -1.000000;,
0.000000; 0.000000; -1.000000;,
0.707100; -0.707100; -1.000000;,
0.555600; -0.831500; -1.000000;,
0.000000; 0.000000; -1.000000;,
0.555600; -0.831500; -1.000000;,
0.382700; -0.923900; -1.000000;,
0.000000; 0.000000; -1.000000;,
0.382700; -0.923900; -1.000000;,
0.195100; -0.980800; -1.000000;,
0.000000; 0.000000; -1.000000;,
0.195100; -0.980800; -1.000000;,
-0.000000; -1.000000; -1.000000;,
0.000000; 0.000000; -1.000000;,
-0.000000; -1.000000; -1.000000;,
-0.195100; -0.980800; -1.000000;,
0.000000; 0.000000; -1.000000;,
-0.195100; -0.980800; -1.000000;,
-0.382700; -0.923900; -1.000000;,
0.000000; 0.000000; -1.000000;,
-0.382700; -0.923900; -1.000000;,
-0.555600; -0.831500; -1.000000;,
0.000000; 0.000000; -1.000000;,
-0.555600; -0.831500; -1.000000;,
-0.707100; -0.707100; -1.000000;,
0.000000; 0.000000; -1.000000;,
-0.707100; -0.707100; -1.000000;,
-0.831500; -0.555600; -1.000000;,
0.000000; 0.000000; -1.000000;,
-0.831500; -0.555600; -1.000000;,
-0.923900; -0.382700; -1.000000;,
0.000000; 0.000000; -1.000000;,
-0.923900; -0.382700; -1.000000;,
-0.980800; -0.195100; -1.000000;,
0.000000; 0.000000; -1.000000;,
-0.980800; -0.195100; -1.000000;,
-1.000000; 0.000000; -1.000000;,
0.000000; 0.000000; -1.000000;,
-1.000000; 0.000000; -1.000000;,
-0.980800; 0.195100; -1.000000;,
0.000000; 0.000000; -1.000000;,
-0.980800; 0.195100; -1.000000;,
-0.923900; 0.382700; -1.000000;,
0.000000; 0.000000; -1.000000;,
-0.923900; 0.382700; -1.000000;,
-0.831500; 0.555600; -1.000000;,
0.000000; 0.000000; -1.000000;,
-0.831500; 0.555600; -1.000000;,
-0.707100; 0.707100; -1.000000;,
0.000000; 0.000000; -1.000000;,
-0.707100; 0.707100; -1.000000;,
-0.555600; 0.831500; -1.000000;,
0.000000; 0.000000; -1.000000;,
-0.555600; 0.831500; -1.000000;,
-0.382700; 0.923900; -1.000000;,
0.000000; 0.000000; -1.000000;,
-0.382700; 0.923900; -1.000000;,
-0.195100; 0.980800; -1.000000;,
0.000000; 0.000000; -1.000000;,
-0.195100; 0.980800; -1.000000;,
0.000000; 1.000000; -1.000000;,
0.000000; 0.000000; -1.000000;,
0.000000; 1.000000; -1.000000;,
0.195100; 0.980800; -1.000000;,
0.000000; 0.000000; -1.000000;,
0.195100; 0.980800; -1.000000;,
0.382700; 0.923900; -1.000000;,
0.000000; 0.000000; -1.000000;,
0.382700; 0.923900; -1.000000;,
0.555600; 0.831500; -1.000000;,
0.555600; 0.831500; -1.000000;,
0.707100; 0.707100; -1.000000;,
0.000000; 0.000000; -1.000000;;
64;
3; 0, 2, 1;,
3; 3, 5, 4;,
3; 6, 8, 7;,
3; 9, 11, 10;,
3; 12, 14, 13;,
3; 15, 17, 16;,
3; 18, 20, 19;,
3; 21, 23, 22;,
3; 24, 26, 25;,
3; 27, 29, 28;,
3; 30, 32, 31;,
3; 33, 35, 34;,
3; 36, 38, 37;,
3; 39, 41, 40;,
3; 42, 44, 43;,
3; 45, 47, 46;,
3; 48, 50, 49;,
3; 51, 53, 52;,
3; 54, 56, 55;,
3; 57, 59, 58;,
3; 60, 62, 61;,
3; 63, 65, 64;,
3; 66, 68, 67;,
3; 69, 71, 70;,
3; 72, 74, 73;,
3; 75, 77, 76;,
3; 78, 80, 79;,
3; 81, 83, 82;,
3; 84, 86, 85;,
3; 87, 89, 88;,
3; 90, 92, 91;,
3; 93, 95, 94;,
3; 96, 98, 97;,
3; 99, 101, 100;,
3; 102, 104, 103;,
3; 105, 107, 106;,
3; 108, 110, 109;,
3; 111, 113, 112;,
3; 114, 116, 115;,
3; 117, 119, 118;,
3; 120, 122, 121;,
3; 123, 125, 124;,
3; 126, 128, 127;,
3; 129, 131, 130;,
3; 132, 134, 133;,
3; 135, 137, 136;,
3; 138, 140, 139;,
3; 141, 143, 142;,
3; 144, 146, 145;,
3; 147, 149, 148;,
3; 150, 152, 151;,
3; 153, 155, 154;,
3; 156, 158, 157;,
3; 159, 161, 160;,
3; 162, 164, 163;,
3; 165, 167, 166;,
3; 168, 170, 169;,
3; 171, 173, 172;,
3; 174, 176, 175;,
3; 177, 179, 178;,
3; 180, 182, 181;,
3; 183, 185, 184;,
3; 186, 188, 187;,
3; 189, 191, 190;;
  MeshMaterialList {
    0;
    64;
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0;;
    }  //End of MeshMaterialList
  MeshNormals {
192;
    0.705893; 0.471664; -0.528367;,
    0.600330; 0.600330; -0.528367;,
    0.000000; 0.000000; 1.000000;,
    0.000000; 0.000000; 1.000000;,
    0.784356; 0.324870; -0.528367;,
    0.705893; 0.471664; -0.528367;,
    0.000000; 0.000000; 1.000000;,
    0.832667; 0.165624; -0.528367;,
    0.784356; 0.324870; -0.528367;,
    0.000000; 0.000000; 1.000000;,
    0.848994; 0.000000; -0.528367;,
    0.832667; 0.165624; -0.528367;,
    0.000000; 0.000000; 1.000000;,
    0.832667; -0.165624; -0.528367;,
    0.848994; 0.000000; -0.528367;,
    0.000000; 0.000000; 1.000000;,
    0.784356; -0.324870; -0.528367;,
    0.832667; -0.165624; -0.528367;,
    0.000000; 0.000000; 1.000000;,
    0.705893; -0.471664; -0.528367;,
    0.784356; -0.324870; -0.528367;,
    0.000000; 0.000000; 1.000000;,
    0.600330; -0.600330; -0.528367;,
    0.705893; -0.471664; -0.528367;,
    0.000000; 0.000000; 1.000000;,
    0.471664; -0.705893; -0.528367;,
    0.600330; -0.600330; -0.528367;,
    0.000000; 0.000000; 1.000000;,
    0.324870; -0.784356; -0.528367;,
    0.471664; -0.705893; -0.528367;,
    0.000000; 0.000000; 1.000000;,
    0.165624; -0.832667; -0.528367;,
    0.324870; -0.784356; -0.528367;,
    0.000000; 0.000000; 1.000000;,
    0.000000; -0.848994; -0.528367;,
    0.165624; -0.832667; -0.528367;,
    0.000000; 0.000000; 1.000000;,
    -0.165624; -0.832667; -0.528367;,
    0.000000; -0.848994; -0.528367;,
    0.000000; 0.000000; 1.000000;,
    -0.324870; -0.784356; -0.528367;,
    -0.165624; -0.832667; -0.528367;,
    0.000000; 0.000000; 1.000000;,
    -0.471664; -0.705893; -0.528367;,
    -0.324870; -0.784356; -0.528367;,
    0.000000; 0.000000; 1.000000;,
    -0.600330; -0.600330; -0.528367;,
    -0.471664; -0.705893; -0.528367;,
    0.000000; 0.000000; 1.000000;,
    -0.705893; -0.471664; -0.528367;,
    -0.600330; -0.600330; -0.528367;,
    0.000000; 0.000000; 1.000000;,
    -0.784356; -0.324870; -0.528367;,
    -0.705893; -0.471664; -0.528367;,
    0.000000; 0.000000; 1.000000;,
    -0.832667; -0.165624; -0.528367;,
    -0.784356; -0.324870; -0.528367;,
    0.000000; 0.000000; 1.000000;,
    -0.848994; 0.000000; -0.528367;,
    -0.832667; -0.165624; -0.528367;,
    0.000000; 0.000000; 1.000000;,
    -0.832667; 0.165624; -0.528367;,
    -0.848994; 0.000000; -0.528367;,
    0.000000; 0.000000; 1.000000;,
    -0.784356; 0.324900; -0.528367;,
    -0.832667; 0.165624; -0.528367;,
    0.000000; 0.000000; 1.000000;,
    -0.705893; 0.471664; -0.528367;,
    -0.784356; 0.324900; -0.528367;,
    0.000000; 0.000000; 1.000000;,
    -0.600330; 0.600330; -0.528367;,
    -0.705893; 0.471664; -0.528367;,
    0.000000; 0.000000; 1.000000;,
    -0.471664; 0.705893; -0.528367;,
    -0.600330; 0.600330; -0.528367;,
    0.000000; 0.000000; 1.000000;,
    -0.324870; 0.784356; -0.528367;,
    -0.471664; 0.705893; -0.528367;,
    0.000000; 0.000000; 1.000000;,
    -0.165624; 0.832667; -0.528367;,
    -0.324870; 0.784356; -0.528367;,
    0.000000; 0.000000; 1.000000;,
    0.000000; 0.848994; -0.528367;,
    -0.165624; 0.832667; -0.528367;,
    0.000000; 0.000000; 1.000000;,
    0.165624; 0.832667; -0.528367;,
    0.000000; 0.848994; -0.528367;,
    0.000000; 0.000000; 1.000000;,
    0.324900; 0.784356; -0.528367;,
    0.165624; 0.832667; -0.528367;,
    0.000000; 0.000000; 1.000000;,
    0.471664; 0.705893; -0.528367;,
    0.324900; 0.784356; -0.528367;,
    0.000000; 0.000000; 1.000000;,
    0.600330; 0.600330; -0.528367;,
    0.471664; 0.705893; -0.528367;,
    0.000000; 0.000000; -1.000000;,
    0.600330; 0.600330; -0.528367;,
    0.705893; 0.471664; -0.528367;,
    0.000000; 0.000000; -1.000000;,
    0.705893; 0.471664; -0.528367;,
    0.784356; 0.324870; -0.528367;,
    0.000000; 0.000000; -1.000000;,
    0.784356; 0.324870; -0.528367;,
    0.832667; 0.165624; -0.528367;,
    0.000000; 0.000000; -1.000000;,
    0.832667; 0.165624; -0.528367;,
    0.848994; 0.000000; -0.528367;,
    0.000000; 0.000000; -1.000000;,
    0.848994; 0.000000; -0.528367;,
    0.832667; -0.165624; -0.528367;,
    0.000000; 0.000000; -1.000000;,
    0.832667; -0.165624; -0.528367;,
    0.784356; -0.324870; -0.528367;,
    0.000000; 0.000000; -1.000000;,
    0.784356; -0.324870; -0.528367;,
    0.705893; -0.471664; -0.528367;,
    0.000000; 0.000000; -1.000000;,
    0.705893; -0.471664; -0.528367;,
    0.600330; -0.600330; -0.528367;,
    0.000000; 0.000000; -1.000000;,
    0.600330; -0.600330; -0.528367;,
    0.471664; -0.705893; -0.528367;,
    0.000000; 0.000000; -1.000000;,
    0.471664; -0.705893; -0.528367;,
    0.324870; -0.784356; -0.528367;,
    0.000000; 0.000000; -1.000000;,
    0.324870; -0.784356; -0.528367;,
    0.165624; -0.832667; -0.528367;,
    0.000000; 0.000000; -1.000000;,
    0.165624; -0.832667; -0.528367;,
    0.000000; -0.848994; -0.528367;,
    0.000000; 0.000000; -1.000000;,
    0.000000; -0.848994; -0.528367;,
    -0.165624; -0.832667; -0.528367;,
    0.000000; 0.000000; -1.000000;,
    -0.165624; -0.832667; -0.528367;,
    -0.324870; -0.784356; -0.528367;,
    0.000000; 0.000000; -1.000000;,
    -0.324870; -0.784356; -0.528367;,
    -0.471664; -0.705893; -0.528367;,
    0.000000; 0.000000; -1.000000;,
    -0.471664; -0.705893; -0.528367;,
    -0.600330; -0.600330; -0.528367;,
    0.000000; 0.000000; -1.000000;,
    -0.600330; -0.600330; -0.528367;,
    -0.705893; -0.471664; -0.528367;,
    0.000000; 0.000000; -1.000000;,
    -0.705893; -0.471664; -0.528367;,
    -0.784356; -0.324870; -0.528367;,
    0.000000; 0.000000; -1.000000;,
    -0.784356; -0.324870; -0.528367;,
    -0.832667; -0.165624; -0.528367;,
    0.000000; 0.000000; -1.000000;,
    -0.832667; -0.165624; -0.528367;,
    -0.848994; 0.000000; -0.528367;,
    0.000000; 0.000000; -1.000000;,
    -0.848994; 0.000000; -0.528367;,
    -0.832667; 0.165624; -0.528367;,
    0.000000; 0.000000; -1.000000;,
    -0.832667; 0.165624; -0.528367;,
    -0.784356; 0.324900; -0.528367;,
    0.000000; 0.000000; -1.000000;,
    -0.784356; 0.324900; -0.528367;,
    -0.705893; 0.471664; -0.528367;,
    0.000000; 0.000000; -1.000000;,
    -0.705893; 0.471664; -0.528367;,
    -0.600330; 0.600330; -0.528367;,
    0.000000; 0.000000; -1.000000;,
    -0.600330; 0.600330; -0.528367;,
    -0.471664; 0.705893; -0.528367;,
    0.000000; 0.000000; -1.000000;,
    -0.471664; 0.705893; -0.528367;,
    -0.324870; 0.784356; -0.528367;,
    0.000000; 0.000000; -1.000000;,
    -0.324870; 0.784356; -0.528367;,
    -0.165624; 0.832667; -0.528367;,
    0.000000; 0.000000; -1.000000;,
    -0.165624; 0.832667; -0.528367;,
    0.000000; 0.848994; -0.528367;,
    0.000000; 0.000000; -1.000000;,
    0.000000; 0.848994; -0.528367;,
    0.165624; 0.832667; -0.528367;,
    0.000000; 0.000000; -1.000000;,
    0.165624; 0.832667; -0.528367;,
    0.324900; 0.784356; -0.528367;,
    0.000000; 0.000000; -1.000000;,
    0.324900; 0.784356; -0.528367;,
    0.471664; 0.705893; -0.528367;,
    0.471664; 0.705893; -0.528367;,
    0.600330; 0.600330; -0.528367;,
    0.000000; 0.000000; -1.000000;;
64;
3; 0, 2, 1;,
3; 3, 5, 4;,
3; 6, 8, 7;,
3; 9, 11, 10;,
3; 12, 14, 13;,
3; 15, 17, 16;,
3; 18, 20, 19;,
3; 21, 23, 22;,
3; 24, 26, 25;,
3; 27, 29, 28;,
3; 30, 32, 31;,
3; 33, 35, 34;,
3; 36, 38, 37;,
3; 39, 41, 40;,
3; 42, 44, 43;,
3; 45, 47, 46;,
3; 48, 50, 49;,
3; 51, 53, 52;,
3; 54, 56, 55;,
3; 57, 59, 58;,
3; 60, 62, 61;,
3; 63, 65, 64;,
3; 66, 68, 67;,
3; 69, 71, 70;,
3; 72, 74, 73;,
3; 75, 77, 76;,
3; 78, 80, 79;,
3; 81, 83, 82;,
3; 84, 86, 85;,
3; 87, 89, 88;,
3; 90, 92, 91;,
3; 93, 95, 94;,
3; 96, 98, 97;,
3; 99, 101, 100;,
3; 102, 104, 103;,
3; 105, 107, 106;,
3; 108, 110, 109;,
3; 111, 113, 112;,
3; 114, 116, 115;,
3; 117, 119, 118;,
3; 120, 122, 121;,
3; 123, 125, 124;,
3; 126, 128, 127;,
3; 129, 131, 130;,
3; 132, 134, 133;,
3; 135, 137, 136;,
3; 138, 140, 139;,
3; 141, 143, 142;,
3; 144, 146, 145;,
3; 147, 149, 148;,
3; 150, 152, 151;,
3; 153, 155, 154;,
3; 156, 158, 157;,
3; 159, 161, 160;,
3; 162, 164, 163;,
3; 165, 167, 166;,
3; 168, 170, 169;,
3; 171, 173, 172;,
3; 174, 176, 175;,
3; 177, 179, 178;,
3; 180, 182, 181;,
3; 183, 185, 184;,
3; 186, 188, 187;,
3; 189, 191, 190;;
}  //End of MeshNormals
  }  // End of the Mesh Cone 
  }  // SI End of the Object Cone 
}  // End of the Root Frame
