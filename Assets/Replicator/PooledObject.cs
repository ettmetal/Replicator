using UnityEngine;

namespace Replicator {
	[AddComponentMenu("")] // Prevents this Component from appearing in the Unity editor.
	internal class PooledObject : MonoBehaviour
	#if UNITY_EDITOR
	, IPooled
	#endif
	{
		private ObjectPool owner;
		private bool recycleFlag;

#region HideInHierarchy
#if UNITY_EDITOR
		private void Awake() {
			hideFlags = HideFlags.HideInInspector;
			if(owner.hideUnspawned) gameObject.hideFlags |= HideFlags.HideInHierarchy;
			UnityEditor.EditorApplication.DirtyHierarchyWindowSorting();
		}

		public void OnSpawn() {
			if(owner.hideUnspawned) gameObject.hideFlags &= ~HideFlags.HideInHierarchy;
			UnityEditor.EditorApplication.DirtyHierarchyWindowSorting();
		}

		public void OnRecycle() {
			if(owner.hideUnspawned) gameObject.hideFlags |= HideFlags.HideInHierarchy;
			UnityEditor.EditorApplication.DirtyHierarchyWindowSorting();
		}
#endif
#endregion

		private void LateUpdate() {
			if(recycleFlag) {
				recycleFlag = false;
				owner.Recycle(this);
			}
		}

		public void SetOwner(ObjectPool newOwner) {
			if(owner == null) {
				owner = newOwner;
				PoolRegistry.pools.Add(gameObject, owner);
				owner.OnDisablePool += deregisterInstance;
			}
			else Debug.Log(Strings.SetOwnerOnOwned);
		}

		private void deregisterInstance() {
			PoolRegistry.pools.Remove(gameObject);
			owner.OnDisablePool -= deregisterInstance;
		}

		public bool BelongsTo(ObjectPool pool) => owner == pool;

		public void Recycle() => recycleFlag = true;

		public GameObject Spawn(GameObject original) => owner.Spawn(transform.position, Quaternion.identity);
	}
}
