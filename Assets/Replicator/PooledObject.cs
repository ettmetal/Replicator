using UnityEngine;

namespace Replicator {
	[AddComponentMenu("")] // Prevents this Component from appearing in the Unity editor.
	internal class PooledObject : MonoBehaviour, IPooled {
		private ObjectPool owner;
		private bool recycleFlag;

		private void LateUpdate() {
			if(recycleFlag) {
				recycleFlag = false;
				owner.Recycle(this);
			}
		}

		public void SetOwner(ObjectPool newOwner) {
			if(owner == null) {
				owner = newOwner;
			}
			else Debug.Log(Strings.SetOwnerOnOwned);
		}

		public bool BelongsTo(ObjectPool pool) {
			return owner == pool;
		}

		public void Recycle() {
			recycleFlag = true;
		}

		public GameObject Spawn(GameObject original) {
			return owner.Spawn(transform.position, Quaternion.identity);
		}

		public void OnSpawn() {
			gameObject.hideFlags = HideFlags.None;
			hideFlags = HideFlags.HideInInspector;
		}

		public void OnRecycle() {
			gameObject.hideFlags = HideFlags.HideInHierarchy;
		}
	}
}
