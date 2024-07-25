#version 420

#ifdef VERTEX
layout(location = 0) in vec3 aPosition;
layout(location = 1) in vec2 aUv;
layout(location = 2) in vec3 aNormal;

uniform mat4 uViewProjection;
uniform vec3 uChunkPosition;
uniform vec3 uCameraPosition;

out vec2 vUv;
out vec3 vNormal;
out float vCameraDistance;

void main()
{
    vec3 worldPosition = aPosition + uChunkPosition;
    vec4 position = vec4(worldPosition,1) * uViewProjection;
    vUv = aUv;
    vNormal = aNormal;
    vCameraDistance = distance(worldPosition.xz,uCameraPosition.xz);
    gl_Position = position;
    
}
#endif

#ifdef FRAGMENT

in vec2 vUv;
in vec3 vNormal;
in float vCameraDistance;

uniform sampler2D uTexture;
uniform float uRenderDistance;

out vec4 fColor;

const vec3 lightDirection = vec3(1f,-1f,0.5f);
//const vec4 fadeColor = vec4(100f / 255f, 149f / 255f, 237f / 255f, 1f); //Cornflowerblue
const vec4 fadeColor = vec4(1f,1f,1f, 1f);

float calcFadeValue(float dist)
{
    float normalizedDistance = dist / uRenderDistance;
    float funcValue = (normalizedDistance * normalizedDistance - 0.7f) * 3.4f;
    float fadeValue =  clamp(funcValue,0,1);
    return fadeValue;
}

void main()
{
    float fadeValue = calcFadeValue(vCameraDistance);
    float diff = clamp(1-dot(vNormal, lightDirection),0.3f,1f);
    vec4 textureColor = texture2D(uTexture,vUv);
    fColor = mix(textureColor * diff, fadeColor, fadeValue);
}



#endif
