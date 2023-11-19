Shader "Custom/StandardWithSettings"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo", 2D) = "white" {}
		_Normal ("Normal", 2D) = "bump" {}
		_NormalScale ("NormalScale", Float) = 1
		_Roughness ("Roughness", 2D) = "white" {}
		_RoughnessScale ("RoughnessScale", Float) = 1
        _Metallic ("Metallic", Range(0,1)) = 0.0

		[HDR] _EmissiveColor ("EmissiveColor", Color) = (0,0,0,0)

		[Space(25)]
		[Toggle] _Clip("Alpha clipping", Float) = 0
		[Header(Blend State)]
		[Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend("SrcBlend", Float) = 1 //"One"
		[Enum(UnityEngine.Rendering.BlendMode)] _DstBlend("DestBlend", Float) = 0 //"Zero"
		[Space(5)]
		[Header(Other)]
		[Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull", Float) = 2 //"Back"
		[Enum(UnityEngine.Rendering.CompareFunction)] _ZTest("ZTest", Float) = 4 //"LessEqual"
		[Enum(Off,0,On,1)] _ZWrite("ZWrite", Float) = 1.0 //"On"
		[Enum(UnityEngine.Rendering.ColorWriteMask)] _ColorWriteMask("ColorWriteMask", Float) = 15 //"All"
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

		Blend[_SrcBlend][_DstBlend]
		ZTest[_ZTest]
		ZWrite[_ZWrite]
		Cull[_Cull]
		ColorMask[_ColorWriteMask]

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard addshadow //fullforwardshadows

        #pragma target 3.0
		#pragma multi_compile __ _CLIP_ON

        sampler2D _MainTex;
		sampler2D _Normal;
		sampler2D _Roughness;

		float _NormalScale;
		float _RoughnessScale;
		float _Metallic;
		float4 _Color;
		float4 _EmissiveColor;

        struct Input
        {
            float2 uv_MainTex;
			float facing : VFACE;
        };

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			float3 n = UnpackScaleNormal(tex2D(_Normal, IN.uv_MainTex), _NormalScale);
			n.z = IN.facing*n.z;
			float r = saturate(tex2D(_Roughness, IN.uv_MainTex) * _RoughnessScale);

			#ifdef _CLIP_ON
				clip(c.a - 0.5);
			#endif

            o.Albedo = c.rgb;
			o.Normal = n.xyz;
            o.Metallic = _Metallic;
            o.Smoothness = 1 - r;
            o.Alpha = c.a;
			o.Emission = _EmissiveColor;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
