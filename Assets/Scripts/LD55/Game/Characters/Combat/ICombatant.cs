using UnityEngine;

namespace LD55.Game {
	public interface ICombatant : ICombatTarget {
		void Move(Vector2 targetPosition);
		void Attack(ICombatTarget target, int amount, float delayBetweenAttacks);
	}
}