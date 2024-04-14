using UnityEngine;

namespace LD55.Game {
	public abstract class AiCombatStrategy : ScriptableObject {
		public abstract void Solve(ICombatant self);
		public abstract ICombatTarget FindTarget(ICombatant self);

		protected static ICombatTarget GetClosestOpponent(ICombatant self) =>
			CombatGlobalParameters.TryGetClosest(self.Team.Opponent(), self.Position, 20, out var bestTarget) ? bestTarget : default;
	}
}