using UnityEngine;

namespace LD55.Game {
	public interface ICombatant : ICombatTarget {
		ICombatTarget Target { get; }
		void ChangeTarget(ICombatTarget target);
		CombatantData CombatantData { get; }
		void Move(Vector2 targetPosition);
		void Attack(ICombatTarget target);
	}
}