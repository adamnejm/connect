using System;

namespace Connect
{
	public class Helper
	{
		public static void PrintWithColor(string text, ConsoleColor foreground)
		{
			var prevForeground = Console.ForegroundColor;

			Console.ForegroundColor = foreground;
			Console.Write(text);

			Console.ForegroundColor = prevForeground;
		}

		public static void PrintWithColor(string text, ConsoleColor foreground, ConsoleColor background)
		{
			var prevForeground = Console.ForegroundColor;
			var prevBackground = Console.BackgroundColor;

			Console.ForegroundColor = foreground;
			Console.BackgroundColor = background;
			Console.Write(text);

			Console.ForegroundColor = prevForeground;
			Console.BackgroundColor = prevBackground;
		}

	}
}
