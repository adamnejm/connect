using System;

namespace Connect.Modules
{
	public struct Disc
	{
		public int PlayerId;
		public string Symbol;
		public ConsoleColor ForegroundColor;
		public ConsoleColor BackgroundColor;

		public Disc Clone()
		{
			return new Disc
			{
				PlayerId = this.PlayerId,
				Symbol = this.Symbol,
				ForegroundColor = this.ForegroundColor,
				BackgroundColor = this.BackgroundColor
			};
		}

		public static bool operator ==(Disc lhs, Disc rhs)
		{
			return lhs.Equals(rhs);
		}

		public static bool operator !=(Disc lhs, Disc rhs)
		{
			return !lhs.Equals(rhs);
		}

		// Thanks, Robert Synoradzki! https://stackoverflow.com/a/48231713
		public override bool Equals(object obj)
		{
			if (!(obj is Disc disc))
				return false;

			return PlayerId == disc.PlayerId;
		}

		// Thanks, Damianu!
		public override int GetHashCode()
		{
			return PlayerId.GetHashCode();
		}
	}
}
