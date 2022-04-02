Shader "Neon Play/Flipbook PFX"
{
    Properties
    {
		_BaseColor("Color", Color) = (0.5,0.5,0.5,0.5)
        _MainTex ("Texture", 2D) = "white" {}
        _columns ("Columns", int) = 1
        _rows ("Rows", int) = 1
        _fps ("FPS", int) = 30
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent+5" "IgnoreProjector"="True" }
        LOD 100

        Pass
        {
            ZWrite Off
            ZTest Always
            Blend SrcAlpha One// SrcAlpha OneMinusSrcAlpha
			AlphaTest Greater .01
		    ColorMask RGB

			BindChannels {
				Bind "Color", color
			}

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
        
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                fixed color : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
				fixed color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed _columns;
            fixed _rows;
            fixed _fps;
			fixed4 _BaseColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                
                half xOff = 1 / _columns;
                half yOff = 1 / _rows;
                int frames = _rows * _columns;
                int index = (_Time.y + v.color.x) % frames * _fps;
                
                _MainTex_ST = float4(xOff, yOff, index % _columns * xOff, yOff * (_rows - 1 - (floor(index / _columns) % _rows)));
                
				o.color = v.color;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				fixed4 col = tex2D(_MainTex, i.uv) * i.color * _BaseColor;
                return col;
            }
            ENDCG
        }
    }
}
