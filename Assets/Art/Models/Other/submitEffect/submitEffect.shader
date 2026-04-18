Shader "Custom/submitEffect"
{
    Properties
{
	_Color("Color", Color) = (1,1,1)
	_MainTex("Texture", 2D) = ""
}

SubShader
{
	Blend SrcAlpha OneMinusSrcAlpha
	
	Tags {Queue = Transparent}
	
	BindChannels
	{
		Bind "vertex", vertex
		Bind "color", color
	}
	Pass
	{
		SetTexture[_MainTex]
		{
			combine texture * primary
		}
	}
}
}
