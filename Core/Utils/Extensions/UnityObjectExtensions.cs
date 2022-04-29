using UnityEngine;

namespace GrimoraMod;

public static class UnityObjectExtensions
{
	private static readonly int SrcBlend = Shader.PropertyToID("_SrcBlend");
	private static readonly int DstBlend = Shader.PropertyToID("_DstBlend");
	private static readonly int ZWrite = Shader.PropertyToID("_ZWrite");

	public enum BlendMode
	{
		Opaque,
		Cutout,
		Fade,
		Transparent
	}

	public static bool IsNotNull(this object obj)
	{
		return obj != null;
	}

	public static void SetAlbedoTexture(this Material self, Texture texture)
	{
		self.SetTexture(GrimoraColors.Albedo, texture);
	}

	public static void SetEmissionColor(this Material self, Color color)
	{
		self.SetColor(GrimoraColors.EmissionColor, color);
	}

	public static void ChangeRenderMode(this Material material, BlendMode blendMode)
	{
		switch (blendMode)
		{
			case BlendMode.Opaque:
				material.SetInt(SrcBlend, (int)UnityEngine.Rendering.BlendMode.One);
				material.SetInt(DstBlend, (int)UnityEngine.Rendering.BlendMode.Zero);
				material.SetInt(ZWrite, 1);
				material.DisableKeyword("_ALPHATEST_ON");
				material.DisableKeyword("_ALPHABLEND_ON");
				material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
				material.renderQueue = -1;
				break;
			case BlendMode.Cutout:
				material.SetInt(SrcBlend, (int)UnityEngine.Rendering.BlendMode.One);
				material.SetInt(DstBlend, (int)UnityEngine.Rendering.BlendMode.Zero);
				material.SetInt(ZWrite, 1);
				material.EnableKeyword("_ALPHATEST_ON");
				material.DisableKeyword("_ALPHABLEND_ON");
				material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
				material.renderQueue = 2450;
				break;
			case BlendMode.Fade:
				material.SetInt(SrcBlend, (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
				material.SetInt(DstBlend, (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
				material.SetInt(ZWrite, 0);
				material.DisableKeyword("_ALPHATEST_ON");
				material.EnableKeyword("_ALPHABLEND_ON");
				material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
				material.renderQueue = 3000;
				break;
			case BlendMode.Transparent:
				material.SetInt(SrcBlend, (int)UnityEngine.Rendering.BlendMode.One);
				material.SetInt(DstBlend, (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
				material.SetInt(ZWrite, 0);
				material.DisableKeyword("_ALPHATEST_ON");
				material.DisableKeyword("_ALPHABLEND_ON");
				material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
				material.renderQueue = 3000;
				break;
		}
	}
}
