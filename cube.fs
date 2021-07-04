#version 330 core
out vec4 FragColor;
 
in vec3 normal;
in vec3 fragPos;

uniform vec3 objectColor;
uniform vec3 lightColor;
uniform vec3 lightPos;
uniform vec3 viewPos;

void main()
{
    float ambientStrength = 0.1;
    vec3 ambient = ambientStrength*lightColor;

    vec3 norm = normalize(normal);
    vec3 lightDir = normalize(lightPos-fragPos);
    float diffusionStrength = max(dot(norm, lightDir), 0);
    vec3 diffuse = diffusionStrength*lightColor;

    float specularStrength = 0.5;
    vec3 viewDir = normalize(viewPos-fragPos);
    vec3 reflectDir = normalize(reflect(-lightDir, norm));
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32);
    vec3 specular = specularStrength*spec*lightColor;

    vec3 result = (ambient+diffuse+specular)*objectColor;
    FragColor = vec4(result, 1.0);
}