using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LD55.Game {
	public static class CombatGlobalParameters {
		private static Dictionary<Team, HashSet<ICombatTarget>> targetsPerTeam { get; } = new Dictionary<Team, HashSet<ICombatTarget>>();

		private static IReadOnlyList<ICombatTarget> EmptyTargetArray { get; } = Array.Empty<ICombatTarget>();

		public static void SubscribeTarget(ICombatTarget combatTarget) {
			if (!targetsPerTeam.ContainsKey(combatTarget.Team)) targetsPerTeam.Add(combatTarget.Team, new HashSet<ICombatTarget>());
			targetsPerTeam[combatTarget.Team].Add(combatTarget);
		}

		public static void UnsubscribeTarget(ICombatTarget combatTarget) => targetsPerTeam[combatTarget.Team].Remove(combatTarget);

		public static void Clear() => targetsPerTeam.Clear();

		public static bool TryGetClosest(Team withinTeam, Vector2 position, float allowedRadius, out ICombatTarget foundTarget) {
			foundTarget = default;
			if (!targetsPerTeam.ContainsKey(withinTeam)) return false;

			var closest = targetsPerTeam[withinTeam].Select(t => (t, (t.Position - position).sqrMagnitude)).OrderBy(t => t.sqrMagnitude).FirstOrDefault();
			foundTarget = closest.t;
			return foundTarget != null && closest.sqrMagnitude < allowedRadius * allowedRadius;
		}

		public static IReadOnlyList<ICombatTarget> GetAllInRange(Team withinTeam, Vector2 position, float radius) {
			if (!targetsPerTeam.ContainsKey(withinTeam)) return EmptyTargetArray;
			return targetsPerTeam[withinTeam].Where(t => (t.Position - position).sqrMagnitude < radius * radius).ToArray();
		}
	}
}