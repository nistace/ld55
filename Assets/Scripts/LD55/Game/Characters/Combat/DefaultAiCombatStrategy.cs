namespace LD55.Game {
	public class DefaultAiCombatStrategy : AiCombatStrategy {
		public override void Solve(ICombatant self, ICombatTarget target) {
			if (self == null) return;
			if (target == null) return;

			var selfToTarget = target.Position - self.Position;

			if (selfToTarget.sqrMagnitude > self.CombatantData.SqrAttackRange) {
				self.Move(target.Position - self.Position);
				return;
			}

			self.Attack(target);
		}
	}
}