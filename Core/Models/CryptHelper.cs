using DiskCardGame;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class CryptHelper
{
	private const float LightHeight = 8f;

	private const float X0Coord = -70f;
	private const float X1Coord = -56f;
	private const float X2Coord = -35.75f;
	private const float X3Coord = -15f;
	private const float X4Coord = 0f;
	private const float X5Coord = 18.5f;
	private const float X6Coord = 30.5f;

	private const float Z0Coord = 57.5f;  // 65.6 in editor
	private const float Z1Coord = 37.5f;  // 47.5 in editor
	private const float Z2Coord = 17.5f;  // 28 in editor
	private const float Z3Coord = 0f;     // 5 in editor
	private const float Z4Coord = -12.5f; // -15 in editor
	private const float Z5Coord = -28f;   // -25 in editor
	private const float Z6Coord = -40f;   // from editor, minus -2.5. So if editor is -40.5, this Zed value is -43

	public static void SetupNewCryptAndZones()
	{
		if (CryptManager.Instance is not null)
		{
			Log.LogDebug($"Finding structure");
			CryptManager.Instance.transform.Find("Structure").gameObject.SetActive(false);
			Transform interactbles = CryptManager.Instance.transform.Find("Interactables");
			for (int i = 0; i < interactbles.transform.childCount; i++)
			{
				if (interactbles.transform.GetChild(i).name != "EpitaphPuzzle")
				{
					interactbles.transform.GetChild(i).gameObject.SetActive(false);
				}
			}

			Log.LogDebug($"Creating layout");
			GameObject newLayout = Object.Instantiate(
				AssetUtils.GetPrefab<GameObject>("NewNewLayout"),
				CryptManager.Instance.transform
			);

			Log.LogDebug($"Getting nav grid");
			NavigationGrid navGrid = Object.FindObjectsOfType<NavigationGrid>().Single(grid => grid.name == "NavigationGrid");
			navGrid.zones = new NavigationZone[7, 7];
			NavigationZone[,] zones = navGrid.zones;

			Log.LogDebug($"Setting existing zones to null");
			for (int i = 0; i < zones.GetLength(0); i++)
			{
				for (int j = 0; j < zones.GetLength(1); j++)
				{
					// Log.LogDebug($"[Zone] x{i}y{j} Name [{zones[i, j]?.name}]");
					zones[i, j] = null;
					// UnityExplorer.ExplorerCore.Log($"[Zone] x{i}y{j} Parent [{zone.transform.parent.name}] Name [{zone?.name}]");
				}
			}

			NavigationZone3D east3Zone = navGrid.transform.Find("East_3").GetComponent<NavigationZone3D>();

			Log.LogDebug($"Destroying existing zones");
			// destroy existing zones to avoid confusion
			UnityEngine.Object.Destroy(navGrid.transform.Find("East_1").gameObject);
			UnityEngine.Object.Destroy(navGrid.transform.Find("East_2").gameObject);
			UnityEngine.Object.Destroy(navGrid.transform.Find("West_1").gameObject);
			UnityEngine.Object.Destroy(navGrid.transform.Find("West_1").gameObject);
			UnityEngine.Object.Destroy(navGrid.transform.Find("West_2").gameObject);


			#region Lights

			Transform lightsParent = CryptManager.Instance.transform.Find("Lights");
			Light lightToCopy = lightsParent.Find("CryptLight").GetComponent<Light>();
			Light traderRoomLight = Object.Instantiate(
				lightToCopy,
				new Vector3(-70, LightHeight, -27.5f),
				Quaternion.identity,
				lightsParent.transform
			);
			traderRoomLight.range = 24;

			Light kayceesRoomLight = Object.Instantiate(
				traderRoomLight,
				new Vector3(-35, LightHeight, -27.5f),
				Quaternion.identity,
				lightsParent.transform
			);
			kayceesRoomLight.name = "kaycees_room";

			Light wellRoomLight = Object.Instantiate(
				traderRoomLight,
				new Vector3(-35, LightHeight, 19),
				Quaternion.identity,
				lightsParent.transform
			);
			wellRoomLight.name = "well_room";

			Light mirrorRoomLight = Object.Instantiate(
				traderRoomLight,
				new Vector3(30, LightHeight, 57.5f),
				Quaternion.identity,
				lightsParent.transform
			);
			mirrorRoomLight.name = "mirror_room";

			Light doubleCryptRoomLight = Object.Instantiate(
				traderRoomLight,
				new Vector3(30, LightHeight, 19),
				Quaternion.identity,
				lightsParent.transform
			);
			doubleCryptRoomLight.name = "double_crypt_room";

			Light centralRoomEast1Light = Object.Instantiate(
				traderRoomLight,
				new Vector3(37.5f, LightHeight, -42.5f),
				Quaternion.identity,
				lightsParent.transform
			);
			centralRoomEast1Light.name = "central_room_east1";
			
			Light centralRoomEast2Light = Object.Instantiate(
				traderRoomLight,
				new Vector3(37.5f, LightHeight, -10f),
				Quaternion.identity,
				lightsParent.transform
			);
			centralRoomEast2Light.name = "central_room_east2";
			
			Light centralRoomWestLight = Object.Instantiate(
				traderRoomLight,
				new Vector3(-2.5f, LightHeight, -42.5f),
				Quaternion.identity,
				lightsParent.transform
			);
			centralRoomWestLight.name = "central_room_west";

			#endregion

			//

			NavigationZone3D CreateZone(string name, Vector3 position, int x, int y)
			{
				NavigationZone3D newZone = Object.Instantiate(
						east3Zone,
						position,
						Quaternion.identity,
						navGrid.transform
					)
					.GetComponent<NavigationZone3D>();
				newZone.name = $"x{x}_y{y}_" + name;
				Log.LogDebug($"Creating zone [{newZone.name}]");

				navGrid.InsertZone(newZone, x, y);
				return newZone;
			}

			// x0
			var zoneTrader = CreateZone(
				"room_trader",
				new Vector3(X0Coord, 0, Z5Coord),
				0,
				5
			);

			// x1
			var zoneKayceeWestHallway = CreateZone(
				"room_kaycee",
				new Vector3(X1Coord, 0, Z5Coord),
				1,
				5
			);


			// x2
			var zoneKaycee1 = CreateZone(
				"room_kaycee",
				new Vector3(X2Coord, 0, Z2Coord),
				2,
				2
			);

			var zoneKaycee2 = CreateZone(
				"room_kaycee",
				new Vector3(X2Coord, 0, Z3Coord),
				2,
				3
			);

			var zoneKaycee3 = CreateZone(
				"room_kaycee",
				new Vector3(X2Coord, 0, Z4Coord),
				2,
				4
			);

			var zoneKaycee4 = CreateZone(
				"room_kaycee",
				new Vector3(X2Coord, 0, Z5Coord),
				2,
				5
			);

			var zoneKaycee5 = CreateZone(
				"room_kaycee",
				new Vector3(X2Coord, 0, Z6Coord),
				2,
				6
			);


			// x3
			var centralRoomWestHallway = CreateZone(
				"Central_Room_West_Hallway",
				new Vector3(X3Coord, 0, Z5Coord),
				3,
				5
			);

			// x4
			var playerSeat = navGrid.transform.Find("PlayerSeat").GetComponent<NavigationZone>();
			playerSeat.name += "_x4_y3";
			navGrid.InsertZone(playerSeat, 4, 3);

			var tableArea1 = navGrid.transform.Find("TableArea_1").GetComponent<NavigationZone>();
			tableArea1.name += "_x4_y4";
			navGrid.InsertZone(tableArea1, 4, 4);

			var tableArea2 = navGrid.transform.Find("TableArea_2").GetComponent<NavigationZone>();
			tableArea2.name += "_x4_y5";
			navGrid.InsertZone(tableArea2, 4, 5);
			tableArea2.transform.position = new Vector3(0, 0, Z5Coord);

			var tableArea3 = navGrid.transform.Find("TableArea_3").GetComponent<NavigationZone>();
			tableArea3.name += "_x4_y6";
			tableArea3.transform.position = new Vector3(0, 0, Z6Coord);
			navGrid.InsertZone(tableArea3, 4, 6);


			// x5
			var centralRoomCenter1 = CreateZone(
				"Central_Room_Center",
				new Vector3(X5Coord, 0, -11f),
				5,
				4
			);

			var centralRoomCenter2 = CreateZone(
				"Central_Room_Center",
				new Vector3(X5Coord, 0, Z5Coord),
				5,
				5
			);

			var centralRoomCenter3 = CreateZone(
				"Central_Room_Center",
				new Vector3(X5Coord, 0, Z6Coord),
				5,
				6
			);


			// x6
			var mirrorRoom = CreateZone(
				"Mirror_Room",
				new Vector3(X6Coord, 0, Z0Coord),
				6,
				0
			);

			var mirrorRoomHallway = CreateZone(
				"Mirror_Room_Hallway",
				new Vector3(X6Coord, 0, Z1Coord),
				6,
				1
			);

			var skullRoomEast = CreateZone(
				"Skull_Room_East",
				new Vector3(X6Coord, 0, Z2Coord),
				6,
				2
			);

			var skullRoomEastHallway = CreateZone(
				"Skull_Room_East_Hallway",
				new Vector3(X6Coord, 0, Z3Coord),
				6,
				3
			);

			var centralRoomEast1 = CreateZone(
				"Central_Room_East",
				new Vector3(X6Coord, 0, Z4Coord),
				6,
				4
			);

			var centralRoomEast2 = CreateZone(
				"Central_Room_East",
				new Vector3(X6Coord, 0, Z5Coord),
				6,
				5
			);

			var centralRoomEast3 = CreateZone(
				"Central_Room_East",
				new Vector3(X6Coord, 0, Z6Coord),
				6,
				6
			);
		}
	}
}
