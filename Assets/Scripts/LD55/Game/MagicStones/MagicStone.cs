using System.Linq;
using UnityEngine;

namespace LD55.Game {
	public class MagicStone : MonoBehaviour {
		[SerializeField] protected SpriteRenderer spriteRenderer;
		[SerializeField] protected Animator animator;
		[SerializeField] protected AnimationClip activeClip;

		private static readonly int consumedAnimParam = Animator.StringToHash("Consumed");

		public bool CanInteract => animator.GetCurrentAnimatorClipInfo(0).Any(t => t.clip == activeClip);
		public Vector2 Position => transform.position;

		private void Start() {
			spriteRenderer.sortingOrder = -Mathf.FloorToInt(100 * transform.position.y);
		}

		public void Consume() {
			animator.SetBool(consumedAnimParam, true);
		}
	}
}