using UnityEngine;

namespace LD55.Game {
	public abstract class AiCombatStrategy : ScriptableObject {
		public abstract void Solve(ICombatant self, ICombatTarget target);
	}
}