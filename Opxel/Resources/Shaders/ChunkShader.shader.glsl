#version 420

#ifdef VERTEX
layout(location = 0) in vec3 aPosition;
layout(location = 1) in vec2 aUv;
layout(location = 2) in vec3 aNormal;

uniform mat4 uViewProjection;
uniform float uViewport;
uniform vec3 uChunkPosition;

out vec2 vUv;
out vec3 vNormal;

void main()
{
    vec4 position = vec4(aPosition + uChunkPosition,1) * uViewProjection;
    position.y *= uViewport;
    vUv = aUv;
    vNormal = aNormal;
    gl_Position = position;
}
#endif

#ifdef FRAGMENT

in vec2 vUv;
in vec3 vNormal;

uniform sampler2D uTexture;

out vec4 fColor;

const vec3 lightDirection = vec3(2f,-1f,0f);

void main()
{
    float diff = clamp(1- dot(vNormal, lightDirection),0.5f,1f);
    vec4 textureColor = texture2D(uTexture,vUv);
    fColor = textureColor * diff;
}
#endif
