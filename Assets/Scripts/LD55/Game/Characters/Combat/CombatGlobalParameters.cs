using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LD55.Game {
	public static class CombatGlobalParameters {
		private static Dictionary<Team, HashSet<ICombatTarget>> targetsPerTeam { get; } = new Dictionary<Team, HashSet<ICombatTarget>>();

		public static void SubscribeTarget(ICombatTarget combatTarget) {
			if (!targetsPerTeam.ContainsKey(combatTarget.Team)) targetsPerTeam.Add(combatTarget.Team, new HashSet<ICombatTarget>());
			targetsPerTeam[combatTarget.Team].Add(combatTarget);
		}

		public static void UnsubscribeTarget(ICombatTarget combatTarget) {
			targetsPerTeam[combatTarget.Team].Remove(combatTarget); 
			Debug.Log("");
		}

		public static void Clear() => targetsPerTeam.Clear();

		public static bool TryGetClosest(Team withinTeam, Vector2 position, float allowedRadius, out ICombatTarget foundTarget) {
			foundTarget = default;
			if (!targetsPerTeam.ContainsKey(withinTeam)) return false;

			var closest = targetsPerTeam[withinTeam].Select(t => (t, (t.Position - position).sqrMagnitude)).OrderBy(t => t.sqrMagnitude).FirstOrDefault();
			foundTarget = closest.t;
			return closest.sqrMagnitude < allowedRadius * allowedRadius;
		}
	}
}