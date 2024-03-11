#version 330 core
out vec4 FragColor;
  
//in vec2 TexCoord;
// in varyings
in vec3 Normal;
in vec3 FragPos;
in vec2 TexCoords;

// uniforms
uniform sampler2D ourTexture;
uniform sampler2D specularTexture;

uniform vec3 viewPos;

struct DirectionalLight
{
    vec3 direction;
    vec3 color;
    vec3 ambient;
};

struct Material
{
    vec3 diffuse;
    vec3 specular;
    float shininess;
};

void main()
{
    DirectionalLight dl;
    dl.direction = vec3(0, -1, 0);
    dl.color = vec3(1, 1, 1);
    dl.ambient = vec3(0.5);

    Material mat;
    mat.diffuse = vec3(texture(ourTexture, TexCoords));
    mat.specular = vec3(texture(specularTexture, TexCoords));
    mat.shininess = 64;

    // diffuse angle
    float NdL = max(dot(Normal, -dl.direction), 0);

    // variables to calculate specular lighting
    vec3 viewDir = normalize(viewPos - FragPos);
    vec3 reflectDir = reflect(dl.direction, Normal);
    float VdR = pow(max(dot(viewDir, reflectDir), 0), mat.shininess); 

    // specular light
    vec3 specularContribution = VdR * mat.specular * dl.color;

    // diffuse light
    vec3 diffuseContribution = NdL * mat.diffuse * dl.color;

    // ambient light
    vec3 ambientContribution = dl.ambient * mat.diffuse;

    // final color
    vec3 finalColor = ambientContribution + diffuseContribution + specularContribution;

    FragColor = vec4(finalColor, 1);//texture(ourTexture, TexCoord);  
}