using NiUtils.Extensions;
using UnityEngine;
using UnityEngine.Rendering;

namespace LD55.Game {
	[RequireComponent(typeof(CharacterController))]
	public class CharacterAnimator : MonoBehaviour {
		[SerializeField] protected CharacterController characterController;
		[SerializeField] protected SortingGroup sortingGroup;
		[SerializeField] protected SpriteRenderer sortingGroupRenderer;
		[SerializeField] protected Animator animator;

		private static readonly int movementXAnimParam = Animator.StringToHash("MovementX");
		private static readonly int movementYAnimParam = Animator.StringToHash("MovementY");
		private static readonly int deadAnimParam = Animator.StringToHash("Dead");
		private static readonly int tookDamageAnimParam = Animator.StringToHash("TookDamage");

		private SpriteRenderer ActiveRenderer { get; set; }
		private float TimeOfLastDamageTaken { get; set; } = -2;
		public Animator Animator => animator;

		private void Reset() {
			characterController = GetComponent<CharacterController>();
			sortingGroup = GetComponentInChildren<SortingGroup>();
			sortingGroupRenderer = GetComponentInChildren<SpriteRenderer>();
		}

		private void Start() {
			foreach (var child in animator.transform.Children()) {
				child.gameObject.SetActive(child.name == characterController.Type.name);
				if (child.name == characterController.Type.name) {
					ActiveRenderer = child.GetComponent<SpriteRenderer>();
				}
			}
			characterController.OnTookDamage.AddListenerOnce(HandleTookDamage);
		}

		private void HandleTookDamage() {
			animator.SetTrigger(tookDamageAnimParam);
			TimeOfLastDamageTaken = Time.time;
		}

		private void LateUpdate() {
			if (sortingGroup) sortingGroup.sortingOrder = -Mathf.FloorToInt(100 * transform.position.y);
			if (sortingGroupRenderer) sortingGroupRenderer.sortingOrder = -Mathf.FloorToInt(100 * transform.position.y);
			animator.SetFloat(movementXAnimParam, characterController.LastMovementNormalized.x);
			animator.SetFloat(movementYAnimParam, characterController.LastMovementNormalized.y);
			animator.SetBool(deadAnimParam, characterController.IsDead);
			if (ActiveRenderer && Time.time < TimeOfLastDamageTaken + 1) {
				ActiveRenderer.color = Color.Lerp(Color.red, Color.white, (Time.time - TimeOfLastDamageTaken) / .5f);
			}
		}
	}
}