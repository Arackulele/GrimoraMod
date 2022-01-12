using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BepInEx;
using DiskCardGame;
using Unity.Cloud.UserReporting.Plugin.SimpleJson;

namespace GrimoraMod
{
	public class GrimoraChessboard
	{
		public readonly List<ChessRow> Rows;

		public GrimoraChessboard(List<List<int>> board)
		{
			this.Rows = board.Select((b, idx) => new ChessRow(b, idx)).ToList();
		}

		public ChessRow GetFirstRowWithOpenPathNode => Rows.SkipWhile(row => !row.HasOpenPathNodes()).First();
		public ChessRow GetLastRowWithOpenPathNode => Rows.SkipWhile(row => !row.HasOpenPathNodes()).Last();

		public List<ChessRow> GetRowsWithOpenPathNodes => Rows.Where(row => row.HasOpenPathNodes()).ToList();

		public List<ChessNode> GetAllOpenPathNodes()
		{
			return Rows.SelectMany(row => row.GetOpenPathNodes()).ToList();
		}

		public List<ChessNode> GetAllBlockerNodes()
		{
			return Rows.SelectMany(row => row.GetBlockerNodes()).ToList();
		}

		public void CreateBlockerPiecesForBoard(ChessboardMap map)
		{
			GrimoraPlugin.Log.LogDebug($"[SetupGamePieces] Creating blocker pieces for the board");
			GetAllBlockerNodes().ForEach(node => ChessPieceUtils.CreateBlockerPiece(map, node.GridX, node.GridY));
		}
	}

	public class ChessRow
	{
		public readonly List<ChessNode> Columns;
		protected internal readonly int Index;

		public ChessRow(List<int> columns, int index)
		{
			this.Columns = columns.Select((c, idx) => new ChessNode(c, idx, this)).ToList();
			this.Index = index;
		}

		public bool HasOpenPathNodes()
		{
			return this.Columns.Any(c => c.IsPath);
		}

		public List<ChessNode> GetOpenPathNodes()
		{
			return this.Columns.Where(c => c.IsPath).ToList();
		}

		public List<ChessNode> GetBlockerNodes()
		{
			return this.Columns.Where(c => c.IsBlocker).ToList();
		}
	}

	public class ChessNode
	{
		private readonly ChessRow _row;
		private readonly int _index;
		public readonly bool IsBlocker;
		public readonly bool IsPath;

		public int GridX => _index;
		public int GridY => _row.Index;

		public ChessNode(int val, int index, ChessRow row)
		{
			this.IsBlocker = val == 1;
			this.IsPath = val == 0;
			this._row = row;
			this._index = index;
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
}