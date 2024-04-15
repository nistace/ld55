using LD55.Inputs;
using NiUtils.Extensions;
using NiUtils.StaticUtils;
using UnityEngine;

namespace LD55.Game.Ui {
	public class InteractionUi : MonoBehaviour {
		[SerializeField] protected Transform interactIcon;
		[SerializeField] protected ControlUi interactBack;
		[SerializeField] protected ControlUi interactFront;

		private void Start() {
			interactIcon.gameObject.SetActive(false);
			interactBack.Image.sprite = InputManager.ControllerSprites['H'];
			interactBack.KeyText.enabled = false;
			interactFront.Image.sprite = InputManager.ControllerSprites['H'];
			interactFront.KeyText.enabled = InputManager.ControllerSprites.DisplayKey;
			if (interactFront.KeyText.enabled) interactFront.KeyText.text = InputManager.GetKeyText('C');
		}

		private void Update() {
			UpdateInteractButton();
		}

		private void UpdateInteractButton() {
			if (TryGetActiveInteractionManager(out var interactableManager)) {
				interactIcon.gameObject.SetActive(true);
				interactIcon.position = CameraUtils.main.WorldToScreenPoint(interactableManager.CurrentInteractablePosition);
				interactBack.MarkAsNotMissed(Color.white.With(a: Mathf.Approximately(interactableManager.InteractionProgress, 0) ? .8f : .5f));
				interactFront.Image.fillAmount = interactableManager.InteractionProgress;
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