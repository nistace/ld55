using NiUtils.Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace LD55.Game {
	public class AiCharacter : MonoBehaviour, ICombatant {
		[SerializeField] protected CharacterController characterController;
		[SerializeField] protected AiCombatStrategy combatStrategy;

		public CharacterController CharacterController => characterController;

		public CombatantData CombatantData => characterController.CombatantData;
		public Vector2 Position => characterController.Position;
		private float NextAllowedAttackTime { get; set; }
		public ICombatTarget Target { get; private set; }
		public UnityEvent OnDied => characterController.OnDied;

		private void Update() {
			if (CharacterController.IsDead) return;
			combatStrategy.Solve(this, Target);
		}

		public void Move(Vector2 targetPosition) => characterController.Move(targetPosition);

		public void Attack(ICombatTarget target) {
			if (NextAllowedAttackTime > Time.time) return;
			target.TakeDamage(CombatantData.AttackDamage);
			NextAllowedAttackTime = Time.time + CombatantData.DelayBetweenAttacks;
		}

		public void TakeDamage(int damage) => characterController.TakeDamage(damage);

		public void ChangeTarget(ICombatTarget target) {
			Target?.OnDied.RemoveListener(HandleTargetDied);
			Target = target;
			Target?.OnDied.AddListenerOnce(HandleTargetDied);
		}

		private void HandleTargetDied() {
			Target?.OnDied.RemoveListener(HandleTargetDied);
			Target = default;
		}
	}
}