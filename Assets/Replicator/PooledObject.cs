using UnityEngine;

namespace Replicator {
	[AddComponentMenu("")] // Prevents this Component from appearing in the Unity editor.
	internal class PooledObject : MonoBehaviour, IPooled {
		private ObjectPool owner;
		private bool recycleFlag;

		private void Awake() {
			hideFlags = HideFlags.HideInInspector;
		}

		private void LateUpdate() {
			if(recycleFlag) {
				recycleFlag = false;
				owner.Recycle(this);
			}
		}

		public void SetOwner(ObjectPool newOwner) {
			if(owner == null) {
				owner = newOwner;
				GameObjectExtensions.poolRegistry.Add(gameObject, owner);
				owner.OnDisablePool += deregisterInstance;
			}
			else Debug.Log(Strings.SetOwnerOnOwned);
		}

		private void deregisterInstance() {
			GameObjectExtensions.poolRegistry.Remove(gameObject);
			owner.OnDisablePool -= deregisterInstance;
		}

		public bool BelongsTo(ObjectPool pool) => owner == pool;

		public void Recycle() => recycleFlag = true;

		public GameObject Spawn(GameObject original) => owner.Spawn(transform.position, Quaternion.identity);

		public void OnSpawn() => gameObject.hideFlags |= HideFlags.HideInHierarchy;

		public void OnRecycle() => gameObject.hideFlags &= ~HideFlags.HideInHierarchy;
	}
}
