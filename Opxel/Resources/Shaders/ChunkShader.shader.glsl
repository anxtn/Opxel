#version 420

#ifdef VERTEX
layout(location = 0) in vec3 aPosition;
layout(location = 1) in vec2 aUv;

uniform mat4 uViewProjection;
uniform float uViewport;
uniform vec3 uChunkPosition;

//out vec4 vColor;
out vec2 vUv;

void main()
{
    vec4 position = vec4(aPosition + uChunkPosition,1) * uViewProjection;
    position.y *= uViewport;
    vUv = aUv;
    gl_Position = position;
}
#endif

#ifdef FRAGMENT

//in vec4 vColor;
in vec2 vUv;

uniform sampler2D uTexture;

out vec4 fColor;

void main()
{
    fColor = texture2D(uTexture,vUv);
}
#endif
