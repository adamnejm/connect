using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Connect.Modules
{
	public class GameBoard
	{
		public bool isDraw;

		private readonly Disc[,] _board;
		private readonly Connect _connect;
		private readonly Disc _emptyDisc;

		private int _discPointer;
		private readonly int _width;
		private readonly int _height;
		private readonly int _needed;

		private int _lastDiscX;
		private int _lastDiscY;


		public GameBoard(Connect connect, int w, int h, int needed)
		{
			_connect = connect;
			_width = w;
			_height = h;
			_needed = needed;

			_emptyDisc = new Disc
			{
				PlayerId = -1,
				Symbol = "_",
				ForegroundColor = ConsoleColor.Cyan,
				BackgroundColor = ConsoleColor.Blue
			};

			_board = new Disc[w, h];

			Empty();
		}

		public void MoveDiscPointer(int offset)
		{
			if (offset > 0 && _discPointer < _width - 1)
				_discPointer++;
			else if (offset < 0 && _discPointer > 0)
				_discPointer--;
		}

		public bool InsertDisc()
		{
			var x = _discPointer;

			if (_board[x, 0] != _emptyDisc)
				return false;

			int y = 0;
			while (++y <= _height)
			{
				if (y == _height || _board[x, y] != _emptyDisc)
				{
					_board[x, --y] = _connect.GetPlayer().GetDisc();
					_lastDiscX = x;
					_lastDiscY = y;
					break;
				}
			}

			return true;
		}

		private int[][] checkOffsets = new int[4][]
		{
			new int[] {1, 0}, // horizontal
			new int[] {0, 1}, // vertical
			new int[] {1, 1}, // diagonal descending
			new int[] {-1, 1}, // diagonal ascending
		};

		public bool HasFinished()
		{
			var playerDisc = _connect.GetPlayer().GetDisc();
			var winningDiscs = new List<(int, int)>();

			// 4 directional check
			for (int i = 0; i < 4; i++)
			{
				var offset = checkOffsets[i % 4];
				var currentWinningLine = new List<(int, int)>();

				// check along the axis and it's mirror
				for (int j = 0; j < 2; j++)
				{
					int mirror = j == 0 ? 1 : -1;
					var x = _lastDiscX;
					var y = _lastDiscY;

					// contain within board dimensions
					do
					{
						var disc = _board[x, y];

						if (disc != playerDisc)
						{
							break;
						}
						else if (!currentWinningLine.Contains((x, y)))
						{
							currentWinningLine.Add((x, y));
						}	

						x += offset[0] * mirror;
						y += offset[1] * mirror;
					}
					while (x >= 0 && x < _width && y >= 0 && y < _height);
				}

				if (currentWinningLine.Count >= _needed)
					winningDiscs = winningDiscs.Union(currentWinningLine).ToList();
				
			}

			if (winningDiscs.Count > 0)
			{
				MarkWinningDiscs(winningDiscs);
				return true;
			}
			
			// check draw
			if (_lastDiscY == 0)
			{
				for (int x = 0; x < _width; x++)
					if (_board[x, 0] == _emptyDisc)
						return false;

				return isDraw = true;
			}

			return false;
		}

		private void MarkWinningDiscs(List<(int,int)> discsPositions)
		{

			foreach (var discPos in discsPositions)
			{
				var (x, y) = discPos;
				var markedDisc = _board[x, y].Clone();
				markedDisc.BackgroundColor = ConsoleColor.White;

				_board[x, y] = markedDisc;
			}
		}

		public void Empty()
        {
			for (int x = 0; x < _width; x++)
				for (int y = 0; y < _height; y++)
					_board[x, y] = _emptyDisc.Clone();
		}

		public void DisplayDiscPointer()
		{
			for (int i = 0; i < _width; i++)
			{
				if (i == _discPointer)
				{
					Disc currentDisc = _connect.GetPlayer().GetDisc();
					Helper.PrintWithColor("V ", currentDisc.ForegroundColor);
				}
				else
				{
					Helper.PrintWithColor("- ", ConsoleColor.Gray);
				}
			}
		}

		public void DisplayBoard()
		{
			for (int y = 0; y < _height; y++)
			{
				for (int x = 0; x < _width; x++)
				{
					var disc = _board[x, y];
					Helper.PrintWithColor(disc.Symbol, disc.ForegroundColor, disc.BackgroundColor);

					if (x < _width - 1)
						Helper.PrintWithColor(" ", ConsoleColor.White, ConsoleColor.Blue);
				}

				Console.Write(Environment.NewLine);
			}

			Console.ResetColor();
		}

	}
}
