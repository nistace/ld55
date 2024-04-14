using UnityEngine;

namespace LD55.Game {
	public class Projectile : MonoBehaviour {
		[SerializeField] protected ProjectileType type;

		private Vector2 Origin { get; set; }
		private Vector2 Destination { get; set; }
		private float OriginToDestinationMagnitude { get; set; }
		private Vector3 DestinationToTargetOffset { get; set; }
		private float Progress { get; set; }
		private Team TargetTeam { get; set; }
		private SpriteRenderer spriteRenderer { get; set; }

		private void Start() {
			spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		}

		public void Shoot(Vector2 origin, Vector2 destination, Team targetTeam, float destinationOffsetY) {
			GameAudio.PlaySfx(type.ShotClip, origin);
			Origin = origin;
			Destination = destination;
			TargetTeam = targetTeam;
			DestinationToTargetOffset = new Vector3(0, -destinationOffsetY, 0);
			OriginToDestinationMagnitude = (Origin - Destination).magnitude;
			RefreshPosition();
			Progress = 0;
			if (!spriteRenderer) spriteRenderer = GetComponentInChildren<SpriteRenderer>();
			spriteRenderer.enabled = false;
			gameObject.SetActive(true);
		}

		private void Update() {
			if (Progress > 1) {
				spriteRenderer.enabled = false;
				gameObject.SetActive(false);
				return;
			}

			Progress += Time.deltaTime * type.Speed / OriginToDestinationMagnitude;
			spriteRenderer.enabled = true;
			RefreshPosition();
			if (GetYOffset(Progress) < .1f && CombatGlobalParameters.TryGetClosest(TargetTeam, transform.position - DestinationToTargetOffset, .5f, out var hitTarget)) {
				hitTarget.TakeDamage(type.Damage);
				gameObject.SetActive(false);
			}
		}

		private void RefreshPosition() {
			transform.position = GetPositionAtProgress(Progress);
			if (type.RightToForward && Progress < .99f) {
				transform.right = (Vector3)GetPositionAtProgress(Progress + .01f) - transform.position;
			}
		}

		private float GetYOffset(float progress) => type.HeightCurve.Evaluate(progress) * OriginToDestinationMagnitude;

		private Vector2 GetPositionAtProgress(float progress) {
			var yOffset = GetYOffset(progress);
			return Vector2.Lerp(Origin, Destination, progress) + new Vector2(0, yOffset);
		}
	}
}