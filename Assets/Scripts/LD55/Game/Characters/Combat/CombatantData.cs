using System;
using UnityEngine;

namespace LD55.Game {
	[Serializable]
	public class CombatantData {
		[SerializeField] protected float attackRange = .5f;
		[SerializeField] protected int attackDamage = 1;
		[SerializeField] protected float attackSpeed = 1;

		public float SqrAttackRange => attackRange * attackRange;
		public int AttackDamage => attackDamage;
		public float DelayBetweenAttacks => 1 / attackSpeed;
	}
}