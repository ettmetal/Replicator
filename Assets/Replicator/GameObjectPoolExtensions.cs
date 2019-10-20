using System.Collections.Generic;
using UnityEngine;

namespace Replicator {
	public static partial class GameObjectExtensions {
		// Provide a registry of 'live' pools internally to facilitate mimicking the Instantiate / Destroy API.
		private static Dictionary<GameObject, ObjectPool> poolByGameObject;
		internal static Dictionary<GameObject, ObjectPool> poolRegistry { get { return poolByGameObject; } }
		static GameObjectExtensions() {
			poolByGameObject = new Dictionary<GameObject, ObjectPool>();
		}

		/// <summary>
		/// Recycle this GameObject, if it belongs to a pool.
		/// </summary>
		public static void Recycle(this GameObject self) {
			var pooledObject = self.GetComponent<PooledObject>();
			if(pooledObject != null) {
				pooledObject.Recycle();
			}
		}


		/// <summary>
		/// Spawn a copy of this GameObject, if it belongs to a pool. Optionally supply a <paramref name="parent"/> for the spawned GameObject.
		/// </summary>
		public static GameObject Spawn(this GameObject self, Transform parent = null) {
			return spawn(self, parent: parent);
		}

		/// <summary>
		/// Spawn a copy of this GameObject, if it belongs to a pool. Optionally provide a <paramref name="parent" to assign to the spawned GameObject.
		/// </summary>
		/// <param name="position">The position to spawn the GameObject.</param>
		/// <param name="rotation">The rotation to apply to the spawned GameObject.</param>
		public static GameObject Spawn(this GameObject self, Vector3 position, Quaternion rotation, Transform parent = null) {
			return spawn(self, position: position, rotation: rotation, parent: parent);
		}

		private static GameObject spawn(GameObject self, Transform parent, Vector3 position = new Vector3(), Quaternion rotation = new Quaternion()) {
			return poolByGameObject[self]?.Spawn(position, rotation, parent);
		}
	}
}
