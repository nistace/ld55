using UnityEngine;

namespace LD55.Game {
	[CreateAssetMenu(menuName = "Combat/" + nameof(DefaultAiCombatStrategy))]
	public class DefaultAiCombatStrategy : AiCombatStrategy {
		[SerializeField] protected float attackRange = .5f;
		[SerializeField] protected int attackDamage = 1;
		[SerializeField] protected float attackSpeed = 1;

		private float SqrAttackRange => attackRange * attackRange;
		private int AttackDamage => attackDamage;
		private float DelayBetweenAttacks => 1 / attackSpeed;

		public override void Solve(ICombatant self, ICombatTarget target) {
			if (self == null) return;
			if (target == null) return;

			var selfToTarget = target.Position - self.Position;

			if (selfToTarget.sqrMagnitude > SqrAttackRange) {
				self.Move(target.Position - self.Position);
				return;
			}

			self.Attack(target, AttackDamage, DelayBetweenAttacks);
		}
	}
}