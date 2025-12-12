Shader "Unlit/Water"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}

        _Color1("Color1", Color) = (1,1,1,1)
        _Color2("Color2", Color) = (1,1,1,1)
        _Color3("Color3", Color) = (1,1,1,1)
        _Color4("Color4", Color) = (1,1,1,1)

        _Amount1("Amount1", Float) = 0.25
        _Amount2("Amount2", Float) = 0.25
        _Amount3("Amount3", Float) = 0.25
        _Amount4("Amount4", Float) = 0.25

        _FillMin("Fill Min", Range(0,1)) = 0.2
        _FillMax("Fill Max", Range(0,1)) = 0.75
        _PosWorld("PosWorld", Vector)=(0,0,0,0)
        _Angle("Angle", Float) = 0
    }
    SubShader
    {
        Pass
        {
            Tags {
                "Queue"="Transparent"
            }
            Blend One OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv1 : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : POSITION;
                float2 uv1 : TEXCOORD0;
                float3 world_Pos: TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _Color1;
            float4 _Color2;
            float4 _Color3;
            float4 _Color4;

            float _Amount1;
            float _Amount2;
            float _Amount3;
            float _Amount4;

            float _FillMin;
            float _FillMax;
            float _FillAmount;
            float4 _PosWorld;
            float _Angle;

            v2f vert(appdata i)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(i.vertex);
                o.uv1 = i.uv1;
                o.world_Pos = mul(unity_ObjectToWorld, i.vertex);
                return o;
            }

            float2 RotateUV(float2 uv, float angle)
            {
                float s = sin(angle);
                float c = cos(angle);

                // rotate around center (0.5,0.5)
                uv -= 0.5;
                float2 r = float2(
                    uv.x * c - uv.y * s,
                    uv.x * s + uv.y * c
                );
                return r + 0.5;
            }

            float4 frag(v2f i):SV_Target
            {
                float4 col = 0;

                // Normalize Amount
                _FillAmount = _Amount1 + _Amount2 + _Amount3 + _Amount4;
                float t = saturate((_FillAmount - _FillMin) / (_FillMax - _FillMin));
                
                float cutoff = lerp(_FillMin, _FillMax, t);
                float _NormAmount1 = lerp(_FillMin, _FillMax, saturate(_Amount1));
                float _NormAmount2 = lerp(_FillMin, _FillMax, saturate(_Amount1 + _Amount2));
                float _NormAmount3 = lerp(_FillMin, _FillMax, saturate(_Amount1 + _Amount2 + _Amount3));
                float _NormAmount4 = lerp(_FillMin, _FillMax, _FillAmount);

                // Control number of color and their amount
                float mask = step(RotateUV(i.uv1, _Angle).y, cutoff);
                float s1 = step(RotateUV(i.uv1, _Angle).y, _NormAmount1);
                float s2 = step(RotateUV(i.uv1, _Angle).y, _NormAmount2) - s1;
                float s3 = step(RotateUV(i.uv1, _Angle).y, _NormAmount3) - step(RotateUV(i.uv1, _Angle).y, _NormAmount2);
                float s4 = step(RotateUV(i.uv1, _Angle).y, _NormAmount4) - step(RotateUV(i.uv1, _Angle).y, _NormAmount3);

                col = lerp(col, _Color4, s4);
                col = lerp(col, _Color3, s3);
                col = lerp(col, _Color2, s2);
                col = lerp(col, _Color1, s1);

                return col * mask;
            }

            ENDCG
        }
    }
}
