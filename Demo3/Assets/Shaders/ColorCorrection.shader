Shader "Hidden/ColorCorrection"
{
	Properties
	{
		_MainTex ("Base(RGB)", 2D) = "" {}
	}
	
	CGINCLUDE
	
	#include "UnityCG.cginc"
	
	sampler2D _MainTex;
	sampler3D _LookupTex;
	
	// scale value
	float _Scale;
	
	// offset value 
	float _Offset;
	
	float4 frag(v2f_img i) : SV_Target
	{
		// get the color value from the main texture at the given UV position
		float4 c = tex2D(_MainTex, i.uv);
		
		// set the color value what is in the lookup texture at the same UV
		c.rgb = tex3D(_LookupTex, c.rgb * _Scale + _Offset).rgb;
		
		// return it
		return c;
	}	
	ENDCG	
	
	SubShader
	{
		Pass
		{
			ZTest Always Cull Off ZWrite On       
			
			CGPROGRAM       
			#pragma vertex vert_img       
			//the name of our vertex function       
			#pragma fragment frag       
			//the name of our fragment function       
			ENDCG   
		}
	}
	Fallback off
}
