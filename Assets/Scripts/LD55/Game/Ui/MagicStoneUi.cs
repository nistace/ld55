using NiUtils.Extensions;
using NiUtils.StaticUtils;
using UnityEngine;

namespace LD55.Game.Ui {
	public class MagicStoneUi : MonoBehaviour {
		[SerializeField] protected Transform nextStoneMarker;

		private void Update() {
			UpdateNextStoneMarker();
		}

		private void UpdateNextStoneMarker() {
			if (!MagicStoneManager.Instance.CanPlayerMoveToStone()) {
				nextStoneMarker.gameObject.SetActive(false);
				return;
			}
			var stoneViewportPoint = CameraUtils.main.WorldToViewportPoint(MagicStoneManager.Instance.CurrentMagicStonePosition);
			var nextIsVisibleInScreen = stoneViewportPoint.x.Between(0, 1) && stoneViewportPoint.y.Between(0, 1);
			nextStoneMarker.gameObject.SetActive(!nextIsVisibleInScreen);
			if (!nextIsVisibleInScreen) {
				var centerOfScreen = new Vector2(Screen.width * .5f, Screen.height * .5f);
				var stoneScreenPoint = (Vector2)CameraUtils.main.WorldToScreenPoint(MagicStoneManager.Instance.CurrentMagicStonePosition);
				var angle = Vector2.SignedAngle(Vector2.right, stoneScreenPoint - centerOfScreen);
				nextStoneMarker.rotation = Quaternion.Euler(0, 0, angle);
			}
		}
	}
}