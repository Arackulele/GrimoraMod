using UnityEngine;

namespace GrimoraMod;

public static class GrimoraColors
{
	public static readonly int Albedo = Shader.PropertyToID("_Albedo");

	public static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");
	
	public static readonly Color DefaultEmission = new Color(0.9647f, 0.5059f, 0.1451f, 1);

	public static readonly Color GrimoraEnergyCost = new(0.420f, 1f, 0.63f, 0.25f);

	public static readonly Color GrimoraText = new Color(0.420f, 1f, 0.63f, 1);

	public static readonly Color ElectricChairLight = new Color(0, 1, 1, 1);

	public static readonly Color GrimoraBossCardLight = new Color(0.55f, 0.1f, 0.72f, 1);

	public static readonly Color AlphaZeroBlack = new Color(0, 0, 0, 0);

	public static readonly Color ResourceEnergyCell = new Color(1, 1, 0.23f, 1);
}
