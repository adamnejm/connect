using Connect.Modules;
using System;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;

namespace Connect
{
	public class Connect
	{
		public bool noClear;
		public bool startRandom;

		private GameBoard _board;
		private Player[] _players;
		private int _currentPlayerId;

		public Player GetPlayer()
		{
			return _players[_currentPlayerId];
		}

		public void SwitchPlayer()
		{
			_currentPlayerId = (_currentPlayerId + 1) % _players.Length;
		}

		private int _mistakeCounter;
		private void HandleUserInput(out bool doSwitchPlayer)
		{
			doSwitchPlayer = false;

			// Stop inputs from queueing, thanks gandjustas! https://stackoverflow.com/a/3769828
			while (Console.KeyAvailable)
				Console.ReadKey(true);

			switch (Console.ReadKey(true).Key)
			{
				case ConsoleKey.RightArrow:
					_board.MoveDiscPointer(1);
					break;

				case ConsoleKey.LeftArrow:
					_board.MoveDiscPointer(-1);
					break;

				case ConsoleKey.Spacebar:
				case ConsoleKey.DownArrow:
					if (_board.InsertDisc())
						doSwitchPlayer = true;
					break;

				default:
					if (++_mistakeCounter > 4)
					{
						_mistakeCounter = 0;

						Console.Beep(); // punish the user for his horrible mistakes
						Console.WriteLine("Use [Left] and [Right] arrows to move the pointer");
						Console.WriteLine("Press [Space] or [Down] arrow to drop the disc");
						Console.WriteLine("Press any key to continue...");
						Console.ReadKey();
					}
					break;
			}
		}

		private void GameOver()
		{
			if (!noClear)
				Console.Clear();

			if (_board.isDraw)
			{
				Console.WriteLine("It's a draw!");
			}
			else
			{
				Helper.PrintWithColor(GetPlayer().Name, GetPlayer().GetDisc().ForegroundColor);
				Console.Write($" wins!{Environment.NewLine}");
			}

			_board.DisplayBoard();

			Console.WriteLine("Play again? (Y/n)");
			var input = Console.ReadLine();
			if (string.IsNullOrEmpty(input) || input.ToLower().StartsWith("y"))
				RestartGame();
		}

		private void GameLoop()
		{
			while (true)
			{
				if (!noClear)
					Console.Clear();

				_board.DisplayDiscPointer();
				Console.Write(Environment.NewLine);
				_board.DisplayBoard();

				HandleUserInput(out bool doSwitchPlayer);
				if (doSwitchPlayer)
				{
					if (_board.HasFinished())
					{
						GameOver();
						break;
					}
					else
					{
						SwitchPlayer();
					}
				}

			}
		}

		public void NewGame(int w, int h, int needed)
		{
			_board = new GameBoard(this, w, h, needed);
			
			_players = new Player[]
			{
				new Player(0, "Red", "X", ConsoleColor.Red, ConsoleColor.DarkRed),
				new Player(1, "Yellow", "O", ConsoleColor.Yellow, ConsoleColor.DarkYellow)
			};

			if (startRandom)
            {
				var rnd = new Random();
				_players = _players.OrderBy(ply => rnd.Next()).ToArray();
            }


			GameLoop();
		}

		public void RestartGame ()
		{
			_board.Empty();
			SwitchPlayer();
			GameLoop();
		}
	}
}
