Shader "Custom/UIBlurWithGrabPass"
{
    Properties
    {
        _BlurSize ("Blur Amount", Range(0, 10)) = 3
        _TintColor ("Tint Color", Color) = (1, 1, 1, 0.5)
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        GrabPass { "_GrabTexture" }  // 현재 화면을 가져옴

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _GrabTexture;
            float4 _TintColor;
            float _BlurSize;

            v2f vert (float4 vertex : POSITION, float2 uv : TEXCOORD0)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(vertex);
                o.uv = uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 texelSize = _BlurSize * float2(1.0 / _ScreenParams.x, 1.0 / _ScreenParams.y);
                fixed4 color = tex2D(_GrabTexture, i.uv) * 0.2;

                // 9개의 샘플을 사용하여 블러 적용
                for (int x = -1; x <= 1; x++) {
                    for (int y = -1; y <= 1; y++) {
                        color += tex2D(_GrabTexture, i.uv + float2(x, y) * texelSize) * 0.1;
                    }
                }

                // 최종 색상 적용
                color *= _TintColor;
                return color;
            }
            ENDHLSL
        }
    }
}