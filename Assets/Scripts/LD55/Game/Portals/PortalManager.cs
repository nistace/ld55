using System.Collections.Generic;
using System.Linq;
using LD55.Game.Portals;
using LD55.Inputs;
using NiUtils.Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace LD55.Game {
	public class PortalManager : MonoBehaviour, IInteractableManager {
		private static PortalManager CachedInstance { get; set; }
		public static PortalManager Instance => CachedInstance ? CachedInstance : CachedInstance = FindObjectOfType<PortalManager>(true);
		public static UnityEvent OnPortalActivated { get; } = new UnityEvent();

		[SerializeField] protected float interactionRange = 1;
		[SerializeField] protected float interactionTime = 1;
		[SerializeField] protected GameEnvironmentType environmentTypeBehindPortal;

		private HashSet<Portal> Portals { get; } = new HashSet<Portal>();
		private bool InteractionWithPortalsEnabled { get; set; } = true;
		private Portal ClosestPortal { get; set; }
		public Vector2 CurrentInteractablePosition => ClosestPortal.Position;
		public float InteractionProgress { get; private set; }

		private void Start() {
			Portal.OnNewSpawned.AddListenerOnce(HandleNewPortal);
			Portal.OnAnyDied.AddListenerOnce(HandlePortalDied);
		}

		private void HandlePortalDied(Portal deadPortal) => Portals.Remove(deadPortal);
		private void HandleNewPortal(Portal newPortal) => Portals.Add(newPortal);

		public bool CanPlayerInteract() {
			if (!InteractionWithPortalsEnabled) return false;
			if (!ClosestPortal) return false;
			if (ClosestPortal.IsDead) return false;
			if (CharacterManager.Instance.Player.CharacterController.IsDead) return false;
			if (CharacterManager.Instance.Player.Summoner.IsSummoning) return false;
			return (CharacterManager.Instance.Player.Position - ClosestPortal.Position).sqrMagnitude < interactionRange * interactionRange;
		}

		private void Update() {
			if (InteractionWithPortalsEnabled) {
				RefreshClosestPortal();
			}

			if (!CanPlayerInteract()) return;

			InteractionProgress = Mathf.MoveTowards(InteractionProgress, InputManager.Controls.Player.Interact.inProgress ? 1 : 0, Time.deltaTime / interactionTime);
			if (Mathf.Approximately(InteractionProgress, 1)) {
				GameEnvironmentManager.Instance.ChangeEnvironmentType(environmentTypeBehindPortal);
				CharacterManager.Instance.KillAllEnemies();
				InteractionWithPortalsEnabled = false;
				InteractionProgress = 0;
				OnPortalActivated.Invoke();
			}
		}

		private void RefreshClosestPortal() {
			ClosestPortal = Portals.Count switch {
				0 => default,
				1 => Portals.First(),
				_ => Portals.Select(t => (t, (CharacterManager.Instance.Player.Position - t.Position).sqrMagnitude)).OrderBy(t => t.sqrMagnitude).FirstOrDefault().t
			};
		}
	}
}