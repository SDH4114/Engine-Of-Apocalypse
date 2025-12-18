#version 330 core

out vec4 FragColor;

uniform vec3 uAlbedo;

void main()
{
    FragColor = vec4(uAlbedo, 1.0);
}
