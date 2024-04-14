using UnityEngine;

namespace LD55.Game {
	public interface ICombatant {
		public ICombatTarget Target { get; }
		Vector2 Position { get; }
		Team Team { get; }

		void Move(Vector2 targetPosition);
		void SetDelayBeforeNextAttack(float delayBeforeNextAttack);
		bool IsNextAttackReady();
	}
}