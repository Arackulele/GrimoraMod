namespace GrimoraMod;

public class ChessNode
{
	public const char PathNode = '0';
	public const char BlockerNode = '1';
	public const char ChestNode = '2';
	public const char EnemyNode = '3';
	public const char BossNode = '4';
	public const char CardRemovalNode = '5';
	public const char BoneyardNode = '6';
	public const char ElectricChairNode = '7';
	public const char GoatEyeNode = '8';
	public const char PlayerNode = '9';
	
	public const char ConsumableNode = 'i';


	private readonly ChessRow _row;
	private readonly int _index;
	public char JsonValue;
	public readonly bool IsBlocker;
	public readonly bool isBoss;
	public readonly bool isCardRemoval;
	public readonly bool isGainConsumable;
	public readonly bool isChest;
	public readonly bool isEnemy;
	public readonly bool IsPath;
	public readonly bool isPlayer;

	public int GridX => _index;
	public int GridY => _row.Index;


	public ChessNode(char jsonValue, int index, ChessRow row)
	{
		switch (jsonValue)
		{
			case PathNode:
				IsPath = true;
				break;
			case BlockerNode:
				IsBlocker = true;
				break;
			case ChestNode:
				isChest = true;
				break;
			case EnemyNode:
				isEnemy = true;
				break;
			case BossNode:
				isBoss = true;
				break;
			case CardRemovalNode:
				isCardRemoval = true;
				break;
			case ConsumableNode:
				isGainConsumable = true;
				break;
			case PlayerNode:
				isPlayer = true;
				break;
		}

		this._index = index;
		this._row = row;
		this.JsonValue = jsonValue;
	}

	public string GetCoords()
	{
		return $"x{_index}_y{_row.Index}";
	}

	public override string ToString()
	{
		if (IsPath)
		{
			return $"IsPath_{GetCoords()}";
		}

		if (IsBlocker)
		{
			return $"IsBlocker_{GetCoords()}";
		}

		if (isChest)
		{
			return $"IsChest_{GetCoords()}";
		}

		if (isEnemy)
		{
			return $"IsEnemy_{GetCoords()}";
		}

		if (isBoss)
		{
			return $"IsBoss_{GetCoords()}";
		}

		if (isPlayer)
		{
			return $"IsPlayer_{GetCoords()}";
		}

		if (isGainConsumable)
		{
			return $"IsGainConsumable_{GetCoords()}";
		}

		if (isCardRemoval)
		{
			return $"IsCardRemoval_{GetCoords()}";
		}
		
		return $"IsUnknown({JsonValue})_{GetCoords()}";
	}
}
