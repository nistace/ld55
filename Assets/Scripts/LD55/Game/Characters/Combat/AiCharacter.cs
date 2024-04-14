using NiUtils.Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace LD55.Game {
	public class AiCharacter : MonoBehaviour, ICombatant {
		[SerializeField] protected CharacterController characterController;
		[SerializeField] protected AiCombatStrategy combatStrategy;

		public CharacterController CharacterController => characterController;

		public Vector2 Position => characterController.Position;
		public bool IsDead => characterController.IsDead;
		private float NextAllowedAttackTime { get; set; }
		private ICombatTarget Target { get; set; }
		public UnityEvent OnDied => characterController.OnDied;

		private void Update() {
			if (CharacterController.IsDead) return;
			combatStrategy.Solve(this, Target);
		}

		public void Move(Vector2 targetPosition) => characterController.Move(targetPosition);

		public void SetDelayBeforeNextAttack(float delayBetweenAttacks) {
			NextAllowedAttackTime = Time.time + delayBetweenAttacks;
		}

		public bool IsNextAttackReady() => Time.time >= NextAllowedAttackTime;
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