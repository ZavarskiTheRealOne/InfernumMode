sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float3 uColor;
float3 uSecondaryColor;
float uOpacity;
float uSaturation;
float uRotation;
float uTime;
float4 uSourceRect;
float2 uWorldPosition;
float uDirection;
float3 uLightSource;
float2 uImageSize0;
float2 uImageSize1;
matrix uWorldViewProjection;
float4 uShaderSpecificData;

struct VertexShaderInput
{
    float4 Position : POSITION0;
    float4 Color : COLOR0;
    float3 TextureCoordinates : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
    float3 TextureCoordinates : TEXCOORD0;
};

VertexShaderOutput VertexShaderFunction(in VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput) 0;
    float4 pos = mul(input.Position, uWorldViewProjection);
    output.Position = pos;
    
    output.Color = input.Color;
    output.TextureCoordinates = input.TextureCoordinates;
    output.TextureCoordinates.y = (output.TextureCoordinates.y - 0.5) / input.TextureCoordinates.z + 0.5;
    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    float4 color = input.Color;
    float2 coords = input.TextureCoordinates;
    float originalAlpha = color.a;
    
    // Use a noise texture to determine the texture of the flesh on the primitive strip.
    // Said noise value determines the base color.
    float fleshColorFade = tex2D(uImage1, coords).r * 0.7;
    color.rgb = lerp(color.rgb, uSecondaryColor * 1.5, fleshColorFade);
    
    // Interpolant towards the "edge" colors depending on how close a pixel is towards the ends.
    float boundFade = (1 - pow(sin(coords.x * 3.141), 2)) + (1 - pow(sin(coords.y * 3.141), 1.1));
    
    // Clamp the interpolant between 0-1, to prevent the lerp from entering undefined areas.
    boundFade = saturate(boundFade);
    color.rgb = lerp(color.rgb, uColor * 0.6, boundFade);
    return color * originalAlpha;
}

technique Technique1
{
    pass TrailPass
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
