using UnityEngine;
using UnityEngine.Events;

namespace LD55.Game {
	public interface ICombatTarget {
		Vector2 Position { get; }
		bool IsDead { get; }
		public Team Team { get; }

		UnityEvent OnDied { get; }

		void TakeDamage(int damage);
	}
}