using UnityEngine;

namespace LD55.Game {
	[RequireComponent(typeof(CharacterAnimator))]
	public class PlayerAnimator : MonoBehaviour {
		[SerializeField] protected PlayerController playerController;
		[SerializeField] protected CharacterAnimator characterAnimator;

		private static readonly int summoningAnimParam = Animator.StringToHash("Summoning");

		private void Reset() {
			characterAnimator = GetComponent<CharacterAnimator>();
		}

		private void Update() {
			characterAnimator.Animator.SetBool(summoningAnimParam, playerController.Summoner.IsSummoning);
		}
	}
}