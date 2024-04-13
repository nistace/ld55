using System;
using UnityEngine;
using CharacterController = LD55.Game.CharacterController;

public class MonsterController : MonoBehaviour {
	[SerializeField] protected CharacterController characterController;

	public CharacterController Target { get; set; }

	private void Update() {
		if (!Target) return;
		characterController.Move(Target.Position - characterController.Position);
	}
}