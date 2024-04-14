using NiUtils.Extensions;
using NiUtils.StaticUtils;
using UnityEngine;
using UnityEngine.UI;

namespace LD55.Game.Ui {
	public class InteractionUi : MonoBehaviour {
		[SerializeField] protected Transform interactIcon;
		[SerializeField] protected Image interactBack;
		[SerializeField] protected Image interactFront;

		private void Start() {
			interactIcon.gameObject.SetActive(false);
		}

		private void Update() {
			UpdateInteractButton();
		}

		private void UpdateInteractButton() {
			if (TryGetActiveInteractionManager(out var interactableManager)) {
				interactIcon.gameObject.SetActive(true);
				interactIcon.position = CameraUtils.main.WorldToScreenPoint(interactableManager.CurrentInteractablePosition);
				interactBack.color = interactBack.color.With(a: Mathf.Approximately(interactableManager.InteractionProgress, 0) ? .8f : .5f);
				interactFront.fillAmount = interactableManager.InteractionProgress;
			}
			else {
				interactIcon.gameObject.SetActive(false);
			}
		}

		private static bool TryGetActiveInteractionManager(out IInteractableManager manager) {
			manager = default;
			if (MagicStoneManager.Instance.CanPlayerInteract()) manager = MagicStoneManager.Instance;
			else if (PortalManager.Instance.CanPlayerInteract()) manager = PortalManager.Instance;
			return manager != null;
		}
	}
}