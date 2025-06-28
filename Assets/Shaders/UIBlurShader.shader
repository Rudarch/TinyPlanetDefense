Shader "UI/BlurWithGrabPass"
{
    Properties
    {
        _Size ("Blur Size", Float) = 1.0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        GrabPass { "_GrabTexture" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            float _Size;
            sampler2D _GrabTexture;

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(appdata_base v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = ComputeGrabScreenPos(o.vertex).xy;
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float2 offset = _Size / _ScreenParams.xy;

                float4 color = tex2D(_GrabTexture, uv) * 0.36;
                color += tex2D(_GrabTexture, uv + float2(offset.x, 0)) * 0.16;
                color += tex2D(_GrabTexture, uv - float2(offset.x, 0)) * 0.16;
                color += tex2D(_GrabTexture, uv + float2(0, offset.y)) * 0.16;
                color += tex2D(_GrabTexture, uv - float2(0, offset.y)) * 0.16;

                return color;
            }
            ENDCG
        }
    }
}
