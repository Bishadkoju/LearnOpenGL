#version 330 core
out vec4 FragColor;
 
in vec3 normal;
in vec3 fragPos;
in vec2 TexCoords;

struct Light {
    vec3 position;
    vec3 direction;
    float cutOff;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;

    float constant;
    float linear;
    float quadratic;
};
struct Material {
    sampler2D diffuse;
    sampler2D specular;
    float shininess;
};

uniform Material material;
uniform Light light;
uniform vec3 objectColor;
uniform vec3 viewPos;

void main()
{
    
    vec3 lightDir = normalize(light.position-fragPos);
    float theta = dot(lightDir, normalize(-light.direction) );
    if (theta > light.cutOff) {
       // ambient
        vec3 ambient = light.ambient*vec3(texture(material.diffuse,TexCoords).rgb);

        // diffuse
        vec3 norm = normalize(normal);
        float diff = max(dot(norm, lightDir), 0);
        vec3 diffuse = diff*light.diffuse*vec3(texture(material.diffuse, TexCoords));

        //specular
        vec3 viewDir = normalize(viewPos-fragPos);
        vec3 reflectDir = normalize(reflect(-lightDir, norm));
        float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
        vec3 specular = vec3(texture(material.specular, TexCoords))*spec*light.specular;

        // handle attenuation
        float distance = length(fragPos-light.position);
        float attenuation = 1.0/(light.constant+ distance* light.linear+ (distance*distance)*light.quadratic);
        ambient*=attenuation;
        diffuse*=attenuation;
        specular*=attenuation;

        vec3 result = (ambient+diffuse+specular)*objectColor;
        FragColor = vec4(result, 1.0);
    } else {
        FragColor = vec4(light.ambient*vec3(texture(material.diffuse,TexCoords).rgb),1.0);
    }
}