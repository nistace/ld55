using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace LD55.Game {
	public class GameEnvironmentManager : MonoBehaviour {
		private static GameEnvironmentManager CachedInstance { get; set; }
		public static GameEnvironmentManager Instance => CachedInstance ? CachedInstance : CachedInstance = FindObjectOfType<GameEnvironmentManager>(true);

		[SerializeField] protected Camera backgroundCamera;
		[SerializeField] protected GameEnvironmentType defaultEnvironmentType;

		private GameEnvironmentType CurrentEnvironmentType { get; set; }
		public bool SpawningAllowed => CurrentEnvironmentType && CurrentEnvironmentType.SpawningAllowed;

		public UnityEvent OnChangingEnvironment { get; } = new UnityEvent();

		private void Start() => SetEnvironmentType(defaultEnvironmentType);

		private void SetEnvironmentType(GameEnvironmentType type) {
			CurrentEnvironmentType = type;
			LerpEnvironment(type, type, 1);
		}

		public void ChangeEnvironmentType(GameEnvironmentType environmentType) => StartCoroutine(DoChangeEnvironmentType(environmentType));

		private IEnumerator DoChangeEnvironmentType(GameEnvironmentType environmentType) {
			if (CurrentEnvironmentType == environmentType) yield break;

			var previous = CurrentEnvironmentType;
			CurrentEnvironmentType = environmentType;
			OnChangingEnvironment.Invoke();

			for (var t = 0f; t < environmentType.TransitionDuration; t += Time.deltaTime) {
				LerpEnvironment(previous, environmentType, t / environmentType.TransitionDuration);
				yield return null;
			}
		}

		private void LerpEnvironment(GameEnvironmentType previous, GameEnvironmentType next, float lerp) {
			backgroundCamera.backgroundColor = Color.Lerp(previous.Background, next.Background, lerp);
		}
	}
}