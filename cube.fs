#version 330 core
out vec4 FragColor;
 
in vec3 normal;
in vec3 fragPos;

struct Light {
    vec3 position;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};
struct Material {
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
    float shininess;
};

uniform Material material;
uniform Light light;
uniform vec3 objectColor;
uniform vec3 viewPos;

void main()
{
   // ambient
    vec3 ambient = material.ambient*light.ambient;

    // diffuse
    vec3 norm = normalize(normal);
    vec3 lightDir = normalize(light.position-fragPos);
    float diff = max(dot(norm, lightDir), 0);
    vec3 diffuse = diff*light.diffuse*material.diffuse;

    //specular
    vec3 viewDir = normalize(viewPos-fragPos);
    vec3 reflectDir = normalize(reflect(-lightDir, norm));
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    vec3 specular = material.specular*spec*light.specular;

    vec3 result = (ambient+diffuse+specular)*objectColor;
    FragColor = vec4(result, 1.0);
}