Shader "UI/CircleVisionMask_InnerOuter"
{
    Properties
    {
        _Color ("Color", Color) = (0,0,0,1)
        _InnerRadius ("Inner Radius (alpha 0)", Range(0, 2)) = 0.2
        _OuterRadius ("Outer Radius (alpha 1)", Range(0, 2)) = 0.4
        _Center ("Center", Vector) = (0.5, 0.5, 0, 0)
        _Aspect ("Aspect Ratio", Float) = 1.0
        _RectSize ("Rect Size", Vector) = (1,1,0,0)
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv     : TEXCOORD0;
            };

            float _InnerRadius;
            float _OuterRadius;
            float4 _Center;
            float _Aspect;
            fixed4 _Color;
            float4 _RectSize;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 pixelPos = i.uv * _RectSize.xy;
                float2 centerPos = _Center.xy * _RectSize.xy;
                float2 d = pixelPos - centerPos;

                // ✅ Normalize d by the shorter side to get a uniform circle
                float minSide = min(_RectSize.x, _RectSize.y);
                float dist = length(d / minSide);  // <-- key change

                float inner = _InnerRadius;
                float outer = _OuterRadius;

                // Band 1: 0.9*inner → inner, alpha 0 → 0.99
                float t1 = saturate((dist - inner * 0.9) / max(inner * 0.1, 1e-5));
                // Band 2: inner → outer, alpha 0.99 → 1
                float t2 = saturate((dist - inner) / max(outer - inner, 1e-5));
                float alpha = 0.99 * smoothstep(0.0, 1.0, t1)
                            + 0.01 * smoothstep(0.0, 1.0, t2);
                return float4(_Color.rgb, alpha * _Color.a);
            }
            ENDCG
        }
    }
}