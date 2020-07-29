Shader "Unlit/Outline2"
{
    Properties
    {
        _Color ("Outline Color", Color) = (1,0,0,0)
        _OutlineWidth ("Outline Width", float) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        Zwrite ON
        Ztest LEqual
        Cull Front

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;                
            };
            
            float4 _Color;
            float _OutlineWidth;

            v2f vert (appdata v)
            {
                v2f o;
                v.vertex.xyz += v.normal * _OutlineWidth;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
            
                return _Color;
            }
            ENDCG
        }
    }
}
