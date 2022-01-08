using UnityEngine;

namespace GrimoraMod
{
	public static class ImageUtils
	{
		public static Texture2D LoadTextureFromResource(byte[] resourceFile)
		{
			var texture = new Texture2D(2, 2);
			texture.LoadImage(resourceFile);
			return texture;
		}
	}
}