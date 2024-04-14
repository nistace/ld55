using NiUtils.Audio;
using UnityEngine;

namespace LD55.Game {
	public static class GameAudio {
		public static void PlaySfx(AudioClip clip, Vector2 position) {
			if ((position - CharacterManager.Instance.Player.Position).sqrMagnitude > 40 * 40) return;
			var source = AudioManager.Sfx.Play(clip);
			source.spatialBlend = 1;
			source.minDistance = 15;
			source.maxDistance = 40;
			source.transform.position = position;
		}
	}
}