using System.Collections;
using NiUtils.Extensions;
using NiUtils.Types;
using UnityEngine;
using Random = UnityEngine.Random;

public class MudSpawner : MonoBehaviour {
	[SerializeField] protected SpriteRenderer mudPrefab;
	[SerializeField] protected FloatRange spawnFrequency = new FloatRange(3, 8);
	[SerializeField] protected FloatRange range = new FloatRange(.3f, .8f);
	[SerializeField] protected AnimationCurve heightCurve;

	private float NextSpawnTime { get; set; }

	private void Start() {
		NextSpawnTime = Time.time + spawnFrequency.Random();
	}

	private void Update() {
		if (Time.time > NextSpawnTime) {
			StartCoroutine(SpawnMud((Vector2)transform.position + Vector2.right.Rotate(Random.Range(0, 360)) * range.Random()));
			NextSpawnTime = Time.time + spawnFrequency.Random();
		}
	}

	private IEnumerator SpawnMud(Vector2 destination) {
		var mudInstance = Instantiate(mudPrefab, transform.position, Quaternion.identity, transform);
		var distance = (destination - (Vector2)transform.position).magnitude;
		mudInstance.sortingOrder = -Mathf.FloorToInt(100 * destination.y);
		for (var t = 0f; t < 1; t += Time.deltaTime / distance) {
			mudInstance.transform.position = Vector2.Lerp(transform.position, destination, t) + new Vector2(0, heightCurve.Evaluate(t));
			yield return null;
		}
		mudInstance.transform.position = destination;
	}
}