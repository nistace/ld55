using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LD55.Game {
	public static class CombatGlobalParameters {
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

		public static bool TryGetTarget(Team team, Vector2 position, out ICombatTarget foundTarget) {
			var closest = targetsPerTeam[team].Select(t => (t, (t.Position - position).sqrMagnitude)).OrderBy(t => t.sqrMagnitude).FirstOrDefault();
			foundTarget = closest.t;
			return closest.sqrMagnitude < .5f * .5f;
		}
	}
}