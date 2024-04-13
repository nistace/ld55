using System.Collections.Generic;
using UnityEngine;

namespace LD55.Game {
	public class CharacterManager : MonoBehaviour {
		[SerializeField] protected PlayerController player;
		[SerializeField] protected List<MonsterController> monsters = new List<MonsterController>();

		public void Update() {
			foreach (var monster in monsters) {
				monster.Target = player.CharacterController;
			}
		}
	}
}