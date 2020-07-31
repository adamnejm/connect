namespace Connect
{
	public static class Program
	{
		static void Main(string[] args)
		{
			//TODO: Parse args (no ext lib):
			// -w int - width of the board
			// -h int - height of the board
			// -needed int - amount of discs needed to win
			// -noclear - dont clear the console
			// -startrandom - random starting player, otherwise always red

			Connect connect = new Connect();
			connect.NewGame();
		}
	}
}
