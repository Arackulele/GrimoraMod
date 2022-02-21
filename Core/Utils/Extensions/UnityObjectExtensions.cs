using UnityEngine;

namespace GrimoraMod;

public static class UnityObjectExtensions
{
	public static void SetAlbedoTexture(this Material self, Texture texture)
	{
		self.SetTexture(GrimoraColors.Albedo, texture);
	}
	
	public static void SetEmissionColor(this Material self, Color color)
	{
		self.SetColor(GrimoraColors.EmissionColor, color);
	} 
}
