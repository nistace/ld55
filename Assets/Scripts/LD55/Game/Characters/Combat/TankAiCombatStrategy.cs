using UnityEngine;

namespace LD55.Game {
	[CreateAssetMenu(menuName = "Combat/" + nameof(TankAiCombatStrategy))]
	public class TankAiCombatStrategy : AiCombatStrategy {
		[SerializeField] protected float tankRange = .3f;

		private float SqrTankRange => tankRange * tankRange;

		public override void Solve(ICombatant self) {
			if (self?.Target == null) return;

			var selfToTarget = self.Target.Position - self.Position;

			if (selfToTarget.sqrMagnitude > SqrTankRange) {
				self.Move(self.Target.Position - self.Position);
			}
		}

		public override ICombatTarget FindTarget(ICombatant self) => GetClosestOpponent(self);
	}
}