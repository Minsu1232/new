// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "KumaBeer/Flow_Tessellation"
{
	Properties
	{
		[HDR]_EmissionColor("Emission Color", Color) = (0.3411765,0.3411765,0.3411765,1)
		_AlbedoColor("Albedo Color", Color) = (0.3411765,0.3411765,0.3411765,1)
		_TessValue( "Max Tessellation", Range( 1, 32 ) ) = 25.1
		_TessMin( "Tess Min Distance", Float ) = 0.5
		_TessMax( "Tess Max Distance", Float ) = 5.2
		_Albedo("Albedo", 2D) = "white" {}
		_NormalMap("Normal Map", 2D) = "bump" {}
		_Emission("Emission", 2D) = "gray" {}
		_Smoothness("Smoothness", 2D) = "gray" {}
		_Noisetiling("Noise tiling", Float) = 1
		_Noiseflow("Noise flow", Range( 0 , 1)) = 0.35
		_Noise("Noise", 2D) = "white" {}
		_Displacement("Displacement", Float) = 0.04
		_TextureMoving("Texture Moving", Vector) = (0,0.03,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityStandardUtils.cginc"
		#include "Tessellation.cginc"
		#pragma target 4.6
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc tessellate:tessFunction 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Albedo;
		uniform float2 _TextureMoving;
		uniform sampler2D _Noise;
		uniform float _Noiseflow;
		uniform float _Displacement;
		uniform sampler2D _NormalMap;
		uniform float _Noisetiling;
		uniform sampler2D _Emission;
		uniform float4 _AlbedoColor;
		uniform float4 _EmissionColor;
		uniform sampler2D _Smoothness;
		uniform float _TessValue;
		uniform float _TessMin;
		uniform float _TessMax;

		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			return UnityDistanceBasedTess( v0.vertex, v1.vertex, v2.vertex, _TessMin, _TessMax, _TessValue );
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float3 ase_vertexNormal = v.normal.xyz;
			float2 panner23 = ( 1.0 * _Time.y * _TextureMoving + ( ( tex2Dlod( _Noise, float4( v.texcoord.xy, 0, 0.0) ).r * (0.01 + (_Noiseflow - 0.0) * (0.03 - 0.01) / (1.0 - 0.0)) ) + v.texcoord.xy ));
			float4 tex2DNode1 = tex2Dlod( _Albedo, float4( panner23, 0, 0.0) );
			v.vertex.xyz += ( ( ase_vertexNormal * tex2DNode1.a ) * _Displacement );
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 panner23 = ( 1.0 * _Time.y * _TextureMoving + ( ( tex2D( _Noise, i.uv_texcoord ).r * (0.01 + (_Noiseflow - 0.0) * (0.03 - 0.01) / (1.0 - 0.0)) ) + i.uv_texcoord ));
			float2 temp_cast_0 = (_Noisetiling).xx;
			float mulTime68 = _Time.y * 1.3;
			float2 uv_TexCoord69 = i.uv_texcoord * temp_cast_0 + ( _TextureMoving * mulTime68 );
			float4 tex2DNode82 = tex2D( _Noise, uv_TexCoord69 );
			float4 tex2DNode3 = tex2D( _Emission, panner23 );
			float2 lerpResult83 = lerp( panner23 , ( panner23 + tex2DNode82.r ) , tex2DNode3.r);
			o.Normal = UnpackScaleNormal( tex2D( _NormalMap, lerpResult83 ), 1.0 );
			float4 tex2DNode1 = tex2D( _Albedo, panner23 );
			float4 appendResult11 = (float4(tex2DNode1.r , tex2DNode1.g , tex2DNode1.b , 0.0));
			o.Albedo = ( _AlbedoColor * appendResult11 ).rgb;
			o.Emission = ( _EmissionColor * ( ( tex2DNode82.r * tex2DNode3 ) + float4( 0,0,0,0 ) ) ).rgb;
			o.Metallic = 0.0;
			o.Smoothness = tex2D( _Smoothness, lerpResult83 ).a;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18900
273;830;1536;747;1345.073;-37.94157;1.233343;True;True
Node;AmplifyShaderEditor.TextureCoordinatesNode;113;-3007.326,533.9557;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;117;-2917.399,716.1691;Inherit;False;Property;_Noiseflow;Noise flow;12;0;Create;True;0;0;0;False;0;False;0.35;0.25;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;118;-2598.199,718.9127;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0.01;False;4;FLOAT;0.03;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;112;-2685.326,516.9556;Inherit;True;Property;_TextureSample0;Texture Sample 0;13;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Instance;82;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;115;-2369.962,566.1163;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;26;-3018.658,1006.197;Inherit;False;Property;_TextureMoving;Texture Moving;15;0;Create;True;0;0;0;False;0;False;0,0.03;0,0.03;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleTimeNode;68;-2977.345,1295.398;Inherit;False;1;0;FLOAT;1.3;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;29;-2405.159,837.2963;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;111;-2757.866,1158.931;Inherit;False;Property;_Noisetiling;Noise tiling;11;0;Create;True;0;0;0;False;0;False;1;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;114;-2161.401,772.6801;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;74;-2800.166,1272.658;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;69;-2583.182,1242.578;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;23;-2130.652,988.5828;Inherit;False;3;0;FLOAT2;0,0.03;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;3;-1900.245,697.3801;Inherit;True;Property;_Emission;Emission;9;0;Create;True;0;0;0;False;0;False;-1;None;78f6d6c3403cecb489ffb81a03ced9e9;True;0;False;gray;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;82;-2199.418,1275.375;Inherit;True;Property;_Noise;Noise;13;0;Create;True;0;0;0;False;0;False;-1;None;bb747a7a3693d7d468a7d0d37298a0ab;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;87;-1919.078,128.7985;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;81;-1544.494,777.5728;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;1;-1420.664,-8.885885;Inherit;True;Property;_Albedo;Albedo;7;0;Create;True;0;0;0;False;0;False;-1;None;a702928b1fc89d1468440e20f77c30da;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;88;-905.7086,401.5527;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;52;-762.6207,317.3574;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;71;-1799.003,1220.818;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;109;-1335.042,783.8916;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;30;-1886.478,496.1586;Inherit;False;Property;_EmissionColor;Emission Color;0;1;[HDR];Create;True;0;0;0;False;0;False;0.3411765,0.3411765,0.3411765,1;47.93726,2.509804,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;35;-595.0403,608.4158;Inherit;False;Property;_Displacement;Displacement;14;0;Create;True;0;0;0;False;0;False;0.04;0.04;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;-1374.958,506.484;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;83;-1512.03,987.3472;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-1499.687,298.6326;Inherit;False;Constant;_Normalpower;Normal power;4;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;57;-534.5118,385.3857;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;56;-386.7042,383.7992;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;11;-1027.041,21.35964;Inherit;False;COLOR;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;93;-931.7228,485.5095;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;7;-1419.55,-234.6983;Inherit;False;Property;_AlbedoColor;Albedo Color;1;0;Create;True;0;0;0;False;0;False;0.3411765,0.3411765,0.3411765,1;0.3411764,0.3411764,0.3411764,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;2;-1307.013,251.9178;Inherit;True;Property;_NormalMap;Normal Map;8;0;Create;True;0;0;0;False;0;False;-1;None;bc3b37740f3d8034cad7f59109cd33bf;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;42;-250.1182,385.3189;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-407.3405,83.15464;Inherit;False;Constant;_Float0;Float 0;3;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-861.1439,-227.6665;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;86;-673.166,43.42694;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;6;-561.312,166.8872;Inherit;True;Property;_Smoothness;Smoothness;10;0;Create;True;0;0;0;False;0;False;-1;None;5e0e14fddf9a63d4cb2741583c34ac87;True;0;False;gray;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;85;-653.3531,110.4501;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-184.9257,-36.02411;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;KumaBeer/Flow_Tessellation;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;0;25.1;0.5;5.2;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;2;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;118;0;117;0
WireConnection;112;1;113;0
WireConnection;115;0;112;1
WireConnection;115;1;118;0
WireConnection;114;0;115;0
WireConnection;114;1;29;0
WireConnection;74;0;26;0
WireConnection;74;1;68;0
WireConnection;69;0;111;0
WireConnection;69;1;74;0
WireConnection;23;0;114;0
WireConnection;23;2;26;0
WireConnection;3;1;23;0
WireConnection;82;1;69;0
WireConnection;87;0;23;0
WireConnection;81;0;82;1
WireConnection;81;1;3;0
WireConnection;1;1;87;0
WireConnection;88;0;1;4
WireConnection;71;0;23;0
WireConnection;71;1;82;1
WireConnection;109;0;81;0
WireConnection;31;0;30;0
WireConnection;31;1;109;0
WireConnection;83;0;23;0
WireConnection;83;1;71;0
WireConnection;83;2;3;1
WireConnection;57;0;52;0
WireConnection;57;1;88;0
WireConnection;56;0;57;0
WireConnection;56;1;35;0
WireConnection;11;0;1;1
WireConnection;11;1;1;2
WireConnection;11;2;1;3
WireConnection;93;0;31;0
WireConnection;2;1;83;0
WireConnection;2;5;9;0
WireConnection;42;0;56;0
WireConnection;8;0;7;0
WireConnection;8;1;11;0
WireConnection;86;0;2;0
WireConnection;6;1;83;0
WireConnection;85;0;93;0
WireConnection;0;0;8;0
WireConnection;0;1;86;0
WireConnection;0;2;85;0
WireConnection;0;3;5;0
WireConnection;0;4;6;4
WireConnection;0;11;42;0
ASEEND*/
//CHKSM=94D636EA05CFB69CFC24B95473F0FC46133317EA