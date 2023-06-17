namespace GrimoraMod;

public class ChessRow
{
	public readonly List<ChessNode> Columns;
	protected internal readonly int Index;

	public ChessRow(IEnumerable<char> columns, int index)
	{
		this.Columns = columns.Select((c, idx) => new ChessNode(c, idx, this)).ToList();
		this.Index = index;
	}

	public bool HasOpenPathNodes()
	{
		return this.Columns.Any(c => c.IsPath);
	}

	public List<ChessNode> GetNodesOfType(char type)
	{
		return this.Columns.Where(c => c.JsonValue == type).ToList();
	}
}
