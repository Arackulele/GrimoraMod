namespace GrimoraMod;

public class ChessNode
{
	private readonly ChessRow _row;
	private readonly int _index;
	public int JsonValue;
	public readonly bool IsBlocker;
	public readonly bool isBoss;
	public readonly bool isChest;
	public readonly bool isEnemy;
	public readonly bool IsPath;
	public readonly bool isPlayer;

	public int GridX => _index;
	public int GridY => _row.Index;


	public ChessNode(int jsonValue, int index, ChessRow row)
	{
		switch (jsonValue)
		{
			case 0:
				IsPath = true;
				break;
			case 1:
				IsBlocker = true;
				break;
			case 2:
				isChest = true;
				break;
			case 3:
				isEnemy = true;
				break;
			case 4:
				isBoss = true;
				break;
			default:
				isPlayer = true;
				break;
		}

		this._index = index;
		this._row = row;
		this.JsonValue = jsonValue;
	}

	public string GetCoords()
	{
		return $"{_index}_{_row.Index}";
	}

	public override string ToString()
	{
		return IsPath ? "Path" : "isBlocker";
	}
}