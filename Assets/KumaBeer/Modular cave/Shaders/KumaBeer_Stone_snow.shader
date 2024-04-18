// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "KumaBeer/Stone_snow"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTiling("Main Tiling", Float) = 1
		_Albedo("Albedo", 2D) = "white" {}
		_MetallicSmoothness("MetallicSmoothness", 2D) = "white" {}
		_Normalmap("Normal map", 2D) = "bump" {}
		_Normalscale("Normal scale", Float) = 1
		_AOscale("AO scale", Float) = 1
		_AmbientOcclusion("Ambient Occlusion", 2D) = "white" {}
		[Toggle(_USEDETAIL_ON)] _Usedetail("Use detail", Float) = 1
		_DetailAlbedo("Detail Albedo", 2D) = "white" {}
		_Detailnormalscale("Detail normal scale", Float) = 1
		_DetailNormal("Detail Normal", 2D) = "bump" {}
		_Detailmask("Detail mask", 2D) = "white" {}
		[Toggle(_TRIPLANARDETAILUSEINSTEAD_ON)] _Triplanardetailuseinstead("Triplanar detail use instead", Float) = 0
		_TriplanarDetailTiling("Triplanar Detail Tiling", Float) = 1
		_TriplanarDetailAlbedo("Triplanar Detail Albedo", 2D) = "white" {}
		_TriplanarDetailNormal("Triplanar Detail Normal", 2D) = "bump" {}
		_TriplanarColor("Triplanar Color", Color) = (1,1,1,1)
		_Triplanarvalue("Triplanar value", Float) = 1
		[Toggle(_USEDETAILMASKFORTRIPLANAR_ON)] _Usedetailmaskfortriplanar("Use detail mask for triplanar", Float) = 0
		_Detailmasktriplanar("Detail mask triplanar", Float) = 1
		_TriplanarSmoothness("Triplanar Smoothness", Range( 0 , 1)) = 1
		_TriplanarTiling("Triplanar Tiling", Float) = 1
		_TriplanarAlbedo("TriplanarAlbedo", 2D) = "white" {}
		_TriplanarNormalScale("TriplanarNormal Scale", Float) = 1
		_TriplanarNormal("TriplanarNormal", 2D) = "bump" {}
		[Toggle(_USESNOWNOISE_ON)] _UseSnownoise("Use Snow noise", Float) = 0
		_SnownoiseTiling("Snow noise Tiling", Float) = 1
		_Snownoisescale("Snow noise scale", Float) = 1.5
		_Snownoise("Snow noise", 2D) = "gray" {}
		[Toggle(_USENOISE_ON)] _UseNoise("Use Noise", Float) = 0
		_NoiseTiling("Noise Tiling", Float) = 1
		_Noise("Noise", 2D) = "white" {}
		_Overlaynoise("Overlay noise", Float) = 0
		_Blendnormals("Blend normals", Range( 0 , 1)) = 0.5
		_Noisevalue("Noise value", Float) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGINCLUDE
		#include "UnityStandardUtils.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma shader_feature_local _USESNOWNOISE_ON
		#pragma shader_feature_local _USEDETAIL_ON
		#pragma shader_feature_local _USENOISE_ON
		#pragma shader_feature_local _USEDETAILMASKFORTRIPLANAR_ON
		#pragma shader_feature_local _TRIPLANARDETAILUSEINSTEAD_ON
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
		};

		uniform sampler2D _Normalmap;
		uniform float _MainTiling;
		uniform float _Normalscale;
		sampler2D _TriplanarNormal;
		uniform float _TriplanarTiling;
		uniform float _TriplanarNormalScale;
		uniform float _Blendnormals;
		uniform float _Triplanarvalue;
		uniform sampler2D _DetailNormal;
		uniform float4 _DetailNormal_ST;
		uniform float _Detailnormalscale;
		sampler2D _TriplanarDetailNormal;
		uniform float _TriplanarDetailTiling;
		uniform sampler2D _Detailmask;
		uniform float _Detailmasktriplanar;
		sampler2D _Noise;
		uniform float _NoiseTiling;
		uniform float _Noisevalue;
		uniform float _Overlaynoise;
		uniform float _Snownoisescale;
		sampler2D _Snownoise;
		uniform float _SnownoiseTiling;
		uniform sampler2D _Albedo;
		sampler2D _TriplanarAlbedo;
		uniform float4 _TriplanarColor;
		uniform sampler2D _DetailAlbedo;
		uniform float4 _DetailAlbedo_ST;
		sampler2D _TriplanarDetailAlbedo;
		uniform float4 _Color;
		uniform sampler2D _MetallicSmoothness;
		uniform float4 _MetallicSmoothness_ST;
		uniform float _TriplanarSmoothness;
		uniform sampler2D _AmbientOcclusion;
		uniform float4 _AmbientOcclusion_ST;
		uniform float _AOscale;


		inline float3 TriplanarSampling243( sampler2D topTexMap, float3 worldPos, float3 worldNormal, float falloff, float2 tiling, float3 normalScale, float3 index )
		{
			float3 projNormal = ( pow( abs( worldNormal ), falloff ) );
			projNormal /= ( projNormal.x + projNormal.y + projNormal.z ) + 0.00001;
			float3 nsign = sign( worldNormal );
			half4 xNorm; half4 yNorm; half4 zNorm;
			xNorm = tex2D( topTexMap, tiling * worldPos.zy * float2(  nsign.x, 1.0 ) );
			yNorm = tex2D( topTexMap, tiling * worldPos.xz * float2(  nsign.y, 1.0 ) );
			zNorm = tex2D( topTexMap, tiling * worldPos.xy * float2( -nsign.z, 1.0 ) );
			xNorm.xyz  = half3( UnpackScaleNormal( xNorm, normalScale.y ).xy * float2(  nsign.x, 1.0 ) + worldNormal.zy, worldNormal.x ).zyx;
			yNorm.xyz  = half3( UnpackScaleNormal( yNorm, normalScale.x ).xy * float2(  nsign.y, 1.0 ) + worldNormal.xz, worldNormal.y ).xzy;
			zNorm.xyz  = half3( UnpackScaleNormal( zNorm, normalScale.y ).xy * float2( -nsign.z, 1.0 ) + worldNormal.xy, worldNormal.z ).xyz;
			return normalize( xNorm.xyz * projNormal.x + yNorm.xyz * projNormal.y + zNorm.xyz * projNormal.z );
		}


		inline float3 TriplanarSampling364( sampler2D topTexMap, float3 worldPos, float3 worldNormal, float falloff, float2 tiling, float3 normalScale, float3 index )
		{
			float3 projNormal = ( pow( abs( worldNormal ), falloff ) );
			projNormal /= ( projNormal.x + projNormal.y + projNormal.z ) + 0.00001;
			float3 nsign = sign( worldNormal );
			half4 xNorm; half4 yNorm; half4 zNorm;
			xNorm = tex2D( topTexMap, tiling * worldPos.zy * float2(  nsign.x, 1.0 ) );
			yNorm = tex2D( topTexMap, tiling * worldPos.xz * float2(  nsign.y, 1.0 ) );
			zNorm = tex2D( topTexMap, tiling * worldPos.xy * float2( -nsign.z, 1.0 ) );
			xNorm.xyz  = half3( UnpackScaleNormal( xNorm, normalScale.y ).xy * float2(  nsign.x, 1.0 ) + worldNormal.zy, worldNormal.x ).zyx;
			yNorm.xyz  = half3( UnpackScaleNormal( yNorm, normalScale.x ).xy * float2(  nsign.y, 1.0 ) + worldNormal.xz, worldNormal.y ).xzy;
			zNorm.xyz  = half3( UnpackScaleNormal( zNorm, normalScale.y ).xy * float2( -nsign.z, 1.0 ) + worldNormal.xy, worldNormal.z ).xyz;
			return normalize( xNorm.xyz * projNormal.x + yNorm.xyz * projNormal.y + zNorm.xyz * projNormal.z );
		}


		inline float4 TriplanarSampling237( sampler2D topTexMap, float3 worldPos, float3 worldNormal, float falloff, float2 tiling, float3 normalScale, float3 index )
		{
			float3 projNormal = ( pow( abs( worldNormal ), falloff ) );
			projNormal /= ( projNormal.x + projNormal.y + projNormal.z ) + 0.00001;
			float3 nsign = sign( worldNormal );
			half4 xNorm; half4 yNorm; half4 zNorm;
			xNorm = tex2D( topTexMap, tiling * worldPos.zy * float2(  nsign.x, 1.0 ) );
			yNorm = tex2D( topTexMap, tiling * worldPos.xz * float2(  nsign.y, 1.0 ) );
			zNorm = tex2D( topTexMap, tiling * worldPos.xy * float2( -nsign.z, 1.0 ) );
			return xNorm * projNormal.x + yNorm * projNormal.y + zNorm * projNormal.z;
		}


		inline float4 TriplanarSampling348( sampler2D topTexMap, float3 worldPos, float3 worldNormal, float falloff, float2 tiling, float3 normalScale, float3 index )
		{
			float3 projNormal = ( pow( abs( worldNormal ), falloff ) );
			projNormal /= ( projNormal.x + projNormal.y + projNormal.z ) + 0.00001;
			float3 nsign = sign( worldNormal );
			half4 xNorm; half4 yNorm; half4 zNorm;
			xNorm = tex2D( topTexMap, tiling * worldPos.zy * float2(  nsign.x, 1.0 ) );
			yNorm = tex2D( topTexMap, tiling * worldPos.xz * float2(  nsign.y, 1.0 ) );
			zNorm = tex2D( topTexMap, tiling * worldPos.xy * float2( -nsign.z, 1.0 ) );
			return xNorm * projNormal.x + yNorm * projNormal.y + zNorm * projNormal.z;
		}


		inline float4 TriplanarSampling244( sampler2D topTexMap, float3 worldPos, float3 worldNormal, float falloff, float2 tiling, float3 normalScale, float3 index )
		{
			float3 projNormal = ( pow( abs( worldNormal ), falloff ) );
			projNormal /= ( projNormal.x + projNormal.y + projNormal.z ) + 0.00001;
			float3 nsign = sign( worldNormal );
			half4 xNorm; half4 yNorm; half4 zNorm;
			xNorm = tex2D( topTexMap, tiling * worldPos.zy * float2(  nsign.x, 1.0 ) );
			yNorm = tex2D( topTexMap, tiling * worldPos.xz * float2(  nsign.y, 1.0 ) );
			zNorm = tex2D( topTexMap, tiling * worldPos.xy * float2( -nsign.z, 1.0 ) );
			return xNorm * projNormal.x + yNorm * projNormal.y + zNorm * projNormal.z;
		}


		inline float4 TriplanarSampling368( sampler2D topTexMap, float3 worldPos, float3 worldNormal, float falloff, float2 tiling, float3 normalScale, float3 index )
		{
			float3 projNormal = ( pow( abs( worldNormal ), falloff ) );
			projNormal /= ( projNormal.x + projNormal.y + projNormal.z ) + 0.00001;
			float3 nsign = sign( worldNormal );
			half4 xNorm; half4 yNorm; half4 zNorm;
			xNorm = tex2D( topTexMap, tiling * worldPos.zy * float2(  nsign.x, 1.0 ) );
			yNorm = tex2D( topTexMap, tiling * worldPos.xz * float2(  nsign.y, 1.0 ) );
			zNorm = tex2D( topTexMap, tiling * worldPos.xy * float2( -nsign.z, 1.0 ) );
			return xNorm * projNormal.x + yNorm * projNormal.y + zNorm * projNormal.z;
		}


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 temp_cast_0 = (_MainTiling).xx;
			float2 uv_TexCoord372 = i.uv_texcoord * temp_cast_0;
			float3 tex2DNode3 = UnpackScaleNormal( tex2D( _Normalmap, uv_TexCoord372 ), _Normalscale );
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_worldTangent = WorldNormalVector( i, float3( 1, 0, 0 ) );
			float3 ase_worldBitangent = WorldNormalVector( i, float3( 0, 1, 0 ) );
			float3x3 ase_worldToTangent = float3x3( ase_worldTangent, ase_worldBitangent, ase_worldNormal );
			float3 triplanar243 = TriplanarSampling243( _TriplanarNormal, ase_worldPos, ase_worldNormal, 8.0, _TriplanarTiling, _TriplanarNormalScale, 0 );
			float3 tanTriplanarNormal243 = mul( ase_worldToTangent, triplanar243 );
			float3 lerpResult343 = lerp( tanTriplanarNormal243 , BlendNormals( tex2DNode3 , tanTriplanarNormal243 ) , _Blendnormals);
			float2 uv_DetailNormal = i.uv_texcoord * _DetailNormal_ST.xy + _DetailNormal_ST.zw;
			float3 triplanar364 = TriplanarSampling364( _TriplanarDetailNormal, ase_worldPos, ase_worldNormal, 8.0, _TriplanarDetailTiling, _Detailnormalscale, 0 );
			float3 tanTriplanarNormal364 = mul( ase_worldToTangent, triplanar364 );
			#ifdef _TRIPLANARDETAILUSEINSTEAD_ON
				float3 staticSwitch365 = tanTriplanarNormal364;
			#else
				float3 staticSwitch365 = UnpackScaleNormal( tex2D( _DetailNormal, uv_DetailNormal ), _Detailnormalscale );
			#endif
			float3 temp_output_92_0 = BlendNormals( staticSwitch365 , tex2DNode3 );
			#ifdef _USEDETAIL_ON
				float staticSwitch393 = (WorldNormalVector( i , temp_output_92_0 )).y;
			#else
				float staticSwitch393 = (WorldNormalVector( i , tex2DNode3 )).y;
			#endif
			float4 tex2DNode396 = tex2D( _Detailmask, uv_TexCoord372 );
			#ifdef _USEDETAILMASKFORTRIPLANAR_ON
				float staticSwitch397 = ( staticSwitch393 * tex2DNode396.r * _Detailmasktriplanar );
			#else
				float staticSwitch397 = staticSwitch393;
			#endif
			float dotResult22 = dot( _Triplanarvalue , staticSwitch397 );
			float4 triplanar237 = TriplanarSampling237( _Noise, ase_worldPos, ase_worldNormal, 6.0, _NoiseTiling, 1.0, 0 );
			float lerpResult282 = lerp( dotResult22 , ( triplanar237.x * _Noisevalue ) , _Overlaynoise);
			#ifdef _USENOISE_ON
				float staticSwitch374 = lerpResult282;
			#else
				float staticSwitch374 = dotResult22;
			#endif
			float clampResult26 = clamp( staticSwitch374 , 0.0 , 1.0 );
			float3 lerpResult395 = lerp( tex2DNode3 , lerpResult343 , clampResult26);
			float3 lerpResult299 = lerp( temp_output_92_0 , lerpResult343 , clampResult26);
			#ifdef _USEDETAIL_ON
				float3 staticSwitch394 = lerpResult299;
			#else
				float3 staticSwitch394 = lerpResult395;
			#endif
			float temp_output_388_0 = ( 1.0 - _Snownoisescale );
			float4 ase_vertexTangent = mul( unity_WorldToObject, float4( ase_worldTangent, 0 ) );
			ase_vertexTangent = normalize( ase_vertexTangent );
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float dotResult362 = dot( ase_vertexTangent.xyz , ase_worldViewDir );
			float4 triplanar348 = TriplanarSampling348( _Snownoise, ase_worldPos, ase_worldNormal, 8.0, _SnownoiseTiling, 1.0, 0 );
			float temp_output_386_0 = ( triplanar348.x * clampResult26 );
			float lerpResult361 = lerp( temp_output_388_0 , dotResult362 , temp_output_386_0);
			float3 lerpResult358 = lerp( staticSwitch394 , float3( 0,0,0 ) , lerpResult361);
			#ifdef _USESNOWNOISE_ON
				float3 staticSwitch369 = lerpResult358;
			#else
				float3 staticSwitch369 = staticSwitch394;
			#endif
			o.Normal = staticSwitch369;
			float4 tex2DNode2 = tex2D( _Albedo, uv_TexCoord372 );
			float4 triplanar244 = TriplanarSampling244( _TriplanarAlbedo, ase_worldPos, ase_worldNormal, 8.0, _TriplanarTiling, 1.0, 0 );
			float4 temp_output_328_0 = ( triplanar244 * _TriplanarColor );
			float4 lerpResult390 = lerp( tex2DNode2 , temp_output_328_0 , clampResult26);
			float2 uv_DetailAlbedo = i.uv_texcoord * _DetailAlbedo_ST.xy + _DetailAlbedo_ST.zw;
			float temp_output_9_0_g12 = tex2DNode396.r;
			float temp_output_18_0_g12 = ( 1.0 - temp_output_9_0_g12 );
			float3 appendResult16_g12 = (float3(temp_output_18_0_g12 , temp_output_18_0_g12 , temp_output_18_0_g12));
			float4 triplanar368 = TriplanarSampling368( _TriplanarDetailAlbedo, ase_worldPos, ase_worldNormal, 8.0, _TriplanarDetailTiling, 1.0, 0 );
			float temp_output_9_0_g13 = tex2DNode396.r;
			float temp_output_18_0_g13 = ( 1.0 - temp_output_9_0_g13 );
			float3 appendResult16_g13 = (float3(temp_output_18_0_g13 , temp_output_18_0_g13 , temp_output_18_0_g13));
			#ifdef _TRIPLANARDETAILUSEINSTEAD_ON
				float3 staticSwitch375 = ( tex2DNode2.rgb * ( ( ( triplanar368.xyz * (unity_ColorSpaceDouble).rgb ) * temp_output_9_0_g13 ) + appendResult16_g13 ) );
			#else
				float3 staticSwitch375 = ( tex2DNode2.rgb * ( ( ( (tex2D( _DetailAlbedo, uv_DetailAlbedo )).rgb * (unity_ColorSpaceDouble).rgb ) * temp_output_9_0_g12 ) + appendResult16_g12 ) );
			#endif
			float4 lerpResult16 = lerp( float4( staticSwitch375 , 0.0 ) , temp_output_328_0 , clampResult26);
			#ifdef _USEDETAIL_ON
				float4 staticSwitch389 = lerpResult16;
			#else
				float4 staticSwitch389 = lerpResult390;
			#endif
			o.Albedo = ( staticSwitch389 * _Color ).xyz;
			float2 uv_MetallicSmoothness = i.uv_texcoord * _MetallicSmoothness_ST.xy + _MetallicSmoothness_ST.zw;
			float4 tex2DNode4 = tex2D( _MetallicSmoothness, uv_MetallicSmoothness );
			o.Metallic = tex2DNode4.r;
			float lerpResult24 = lerp( tex2DNode4.a , _TriplanarSmoothness , clampResult26);
			float lerpResult382 = lerp( lerpResult24 , temp_output_388_0 , temp_output_386_0);
			#ifdef _USESNOWNOISE_ON
				float staticSwitch370 = lerpResult382;
			#else
				float staticSwitch370 = lerpResult24;
			#endif
			o.Smoothness = staticSwitch370;
			float2 uv_AmbientOcclusion = i.uv_texcoord * _AmbientOcclusion_ST.xy + _AmbientOcclusion_ST.zw;
			o.Occlusion = pow( tex2D( _AmbientOcclusion, uv_AmbientOcclusion ).r , _AOscale );
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows 

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
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
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
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
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
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18935
527;615;1080;430;4335.375;261.0824;1;True;False
Node;AmplifyShaderEditor.RangedFloatNode;371;-2844.308,-735.194;Float;False;Property;_MainTiling;Main Tiling;1;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;367;-3040.818,-1246.595;Float;False;Property;_TriplanarDetailTiling;Triplanar Detail Tiling;17;0;Create;True;0;0;0;False;0;False;1;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;42;-2732.129,-1479.473;Float;False;Property;_Detailnormalscale;Detail normal scale;13;0;Create;True;0;0;0;False;0;False;1;1.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;372;-2696.909,-755.5842;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TriplanarNode;364;-2509,-1321.903;Inherit;True;Spherical;World;True;Triplanar Detail Normal;_TriplanarDetailNormal;bump;19;None;Mid Texture 4;_MidTexture4;white;-1;None;Bot Texture 4;_BotTexture4;white;-1;None;Triplanar Detail Normal;Tangent;10;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;9;FLOAT3;0,0,0;False;8;FLOAT;1;False;3;FLOAT;1;False;4;FLOAT;8;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;39;-2610.105,-586.8998;Float;False;Property;_Normalscale;Normal scale;5;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;7;-2443.825,-1524.693;Inherit;True;Property;_DetailNormal;Detail Normal;14;0;Create;True;0;0;0;False;0;False;-1;None;e6e5d80228a8ee34a8d8cbed50621690;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;3;-2438.715,-675.9926;Inherit;True;Property;_Normalmap;Normal map;4;0;Create;True;0;0;0;False;0;False;-1;None;89a7080e1f26f3745a20b8e299e7edf3;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;365;-2017.502,-1350.433;Inherit;False;Property;_Triplanardetailuseinstead;Triplanar detail use instead;16;0;Create;True;0;0;0;False;0;False;0;0;1;True;;Toggle;2;Key0;Key1;Create;True;True;All;9;1;FLOAT3;0,0,0;False;0;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT3;0,0,0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.BlendNormalsNode;92;-1451.273,-710.6972;Inherit;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldNormalVector;392;-3845.898,-430.6833;Inherit;True;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldNormalVector;305;-3839.852,-689.2628;Inherit;True;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;396;-2412.5,-1108.399;Inherit;True;Property;_Detailmask;Detail mask;15;0;Create;True;0;0;0;False;0;False;-1;None;f5b3c1192d6235942874c2b267f02c9a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;399;-4020.923,23.1346;Inherit;False;Property;_Detailmasktriplanar;Detail mask triplanar;23;0;Create;True;0;0;0;False;0;False;1;0.25;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;393;-3586.609,-387.3017;Inherit;False;Property;_Keyword1;Keyword 1;8;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Reference;389;True;True;All;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;242;-3549.451,702.0248;Float;False;Property;_NoiseTiling;Noise Tiling;34;0;Create;True;0;0;0;False;0;False;1;0.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;398;-3596.299,-91.93198;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;23;-3079.669,491.0536;Float;False;Property;_Triplanarvalue;Triplanar value;21;0;Create;True;0;0;0;False;0;False;1;303.32;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;96;-3210.127,850.7488;Float;False;Property;_Noisevalue;Noise value;38;0;Create;True;0;0;0;False;0;False;1;20;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;397;-3209.024,-121.4259;Inherit;False;Property;_Usedetailmaskfortriplanar;Use detail mask for triplanar;22;0;Create;True;0;0;0;False;0;False;0;0;1;True;;Toggle;2;Key0;Key1;Create;True;True;All;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TriplanarNode;237;-3322.127,658.7484;Inherit;True;Spherical;World;False;Noise;_Noise;white;35;None;Mid Texture 0;_MidTexture0;white;-1;None;Bot Texture 0;_BotTexture0;white;-1;None;Noise;Tangent;10;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;9;FLOAT3;0,0,0;False;8;FLOAT;1;False;3;FLOAT;1;False;4;FLOAT;6;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DotProductOpNode;22;-2839.669,507.0536;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;245;-2758.792,-102.1007;Float;False;Property;_TriplanarTiling;Triplanar Tiling;25;0;Create;True;0;0;0;False;0;False;1;3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;336;-2853.713,746.8182;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;226;-2890.106,873.5752;Float;False;Property;_Overlaynoise;Overlay noise;36;0;Create;True;0;0;0;False;0;False;0;-5.67;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;253;-2835.331,14.99541;Float;False;Property;_TriplanarNormalScale;TriplanarNormal Scale;27;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;282;-2618.312,830.3818;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TriplanarNode;243;-2488.157,-26.69507;Inherit;True;Spherical;World;True;TriplanarNormal;_TriplanarNormal;bump;28;None;Mid Texture 1;_MidTexture1;white;-1;None;Bot Texture 1;_BotTexture1;white;-1;None;TriplanarNormal;Tangent;10;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;9;FLOAT3;0,0,0;False;8;FLOAT;1;False;3;FLOAT;1;False;4;FLOAT;8;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;374;-2427.591,793.0771;Inherit;False;Property;_UseNoise;Use Noise;33;0;Create;True;0;0;0;False;0;False;0;0;1;True;;Toggle;2;Key0;Key1;Create;True;True;All;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-2437.474,-880.3486;Inherit;True;Property;_Albedo;Albedo;2;0;Create;True;0;0;0;False;0;False;-1;None;1f7b80891ee80384ab8d969ed4a39845;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;349;-1742.572,1127.343;Float;False;Property;_SnownoiseTiling;Snow noise Tiling;30;0;Create;True;0;0;0;False;0;False;1;7;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TriplanarNode;368;-2823.818,-1017.324;Inherit;True;Spherical;World;False;Triplanar Detail Albedo;_TriplanarDetailAlbedo;white;18;None;Mid Texture 5;_MidTexture5;white;-1;None;Bot Texture 5;_BotTexture5;white;-1;None;Triplanar Detail Albedo;Tangent;10;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;9;FLOAT3;0,0,0;False;8;FLOAT;1;False;3;FLOAT;1;False;4;FLOAT;8;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;344;-2141.2,212.99;Inherit;False;Property;_Blendnormals;Blend normals;37;0;Create;True;0;0;0;False;0;False;0.5;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.BlendNormalsNode;342;-2068.341,105.1426;Inherit;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TriplanarNode;348;-1444.151,1082.634;Inherit;True;Spherical;World;False;Snow noise;_Snownoise;gray;32;None;Mid Texture 3;_MidTexture3;white;-1;None;Bot Texture 3;_BotTexture3;white;-1;None;Snow noise;Tangent;10;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;9;FLOAT3;0,0,0;False;8;FLOAT;1;False;3;FLOAT;1;False;4;FLOAT;8;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TangentVertexDataNode;363;-926.3698,219.48;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;26;-2000.654,798.1854;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;387;-1314.625,952.265;Float;False;Property;_Snownoisescale;Snow noise scale;31;0;Create;True;0;0;0;False;0;False;1.5;2.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;343;-1654.456,63.86873;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.FunctionNode;300;-2053.073,-938.8726;Inherit;False;Detail Albedo;9;;13;29e5a290b15a7884983e27c8f1afaa8c;0;3;12;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;9;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;327;-2094.144,-153.2762;Float;False;Property;_TriplanarColor;Triplanar Color;20;0;Create;True;0;0;0;False;0;False;1,1,1,1;0.645804,0.7169812,0.4565679,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;376;-2048.582,-1110.948;Inherit;False;Detail Albedo;9;;12;29e5a290b15a7884983e27c8f1afaa8c;0;3;12;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;9;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TriplanarNode;244;-2494.593,-241.1776;Inherit;True;Spherical;World;False;TriplanarAlbedo;_TriplanarAlbedo;white;26;None;Mid Texture 2;_MidTexture2;white;-1;None;Bot Texture 2;_BotTexture2;white;-1;None;TriplanarAlbedo;Tangent;10;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;9;FLOAT3;0,0,0;False;8;FLOAT;1;False;3;FLOAT;1;False;4;FLOAT;8;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;354;-940.1161,371.2056;Inherit;True;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.OneMinusNode;388;-936.3936,843.2239;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;4;-1670.11,528.2733;Inherit;True;Property;_MetallicSmoothness;MetallicSmoothness;3;0;Create;True;0;0;0;False;0;False;-1;None;9f397ff35b99fb344b33182f8d9dfed5;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;133;-1635.171,736.5449;Float;False;Property;_TriplanarSmoothness;Triplanar Smoothness;24;0;Create;True;0;0;0;False;0;False;1;0.1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;375;-1797.142,-1035.905;Inherit;False;Property;_Keyword0;Keyword 0;16;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Reference;365;True;True;All;9;1;FLOAT3;0,0,0;False;0;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT3;0,0,0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;299;-1231.101,115.5815;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;386;-1005.616,957.9124;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;328;-1902.109,-268.0987;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DotProductOpNode;362;-643.3698,349.48;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;395;-1218.183,-53.55421;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;390;-1391.177,-1062.386;Inherit;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.LerpOp;24;-1312.751,716.7692;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;394;-815.0031,78.57449;Inherit;False;Property;_Keyword2;Keyword 2;8;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Reference;389;True;True;All;9;1;FLOAT3;0,0,0;False;0;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT3;0,0,0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;43;-2101.405,-809.0144;Float;False;Property;_Color;Color;0;0;Create;True;0;0;0;False;0;False;1,1,1,1;1,1,1,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;361;-379.9142,328.5163;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;16;-1438.731,-892.3278;Inherit;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.LerpOp;358;-172.6839,200.4491;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;382;-712.9805,819.7455;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;389;-1137.043,-937.0038;Inherit;False;Property;_Usedetail;Use detail;8;0;Create;True;0;0;0;False;0;False;0;1;1;True;;Toggle;2;Key0;Key1;Create;True;True;All;9;1;FLOAT4;0,0,0,0;False;0;FLOAT4;0,0,0,0;False;2;FLOAT4;0,0,0,0;False;3;FLOAT4;0,0,0,0;False;4;FLOAT4;0,0,0,0;False;5;FLOAT4;0,0,0,0;False;6;FLOAT4;0,0,0,0;False;7;FLOAT4;0,0,0,0;False;8;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.WireNode;325;-1328.531,-777.6099;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;263;210.3186,637.3594;Float;False;Property;_AOscale;AO scale;6;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;5;70.83138,433.8791;Inherit;True;Property;_AmbientOcclusion;Ambient Occlusion;7;0;Create;True;0;0;0;False;0;False;-1;None;279f2279a2e248448addbe8a4c992b6f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;369;29.4812,78.2301;Inherit;False;Property;_UseSnownoise;Use Snow noise;29;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;All;9;1;FLOAT3;0,0,0;False;0;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT3;0,0,0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;-905.7563,-862.6557;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.PowerNode;264;392.4201,463.5553;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;370;-412.4489,555.1689;Inherit;False;Property;_Usesnownoisesmoothness;Use snow noise smoothness;29;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Reference;369;True;True;All;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;617.9836,-0.2000937;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;KumaBeer/Stone_snow;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;18;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;2;False;-1;2;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;372;0;371;0
WireConnection;364;8;42;0
WireConnection;364;3;367;0
WireConnection;7;5;42;0
WireConnection;3;1;372;0
WireConnection;3;5;39;0
WireConnection;365;1;7;0
WireConnection;365;0;364;0
WireConnection;92;0;365;0
WireConnection;92;1;3;0
WireConnection;392;0;3;0
WireConnection;305;0;92;0
WireConnection;396;1;372;0
WireConnection;393;1;392;2
WireConnection;393;0;305;2
WireConnection;398;0;393;0
WireConnection;398;1;396;1
WireConnection;398;2;399;0
WireConnection;397;1;393;0
WireConnection;397;0;398;0
WireConnection;237;3;242;0
WireConnection;22;0;23;0
WireConnection;22;1;397;0
WireConnection;336;0;237;1
WireConnection;336;1;96;0
WireConnection;282;0;22;0
WireConnection;282;1;336;0
WireConnection;282;2;226;0
WireConnection;243;8;253;0
WireConnection;243;3;245;0
WireConnection;374;1;22;0
WireConnection;374;0;282;0
WireConnection;2;1;372;0
WireConnection;368;3;367;0
WireConnection;342;0;3;0
WireConnection;342;1;243;0
WireConnection;348;3;349;0
WireConnection;26;0;374;0
WireConnection;343;0;243;0
WireConnection;343;1;342;0
WireConnection;343;2;344;0
WireConnection;300;12;2;0
WireConnection;300;11;368;0
WireConnection;300;9;396;1
WireConnection;376;12;2;0
WireConnection;376;9;396;1
WireConnection;244;3;245;0
WireConnection;388;0;387;0
WireConnection;375;1;376;0
WireConnection;375;0;300;0
WireConnection;299;0;92;0
WireConnection;299;1;343;0
WireConnection;299;2;26;0
WireConnection;386;0;348;1
WireConnection;386;1;26;0
WireConnection;328;0;244;0
WireConnection;328;1;327;0
WireConnection;362;0;363;0
WireConnection;362;1;354;0
WireConnection;395;0;3;0
WireConnection;395;1;343;0
WireConnection;395;2;26;0
WireConnection;390;0;2;0
WireConnection;390;1;328;0
WireConnection;390;2;26;0
WireConnection;24;0;4;4
WireConnection;24;1;133;0
WireConnection;24;2;26;0
WireConnection;394;1;395;0
WireConnection;394;0;299;0
WireConnection;361;0;388;0
WireConnection;361;1;362;0
WireConnection;361;2;386;0
WireConnection;16;0;375;0
WireConnection;16;1;328;0
WireConnection;16;2;26;0
WireConnection;358;0;394;0
WireConnection;358;2;361;0
WireConnection;382;0;24;0
WireConnection;382;1;388;0
WireConnection;382;2;386;0
WireConnection;389;1;390;0
WireConnection;389;0;16;0
WireConnection;325;0;43;0
WireConnection;369;1;394;0
WireConnection;369;0;358;0
WireConnection;34;0;389;0
WireConnection;34;1;325;0
WireConnection;264;0;5;1
WireConnection;264;1;263;0
WireConnection;370;1;24;0
WireConnection;370;0;382;0
WireConnection;0;0;34;0
WireConnection;0;1;369;0
WireConnection;0;3;4;1
WireConnection;0;4;370;0
WireConnection;0;5;264;0
ASEEND*/
//CHKSM=7CEC596FCC3625E3450806FC46295EB6CD482C62