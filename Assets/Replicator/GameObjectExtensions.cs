using UnityEngine;

namespace Replicator {
	public static partial class GameObjectExtensions {
		/// <summary>
		/// Recycle this GameObject, if it belongs to a pool.
		/// </summary>
		public static void Recycle(this GameObject self) => self.GetComponent<PooledObject>()?.Recycle();

		/// <summary>
		/// Spawn a copy of this GameObject, if it belongs to a pool. Optionally supply a <paramref name="parent"/> for the spawned GameObject.
		/// </summary>
		public static GameObject Spawn(this GameObject self, Transform parent = null) {
			return spawn(self, parent, new Vector3(), new Quaternion());
		}

		/// <summary>
		/// Spawn a copy of this GameObject, if it belongs to a pool. Optionally provide a <paramref name="parent" to assign to the spawned GameObject.
		/// </summary>
		/// <param name="position">The position to spawn the GameObject.</param>
		/// <param name="rotation">The rotation to apply to the spawned GameObject.</param>
		public static GameObject Spawn(this GameObject self, Vector3 position, Quaternion rotation, Transform parent = null) {
			return spawn(self, parent, position, rotation);
		}

		private static GameObject spawn(GameObject self, Transform parent, Vector3 position, Quaternion rotation) {
			return PoolRegistry.Pools[self]?.Spawn(position, rotation, parent);
		}
	}
}
