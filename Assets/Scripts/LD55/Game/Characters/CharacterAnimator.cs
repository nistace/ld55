﻿using NiUtils.Extensions;
using UnityEngine;
using UnityEngine.Rendering;

namespace LD55.Game {
	[RequireComponent(typeof(CharacterController))]
	public class CharacterAnimator : MonoBehaviour {
		[SerializeField] protected CharacterController characterController;
		[SerializeField] protected SortingGroup sortingGroup;
		[SerializeField] protected Animator animator;

		private static readonly int movementXAnimParam = Animator.StringToHash("MovementX");
		private static readonly int movementYAnimParam = Animator.StringToHash("MovementY");

		public Animator Animator => animator;

		private void Reset() {
			characterController = GetComponent<CharacterController>();
			sortingGroup = GetComponentInChildren<SortingGroup>();
		}

		private void Start() {
			foreach (var child in animator.transform.Children()) {
				child.gameObject.SetActive(child.name == characterController.Type.name);
			}
		}

		private void Update() {
			sortingGroup.sortingOrder = -Mathf.FloorToInt(100 * transform.position.y);
			animator.SetFloat(movementXAnimParam, characterController.LastMovementNormalized.x);
			animator.SetFloat(movementYAnimParam, characterController.LastMovementNormalized.y);
		}
	}
}