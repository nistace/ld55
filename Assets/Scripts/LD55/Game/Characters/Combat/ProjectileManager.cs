using System.Collections.Generic;
using NiUtils.Extensions;
using UnityEngine;

namespace LD55.Game {
	public static class ProjectileManager {
		private static Transform CachedParent { get; set; }
		private static Transform Parent => CachedParent ? CachedParent : CachedParent = new GameObject("ProjectileParent").transform;

		private static Dictionary<Projectile, List<Projectile>> ProjectilesPerPrefab { get; } = new Dictionary<Projectile, List<Projectile>>();

		public static void Clear() {
			Parent.ClearChildren();
			ProjectilesPerPrefab.Clear();
		}

		public static void Shoot(Projectile prefab, Vector2 origin, Vector2 destination, Team targetTeam, float destinationOffsetY) {
			if (!ProjectilesPerPrefab.ContainsKey(prefab)) ProjectilesPerPrefab.Add(prefab, new List<Projectile>());
			var prefabInstances = ProjectilesPerPrefab[prefab];
			var instance = prefabInstances.Count > 0 && !prefabInstances[0].gameObject.activeSelf ? prefabInstances[0] : Object.Instantiate(prefab, Parent);
			prefabInstances.Remove(instance);
			prefabInstances.Add(instance);
			instance.Shoot(origin, destination, targetTeam, destinationOffsetY);
		}
	}
}