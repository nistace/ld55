using UnityEngine;

namespace LD55.Game {
	[CreateAssetMenu(menuName = "Combat/" + nameof(TankAiCombatStrategy))]
	public class TankAiCombatStrategy : AiCombatStrategy {
		[SerializeField] protected float tankRange = .3f;

		private float SqrTankRange => tankRange * tankRange;

		public override void Solve(ICombatant self, ICombatTarget target) {
			if (self == null) return;
			if (target == null) return;

			var selfToTarget = target.Position - self.Position;

			if (selfToTarget.sqrMagnitude > SqrTankRange) {
				self.Move(target.Position - self.Position);
			}
		}
	}
}