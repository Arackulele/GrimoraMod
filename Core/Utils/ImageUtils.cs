using APIPlugin;
using UnityEngine;

namespace GrimoraMod;

public static class ImageUtils
{
	// ReSharper disable all InconsistentNaming
	public static readonly Texture Energy1 = LoadTextureFromFile("Energy1");
	public static readonly Texture Energy2 = LoadTextureFromFile("Energy2");
	public static readonly Texture Energy3 = LoadTextureFromFile("Energy3");
	public static readonly Texture Energy4 = LoadTextureFromFile("Energy4");
	public static readonly Texture Energy5 = LoadTextureFromFile("Energy5");
	public static readonly Texture Energy6 = LoadTextureFromFile("Energy6");

	public static Texture2D LoadTextureFromBytes(byte[] nameOfCardArt)
	{
		var texture = new Texture2D(2, 2)
		{
			filterMode = FilterMode.Point
		};
		texture.LoadImage(nameOfCardArt);
		return texture;
	}

	public static Texture2D LoadTextureFromFile(string nameOfCardArt)
	{
		nameOfCardArt = nameOfCardArt.Replace("ara_", "");
		if (!nameOfCardArt.EndsWith(".png"))
		{
			nameOfCardArt += ".png";
		}

		return LoadTextureFromBytes(
			FileUtils.ReadFileAsBytes(nameOfCardArt)
		);
	}

	public static Sprite CreateSpriteFromFile(string nameOfCardArt)
	{
		return Sprite.Create(
			LoadTextureFromFile(nameOfCardArt),
			CardUtils.DefaultCardArtRect,
			new Vector2(0.5f, 0.65f),
			125f
		);
	}
}