sampler2D inputTexture : register(s0);         // The base image
sampler2D noiseTexture : register(s1);         // Turbulent noise texture

float time; // Time in seconds, passed from the game
float intensity = 0.02; // Strength of the distortion

float4 PixelShaderFunction(float2 uv : TEXCOORD0) : COLOR0
{
    // Animate the noise by offsetting with time
    float2 noiseUV = uv + float2(time * 0.1, time * 0.15);

    // Sample the noise and map it from [0,1] → [-1,1]
    float2 noise = tex2D(noiseTexture, noiseUV).rg * 2.0 - 1.0;

    // Apply the distortion
    float2 distortedUV = uv + noise * intensity;

    // Sample the base texture using the distorted UV
    return tex2D(inputTexture, distortedUV);
}

technique Distortion
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
