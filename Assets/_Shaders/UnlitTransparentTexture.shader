// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/TextureTint" {
	Properties {
		_MainTex ("Main Texture", 2D) = "white" {}
		_Color ("Main Color", Color) = (1, 1, 1, 1)
	}
	
	SubShader {
    Tags {Queue=Transparent}
    Blend SrcAlpha OneMinusSrcAlpha
	ZWrite On
		Pass {			
			CGPROGRAM
			#pragma vertex vert  
	        #pragma fragment frag
	        
	        uniform float4 _Color;
	        uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
	        
	        struct vIn {
	        	float4 vertex : POSITION;
	        	float4 texcoord : TEXCOORD0;
	        };
	        struct v2f {
	        	float4 pos : SV_POSITION;
	        	float4 uv : TEXCOORD0;
	        };
	        
	        v2f vert(vIn input) {
	        	v2f output;
	        	output.pos = UnityObjectToClipPos(input.vertex);
	        	output.uv = input.texcoord;
	        	return output;
	        }
	        
	        float4 frag(v2f input) : COLOR {
	        	float4 col = tex2D(_MainTex, input.uv.xy * _MainTex_ST.xy + _MainTex_ST.zw);
	        	col *= _Color;
	        	return col;
	        }

			
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
