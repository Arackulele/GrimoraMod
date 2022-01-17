using UnityEngine;
using Resources = GrimoraMod.Properties.Resources;

namespace GrimoraMod;

public static class ImageUtils
{
	public static Texture Energy1 = LoadTextureFromResource(Resources.Energy1);
	public static Texture Energy2 = LoadTextureFromResource(Resources.Energy2);
	public static Texture Energy3 = LoadTextureFromResource(Resources.Energy3);
	public static Texture Energy4 = LoadTextureFromResource(Resources.Energy4);
	public static Texture Energy5 = LoadTextureFromResource(Resources.Energy5);
	public static readonly Texture Energy6 = LoadTextureFromResource(Resources.Energy6);

	public static Texture2D LoadTextureFromResource(byte[] resourceFile)
	{
		var texture = new Texture2D(2, 2);
		texture.LoadImage(resourceFile);
		texture.filterMode = FilterMode.Point;
		return texture;
	}
}