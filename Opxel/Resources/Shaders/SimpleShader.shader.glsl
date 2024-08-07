#version 330 core

#ifdef VERTEX
layout(location = 0) in vec3 aPosition;
layout(location = 1) in vec4 aColor;
layout(location = 2) in vec3 aNormal;

uniform mat4 uModel;
uniform mat4 uViewProjection;

out vec4 vColor;
out  vec3 vTransformedNormal;

void main()
{
    vec4 position = vec4(aPosition.x, aPosition.y, aPosition.z,1)* uModel * uViewProjection;
    gl_Position = position;
    vColor = aColor;
    mat3 normalMatrix = transpose(inverse(mat3(uModel)));
    vTransformedNormal = normalize(normalMatrix * aNormal);;
}
#endif

#ifdef FRAGMENT

in vec4 vColor;
in vec3 vTransformedNormal;

out vec4 fColor;

const vec3 lightDirection = vec3(0.03,-1,0);

void main()
{
float diff = 1 - (0.45 * 0.7 * dot(vTransformedNormal, lightDirection));

fColor = vColor * diff;
}
#endif
