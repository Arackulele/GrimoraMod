using UnityEngine;

namespace GrimoraMod;

public static class UnityObjectExtensions
{
	public static bool IsNotNull(this UnityEngine.Object obj)
	{
		return obj;
	}
	
	public static bool IsNull(this UnityEngine.Object obj)
	{
		return !obj;
	}
	
	public static void SetAlbedoTexture(this Material self, Texture texture)
	{
		self.SetTexture(GrimoraColors.Albedo, texture);
	}
	
	public static void SetEmissionColor(this Material self, Color color)
	{
		self.SetColor(GrimoraColors.EmissionColor, color);
	} 
}
