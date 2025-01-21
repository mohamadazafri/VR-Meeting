Shader "Custom/TwoSidedShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            Cull Off // Disable backface culling
            Lighting On
            SetTexture [_MainTex] { combine texture * primary }
        }
    }
}

