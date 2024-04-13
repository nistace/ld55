using UnityEngine;
using UnityEngine.Events;

namespace LD55.Game {
	public interface ICombatTarget {
		Vector2 Position { get; }
		void TakeDamage(int damage);
		UnityEvent OnDied { get; }
	}
}