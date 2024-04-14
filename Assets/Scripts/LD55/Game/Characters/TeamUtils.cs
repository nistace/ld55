using System;

namespace LD55.Game {
	public static class TeamUtils {
		public static Team Opponent(this Team first) {
			return first switch {
				Team.Player => Team.Enemy,
				Team.Enemy => Team.Player,
				_ => throw new ArgumentOutOfRangeException(nameof(first), first, null)
			};
		}
	}
}