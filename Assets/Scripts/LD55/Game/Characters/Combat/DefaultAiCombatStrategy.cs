using UnityEngine;

namespace LD55.Game {
	[CreateAssetMenu(menuName = "Combat/" + nameof(DefaultAiCombatStrategy))]
	public class DefaultAiCombatStrategy : AiCombatStrategy {
		[SerializeField] protected float attackRange = .5f;
		[SerializeField] protected int attackDamage = 1;
		[SerializeField] protected float attackSpeed = 1;

		private float SqrAttackRange => attackRange * attackRange;
		private float DelayBetweenAttacks => 1 / attackSpeed;

		public override void Solve(ICombatant self) {
			if (self?.Target == null) return;

			var selfToTarget = self.Target.Position - self.Position;

			if (selfToTarget.sqrMagnitude > SqrAttackRange) {
				self.Move(self.Target.Position - self.Position);
				return;
			}

			if (!self.Target.IsDead && self.IsNextAttackReady()) {
				self.Target.TakeDamage(attackDamage);
				self.SetDelayBeforeNextAttack(DelayBetweenAttacks);
			}
		}

		public override ICombatTarget FindTarget(ICombatant self) => GetClosestOpponent(self);
	}
}