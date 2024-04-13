using UnityEngine;

namespace LD55.Game
{
    [CreateAssetMenu(menuName = "Combat/" + nameof(RangedAiCombatStrategy))]
    public class RangedAiCombatStrategy : AiCombatStrategy
    {
        [SerializeField] protected float attackRange = 5f;
        [SerializeField] protected Projectile projectilePrefab;
        [SerializeField] protected float attackSpeed = 1;
        [SerializeField] protected Vector2 originYOffset = new Vector2(0, .1f);
        [SerializeField] protected Vector2 destinationYOffset = new Vector2(0, .1f);

        private float SqrAttackRange => attackRange * attackRange;
        private float DelayBetweenAttacks => 1 / attackSpeed;

        public override void Solve(ICombatant self, ICombatTarget target)
        {
            if (self == null) return;
            if (target == null) return;

            var selfToTarget = target.Position - self.Position;

            if (selfToTarget.sqrMagnitude > SqrAttackRange)
            {
                self.Move(target.Position - self.Position);
                return;
            }

            if (self.IsNextAttackReady())
            {
                ProjectileManager.Shoot(projectilePrefab, self.Position + originYOffset, target.Position + destinationYOffset);
                self.SetDelayBeforeNextAttack(DelayBetweenAttacks);
            }
        }
    }
}