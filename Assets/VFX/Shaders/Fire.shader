// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Fire"
{
	Properties
	{
		_Mask1("Mask 1", 2D) = "white" {}
		_Mask2("Mask 2", 2D) = "white" {}
		_GradientTexture("Gradient Texture", 2D) = "white" {}
		_Noise1("Noise 1", 2D) = "white" {}
		_Noise1Speed("Noise 1 Speed", Vector) = (0,0,0,0)
		_Noise2("Noise 2", 2D) = "white" {}
		_Noise2Speed("Noise 2 Speed", Vector) = (0,0,0,0)
		_UVDistortionIntensity("UV Distortion Intensity", Float) = 0
		_UVDistortionOpacityIntensity("UV Distortion Opacity Intensity", Float) = 0.05
		_SmoothstepOpacityIn("Smoothstep Opacity In", Float) = 0
		_SmoothstepOpacityOut("Smoothstep Opacity Out", Float) = 1
		_ColorIntensity("Color Intensity", Float) = 0
		[HDR]_Color("Color", Color) = (1,1,1,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float4 vertexColor : COLOR;
			float2 uv_texcoord;
		};

		uniform sampler2D _GradientTexture;
		uniform sampler2D _Noise1;
		uniform float2 _Noise1Speed;
		uniform float4 _Noise1_ST;
		uniform sampler2D _Noise2;
		uniform float2 _Noise2Speed;
		uniform float4 _Noise2_ST;
		uniform sampler2D _Mask1;
		uniform float4 _Mask1_ST;
		uniform float _UVDistortionIntensity;
		uniform float _ColorIntensity;
		uniform float4 _Color;
		uniform float _SmoothstepOpacityIn;
		uniform float _SmoothstepOpacityOut;
		uniform sampler2D _Mask2;
		uniform float _UVDistortionOpacityIntensity;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Noise1 = i.uv_texcoord * _Noise1_ST.xy + _Noise1_ST.zw;
			float2 panner2 = ( 1.0 * _Time.y * _Noise1Speed + uv_Noise1);
			float2 uv_Noise2 = i.uv_texcoord * _Noise2_ST.xy + _Noise2_ST.zw;
			float2 panner16 = ( 1.0 * _Time.y * _Noise2Speed + uv_Noise2);
			float4 temp_output_18_0 = ( tex2D( _Noise1, panner2 ) + tex2D( _Noise2, panner16 ) );
			float2 uv_Mask1 = i.uv_texcoord * _Mask1_ST.xy + _Mask1_ST.zw;
			float4 tex2DNode9 = tex2D( _GradientTexture, ( ( ( temp_output_18_0 * tex2D( _Mask1, uv_Mask1 ) ) * ( 1.0 - _UVDistortionIntensity ) ) + float4( i.uv_texcoord, 0.0 , 0.0 ) ).rg );
			o.Albedo = ( ( i.vertexColor * tex2DNode9 ) * _ColorIntensity * _Color ).rgb;
			float4 temp_cast_3 = (_SmoothstepOpacityIn).xxxx;
			float4 temp_cast_4 = (_SmoothstepOpacityOut).xxxx;
			float4 smoothstepResult27 = smoothstep( temp_cast_3 , temp_cast_4 , ( tex2DNode9 * tex2D( _Mask2, ( ( temp_output_18_0 * _UVDistortionOpacityIntensity ) + float4( i.uv_texcoord, 0.0 , 0.0 ) ).rg ) ));
			o.Alpha = saturate( smoothstepResult27 ).r;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				half4 color : COLOR0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.color = v.color;
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.vertexColor = IN.color;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18921
272.6667;338;1541;978;1724.503;-387.2531;1.937005;True;False
Node;AmplifyShaderEditor.Vector2Node;15;-2585.99,687.0121;Inherit;False;Property;_Noise2Speed;Noise 2 Speed;6;0;Create;True;0;0;0;False;0;False;0,0;-0.3,-0.7;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;3;-2228.881,35.28811;Inherit;False;0;1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;14;-2589.99,510.0117;Inherit;False;0;17;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;4;-2224.881,212.2881;Inherit;False;Property;_Noise1Speed;Noise 1 Speed;4;0;Create;True;0;0;0;False;0;False;0,0;0,-1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.PannerNode;16;-2287.99,622.0121;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;2;-1926.881,147.2881;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;17;-2055.99,568.0121;Inherit;True;Property;_Noise2;Noise 2;5;0;Create;True;0;0;0;False;0;False;-1;None;c087ede09c5c5dd49954c4f91394708c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-1776.191,102.7426;Inherit;True;Property;_Noise1;Noise 1;3;0;Create;True;0;0;0;False;0;False;-1;None;c1520997eaef8da41b22a2e250a60ece;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;20;-1219.036,979.5369;Inherit;True;Property;_Mask1;Mask 1;0;0;Create;True;0;0;0;False;0;False;-1;None;2b1e7a94a7c7ffa4b82b13d97d98042b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;18;-1472.87,487.2628;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-918.8972,881.6362;Inherit;False;Property;_UVDistortionIntensity;UV Distortion Intensity;7;0;Create;True;0;0;0;False;0;False;0;2.78;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-826.006,678.8663;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;39;-682.2849,934.8795;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;32;-1823.103,1269.418;Inherit;False;Property;_UVDistortionOpacityIntensity;UV Distortion Opacity Intensity;8;0;Create;True;0;0;0;False;0;False;0.05;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;-1455.102,1208.618;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;13;-652.6345,1058.339;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;35;-1275.902,1515.818;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;-646.897,667.6365;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;12;-475.8345,913.0379;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT2;0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;34;-976.7019,1415.018;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT2;0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;9;-291.9017,746.6821;Inherit;True;Property;_GradientTexture;Gradient Texture;2;0;Create;True;0;0;0;False;0;False;-1;None;862801ab3adf5f244a2762ddb19953e1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;25;-531.7617,1321.234;Inherit;True;Property;_Mask2;Mask 2;1;0;Create;True;0;0;0;False;0;False;-1;None;972a5390e0338384b8dd29b1f58e6792;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;28;28.20776,1555.519;Inherit;False;Property;_SmoothstepOpacityIn;Smoothstep Opacity In;9;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;97.47144,1371.849;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;29;-8.792236,1680.519;Inherit;False;Property;_SmoothstepOpacityOut;Smoothstep Opacity Out;10;0;Create;True;0;0;0;False;0;False;1;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;22;-159.8189,1017.206;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SmoothstepOpNode;27;341.8303,1429.345;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;132.146,884.8247;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;24;155.7206,1098.812;Inherit;False;Property;_ColorIntensity;Color Intensity;11;0;Create;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;38;139.873,1183.048;Inherit;False;Property;_Color;Color;12;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,0;2.996079,1.270588,0.5803921,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;30;500.9756,1337.979;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;333.4382,1013.579;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;45;936.674,966.7327;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Fire;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;18;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;16;0;14;0
WireConnection;16;2;15;0
WireConnection;2;0;3;0
WireConnection;2;2;4;0
WireConnection;17;1;16;0
WireConnection;1;1;2;0
WireConnection;18;0;1;0
WireConnection;18;1;17;0
WireConnection;19;0;18;0
WireConnection;19;1;20;0
WireConnection;39;0;6;0
WireConnection;33;0;18;0
WireConnection;33;1;32;0
WireConnection;5;0;19;0
WireConnection;5;1;39;0
WireConnection;12;0;5;0
WireConnection;12;1;13;0
WireConnection;34;0;33;0
WireConnection;34;1;35;0
WireConnection;9;1;12;0
WireConnection;25;1;34;0
WireConnection;26;0;9;0
WireConnection;26;1;25;0
WireConnection;27;0;26;0
WireConnection;27;1;28;0
WireConnection;27;2;29;0
WireConnection;21;0;22;0
WireConnection;21;1;9;0
WireConnection;30;0;27;0
WireConnection;23;0;21;0
WireConnection;23;1;24;0
WireConnection;23;2;38;0
WireConnection;45;0;23;0
WireConnection;45;9;30;0
ASEEND*/
//CHKSM=18A52F684CC9E483B32C0DD196CDA82466DF921C