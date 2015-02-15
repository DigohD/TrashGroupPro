Shader "Custom/LightShafts" 
{
	Properties 
	{
		_Color ( "Tint Color", Color ) = ( 1.0, 1.0, 1.0, 1.0 )
	}
	SubShader 
	{
		Tags { "Queue"="Transparent" "RenderType"="Opaque" }
		
		// To allow for objects behind transparent objects to be drawn
		ZWrite Off
		
		Blend SrcAlpha OneMinusSrcAlpha
		
		//LOD 200
		
		Pass
		{ 
		
		CGPROGRAM
		#pragma exclude_renderers ps3 xbox360
		#pragma vertex vert
		#pragma fragment frag
		
		#include "UnityCG.cginc"
		
		//uniforms
		uniform fixed4 _Color;
		
		struct vertexInput
		{
			float4 vertex : POSITION;
			float2 texCoord : TEXCOORD0;
		};
		
		struct fragmentInput
		{
			float4 position : SV_POSITION;
			float4 color : COLOR0;
		};
		
		float sinn(float x)
		{
		
			return sin(x)/2.0+0.5;
		
		}

		float CausticPatternFn(float2 pos)
		{
		
			return (sin(pos.x*40.0+_Time.y)
					+pow(sin(-pos.x*130.0+_Time.y),1.0)
					+pow(sin(pos.x*30.0+_Time.y),2.0)
					+pow(sin(pos.x*50.0+_Time.y),2.0)
					+pow(sin(pos.x*80.0+_Time.y),2.0)
					+pow(sin(pos.x*90.0+_Time.y),2.0)
					+pow(sin(pos.x*12.0+_Time.y),2.0)
					+pow(sin(pos.x*6.0+_Time.y),2.0)
					+pow(sin(-pos.x*13.0+_Time.y),5.0))/2.0;
		
		}

		float2 CausticDistortDomainFn(float2 pos)
		{
			pos.x*=(pos.y*0.20+0.5);
			pos.x*=1.0+sin(_Time.y/1.0)/10.0;
			
			return pos;
		}
				
		fragmentInput vert( vertexInput input )
		{
			fragmentInput output;
			output.position = mul( UNITY_MATRIX_MVP, input.vertex);
			output.color = _Color;
			
			float2 pos = -input.texCoord.xy;
			pos+=0.5;
			float2 CausticDistortedDomain = CausticDistortDomainFn(pos);
			float CausticShape = clamp(7.0-length(CausticDistortedDomain.x*20.0),0.0,1.0);
			float CausticPattern = CausticPatternFn(CausticDistortedDomain);
			float Caustic = CausticShape*CausticPattern;
			Caustic *= (pos.y+0.5)/4.0;
			float f = length(pos+float2(-0.5,0.5))*length(pos+float2(0.5,0.5))*(1.0+Caustic)/1.0; //set last devide (alpah) back to 1.0
			
			output.color = output.color * f;
			//output.color = float4(Caustic,0,0,1);
			
			return output;
		}
		
		half4 frag( fragmentInput input ) : COLOR
		{
			return input.color;
		}
		
		ENDCG
		}
	} 
	FallBack "Diffuse"
}
