using UnityEngine;

namespace LD55.Game {
	[CreateAssetMenu(menuName = "Combat/" + nameof(TurretAiCombatStrategy))]
	public class TurretAiCombatStrategy : AiCombatStrategy {
		[SerializeField] protected float attackRange = 5f;
		[SerializeField] protected Projectile projectilePrefab;
		[SerializeField] protected float attackSpeed = 1;
		[SerializeField] protected Vector2 originOffset = new Vector2(0, .1f);
		[SerializeField] protected Vector2 destinationOffset = new Vector2(0, .1f);

		private float SqrAttackRange => attackRange * attackRange;
		private float DelayBetweenAttacks => 1 / attackSpeed;

		public override void Solve(ICombatant self) {
			if (self?.Target == null) return;
			if (self.Target.IsDead) return;

			var selfToTarget = self.Target.Position - self.Position;

			if (selfToTarget.sqrMagnitude < SqrAttackRange && self.IsNextAttackReady()) {
				ProjectileManager.Shoot(projectilePrefab, self.Position + originOffset, self.Target.Position + destinationOffset, self.Team.Opponent(), destinationOffset.y);
				self.SetDelayBeforeNextAttack(DelayBetweenAttacks);
			}
		}

		public override ICombatTarget FindTarget(ICombatant self) => GetClosestOpponent(self);
	}
}