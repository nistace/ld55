using System;
using System.Collections;
using NiUtils.Extensions;
using UnityEngine;

public class ScenarioVillageIdiot : MonoBehaviour {
	[SerializeField] protected Animator animator;
	[SerializeField] protected SpriteRenderer spriteRenderer;
	[SerializeField] protected Vector2 outsidePosition = new Vector2(-5, 0);
	[SerializeField] protected Vector2 insidePosition = new Vector2(-3, 0);
	[SerializeField] protected AnimationCurve animationCurve;
	[SerializeField] protected float inOutTime = 2;

	private static readonly int movementXAnimParam = Animator.StringToHash("MovementX");
	private static readonly int pointingAnimParam = Animator.StringToHash("Pointing");

	private void Start() {
		spriteRenderer.color = Color.clear;
	}

	public IEnumerator PlayEntering(Action callback) {
		animator.SetFloat(movementXAnimParam, 1);
		animator.SetBool(pointingAnimParam, false);
		for (var t = 0f; t < 1; t += Time.deltaTime / inOutTime) {
			spriteRenderer.color = Color.white.With(a: animationCurve.Evaluate(t));
			transform.position = Vector2.Lerp(outsidePosition, insidePosition, t);
			yield return null;
		}
		spriteRenderer.color = Color.white;
		transform.position = insidePosition;
		animator.SetBool(pointingAnimParam, true);

		callback?.Invoke();
	}

	public IEnumerator PlayExiting(Action callback) {
		animator.SetFloat(movementXAnimParam, -1);
		animator.SetBool(pointingAnimParam, false);
		for (var t = 0f; t < 1; t += Time.deltaTime / inOutTime) {
			spriteRenderer.color = Color.white.With(a: animationCurve.Evaluate(1 - t));
			transform.position = Vector2.Lerp(outsidePosition, insidePosition, 1 - t);
			yield return null;
		}
		spriteRenderer.color = Color.clear;
		transform.position = outsidePosition;

		callback?.Invoke();
	}
}