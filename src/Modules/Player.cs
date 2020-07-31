using System;

namespace Connect.Modules
{
	public class Player
	{
		public int PlayerId { get; }
		public string Name;

		private Disc _discTemplate;


		public Player(int playerId, string name, string symbol, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
		{
			PlayerId = playerId;
			Name = name;

			_discTemplate = new Disc
			{
				PlayerId = playerId,
				Symbol = symbol,
				ForegroundColor = foregroundColor,
				BackgroundColor = backgroundColor
			};
		}

		public Disc GetDisc()
		{
			return _discTemplate.Clone();
		}
	}
}
