using System.Collections.Generic;

namespace LD55.Game {
	public static class CombatGlobalParameters {
		public enum Team {
			Player = 0,
			Enemy = 1
		}

		private static Dictionary<Team, HashSet<ICombatTarget>> targetsPerTeam { get; } = new Dictionary<Team, HashSet<ICombatTarget>>();

		public static void SubscribeTargetOfTeam(Team team, ICombatTarget combatTarget) {
			if (!targetsPerTeam.ContainsKey(team)) targetsPerTeam.Add(team, new HashSet<ICombatTarget>());
			targetsPerTeam[team].Add(combatTarget);
		}

		public static void UnsubscribeTarget(ICombatTarget combatTarget) {
			foreach (var teamTargets in targetsPerTeam.Values) {
				teamTargets.Remove(combatTarget);
			}
		}

		public static void Clear() => targetsPerTeam.Clear();
	}
}