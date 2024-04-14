using LD55.Inputs;
using NiUtils.Extensions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LD55.Game {
	public class MagicStoneManager : MonoBehaviour, IInteractableManager {
		private static MagicStoneManager CachedInstance { get; set; }
		public static MagicStoneManager Instance => CachedInstance ? CachedInstance : CachedInstance = FindObjectOfType<MagicStoneManager>(true);

		[SerializeField] protected MagicStone prefab;
		[SerializeField] protected float interactionRange = 1;
		[SerializeField] protected float[] spawnDistanceToPlayer = { 30f };
		[SerializeField] protected float interactionTime = 2;

		private MagicStone CurrentMagicStone { get; set; }
		public Vector2 CurrentMagicStonePosition => CurrentMagicStone.Position;
		public Vector2 CurrentInteractablePosition => CurrentMagicStone.Position;
		private bool NextMagicSpawnAllowed { get; set; }
		public float InteractionProgress { get; private set; }

		public void SpawnFirstMagicStone() => SpawnNextMagicStone();

		public bool CanPlayerMoveToStone() {
			if (!CurrentMagicStone) return false;
			if (!CurrentMagicStone.CanInteract) return false;
			if (CharacterManager.Instance.Player.CharacterController.IsDead) return false;
			return true;
		}

		public void SetNextMagicSpawnAllowed(bool allowed) {
			NextMagicSpawnAllowed = allowed;
			if (allowed && (!CurrentMagicStone || CurrentMagicStone.Consumed)) {
				SpawnNextMagicStone();
			}
		}

		private void SpawnNextMagicStone() {
			var spawnOffset = Vector2.right.Rotate(Random.Range(0, 360)) * spawnDistanceToPlayer[CharacterManager.Instance.Player.Summoner.Level.Clamp(spawnDistanceToPlayer)];
			CurrentMagicStone = Instantiate(prefab, CharacterManager.Instance.Player.Position + spawnOffset, Quaternion.identity, transform);
		}

		public bool CanPlayerInteract() {
			if (!CurrentMagicStone) return false;
			if (!CurrentMagicStone.CanInteract) return false;
			if (CharacterManager.Instance.Player.CharacterController.IsDead) return false;
			if (CharacterManager.Instance.Player.Summoner.IsSummoning) return false;
			return (CharacterManager.Instance.Player.Position - CurrentMagicStone.Position).sqrMagnitude < interactionRange * interactionRange;
		}

		private void Update() {
			if (!CanPlayerInteract()) return;

			InteractionProgress = Mathf.MoveTowards(InteractionProgress, InputManager.Controls.Player.Interact.inProgress ? 1 : 0, Time.deltaTime / interactionTime);
			if (Mathf.Approximately(InteractionProgress, 1)) {
				CharacterManager.Instance.Player.LevelUp();
				CurrentMagicStone.Consume();
				if (NextMagicSpawnAllowed && !CharacterManager.Instance.Player.Summoner.IsMaxLevel) {
					SpawnNextMagicStone();
				}
				InteractionProgress = 0;
			}
		}
	}
}