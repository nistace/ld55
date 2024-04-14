using NiUtils.Extensions;
using UnityEngine;

namespace LD55.Game {
	[CreateAssetMenu(menuName = "Combat/" + nameof(StaticAiCombatStrategy))]
	public class StaticAiCombatStrategy : AiCombatStrategy {
		[SerializeField] protected float attackRange = .5f;
		[SerializeField] protected int attackDamage = 1;
		[SerializeField] protected float attackSpeed = 1;

		private float DelayBetweenAttacks => 1 / attackSpeed;

		public override void Solve(ICombatant self) {
			if (self == null) return;

			if (self.IsNextAttackReady()) {
				foreach (var t in GetAllOpponentsInRange(self, attackRange)) {
					t.TakeDamage(attackDamage);
				}
				self.SetDelayBeforeNextAttack(DelayBetweenAttacks);
			}
		}

		public override ICombatTarget FindTarget(ICombatant self) => default;
	}
}