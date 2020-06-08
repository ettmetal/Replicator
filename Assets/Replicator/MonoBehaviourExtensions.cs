using System.Collections;
using UnityEngine;

namespace Replicator {
	public static partial class MonoBehaviourExtensions {
		/// <summary>Spawn a copy of <paramref name="original"/> if it is a pooled GameObject.</summary>
		/// <param name="parent">(optional) Parent for the spawned GameObject</param>
		public static GameObject Spawn(this MonoBehaviour self, GameObject original, Transform parent = null) {
			return original.Spawn(parent);
		}

		/// <summary>Spawn a copy of <paramref name="original"/> if it is a pooled GameObject.</summary>
		/// <param name="position">The position of the spawned GameObject</param>
		/// <param name="rotation">The rotation of the spawned GameObject</param>
		/// <param name="parent">(optional) Parent for the spawned GameObject</param>
		public static GameObject Spawn(this MonoBehaviour self, GameObject original, Vector3 position, Quaternion rotation, Transform parent = null) {
			return original.Spawn(position, rotation, parent);
		}

		/// <summary>Recycle <paramref name="target"/>, if it was spawned from a pool.</summary>
		public static void Recycle(this MonoBehaviour self, GameObject target) {
			target.Recycle();
		}

		/// <summary>
		/// Recycle <paramref name="target"/>, if it was spawned from a pool with <paramref name="delay"/> seconds delay before recycling.
		/// </summary>
		public static void Recycle(this MonoBehaviour self, GameObject target, float delay) {
			self.StartCoroutine(delayedRecycle(target, delay));
		}

		private static IEnumerator delayedRecycle(GameObject target, float delay) {
			yield return new WaitForSeconds(delay);
			target.Recycle();
		}
	}
}
