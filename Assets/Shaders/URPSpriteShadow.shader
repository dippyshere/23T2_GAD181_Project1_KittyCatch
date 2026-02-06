Shader "Universal Render Pipeline/2D/Sprite-Lit-Default"
{
    Properties
    {
        _MainTex("Diffuse", 2D) = "white" {}

		_ShadowOffset("Shadow Offset", Vector) = (0.02,-0.02,0,0)
		_ShadowColor("Shadow Color", Color) = (0, 0, 0, 0.5)

        // Legacy properties. They're here so that materials using this shader can gracefully fallback to the legacy sprite shader.
        [HideInInspector] _Color("Tint", Color) = (1,1,1,1)
        [HideInInspector] PixelSnap ("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _AlphaTex ("External Alpha", 2D) = "white" {}
        [HideInInspector] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
    }

    SubShader
    {
        Tags {"Queue" = "Transparent" "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" }

        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
	    {
        CGPROGRAM
		    #pragma vertex SpriteVertShadow
		    #pragma fragment SpriteFragShadow
		    #pragma multi_compile_instancing
            #include "UnitySprites.cginc"

			fixed4 _ShadowColor;
            fixed4 _ShadowOffset;

            v2f SpriteVertShadow(appdata_t IN)
            {
                v2f OUT;

                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

                #ifdef UNITY_INSTANCING_ENABLED
					IN.vertex.xy *= _Flip;
                #endif

                float4 worldPos = mul(unity_ObjectToWorld, IN.vertex);
                worldPos.xyz += _ShadowOffset.xyz;
                OUT.vertex = UnityWorldToClipPos(worldPos);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color * _Color * _RendererColor;

                return OUT;
            }

            fixed4 SpriteFragShadow(v2f IN) : SV_Target
            {
                fixed4 c = SampleSpriteTexture(IN.texcoord);
                c.rgb = _ShadowColor;
                c.a = c.a * _ShadowColor.a;
                return c;
            }

		ENDCG
		}

        Pass
        {
            Tags { "LightMode" = "UniversalForward" "Queue"="Transparent" "RenderType"="Transparent"}

            HLSLPROGRAM
            #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/Core2D.hlsl"

            #pragma vertex UnlitVertex
            #pragma fragment UnlitFragment

            struct Attributes
            {
                COMMON_2D_INPUTS
                half4 color : COLOR;
                UNITY_SKINNED_VERTEX_INPUTS
            };

            struct Varyings
            {
                COMMON_2D_OUTPUTS
                half4 color : COLOR;
            };

            #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/2DCommon.hlsl"

            // GPU Instancing
            #pragma multi_compile_instancing
            #pragma multi_compile _ DEBUG_DISPLAY SKINNED_SPRITE

            // NOTE: Do not ifdef the properties here as SRP batcher can not handle different layouts.
            CBUFFER_START(UnityPerMaterial)
                half4 _Color;
            CBUFFER_END

            float4 _MainTex_ST;
            half4 _RendererColor;

            Varyings UnlitVertex(Attributes input)
            {
                SetUpSpriteInstanceProperties();
                input.positionOS = UnityFlipSprite(input.positionOS, unity_SpriteProps.xy);

                Varyings o = CommonUnlitVertex(input);
                o.color = input.color *_Color * unity_SpriteColor;
                return o;
            }

            float4 UnlitFragment(Varyings input) : SV_Target
            {
                return CommonUnlitFragment(input, input.color);
            }
            ENDHLSL
        }
    }
}
