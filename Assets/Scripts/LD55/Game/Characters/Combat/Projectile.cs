using UnityEngine;

namespace LD55.Game
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] protected ProjectileType type;

        public Vector2 Origin { get; set; }
        public Vector2 Destination { get; set; }
        private float OriginToDestinationMagnitude { get; set; }
        private float Progress { get; set; }

        public void Shoot(Vector2 origin, Vector2 destination)
        {
            Origin = origin;
            Destination = destination;
            OriginToDestinationMagnitude = (Origin - Destination).magnitude;
            RefreshPosition();
            Progress = 0;
            gameObject.SetActive(true);
        }

        private void Update()
        {
            if (Progress > 1)
            {
                gameObject.SetActive(false);
                return;
            }

            Progress += Time.deltaTime * type.Speed / OriginToDestinationMagnitude;
            RefreshPosition();
            // TODO Deal Damage
        }

        private void RefreshPosition()
        {
            transform.position = GetPositionAtProgress(Progress);
            if (type.RightToForward && Progress < .99f)
            {
                transform.right = (Vector3)GetPositionAtProgress(Progress + .01f) - transform.position;
            }
        }

        private Vector2 GetPositionAtProgress(float progress)
        {
            var yOffset = type.HeightCurve.Evaluate(progress) * OriginToDestinationMagnitude;
            return Vector2.Lerp(Origin, Destination, progress) + new Vector2(0, yOffset);
        }
    }
}