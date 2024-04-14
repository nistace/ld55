using System;

namespace LD55.Game {
	[Flags]
	public enum Teams {
		Player = 1 << Team.Player,
		Enemy = 1 << Team.Enemy,
	}
}