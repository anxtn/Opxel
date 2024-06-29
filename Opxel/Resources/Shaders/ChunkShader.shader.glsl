#version 400 core

#ifdef VERTEX
layout(location = 0) in uint aVertexData;

uniform mat4 uViewProjection;
uniform float uViewport;

out vec4 vColor;

void main()
{
    
    float aPosX = bitfieldExtract(aVertexData, 0, 8);
    float aPosY = bitfieldExtract(aVertexData, 8, 8);
    float aPosZ = bitfieldExtract(aVertexData, 16, 8);
    
    vColor = vec4(0.8,0.8,0.8,1.0);

    aPosY *= uViewport;
    gl_Position = vec4(aPosX, aPosY, aPosZ,1);
}
#endif

#ifdef FRAGMENT

in vec4 vColor;

out vec4 fColor;

const vec3 lightDirection = vec3(1,-1,0);

void main()
{

fColor = vColor;
}
#endif
