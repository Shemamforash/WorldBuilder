// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

 Shader "TintedHeight" {
 
    Properties 
    {
      _MainTex ("Base (RGB)", 2D) = "white" {}
      _HeightMin ("Height Min", Float) = -1
      _Color ("Color", Color) = (0,0,0,1)
	  _DivVal ("Divider", Float) = 5
    }

     SubShader {
         Pass {
 
             CGPROGRAM
 
             #pragma vertex vert
             #pragma fragment frag
             #include "UnityCG.cginc"
 
             float _HeightMin;
             fixed4 _Color;
			 float _DivVal;
 
             struct v2f {
                 float4 pos : SV_POSITION;
                 fixed3 color : COLOR0;
             };
 
             v2f vert (appdata_base v)
            {
                v2f o;
                float3 wpos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.pos = UnityObjectToClipPos(v.vertex);
				float x = abs(wpos.x);
				float y = abs(wpos.y);
				float z = abs(wpos.z);
				float d = sqrt(x * x + y * y + z * z);
				d -= 1;
				d *= _DivVal;
				o.color = float4(0.45 + d, 0.3 + d, 0.17 + d, 1);
                return o;
            }

 
             fixed4 frag (v2f i) : SV_Target
             {
                 return fixed4 (i.color, 1);
             }
             ENDCG
 
         }
     }
 }