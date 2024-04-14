using UnityEngine;

namespace LD55.Game {
	public interface IInteractableManager {
		Vector2 CurrentInteractablePosition { get; }
		float InteractionProgress { get; }
	}
}