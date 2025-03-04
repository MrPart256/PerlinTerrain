Shader "Custom/Triplanar"
{
    Properties
    {
        _GrassTex ("Grass Tex", 2D) = "white" {}
        _RockTex ("Rock Texture", 2D) = "white" {}
        _SnowTex ("Snow Texture", 2D) = "white" {}
        _RockHeight("Rock Height", Float) = 0.0
        _SnowHeight("Snow Height", Float) = 0.0
        _EnableTexture("Enable texture", Int) = 0
    }
    
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _GrassTex;
        sampler2D _RockTex;
        sampler2D _SnowTex;

        int _EnableTexture;

        float _RockHeight;
        float _SnowHeight;

        struct Input
        {
            float3 customColor;
            float3 worldPos;
            float2 uv_GrassTex;
            float2 uv_RockTex;
            float2 uv_SnowTex;
        };

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            fixed3 grassColor = tex2D(_GrassTex, IN.uv_GrassTex);
            fixed3 rockColor = tex2D(_RockTex, IN.uv_RockTex);
            fixed3 snowColor = tex2D(_SnowTex, IN.uv_SnowTex);

            fixed3 rockBlend = lerp(grassColor, rockColor, clamp(min(1, (IN.worldPos.y / _RockHeight)), 0, 1));
            fixed3 snowBlend = lerp(rockBlend, snowColor, clamp(min(1, IN.worldPos.y / _SnowHeight), 0, 1));

            o.Albedo = _EnableTexture == 1 ? snowBlend : float3(.5f, .5f, .5f);
            o.Alpha = 1;
        }
        ENDCG
    }
    FallBack "Diffuse"
}