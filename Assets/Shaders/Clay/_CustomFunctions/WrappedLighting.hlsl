void LightingSubsurface_float(
    half3 lightDirection, 
    half3 normalWS, 
    half3 subsurfaceColor, 
    half subsurfaceRadius,
    out float subsurfaceRadiance) 
{
    half NdotL = dot(normalWS, lightDirection);
    half alpha = subsurfaceRadius;
    half theta_m = acos(-alpha);

    half theta = max(0, NdotL + alpha) - alpha;
    half normalization_jgt = (2 + alpha) / (2 * (1 + alpha));
    half wrapped_jgt = (pow(((theta + alpha) / (1 + alpha)), 1 + alpha)) * normalization_jgt;

    half wrapped_valve = 0.25 * (NdotL + 1) * (NdotL + 1);
    half wrapped_simple = (NdotL + alpha) / (1 + alpha);

    subsurfaceRadiance = subsurfaceColor * wrapped_jgt;
}