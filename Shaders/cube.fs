#version 330 core
out vec4 FragColor;
 
in vec3 normal;
in vec3 fragPos;
in vec2 TexCoords;

struct DirectionLight {
    vec3 direction;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};

struct PointLight {
    vec3 position;

    float constant;
    float linear;
    float quadratic;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};

struct SpotLight {
    vec3 position;
    vec3 direction;
    float cutOff;
    float outerCutOff;
  
    float constant;
    float linear;
    float quadratic;
  
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;       
};

struct Material {
    sampler2D diffuse;
    sampler2D specular;
    float shininess;
};

uniform Material material;
uniform vec3 objectColor;
uniform vec3 viewPos;

#define NR_POINT_LIGHTS 4
uniform DirectionLight dirLight;
uniform PointLight pointLights[NR_POINT_LIGHTS];
uniform SpotLight spotLight;

vec3 calculateDirectionLightImpact(DirectionLight dirLight, vec3 normal, vec3 viewDir);
vec3 calculatePointLightImpact(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir);
vec3 calculateSpotLightImpact(SpotLight light, vec3 normal, vec3 fragPos, vec3 viewDir);

void main()
{
    vec3 norm = normalize(normal);
    vec3 viewDir = normalize(viewPos - fragPos);

    // Directional Lighting
    vec3 result = calculateDirectionLightImpact(dirLight, norm, viewDir);

    // Point Lights
    for (int i=0; i< NR_POINT_LIGHTS; i++) {
        result += calculatePointLightImpact(pointLights[i], norm, fragPos, viewDir);
    }
    
    // Spot light
    //result += calculateSpotLightImpact(spotLight, norm, fragPos, viewDir);
    
    FragColor = vec4(result, 1.0);
}

vec3 calculateDirectionLightImpact(DirectionLight light, vec3 normal, vec3 viewDir) {
    vec3 lightDir = normalize(-light.direction);
    // diffuse shading
    float diff = max(dot(normal, lightDir), 0.0);
    // specular shading
    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0),
    material.shininess);
    // combine results
    vec3 ambient = light.ambient * vec3(texture(material.diffuse,TexCoords).rgb);
    vec3 diffuse = light.diffuse * diff * vec3(texture(material.diffuse, TexCoords).rgb);
    vec3 specular = light.specular * spec * vec3(texture(material.specular,
    TexCoords));
    return (ambient + diffuse + specular);
}

vec3 calculatePointLightImpact(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir) {
    vec3 lightDir = normalize (light.position - fragPos);
    //diffuse shading
    float diff = max(dot(normal, lightDir), 0);
    // specular shading
    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(reflectDir, viewDir), 0.0f), material.shininess);
    // attenuation
    float distance = length(light.position - fragPos);
    float attenuation = 1.0/(light.constant + light.linear * distance + light.quadratic * distance*distance);

    vec3 ambient = light.ambient * vec3(texture(material.diffuse, TexCoords));
    vec3 diffuse = light.diffuse * diff * vec3(texture(material.diffuse, TexCoords));
    vec3 specular = light.specular * spec * vec3(texture(material.specular, TexCoords).rgb);

    return (ambient + diffuse + specular);
}

vec3 calculateSpotLightImpact(SpotLight light, vec3 normal, vec3 fragPos, vec3 viewDir){
    vec3 lightDir = normalize(light.position - fragPos);
    // diffuse shading
    float diff = max(dot(normal, lightDir), 0.0);
    // specular shading
    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    // attenuation
    float distance = length(light.position - fragPos);
    float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic * (distance * distance));    
    // spotlight intensity
    float theta = dot(lightDir, normalize(-light.direction)); 
    float epsilon = light.cutOff - light.outerCutOff;
    float intensity = clamp((theta - light.outerCutOff) / epsilon, 0.0, 1.0);
    // combine results
    vec3 ambient = light.ambient * vec3(texture(material.diffuse, TexCoords));
    vec3 diffuse = light.diffuse * diff * vec3(texture(material.diffuse, TexCoords));
    vec3 specular = light.specular * spec * vec3(texture(material.specular, TexCoords));
    ambient *= attenuation * intensity;
    diffuse *= attenuation * intensity;
    specular *= attenuation * intensity;
    return (ambient + diffuse + specular);
}
